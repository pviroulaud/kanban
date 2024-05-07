$(function () {
    clearFormTiempo();
    $('#btnGuardarTiempo').click(function () {
        guardarRegistro()
    });

});


function cargarDatosTarea() {
    clearFormTiempo()
    $("#idTareaTiempo").val($("#idTarea").val());
    $("#txtNombreTareaTiempo").val($('#txtNombreTarea').val());
    $("#ddlEstadoTareaTiempo").val($('#ddlEstadoTarea').val());
    $('#modal-tiempo').modal('show');
}

function guardarRegistro() {
    if (validarFormTiempo()) {

        showSpinner();
        if (($("#idIncidencia").val() != '') || ($("#idIncidencia").val() != '0')) {
            //Nuevo Registro de tiempo
            var data = {
                tareaId: $("#idTareaTiempo").val(),
                //usuarioId: $("#idTarea").val(),
                ejecucion: $("#txtTiempo").val(),
                fechaEjecucion: $("#fechaTiempo").val(),
                estadoTareaId: $("#ddlEstadoTareaTiempo").val(),
                descripcion: $('#txtDescripcionTiempo').val()
            };

            $.ajax({
                type: 'POST',
                url: $("#registrarTiempo").val(),
                data: data,
                dataType: 'json',
                crossDomain: true,
                beforeSend: function (xhr) {
                    var token = sessionStorage.getItem("tkn");
                    if (token != undefined) {
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    }
                },
            }).done(function (result) {
                hideSpinner();borrarSomee();
                clearFormTiempo();
                if (result.success) {

                    $('#modal-tiempo').modal('hide');
                    
                    Swal.fire("Información", "Se registro el tiempo.", "success");
                } else {
                    Swal.fire("Información", "No se registro el tiempo.", "error");
                }
            });
        }        
    }
}

function validarFormTiempo() {
    var valid = true;
    var tiempo = $("#txtTiempo").val().trim();
    if (tiempo == "") {
        $("#validtxtTiempo").show();
        valid = false;
    } else {
        $("#validtxtTiempo").hide();
    }

    var e = $("#ddlEstadoTareaTiempo").val();
    if (e == "" || e == null) {
        $("#validddlEstadoTareaTiempo").show();
        valid = false;
    } else {
        $("#validddlEstadoTareaTiempo").hide();
    }

    var f = $("#fechaTiempo").val().trim();
    if (f == "") {
        $("#validfechaTiempo").show();
        valid = false;
    } else {
        $("#validfechaTiempo").hide();
    }

    var d = $("#txtDescripcionTiempo").val().trim();
    if (d == "") {
        $("#validtxtDescripcionTiempo").show();
        valid = false;
    } else {
        $("#validtxtDescripcionTiempo").hide();
    }
    return valid;
}

function clearFormTiempo() {
    
    $("#idTareaTiempo").val('');
    $("#txtTiempo").val('');
    $("#ddlEstadoTareaTiempo").val('');
    $("#fechaTiempo").val('');
    $("#txtDescripcionTiempo").val('');
}