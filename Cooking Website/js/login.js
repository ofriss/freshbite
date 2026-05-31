// login.js — client-side validation for the Login form

const uname = document.querySelector('#uname');
const pwd = document.querySelector('#pwd');
const unameMsg = document.querySelector('#uname-msg');
const pwdMsg = document.querySelector('#pwd-msg');

// Sets the error message on el; returns true when text is empty (no error)
function setMsg(el, text = '') {
    el.textContent = text;
    return text === '';
}

// Shows msg on el when cond is false; clears it otherwise
function requireCondition(cond, el, msg) {
    if (cond) {
        setMsg(el);
        return true;
    }
    setMsg(el, msg);
    return false;
}

// Returns true only when s has at least one lowercase and one uppercase letter
function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}

// Validates username: min length, no spaces, alphanumeric only
function checkName() {
    const val = uname.value || '';

    if (!requireCondition(val.length >= 4, unameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), unameMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(val), unameMsg, 'No special characters allowed.')) return false;

    return true;
}

// Validates password: min length, no spaces, mixed case, allowed special chars only
function checkPass() {
    const val = pwd.value || '';

    if (!requireCondition(val.length >= 8, pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(!/\s/.test(val), pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), pwdMsg, 'Must include upper & lower case.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9!*@_$#]/.test(val), pwdMsg, "Password can't have special characters (other than !*@_$#).")) return false;

    return true;
}

// Called by the form's onsubmit; returns false to block submission on any failure
function onSubmit() {
    const nameOk = checkName();
    const passOk = checkPass();

    return nameOk && passOk;
}

// Clears all error messages when the form is reset
function onReset() {
    setMsg(unameMsg)
    setMsg(pwdMsg)
}
