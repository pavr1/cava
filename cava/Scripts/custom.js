$(document).ready(function () {
    $("#menu-bar-id").click(function () {
        LoadBar();
    });

    $("#menu-experience-id").click(function () {
        LoadExperience();
    });

    $("#menu-kitchen-id").click(function () {
        LoadKitchen();
    });

    $("#menu-reservation-id").click(function () {
        LoadReservation();
    });

    $("#menu-login").click(function () {
        LoadLogin();
    });

    $("#lbl-login").click(function () {
        LoadLogin();
    });

    $("input[type=text]").keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    $(window).bind("resize", function () {
        return false;
    });
});

function ShouldItCollapse() {
    var width = $(window).width();

    //xs = < 576;
    //sm = 576 - 767;
    //md = 768 - 991;
    //lg = 992 - 1190;
    //xl = > 1200

    //if with is less lg or xl (md/sm/xs)
    return (width < 992);
}

function LoadBar() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("/Home/Bar", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 2000);
    }, 500);
}
function LoadKitchen() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);
    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("/Home/Kitchen", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 2000);
    }, 500);
}

function LoadExperience() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("Home/Experience", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 2000);
    }, 500);
}

function LoadReservation() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("Home/Reservation", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 2000);
    }, 500);
}

function LoadLogin() {
    $('#div-reservation-handler-login-container').fadeOut(500);

    setTimeout(() => {
        $.get("/Home/Login", {}, function (data) {
            $('#div-reservation-handler-login-container').html(data);
            $('#div-reservation-handler-login-container').fadeIn(800);
        });
    }, 500);
}

function StartProcessing(button) {
    $('#' + button).html('<div class="d-inline-flex"><span class="spinner-grow spinner-grow-sm d-inline-block" role="status" aria-hidden="true"></span><span class="input-group-prepend ml-2">PROCESANDO...</span></div>');

    DisableControl(button);
}

function DisableControl(control) {
    $('#' + control).prop('disabled', true);
}

function EnableControl(control) {
    $('#' + control).prop('disabled', false);
}