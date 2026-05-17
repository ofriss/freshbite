/**
 * References to DOM elements used by the login form.
 * - `#uname` input: username text field.
 * - `#pwd` input: password text field.
 * - `#uname-msg` element: place to show username validation messages.
 * - `#pwd-msg` element: place to show password validation messages.
 */
const uname = document.querySelector('#uname');
const pwd = document.querySelector('#pwd');
const unameMsg = document.querySelector('#uname-msg');
const pwdMsg = document.querySelector('#pwd-msg');

/**
 * Validation state flags updated by `checkName` and `checkPass`.
 * These are used by `onSubmit` to determine whether the form should be allowed to submit.
 */
let nameOk = false;
let passOk = false;

/**
 * isLower
 * Check whether a single character is a lowercase ASCII letter.
 *
 * @param {string} c - Single-character string to test.
 * @returns {boolean} True if `c` is between 'a' and 'z'.
 */
function isLower(c) {
    let code = c.charCodeAt(0);
    return code >= 97 && code <= 122;
}

/**
 * isUpper
 * Check whether a single character is an uppercase ASCII letter.
 *
 * @param {string} c - Single-character string to test.
 * @returns {boolean} True if `c` is between 'A' and 'Z'.
 */
function isUpper(c) {
    let code = c.charCodeAt(0);
    return code >= 65 && code <= 90;
}

/**
 * checkName
 * Validate the username input and update UI state.
 *
 * Rules:
 * - Minimum length: 4 characters.
 * - No spaces allowed.
 *
 * Side effects:
 * - Sets `unameMsg.textContent` with an error message when invalid, or clears it when valid.
 * - Updates `nameOk` to `true` when the value passes validation, otherwise `false`.
 */
function checkName() {
    const val = uname.value;
    nameOk = false;
    if (val.length < 4) {
        unameMsg.textContent = 'Username too short.';
        return;
    }
    if (val.includes(' ')) {
        unameMsg.textContent = 'No spaces allowed.';
        return;
    }
    unameMsg.textContent = '';
    nameOk = true;
}

/**
 * checkPass
 * Validate the password input and update UI state.
 *
 * Rules:
 * - No spaces allowed.
 * - Minimum length: 8 characters.
 * - Must include at least one lowercase and one uppercase ASCII letter.
 *
 * Side effects:
 * - Sets `pwdMsg.textContent` with an error message when invalid, or clears it when valid.
 * - Updates `passOk` to `true` when the value passes validation, otherwise `false`.
 */
function checkPass() {
    const val = pwd.value;
    passOk = false;

    if (val.includes(' ')) {
        pwdMsg.textContent = 'No spaces allowed.';
        return;
    }

    if (val.length < 8) {
        pwdMsg.textContent = 'Password too short.';
        return;
    }

    let hasLower = false, hasUpper = false;
    for (let c of val) {
        if (isLower(c)) hasLower = true;
        if (isUpper(c)) hasUpper = true;
    }

    if (!hasLower || !hasUpper) {
        pwdMsg.textContent = 'Must include upper & lower case.';
        return;
    }

    pwdMsg.textContent = '';
    passOk = true;
}

/**
 * onSubmit
 * Form submission handler. Runs both validation checks and returns a boolean
 * indicating whether form submission should proceed.
 *
 * @returns {boolean} True if both `nameOk` and `passOk` are true (valid), otherwise false.
 */
function onSubmit() {
    checkName();
    checkPass();

    return nameOk && passOk;
}

/**
 * onReset
 * Form reset handler. Clears validation messages from the UI.
 *
 * Side effects:
 * - Clears `unameMsg.textContent` and `pwdMsg.textContent`.
 */
function onReset() {
    unameMsg.textContent = '';
    pwdMsg.textContent = '';
}
