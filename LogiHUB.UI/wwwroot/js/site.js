window.startTypingEffect = function () {
    const phrases = [
        "Real-time shipment tracking",
        "Automated invoicing",
        "Seamless customer management"
    ];

    let i = 0, j = 0;
    let isDeleting = false;
    const tagline = document.getElementById("animatedTagline");

    function type() {
        if (!tagline) return;
        if (!isDeleting) {
            tagline.textContent = phrases[i].substring(0, j + 1);
            j++;
            if (j === phrases[i].length) {
                isDeleting = true;
                setTimeout(type, 1000);
                return;
            }
        } else {
            tagline.textContent = phrases[i].substring(0, j - 1);
            j--;
            if (j === 0) {
                isDeleting = false;
                i = (i + 1) % phrases.length;
            }
        }
        setTimeout(type, isDeleting ? 50 : 150);
    }
    type();
};