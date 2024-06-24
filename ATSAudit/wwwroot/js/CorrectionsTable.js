$('#readCPARCorrectionsTable > loading').hide();

async function renderCorrectionsTable() {
    $('#readCPARCorrectionsTable').load(`?handler=Corrections&cparId=${$('#readCPARId').val()}`, () => { 
        $('.uploadEvidenceButton').on('click', e => {
            $('#readCPAR').modal('toggle');
            $("#uploadEvidenceId").val(e.currentTarget.dataset.correctionId);
        });
    });
    // fetch(`?handler=Corrections&cparId=${$('#readCPARId').val()}`)
    // .then(response => $('#readCPARCorrectionsTable').load(response));
}

$('#createCorrectionButton').on('click', e => {
    $('#createCorrectionCPARId').val($('#readCPARId').val());
    $('#readCPAR').modal('toggle');
});

$('#createCorrection button.btn-close').on('click', e => {
    $('#readCPAR').modal('toggle');
});

$('#createCorrectionSubmit').on('click', e => {
    $('#createCorrection').modal('toggle');

    let formData = {
        CPARId: $('#createCorrectionCPARId').val(),
        CorrectionDescription: $('#createCorrectionDescription').val(),
        EscapeCause: $('#createCorrectionEscapeCause').val(),
        Action: $('#createCorrectionAction').val()
    };

    fetch("/api/corrections/", {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        console.log(response)
        renderCorrectionsTable();
    })
    .catch(error => console.log(error));
});

