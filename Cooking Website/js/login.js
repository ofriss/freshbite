const uname = document.querySelector('#uname');
const pwd = document.querySelector('#pwd');
const unameMsg = document.querySelector('#uname-msg');
const pwdMsg = document.querySelector('#pwd-msg');

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

function checkName() {
    const val = uname.value || '';

    if (!requireCondition(val.length >= 6, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(username), els.usernameMsg, 'No special characters allowed.')) return false;
    
    return true;
}


function checkPass() {
    const val = (pwd.value);

    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(username), els.usernameMsg, 'No special characters allowed.')) return false;

    return true;
}

function onSubmit() {
    const nameOk = checkName();
    const passOk = checkPass();

    return nameOk && passOk;
}

function onReset() {
    setMsg(unameMsg)
    setMsg(pwdMsg)
}
