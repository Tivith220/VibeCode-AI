document.addEventListener("DOMContentLoaded", function () {

    // -------------------------
    // Intro Video
    // -------------------------

    const intro = document.getElementById("intro-screen");
    const video = document.getElementById("intro-video");
    const main = document.getElementById("main-content");

    if (video) {

        video.onended = function () {

            intro.style.transition = "opacity 1s";
            intro.style.opacity = "0";

            setTimeout(function () {
                intro.style.display = "none";
                main.style.display = "block";
            }, 1000);
        };

    } else {

        if (main)
            main.style.display = "block";
    }

    // -------------------------
    // AI Loading
    // -------------------------

    const form = document.querySelector("form");
    const button = document.querySelector("button[type='submit']");

    if (form && button) {

        form.addEventListener("submit", function () {

            button.disabled = true;

            button.innerHTML =
                "🤖 Analyzing Code... Please Wait";

        });

    }

});