$(document).ready(function ($) {
    var width = window.innerWidth;
    var height = windows.innerHeight;
    var ancho = $(window).width();
    var alto = $(window).height();

    if (ancho <= 1024) {
        lineas = 6
    } else if (ancho <= 1366) {
        lineas = 10
    } else if (ancho > 1366) {
        lineas = 18
    }
    return lineas;
});    