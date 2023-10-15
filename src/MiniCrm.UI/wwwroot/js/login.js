$(document).ready(function (e) {
    const login = $(".login-form");
    const errorBlock = $("#errorBlock");
    const errorText = $(".error-text");

    login.submit(function (e) {
        e.preventDefault();

        if (login.valid()) {
            $.ajax({
                type: "POST",
                url: "/login",
                data: login.serialize(),
                success: function (response) {
                    window.location.replace("/Home");
                },
                error: function (xhr, status, error) {
                    errorBlock.show();
                    errorText.text(error);
                },
            });
        }
    });
});