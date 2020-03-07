$(document).on('click', '#btnUpload', function () {
    if (window.FormData !== undefined) {
        var fileUpload = $("#file").get(0);
        if ($("#file").get(0).files.length == 0) {
            const msg = document.querySelector('#mensaje');
            msg.innerHTML =
                `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                <strong style="font-size:medium">${item.Nombre_Seccion}</strong>
                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>`
            return;
        }
        showLoader();
        var files = "";
        files = fileUpload.files;
        var fileData = new FormData();
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.ajax({
            url: '/Excel/Index',
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {
                showLoader();
                if (result != null || result != false) {
                    $.each(JSON.parse(result), function (i, item) {
                        if (item.Codigo_Seccion == "Error") {
                            let msg = document.querySelector('#mensaje');
                            msg.innerHTML =
                                `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                                <strong style="font-size:medium">${item.Nombre_Seccion}</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`
                            return;
                        }
                        else {
                            let thead = document.querySelector('thead');
                            thead.innerHTML = '';
                            thead.innerHTML = `
                            <tr >
                                <th>Codigo Dominio</th>
                                <th>Nombre Dominio</th>
                                <th>Ponderación</th>
                                <th>Estado</th>
                            </tr >`

                            let res = document.querySelector('#res');
                            res.innerHTML = '';
                            $.each(JSON.parse(result), function (i, item) {
                                res.innerHTML += `
                        <tr>
                            <td>${item.Codigo_Seccion}</td>
                            <td>${item.Nombre_Seccion}</td>
                            <td>${item.Ponderacion_S}</td>
                            <td>${item.IdState == 1 ? 'Vigente' : 'No vigente'}</td>
                        </tr>`
                            });
                            let res2 = document.querySelector('#titulo');
                            res2.innerHTML = `Dominios Importados desde Excel`;
                        }
                    });
                }
                else {
                    let msg = document.querySelector('#mensaje');
                    msg.innerHTML =
                        `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">Algo salió mal.Por favor Contacte con el Administrador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`
                    return;
                }
            },
            error: function (err) {
                hideLoader();
                let msg = document.querySelector('#mensaje');
                msg.innerHTML =
                    `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">Algo salió mal.Por favor Contacte con el Administrador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`
            },
        });
    } else {
        let msg = document.querySelector('#mensaje');
        msg.innerHTML =
            `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">FormData No es compatible con el Navegador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`;
    }

    function showLoader() {
        $('#loading').show();
    }
    function hideLoader() {
        $('#loading').fadeOut();
    }
    $(document).ready(function () {
        hideLoader();
    })
});

$(document).on('click', '#btnExcelRQ', function () {
    if (window.FormData !== undefined) {
        var fileUpload = $("#file").get(0);
        if ($("#file").get(0).files.length == 0) {
            const msg = document.querySelector('#mensaje');
            msg.innerHTML =
                `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                <strong style="font-size:medium">${itemRQ.Texto_Pregunta}</strong>
                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>`
            return;
        }
        showLoader();
        var files = "";
        files = fileUpload.files;
        var fileData = new FormData();
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.ajax({
            url: '/Excel/IndexRQ',
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {
                showLoader();
                if (result != null || result != false) {
                    let thead = document.querySelector('thead');
                    thead.innerHTML = '';
                    thead.innerHTML = `
                            <tr >
                                <th>Codigo Dominio</th>
                                <th>Número de Pregunta</th>
                                <th>Texto de Pregunta</th>
                                <th>Ponderación</th>
                            </tr >`
                    let res = document.querySelector('#res');
                    res.innerHTML = '';
                    $.each(JSON.parse(result), function (i, itemRQ) {
                        if (itemRQ.Codigo_Seccion == "Error") {
                            let msg = document.querySelector('#mensaje');
                            msg.innerHTML =
                                `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                                <strong style="font-size:medium">${itemRQ.Texto_Pregunta}</strong>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                </div>`
                            return;
                        }
                        else {
                            res.innerHTML += `
                            <tr>
                                <td>${itemRQ.Codigo_Seccion}</td>
                                <td>${itemRQ.Numero_Pregunta}</td>
                                <td>${itemRQ.Texto_Pregunta}</td>
                                <td>${itemRQ.Ponderacion_P}</td>
                            </tr>`
                        }
                    })
                    let res2 = document.querySelector('#titulo');
                    res2.innerHTML = `Preguntas Aleatorias Importadas desde Excel`;
                }
                else {
                    let msg = document.querySelector('#mensaje');
                    msg.innerHTML =
                        `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">Algo salió mal.Por favor Contacte con el Administrador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`
                    return
                }
            },
            error: function (err) {
                hideLoader();
                let msg = document.querySelector('#mensaje');
                msg.innerHTML =
                    `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">Algo salió mal.Por favor Contacte con el Administrador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`
            },
        });
    } else {
        let msg = document.querySelector('#mensaje');
        msg.innerHTML =
            `<div class="alert alert-danger alert-dismissible fade show" role="alert" id="Ok" >
                            <strong style="font-size:medium">FormData No es compatible con el Navegador</strong>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`;
    }

    function showLoader() {
        $('#loading').show();
    }
    function hideLoader() {
        $('#loading').fadeOut();
    }
    $(document).ready(function () {
        hideLoader();
    })
})