$(function () {
    var a = new Date();
    $("#txtFechaDesde").val(a.toISOString().slice(0, a.toISOString().indexOf("T")));
    $("#txtFechaHasta").val(a.toISOString().slice(0, a.toISOString().indexOf("T")));
    fillSelectUsuarios($('#listarUsuarios').val(), "ddlFiltroUsuarios", "contenedorFiltros", "Todos", "nombre");

    $('#btnGenerarReporte').click(function () {
        obtenerReportePorDia()
    })
    
   
});


function obtenerReportePorDia() {
    $.ajax({
        type: 'GET',
        url: $("#ejecucionPorHora").val(),
        data: {
            fechaDesde:$('#txtFechaDesde').val(),
            fechaHasta:$('#txtFechaHasta').val(),
            usuarioId:$('#ddlFiltroUsuarios').val(),
            verCerrados:$('#ddlCerrados').val()
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
        hideSpinner();

        if (result.success) {
            $('#detalleReporte').html(JSON.stringify(result.data));
            var etiquetas = [];
            var ejecuciones = [];
            var bloqueos = [];
            for (var n = 0; n < result.data.detalleDiario.length; n++) {
                etiquetas.push(result.data.detalleDiario[n].fecha)
                ejecuciones.push(result.data.detalleDiario[n].totalEjecucion)
                bloqueos.push(result.data.detalleDiario[n].totalBloqueo)
            }

            graficoBarrasApiladas('graficoDiario', etiquetas, bloqueos, ejecuciones,null)
            graficoTorta('graficoTotales', ['Ejecucion', 'Bloqueo'],
                [result.data.totalEjecucion, result.data.totalBloqueo],
                ['#00a65a', '#f56954'])
        } else {
            Swal.fire("Información", "No se pudo obtener la tarea.", "error");
        }
    });
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
        hideSpinner();

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

