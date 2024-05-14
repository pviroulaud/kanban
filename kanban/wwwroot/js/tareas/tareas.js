$(function () {

    $('#btnGuardarTarea').click(function () {
        guardarTarea()
    });
    $('#btnNuevaTarea').click(function () {
        nuevaTarea();
        $('#ddlProyectoTarea').trigger('change');
    });

    $('.toggleTablero').click(function () {
        if ($('#btnToggleTabla').hasClass('d-none')) {
            // Ver Tablero
            $('#visualizadorTabla').addClass('d-none');
            $('#visualizadorTablero').removeClass('d-none');
            $('#btnToggleTabla').removeClass('d-none');
            $('#btnToggleTablero').addClass('d-none');

        }
        else {
            // Ver Tabla
            $('#visualizadorTabla').removeClass('d-none');
            $('#visualizadorTablero').addClass('d-none');
            $('#btnToggleTabla').addClass('d-none');
            $('#btnToggleTablero').removeClass('d-none');
        }


    });

    grillaTareas();
    $('.filtroBusqueda').change(function () {
        $('#dt-dataTareas').DataTable().ajax.reload();
    });

    $('#ddlProyectoTarea').change(function () {
        fillSelectUsuarios($('#listarIncidenciasProyecto').val() + "?proyectoId=" + $('#ddlProyectoTarea').val(), "ddlIncidenciaTarea", "", "", "nombre");
    });

    fillSelectUsuarios($('#listarIncidencias').val(), "ddlFiltroIncidencia", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarUsuarios').val(), "ddlFiltroUsuarios", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarProyectos').val(), "ddlFiltroProyectos", "contenedorFiltros", "Todos", "nombre");
    fillSelect($('#listarProyectos').val(), "ddlProyectoTarea", "", "", "nombre");
    
    fillSelectUsuarios($('#listarTipoTarea').val(), "ddlFiltroTipoTarea", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarEstados').val(), "ddlFiltroEstadoTarea", "contenedorFiltros", "Todos", "nombre");

});

function grillaTareas() {
    // Column Definitions
    var columnSet = [        
        { title: "ID", id: "id", data: "id", type: "number" },
        //{ title: "Codigo Proyecto", id: "codigoProyecto", data: "codigoProyecto", type: "text" },
        { title: "Proyecto", id: "nombreProyecto", data: "nombreProyecto", type: "text" },
        { title: "Incidencia", id: "nombreIncidencia", data: "nombreIncidencia", type: "text" },
        { title: "Nombre", id: "nombre", data: "nombre", type: "text" },
        { title: "Tipo", id: "nombreTarea", data: "nombreTarea", type: "text", className: 'text-center' },
        { title: "Estado", id: "nombreEstadoTarea", data: "nombreEstadoTarea", type: "text", className: 'text-center' },
        { title: "Responsable", id: "nombreUsuarioResponsable", data: "nombreUsuarioResponsable", type: "text", className: 'text-center' },
        { title: "Ejecucion", id: "id", data: null, type: "readonly", className: 'text-center' },
        { title: "Ejecucion/Estimacion", id: "id", data: null, type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-center' },
        { title: "Acciones", id: "id", data: "id", type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-center' },
    ];
    /*data table */
    showSpinner();

    table = $('#dt-dataTareas').DataTableEdit({
        ajax: {
            url: $("#listarTareas").val(),
            type: "POST",
            datatype: "json",
            crossDomain: true,
            beforeSend: function (xhr) {
                var token = sessionStorage.getItem("tkn");
                if (token != undefined) {
                    xhr.setRequestHeader("Authorization", "Bearer " + token);
                }
            },
            data: function (d) {
                d.proyectoId = $("#ddlFiltroProyectos").val(),
                    d.incidenciaId = $("#ddlFiltroIncidencia").val(),
                    d.tipoTareaId = $("#ddlFiltroTipoTarea").val(),
                    d.estadoTareaId = $("#ddlFiltroEstadoTarea").val(),
                    d.usuarioResponsableId = $("#ddlFiltroUsuarios").val()
            },
        }, columns: columnSet,
        columnDefs: [
            {
                targets:
                    5, render: function (data, type, full, meta) {
                    var block = '';
                    if (full.estadoId == 5) {
                        block = '<i class="fas fa-exclamation-triangle text-warning me-1"></i>';
                    }
                    return full.nombreEstadoTarea + block;
                    },
            },
            {
                targets:
                    7, render: function (data, type, full, meta) {
                        var porcentaje = parseInt((full.ejecucion / full.estimacion) * 100);
                        if (full.estimacion == 0) {
                            porcentaje = 0;
                        }
                        var color = "bg-info";
                        if (porcentaje == 100) {
                            color = "bg-success"
                        }
                        if (porcentaje > 100) {
                            color = "bg-danger"
                        }
                        
                        return '<div class="progress"><div class="progress-bar ' + color + '" role="progressbar" style="width: ' + porcentaje + '%;" aria-valuenow="' + porcentaje + '" aria-valuemin="0" aria-valuemax="100">' + porcentaje + '%</div></div>';
                    },
            },
            {
                targets:
                    8, render: function (data, type, full, meta) {
                    return full.ejecucion +'/'+ full.estimacion;
                    },
            },
            {
                targets:
                    9, render: function (data, type, full, meta) {
                    return '<button data="' + data + '" class="btn btn-primary btn-sm editData" tabindex="0" title="Editar"><i class="fas fa-edit"></i></button>  '+
                        '<button data="' + data + '" class="ml-1 btn btn-primary btn-sm verActividad" tabindex="0" title="Ver Actividad"><i class="fas fa-hourglass-end"></i></button>';
                    },
            }
        ],
        drawCallback: function (settings, json) {
            hideSpinner();borrarSomee();
            $(".editData").click(function () {
                obtenerTarea($(this).attr('data'));
            });
            $(".verActividad").click(function () {
                verActividadTarea($(this).attr('data'));
            });
        }
    });
}

function verActividadTarea(tareaId) {
    $.ajax({
        type: 'GET',
        url: $("#actividadTarea").val(),
        data: { id: tareaId },
        dataType: 'json',
        crossDomain: true,
        beforeSend: function (xhr) {
            var token = sessionStorage.getItem("tkn");
            if (token != undefined) {
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            }
        },
    }).done(function (result) {
        hideSpinner(); borrarSomee();
        if (result.success) {

            completarActividad(result.data);

        } else {
            Swal.fire("Información", "No se pudo obtener la incidencia.", "error");
        }
    });
}

function nuevaTarea() {
    clearFormTarea();
    $('#modal-tarea').modal('show');

}

function detalleTarea(tareaId) {
    obtenerTarea(tareaId)
}

function validarFormTarea() {
    var valid = true;
    var p = $("#ddlProyectoTarea").val();
    if (p == "" || p == null) {
        $("#validddlProyectoTarea").show();
        valid = false;
    } else {
        $("#validddlProyectoTarea").hide();
    }
    var i = $("#ddlIncidenciaTarea").val();
    if (i == "" || i == null) {
        $("#validddlIncidenciaTarea").show();
        valid = false;
    } else {
        $("#validddlIncidenciaTarea").hide();
    }

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
    $('#ddlProyectoTarea').val('');
    $('#ddlIncidenciaTarea').val('');
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
            //Nueva Tarea
            var data = {

                nombre: $("#txtNombreTarea").val(),
                tipoTareaId: $("#ddlTipoTarea").val(),
                estadoId: $("#ddlEstadoTarea").val(),
                usuarioResponsableId: $("#ddlResponsableTarea").val(),
                incidenciaId: $("#ddlIncidenciaTarea").val(),
                descripcion: $('#txtDescripcionTarea').val(),
                estimacion: $('#txtEstimacionTarea').val().replaceAll(".", ","),
                //ejecucion: $('#txtEjecucionTarea').val().replaceAll(".", ","),
                semanaDeEjecucionPlanificada: $('#txtsemanaDeEjecucionPlanificadaTarea').val().replaceAll("-W", ""),
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
                    $('#dt-dataTareas').DataTable().ajax.reload();
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
                incidenciaId: $("#ddlIncidenciaTarea").val(),
                descripcion: $('#txtDescripcionTarea').val(),
                estimacion: $('#txtEstimacionTarea').val().replaceAll(".", ","),
                //ejecucion: $('#txtEjecucionTarea').val().replaceAll(".", ","),
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
                    $('#dt-dataTareas').DataTable().ajax.reload();
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
    $('#ddlProyectoTarea').val(datos.proyectoId);

    fillSelectUsuarios($('#listarIncidenciasProyecto').val() + "?proyectoId=" + $('#ddlProyectoTarea').val(), "ddlIncidenciaTarea", "", "", "nombre",
);
    
    $("#txtNombreTarea").val(datos.nombre);
    $("#ddlTipoTarea").val(datos.tipoTareaId);
    $("#ddlEstadoTarea").val(datos.estadoId);
    $('#ddlResponsableTarea').val(datos.usuarioResponsableId).change();
    $('#txtInformadorTarea').val(buscarEnSelect('ddlResponsableTarea', datos.usuarioCreadorId));
    $('#txtDescripcionTarea').val(datos.descripcion);
    if (datos.semanaDeEjecucionPlanificada != null) $('#txtsemanaDeEjecucionPlanificadaTarea').val(datos.semanaDeEjecucionPlanificada.toString().slice(0, 4) + "-W" + datos.semanaDeEjecucionPlanificada.toString().slice(4));
    if (datos.semanaDeEjecucionReal != null) $('#txtsemanaDeEjecucionRealTarea').val(datos.semanaDeEjecucionReal.toString().slice(0, 4) + "-W" + datos.semanaDeEjecucionReal.toString().slice(4));
    $('#txtEstimacionTarea').val(datos.estimacion);
    $('#txtEjecucionTarea').val(datos.ejecucion);
    //$('#ddlIncidenciaTarea').val(datos.incidenciaId);
    setTimeout(function () { establecerIncidenciaTarea(datos.incidenciaId) },100)
}

function establecerIncidenciaTarea(incidenciaId) {
    $('#ddlIncidenciaTarea').val(incidenciaId);
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
        hideSpinner();borrarSomee();
        clearFormTarea();
        if (result.success) {

            completarFormTarea(result.data);
            $('#modal-tarea').modal('show');
        } else {
            Swal.fire("Información", "No se pudo obtener la tarea.", "error");
        }
    });
}



function fillSelectUsuarios(url, selectId, modalId, emptyOpt, textProp) {
    showSpinner();
    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
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

        if (true) {
            $("#" + selectId).empty();
            if (emptyOpt != null && emptyOpt != "")
                $("#" + selectId).append($("<option></option>").val('').html(emptyOpt));

            $.each(result.data, function (data, value) {
                $("#" + selectId).append($("<option></option>").val(value.id).html(Reflect.get(value, textProp)));
            });

            if (modalId != null && modalId != "")
                $("#" + selectId).select2({ dropdownParent: $("#" + modalId) });

        }
    });
}

function buscarEnSelect(selectId, valueBuscar) {
    var opt = $('#' + selectId + ' option');
    var ret = undefined;
    for (var n = 0; n < opt.length; n++) {
        if (opt[n].value == valueBuscar) {
            ret = opt[n].text;
            break;
        }
    }
    return ret;
}

