var readCPARToggle = true;

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

async function readCPAR(e) {
    $('#readCPAR .modal-footer').attr('hidden', true);
    $('#readCPAR input').val('');

    let id = e.currentTarget.parentNode.dataset.cparid;

    let cpar = await fetch(`/api/CPARs/${id}`, { method: "GET" })
        .then(response => response.json())
        .then(data => data[0])
        .catch(error => console.log(error));

    $('#readCPARInitialIssuedTo').val(cpar.respondent);
    $('#readCPARInitialIssuedBy').val(cpar.requestor);
    $('#readCPARInitialIssuedDate').val(cpar.issuedDate);
    $('#readCPARInitialAuditDate').val(new Date(cpar.actualAuditDate).toLocaleDateString());
    $('#readCPARInitialResponseDate').val(new Date(cpar.responseDueDate).toLocaleDateString());
    $('#readCPARInitialCPARControlNo').val(cpar.cparId);
    $('#readCPARInitialISOClause').val(cpar.isoClause);
    $('#readCPARInitialProblemStatement').val(cpar.problemStatement);
    $('#readCPARFooterPreparedBy').val(cpar.preparedBy);

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

$('#readCPAR').resize(() => { resizeCPARTextareas() });