$(function () {
    $("#btnBuscarProyecto").click(function () { $('#dt-dataProyectos').DataTable().ajax.reload(); });
    $("#btnNuevoProyecto").click(function () {
        clearFormProyecto();
        $('#modal-proyecto').modal('show');
    });

    $("#btnGuardarProyecto").click(function () {
        saveFormProyecto();
    });


    // Column Definitions
    var columnSet = [
        { title: "ID", id: "id", data: "id", type: "number" },
        { title: "Nombre", id: "nombre", data: "nombre", type: "text" },    
        { title: "Codigo", id: "codigoProyecto", data: "codigoProyecto", type: "text" },       
        { title: "Acciones", id: "id", data: "id", type: "readonly", orderable: false, searchable: false, width: 65 },
    ];


    /*data table */
    showSpinner();
    var myTable = $('#dt-dataProyectos').DataTableEdit({
        ajax: {
            url: $("#listarProyectos").val(),
            type: "GET",
            datatype: "json",
            crossDomain: true,
            beforeSend: function (xhr) {
                var token= sessionStorage.getItem("tkn");
                if (token!=undefined){
                    xhr.setRequestHeader("Authorization", "Bearer " + token);
                }                    
            },            
            data: function (d) {
                d.nombre = $("#txtFilterNombreProyecto").val();
            }
        }, columns: columnSet,
        columnDefs: [{
            targets: 3, render: function (data, type, full, meta) {
                return '<button data="' + data + '" class="btn btn-primary btn-sm editData" tabindex="0" title="Editar"><i class="fas fa-edit"></i></button>'                            
                    + '<button data="' + data + '" class="btn btn-danger btn-sm deleteEnt" tabindex="0" type="button" title="Deshabilitar"><i class="fas fa-trash"></i></button>';
            },
        }],
        drawCallback: function (settings, json) {
            hideSpinner();borrarSomee();
            $(".editData").click(function () {
                obtenerProyecto($(this).attr('data'));
            });
            $(".deleteEnt").click(function () {
                eliminarProyecto($(this).attr('data'));
            });
        }
    });
});

function saveFormProyecto(){
    if (validarFormProyecto()) {
        
    
        $("#msgErrorNvo").hide();
        $("#errorNvo").hide();
    
        showSpinner();
        if ($('#idProyecto').val() ==0){
            var data = {            
                nombre: $("#txtNombreProyecto").val(),
                codigoProyecto: $("#txtCodigoProyecto").val(),            
            };

            $.ajax({
                type: 'POST',
                url: $("#nuevoProyecto").val(),
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
                if (result.success) {
                    clearFormProyecto();
                    $('#modal-proyecto').modal('hide');
                    $('#dt-dataProyectos').DataTable().ajax.reload();                
                    Swal.fire("Información", "Se creo el proyecto.", "success");
                } else {
                    Swal.fire("Error", "No se creo el proyecto." + result.error, "error");
                }
            });
        }
        else{
            var data = {   
                id: $('#idProyecto').val(),
                nombre: $("#txtNombreProyecto").val(),
                codigoProyecto: $("#txtCodigoProyecto").val(),            
            };
            
            $.ajax({
                type: 'PUT',
                url: $("#actualizarProyecto").val(),
                data: data,
                dataType: 'json'
            }).done(function (result) {
                hideSpinner();borrarSomee();
                if (result.success) {
                    clearFormProyecto();
                    $('#modal-proyecto').modal('hide');
                    $('#dt-dataProyectos').DataTable().ajax.reload();                
                    Swal.fire("Información", "Se actualizo el proyecto.", "success");
                } else {
                    Swal.fire("Error", "No se actualizo el proyecto." + result.error, "error");
                }
            });
        }

    }
}

function validarFormProyecto(){
    var valid =true;
    var nombre = $("#txtNombreProyecto").val().trim();
    if (nombre == "") {
        $("#validtxtNombreProyecto").show();
        valid = false;
    } else {
        $("#validtxtNombreProyecto").hide();
    }
    var codigo = $("#txtCodigoProyecto").val().trim();
    if (codigo == "") {
        $("#validtxtCodigoProyecto").show();
        valid = false;
    } else {
        $("#validtxtCodigoProyecto").hide();
    }
    return valid;
}

function obtenerProyecto(id) {
    showSpinner();
    $.ajax({
        type: 'GET',
        url: $("#obtenerProyecto").val(),
        data: { id: id },
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
        if (result.success) {                    
            clearFormProyecto();
            completarFormProyecto(result.data);
            $('#modal-proyecto').modal('show');
        } else {
            Swal.fire("Error", "No pudo obtener el proyecto." + result.error, "error");
        }
    });
}

function eliminarProyecto(id) {
    Swal.fire({
        title: 'La acción requiere de confirmación',
        text: "¡Está por eliminar un proyecto!. Se eliminaran todas las incidencias, tareas y Registros. ¿Desea continuar?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Continuar'
    }).then((result) => {
        if (result.value) {
            showSpinner();
            $.ajax({
                type: 'DELETE',
                url: $("#eliminarProyecto").val(),
                data: { id: id },
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
                if (result.success) {
                    $('#dt-dataProyectos').DataTable().ajax.reload();
                    Swal.fire("Información", "El proyecto fue eliminado correctamente.", "success");
                } else {
                    Swal.fire("Error", "No se pudo eliminar el proyecto.", "error");
                }
            });
        }
    })
}

function completarFormProyecto(datos){
    $('#idProyecto').val(datos.id);
    $('#txtNombreProyecto').val(datos.nombre);
    $('#txtCodigoProyecto').val(datos.codigoProyecto);
    $("#validtxtNombreProyecto").hide();
    $("#validtxtCodigoProyecto").hide();
}

function clearFormProyecto(){
    $('#idProyecto').val(0);
    $('#txtNombreProyecto').val('');
    $('#txtCodigoProyecto').val('');
    $("#validtxtNombreProyecto").hide();
    $("#validtxtCodigoProyecto").hide();
}


