$(document).ready(function () {
    $('#btn-toggle-reservation').click(function () {
        $('#reservation-handler-modal').modal('toggle');
    });

    $('#btn-cancel-reservation').click(function () {
        var reservationId = $('#hdf-reservation-id').val();

        //3 = Canceled
        UpdateReservation(reservationId, 3, 'btn-cancel-reservation');
    });

    $('#btn-confirm-reservation').click(function () {
        var reservationId = $('#hdf-reservation-id').val();

        //4 = Confirmed
        UpdateReservation(reservationId, 4, 'btn-confirm-reservation');
    });
});

//add a new style 'foo'
$.notify.addStyle('foo', {
    html:
        "<div>" +
        "<div class='clearfix'>" +
        "<div class='title' data-notify-html='title'></div>" +
        "<div class='buttons'>" +
        "<button class='btn btn-danger no'>SALIR</button>" +
        "<button class='btn btn-info yes' data-notify-text='button'></button>" +
        "</div>" +
        "</div>" +
        "</div>"
});

//listen for click events from this style
$(document).on('click', '.notifyjs-foo-base .no', function () {
    //programmatically trigger propogating hide event
    $(this).trigger('notify-hide');
});
$(document).on('click', '.notifyjs-foo-base .yes', function () {
    //show button text
    alert($(this).text() + " clicked!");
    //hide notification
    $(this).trigger('notify-hide');
});
$(document).on('triggered', '.notifyjs-foo-base', function () {
    //show button text
    alert($(this).text() + " triggered!");
});

function UpdateReservationModal(reservationId, date, time, name, lastName, email, phone, amount) {
    $('#hdf-reservation-id').val(reservationId);
    $('#lbl-reservation-date').html(date);
    $('#lbl-reservation-time').html(time);
    $('#lbl-reservation-amount').html(amount);

    $('#txt-name').val(name);
    $('#txt-last-names').val(lastName);
    $('#txt-email').val(email);
    $('#txt-phone').val(phone);

    $('#reservation-handler-modal').modal('toggle');
}

function UpdateReservation(reservationId, statusId, ctrlid) {
    if (statusId === 3) {
        $('#btn-cancel-reservation').notify({
            title: '¡CONFIRMA ANTES DE PROCEDER!',
            button: 'CANCELAR',
            position: "top-center"
        }, {
            style: 'foo',
            autoHide: true,
            clickToHide: true
        });
        return;
        //if (!confirm("")) {
        //    return;
        //}
    } else if (statusId === 4) {
        $('#btn-cancel-reservation').notify({
            title: '¡CONFIRMA ANTES DE PROCEDER!',
            button: 'VERIFICAR',
            position: "top-center"
        }, {
            style: 'foo',
            autoHide: true,
            clickToHide: true
        });

        return;
    }

    StartProcessing(ctrlid);
    DisableControl('rdb-active');
    DisableControl('rdb-canceled');
    DisableControl('rdb-confirmed');
    DisableControl('btn-retrieve-reservations');

    $.post('/Home/UpdateReservationStatus', { reservationId: reservationId, status: statusId },
        function (data) {
            var code = Number(data);

            if (code === -1) {
                $.notify("¡HUBO UN ERROR AL ENCONTRAR REGISTRO!", "error");
            } else if (code === -2) {
                $.notify("¡ESTADO INCORRECTO!", "error");
            } else if (code === -3) {
                $.notify("¡HUBO UN ERROR!", "error");
            } else if (code === 1) {
                var status = (statusId === 4) ? "CONFIRMADA" : "CANCELADA";

                $.notify("¡RESERVACIÓN " + status + "!", "success");

                $('#reservation-handler-modal').modal('toggle');

                GetReservations();
            }

            $('#reservation-handler-modal').modal('toggle');

            EnableControl('rdb-active');
            EnableControl('rdb-canceled');
            EnableControl('rdb-confirmed');
            EnableControl('btn-retrieve-reservations');
        });
}