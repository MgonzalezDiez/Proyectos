$(document).ready(function () {

    const btn = document.getElementById('btn');
    if (btn != null) {
        btn.addEventListener('click', validarNotas, true);
    }

    $("#decimales").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{1})$/, '$1.$2')
                    .replace(/\B(?=(\d{5})+(?!\d)\.?)/g, ",");
            });
        }
    });

    $(function () {
        $('#decimales').on('input', function () {
            var field = $(this);
            var car = $(this).val();
            if (car > 100) {
                let msg = document.querySelector('#mensaje');
                msg.innerHTML =
                    `<div class="alert alert-success alert-dismissible fade show" role="alert" id="alertInfo" >
                                <strong style="font-size:medium">El valor no puede ser mayor a 100%</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`;
                window.setTimeout(function () {
                    $("#alertInfo").alert('close');
                }, 4000);
                return
            }
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
                if (valor > 100) {
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
                    return
                }
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

    var frm = document.title;
    if (frm == "Preguntas Auto Evaluación") {
        /*Ingreso de notas de Autoevaluacion*/
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if (logros != "") {
            if ((logros != null && logros != "") && (metas != null && metas != "")) {
                codSec.disabled = false;
            }
            else {
                codSec.disabled = true;
            }
        }
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
        $('[data-toggle="tooltip"]').tooltip().show("slow");

    });

    function validarNotas() {
        notas = document.querySelectorAll('#nota');
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
        showLoader();
        for (i = 0; i < filas.length; i++) {
            var celdas = $(filas[i]).find("td");
            var numQuestion = $(celdas[0]).children("label")[0].innerText;
            var textQuestion = $(celdas[1]).children("label")[0].innerText;
            var nota = $($(celdas[2]).children("input")[0]).val();
            var sUrlPost = $("#urlPost").val();

            nota = nota.replace(/[^0-9]/g, '.');
            nota = parseFloat(nota).toFixed(1);
            nota = nota.replace(/[^0-9]/g, ',');
            $($(celdas[2]).children("input")[0]).text = nota;

            if (sUrlPost.includes("AutoEvaluation")) {
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
                }
            }
            else {
                var newData = {
                    Numero_Evaluacion: $('#numEval').val(),
                    Codigo_Proceso: $('#codProc').val(),
                    Codigo_Usuario_Evaluado: $('#codUser').val(),
                    Cod_Cargo_Evaluado: $("#codCargoEval").val(),
                    Codigo_Seccion: $('#codSec').val(),
                    Numero_Pregunta: numQuestion,
                    TextoPregunta: textQuestion,
                    Nota: nota
                }
            }

            $.ajax({
                type: "POST",
                url: sUrlPost,
                dataType: 'json',
                data: newData,
                success: function (data) {
                    let msg = document.querySelector('#mensaje');
                    if (data == "Ok") {
                        hideLoader();
                        $("#modalNotas").modal("hide");
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
                    }
                    else {
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
                        }, 8000);
                        return;
                    }
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
    }

    $('#logros').keyup(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
            return;
        }
        else {
            codSec.disabled = true;
            return;
        }
    });

    $('#metas').blur(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
            return;
        }
        else {
            codSec.disabled = true;
            return;
        }
    });

    $('#logros').change(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
            return;
        }
        else {
            codSec.disabled = true;
            return;
        }
    });
    $('#metas').change(function () {
        var logros = $('#logros').val();
        var metas = $('#metas').val();
        var codSec = document.getElementById('codSec');

        if ((logros != null && logros != "") && (metas != null && metas != "")) {
            codSec.disabled = false;
            return;
        }
        else {
            codSec.disabled = true;
            return;
        }
    });

    $('#codSec').change(function () {
        var cProc = $("#codProc").val();
        var section = $(this).children("option:selected").val();
        var nEval = $("#numEval").val();
        var Cod_Cargo_Evaluador = $("#Cod_Cargo_Evaluador").val();
        var Cod_Cargo_Evaluado = $("#codUserEval").val();
        var sUrlGet = $("#urlGet").val();

        if (section != "") {
            $.ajax({
                url: sUrlGet,
                type: 'POST',
                data: { codCargoEvaluador: Cod_Cargo_Evaluador, codCargoEvaluado: Cod_Cargo_Evaluado, codProc: cProc, numEval: nEval, codSecc: section },
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
                                        <td><label id = "textQuestion">${item.Texto_Pregunta}</td>
                                        <td><input id = "nota" type="number" class="form-control1", @required = "required", value = 0, style="width:60px; text-align:right;" maxlength = "3", min = "1.0", max = "7.0", step = "0.1"></td>
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
                    }
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
        }
        function showLoader() {
            $('#loading').show();
        };

        function hideLoader() {
            $('#loading').fadeOut();
        };
    });


    $("#codUserEval").change(function () {
        // Vacío EL DropDownList
        $("#codCargoEval").empty();
        $("#codSec").empty();
        $.ajax({
            type: 'POST',
            url: "/EvaluationPositions/GetPositionByUser",
            dataType: 'json',
            data: { codUsuario: $("#codUserEval").val() },
            success: function (positions) {
                $.each(positions, function (i, item) {
                    $("#codCargoEval").append("<option value='" + item.Codigo_Cargo + "'>" + item.Nombre_Cargo + "</option>");
                });
                $("#codCargoEval").change();
            },
            error: function (err) {
                {
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
                }
            }
        });
        return false;
    });

    $("#codCargoEval").change(function () {
        // Vacío EL DropDownList
        $("#codSec").empty();
        $.ajax({
            type: 'POST',
            url: "/EvaluationPositions/GetDomainByUser",
            dataType: 'json',
            data: { CodCargoEvaluador: $("#Cod_Cargo_Evaluador").val(), CodCargoEvaluado: $("#codUserEval").val() },
            success: function (domains) {
                $("#codSec").append("<option value>Seleccione Dominio </option>")
                $.each(domains, function (i, item) {
                    $("#codSec").append("<option value='" + item.Codigo_Seccion + "'>" + item.Nombre_Seccion + "</option>");
                });
            },
            error: function (err) {
                {
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
                }
            }
        });
        return false;
    });



})

