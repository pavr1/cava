var selectedReservationDate = new Date();

//$('#lbl-reservation-message').fadeOut(10);

//var logic = function (currentDateTime) {

//    var formattedDate = FormatDate(currentDateTime);
//    var formattedTime = FormatTime(currentDateTime);

//    $('#lbl-reservation-date').html(formattedDate);
//    $('#lbl-reservation-time').html(formattedTime);

//    selectedReservationDate = currentDateTime.toLocaleString('en-US');
//};

function FormatDate(currentDateTime) {
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

    return weekday[currentDateTime.getDay()].toUpperCase() + " " + currentDateTime.getDate() + " DE " + month[currentDateTime.getMonth()].toUpperCase();
}

function FormatTime(currentDateTime) {
    return currentDateTime.toLocaleString('es-CR', { hour: 'numeric', minute: 'numeric', hour12: true }).toUpperCase();
}

jQuery('.custom-date').datetimepicker({
    format: 'd/m/Y h:i a',
    onChangeDateTime: function (currentDateTime) {
        var formattedDate = FormatDate(currentDateTime);
        var formattedTime = FormatTime(currentDateTime);

        $('#lbl-reservation-date').html(formattedDate);
        $('#lbl-reservation-time').html(formattedTime);

        selectedReservationDate = currentDateTime.toLocaleString('en-US');
    },
    onShow: function (currentDateTime) {
        var formattedDate = FormatDate(currentDateTime);
        var formattedTime = FormatTime(currentDateTime);

        $('#lbl-reservation-date').html(formattedDate);
        $('#lbl-reservation-time').html(formattedTime);

        selectedReservationDate = currentDateTime.toLocaleString('en-US');
    },
    allowTimes: [
        '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30', '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '24:00'
    ],
    beforeShowDay: function (date) {
        if (date.getDay() == 1) { // 1= monday = monday closed
            return [false, "", "unAvailable"];
        } else {
            return [true, "", "Available"]; 
        }
        
    },
    disableDates: function (date) {
        return false;
    },
    formatDate: 'd/m/Y h:mm a',
    formatTime: 'h:i a',
    validateOnBlur: false,
    minDate: new Date()
});

setTimeout(function () {
    $('#whatsapp-link').notify(
        "¡CONTACTANOS!",
        {
            position: "left-top",
            className: "success"
        },
    );
}, 1000);

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
            {
                position: "top",
                className: "warn"
            },
        );

        $("#txt-people-amount").focus();
        $("#txt-people-amount").addClass("border-danger");
        return;
    } else if (reservationDateTime === "") {
        $("#txt-reservation-date").notify(
            "¡FECHA DE RESERVACIÓN REQUERIDA!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-reservation-date").focus();
        $("#txt-people-amount").removeClass("border-danger");
        $("#txt-reservation-date").addClass("border-danger");
        return;
    }

    $("#txt-people-amount").removeClass("border-danger");
    $("#txt-reservation-date").removeClass("border-danger");

    $('#reservation-modal').modal('toggle');
    $("#txt-name").focus();
});

$("#btn-cancel-modal-reservation").click(function () {
    ClearAndCloseModal();
});

$("#btn-confirm-modal-reservation").click(function () {
    var firstName = $("#txt-name").val();
    var lastName = $("#txt-last-names").val();
    var email = String($("#txt-email").val()).toLowerCase();
    var phone = $("#txt-phone").val();

    if (firstName === "") {
        $("#txt-name").notify(
            "¡NOMBRE REQUERIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-name").focus();
        $("#txt-name").removeClass("border-danger");

        return;
    }

    if (lastName === "") {
        $("#txt-last-names").notify(
            "¡APELLIDOS REQUERIDOS!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-last-names").focus();
        $("#txt-last-names").removeClass("border-danger");

        return;
    }

    if (email === "") {
        $("#txt-email").notify(
            "¡CORREO ELECTRÓNICO REQUERIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-email").focus();
        $("#txt-email").removeClass("border-danger");

        return;
    }

    var re = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;

    if (!re.test(email)) {
        $("#txt-email").notify(
            "¡CORREO ELECTRÓNICO INVÁLIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-email").focus();
        $("#txt-email").removeClass("border-danger");

        return;
    }

    if (phone === "") {
        $("#txt-phone").notify(
            "¡TELÉFONO REQUERIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-phone").focus();
        $("#txt-phone").removeClass("border-danger");

        return;
    }

    if (phone.length != 9) {
        $("#txt-phone").notify(
            "¡TELÉFONO INVÁLIDO!",
            {
                position: "top",
                className: "warn"
            }
        );

        $("#txt-phone").focus();
        $("#txt-phone").removeClass("border-danger");

        return;
    }

    CreateReservation();
});

function ClearAndCloseModal() {
    $("#txt-name").val("");
    $("#txt-last-names").val("");
    $("#txt-email").val("");
    $("#txt-phone").val("");

    $('#reservation-modal').modal('toggle');
}

function ClearReservationData() {
    $("#txt-people-amount").val("");
    $("#txt-reservation-date").val("");
}

function CreateReservation() {
    StartProcessing('btn-confirm-modal-reservation');
    DisableControl('btn-cancel-modal-reservation');

    //$('#btn-confirm-modal-reservation').html('<div class="d-inline-flex"><span class="spinner-grow spinner-grow-sm d-inline-block" role="status" aria-hidden="true"></span><span class="input-group-prepend ml-2">PROCESANDO...</span></div>');
    //$('#btn-confirm-modal-reservation').prop('disabled', true);
    //$('#btn-cancel-modal-reservation').prop('disabled', true);

    var amount = $("#txt-people-amount").val();
    var firstName = $("#txt-name").val();
    var lastName = $("#txt-last-names").val();
    var email = String($("#txt-email").val()).toLowerCase();
    var phone = $("#txt-phone").val();

    setTimeout(function () {
        $.post("/Home/CreateReservation", { reservationDate: selectedReservationDate, numberOfPeople: amount, reserverFirstName: firstName, reserverLastName: lastName, DOB: null, phone: phone, email: email },
            function (data) {
                var code = Number(data);

                if (code === 1) {
                    ClearReservationData();
                    ClearAndCloseModal();

                    $.notify("¡RESERVACIÓN CREADA!", "success");

                    $('#lbl-reservation-message').html('HAS HECHO UNA RESERVA PARA ' + amount + ' PERSONAS EL ' + FormatDate(new Date(selectedReservationDate)).toUpperCase() + ' A LAS ' + FormatTime(new Date(selectedReservationDate)));
                    $('#lbl-reservation-message').fadeIn(500);

                    setTimeout(function () {
                        $('#lbl-reservation-message').fadeOut(500);

                        setTimeout(function () {
                            $('#lbl-reservation-message').html("");
                        }, 500);
                    }, 8000);
                } else if (code === 2) {
                    $.notify("¡DATOS INVÁLIDOS!", "warn");
                } else if (code === -1) {
                    $.notify("¡ALGO SALIÓ MAL, INTENTA DE NUEVO!", "warn");
                }

                $('#btn-confirm-modal-reservation').html('CONFIRMAR RESERVACIÓN');

                EnableControl('btn-confirm-modal-reservation');
                EnableControl('btn-cancel-modal-reservation');
            });
    }, 1000);
}

function UpdateReservationModal(reservationId) {
    $('#reservation-modal').modal('toggle')
}