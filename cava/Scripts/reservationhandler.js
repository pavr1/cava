$(document).ready(function () {
    $('#filter-container').fadeOut(1);
    setTimeout(() => {
        $('#filter-container').removeAttr('hidden');
    }, 200);

    $('#filter-container').fadeIn(800);

    setTimeout(() => {
        GetReservations();
    }, 500);

    $('#menu-reservations').click(function () {
        GetReservations();
    });

    $('#btn-retrieve-reservations').click(function () {
        GetReservations();
    });

    $('#rbt-active').click(function () {
        $('#rbt-active').attr("checked", "checked");
        $('#rbt-canceled').removeAttr("checked");
        $('#rbt-confirmed').removeAttr("checked");

        GetReservations();

        $('#lbl-retrieve-tltie').html("RESERVACIONES ACTIVAS");
    });

    $('#rbt-canceled').click(function () {
        $('#rbt-canceled').attr("checked", "checked");
        $('#rbt-active').removeAttr("checked");
        $('#rbt-confirmed').removeAttr("checked");

        GetReservations();

        $('#lbl-retrieve-tltie').html("RESERVACIONES CANCELADAS");
    });

    $('#rbt-confirmed').click(function () {
        $('#rbt-confirmed').attr("checked", "checked");
        $('#rbt-active').removeAttr("checked");
        $('#rbt-canceled').removeAttr("checked");

        GetReservations();

        $('#lbl-retrieve-tltie').html("RESERVACIONES CONFIRMADAS");
    });

    $('#rdb-active').click(function () {
        $('#hdn-selected-status').val(1);
    });

    $('#rdb-canceled').click(function () {
        $('#hdn-selected-status').val(3);
    });

    $('#rdb-confirmed').click(function () {
        $('#hdn-selected-status').val(4);
    });
});

var selectedDate;
var logic = function (currentDateTime) {
    selectedDate = currentDateTime.toLocaleString('en-US');
};

$('.custom-date').datetimepicker({
    format: 'd/m/Y',
    onChangeDateTime: logic,
    onShow: logic,
    timepicker: false,
    allowTimes: [
        '12:00', '13:00', '15:00',
        '17:00', '17:05', '17:20', '19:00', '20:00'
    ],
    formatDate: 'd/m/Y',
    validateOnBlur: false,
    minDate: new Date()
});

function GetReservations() {
    var status = $('#hdn-selected-status').val();
    var name = $('#txt-reservation-name').val();
    var email = $('#txt-reservation-email').val();
    var phone = $('#txt-reservation-phone').val();
    var date = selectedDate;

    var status = status.trim();
    var name = name.trim();
    var email = email.trim();
    var phone = phone.trim();

    $('#div-reservation-handler-container').fadeOut(500);

    StartProcessing('btn-retrieve-reservations');
    DisableControl('rdb-active');
    DisableControl('rdb-canceled');
    DisableControl('rdb-confirmed');

    setTimeout(() => {
        $.get('/Home/RetrieveReservations', { status: status, name, email: email, phone: phone, date: date },
            function (data) {
                $('#div-reservation-handler-container').html(data);
                $('#div-reservation-handler-container').fadeIn(800);

                EnableControl('btn-retrieve-reservations');
                EnableControl('rdb-active');
                EnableControl('rdb-canceled');
                EnableControl('rdb-confirmed');
            });
    }, 500);
}

$('#txt-reservation-date').change(function () {
    var currentValue = $('#txt-reservation-date').val().trim();

    if (currentValue === "") {
        var today = new Date();
        var todayFormatted = FormatEsDate(today);

        $('#txt-reservation-date').val(todayFormatted);
    }
});

function FormatEsDate(date) {
    var day = date.getDate();
    var month = date.getMonth() + 1;

    if (day < 10) {
        day = '0' + day;
    }

    if (month < 10) {
        month = '0' + month;
    }

    return day + '/' + month + '/' + date.getFullYear();
}

function FormatEnDate(date) {
    var day = date.getDate();
    var month = date.getMonth() + 1;

    if (day < 10) {
        day = '0' + day;
    }

    if (month < 10) {
        month = '0' + month;
    }

    return month + '/' + day + '/' + date.getFullYear();
}