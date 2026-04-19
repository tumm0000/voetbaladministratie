// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Globale bescherming tegen het verliezen van niet-opgeslagen wijzigingen in formulieren.
(function () {
    const forms = Array.from(document.querySelectorAll("form"));
    if (forms.length === 0) return;

    let hasUnsavedChanges = false;
    let allowNavigation = false;

    const confirmationText = "Je hebt niet-opgeslagen wijzigingen. Weet je zeker dat je deze pagina wilt verlaten?";
    const editableSelector = "input:not([type='hidden']):not([type='submit']):not([type='button']):not([type='reset']), select, textarea";

    const markDirty = function (event) {
        const target = event.target;
        if (!(target instanceof HTMLElement)) return;
        if (!target.matches(editableSelector)) return;
        hasUnsavedChanges = true;
    };

    document.addEventListener("input", markDirty);
    document.addEventListener("change", markDirty);

    forms.forEach(function (form) {
        form.addEventListener("submit", function () {
            allowNavigation = true;
            hasUnsavedChanges = false;
        });
    });

    document.addEventListener("click", function (event) {
        const clicked = event.target;
        if (!(clicked instanceof Element)) return;

        const link = clicked.closest("a[href]");
        if (!link) return;

        const linkText = (link.textContent || "").trim().toLowerCase();
        const isCancelAction =
            link.hasAttribute("data-unsaved-allow-nav") ||
            linkText === "annuleren" ||
            linkText === "terug";

        if (isCancelAction) {
            allowNavigation = true;
            return;
        }

        if (!hasUnsavedChanges || allowNavigation) return;

        const confirmed = window.confirm(confirmationText);
        if (!confirmed) {
            event.preventDefault();
            event.stopPropagation();
            return;
        }

        allowNavigation = true;
    });

    window.addEventListener("beforeunload", function (event) {
        if (!hasUnsavedChanges || allowNavigation) return;
        event.preventDefault();
        event.returnValue = "";
    });
})();

// Laat succes/info/foutmeldingen automatisch verdwijnen na een paar seconden.
(function () {
    const alerts = Array.from(document.querySelectorAll(".alert"));
    if (alerts.length === 0) return;

    const visibleMs = 4000;
    const fadeMs = 400;

    alerts.forEach(function (alert) {
        window.setTimeout(function () {
            alert.style.transition = `opacity ${fadeMs}ms ease`;
            alert.style.opacity = "0";

            window.setTimeout(function () {
                alert.remove();
            }, fadeMs);
        }, visibleMs);
    });
})();
