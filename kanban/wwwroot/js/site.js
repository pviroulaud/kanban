function genericErrorMsg() { Swal.fire("Error", "Ocurrio un error al obtener los datos, contacte al soporte de la aplicación", "error");}
function genericErrorDatoABMMsg() { Swal.fire("Error", "Ocurrio un error, verifique los datos ingresados", "error"); }

var globalSpinner=[];

function showSpinner() { $("#spinner").show(); globalSpinner.push(1); }
function hideSpinner() { globalSpinner.pop(); if (globalSpinner.length == 0) { $("#spinner").hide(); } }

function getData(url,data, callback) {
    showSpinner();
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
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
            callback(result.data);
        } else {
            genericErrorMsg();
        }
    });
}

function fillSelect(url, selectId,modalId,emptyOpt,textProp ) {
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
        hideSpinner();
        if (result.success) {
            $("#" + selectId).empty();
            if (emptyOpt != null && emptyOpt != "")
                $("#" + selectId).append($("<option></option>").val('').html(emptyOpt));

            $.each(result.data, function (data, value) {
                $("#" + selectId).append($("<option></option>").val(value.id).html(Reflect.get(value, textProp)));
            });
            if (modalId!=null && modalId!="")
                $("#" + selectId).select2({ dropdownParent: $("#" + modalId) });

        } else {
            genericErrorMsg();
        }
    });
}

(function ($) {
    /**/
    $.ajax({
        type: 'GET',
        url: $("#menuContainerURL").val(),
        cache: false,
        dataType: 'html'
    }).done(function (result) {
        $("#menuContainer").html(result);
    });
    /* Initialize basic datatable */
    $.fn.DataTableEdit = function ($options) {
        var options = $.extend({
            language: {
                "decimal": ",",
                "emptyTable": "No hay información",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
                "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
                "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                "infoPostFix": "",
                "thousands": ".",
                "lengthMenu": "Mostrar _MENU_ Entradas",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "Sin resultados encontrados",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                "buttons": {
                    "copy": "Copiar",
                    "csv": "CSV",
                    "excel": "Excel",
                    "pdf": "PDF",
                    "print": "Imprimir"
                }
            },
            searching: false,
            ordering: false,
            lengthChange: false,
            responsive: true,
            serverSide: true,
            altEditor: false,
            processing: true,
            pageLength: 50,
            pagingType: "full_numbers",
            select: { style: "none" }

        }, $options);

        return $(this).DataTable(options).on('init.dt', function () {
            $("span[data-role=filter]").off().on("click", function () {
                const search = $(this).data("filter");
                if (table)
                    table.search(search).draw();
            });
        });
    };
}(jQuery));
