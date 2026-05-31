/* ============================================================
   manage-users.js — admin panel for creating, editing, and
   deleting users. Communicates with the codebehind by writing
   values into hidden form fields and triggering hidden ASP.NET
   buttons (btnCreate, btnEdit, btnDelete) via .click().

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

// Validates username: min 4 chars, no spaces, alphanumeric only
function checkName() {
    const val = (els.username.value || '').trim();
    if (!requireCondition(val.length >= 4, els.usernameMsg, 'Username too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.usernameMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9]/.test(val), els.usernameMsg, 'No special characters allowed.')) return false;
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
    if (!requireCondition(val.length >= 8, els.pwdMsg, 'Password too short.')) return false;
    if (!requireCondition(!/\s/.test(val), els.pwdMsg, 'No spaces allowed.')) return false;
    if (!requireCondition(hasLowerAndUpper(val), els.pwdMsg, 'Must include upper & lower case.')) return false;
    if (!requireCondition(!/[^a-zA-Z0-9!*@_$#]/.test(val), els.pwdMsg, "Password can't have special characters (other than !*@_$#).")) return false;

    if (els.bday.value) {
        const birthdayYear = new Date(els.bday.value).getFullYear();
        if (!requireCondition(!val.includes(birthdayYear), els.pwdMsg, "Password can't have the birthday year in it.")) return false;
    }

    if (els.username.value) {
        if (!requireCondition(!val.includes(els.username.value), els.usernameMsg, "Password can't have the username in it.")) return false;
    }

    return true;
}

function calculateAge(birthdate) {
    if (!birthdate) return 0;
    const today = new Date();
    const birth = new Date(birthdate);
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) age--;
    return age;
}

function ageInRange(birthdate, min, max) {
    const age = calculateAge(birthdate);
    return age >= min && age <= max;
}

// Validates birthday: required, age between 12 and 120
function checkBirthday() {
    const val = els.bday.value;
    if (!requireCondition(!!val, els.bdayMsg, 'Birthday required.')) return false;
    if (!requireCondition(ageInRange(val, 12, 120), els.bdayMsg, 'Age has to be above 12 and below 120.')) return false;
    return true;
}

// Validates that one gender radio is selected
function checkGender() {
    const selected = document.querySelector('input[name="gender"]:checked');
    return requireCondition(!!selected, els.genderMsg, 'Select gender.');
}

// Validates that at least one cuisine is checked
function checkCuisine() {
    const count = document.querySelectorAll('input[name="cuisine"]:checked').length;
    return requireCondition(count > 0, els.cuisineMsg, 'Select at least one cuisine.');
}

// Validates that a skill level is selected
function checkSkill() {
    return requireCondition(!!els.skill.value, els.skillMsg, 'Select skill level.');
}


/* ── 4. PANEL — OPEN / CLOSE ─────────────────────────────────── */

// Resets the form, labels it for creation, and shows the panel
function openCreate() {
    currentMode = 'create';
    formTitle.textContent = 'Create User';
    formSubtitle.textContent = 'Fill in the details for the new user.';
    pwdLabel.classList.add('req');
    pwdVldLabel.classList.add('req');
    resetForm();
    formPanel.hidden = false;
}

// Populates the form with the existing user's data and opens the panel in edit mode
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

// Hides the form panel and resets all fields and the hidden user ID
function closePanel() {
    formPanel.hidden = true;
    hiddenId.value = 0;
    resetForm();
}

formPanel.addEventListener('click', function (e) {
    if (e.target === formPanel) closePanel();
});


/* ── 5. RESET ────────────────────────────────────────────────── */

// Clears all form inputs and validation messages without touching select option lists
function resetForm() {
    els.username.value = '';
    els.pwd.value = '';
    els.pwdValid.value = '';
    els.bday.value = '';
    els.skill.value = '';
    document.querySelectorAll('input[name="gender"]').forEach(function (r) { r.checked = false; });
    document.querySelectorAll('input[name="cuisine"]').forEach(function (c) { c.checked = false; });

    // Only clear the message <div>s, not the input/select controls
    // (clearing the skill <select> would wipe its <option> list).
    [els.usernameMsg, els.pwdMsg, els.pwdValidMsg, els.bdayMsg,
     els.genderMsg, els.cuisineMsg, els.skillMsg]
        .forEach(function (el) { setMsg(el); });
}


/* ── 6. SUBMIT ───────────────────────────────────────────────── */

// Validates the form, writes values to hidden fields, then triggers the appropriate ASP.NET button
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

// Stores the pending delete target and shows the confirmation panel
function confirmDelete(id, username) {
    pendingDeleteId = id;
    document.getElementById('delete-confirm-msg').textContent =
        'Are you sure you want to delete "' + username + '"? This cannot be undone.';
    deletePanel.hidden = false;
}

// Writes the pending delete ID to the hidden field and triggers the delete button
function submitDelete() {
    writeHidden('mu-id', pendingDeleteId);
    deletePanel.hidden = true;
    btnDelete.click();
}

// Cancels the pending delete and hides the confirmation panel
function closeDeletePanel() {
    deletePanel.hidden = true;
    pendingDeleteId = 0;
}

deletePanel.addEventListener('click', function (e) {
    if (e.target === deletePanel) closeDeletePanel();
});