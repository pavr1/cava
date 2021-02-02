jQuery.datetimepicker.setLocale('es');

var logic = function (currentDateTime) {
    return;
    //// 'this' is jquery object datetimepicker
    //if (currentDateTime.getDay() == 6) {
    //    this.setOptions({
    //        minTime: '11:00'
    //    });
    //} else
    //    this.setOptions({
    //        minTime: '8:00'
    //    });
};
jQuery('.custom-date').datetimepicker({
    format: 'd/m/Y h:i a',
    onChangeDateTime: logic,
    onShow: logic,
    allowTimes: [
        '12:00', '13:00', '15:00',
        '17:00', '17:05', '17:20', '19:00', '20:00'
    ],
    formatDate: 'MM/dd/yyyy h:mm a',
    formatTime: 'h:i a',
    validateOnBlur: false,
});


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

$("#btn-reservation-1").click(function () {
    var amount = $("#txt-people-amount").val();
    var reservationDateTime = $("#txt-reservation-date").val();

    if (amount === "") {
        $.notify("Número de personas requerido", "warning");

        $("#txt-people-amount").focus();
        $("#txt-people-amount").addClass("border-danger");
        return;
    } else if (reservationDateTime === "") {
        $.notify("Fecha de reservación requerida", "warning");

        $("#txt-reservation-date").focus();
        $("#txt-people-amount").removeClass("border-danger");
        $("#txt-reservation-date").addClass("border-danger");
        return;
    }

    console.log("Str Date: " + navigator.language);
    $("#txt-people-amount").removeClass("border-danger");
    $("#txt-reservation-date").removeClass("border-danger");

    var dateObject = new Date(reservationDateTime);
    console.log("Date: " + new Date(reservationDateTime));

    //const formattedDate = dateObject.toLocaleDateString('en-GB', {
    //    day: 'numeric', month: 'short', year: 'numeric'
    //}).replace(/ /g, '-');
    //console.log(formattedDate);

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

    
    console.log(dateObject.getDay() + " : " + weekday[dateObject.getDay()]);
    console.log(dateObject.getMonth() + " : " + month[dateObject.getMonth()]);

    $('#lbl-reservation-date').html(weekday[dateObject.getDay()] + " " + dateObject.getDate() + " de " + month[dateObject.getMonth()]);

    $('#reservation-modal').modal('toggle')
});