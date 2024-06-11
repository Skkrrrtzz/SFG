var readCPARToggle = true;

async function renderCPARsTable() {
    $('#cparsTableBody').empty();
    $('#conformitiesTable').show();
    $('.emptyTable').show();

    $('#cparTabPane').load(`?handler=CPARs&planId=${$('#readAuditPlanId').val()}`, () => {
        $('.cpar-view').on('click',  e => { readCPAR(e) });
    })

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

}

async function readCPAR(e) {
    $('#readCPAR .modal-footer').attr('hidden', true);
    $('#readCPAR input').val('');

    let id = e.currentTarget.parentNode.dataset.cparid;

    let cpar = await fetch(`/api/CPARs/${id}`, { method: "GET" })
        .then(response => response.json())
        .then(data => data[0])
        .catch(error => console.log(error));

    $('#readCPARId').val(cpar.cparId);
    $('#readCPARInitialIssuedTo').val(cpar.respondent);
    $('#readCPARInitialIssuedBy').val(cpar.requestor);
    $('#readCPARInitialIssueDate').val(new Date(cpar.issueDate).toLocaleDateString());
    $('#readCPARInitialAuditDate').val(new Date(cpar.actualAuditDate).toLocaleDateString());
    $('#readCPARInitialResponseDate').val(new Date(cpar.responseDueDate).toLocaleDateString());
    $('#readCPARInitialCPARControlNo').val(cpar.cparId);
    $('#readCPARInitialISOClause').val(cpar.isoClause);
    $('#readCPARInitialProblemStatement').val(cpar.problemStatement);
    $('#readCPARFooterPreparedBy').val(cpar.preparedBy);

    $('#correctionsTable').empty();
    $('#readCPARCorrectionsTable').load(`?handler=Corrections&cparId=${id}`);
    $('#readCPARCorrectiveActionsTable').load(`?handler=CorrectiveActions&cparId=${id}`);
    $('#readCPARPreventiveActionsTable').load(`?handler=PreventiveActions&cparId=${id}`);

    resizeCPARTextareas();
}

function resizeCPARTextareas() {
    $('#readCPARInitialISOClause')
        .css('height', 'auto') 
        .css('max-height', '100%') 
        .css('height', $('#readCPARInitialISOClause').prop('scrollHeight') + 'px' )

    $('#readCPARInitialProblemStatement').css('height', 'auto').css('max-height', '100%') 
    $('#readCPARInitialProblemStatement').css('height', $('#readCPARInitialProblemStatement').prop('scrollHeight') + 'px' )
}

$('#readCPAREdit').click(e => {
    // $('#readCPAR .modal-footer').hide();
    if (readCPARToggle) {
        $('#readCPAR .modal-footer').attr('hidden', false);
        $('#readCPAR .respondent input').attr('readonly', false);
        readCPARToggle = false;
    } else {
        $('#readCPAR .modal-footer').attr('hidden', true);
        $('#readCPAR .respondent input').attr('readonly', true);
        readCPARToggle = true;
    }
});

$('#readCPARClose').click(e => { 
    $('#readCPAR .modal-footer').attr('hidden', true);
    $('#readCPAR .respondent input').attr('readonly', true);
    readCPARToggle = true;
 })


// Create CPAR

$('#createCPAREdit').on('click', e => {
    $('#readCPAR [readonly]').prop('readonly', false);
});

$('#createCPARSubmit').on('click', e => {
    let formData = {
        PlanId: $('#readAuditPlanId').val(),
        Respondent: $('#createCPARRespondent').val(),
        Requestor: $('#createCPARRequestor').val(),
        IssueDate: new Date().toISOString().substring(0,10),
        ResponseDueDate: $('#createCPARResponseDueDate').val(),
        ISOClause: $('#createCPARISOClause').val(),
        ProblemStatement: $('#createCPARProblemStatement').val(),
        PreparedBy: $('#createCPARPreparedBy').val()
    };

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

$('#cparTab').on('click', e => { renderCPARsTable() });

