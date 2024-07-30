function toggleTheme() {
    var body = document.body;
    body.classList.toggle("dark-mode");

    // Save preference to local storage (optional)
    if (body.classList.contains("dark-mode")) {
        localStorage.setItem("theme", "dark");
    } else {
        localStorage.setItem("theme", "light");
    }
}

// Load preference from local storage on page load (optional)
document.addEventListener("DOMContentLoaded", () => {
    if (localStorage.getItem("theme") === "dark") {
        document.body.classList.add("dark-mode");
        document.getElementById("themeToggle").checked = true;
    }
});
