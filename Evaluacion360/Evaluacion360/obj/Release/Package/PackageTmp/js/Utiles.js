$(document).ready(function () {

    $(function () {
        $('#decimales').on('input', function () {
            var field = $(this);
            var car = $(this).val();
            len = car.length;
            if (car.charCodeAt(len - 1) > 57) {
                car = car.replace(car.substring(len -1), "");
                this.value = car;
                return false;
            }
            var existePto = (/[.]/).test(field.val());
            var existeCom = (/[,]/).test(field.val());
            if (existePto === false || existeCom === false) {
                this.value = this.value.replace(/[^0-9]/g, ',');
                //this.value = Math.pow(this.value, 1);
            }
        });
    });

    

    $(function () {
        $('#mesProceso').blur('input', function () {
            var mes = $('#mesProceso').val();
            if (mes > 12 && mes < 1) {
                $("#myModal").modal();
            }
        });
    });

    $(function () {
        $('#decimales').blur(function () {
            var field = $(this);
            var valor;
            var existePto = (/[.]/).test(field.val());
            var existeCom = (/[,]/).test(field.val());
            if (existePto === false && existeCom === false) {
                valor = document.getElementById('decimales').value;
                valor = valor.replace(/[^0-9]/g, ',');
                document.getElementById('decimales').value = valor;
            }
            else {
                var valor = document.getElementById('decimales').value;
                valor = valor.replace(/[^0-9]/g, '.');
                valor = parseFloat(valor).toFixed(1);
                valor = valor.replace(/[^0-9]/g, ',');
                document.getElementById('decimales').value = valor;
            }
        })
    });

})

