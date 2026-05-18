/* ============================================================
   manage-users.js

   Follows the exact same pattern as register.js:
     - els object, setMsg, requireCondition
     - Individual check functions
     - onSubmit orchestrator

   Additional:
     - Panel open/close for create and edit modes
     - Form population when editing a user
     - Delete confirmation panel
   ============================================================ */


/* ── 1. ELEMENT REFERENCES ───────────────────────────────────── */

const els = {
    username: document.getElementById('uname'),
    pwd: document.getElementById('pwd'),
    pwdValid: document.getElementById('pwd-valid'),
    bday: document.getElementById('bday'),
    skill: document.getElementById('skill'),

    usernameMsg: document.getElementById('uname-msg'),
    pwdMsg: document.getElementById('pwd-msg'),
    pwdValidMsg: document.getElementById('pwd-valid-msg'),
    bdayMsg: document.getElementById('bday-msg'),
    genderMsg: document.getElementById('gender-msg'),
    cuisineMsg: document.getElementById('cuisine-msg'),
    skillMsg: document.getElementById('skill-msg')
};

const formPanel = document.getElementById('form-panel');
const deletePanel = document.getElementById('delete-panel');
const formTitle = document.getElementById('form-panel-title');
const formSubtitle = document.getElementById('form-panel-subtitle');
const pwdLabel = document.getElementById('pwd-label');
const pwdVldLabel = document.getElementById('pwd-valid-label');

const hiddenId = document.getElementById(ManageUsersConfig.hiddenUserIdId);
const btnCreate = document.getElementById(ManageUsersConfig.btnCreateId);
const btnEdit = document.getElementById(ManageUsersConfig.btnEditId);
const btnDelete = document.getElementById(ManageUsersConfig.btnDeleteId);

let currentMode = 'create';   // 'create' | 'edit'
let pendingDeleteId = 0;


/* ── 2. UTILITIES (same as register.js) ──────────────────────── */

function setMsg(el, text) {
    if (!el) return !text;
    el.textContent = text || '';
    return !text;
}

function requireCondition(cond, el, msg) {
    if (cond) { setMsg(el); return true; }
    setMsg(el, msg);
    return false;
}

function hasLowerAndUpper(s) {
    return /[a-z]/.test(s) && /[A-Z]/.test(s);
}


/* ── 3. VALIDATORS (same pattern as register.js) ─────────────── */

function checkName() {
    const val = (els.username.value || '').trim();
    if (!requireCondition(val.length >= 6, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    return true;
}

function checkPassValidation() {
    // Edit mode with both fields blank = keep existing password = valid
    if (currentMode === 'edit' && !els.pwd.value && !els.pwdValid.value) {
        setMsg(els.pwdValidMsg);
        return true;
    }
    return requireCondition(
        els.pwd.value === els.pwdValid.value,
        els.pwdValidMsg,
        "Password doesn't match."
    );
}

function checkPass() {
    const val = els.pwd.value || '';

    // Edit mode blank password = keep existing = skip validation
    if (currentMode === 'edit' && val === '') {
        setMsg(els.pwdMsg);
        checkPassValidation();
        return true;
    }

    checkPassValidation();
    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    return true;
}

function isAtLeastAge(dateString, age) {
    if (!dateString) return false;
    const birth = new Date(dateString + 'T00:00:00');
    const today = new Date();
    let years = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) years--;
    return years >= age;
}

function checkBirthday() {
    const val = els.bday.value;
    if (!requireCondition(!!val, els.bdayMsg, 'Birthday required.')) return false;
    if (!requireCondition(isAtLeastAge(val, 12), els.bdayMsg, 'Must be at least 12.')) return false;
    return true;
}

function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    return requireCondition(!!selected, els.genderMsg, 'Select gender.');
}

function checkCuisine() {
    const count = document.querySelectorAll('input[name="cuisine"]:checked').length;
    return requireCondition(count > 0, els.cuisineMsg, 'Select at least one cuisine.');
}

function checkSkill() {
    return requireCondition(!!els.skill.value, els.skillMsg, 'Select skill level.');
}


/* ── 4. PANEL — OPEN / CLOSE ─────────────────────────────────── */

function openCreate() {
    currentMode = 'create';
    formTitle.textContent = 'Create User';
    formSubtitle.textContent = 'Fill in the details for the new user.';
    pwdLabel.classList.add('req');
    pwdVldLabel.classList.add('req');
    resetForm();
    formPanel.hidden = false;
}

function openEdit(id, username, birthday, gender, cuisine, skill) {
    currentMode = 'edit';
    formTitle.textContent = 'Edit User';
    formSubtitle.textContent = 'Leave password blank to keep the existing one.';
    pwdLabel.classList.remove('req');
    pwdVldLabel.classList.remove('req');
    resetForm();

    hiddenId.value = id;
    els.username.value = username;
    els.bday.value = birthday;
    els.skill.value = skill.toLowerCase();

    var genderRadio = document.querySelector('input[name="gender"][value="' + gender.toLowerCase() + '"]');
    if (genderRadio) genderRadio.checked = true;

    var cuisines = (cuisine || '').split(',').map(function (c) { return c.trim().toLowerCase(); });
    document.querySelectorAll('input[name="cuisine"]').forEach(function (cb) {
        cb.checked = cuisines.indexOf(cb.value) !== -1;
    });

    formPanel.hidden = false;
}

function closePanel() {
    formPanel.hidden = true;
    hiddenId.value = 0;
    resetForm();
}

formPanel.addEventListener('click', function (e) {
    if (e.target === formPanel) closePanel();
});


/* ── 5. RESET ────────────────────────────────────────────────── */

function resetForm() {
    els.username.value = '';
    els.pwd.value = '';
    els.pwdValid.value = '';
    els.bday.value = '';
    els.skill.value = '';
    document.querySelectorAll('input[name="gender"]').forEach(function (r) { r.checked = false; });
    document.querySelectorAll('input[name="cuisine"]').forEach(function (c) { c.checked = false; });
    Object.values(els).forEach(function (el) { setMsg(el); });
}


/* ── 6. SUBMIT ───────────────────────────────────────────────── */

function onSubmit() {
    var ok = checkName() & checkPass() & checkPassValidation()
        & checkBirthday() & checkGender() & checkCuisine() & checkSkill();

    if (!ok) return false;

    var cuisines = [];
    document.querySelectorAll('input[name="cuisine"]:checked').forEach(function (cb) {
        cuisines.push(cb.value);
    });

    var genderEl = document.querySelector('input[name="gender"]:checked');

    // Write values into hidden form fields for the codebehind to read via Request.Form
    writeHidden('mu-username', els.username.value.trim());
    writeHidden('mu-password', els.pwd.value);
    writeHidden('mu-birthday', els.bday.value);
    writeHidden('mu-gender', genderEl ? genderEl.value : '');
    writeHidden('mu-cuisine', cuisines.join(','));
    writeHidden('mu-skill', els.skill.value);
    writeHidden('mu-id', hiddenId.value);

    if (currentMode === 'create') {
        btnCreate.click();
    } else {
        btnEdit.click();
    }

    return false;
}

// Creates or updates a hidden input so the codebehind can read it via Request.Form
function writeHidden(name, value) {
    var el = document.querySelector('input[name="' + name + '"][type="hidden"]');
    if (!el) {
        el = document.createElement('input');
        el.type = 'hidden';
        el.name = name;
        document.forms[0].appendChild(el);
    }
    el.value = value;
}


/* ── 7. DELETE ───────────────────────────────────────────────── */

function confirmDelete(id, username) {
    pendingDeleteId = id;
    document.getElementById('delete-confirm-msg').textContent =
        'Are you sure you want to delete "' + username + '"? This cannot be undone.';
    deletePanel.hidden = false;
}

function submitDelete() {
    writeHidden('mu-id', pendingDeleteId);
    deletePanel.hidden = true;
    btnDelete.click();
}

function closeDeletePanel() {
    deletePanel.hidden = true;
    pendingDeleteId = 0;
}

deletePanel.addEventListener('click', function (e) {
    if (e.target === deletePanel) closeDeletePanel();
});