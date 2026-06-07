// timer.js — the simplest possible kitchen countdown timer.
// Enter hours/minutes/seconds, press Start to count down, Pause to halt,
// Reset to clear. When it reaches zero it shows "Done!" and beeps once.

/* ── ELEMENT REFERENCES ──────────────────────────────────────── */

const inpHours = document.getElementById("inp-hours");
const inpMinutes = document.getElementById("inp-minutes");
const inpSeconds = document.getElementById("inp-seconds");
const timerDisplay = document.getElementById("timer-display");
const btnStart = document.getElementById("btn-start");
const btnReset = document.getElementById("btn-reset");


/* ── STATE ───────────────────────────────────────────────────── */

let remainingSeconds = 0;   // seconds left on the countdown
let intervalId = null;      // setInterval handle while running
let isRunning = false;      // true while counting down


/* ── HELPERS ─────────────────────────────────────────────────── */

// Pad a number to two digits, e.g. 5 -> "05"
function pad(n) {
    return String(n).padStart(2, "0");
}

// Read the three inputs and return the total number of seconds
function readInputs() {
    const h = parseInt(inpHours.value, 10) || 0;
    const m = parseInt(inpMinutes.value, 10) || 0;
    const s = parseInt(inpSeconds.value, 10) || 0;
    return (h * 3600) + (m * 60) + s;
}

// Format a total-seconds value as "HH:MM:SS"
function format(totalSeconds) {
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;
    return pad(h) + ":" + pad(m) + ":" + pad(s);
}


/* ── COUNTDOWN ───────────────────────────────────────────────── */

// Called once per second while running
function tick() {
    remainingSeconds -= 1;
    timerDisplay.textContent = format(remainingSeconds);

    if (remainingSeconds <= 0) {
        clearInterval(intervalId);
        isRunning = false;
        timerDisplay.textContent = "Done!";
        timerDisplay.classList.add("done");
        btnStart.textContent = "Start";
        playBeep();
    }
}

// Start button doubles as Pause while the timer is running
function startOrPause() {
    if (isRunning) {
        // Pause: stop ticking but keep remainingSeconds so Start resumes
        clearInterval(intervalId);
        isRunning = false;
        btnStart.textContent = "Start";
        return;
    }

    // If we're not mid-countdown, load a fresh time from the inputs
    if (remainingSeconds <= 0) {
        remainingSeconds = readInputs();
        if (remainingSeconds <= 0) return;   // nothing to count down
    }

    timerDisplay.classList.remove("done");
    timerDisplay.textContent = format(remainingSeconds);
    isRunning = true;
    btnStart.textContent = "Pause";
    intervalId = setInterval(tick, 1000);
}

// Reset back to the entered time and a stopped state
function reset() {
    clearInterval(intervalId);
    isRunning = false;
    remainingSeconds = 0;
    timerDisplay.classList.remove("done");
    timerDisplay.textContent = format(readInputs());
    btnStart.textContent = "Start";
}


/* ── AUDIO ALERT ─────────────────────────────────────────────── */

// A single short sine-wave beep when the timer finishes
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
    } catch (e) { /* fail silently if Web Audio is unavailable */ }
}


/* ── INITIALISATION ──────────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    btnStart.addEventListener("click", startOrPause);
    btnReset.addEventListener("click", reset);

    // Show the entered time on load and whenever an input changes (while stopped)
    [inpHours, inpMinutes, inpSeconds].forEach(function (inp) {
        inp.addEventListener("input", function () {
            if (!isRunning) {
                timerDisplay.classList.remove("done");
                timerDisplay.textContent = format(readInputs());
            }
        });
    });
});
