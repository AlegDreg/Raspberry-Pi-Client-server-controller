//$(".buttonpressed").mousedown(function (e) {
//    let d = e.currentTarget.outerText;

//    $.get("/action=" + d);
//});

var timeout, timeout1, clicker = $('.buttonpressed');

setInterval(function () {
    $.get("/check", function (data) {
        if (data == false) {
            RemoveClass()
        } else {
            AddClass()
        }
    });
}, 1000)

clicker.mousedown(function (e) {
    timeout = setInterval(function () {
        let d = e.currentTarget.outerText;

        $.get("/action=" + d);
    }, 100);

    return false;
});

function AddClass() {
    $("#galka").addClass("galka_ok");
}

function RemoveClass() {
    $("#galka").removeClass("#galka_ok");
}

$(document).mouseup(function () {
    clearInterval(timeout);
    return false;
});

$("#savebut").click(function () {
    let f1 = $("#f1").val()
    let f2 = $("#f2").val();
    let f3 = $("#f3").val();
    let f4 = $("#f4").val();
    let ms = $("#f5").val();

    $.get("/actionstrong=" + f1 + "&" + f2 + "&" + f3 + "&" + f4 + "&" + ms);
});

$(clicker).bind('touchstart', function (e) {
    timeout = setInterval(function () {
        let d = e.currentTarget.outerText;

        $.get("/action=" + d);
    }, 100);

    return false;
}).bind('touchend', function () {
    clearInterval(timeout);
    return false;
});

let d = ""
let pressed = false

timeout1 = setInterval(function () {
    if (pressed) {
        console.log(d)
        $.get("/action=" + d);
    }
}, 100);

$("#asd").keydown(function (event) {
    let a = event.keyCode
    if (a == 38) {
        d = "^"
    } else if (a == 37) {
        d = "<"
    } else if (a == 39) {
        d = ">"
    }
    else if (a == 40) {
        d = "."
    }

    pressed = true

    return false
});

$("#asd").keyup(function (event) {
    pressed = false
    return false;
});