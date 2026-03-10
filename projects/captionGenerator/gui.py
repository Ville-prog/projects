"""
MIT License
Copyright (c) 2026 Ville Laaksoaho

Caption Generator GUI

Drag-and-drop (or file-picker) desktop application that converts
an audio or video file into an SRT caption file.

The GUI is built with tkinter and the tkinterdnd2 extension
for native drag-and-drop support. All heavy processing: audio
extraction, Whisper transcription, caption splitting — is delegated
to the companion module captions.py.

Requirements:
    tkinterdnd2 (for drag-and-drop support, optional but recommended)
    plus all requirements from captions.py: openai-whisper, torch, ffmpeg

Usage:
    python gui.py
"""

import os
import tkinter as tk
from tkinter import filedialog
import tempfile

# Try to import tkinterdnd2 for native drag-and-drop support.
# If unavailable falls back to a click-to-browse
try:
    from tkinterdnd2 import TkinterDnD, DND_FILES
    DND_AVAILABLE = True
except ImportError:
    DND_AVAILABLE = False

from captions import (
    extract_audio,
    transcribe,
    words_to_captions_by_punctuation,
    words_to_captions_by_count,
    save_srt,
)

# Colours ======================================================================

BG          = "#5a8a9f"   # window background 
SURFACE     = "#71a0b7"   # drop zone / log
SURFACE_BTN = "#763c63"   # dropdown surface
BORDER      = "#ffffff"   # separators and borders
TEXT        = "#ffffff"   # primary text
TEXT_GREEN  = "#71b788"   # title accent word
TEXT_FADED  = "#d0e8f0"   # dimmed labels / status
BTN_BG      = "#71b788"   # generate button
BTN_HOVER   = "#b771a0"   # drop icon / hover tint

# Fonts ========================================================================

MONO       = ("Courier", 9)

MONO_SM    = ("Courier", 8)
MONO_LG    = ("Courier", 10, "bold")
MONO_TITLE = ("Courier", 13, "bold")


class App(TkinterDnD.Tk if DND_AVAILABLE else tk.Tk):
    """
    Main application window.

    Inherits from TkinterDnD.Tk when the drag-and-drop library is
    available, otherwise falls back to the plain tk.Tk root window.
    All UI construction happens in _build(); the captioning work
    is handled by _run_captioning().
    """

    def __init__(self):
        super().__init__()

        self.title("Caption Generator")
        self.configure(bg=BG)
        self.resizable(False, False)

        # Path to the media file selected by the user (None until set).
        self._file_path = None

        # Assemble the UI and run a quick dependency check.
        self._build()
        self._check_deps()

    def _build(self):
        """
        Construct every UI element and lay them out.

        The window is divided into vertical sections, top to bottom:
          1. Title bar
          2. Horizontal separator
          3. File drop zone
          4. Settings row: split-mode dropdown and generate button
          5. Log panel: scrollable text area for status messages
          6. Status bar: single-line summary at the bottom
        """

        # Title ================================================================

        hdr = tk.Frame(self, bg=BG)
        hdr.pack(fill="x", padx=20, pady=(20, 0))

        title = tk.Text(
            hdr, bg=BG, relief="flat", height=1, width=18,
            font=MONO_TITLE, cursor="arrow",
        )
        title.tag_config("c1", foreground=TEXT)
        title.tag_config("c2", foreground=TEXT_GREEN)
        title.insert("end", "CAPTION", "c1")
        title.insert("end", "GENERATOR", "c2")
        title.config(state="disabled")   # read-only; purely decorative
        title.pack(side="left")

        # Thin horizontal rule below the title.
        tk.Frame(self, bg=BORDER, height=1).pack(fill="x", padx=20, pady=(12, 0))

        # Drop zone ============================================================

        self.drop_frame = tk.Frame(
            self, bg=SURFACE, width=440, height=170,
            highlightbackground=BORDER, highlightthickness=1,
        )
        self.drop_frame.pack(padx=20, pady=16)
        self.drop_frame.pack_propagate(False)

        # Large arrow icon — acts as a visual cue for the drop target.
        self.drop_icon = tk.Label(
            self.drop_frame, text="⬇", bg=SURFACE, fg=BTN_HOVER,
            font=("Courier", 30),
        )
        self.drop_icon.pack(pady=(24, 4))

        # Instruction label — text adapts based on DnD availability.
        self.drop_label = tk.Label(
            self.drop_frame,
            text="drop audio / video file here" if DND_AVAILABLE else "click to select file",
            bg=SURFACE, fg=TEXT_FADED, font=MONO,
        )
        self.drop_label.pack()

        # Secondary hint shown beneath the main instruction.
        self.drop_sub = tk.Label(
            self.drop_frame, text="or click to browse",
            bg=SURFACE, fg=TEXT_FADED, font=MONO_SM,
        )
        self.drop_sub.pack(pady=(4, 0))

        # Bind click and hover events on every child widget inside the
        # drop zone so the entire area feels interactive.
        for w in [self.drop_frame, self.drop_icon, self.drop_label, self.drop_sub]:
            w.bind("<Button-1>", lambda e: self._browse())
            w.bind("<Enter>",    lambda e: self._hover(True))
            w.bind("<Leave>",    lambda e: self._hover(False))

        # Register for native file-drop events when the library is present.
        if DND_AVAILABLE:
            self.drop_frame.drop_target_register(DND_FILES)
            self.drop_frame.dnd_bind("<<Drop>>", self._on_drop)

        # Settings + generate button ===========================================

        tk.Frame(self, bg=BORDER, height=1).pack(fill="x", padx=20)

        sf = tk.Frame(self, bg=BG)
        sf.pack(fill="x", padx=20, pady=12)

        tk.Label(
            sf, text="SPLIT MODE", bg=BG, fg=TEXT_FADED,
            font=MONO_LG,
        ).pack(side="left")

        # Dropdown to select the caption splitting mode: either at punctuation
        # boundaries or after a fixed number of words.
        options = ["punctuation"] + [str(i) for i in range(0, 11)]
        self.split_var = tk.StringVar(value="punctuation")

        self.split_dropdown = tk.OptionMenu(sf, self.split_var, *options)
        self.split_dropdown.config(
            bg=SURFACE_BTN, fg=TEXT,
            activebackground=SURFACE, activeforeground=SURFACE_BTN,
            relief="flat", font=MONO,
            highlightthickness=0, bd=0, cursor="hand2",
            indicatoron=True, width=12,
        )
        self.split_dropdown["menu"].config(
            bg=SURFACE_BTN, fg=TEXT,
            activebackground=SURFACE, activeforeground=SURFACE_BTN,
            font=MONO, relief="flat", bd=0,
        )
        self.split_dropdown.pack(side="left", padx=(10, 0))

        # Main action button, kicks off the captioning pipeline.
        self.gen_btn = tk.Button(
            sf, text="Generate Captions!", command=self._on_generate,
            bg=BTN_BG, fg=TEXT,
            activebackground=TEXT, activeforeground=BG,
            relief="flat", cursor="hand2",
            font=MONO_LG, pady=4, padx=12, bd=0,
        )
        self.gen_btn.pack(side="left", padx=(12, 0))

        # Log panel ============================================================

        log_outer = tk.Frame(
            self, bg=SURFACE,
            highlightbackground=BORDER, highlightthickness=1,
        )
        log_outer.pack(fill="x", padx=20, pady=(0, 8))

        self.log_text = tk.Text(
            log_outer, height=6, state="disabled",
            bg=SURFACE, fg=TEXT, font=MONO,
            relief="flat", padx=10, pady=8, wrap="word",
        )

        sb = tk.Scrollbar(
            log_outer, command=self.log_text.yview,
            bg=SURFACE, troughcolor=SURFACE,
        )
        self.log_text.configure(yscrollcommand=sb.set)

        self.log_text.pack(side="left", fill="x", expand=True)
        sb.pack(side="right", fill="y")

        # Status bar ===========================================================

        self.status_var = tk.StringVar(value="ready")

        tk.Label(
            self, textvariable=self.status_var, bg=BG, fg=TEXT_FADED,
            font=MONO_SM, anchor="w",
        ).pack(fill="x", padx=20, pady=(0, 14))

    # Captioning pipeline ======================================================

    def _run_captioning(self, input_path, words_per_caption):
        """
        Execute the full caption-generation pipeline.

        Parameters:
            input_path (str): Absolute path to the source audio/video file.
            words_per_caption (int or None): None for punctuation-based splitting, or an
                                              int for fixed word-count splitting.
        """
        try:
            # Resolve the output directory relative to the script location
            # so the SRT file always lands in a predictable place.
            script_dir = os.path.dirname(os.path.abspath(__file__))
            output_dir = os.path.join(script_dir, "output")
            os.makedirs(output_dir, exist_ok=True)

            # Derive the .srt filename from the original media filename.
            srt_name = os.path.splitext(os.path.basename(input_path))[0] + ".srt"
            srt_path = os.path.join(output_dir, srt_name)

            # Use a temporary directory for the intermediate WAV so we
            # don't leave stale files on disk if the process fails.
            with tempfile.TemporaryDirectory() as tmp:
                wav_path = os.path.join(tmp, "audio.wav")

                # Stage 1: extract raw audio from the media container.
                self._log("Extracting audio...")
                extract_audio(input_path, wav_path)

                # Stage 2: run Whisper speech-to-text transcription.
                self._log("Transcribing with Whisper...")
                words = transcribe(wav_path)

                # Stage 3: split the word list into caption segments.
                if words_per_caption is None:
                    self._log("Splitting by punctuation...")
                    captions = words_to_captions_by_punctuation(words)
                else:
                    self._log(f"Splitting by word count ({words_per_caption})...")
                    captions = words_to_captions_by_count(words, words_per_caption)

                # Write the final SRT file and signal success.
                save_srt(captions, srt_path)
                self._on_done(True, srt_path, len(captions))

        except Exception as e:
            self._log(f"Error: {e}")
            self._on_done(False, None, 0)

    # UI event handlers ========================================================

    def _hover(self, on):
        """
        Visual feedback for the drop zone on mouse enter / leave.

        Parameters:
            on (bool): True when the mouse enters, False when it leaves.
        """
        self.drop_frame.config(highlightbackground=TEXT if on else BORDER)
        self.drop_icon.config(fg=TEXT if on else BTN_HOVER)


    def _browse(self):
        """
        Open a native file-picker dialog filtered by common audio and
        video formats.  If the user selects a file it is loaded into
        the application via _set_file().
        """
        path = filedialog.askopenfilename(filetypes=(
            ("Audio/Video", "*.mp4 *.mov *.mp3 *.wav *.m4a *.aac *.mkv *.avi *.flac *.ogg"),
            ("All files", "*.*"),
        ))

        if path:
            self._set_file(path)


    def _on_drop(self, event):
        """
        Handle a drag-and-drop event on the drop zone.
        Parameters:
            event (Tkinter.Event): The Object containing the dropped data.
        """
        paths = self.tk.splitlist(event.data)

        if paths:
            path = paths[0].strip("{}")

            if os.path.isfile(path):
                self._set_file(path)


    def _set_file(self, path):
        """
        Store the selected file path and update all drop-zone widgets
        to reflect the new selection.

        Parameters:
            path (str): Absolute filesystem path to the selected media file.
        """
        self._file_path = path
        name = os.path.basename(path)

        # Shorten long filenames so they fit inside the drop zone.
        display = ("…" + name[-38:]) if len(name) > 40 else name

        # Swap the arrow icon for a music note to indicate a file is loaded.
        self.drop_icon.config(text="♪", fg=TEXT)
        self.drop_label.config(text=display, fg=TEXT)
        self.drop_sub.config(text="click to change file")

        self.status_var.set("file ready")
        self._log(f"Loaded: {name}")


    def _log(self, msg):
        """
        Append a single line to the scrollable log panel.

        Parameters:
            msg (str): Plain-text message to display.
        """
        self.log_text.config(state="normal")
        self.log_text.insert("end", f"> {msg}\n")
        self.log_text.see("end")
        self.log_text.config(state="disabled")


    def _on_generate(self):
        """
        Validate inputs and start the captioning pipeline.
        """
        # Guard: make sure a valid file is loaded.
        if not self._file_path or not os.path.exists(self._file_path):
            self._log("No valid file selected.")
            return

        # Read the split-mode: "punctuation" maps to None,
        # numeric strings are converted to int.
        val = self.split_var.get()
        wpc = None if val == "punctuation" else int(val)

        # Disable the button and update status while processing.
        self.gen_btn.config(state="disabled", text="GENERATING…")
        self.status_var.set("generating…")

        # Run the pipeline.
        self._run_captioning(self._file_path, wpc)


    def _on_done(self, success, srt_path, count):
        """
        Callback invoked when _run_captioning finishes (success or
        failure).  Re-enables the generate button and updates the status
        bar with the outcome.

        Parameters:
            success (bool): True if the pipeline completed without error.
            srt_path (str or None): Path to the generated SRT file (or None on failure).
            count (int): Number of caption segments written.
        """
        # Re-enable the generate button regardless of outcome.
        self.gen_btn.config(state="normal", text="Generate Captions!")

        if success:
            fname = os.path.basename(srt_path)
            self.status_var.set(f"done  —  {count} captions  —  {fname}")
            self._log(f"Saved {count} captions → output/{fname}")
        else:
            self.status_var.set("failed — see log")

    def _check_deps(self):
        """
        Run a quick dependency audit on startup.

        Checks for the presence of:
          - openai-whisper
          - ffmpeg       
          - tkinterdnd2

        Any missing items are logged as warnings so the user knows what
        to install before attempting to generate captions.
        """
        import shutil

        missing = []

        # Check for the Whisper speech recognition library.
        try:
            import whisper  # noqa: F401 – presence check only
        except ImportError:
            missing.append("openai-whisper  →  pip install openai-whisper torch")

        # Check that the ffmpeg binary is on the system PATH.
        if not shutil.which("ffmpeg"):
            missing.append("ffmpeg  →  winget install ffmpeg")

        # Note if drag-and-drop support is unavailable.
        if not DND_AVAILABLE:
            missing.append("tkinterdnd2  →  pip install tkinterdnd2  (drag-drop disabled)")

        # Surface all warnings in the log panel.
        for m in missing:
            self._log(f"⚠  {m}")

        if missing:
            self.status_var.set("⚠ missing dependencies — see log")

if __name__ == "__main__":
    App().mainloop()