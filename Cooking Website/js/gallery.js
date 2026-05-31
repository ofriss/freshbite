// gallery.js — simple image carousel with keyboard and button navigation

const images = document.querySelectorAll('.frame img');
const counter = document.getElementById('counter');
const caption = document.getElementById('caption');
// Captions are indexed to match the order images appear in the DOM
const captions = [
    "Fresh plated dishes",
    "Grilled chicken",
    "Seared steak",
    "Fried chicken",
    "Sizzling steak",
    "Beef burger",
    "Smash burger",
    "Pancakes",
    "BBQ ribs",
    "Fried chicken pieces",
    "Pan-fried beef",
    "Grilled tacos",
    "Roasted veggie bowl",
    "Beef ribs",
    "Herb chicken thighs",
];
let current = 0;

// Navigates to the given index (wraps around), updates counter and fades in caption
function go(index) {
    images[current].classList.remove('active');
    // Modulo wrap so index can go below 0 or above length safely
    current = (index + images.length) % images.length;
    images[current].classList.add('active');
    counter.textContent = String(current + 1).padStart(2, '0') + ' / ' + String(images.length).padStart(2, '0');
    caption.style.opacity = 0;
    // Brief fade-out delay before updating caption text for a smooth transition
    setTimeout(() => {
        caption.textContent = captions[current];
        caption.style.opacity = 1;
    }, 200);
}

document.getElementById('prev').addEventListener('click', () => go(current - 1));
document.getElementById('next').addEventListener('click', () => go(current + 1));

// Allow arrow key navigation as a convenience for keyboard users
document.addEventListener('keydown', e => {
    if (e.key === 'ArrowLeft') go(current - 1);
    if (e.key === 'ArrowRight') go(current + 1);
});

// Initialize
caption.textContent = captions[0];
counter.textContent = '01 / ' + String(images.length).padStart(2, '0')