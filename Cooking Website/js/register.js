/* Form controls and message elements */
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

/* Utility: set message and return whether there's no error */
function setMsg(el, text = '') {
    el.textContent = text;
    return text === '';
}

/* Utility: require condition, set message on failure */
function requireCondition(cond, el, msg) {
    if (cond) {
        setMsg(el);
        return true;
    }
    setMsg(el, msg);
    return false;
}

/* Check presence of both lowercase and uppercase */
function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}

/* Username validation */
function checkName() {
    const val = (els.username.value || '').trim();
    if (!requireCondition(val.length >= 6, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    return true;
}

/* Password confirmation */
function checkPassValidation() {
    return requireCondition(els.pwd.value === els.pwdValid.value, els.pwdValidMsg, "Password doesn't match.");
}

/* Password validation */
function checkPass() {
    const val = els.pwd.value || '';
    // Show confirmation mismatch early
    checkPassValidation();

    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    return true;
}

/* Age helper */
function isAtLeastAge(dateString, age) {
    if (!dateString) return false;
    const birth = new Date(dateString + 'T00:00:00');
    const today = new Date();
    let years = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) years--;
    return years >= age;
}

/* Birthday check */
function checkBirthday() {
    const val = els.bday.value;
    if (!requireCondition(!!val, els.bdayMsg, 'Birthday required.')) return false;
    if (!requireCondition(isAtLeastAge(val, 12), els.bdayMsg, 'Must be at least 12.')) return false;
    return true;
}

/* Gender radio check */
function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    return requireCondition(!!selected, els.genderMsg, 'Select gender.');
}

/* Terms checkbox */
function checkTerms() {
    return requireCondition(els.terms.checked, els.termsMsg, 'You must agree.');
}

/* Cuisine checkbox group */
function checkCuisine() {
    const checkedCount = document.querySelectorAll('input[name="cuisine"]:checked').length;
    return requireCondition(checkedCount > 0, els.cuisineMsg, 'Select at least one cuisine.');
}

/* Skill select */
function checkSkill() {
    return requireCondition(!!els.skill.value, els.skillMsg, 'Select skill level.');
}

/* Orchestrate validations for submission */
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

/* Clear all validation messages (for form reset) */
function onReset() {
    Object.values(els).forEach(el => setMsg(el))
}