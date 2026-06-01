// register.js — client-side validation for the Register form

// Centralised element references to avoid repeated getElementById calls
const els = {
    username: document.getElementById('uname'),
    pwd: document.getElementById('pwd'),
    pwdValid: document.getElementById('pwd-valid'),
    bday: document.getElementById('bday'),
    terms: document.getElementById('terms'),
    skill: document.getElementById('skill'),

    usernameMsg: document.getElementById('uname-msg'),
    pwdMsg: document.getElementById('pwd-msg'),
    pwdValidMsg: document.getElementById('pwd-valid-msg'),
    bdayMsg: document.getElementById('bday-msg'),
    genderMsg: document.getElementById('gender-msg'),
    termsMsg: document.getElementById('terms-msg'),
    cuisineMsg: document.getElementById('cuisine-msg'),
    skillMsg: document.getElementById('skill-msg')
};

// Sets error text on el; returns true when text is empty (no error)
function setMsg(el, text = '') {
    el.textContent = text;
    return text === '';
}

// Clears el's message when cond passes, otherwise shows msg
function requireCondition(cond, el, msg) {
    if (cond) {
        setMsg(el);
        return true;
    }
    setMsg(el, msg);
    return false;
}

// Returns true only when s contains at least one lowercase and one uppercase letter
function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}

// Validates username: min 4 chars, no spaces, alphanumeric only
function checkName() {
    const val = (els.username.value || '').trim();

    if (!requireCondition(val.length >= 4, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(val), els.usernameMsg, 'No special characters allowed.')) return false;

    return true;
}

// Confirms the confirmation field matches the password field
function checkPassValidation() {
    return requireCondition(els.pwd.value === els.pwdValid.value, els.pwdValidMsg, "Password doesn't match.");
}

// Validates password strength, then cross-checks against birthday year and username
function checkPass() {
    const val = els.pwd.value || '';

    checkPassValidation();

    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    if (!requireCondition(/[0-9]/.test(val), els.pwdMsg, 'Must include at least one digit.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9!*@_$#]/.test(val), els.pwdMsg, "Password can't have special characters (other than !*@_$#).")) return false;

    if (els.bday.value) {
        const birthdayYear = new Date(els.bday.value).getFullYear();
        if (!requireCondition(!val.includes(birthdayYear), els.pwdMsg, "Password can't have your birthday in it.")) return false;
    }

    if (els.username.value) {
        if (!requireCondition(!val.includes(els.username.value), els.pwdMsg, "Password can't have your username in it.")) return false;
    }
    
    return true;
}

// Calculates exact age accounting for whether the birthday has passed this year
function calculateAge(birthdate) {
    if (!birthdate) return 0;
    const today = new Date();
    const birth = new Date(birthdate)

    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();

    if (monthDiff < 0 || (monthDiff == 0 && today.getDate() < birth.getDate())) {
        age--;
    }

    return age;
}

// Returns true when calculated age falls within [min, max]
function ageInRange(birthdate, min, max) {
    const age = calculateAge(birthdate);
    return age >= min && age <= max;
}

// Validates birthday: required and age must be between 12 and 120
function checkBirthday() {
    const val = els.bday.value;
    if (!requireCondition(!!val, els.bdayMsg, 'Birthday required.')) return false;
    if (!requireCondition(ageInRange(val, 12, 120), els.bdayMsg, 'Age has to be above 12 and below 120.')) return false;
    return true;
}

// Validates that one of the gender radio buttons is selected
function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    return requireCondition(!!selected, els.genderMsg, 'Select gender.');
}

// Validates that the terms checkbox is ticked
function checkTerms() {
    return requireCondition(els.terms.checked, els.termsMsg, 'You must agree.');
}

// Validates that at least one cuisine preference is checked
function checkCuisine() {
    const checkedCount = document.querySelectorAll('input[name="cuisine"]:checked').length;
    return requireCondition(checkedCount > 0, els.cuisineMsg, 'Select at least one cuisine.');
}

// Validates that a skill level is selected
function checkSkill() {
    return requireCondition(!!els.skill.value, els.skillMsg, 'Select skill level.');
}

// Runs all field validators; returns false to block form submission on any failure
function onSubmit() {
    const nameOk = checkName();
    const passOk = checkPass();
    const bdayOk = checkBirthday();
    const genderOk = checkGender();
    const termsOk = checkTerms();
    const cuisineOk = checkCuisine();
    const skillOk = checkSkill();

    return nameOk && passOk && bdayOk && genderOk && termsOk && cuisineOk && skillOk;
}

// Clears all error messages when the form is reset
function onReset() {
    // Only clear message elements, not input/select elements
    [els.usernameMsg, els.pwdMsg, els.pwdValidMsg, els.bdayMsg,
        els.genderMsg, els.termsMsg, els.cuisineMsg, els.skillMsg]
        .forEach(el => setMsg(el))
}