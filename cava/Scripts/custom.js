$(document).ready(function () {
    setSpriteCss();

    $("#menu-bar-id").click(function () {
        LoadBar();
    });

    $("#menu-experience-id").click(function () {
        LoadExperience();
    });

    $("#menu-kitchen-id").click(function () {
        $('.collapse').collapse('toggle');

        setTimeout(function () {
            window.open('https://drive.google.com/file/d/1ns87LHZeXpkh0OO1s89g4-Lukl7lRK0_/view', '_blank');
        }, 500);
        //delete LoadKitchen();
    });

    $("#menu-reservation-id").click(function () {
        $('.collapse').collapse('toggle');

        ScrollTo('txt-people-amount');

        setTimeout(function () {
            $('#whatsapp-link').notify(
                "¡CONTÁCTANOS!",
                {
                    position: "left-top",
                    className: "success"
                },
            );
        }, 3000);

        //delete LoadReservation();
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

    $('#experience-carousel').carousel({
        interval: 4000,
        pause: false,
    });

    //$('#audio-player').get(0).play();

    var title;
    var subTitle;

    //This event fires immediately when the slide instance method is invoked.
    $('#experience-carousel').on('slide.bs.carousel', function (ev) {
        if (title) {
            title.fadeOut(100);
            subTitle.fadeOut(100);

            title.addClass("invisible");
            subTitle.addClass("invisible");
        }
    });

    //This event is fired when the carousel has completed its slide transition.
    $('#experience-carousel').on('slid.bs.carousel', function () {
        var activeElement = $("div.carousel-item.active");
        title = activeElement.find(".slide-title");
        subTitle = activeElement.find(".slide-sub-title");

        title.fadeOut(1);
        subTitle.fadeOut(1);

        setTimeout(() => {
            title.removeClass("invisible");
            subTitle.removeClass("invisible");

            title.fadeIn(1000);

            setTimeout(() => {
                subTitle.fadeIn(1000);
            }, 1000);
        }, 1000);

    });
});

function setSpriteCss() {
    var screenWidth = $(window).width();

    if (screenWidth >= 320 && screenWidth <= 426) {
        $("#css-sprite-link").attr("href", "../Content/v2.0/images/slides/sprites/sm-10/mysprite.sprite.css");

        console.log("Sprite set sm");
    } else if (screenWidth >= 427 && screenWidth <= 768) {
        $("#css-sprite-link").attr("href", "../Content/v2.0/images/slides/sprites/md-20/mysprite.sprite.css");

        console.log("Sprite set md");
    } else if (screenWidth >= 769 && screenWidth <= 1024) {
        $("#css-sprite-link").attr("href", "../Content/v2.0/images/slides/sprites/lg-30/mysprite.sprite.css");

        console.log("Sprite set lg");
    } else if (screenWidth >= 1025) {
        $("#css-sprite-link").attr("href", "../Content/v2.0/images/slides/sprites/xl-40/mysprite.sprite.css");

        console.log("Sprite set xl");
    }

    console.log($("#css-sprite-link").attr("href"));
}

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

function ScrollTo(controlId) {
    var control = $('#' + controlId);

    $.scrollTo(control.offset().top - 500, 2000);
}

function LoadBar() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block processing-icon" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);
        $('#main-container').scrollTop = 0;

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("/Home/Bar", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 1200);
    }, 500);
}
function LoadKitchen() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block processing-icon" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);
        $('#main-container').scrollTop = 0;

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("/Home/Kitchen", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 1200);
    }, 500);
}

function LoadExperience() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block processing-icon" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);
        $('#main-container').scrollTop = 0;

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("Home/Experience", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 1200);
    }, 500);
}

function LoadReservation() {
    if (ShouldItCollapse()) {
        $('.collapse').collapse('toggle');
    }

    $('#main-container').fadeOut(500);

    setTimeout(() => {
        $('#main-container').html('<div class="w-100 text-center no-flex"><span class="spinner-grow spinner-grow-lg d-inline-block processing-icon" style="width: 4rem; height: 4rem;" role="status" aria-hidden="true"></div>');
        $('#main-container').fadeIn(500);
        $('#main-container').scrollTop = 0;

        setTimeout(() => {
            $('#main-container').fadeOut(100);

            setTimeout(() => {
                $.get("./Home/Reservation", {}, function (data) {
                    $('#main-container').html(data);
                    $('#main-container').fadeIn(800);
                });
            }, 500);
        }, 1200);
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