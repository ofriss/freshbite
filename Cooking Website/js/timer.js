// timer.js — countdown kitchen timer with click-to-edit time segments, progress bar, and an end beep

/* ── 1. ELEMENT REFERENCES ───────────────────────────────────── */

const timerDisplay = document.getElementById("timer-display");
const segHours = document.getElementById("seg-hours");
const segMinutes = document.getElementById("seg-minutes");
const segSeconds = document.getElementById("seg-seconds");
const segInput = document.getElementById("seg-input");
const timerBtn = document.getElementById("timer-btn");
const progressFill = document.getElementById("progress-fill");

const allSegments = [segHours, segMinutes, segSeconds];


/* ── 2. TIMER STATE ──────────────────────────────────────────── */

const state = {
    totalSeconds: 0,
    remainingSeconds: 0,
    intervalId: null,
    isRunning: false,
    editingSegment: null,   // which segment is being typed into
};


/* ── 3. TIME HELPERS ─────────────────────────────────────────── */

function pad(n) {
    return String(n).padStart(2, "0");
}

// Read the three visible segments and return total seconds
function readSegmentSeconds() {
    const h = parseInt(segHours.textContent, 10) || 0;
    const m = parseInt(segMinutes.textContent, 10) || 0;
    const s = parseInt(segSeconds.textContent, 10) || 0;
    return (h * 3600) + (m * 60) + s;
}

// Write a total-seconds value back into the three segments
function writeSegments(totalSecs) {
    const h = Math.floor(totalSecs / 3600);
    const m = Math.floor((totalSecs % 3600) / 60);
    const s = totalSecs % 60;
    segHours.textContent = pad(h);
    segMinutes.textContent = pad(m);
    segSeconds.textContent = pad(s);
}


/* ── 4. DISPLAY MODES ────────────────────────────────────────── */

function setDisplayMode(mode) {
    // mode: "idle" | "running" | "done"
    timerDisplay.className = "timer-display " + mode;
}


/* ── 5. PROGRESS BAR ─────────────────────────────────────────── */

function updateProgress(remainingSecs, totalSecs) {
    const fraction = totalSecs > 0 ? remainingSecs / totalSecs : 0;
    progressFill.style.transform = "scaleX(" + fraction + ")";

    if (remainingSecs <= 10 && remainingSecs > 0) {
        progressFill.classList.add("urgent");
    } else {
        progressFill.classList.remove("urgent");
    }
}


/* ── 6. SEGMENT EDITING ──────────────────────────────────────── */

/*
  When the user clicks a segment (HH, MM, SS) while the timer is idle:
  - We move and resize a real <input> on top of it
  - On Enter or blur, we commit the value and close the input
*/

function openSegmentEditor(segment) {
    state.editingSegment = segment;

    const max = parseInt(segment.getAttribute("data-max"), 10);
    segInput.max = max;
    segInput.value = parseInt(segment.textContent, 10) || 0;

    // Position the input over the segment
    const rect = segment.getBoundingClientRect();
    const cardRect = segment.closest(".timer-card").getBoundingClientRect();

    segInput.style.position = "absolute";
    segInput.style.opacity = "1";
    segInput.style.pointerEvents = "auto";
    segInput.style.width = rect.width + "px";
    segInput.style.height = rect.height + "px";
    segInput.style.top = (rect.top - cardRect.top) + "px";
    segInput.style.left = (rect.left - cardRect.left) + "px";
    segInput.style.fontSize = "4.5rem";
    segInput.style.fontFamily = "var(--font-heading)";
    segInput.style.fontWeight = "400";
    segInput.style.color = "var(--green-dark)";
    segInput.style.background = "var(--green-soft)";
    segInput.style.border = "2px solid var(--green-dark)";
    segInput.style.borderRadius = "6px";
    segInput.style.textAlign = "center";
    segInput.style.outline = "none";
    segInput.style.zIndex = "10";
    segInput.style.lineHeight = "1";
    segInput.style.padding = "4px 6px";
    segInput.style.MozAppearance = "textfield";
    segInput.style.appearance = "textfield";

    segInput.focus();
    segInput.select();
}

function closeSegmentEditor(commit) {
    if (!state.editingSegment) return;

    if (commit) {
        const max = parseInt(state.editingSegment.getAttribute("data-max"), 10);
        const val = Math.min(max, Math.max(0, parseInt(segInput.value, 10) || 0));
        state.editingSegment.textContent = pad(val);
    }

    // Hide the input again
    segInput.style.opacity = "0";
    segInput.style.pointerEvents = "none";
    segInput.style.width = "0";
    segInput.style.height = "0";
    state.editingSegment = null;
}


/* ── 7. TICK ─────────────────────────────────────────────────── */

function tick() {
    state.remainingSeconds -= 1;

    writeSegments(state.remainingSeconds);
    updateProgress(state.remainingSeconds, state.totalSeconds);

    if (state.remainingSeconds <= 0) {
        clearInterval(state.intervalId);
        state.isRunning = false;
        writeSegments(0);
        updateProgress(0, state.totalSeconds);
        setDisplayMode("done");
        timerBtn.textContent = "Reset";
        timerBtn.classList.remove("stopping");
        playBeep();
    }
}


/* ── 8. START / STOP ─────────────────────────────────────────── */

function handleButton() {
    // ── RESET (after done) ──
    if (!state.isRunning && timerBtn.textContent === "Reset") {
        writeSegments(state.totalSeconds);
        updateProgress(1, 1);
        progressFill.classList.remove("urgent");
        setDisplayMode("idle");
        timerBtn.textContent = "Start";
        timerBtn.classList.remove("stopping");
        return;
    }

    // ── STOP ──
    if (state.isRunning) {
        clearInterval(state.intervalId);
        state.isRunning = false;
        setDisplayMode("idle");
        timerBtn.textContent = "Start";
        timerBtn.classList.remove("stopping");
        return;
    }

    // ── START ──
    const total = readSegmentSeconds();
    if (total <= 0) return;

    state.totalSeconds = total;
    state.remainingSeconds = total;
    state.isRunning = true;

    updateProgress(1, 1);
    setDisplayMode("running");
    timerBtn.textContent = "Stop";
    timerBtn.classList.add("stopping");

    state.intervalId = setInterval(tick, 1000);
}


/* ── 9. AUDIO ALERT ──────────────────────────────────────────── */

function playBeep() {
    try {
        const ctx = new (window.AudioContext || window.webkitAudioContext)();
        const osc = ctx.createOscillator();
        const gain = ctx.createGain();

        osc.connect(gain);
        gain.connect(ctx.destination);

        osc.type = "sine";
        osc.frequency.value = 880;
        gain.gain.setValueAtTime(0.5, ctx.currentTime);
        gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + 1.2);

        osc.start();
        osc.stop(ctx.currentTime + 1.2);
    } catch (e) { /* fail silently */ }
}


/* ── 10. INITIALISATION ──────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {

    timerBtn.addEventListener("click", handleButton);

    // Click a time segment to edit it (only when idle)
    allSegments.forEach(function (seg) {
        seg.addEventListener("click", function () {
            if (state.isRunning) return;
            openSegmentEditor(seg);
        });
    });

    // Commit on Enter, cancel on Escape
    segInput.addEventListener("keydown", function (e) {
        if (e.key === "Enter") { closeSegmentEditor(true); timerBtn.focus(); }
        if (e.key === "Escape") { closeSegmentEditor(false); }
    });

    // Commit on blur (clicking away)
    segInput.addEventListener("blur", function () {
        closeSegmentEditor(true);
    });

    // Set idle mode on load
    setDisplayMode("idle");
});