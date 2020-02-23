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

// agregar y eliminar filas de una tablas
s(function agregarFila() {
    document.getElementById("grd").insertRow(-1).innerHTML = '<td></td><td></td><td></td><td></td>';
});

$(function eliminarFila() {
    var table = document.getElementById("grd");
    var rowCount = table.rows.length;
    //console.log(rowCount);

    if (rowCount <= 1)
        toastr.warning('No se puede eliminar el encabezado');
    else
        table.deleteRow(rowCount - 1);
})