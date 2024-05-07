$(function () {

    $('#btnGuardarTarea').click(function () {
        guardarTarea()
    });

    
});



function validarFormTarea() {
    var valid = true;
    var nombre = $("#txtNombreTarea").val().trim();
    if (nombre == "") {
        $("#validtxtNombreTarea").show();
        valid = false;
    } else {
        $("#validtxtNombreTarea").hide();
    }
    var t = $("#ddlTipoTarea").val();
    if (t == "" || t == null) {
        $("#validddlTipoTarea").show();
        valid = false;
    } else {
        $("#validddlTipoTarea").hide();
    }
    var e = $("#ddlTipoTarea").val();
    if (e == "" || e == null) {
        $("#validddlTipoTarea").show();
        valid = false;
    } else {
        $("#validddlTipoTarea").hide();
    }
    var r = $("#ddlEstadoTarea").val();
    if (r == "" || r == null) {
        $("#validddlEstadoTarea").show();
        valid = false;
    } else {
        $("#validddlEstadoTarea").hide();
    }

    return valid;
}

function clearFormTarea() {
    $("#idTarea").val('');
    $("#txtNombreTarea").val('');
    $("#ddlTipoTarea").val('');
    $("#ddlEstadoTarea").val('');
    $('#ddlResponsableTarea').val('');
    $('#txtInformadorTarea').val('');
    $('#txtDescripcionTarea').val('');
    $('#txtsemanaDeEjecucionPlanificadaTarea').val('');
    $('#txtsemanaDeEjecucionRealTarea').val('');
    $('#txtEstimacionTarea').val('');
    $('#txtEjecucionTarea').val('');

}

function guardarTarea() {
    if (validarFormTarea()) {

        showSpinner();
        if (($("#idTarea").val() == '') || ($("#idTarea").val() == '0')) {
            //Nueva Incidencia
            var data = {                
                nombre: $("#txtNombreTarea").val(),
                tipoTareaId: $("#ddlTipoTarea").val(),
                estadoId: $("#ddlEstadoTarea").val(),
                usuarioResponsableId: $("#ddlResponsableTarea").val(),
                incidenciaId: $("#idIncidencia").val(),
                descripcion: $('#txtDescripcionTarea').val(),
                estimacion: $('#txtEstimacionTarea').val(),
                ejecucion: $('#txtEjecucionTarea').val(),
                semanaDeEjecucionPlanificada: $('#txtsemanaDeEjecucionPlanificadaTarea').val().replaceAll("-W",""),
                semanaDeEjecucionReal: $('#txtsemanaDeEjecucionRealTarea').val().replaceAll("-W", "")

            };

            $.ajax({
                type: 'POST',
                url: $("#nuevaTarea").val(),
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
                clearFormTarea();
                if (result.success) {

                    $('#modal-tarea').modal('hide');
                    obtenerIncidencia($("#idIncidencia").val())
                    Swal.fire("Información", "Se registro la tarea.", "success");
                } else {
                    Swal.fire("Información", "No se registro la tarea.", "error");
                }
            });
        }
        else {
            var data = {
                id: $("#idTarea").val(),
                nombre: $("#txtNombreTarea").val(),
                tipoTareaId: $("#ddlTipoTarea").val(),
                estadoId: $("#ddlEstadoTarea").val(),
                usuarioResponsableId: $("#ddlResponsableTarea").val(),
                incidenciaId: $("#idIncidencia").val(),
                descripcion: $('#txtDescripcionTarea').val(),
                estimacion: $('#txtEstimacionTarea').val(),
                ejecucion: $('#txtEjecucionTarea').val(),
                semanaDeEjecucionPlanificada: $('#txtsemanaDeEjecucionPlanificadaTarea').val().replaceAll("-W", ""),
                semanaDeEjecucionReal: $('#txtsemanaDeEjecucionRealTarea').val().replaceAll("-W", "")

            };

            $.ajax({
                type: 'PUT',
                url: $("#actualizarTarea").val(),
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
                clearFormTarea();
                if (result.success) {

                    $('#modal-tarea').modal('hide');
                    obtenerIncidencia($("#idIncidencia").val())
                    Swal.fire("Información", "Se registro la tarea.", "success");
                } else {
                    Swal.fire("Información", "No se registro la tarea.", "error");
                }
            });
        }

    }
}

function completarFormTarea(datos) {
    $("#idTarea").val(datos.id);
    $("#txtNombreTarea").val(datos.nombre);
    $("#ddlTipoTarea").val(datos.tipoTareaId);
    $("#ddlEstadoTarea").val(datos.estadoId);
    datos.semanaDeEjecucionPlanificada.slice(0, 4) + "-W" + datos.semanaDeEjecucionPlanificada.slice(4)
    $('#txtInformadorTarea').val(buscarEnSelect('ddlResponsableTarea', datos.usuarioCreadorId));
    $('#txtDescripcionTarea').val(datos.descripcion); 
    if (datos.semanaDeEjecucionPlanificada != null) $('#txtsemanaDeEjecucionPlanificadaTarea').val(datos.semanaDeEjecucionPlanificada.toString().slice(0, 4) + "-W" + datos.semanaDeEjecucionPlanificada.toString().slice(4));
    if (datos.semanaDeEjecucionReal!=null) $('#txtsemanaDeEjecucionRealTarea').val(datos.semanaDeEjecucionReal.toString().slice(0, 4) + "-W" + datos.semanaDeEjecucionReal.toString().slice(4));
    $('#txtEstimacionTarea').val(datos.estimacion);
    $('#txtEjecucionTarea').val(datos.ejecucion);
    setTimeout(function () { $('#ddlResponsableTarea').val(datos.usuarioResponsableId).change(); }, 100)
}


function obtenerTarea(idTarea) {
    $.ajax({
        type: 'GET',
        url: $("#obtenerTarea").val(),
        data: { id: idTarea },
        dataType: 'json',
        crossDomain: true,
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem("tkn");
            if (token != undefined) {
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            }
        },
    }).done(function (result) {
        hideSpinner();borrarSomee();borrarSomee();
        clearFormTarea();
        if (result.success) {

            completarFormTarea(result.data);
            $('#modal-tarea').modal('show');
        } else {
            Swal.fire("Información", "No se pudo obtener la tarea.", "error");
        }
    });
}
