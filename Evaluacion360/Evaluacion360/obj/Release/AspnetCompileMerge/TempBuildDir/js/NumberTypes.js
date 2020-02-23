$(function () {
    $('#decimales').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, ',');
    });
});

$(function () {
    $('#enteros').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
});