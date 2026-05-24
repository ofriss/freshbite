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

function setMsg(el, text = '') {
    el.textContent = text;
}

function requireCondition(cond, el, msg) {
    if (cond) {
        setMsg(el);
        return true;
    }
    setMsg(el, msg);
    return false;
}

function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}

function checkName() {
    const val = els.username.value || '';
    if (!requireCondition(val.length >= 6, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(username), els.usernameMsg, 'No special characters allowed.')) return false;
    return true;
}

function checkPassValidation() {
    return requireCondition(els.pwd.value === els.pwdValid.value, els.pwdValidMsg, "Password doesn't match.");
}

function checkPass() {
    const val = els.pwd.value || '';

    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(username), els.usernameMsg, 'No special characters allowed.')) return false;

    if (els.bday.value) {
        const birthdayYear = new Date(els.bday.value).getFullYear();
        if (!requireCondition(!val.includes(birthdayYear), els.pwdMsg, "Password can't have your birthday in it.")) return false;
    }

    if (els.username.value) {
        if (!requireCondition(!val.includes(els.username.value), els.usernameMsg, "Password can't have your username in it.")) return false;
    }
    
    return true;
}

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

function ageInRange(birthdate, min, max) {
    const age = calculateAge(birthdate);
    return age >= min && age <= max;
}

function checkBirthday() {
    const val = els.bday.value;
    if (!requireCondition(!!val, els.bdayMsg, 'Birthday required.')) return false;
    if (!requireCondition(ageInRange(val, 12, 120), els.bdayMsg, 'Age has to be above 12 and below 120.')) return false;
    return true;
}

function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    return requireCondition(!!selected, els.genderMsg, 'Select gender.');
}

function checkTerms() {
    return requireCondition(els.terms.checked, els.termsMsg, 'You must agree.');
}

function checkCuisine() {
    const checkedCount = document.querySelectorAll('input[name="cuisine"]:checked').length;
    return requireCondition(checkedCount > 0, els.cuisineMsg, 'Select at least one cuisine.');
}

function checkSkill() {
    return requireCondition(!!els.skill.value, els.skillMsg, 'Select skill level.');
}

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

function onReset() {
    Object.values(els).forEach(el => setMsg(el))
}