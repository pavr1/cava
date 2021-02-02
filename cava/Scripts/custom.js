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
});

function LoadBar() {
    $('.collapse').collapse('toggle');
    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $.get("/Home/Bar", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}
function LoadKitchen() {
    $('.collapse').collapse('toggle');
    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $.get("/Home/Kitchen", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}
function LoadExperience() {
    $('#main-container').fadeOut(500);
    $('.collapse').collapse('toggle');

    setTimeout(() => {
        $.get("Home/Experience", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}

function LoadReservation() {
    $('#main-container').fadeOut(500);
    $('.collapse').collapse('toggle');

    setTimeout(() => {
        $.get("Home/Reservation", {}, function (data) {
            $('#main-container').html(data);
            $('#main-container').fadeIn(800);
        });
    }, 500);
}