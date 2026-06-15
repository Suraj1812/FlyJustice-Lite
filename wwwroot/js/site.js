(function () {
    const toast = document.querySelector("[data-toast]");

    if (toast) {
        const close = toast.querySelector("[data-toast-close]");
        const dismiss = () => toast.remove();

        close?.addEventListener("click", dismiss);
        window.setTimeout(dismiss, 6000);
    }

    document.querySelectorAll("[data-loading-form]").forEach((form) => {
        form.addEventListener("submit", () => {
            const button = form.querySelector("[data-submit-button]");

            if (!button) {
                return;
            }

            button.disabled = true;
            button.textContent = button.getAttribute("data-loading-text") || "Working...";
        });
    });

    document.querySelectorAll("form[data-confirm]").forEach((form) => {
        form.addEventListener("submit", (event) => {
            const message = form.getAttribute("data-confirm") || "Are you sure?";

            if (!window.confirm(message)) {
                event.preventDefault();
            }
        });
    });
})();
