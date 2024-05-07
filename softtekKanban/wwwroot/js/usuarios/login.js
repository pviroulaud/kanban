$(function () {

    sessionStorage.clear();

    $("#btnLogin").click(function () {
        login();        
    });

});

function login(){
    sessionStorage.clear();
    if (validarFormLogin()) {
        
        showSpinner();
        var data = {            
            usuario: $("#txt_nombreUsuarioLogin").val(),
            pass: $("#txt_passUsuarioLogin").val(),            
        };

        $.ajax({
            type: 'POST',
            url: $("#login").val(),
            data: data,
            dataType: 'json'
        }).done(function (result) {
            hideSpinner();
            if (result.success) {
                clearFormLogin();
                guardarCredenciales(result.data)
                window.location.href=$('#home').val();
            } else {
                Swal.fire("Error", "Nombre de usuario y/o contraseña incorrectos." + result.error, "error");
            }
        });       
    }
}

function validarFormLogin(){
    var valid =true;
    var nombre = $("#txt_nombreUsuarioLogin").val().trim();
    if (nombre == "") {
        $("#validtxt_nombreUsuarioLogin").show();
        valid = false;
    } else {
        $("#validtxt_nombreUsuarioLogin").hide();
    }
    var pass = $("#txt_passUsuarioLogin").val().trim();
    if (nombre == "") {
        $("#validtxt_passUsuarioLogin").show();
        valid = false;
    } else {
        $("#validtxt_passUsuarioLogin").hide();
    }
    return valid;
}

function clearFormLogin(){

    $('#txt_nombreUsuarioLogin').val('');
    $('#txt_passUsuarioLogin').val('');
    $("#validtxt_nombreUsuarioLogin").hide();
    $("#validtxt_passUsuarioLogin").hide();
}

function guardarCredenciales(datosUsuario){
    sessionStorage.clear();
    sessionStorage.setItem("tkn", datosUsuario.tkn);
    sessionStorage.setItem("nUsuario", datosUsuario.usuario);
}
