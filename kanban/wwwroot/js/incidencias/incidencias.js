var table;
$(function () {
    $('#btnNuevaIncidencia').click(function(){
        clearFormIncidencia();
        $('#dialogIncidencia').removeClass('modal-xl')
        $('#dialogIncidencia').addClass('modal-lg')
        $('#informacionBasica').addClass('col-sm-12');
        $('#informacionBasica').removeClass('col-sm-9');
        $('#informacionAdicional').addClass('d-none')

        $('#informacionTareas').addClass('d-none')
        $('#modal-incidencia').modal('show');
    });
    $('#btnGuardarIncidencia').click(function(){
        guardarIncidencia()
    });
    

    grillaInsidencias();
    $('#dt-dataIncidencias tbody').on('click', 'td.details-control', function (e) {
        let tr = e.target.closest('tr');
        let row = table.row(tr);
     
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(mostarDetalleFila(row.data())).show();
        }
    });


    $('.filtroBusqueda').change(function () {
        $('#dt-dataIncidencias').DataTable().ajax.reload();
    });

    fillSelect($('#listarProyectos').val(),"ddlProyecto","","","nombre");
    fillSelect($('#listarTipoIncidencia').val(),"ddlTipo","","","nombre");
    fillSelect($('#listarEstados').val(),"ddlEstado","","","nombre");    
    fillSelectUsuarios($('#listarUsuarios').val(), "ddlResponsable", "modal-incidencia", "", "nombre");

    fillSelect($('#listarEstados').val(), "ddlEstadoTarea", "", "", "nombre");
    fillSelectUsuarios($('#listarUsuarios').val(), "ddlResponsableTarea", "modal-tarea", "Sin Asignar", "nombre");
    fillSelect($('#listarTipoTarea').val(), "ddlTipoTarea", "", "", "nombre");

    fillSelectUsuarios($('#listarUsuarios').val(), "ddlFiltroUsuarios", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarProyectos').val(), "ddlFiltroProyectos", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarTipoIncidencia').val(), "ddlFiltroTipoIncidencia", "contenedorFiltros", "Todos", "nombre");
    fillSelectUsuarios($('#listarEstados').val(), "ddlFiltroEstadoIncidencia", "contenedorFiltros", "Todos", "nombre");
    
    fillSelect($('#listarEstados').val(), "ddlEstadoTareaTiempo", "", "", "nombre");
});

function fillSelectUsuarios(url, selectId,modalId,emptyOpt,textProp ) {
    showSpinner();
    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
        dataType: 'json',
        crossDomain: true,
                beforeSend: function (xhr) {
                    var token= sessionStorage.getItem("tkn");
                    if (token!=undefined){
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
            if (modalId!=null && modalId!="")
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

function grillaInsidencias(){
        // Column Definitions
        var columnSet = [
            { className: 'details-control', orderable: false,data: null, defaultContent: '<i style="cursor:pointer;" class="fas fa-search"></i>'},
            { title: "ID", id: "id", data: "id", type: "number" },
            { title: "Codigo Proyecto", id: "codigoProyecto", data: "codigoProyecto", type: "text" },       
            { title: "Proyecto", id: "nombreProyecto", data: "nombreProyecto", type: "text" },    
            { title: "Nombre", id: "nombre", data: "nombre", type: "text" },
            { title: "Tipo", id: "nombreTipoIncidencia", data: "nombreTipoIncidencia", type: "text", className: 'text-center' },   
            { title: "Estado", id: "nombreEstado", data: "nombreEstado", type: "text", className: 'text-center' },
            { title: "Estimacion (Hs)", id: "estimacionTotal", data: "estimacionTotal", type: "text", className: 'text-center' },
            { title: "Ejecucion", id: "id", data: null, type: "readonly", className: 'text-center' },
            { title: "Acciones", id: "id", data: "id", type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-center' },
        ];
        /*data table */
    //showSpinner();
    
        table = $('#dt-dataIncidencias').DataTableEdit({
            ajax: {
                url: $("#listarIncidencias").val(),
                type: "POST",
                datatype: "json",
                crossDomain: true,
                beforeSend: function (xhr) {
                    var token= sessionStorage.getItem("tkn");
                    if (token!=undefined){
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    }                    
                },
                data: function (d) {
                    d.proyectoId= $("#ddlFiltroProyectos").val(),
                    d.tipoIncidenciaId= $("#ddlFiltroTipoIncidencia").val(),
                    d.estadoIncidenciaId= $("#ddlFiltroEstadoIncidencia").val(),
                    d.usuarioResponsableId = $("#ddlFiltroUsuarios").val()
                },
            }, columns: columnSet,
            columnDefs: [
                {
                    targets:
                        8, render: function (data, type, full, meta) {
                        var porcentaje = parseInt((full.ejecucionTotal / full.estimacionTotal) * 100);
                        if (full.estimacionTotal == 0) {
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
                        9, render: function (data, type, full, meta) {
                            return '<button data="' + data + '" class="btn btn-primary btn-sm editData" tabindex="0" title="Editar"><i class="fas fa-edit"></i></button>';
                    },
                }
            ],
            drawCallback: function (settings, json) {
                hideSpinner();borrarSomee();
                $(".editData").click(function () {
                    obtenerIncidencia($(this).attr('data'));
                });            
            }
        });
}

function mostarDetalleFila(datos) {
    console.log(datos.resumenTareas)
    var _html = ''
    for (var n = 0; n < datos.resumenTareas.length; n++) {
        var porcentaje = 0;
        if (datos.resumenTareas[n].estimacion>0) {
            porcentaje = parseInt((datos.resumenTareas[n].ejecucion / datos.resumenTareas[n].estimacion) * 100);
        }
        var color = "bg-info";
        if (porcentaje == 100) {
            color ="bg-success"
        }
        if (porcentaje > 100) {
            color ="bg-danger"
        }
        _html += '<dl class="row">'
        _html += '<div class="col-sm-3"><strong>' + datos.resumenTareas[n].nombreTarea + '</strong></div>'
        _html += '<div class="col-sm-2"><strong>Tareas: </strong>' + datos.resumenTareas[n].cantidadTareas + '</div>'
        _html += '<div class="col-sm-2"><strong>Estimaci&oacute;n: </strong>' + datos.resumenTareas[n].estimacion + 'Hs.</div>'
        _html += '<div class="col-sm-2"><strong>Ejecuci&oacute;n: </strong>' + datos.resumenTareas[n].ejecucion + 'Hs.</div>'
        _html += '<div class="col-sm-3"><div class="progress"><div class="progress-bar ' + color +'" role="progressbar" style="width: ' + porcentaje +'%;" aria-valuenow="'+porcentaje+'" aria-valuemin="0" aria-valuemax="100">'+porcentaje+'%</div></div></div>'
        _html += '</dl>'
    }

    return _html;   
}

function validarFormIncidencia(){
    var valid =true;
    var nombre = $("#txtNombreIncidencia").val().trim();
    if (nombre == "") {
        $("#validtxtNombreIncidencia").show();
        valid = false;
    } else {
        $("#validtxtNombreIncidencia").hide();
    }
    var p = $("#ddlProyecto").val();
    if (p == "" || p == null) {
        $("#validddlProyecto").show();
        valid = false;
    } else {
        $("#validddlProyecto").hide();
    }
    var t = $("#ddlTipo").val();
    if (t == "" || t == null) {
        $("#validddlTipo").show();
        valid = false;
    } else {
        $("#validddlTipo").hide();
    }
    if (($("#idIncidencia").val()=='')||($("#idIncidencia").val()=='0')){
        //Validaciones de actualizacion
    }
    return valid;
}

function clearFormIncidencia(){
    $("#idIncidencia").val('');
    $("#txtNombreIncidencia").val('');
    $("#ddlProyecto").val('');
    $("#ddlTipo").val('');
}

function guardarIncidencia(){
    if (validarFormIncidencia()){
        
        showSpinner();
        if (($("#idIncidencia").val()=='')||($("#idIncidencia").val()=='0')){
            //Nueva Incidencia
            var data = {
                proyectoId: $("#ddlProyecto").val(),
                nombre: $("#txtNombreIncidencia").val(),
                tipoIncidenciaId: $("#ddlTipo").val(),
                descripcion: $('#txtDescripcion').val()
            };
           
            $.ajax({
                type: 'POST',
                url: $("#nuevaIncidencia").val(),
                data: data,
                dataType: 'json',
                crossDomain: true,
                beforeSend: function (xhr) {
                    var token= sessionStorage.getItem("tkn");
                    if (token!=undefined){
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    }                    
                },            
            }).done(function (result) {
                hideSpinner();borrarSomee();
                clearFormIncidencia();
                if (result.success) {
                    
                    $('#modal-incidencia').modal('hide');
                    $('#dt-dataIncidencias').DataTable().ajax.reload();
                    Swal.fire("Información", "Se registro la incidencia.", "success");
                } else {
                    Swal.fire("Información", "No se registro la incidencia.", "error");               
                }
            });
        }
        else{
            var data = {
                id: $("#idIncidencia").val(),
                proyectoId: $("#ddlProyecto").val(),
                nombre: $("#txtNombreIncidencia").val(),
                tipoIncidenciaId: $("#ddlTipo").val(),
                descripcion: $('#txtDescripcion').val(),
                usuarioResponsableId: $('#ddlResponsable').val(),
                estadoId: $('#ddlEstado').val()
            };

            $.ajax({
                type: 'PUT',
                url: $("#actualizaIncidencia").val(),
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
                clearFormIncidencia();
                if (result.success) {

                    $('#modal-incidencia').modal('hide');
                    $('#dt-dataIncidencias').DataTable().ajax.reload();
                    Swal.fire("Información", "Se registro la incidencia.", "success");
                } else {
                    Swal.fire("Información", "No se registro la incidencia.", "error");
                }
            });
        }
        
    }
}

function completarFormIncidencia(datos){
    $("#idIncidencia").val(datos.id);
    $("#ddlProyecto").val(datos.proyectoId);
    $("#txtNombreIncidencia").val(datos.nombre);
    $("#ddlTipo").val(datos.tipoIncidenciaId);  
    $('#txtDescripcion').val(datos.descripcion);
    
    $('#txtInformador').val(datos.nombreCreador);
    $('#ddlResponsable').val(datos.usuarioResponsableId);
    $('#ddlEstado').val(datos.estadoId);
    $('#txtEstimacion').val(datos.estimacionTotal);
    $('#txtEjecucion').val(datos.ejecucionTotal);

    //detalle de Tareas (datos.tareas)
    var _html = '';
    for (var n = 0; n < datos.tareas.length; n++) {
        var porcentaje = 0;
        if (datos.tareas[n].estimacion > 0) {
            porcentaje = parseInt((datos.tareas[n].ejecucion / datos.tareas[n].estimacion) * 100);
        }
        
        var color = "bg-info";
        if (porcentaje == 100) {
            color = "bg-success"
        }
        if (porcentaje > 100) {
            color = "bg-danger"
        }
        var responsable = 'Sin Asignar';
        if (datos.tareas[n].usuarioResponsableId != null) {
            responsable = buscarEnSelect('ddlResponsable', datos.tareas[n].usuarioResponsableId)
        }
        
        _html += '<tr>'
        _html += '<td>' + datos.tareas[n].nombreTarea + '</td>'
        _html += '<td>' + datos.tareas[n].nombre + '</td>'
        _html += '<td>' + buscarEnSelect('ddlEstado', datos.tareas[n].estadoId) + '</td>'
        _html += '<td>' + datos.tareas[n].estimacion + 'Hs.</td>'
        _html += '<td>' + datos.tareas[n].ejecucion + 'Hs.</td>'
        _html += '<td><div class="progress"><div class="progress-bar ' + color + '" role="progressbar" style="width: ' + porcentaje + '%;" aria-valuenow="' + porcentaje + '" aria-valuemin="0" aria-valuemax="100">' + porcentaje + '%</div></div></td>'
        _html += '<td>' + responsable + '</td>'
        _html += '<td><i style="cursor:pointer;;font-size:1.5em" class="fas fa-search" onclick="detalleTarea(' + datos.tareas[n].id + ')"></i></td>'
        _html += '</tr>'
    }
   


    $('#filasTareas').html(_html);
}

function detalleTarea(tareaId) {
    obtenerTarea(tareaId)    
}

function nuevaTarea() {
    clearFormTarea();
    $('#modal-tarea').modal('show');
   
}

function obtenerIncidencia(idIncidencia){
    $.ajax({
        type: 'GET',
        url: $("#obtenerIncidencia").val(),
        data: {id:idIncidencia},
        dataType: 'json',
        crossDomain: true,
        beforeSend: function (xhr) {
            var token= sessionStorage.getItem("tkn");
            if (token!=undefined){
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            }                    
        },            
    }).done(function (result) {
        hideSpinner();borrarSomee();
        clearFormIncidencia();
        if (result.success) {
            
            completarFormIncidencia(result.data);
            $('#dialogIncidencia').removeClass('modal-lg')
            $('#dialogIncidencia').addClass('modal-xl')
            $('#informacionBasica').removeClass('col-sm-12');
            $('#informacionBasica').addClass('col-sm-9');
            $('#informacionAdicional').removeClass('d-none')
            $('#informacionTareas').removeClass('d-none')
           $('#modal-incidencia').modal('show');
        } else {
            Swal.fire("Información", "No se pudo obtener la incidencia.", "error");               
        }
    });
}