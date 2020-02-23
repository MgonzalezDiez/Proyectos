$(document).ready(function () {

    $(function () {
        $('#decimales').on('input', function () {
            this.value = this.value.replace(/[^0-9]/g, ',');
        });
    });

    $(function () {
        $('#mesProceso').blur('input', function () {
            var mes = $('#mesProceso').val();
            if (mes > 12 && mes < 1) {
                $("#myModal").modal();
            }
        })
    })


})

