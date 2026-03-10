# Caption Generator

A local, free, and offline tool that generates SRT caption files from any audio or video file using OpenAI's Whisper speech recognition model.

## Background

I run a YouTube channel where adding captions to a video manually is a very repetitive and time-consuming task. Most captioning tools are either paywalled or produce poor results. This tool solves that by running entirely on your own machine. No subscriptions no internet connection required after the initial setup.

## How it works

1. Export your finished timeline audio (or any audio with speech) as a WAV or MP3
2. Run the tool either through the GUI or the command line
3. Import the generated SRT file back into your editor

---

## Requirements

- Python 3.9+
- **ffmpeg** converts any audio or video format into the raw WAV that Whisper requires as input.
- **openai-whisper** transcribes the audio into words
- **torch** framework that Whisper relies on
- **tkinterdnd2** adds native drag-and-drop support

**Install Python dependencies:**
```bash
pip install -r requirements.txt
```

**Install ffmpeg:**
```bash
# Windows
winget install ffmpeg

# macOS
brew install ffmpeg

# Linux
sudo apt install ffmpeg
```

---

## Usage

### Trough GUI

```bash
python gui.py
```

Drop your audio or video file onto the window, choose a split mode, and click **Generate Captions**. The SRT file is saved to the `output/` folder next to the script.

### Command line

```bash
python captions.py <file> [words_per_caption]
```

**Examples:**
```bash
# Split at punctuation boundaries (default)
python captions.py my_export.wav

# Split every 4 words
python captions.py my_export.wav 4

# No splitting — one caption for the whole file
python captions.py my_export.wav 0
```

---

## Configuration

Open `captions.py` and edit the constants at the top:

```python
WHISPER_MODEL = "base"   # tiny | base | small | medium | large
LANGUAGE      = None     # e.g. "en", "fi" — None = auto-detect
```

### Whisper model sizes

| Model | Speed | Accuracy | VRAM |
|---|---|---|---|
| `tiny` | Fastest | Low | ~75 MB |
| `base` | Fast | OK | ~145 MB |
| `small` | Moderate | Good | ~460 MB |
| `medium` | Slow | Great | ~1.5 GB |
| `large` | Slowest | Best | ~3 GB |

Models are downloaded automatically on first use and cached locally.

---

## License

This project is licensed under the MIT License. See the LICENSE file for details.