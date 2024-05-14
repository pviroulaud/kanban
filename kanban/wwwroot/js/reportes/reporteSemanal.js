$(function () {
    var a = new Date();
    $("#txtSemana").val(obtenerSemana(a));
    $("#txtSemanaHasta").val(obtenerSemana(a));
    

   

    fillSelectUsuarios($('#listarUsuarios').val(), "ddlFiltroUsuarios", "contenedorFiltros", "Todos", "nombre");

    $('#btnGenerarReporte').click(function () {
        obtenerReporteSemana()
    })
    
   
});


function obtenerSemana(fecha) {
    todaydate = fecha;

    //find the year of the current date  
    var oneJan = new Date(todaydate.getFullYear(), 0, 1);

    // calculating number of days in given year before a given date   
    var numberOfDays = Math.floor((todaydate - oneJan) / (24 * 60 * 60 * 1000));

    // adding 1 since to current date and returns value starting from 0   
    return fecha.getFullYear()+"-W"+ Math.ceil((todaydate.getDay() + 1 + numberOfDays) / 7);
}

function obtenerReporteSemana() {
    $('#validtxtSemana').hide();
    if (validarRangoFecha() == false) {
        $('#validtxtSemana').show();
        return;
    }
    var arr = "";
    for (var n = 0; n<$('#ddlFiltroUsuarios').val().length; n++) {
        arr += $('#ddlFiltroUsuarios').val()[n] + ",";
    }
    
    $.ajax({
        type: 'GET',
        url: $("#estimacionSemanal").val(),
        data: {
            semana: $('#txtSemana').val(),  
            semanaHasta: $('#txtSemanaHasta').val(),  
            usuarioId:arr
        },
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

        if (result.success) {
            if (result.data.length > 0) {
                $('#infoSemana').html($("#txtSemana").val().replaceAll("-W", "/") + " a " + $("#txtSemanaHasta").val().replaceAll("-W", "/"));
                $('#infoDesde').html(result.data[0].fechaDesde.slice(0, result.data[0].fechaDesde.indexOf("T")));
                $('#infoHasta').html(result.data[0].fechaHasta.slice(0, result.data[0].fechaHasta.indexOf("T")));

                
                
                

                var us = "";
                if ($('#ddlFiltroUsuarios').val().length == 0) {
                    us = "TODOS"
                }
                else {
                    for (var n = 0; n < $('#ddlFiltroUsuarios').val().length; n++) {
                        us += buscarEnSelect('ddlFiltroUsuarios', $('#ddlFiltroUsuarios').val()[n]) + ", "
                    }
                }
                $('#infoUsuarios').html(us);

                var totalHoras=detalleReporteSemana('detalleReporte', result.data)
                $('#infoEestimacion').html(totalHoras);
                $('#infoHsPeriodo').html(80 * result.data.length);

                graficoTorta('graficoTotales', ['Estimado', 'Sin Asignacion'],
                    [totalHoras, 80 * result.data.length],
                    ['#00a65a', '#f56954'])
            }

        } else {
            Swal.fire("Información", "No se pudo obtener la tarea.", "error");
        }
    });
}

function validarRangoFecha() {

    var d= parseInt($("#txtSemana").val().replaceAll("-W",""));
    var h = parseInt($("#txtSemanaHasta").val().replaceAll("-W", ""));
    if (d > h) {
        return false;
    }
    else {
        return true;
    }
}

function detalleReporteSemana(contenedor, data) {
    var _html = '';
    var total = 0;
    for (var n = 0; n < data.length; n++) {
        for (var j = 0; j < data[n].detalle.length; j++) {
            _html += '<tr>'
            if (j == 0) {
                _html += '  <td style="vertical-align: middle;" rowspan="' + data[n].detalle.length + '">' + data[n].semanaEjecucionPlanificada + '</td>'
            }
            total += data[n].detalle[j].estimacion;

            _html += '  <td>' + data[n].detalle[j].nombreIncidencia + '</td>'
            _html += '  <td>' + data[n].detalle[j].nombreTarea + '</td>'
            _html += '  <td>' + buscarEnSelect('ddlFiltroUsuarios', data[n].detalle[j].usuarioResponsableId) + '</td>'
            _html += '  <td>' + data[n].detalle[j].estimacion + '</td>'


            if (j == 0) {
                _html += '  <td style="vertical-align: middle;" rowspan="' + data[n].detalle.length + '">' + data[n].estimacionTotal + '</td>'
            }

            _html += '</tr>'
        }
    }
   

        
        
    
    $('#tabla').html(_html)
    return total;
    //rowspan="' + data.detalleDiario[n].tareas.length + '"

    //$('#' + contenedor).html(JSON.stringify(data));
}

function graficoBarrasApiladas(idCanvasConenedor, etiquetas, datosA,datosB, colores) {
    var areaChartData = {
        labels: etiquetas,
        datasets: [
            {
                label: 'Bloqueos',
                backgroundColor: 'rgba(245,105,84,0.8)',
                borderColor: 'rgba(60,141,188,0.8)',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data: datosA
            },
            {
                label: 'Ejecutado',
                backgroundColor: 'rgba(00, 166, 90, 0.65)',
                borderColor: 'rgba(210, 214, 222, 1)',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: datosB
            },
        ]
    }


    var barChartData = $.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    var temp1 = areaChartData.datasets[1]
    barChartData.datasets[0] = temp1
    barChartData.datasets[1] = temp0
    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var stackedBarChartCanvas = $('#' + idCanvasConenedor).get(0).getContext('2d')
    var stackedBarChartData = $.extend(true, {}, barChartData)

    var stackedBarChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                stacked: true,
            }],
            yAxes: [{
                stacked: true
            }]
        }
    }

    new Chart(stackedBarChartCanvas, {
        type: 'bar',
        data: stackedBarChartData,
        options: stackedBarChartOptions
    })
}

function graficoTorta(idCanvasConenedor,etiquetas,datos,colores) {
    var donutData = {
        labels: etiquetas,
        datasets: [
            {
                data: datos,
                backgroundColor: colores,
            }
        ]
    }

    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $('#' + idCanvasConenedor).get(0).getContext('2d')
    var pieData = donutData;
    var pieOptions = {
        maintainAspectRatio: false,
        responsive: true,
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    new Chart(pieChartCanvas, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    })
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
        hideSpinner();borrarSomee();borrarSomee();

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