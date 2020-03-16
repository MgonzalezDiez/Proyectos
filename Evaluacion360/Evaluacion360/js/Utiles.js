$(document).ready(function () {

    const btn = document.getElementById('btn');
    if (btn != null) {
        btn.addEventListener('click', validarNotas, true);
    }

    $(function () {
        $('#decimales').on('input', function () {
            var field = $(this);
            var car = $(this).val();
            len = car.length;
            if (car.charCodeAt(len - 1) > 57) {
                car = car.replace(car.substring(len - 1), "");
                this.value = car;
                return false;
            }
            var existePto = (/[.]/).test(field.val());
            var existeCom = (/[,]/).test(field.val());
            if (existePto === false || existeCom === false) {
                this.value = this.value.replace(/[^0-9]/g, ',');
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

    $(function () {
        $('#mesProceso').blur('input', function () {
            var mes = $('#mesProceso').val();
            if (mes > 12 && mes < 1) {
                $("#myModal").modal();
            }
        });
    });


    /*Ingreso de notas de Autoevaluacion*/
    var logros = $('#logros').val();
    var metas = $('#metas').val();
    var codSec = document.getElementById('codSec');

    if ((logros != null && logros != "") && (metas != null && metas != "")) {
        codSec.disabled = false;
    }
    else {
        codSec.disabled = true;
    }

    $(function () {
        $('#notas').on('input', function () {
            var field = $(this);
            var car = $(this).val();
            len = car.length;
            if (car.charCodeAt(len - 1) > 57) {
                car = car.replace(car.substring(len - 1), "");
                this.value = car;
                return false;
            }
            var existePto = (/[.]/).test(field.val());
            var existeCom = (/[,]/).test(field.val());
            if (existePto === false || existeCom === false) {
                this.value = this.value.replace(/[^0-9]/g, ',');
            }
        });
    });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });

    function validarNotas() {

        notas = document.querySelectorAll('#nota'); // input.form-control1');
        len = notas.length;
        valido = true;
        [].slice.call(notas, 0, len + 1).forEach(function (nota) {
            if (nota.value > 7 || nota.value < 1) {
                alert('La nota ' + nota.value + ' está fuera de rango');
                nota = (this).value
                valido = false
            }
        });
        if (valido) {
            GrabaNotas();

            return;
        } else {
            return;
        }
    };

    function GrabaNotas() {
        var filas = $('#preguntas').find("tr");
        for (i = 0; i < filas.length; i++) {
            var celdas = $(filas[i]).find("td");
            var numQuestion = $(celdas[0]).children("label")[0].innerText;
            var textQuestion = $(celdas[1]).children("label")[0].innerText;
            var nota = $($(celdas[2]).children("input")[0]).val();

            nota = nota.replace(/[^0-9]/g, '.');
            nota = parseFloat(nota).toFixed(1);
            nota = nota.replace(/[^0-9]/g, ',');
            $($(celdas[2]).children("input")[0]).text = nota;

            var newData = {
                Numero_Evaluacion: $('#numEval').val(),
                Codigo_Proceso: $('#codProc').val(),
                NombreProceso: $('#nomProc').val(),
                Codigo_Usuario: $('#codUser').val(),
                NombreUsuario: $('#nomUser').val(),
                Fecha: $('#fecha').val(),
                Logros: $('#logros').val(),
                Metas: $('#metas').val(),
                Estado_AE: $('#estadoAE').val(),
                StateDescription: $('#stateDesc').val(),
                Nota_Final_AE: $('#nFinAE').val(),
                Codigo_Seccion: $('#codSec').val(),
                Numero_Pregunta: numQuestion,
                TextoPregunta: textQuestion,
                Nota: nota
            };

            showLoader();
            $.ajax({
                type: "POST",
                url: "/AutoEvaluationQuestion/Edit",
                dataType: 'json',
                data: newData,
                success: function (data) {
                    hideLoader();
                    $("#modalNotas").modal("hide");
                    let msg = document.querySelector('#mensaje');
                    msg.innerHTML =
                        `<div class="alert alert-success alert-dismissible fade show" role="alert" id="alertInfo" >
                                <strong style="font-size:medium">Notas ingresadas al sistema de Evaluación</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`;
                    window.setTimeout(function () {
                        $("#alertInfo").alert('close');
                    }, 4000);
                },
                error: function () {
                    hideLoader();
                    $("#modalNotas").modal("hide");
                    let msg = document.querySelector('#mensaje');
                    msg.innerHTML =
                        `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="alertInfo" >
                                <strong style="font-size:medium">${Mensaje}</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`;
                    window.setTimeout(function () {
                        $("#alertInfo").alert('close');
                    }, 4000);
                    return;
                }
            })
        }
        hideLoader();
        $("#modalNotas").modal("hide");
        //modificarAutoEvaluaciones();
    }

    $('#logros').keyup(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
        }
        else {
            codSec.disabled = true;
        }
    });

    $('#metas').blur(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
        }
        else {
            codse.disabled = true;
        }
    });

    $('#codSec').change(function () {
        var section = $(this).children("option:selected").val();
        var nEval = $("#numEval").val();
        var cProc = $("#codProc").val();

        $.ajax({
            url: '/AutoEvaluationQuestion/GetQuestionByUser',
            type: 'POST',
            data: { numEval: nEval, codProc: cProc, codSecc: section },
            success: function (result) {
                if (result != null) {
                    let datos = JSON.parse(result);
                    for (let item of datos) {
                        if (item.Codigo_Seccion == "Error") {
                            let msg = document.querySelector('#mensaje');
                            msg.innerHTML =
                                `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="alertInfo" >
                                <strong style="font-size:medium">${item.Texto_Pregunta}</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`;
                            window.setTimeout(function () {
                                $("#alertInfo").alert('close');
                            }, 4000);
                            return;
                        }
                        else {
                            let res = document.querySelector('#preguntas');

                            res.innerHTML = '';
                            $.each(JSON.parse(result), function (i, item) {
                                res.innerHTML += `
                            <tr>
                                <td><label id = "numQuestion">${item.Numero_Pregunta}</td>
                                <td><label id = "textQuestion">${item.TextoPregunta}</td>
                                <td><input id='nota' type='number' class='form-control1' required ='required' value = 0 style='width:60px; text-align:right;' maxlength = 3, min = 1.0, max = 7.0, step = 0.1></td>
                            </tr>`
                            });
                        };
                    };
                    if (datos.length != 0) {
                        $("#modalNotas").modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                        $("#modalNotas").modal("show");

                    }
                };
            },
            error: function (err) {
                let msg = document.querySelector('#mensaje');
                msg.innerHTML =
                    `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="alertInfo" >
                            <strong style="font-size:medium">Algo salió mal.Por favor Contacte con el Administrador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`;
                window.setTimeout(function () {
                    $("#alertInfo").alert('close');
                }, 4000);
            },
        });
    });

    function modificarAutoEvaluaciones() {
        var myUrl = '/Tools.UpdateAutoEvaluation';
        var newData = {
            Numero_Evaluacion: $('#numEval').val(),
            Codigo_Proceso: $('#codProc').val(),
            Codigo_Usuario: $('#codUser').val(),
            Estado_AE: $('#estadoAE').val()
        }

        function actualiza() {
            $.ajax({
                type: "POST",
                url: myUrl,
                data: newData,
                dataType: "Json",
                success: function (result) {

                }
            })
        }
    }

    function showLoader() {
        $('#loading').show();
    };

    function hideLoader() {
        $('#loading').fadeOut();
    };

})

