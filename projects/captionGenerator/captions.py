"""
MIT License
Copyright (c) 2026 Ville Laaksoaho

Input:  any audio or video file
Output: SRT file saved in output/ folder next to this script

Requirements:
    pip install openai-whisper torch
    ffmpeg installed (winget install ffmpeg)

Usage:
    python captions.py <file> [words_per_caption]
    (leave words_per_caption empty for punctuation-based splitting)

Examples:
    python captions.py my_export.wav        # punctuation-based splitting (default)
    python captions.py my_export.wav 4      # fixed 4 words per caption
    python captions.py my_export.wav 0      # no splitting, one caption for all
"""

import whisper
import os
import sys
import subprocess
import tempfile

WHISPER_MODEL = "base"
LANGUAGE      = None    # e.g. "en", "fi" — None = auto-detect

# Punctuation that signals a natural caption break
BREAK_PUNCTUATION = {".", ",", "!", "?", ";", ":", "…"}


def extract_audio(input_path: str, output_path: str):
    """
    Extracts audio from the input file using ffmpeg.
    Parameters:
        input_path (str): Path to the input audio or video file.
        output_path (str): Path where the extracted WAV audio will be saved.
    Raises:
        RuntimeError: If ffmpeg fails to extract the audio.
     """

    # Extract mono 16kHz PCM audio using ffmpeg
    result = subprocess.run([
        "ffmpeg", "-y", "-i", input_path,
        "-ar", "16000", "-ac", "1", "-c:a", "pcm_s16le",
        output_path
    ], capture_output=True)
    
    if result.returncode != 0:
        raise RuntimeError(f"ffmpeg failed:\n{result.stderr.decode()}")


def transcribe(wav_path: str) -> list:
    """
    Transcribes the given WAV audio file into a list of words with timestamps using
    the Whisper model.
    Parameters:
        wav_path (str): Path to the WAV audio file to transcribe.
    Returns:
        list: A list of dictionaries, each containing 'word', 'start', and 'end'
        keys for each recognized word.
    """
    
    print(f"Loading Whisper model '{WHISPER_MODEL}'...")

    model = whisper.load_model(WHISPER_MODEL)
    kwargs = {"word_timestamps": True}

    if LANGUAGE:
        kwargs["language"] = LANGUAGE

    print("Transcribing...")
    
    # Transcribe the audio file and get word-level timestamps
    result = model.transcribe(wav_path, **kwargs)
    words = []

    # Extract words and their timestamps from the transcription result
    for seg in result["segments"]:
        for w in seg.get("words", []):
            words.append({
                "word":  w["word"].strip(),
                "start": w["start"],
                "end":   w["end"],
            })

    print(f"Got {len(words)} word(s).")
    return words


def words_to_captions_by_punctuation(words: list) -> list:
    """
    Split words into captions at natural punctuation boundaries.
    A caption ends when a word ends with a punctuation character.
    Parameters:
        words (list): A list of dictionaries, each containing 'word', 'start', and
        'end' keys for each recognized word.
    Returns:
        list: A list of caption dictionaries, each containing 'start', 'end', and
        'text' keys for each caption.
    """
    if not words:
        return []

    captions = []
    chunk    = []
    
    for w in words:
        chunk.append(w)

        if any(w["word"].endswith(p) for p in BREAK_PUNCTUATION):
            captions.append({
                "start": chunk[0]["start"],
                "end":   chunk[-1]["end"],
                "text":  " ".join(x["word"] for x in chunk),
            })
            chunk = []

    # Flush any remaining words that had no trailing punctuation
    if chunk:
        captions.append({
            "start": chunk[0]["start"],
            "end":   chunk[-1]["end"],
            "text":  " ".join(x["word"] for x in chunk),
        })

    return captions


def words_to_captions_by_count(words: list, n: int) -> list:
    """
    Splits a list of word dictionaries into captions with a fixed number of
    words per caption.
    Parameters:
        words (list): List of word dictionaries, each with 'word', 'start',
        and 'end' keys.
        n (int): Number of words per caption. If 0, all words are combined
        into a single caption.
    Returns:
        list: List of caption dictionaries, each with 'start', 'end', and
        'text' keys.
    """
    if not words:
        return []

    # If n=0, combine all words into a single caption
    if n == 0:
        return [{"start": words[0]["start"], "end": words[-1]["end"],
                 "text": " ".join(w["word"] for w in words)}]

    # Split words into chunks of size n and create captions for each chunk
    out = []
    for i in range(0, len(words), n):
        chunk = words[i:i + n]
        out.append({"start": chunk[0]["start"], "end": chunk[-1]["end"],
                    "text": " ".join(w["word"] for w in chunk)})

    return out


def fmt_ts(s: float) -> str:
    """
    Formats a time value in seconds as an SRT timestamp string (HH:MM:SS,mmm).
    Parameters:
        s (float): Time in seconds.
    Returns:
        str: Timestamp in SRT format (e.g., '00:01:23,456').
    """
    h = int(s // 3600); m = int((s % 3600) // 60)
    sec = int(s % 60); ms = int((s - int(s)) * 1000)

    return f"{h:02}:{m:02}:{sec:02},{ms:03}"


def save_srt(captions: list, path: str):
    """
    Saves a list of captions to an SRT subtitle file.
    Parameters:
        captions (list): List of caption dictionaries with 'start', 'end', and 'text' keys.
        path (str): Output file path for the SRT file.
    """
    with open(path, "w", encoding="utf-8") as f:
        for i, c in enumerate(captions, 1):
            f.write(f"{i}\n{fmt_ts(c['start'])} --> {fmt_ts(c['end'])}\n{c['text']}\n\n")

    print(f"Saved {len(captions)} captions to: {path}")


def main():

    # Check for valid input argument
    if len(sys.argv) < 2:
        print(__doc__)
        sys.exit(1)

    # Get input file path from command line arguments
    input_path = sys.argv[1]

    if not os.path.exists(input_path):
        print(f"Error: file not found: {input_path}")
        sys.exit(1)

    # Second argument is optional word count
    # If not given, use punctuation-based splitting
    use_punctuation   = len(sys.argv) < 3
    words_per_caption = int(sys.argv[2]) if not use_punctuation else None

    # Prepare output directory and SRT file path
    script_dir = os.path.dirname(os.path.abspath(__file__))
    output_dir = os.path.join(script_dir, "output")
    os.makedirs(output_dir, exist_ok=True)

    # Build output SRT file path
    srt_name = os.path.splitext(os.path.basename(input_path))[0] + ".srt"
    srt_path = os.path.join(output_dir, srt_name)

    # Use a temporary directory for intermediate audio extraction
    with tempfile.TemporaryDirectory() as tmp:
        wav_path = os.path.join(tmp, "audio.wav")
        print(f"Extracting audio from: {os.path.basename(input_path)}")
        extract_audio(input_path, wav_path)

        # Transcribe audio to word-level timestamps
        words = transcribe(wav_path)

        # Split words into captions based on user preference
        if use_punctuation:
            print("Splitting by punctuation...")
            captions = words_to_captions_by_punctuation(words)
        else:
            print(f"Splitting by word count ({words_per_caption})...")
            captions = words_to_captions_by_count(words, words_per_caption)

        # Save captions to SRT file
        save_srt(captions, srt_path)


if __name__ == "__main__":
    main()