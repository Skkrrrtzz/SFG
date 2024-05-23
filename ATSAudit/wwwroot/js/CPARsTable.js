async function renderCPARsTable() {
    $('#cparsTableBody').empty();

    let cpars = await getCPARsByPlanId($('#readAuditPlanId').val());
    console.log(`cpars: ${cpars}`);

    cpars.forEach(cpar => {
        $('#cparsTableBody').append(
            `<tr>` +
                '<td hidden>' + cpar.cparId + '</td>' +
                '<td hidden>' + cpar.planId + '</td>' +
                '<td>' + cpar.respondent + '</td>' +
                '<td>' + cpar.requestor + '</td>' +
                '<td>' + cpar.responseDueDate + '</td>' +
                '<td>' + cpar.problemStatement + '</td>' +
                '<td>' + cpar.preparedBy + '</td>' +
                // `<td data-cparid=${cpar.cparId}>` + 
                //     `<button type="button" class="btn btn-danger cpar-delete">
                //         <i class="fa-solid fa-trash"></i>
                //     </button>` +
                //     `<button type="button" class="btn btn-secondary">
                //         <i class="fa-solid fa-pen-to-square"></i>
                //     </button>` +
                // '</td>' +
            '</tr>'
        );
    });
    
    if (cpars.length <= 0) {
        $('#cparsTable').hide();
        $('.emptyTable').show();
    } else {
        $('#cparsTable').show();
        // $('#cparsTable').addClass('col-lg');
        $('.emptyTable').hide();
    }

    //TODO: This is all still broken. No idea where I should put the data for the cparId. Make a new column maybe.
    $('.cpar-delete').on('click', e => {
        let cparId = e.target.parentNode.dataset.cparid/* .split('-')[1] */;

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
}

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
    // .then(data => {
    //     console.log(data);
    //     rendercparsTable();
    // })
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
    .then(data => { 
        // console.log(data);
        return data;
    })
    .catch(error => console.log(error));
}

$('#cparTab').on('click', e => {
    renderCPARsTable();
});