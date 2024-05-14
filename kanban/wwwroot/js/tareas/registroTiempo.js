var table;
$(function () {
    clearFormTiempo();
    $('#btnGuardarTiempo').click(function () {
        guardarRegistro()
    });

});

function completarActividad(dataSet) {
    if (table != undefined) {
        table.destroy();
    }
    
    // Column Definitions
    var columnSet = [

        { title: "Incidencia", id: "nombreIncidencia", data: "nombreIncidencia", type: "text" },
        { title: "Tarea", id: "nombreTarea", data: "nombreTarea", type: "text" },
        { title: "Usuario", id: "nombreUsuario", data: "nombreUsuario", type: "text", className: 'text-center' },
        { title: "Estado", id: "nombreEstadoTarea", data: "nombreEstadoTarea", type: "text", className: 'text-center' },        
        { title: "Ejecucion", id: "ejecucion", data: 'ejecucion', type: "readonly", className: 'text-center' },
        { title: "Fecha Ejecucion", id: "fechaEjecucion", data: 'fechaEjecucion', type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-center' },
        { title: "Fecha Registro", id: "fechaRegistro", data: 'fechaRegistro', type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-center' },
        { title: "Detalle", id: "descripcion", data: 'descripcion', type: "readonly", orderable: false, searchable: false, width: 65, className: 'text-start' },
    ];
    /*data table */

    table = $('#dt-dataActividad').DataTable({
        data:  dataSet ,       
        columns: columnSet,   
        columnDefs: [
            {
                targets:
                    5, render: function (data, type, full, meta) {                       
                    return full.fechaEjecucion.slice(0, full.fechaEjecucion.indexOf("T"))
                    },
            },
            {
                targets:
                    6, render: function (data, type, full, meta) {
                    return full.fechaRegistro.slice(0, full.fechaEjecucion.indexOf("T"))
                    },
            }
        ],
    });


    $('#modal-actividad').modal('show');
    
}

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
                ejecucion: $("#txtTiempo").val().replaceAll(".",","),
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