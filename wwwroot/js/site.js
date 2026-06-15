(function () {
    const header = document.querySelector("[data-site-header]");
    const navToggle = document.querySelector("[data-nav-toggle]");
    const navMenu = document.querySelector("[data-nav-menu]");

    const closeNavigation = () => {
        if (!navToggle || !navMenu) {
            return;
        }

        navToggle.setAttribute("aria-expanded", "false");
        navToggle.querySelector(".sr-only").textContent = "Open navigation";
        navMenu.classList.remove("is-open");
        document.body.classList.remove("nav-open");
    };

    navToggle?.addEventListener("click", () => {
        const isOpen = navToggle.getAttribute("aria-expanded") === "true";
        navToggle.setAttribute("aria-expanded", String(!isOpen));
        navToggle.querySelector(".sr-only").textContent = isOpen ? "Open navigation" : "Close navigation";
        navMenu?.classList.toggle("is-open", !isOpen);
        document.body.classList.toggle("nav-open", !isOpen);
    });

    navMenu?.querySelectorAll("a").forEach((link) => {
        link.addEventListener("click", closeNavigation);
    });

    window.addEventListener("resize", () => {
        if (window.innerWidth >= 820) {
            closeNavigation();
        }
    });

    window.addEventListener("keydown", (event) => {
        if (event.key === "Escape") {
            closeNavigation();
        }
    });

    const updateHeader = () => {
        header?.classList.toggle("is-scrolled", window.scrollY > 12);
    };

    updateHeader();
    window.addEventListener("scroll", updateHeader, { passive: true });

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

            if (!button || !form.checkValidity()) {
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

    document.querySelectorAll("[data-file-input]").forEach((input) => {
        input.addEventListener("change", () => {
            const output = document.querySelector(input.getAttribute("data-file-output"));

            if (output) {
                output.textContent = input.files?.[0]?.name || "No file selected";
            }
        });
    });

    document.querySelectorAll("[data-accordion]").forEach((accordion) => {
        accordion.querySelectorAll("details").forEach((item) => {
            item.addEventListener("toggle", () => {
                if (!item.open) {
                    return;
                }

                accordion.querySelectorAll("details[open]").forEach((openItem) => {
                    if (openItem !== item) {
                        openItem.open = false;
                    }
                });
            });
        });
    });
})();
