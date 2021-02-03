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

    SetCollapsableBehavior();
});

function ShouldItCollaps() {
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
    if (ShouldItCollaps()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $.get("/Home/Bar", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}
function LoadKitchen() {
    if (ShouldItCollaps()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $.get("/Home/Kitchen", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}
function LoadExperience() {
    if (ShouldItCollaps()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);
    

    setTimeout(() => {
        $.get("Home/Experience", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}

function LoadReservation() {
    if (ShouldItCollaps()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $.get("Home/Reservation", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}