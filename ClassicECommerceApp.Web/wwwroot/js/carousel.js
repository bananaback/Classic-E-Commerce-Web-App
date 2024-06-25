let slideIndex = 1;
let autoSlideTimeout;
let autoSlideDuration = 5000;

showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
        slides[i].classList.add("tw-hidden");
        slides[i].classList.remove("tw-block");
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].classList.remove("active");
    }
    slides[slideIndex - 1].style.display = "block";
    slides[slideIndex - 1].classList.remove("tw-hidden");
    slides[slideIndex - 1].classList.add("tw-block");
    dots[slideIndex - 1].classList.add("active");

    // Clear any existing timeout to avoid multiple intervals running simultaneously
    clearTimeout(autoSlideTimeout);
    autoSlideTimeout = setTimeout(() => plusSlides(1), autoSlideDuration); // Change image every 2 seconds
}

// Initial call to start the slideshow
autoSlideTimeout = setTimeout(() => plusSlides(1), autoSlideDuration);