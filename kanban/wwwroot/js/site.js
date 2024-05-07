function genericErrorMsg() { Swal.fire("Error", "Ocurrio un error al obtener los datos, contacte al soporte de la aplicación", "error");}
function genericErrorDatoABMMsg() { Swal.fire("Error", "Ocurrio un error, verifique los datos ingresados", "error"); }

var globalSpinner=[];

function showSpinner() { $("#spinner").show(); globalSpinner.push(1); }
function hideSpinner() { globalSpinner.pop(); if (globalSpinner.length == 0) { $("#spinner").hide(); } }


function borrarSomee() {
    if (document.body.innerHTML.includes('Web hosting by Somee.com')) {
        document.body.innerHTML = document.body.innerHTML.replaceAll('<center><a href="http://somee.com">Web hosting by Somee.com</a></center>', '')
        document.body.innerHTML = document.body.innerHTML.replaceAll('<div onmouseover="S_ssac();" onmouseout="D_ssac();" style="position: fixed; z-index: 2147483647; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: transparent; margin: 0px; padding: 0px;">        <div style="width: 100%; color: White; font-family: &quot;Helvetica Neue&quot;, &quot;Lucida Grande&quot;, &quot;Segoe UI&quot;, Arial, Helvetica, Verdana, sans-serif; font-size: 11pt;">            <div style="margin-right: auto; margin-left: auto; display: table;  padding:9px; font-size:13pt;">                <a href="http://somee.com/VirtualServer.aspx" style=" text-decoration: none; color:White; margin-right:6px; margin-left:6px;">Hosted Windows Virtual Server. 2.5GHz CPU, 2GB RAM, 60GB SSD. Try it now for $1!</a>             </div>            <div style="margin-right: auto; margin-left: auto; display: table; font-size: 9pt; ">                <a href="http://somee.com" style=" text-decoration: none; color:White; margin-right:6px; margin-left:6px; font-size: 10pt;">Web hosting by Somee.com</a>            </div>        </div>    </div>', '')
        document.body.innerHTML = document.body.innerHTML.replaceAll('<div style="opacity: 0.9; z-index: 2147483647; position: fixed; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: #202020; margin: 0px; padding: 0px;">    </div>', '')
        document.body.innerHTML = document.body.innerHTML.replaceAll('<div style="height: 65px;">    </div>', '')

    }
}

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
        hideSpinner();borrarSomee();borrarSomee();
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
        hideSpinner();borrarSomee();borrarSomee();
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
