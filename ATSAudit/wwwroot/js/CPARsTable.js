// let cpars = [];

async function renderCPARsTable() {
    $('#cparsTableBody').empty();
    $('#conformitiesTable').show();
    $('.emptyTable').show();

    let cpars = await getCPARsByPlanId($('#readAuditPlanId').val());
    console.log(`cpars: ${cpars}`);

    cpars.forEach(cpar => {
        $('#cparsTableBody').append(
            $('<tr>').append(
                $('<td>').attr('hidden', true).text(cpar.cparId),
                $('<td>').attr('hidden', true).text(cpar.planId),
                $('<td>').text(cpar.respondent),
                $('<td>').text(cpar.requestor),
                $('<td>').text(cpar.responseDueDate),
                // $('<td>').text(cpar.problemStatement),
                // $('<td>').text(cpar.preparedBy),
                $('<td>').attr('data-cparid', cpar.cparId)
                            .append(
                                    $('<button>', {type: 'button', class: 'btn btn-primary cpar-view', 'data-bs-toggle': 'modal', 'data-bs-target': '#readCPAR'}).append($('<i class="fa-solid fa-pen-to-square"></i>')),
                                    $('<button>', {type: 'button', class: 'btn btn-danger cpar-delete'}).append($('<i class="fa-solid fa-trash"></i>'))
                                )
            )
        );
    });
    
    if (cpars.length > 0) {
        $('#cparsTable').show();
        $('.emptyTable').hide();
    } else {
        $('#cparsTable').hide();
        $('.emptyTable').show();
    }

    // TODO: This is all still broken. No idea where I should put the data for the cparId. Make a new column maybe.
    $('.cpar-delete').on('click', e => {
        let cparId = e.currentTarget.parentNode.dataset.cparid/* .split('-')[1] */;

        fetch('/api/cpars/' + cparId, {
            method: "DELETE",
        })
        .then(response => response.json())
        .then(data => {
            rendercparsTable();
            console.log(data);
        })
        .catch(error => console.log(error));

    });

    $('.cpar-view').off('click');
    $('.cpar-view').on('click', async e => {
        $('#readCPARInitial > input').val('');

        let id = e.target.parentNode.dataset.cparid;
        console.log(id);

        let cpar = await fetch(`/api/CPARs/${id}`, { method: "GET" })
            .then(response => response.json())
            .then(data => data[0])
            .catch(error => console.log(error));

        console.log(cpar);

        $('#readCPARInitialIssuedTo').val(cpar.respondent);
        $('#readCPARInitialIssuedBy').val(cpar.requestor);
        $('#readCPARInitialIssuedDate').val(cpar.issuedDate);
        $('#readCPARInitialAuditDate').val("Default Value");
        $('#readCPARInitialResponseDate').val(cpar.responseDueDate);
        $('#readCPARInitialCPARControlNo').val(cpar.cparId);
        $('#readCPARInitialISOClause').val("Default Value");
        $('#readCPARInitialProblemStatement').val(cpar.problemStatement);
        $('#readCPARFooterPreparedBy').val(cpar.preparedBy);
    });

    // $('.cparActionView > *').click(e => e.stopPropagation());
}

$('#createCPAREdit').on('click', e => {
    $('#readCPAR [readonly]').prop('readonly', false);
});

$('#createCPARSubmit').on('click', e => {
    let formData = {
        PlanId: $('#readAuditPlanId').val(),
        Respondent: $('#createCPARRespondent').val(),
        Requestor: $('#createCPARRequestor').val(),
        ResponseDueDate: $('#createCPARResponseDueDate').val(),
        ProblemStatement: $('#createCPARProblemStatement').val(),
        PreparedBy: $('#createCPARPreparedBy').val()
    };

    console.log(formData);

    fetch("/api/cpars/", {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        console.log(response)
        renderCPARsTable();
    })
    .catch(error => console.log(error));
});

function getCPARsByPlanId(planId) {
    return fetch("/api/auditplans/" + planId + "/cpars", {
        method: "GET",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => response.json())
    .then(cpars => { 
        // if (cpars.length > 0) {
        //     $('#cparsTable').show();
        //     $('.emptyTable').hide();
        // } else {
        //     $('#cparsTable').hide();
        //     $('.emptyTable').show();
        // }

        return cpars;
    })
    .catch(error => console.log(error));
}

$('#cparTab').on('click', e => renderCPARsTable());``

