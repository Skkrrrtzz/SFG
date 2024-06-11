async function renderPreventiveActionsTable() {
    $('#readCPARPreventiveActionsTable').empty();
    $('#readCPARPreventiveActionsTable').load(`?handler=PreventiveActions&cparId=${$('#readCPARId').val()}`, () => { 
    });
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