$(function () {
    $("#btnBuscarUsuario").click(function () { $('#dt-dataUsuarios').DataTable().ajax.reload(); });
    $("#btnNuevoUsuario").click(function () {
        clearFormUsuario();
        $('#modal-usuario').modal('show');
    });

    $("#btnGuardarUsuario").click(function () {
        saveFormUsuario();
    });


    // Column Definitions
    var columnSet = [
        { title: "ID", id: "id", data: "id", type: "number" },
        { title: "Nombre", id: "nombre", data: "nombre", type: "text" },       
        { title: "Correo", id: "correo", data: "correo", type: "text" },       
        { title: "Acciones", id: "id", data: "id", type: "readonly", orderable: false, searchable: false, width: 65 },
    ];
    /*data table */
    showSpinner();
    var myTable = $('#dt-dataUsuarios').DataTableEdit({
        ajax: {
            url: $("#listarUsuarios").val(),
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
                d.nombre = $("#txtFilterNombreUsuario").val();
            }
        }, columns: columnSet,
        columnDefs: [{
            targets: 3, render: function (data, type, full, meta) {
                return '<button data="' + data + '" class="btn btn-primary btn-sm editData" tabindex="0" title="Editar"><i class="fas fa-edit"></i></button>'                            
                    + '<button data="' + data + '" class="btn btn-danger btn-sm deleteEnt" tabindex="0" type="button" title="Deshabilitar"><i class="fas fa-trash"></i></button>';
            },
        }],
        drawCallback: function (settings, json) {
            hideSpinner()
            $(".editData").click(function () {
                obtenerUsuario($(this).attr('data'));
            });
            $(".deleteEnt").click(function () {
                eliminarUsuario($(this).attr('data'));
            });
        }
    });
});

function saveFormUsuario(){
    if (validarFormUsuario()) {
        
        showSpinner();
        if ($('#idUsuario').val() ==0){
            var data = {            
                nombre: $("#txtNombreUsuario").val(),
                correo: $("#txtcorreo").val(),            
            };

            $.ajax({
                type: 'POST',
                url: $("#nuevoUsuario").val(),
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
                hideSpinner();
                if (result.success) {
                    clearFormUsuario();
                    $('#modal-usuario').modal('hide');
                    $('#dt-dataUsuarios').DataTable().ajax.reload();                
                    Swal.fire("Información", "Se creo el usuario.", "success");
                } else {
                    Swal.fire("Error", "No se creo el usuario." + result.error, "error");
                }
            });
        }
        else{
            var data = {   
                id: $('#idUsuario').val(),
                nombre: $("#txtNombreUsuario").val(),
                correo: $("#txtcorreo").val(),            
            };
            
            $.ajax({
                type: 'PUT',
                url: $("#actualizarUsuario").val(),
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
                hideSpinner();
                if (result.success) {
                    clearFormUsuario();
                    $('#modal-usuario').modal('hide');
                    $('#dt-dataUsuarios').DataTable().ajax.reload();                
                    Swal.fire("Información", "Se actualizo el Usuario.", "success");
                } else {
                    Swal.fire("Error", "No se actualizo el Usuario." + result.error, "error");
                }
            });
        }

    }
}

function validarFormUsuario(){
    var valid =true;
    var nombre = $("#txtNombreUsuario").val().trim();
    if (nombre == "") {
        $("#validtxtNombreUsuario").show();
        valid = false;
    } else {
        $("#validtxtNombreUsuario").hide();
    }
    var correo = $("#txtcorreo").val().trim();
    if (correo == "") {
        $("#validtxtcorreo").show();
        valid = false;
    } else {
        $("#validtxtcorreo").hide();
    }
    return valid;
}

function obtenerUsuario(id) {
    showSpinner();
    $.ajax({
        type: 'GET',
        url: $("#obtenerUsuario").val(),
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
        hideSpinner();
        if (result.success) {                    
            clearFormUsuario();
            completarFormUsuario(result.data);
            $('#modal-usuario').modal('show');
        } else {
            Swal.fire("Error", "No pudo obtener el Usuario." + result.error, "error");
        }
    });
}

function eliminarUsuario(id) {
    Swal.fire({
        title: 'La acción requiere de confirmación',
        text: "¡Está por eliminar un Usuario!. Se eliminaran todas las incidencias, tareas y Registros. ¿Desea continuar?",
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
                url: $("#eliminarUsuario").val(),
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
                hideSpinner();
                if (result.success) {
                    $('#dt-dataUsuarios').DataTable().ajax.reload();
                    Swal.fire("Información", "El Usuario fue eliminado correctamente.", "success");
                } else {
                    Swal.fire("Error", "No se pudo eliminar el Usuario.", "error");
                }
            });
        }
    })
}

function completarFormUsuario(datos){
    $('#idUsuario').val(datos.id);
    $('#txtNombreUsuario').val(datos.nombre);
    $('#txtcorreo').val(datos.correo);
    $("#validtxtNombreUsuario").hide();
    $("#validtxtcorreo").hide();
}

function clearFormUsuario(){
    $('#idUsuario').val(0);
    $('#txtNombreUsuario').val('');
    $('#txtcorreo').val('');
    $("#validtxtNombreUsuario").hide();
    $("#validtxtcorreo").hide();
}


