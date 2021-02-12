$(document).ready(function () {
    $('#btn-toggle-reservation').click(function () {
        $('#reservation-handler-modal').modal('toggle');
    });

    $('#btn-cancel-reservation').click(function () {
        var reservationId = $('#hdf-reservation-id').val();

        //3 = Canceled
        UpdateReservation(reservationId, 3, 'btn-confirm-reservation');
    });

    $('#btn-confirm-reservation').click(function () {
        var reservationId = $('#hdf-reservation-id').val();

        //4 = Confirmed
        UpdateReservation(reservationId, 4, 'btn-confirm-reservation');
    });
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
        if (!confirm("¡CONFIRMA CANCELACIÓN!")) {
            return;
        }
    } else if (statusId === 4) {
        if (!confirm("¡CONFIRMA VERIFICACIÓN!")) {
            return;
        }
    }

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
        });
}