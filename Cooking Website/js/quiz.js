// quiz.js — controls quiz start flow and answer validation before submission
// questionsCount is injected into the page by the server before this script runs

// Reveals the question list and hides the start button
function startQuiz() {
    document.querySelector(".start-btn").style.display = 'none';
    document.querySelector("#quizSection").style.display = 'flex';
    document.querySelector(".submit-btn").style.display = 'block';
    document.querySelector("#quizInfo").textContent = "Good Luck!";
}

// When returning via back-button postback, restore the in-progress state immediately
if (restore == "true") {
    startQuiz();
}

// Submission
const msg = document.querySelector("#msg"); // For failure to submit

// Blocks form submission if any question has no answer selected
function onSubmit() {
    // Check if at least one input is unchecked
    for (let i = 1; i <= questionsCount; i++) {
        if (!document.querySelector(`input[name="q${i}"]:checked`)) {
            msg.textContent = "Answer all of the questions first";
            return false;
        }
    }
    return true;
}
