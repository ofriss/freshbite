const fields = ['username', 'birthday', 'gender', 'skill'];
let editing = false;

// Clears all field errors on the page.
function clearErrors() {
    document.querySelectorAll('.field-error').forEach(el => el.classList.remove('visible'));
}

// Clears a single field error by ID.
function clearError(id) {
    document.getElementById(id).classList.remove('visible');
}

// Shows an error message for a specific field by ID.
function showError(id, message) {
    const el = document.getElementById(id);
    el.textContent = message;
    el.classList.add('visible');
}

// Returns true if the password meets all requirements: 8+ chars, no spaces, upper and lowercase.
function isValidPassword(pw) {
    return pw.length >= 8 &&
        !/\s/.test(pw) &&
        /[A-Z]/.test(pw) &&
        /[a-z]/.test(pw);
}

// Capitalizes the first letter of a string and lowercases the rest.
function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
}

// Toggles the form between read-only and edit mode.
function toggleEdit() {
    editing = !editing;

    fields.forEach(f => {
        document.getElementById('view-' + f).style.display = editing ? 'none' : 'flex';
        document.getElementById('edit-' + f).style.display = editing ? 'block' : 'none';
    });

    document.getElementById('view-cuisine').style.display = editing ? 'none' : 'flex';
    document.getElementById('edit-cuisine').style.display = editing ? 'flex' : 'none';

    document.getElementById('view-password').style.display = editing ? 'none' : 'flex';
    document.getElementById('edit-password-group').style.display = editing ? 'flex' : 'none';

    document.querySelector('.server-message').style.display = editing ? 'none' : 'block';

    clearErrors();
    document.getElementById('card-footer').classList.toggle('visible', editing);
    const btn = document.getElementById('toggle-btn');
    btn.textContent = editing ? 'Editing...' : 'Edit';
    btn.classList.toggle('active', editing);
}

// Closes edit mode and resets the form back to read-only state.
function cancelEdit() {
    editing = false;

    fields.forEach(f => {
        document.getElementById('view-' + f).style.display = 'flex';
        document.getElementById('edit-' + f).style.display = 'none';
    });

    document.getElementById('view-cuisine').style.display = 'flex';
    document.getElementById('edit-cuisine').style.display = 'none';
    document.getElementById('view-password').style.display = 'flex';
    document.getElementById('edit-password-group').style.display = 'none';

    document.getElementById('edit-current-password').value = '';
    document.getElementById('edit-new-password').value = '';
    document.getElementById('edit-confirm-password').value = '';

    document.querySelector('.server-message').style.display = 'none';

    clearErrors();
    document.getElementById('card-footer').classList.remove('visible');
    const btn = document.getElementById('toggle-btn');
    btn.textContent = 'Edit';
    btn.classList.remove('active');
}

// Validates, applies changes to the view, and returns true/false to control postback.
function saveEdit() {
    clearErrors();
    let valid = true;

    const un = document.getElementById('edit-username').value.trim();
    if (un.length < 4) {
        showError('error-username', 'Username must be at least 4 characters.');
        valid = false;
    } else if (/\s/.test(un)) {
        showError('error-username', 'Username cannot contain spaces.');
        valid = false;
    }

    const currentPw = document.getElementById('edit-current-password').value;
    const newPw = document.getElementById('edit-new-password').value;
    const confirmPw = document.getElementById('edit-confirm-password').value;

    if (currentPw) {
        if (!isValidPassword(currentPw)) {
            showError('error-current-password', 'Password must be at least 8 characters, no spaces, with upper and lowercase letters.');
            valid = false;
        }
        if (!newPw) {
            showError('error-new-password', 'Please enter a new password.');
            valid = false;
        } else if (!isValidPassword(newPw)) {
            showError('error-new-password', 'Password must be at least 8 characters, no spaces, with upper and lowercase letters.');
            valid = false;
        } else if (newPw === currentPw) {
            showError('error-new-password', 'New password must be different from current password.');
            valid = false;
        }
        if (!confirmPw) {
            showError('error-confirm-password', 'Please confirm your new password.');
            valid = false;
        } else if (newPw !== confirmPw) {
            showError('error-confirm-password', 'Passwords do not match.');
            valid = false;
        }
    }

    const bd = document.getElementById('edit-birthday').value;
    if (!bd) {
        showError('error-birthday', 'Please enter your birthday.');
        valid = false;
    } else {
        const bdDate = new Date(bd + 'T00:00:00');
        const minAge = new Date();
        minAge.setFullYear(minAge.getFullYear() - 12);
        if (bdDate > minAge) {
            showError('error-birthday', 'You must be at least 12 years old.');
            valid = false;
        }
    }

    const gender = document.getElementById('edit-gender').value;
    if (!gender) {
        showError('error-gender', 'Please select a gender.');
        valid = false;
    }

    const checked = [...document.querySelectorAll('#edit-cuisine input:checked')].map(cb => cb.value);
    if (checked.length === 0) {
        showError('error-cuisine', 'Please select at least one cuisine.');
        valid = false;
    }

    const skill = document.getElementById('edit-skill').value;
    if (!skill) {
        showError('error-skill', 'Please select a skill level.');
        valid = false;
    }

    if (!valid) return false;

    document.getElementById('view-username').textContent = un;
    document.getElementById('header-username').textContent = '@' + un;
    document.getElementById('avatar-initials').textContent = un.slice(0, 2).toUpperCase();

    const [year, month, day] = bd.split('-');
    document.getElementById('view-birthday').textContent = `${day}/${month}/${year}`;

    document.getElementById('view-gender').textContent = capitalize(gender);
    document.getElementById('view-skill').textContent = capitalize(skill);

    const pillsContainer = document.getElementById('view-cuisine');
    pillsContainer.innerHTML = checked.map(c => `<span class="multi-pill">${capitalize(c)}</span>`).join('');

    return true;
}

// Live validation — username format.
document.getElementById('edit-username').addEventListener('input', function () {
    const val = this.value.trim();
    if (val.length < 4) {
        showError('error-username', 'Username must be at least 4 characters.');
    } else if (/\s/.test(val)) {
        showError('error-username', 'Username cannot contain spaces.');
    } else {
        clearError('error-username');
    }
});

// Live validation — current password; clears entire group if emptied.
document.getElementById('edit-current-password').addEventListener('input', function () {
    if (!this.value) {
        clearError('error-current-password');
        clearError('error-new-password');
        clearError('error-confirm-password');
        return;
    }
    if (!isValidPassword(this.value)) {
        showError('error-current-password', 'Password must be at least 8 characters, no spaces, with upper and lowercase letters.');
    } else {
        clearError('error-current-password');
    }
});

// Live validation — new password; also re-evaluates confirm if already filled.
document.getElementById('edit-new-password').addEventListener('input', function () {
    const currentPw = document.getElementById('edit-current-password').value;
    if (!currentPw) return;

    if (!this.value) {
        showError('error-new-password', 'Please enter a new password.');
    } else if (!isValidPassword(this.value)) {
        showError('error-new-password', 'Password must be at least 8 characters, no spaces, with upper and lowercase letters.');
    } else if (this.value === currentPw) {
        showError('error-new-password', 'New password must be different from current password.');
    } else {
        clearError('error-new-password');
    }
    const confirm = document.getElementById('edit-confirm-password').value;
    if (confirm) {
        if (this.value !== confirm) {
            showError('error-confirm-password', 'Passwords do not match.');
        } else {
            clearError('error-confirm-password');
        }
    }
});

// Live validation — confirm password match.
document.getElementById('edit-confirm-password').addEventListener('input', function () {
    const currentPw = document.getElementById('edit-current-password').value;
    if (!currentPw) return;

    const newPw = document.getElementById('edit-new-password').value;
    if (!this.value) {
        showError('error-confirm-password', 'Please confirm your new password.');
    } else if (this.value !== newPw) {
        showError('error-confirm-password', 'Passwords do not match.');
    } else {
        clearError('error-confirm-password');
    }
});

// Live validation — birthday age check.
document.getElementById('edit-birthday').addEventListener('change', function () {
    const bd = new Date(this.value + 'T00:00:00');
    const minAge = new Date();
    minAge.setFullYear(minAge.getFullYear() - 12);
    if (!this.value) {
        showError('error-birthday', 'Please enter your birthday.');
    } else if (bd > minAge) {
        showError('error-birthday', 'You must be at least 12 years old.');
    } else {
        clearError('error-birthday');
    }
});

// Live validation — at least one cuisine checked.
document.getElementById('edit-cuisine').addEventListener('change', function () {
    const checked = [...this.querySelectorAll('input:checked')];
    if (checked.length > 0) {
        clearError('error-cuisine');
    } else {
        showError('error-cuisine', 'Please select at least one cuisine.');
    }
});