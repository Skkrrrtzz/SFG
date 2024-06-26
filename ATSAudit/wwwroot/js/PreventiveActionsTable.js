async function renderPreventiveActionsTable() {
    $('#readCPARPreventiveActionsTable').load(`?handler=PreventiveActions&cparId=${$('#readCPARId').val()}`, () => { 
        $(".uploadEvidenceButton").on('click', e => {
            $('#readCPAR').modal('toggle');

            //Setting form values so I don't have to render the whole form from the client side
            uploadEvidenceData( "CPARs",
                                "CorrectiveActions",
                                e.currentTarget.dataset.correctiveActionId);
        });

        $("#preventiveActionsTable").DataTable({ pageLength: 15, lengthChange: false });
    });
}

function createPreventiveAction() {
    renderPreventiveActionsTable();
}

$('#createPreventiveActionButton').on('click', e => {
    $('#createPreventiveActionCPARId').val($('#readCPARId').val());
    $('#readCPAR').modal('toggle');
});

$('#createPreventiveAction button.btn-close').on('click', e => {
    $('#readCPAR').modal('toggle');
});

$('#createPreventiveActionSubmit').on('click', e => {
    $('#createPreventiveAction').modal('toggle');

    let formData = {
        CPARId: $('#createPreventiveActionCPARId').val(),
        PreventiveActionDescription: $('#createPreventiveActionDescription').val(),
        TargetDate: $('#createPreventiveActionTargetDate').val(),
        Responsible: $('#createPreventiveActionResponsible').val()
    };

    fetch("/api/preventiveactions/", {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        console.log(response)
        renderPreventiveActionsTable();
    })
    .catch(error => console.log(error));
});