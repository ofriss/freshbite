console.log(questionsCount);

// Start quiz
function startQuiz() {
    document.querySelector(".start-btn").style.display = 'none';
    document.querySelector("#quizSection").style.display = 'flex';
    document.querySelector(".submit-btn").style.display = 'block';
    document.querySelector("#quizInfo").textContent = "Good Luck!";
}

if (restore == "true") {
    startQuiz();
}

// Submission
const msg = document.querySelector("#msg"); // For failure to submit

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
