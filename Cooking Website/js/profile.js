/* ============================================================
   profile.js — inline edit/view toggle and validation for the
   Profile page. No page reload on save; updated values are
   written back to the view elements directly.
   ============================================================ */


/* ── 1. UTILITIES ────────────────────────────────────────────── */

// Displays an error message for a field; toggles the 'visible' CSS class
function showError(id, msg) {
    const el = document.getElementById(id);
    if (!el) return;
    el.textContent = msg;
    el.classList.add('visible');
}

// Hides the error element for the given field id
function clearError(id) {
    const el = document.getElementById(id);
    if (el) el.classList.remove('visible');
}

// Hides all field-error elements on the page at once
function clearErrors() {
    document.querySelectorAll('.field-error').forEach(el => el.classList.remove('visible'));
}

// Password must be 8+ chars, no spaces, mixed case, allowed specials only
function isValidPassword(pw) {
    return pw.length >= 8 && !/\s/.test(pw) && /[A-Z]/.test(pw) && /[a-z]/.test(pw) && !/[^a-zA-Z0-9!*@_$#]/.test(pw);
}

// Uppercases the first character and lowercases the rest
function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
}


/* ── 2. MODE SWITCHING ───────────────────────────────────────── */

let editing = false;

/*
  Toggle all view/edit element pairs at once.
  Any element with id starting "view-" has a matching "edit-" counterpart.
  Both are selected in one pass rather than a manual list.
*/
function setMode(isEditing) {
    editing = isEditing;

    document.querySelectorAll('[id^="view-"]').forEach(function (viewEl) {
        const key = viewEl.id.replace('view-', '');
        const editEl = document.getElementById('edit-' + key);
        if (!editEl) return;
        viewEl.style.display = isEditing ? 'none' : '';
        editEl.style.display = isEditing ? '' : 'none';
    });

    // Footer, button label, server message
    document.getElementById('card-footer').classList.toggle('visible', isEditing);
    const btn = document.getElementById('toggle-btn');
    btn.textContent = isEditing ? 'Editing...' : 'Edit';
    btn.classList.toggle('active', isEditing);
    document.querySelector('.server-message').style.display = isEditing ? 'none' : '';

    if (!isEditing) {
        // Clear password fields when leaving edit mode
        ['edit-current-password', 'edit-new-password', 'edit-confirm-password']
            .forEach(id => { document.getElementById(id).value = ''; });
        clearErrors();
    }
}

function toggleEdit() { setMode(!editing); }
function cancelEdit() { setMode(false); }


/* ── 3. INDIVIDUAL VALIDATORS ────────────────────────────────── */

// Validates username: min 4 chars, no spaces, alphanumeric only
function validateUsername() {
    const val = document.getElementById('edit-username').value.trim();
    if (val.length < 4) return showError('error-username', 'Username must be at least 4 characters.'), false;
    if (/\s/.test(val)) return showError('error-username', 'Username cannot contain spaces.'), false;
    if (/[^a-zA-Z0-9]/.test(val)) return showError('error-username', "Username can't have special characters."), false;
    clearError('error-username');
    return true;
}

// Validates the password change flow; all three fields blank means "don't change password"
function validatePassword() {
    const current = document.getElementById('edit-current-password').value;
    const next = document.getElementById('edit-new-password').value;
    const confirm = document.getElementById('edit-confirm-password').value;

    // All blank = not changing password = valid
    if (!current && !next && !confirm) return true;

    let ok = true;
    if (!isValidPassword(current)) { showError('error-current-password', 'At least 8 chars, no spaces, upper and lowercase.'); ok = false; }
    else clearError('error-current-password');

    if (!next) { showError('error-new-password', 'Please enter a new password.'); ok = false; }
    else if (!isValidPassword(next)) { showError('error-new-password', 'At least 8 chars, no spaces, upper and lowercase.'); ok = false; }
    else if (next === current) { showError('error-new-password', 'New password must differ from current.'); ok = false; }
    else clearError('error-new-password');

    if (!confirm) { showError('error-confirm-password', 'Please confirm your new password.'); ok = false; }
    else if (confirm !== next) { showError('error-confirm-password', 'Passwords do not match.'); ok = false; }
    else clearError('error-confirm-password');

    return ok;
}

// Validates birthday: required, age must be between 12 and 120
function validateBirthday() {
    const val = document.getElementById('edit-birthday').value;
    if (!val) return showError('error-birthday', 'Please enter your birthday.'), false;
    const minAge = new Date();
    minAge.setFullYear(minAge.getFullYear() - 12);
    const maxAge = new Date();
    maxAge.setFullYear(maxAge.getFullYear() - 120)
    if (new Date(val) > minAge || new Date(val) < maxAge) return showError('error-birthday', 'Your age has to be between 12 and 120 years old.'), false;
    clearError('error-birthday');
    return true;
}

// Validates that a gender option is selected
function validateGender() {
    const val = document.getElementById('edit-gender').value;
    if (!val) return showError('error-gender', 'Please select a gender.'), false;
    clearError('error-gender');
    return true;
}

// Validates that at least one cuisine preference checkbox is checked
function validateCuisine() {
    const checked = document.querySelectorAll('#edit-cuisine input:checked').length;
    if (!checked) return showError('error-cuisine', 'Please select at least one cuisine.'), false;
    clearError('error-cuisine');
    return true;
}

// Validates that a skill level is selected
function validateSkill() {
    const val = document.getElementById('edit-skill').value;
    if (!val) return showError('error-skill', 'Please select a skill level.'), false;
    clearError('error-skill');
    return true;
}


/* ── 4. SAVE — validate then update the view ─────────────────── */

// Runs all validators; on success, updates view elements in place and returns true for form submit
function saveEdit() {
    clearErrors();

    // bitwise and to go through all functions regardless of their result
    const ok = validateUsername()
        & validatePassword()
        & validateBirthday()
        & validateGender()
        & validateCuisine()
        & validateSkill();

    if (!ok) return false;

    // Update visible view elements to reflect the new values
    const un = document.getElementById('edit-username').value.trim();
    const bd = document.getElementById('edit-birthday').value;
    const gender = document.getElementById('edit-gender').value;
    const skill = document.getElementById('edit-skill').value;
    const checked = [...document.querySelectorAll('#edit-cuisine input:checked')].map(cb => cb.value);
    const [y, m, d] = bd.split('-');

    document.getElementById('view-username').textContent = un;
    document.getElementById('header-username').textContent = '@' + un;
    document.getElementById('avatar-initials').textContent = un.slice(0, 2).toUpperCase();
    document.getElementById('view-birthday').textContent = `${d}/${m}/${y}`;
    document.getElementById('view-gender').textContent = capitalize(gender);
    document.getElementById('view-skill').textContent = capitalize(skill);
    document.getElementById('view-cuisine').innerHTML = checked.map(c => `<span class="multi-pill">${capitalize(c)}</span>`).join('');

    return true;
}


/* ── 5. LIVE VALIDATION — single delegated listener ─────────── */

/*
  One listener on the card body handles all field validation as the user
  types, instead of one addEventListener call per field.
*/
document.querySelector('.card-body').addEventListener('input', function (e) {
    const id = e.target.id;

    if (id === 'edit-username') validateUsername();
    if (id === 'edit-current-password' ||
        id === 'edit-new-password' ||
        id === 'edit-confirm-password') validatePassword();
    if (id === 'edit-birthday') validateBirthday();
});

document.querySelector('.card-body').addEventListener('change', function (e) {
    const id = e.target.id;

    if (id === 'edit-birthday') validateBirthday();
    if (id === 'edit-gender') validateGender();
    if (id === 'edit-skill') validateSkill();

    // Cuisine checkboxes — any checkbox inside #edit-cuisine
    if (e.target.closest('#edit-cuisine')) validateCuisine();
});