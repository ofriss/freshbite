/**
 * Client-side validation helpers for the registration form.
 *
 * This module queries the DOM for the form controls and their corresponding
 * message elements, and exposes `onSubmit` and `onReset` functions used by
 * the form handlers. Each `check*` function validates a single field,
 * sets a human-friendly error message into the corresponding message element
 * (via `setMsg`) and returns `true` when the field is valid.
 *
 * Notes:
 * - Date parsing uses a 'T00:00:00' suffix to avoid timezone issues for
 *   YYYY-MM-DD inputs.
 * - `onSubmit` returns a boolean (all checks must pass) so it can be used
 *   directly as a form `onsubmit` handler (preventing submission when false).
 */

/* Form controls */
const username = document.getElementById('uname');
const pwd = document.getElementById('pwd');
const pwdValid = document.getElementById('pwd-valid');
const bday = document.getElementById('bday');
const terms = document.getElementById('terms');
const skill = document.getElementById('skill');

/* Message elements shown to the user for validation feedback */
const usernameMsg = document.getElementById('uname-msg');
const pwdMsg = document.getElementById('pwd-msg');
const pwdValidMsg = document.getElementById('pwd-valid-msg');
const bdayMsg = document.getElementById('bday-msg');
const genderMsg = document.getElementById('gender-msg');
const termsMsg = document.getElementById('terms-msg');
const cuisineMsg = document.getElementById('cuisine-msg');
const skillMsg = document.getElementById('skill-msg');

/**
 * Set the text content of a message element.
 *
 * @param {Element} el - The DOM element that will receive the message.
 * @param {string} [text=''] - Message text. Empty string clears the message.
 * @returns {boolean} True when message is empty (no error).
 */
function setMsg(el, text = '') {
    el.textContent = text;
    return text === '';
}

/**
 * Check whether a string contains both lowercase and uppercase letters.
 *
 * @param {string} s - Input string.
 * @returns {boolean} True when the string contains at least one lowercase
 * and one uppercase character.
 */
function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}

/**
 * Validate the username.
 *
 * Rules:
 * - Must be at least 4 characters long after trimming.
 * - Must not contain whitespace.
 *
 * Sets `usernameMsg` on error.
 *
 * @returns {boolean} True when the username is valid.
 */
function checkName() {
    const val = (username.value || '').trim();
    if (val.length < 4) {
        setMsg(usernameMsg, 'Username too short.');
        return false;
    }
    if (/\s/.test(val)) {
        setMsg(usernameMsg, 'No spaces allowed.');
        return false;
    }
    setMsg(usernameMsg);
    return true;
}

/**
 * Validate the password confirmation field matches the main password.
 *
 * Sets `pwdValidMsg` when the values don't match.
 *
 * @returns {boolean} True when passwords match.
 */
function checkPassValidation() {
    const ok = pwd.value === pwdValid.value;
    if (!ok) setMsg(pwdValidMsg, "Password doesn't match.");
    else setMsg(pwdValidMsg);
    return ok;
}

/**
 * Validate the password field.
 *
 * Rules:
 * - No whitespace.
 * - Minimum length 8.
 * - Must contain both upper and lower case characters.
 *
 * This function calls `checkPassValidation()` first so users see mismatch
 * feedback early. It sets `pwdMsg` when the password fails validation.
 *
 * @returns {boolean} True when the password meets the rules.
 */
function checkPass() {
    const val = pwd.value || '';
    // validate match first so user sees that early
    checkPassValidation();

    if (/\s/.test(val)) {
        setMsg(pwdMsg, 'No spaces allowed.');
        return false;
    }
    if (val.length < 8) {
        setMsg(pwdMsg, 'Password too short.');
        return false;
    }
    if (!hasLowerAndUpper(val)) {
        setMsg(pwdMsg, 'Must include upper & lower case.');
        return false;
    }
    setMsg(pwdMsg);
    return true;
}

/**
 * Determine whether a birth date string represents at least `age` years old.
 *
 * The `dateString` is expected to be in a format accepted by the `Date`
 * constructor (commonly 'YYYY-MM-DD' from date inputs). Appending 'T00:00:00'
 * avoids timezone-related off-by-one-day issues.
 *
 * @param {string} dateString - Birth date string.
 * @param {number} age - Minimum required age in years.
 * @returns {boolean} True when the calculated age is >= `age`.
 */
function isAtLeastAge(dateString, age) {
    if (!dateString) return false;
    const birth = new Date(dateString + 'T00:00:00');
    const today = new Date();
    let years = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) years--;
    return years >= age;
}

/**
 * Validate the birthday input.
 *
 * Rules:
 * - A value is required.
 * - User must be at least 12 years old.
 *
 * Sets `bdayMsg` on error.
 *
 * @returns {boolean} True when the birthday is provided and meets the age.
 */
function checkBirthday() {
    const val = bday.value;
    if (!val) {
        setMsg(bdayMsg, 'Birthday required.');
        return false;
    }
    if (!isAtLeastAge(val, 12)) {
        setMsg(bdayMsg, 'Must be at least 12.');
        return false;
    }
    setMsg(bdayMsg);
    return true;
}

/**
 * Ensure a gender option (radio) is selected.
 *
 * Looks for `input[name="gender"]:checked` and sets `genderMsg` if none is found.
 *
 * @returns {boolean} True when a gender radio is selected.
 */
function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    if (!selected) {
        setMsg(genderMsg, 'Select gender.');
        return false;
    }
    setMsg(genderMsg);
    return true;
}

/**
 * Ensure the terms checkbox is checked.
 *
 * Sets `termsMsg` when the terms are not agreed to.
 *
 * @returns {boolean} True when `terms` is checked.
 */
function checkTerms() {
    if (!terms.checked) {
        setMsg(termsMsg, 'You must agree.');
        return false;
    }
    setMsg(termsMsg);
    return true;
}

/**
 * Validate cuisine preferences (checkbox group).
 *
 * Requires at least one `input[name="cuisine"]` to be checked. Updates `cuisineMsg`.
 *
 * @returns {boolean} True when at least one cuisine is selected.
 */
function checkCuisine() {
    const checked = document.querySelectorAll('input[name="cuisine"]:checked');
    if (!checked || checked.length === 0) {
        setMsg(cuisineMsg, 'Select at least one cuisine.');
        return false;
    }
    setMsg(cuisineMsg);
    return true;
}

/**
 * Validate the skill/select input is set.
 *
 * Sets `skillMsg` when no selection is made.
 *
 * @returns {boolean} True when a skill level is selected.
 */
function checkSkill() {
    if (!skill.value) {
        setMsg(skillMsg, 'Select skill level.');
        return false;
    }
    setMsg(skillMsg);
    return true;
}

/**
 * Orchestrate all validations for form submission.
 *
 * Calls each `check*` function and returns true only if all validations pass.
 * Intended to be used as a form `onsubmit` handler; returning `false` will
 * prevent submission when validation fails.
 *
 * @returns {boolean} True when all fields are valid.
 */
function onSubmit() {
    const nameOk = checkName();
    const passOk = checkPass();
    const passValidOk = checkPassValidation();
    const bdayOk = checkBirthday();
    const genderOk = checkGender();
    const termsOk = checkTerms();
    const cuisineOk = checkCuisine();
    const skillOk = checkSkill();

    return nameOk && passOk && passValidOk && bdayOk && genderOk && termsOk && cuisineOk && skillOk;
}

/**
 * Clear all validation messages.
 *
 * Useful as the handler for a form reset so the UI messages are removed.
 */
function onReset() {
    [usernameMsg, pwdMsg, pwdValidMsg, bdayMsg, genderMsg, termsMsg, cuisineMsg, skillMsg].forEach(el => setMsg(el));
}