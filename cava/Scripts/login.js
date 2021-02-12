$(document).ready(function () {
    //$('#btn-login').click(function () {
    //    ValidateLoginData();
    //});
});

function ValidateLoginData() {
    var email = $('#Email').val();
    var pwd = $('#Password').val();

    $("#Email").removeClass("border-danger");
    $("#Password").removeClass("border-danger");

    if (email === '') {
        $("#Email").notify(
            "¡CORREO ELECTRÓNICO REQUERIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#Email").focus();
        $("#Email").addClass("border-danger");

        return false;
    }

    $("#Email").removeClass("border-danger");

    if (pwd === '') {
       $("#Password").notify(
            "¡CONTRASEÑA REQUERIDA!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#Password").focus();
        $("#Password").addClass("border-danger");

        return false;
    }

    $("#Password").removeClass("border-danger");

    return true;
}