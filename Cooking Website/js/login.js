const uname = document.querySelector('#uname');
const pwd = document.querySelector('#pwd');
const unameMsg = document.querySelector('#uname-msg');
const pwdMsg = document.querySelector('#pwd-msg');

let nameOk = false;
let passOk = false;

function isLower(c) {
    let code = c.charCodeAt(0);
    return code >= 97 && code <= 122;
}

function isUpper(c) {
    let code = c.charCodeAt(0);
    return code >= 65 && code <= 90;
}

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

function onSubmit() {
    checkName();
    checkPass();

    return nameOk && passOk;
}

function onReset() {
    unameMsg.textContent = '';
    pwdMsg.textContent = '';
}
