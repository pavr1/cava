jQuery.datetimepicker.setLocale('es-CR');

var weekday = new Array(7);
weekday[0] = "Domingo";
weekday[1] = "Lunes";
weekday[2] = "Martes";
weekday[3] = "Miércoles";
weekday[4] = "Jueves";
weekday[5] = "Viernes";
weekday[6] = "Sábado";

var month = new Array();
month[0] = "Enero";
month[1] = "Febrero";
month[2] = "Marzo";
month[3] = "Abríl";
month[4] = "Mayo";
month[5] = "Junio";
month[6] = "Julio";
month[7] = "Agosto";
month[8] = "Setiembre";
month[9] = "Octubre";
month[10] = "Noviembre";
month[11] = "Diciembre";

var logic = function (currentDateTime) {
    var formattedDate = weekday[currentDateTime.getDay()] + " " + currentDateTime.getDate() + " de " + month[currentDateTime.getMonth()];
    var formattedTime = currentDateTime.toLocaleString('es-CR', { hour: 'numeric', minute: 'numeric', hour12: true });

    $('#lbl-reservation-date').html(formattedDate);
    $('#lbl-reservation-time').html(formattedTime);
};

jQuery('.custom-date').datetimepicker({
    format: 'd/m/Y h:i a',
    onChangeDateTime: logic,
    onShow: logic,
    allowTimes: [
        '12:00', '13:00', '15:00',
        '17:00', '17:05', '17:20', '19:00', '20:00'
    ],
    formatDate: 'd/m/Y h:mm a',
    formatTime: 'h:i a',
    validateOnBlur: false,
    minDate: new Date()
});

$('#txt-phone').mask('0000-0000');

$(".reservation-people-number").keyup(function () {
    if ($(this).val() == "") {
        return;
    }

    var amount = Number($(this).val());

    if (amount < 1) {
        $(this).val(1);
    } else if (amount > 20) {
        $(this).val(20);
    }
});

$("input[type=text]").keyup(function () {
    $(this).val($(this).val().toUpperCase());
});

$("#btn-reservation-1").click(function () {
    var amount = $("#txt-people-amount").val();
    var reservationDateTime = $("#txt-reservation-date").val();

    if (amount === "") {
        $("#txt-people-amount").notify(
            "¡NÚMERO DE PERSONAS REQUERIDO!",
            { position: "top" },
        );

        $("#txt-people-amount").focus();
        $("#txt-people-amount").addClass("border-danger");
        return;
    } else if (reservationDateTime === "") {
        $("#txt-reservation-date").notify(
            "¡FECHA DE RESERVACIÓN REQUERIDA!",
            { position: "top" }
        );

        $("#txt-reservation-date").focus();
        $("#txt-people-amount").removeClass("border-danger");
        $("#txt-reservation-date").addClass("border-danger");
        return;
    }

    $("#txt-people-amount").removeClass("border-danger");
    $("#txt-reservation-date").removeClass("border-danger");

    $('#reservation-modal').modal('toggle')
    $("#txt-name").focus();
});

$("#btn-cancel-reservation").click(function () {
    $("#txt-name").val("");
    $("#txt-last-names").val("");
    $("#txt-email").val("");
    $("#txt-phone").val("");

    $('#reservation-modal').modal('toggle')
});

$("#btn-confirm-reservation").click(function () {
    var firstName = $("#txt-name").val();
    var lastName = $("#txt-last-names").val();
    var email = String($("#txt-email").val()).toLowerCase();
    var phone = $("#txt-phone").val();

    if (firstName === "") {
        $("#txt-name").notify(
            "¡NOMBRE REQUERIDO!",
            { position: "top" }
        );

        $("#txt-name").focus();
        $("#txt-name").removeClass("border-danger");

        return;
    }

    if (lastName === "") {
        $("#txt-last-names").notify(
            "¡APELLIDOS REQUERIDOS!",
            { position: "top" }
        );

        $("#txt-last-names").focus();
        $("#txt-last-names").removeClass("border-danger");

        return;
    }

    if (email === "") {
        $("#txt-email").notify(
            "¡CORREO ELECTRÓNICO REQUERIDO!",
            { position: "top" }
        );

        $("#txt-email").focus();
        $("#txt-email").removeClass("border-danger");

        return;
    }

    var re = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;

    if (!re.test(email)) {
        $("#txt-email").notify(
            "¡CORREO ELECTRÓNICO INVÁLIDO!",
            { position: "top" }
        );

        $("#txt-email").focus();
        $("#txt-email").removeClass("border-danger");

        return;
    }

    if (phone === "") {
        $("#txt-phone").notify(
            "¡TELÉFONO REQUERIDO!",
            { position: "top" }
        );

        $("#txt-phone").focus();
        $("#txt-phone").removeClass("border-danger");

        return;
    }

});