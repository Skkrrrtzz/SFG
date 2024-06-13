async function renderCorrectiveActionsTable() {
    $('#readCPARCorrectiveActionsTable').empty();
    $('#readCPARCorrectiveActionsTable').load(`?handler=CorrectiveActions&cparId=${$('#readCPARId').val()}`, () => { 
    });
}

$('#createCorrectiveActionButton').on('click', e => {
    $('#createCorrectiveActionCPARId').val($('#readCPARId').val());
    $('#readCPAR').modal('toggle');
});

$('#createCorrectiveAction button.btn-close').on('click', e => {
    $('#readCPAR').modal('toggle');
});

$('.correctiveAction-delete').on('click', e => {
    let correctiveActionId = e.currentTarget.parentNode.dataset.correctiveactionid/* .split('-')[1] */;

    fetch('/api/correctiveactions/' + correctiveActionId, {
        method: "DELETE",
    })
    .then(response => response.json())
    .then(data => { renderConformitiesTable() })
    .catch(error => console.log(error));
});

$('#createCorrectiveActionSubmit').on('click', e => {
    $('#createCorrectiveAction').modal('toggle');

    let formData = {
        CPARId: $('#createCorrectiveActionCPARId').val(),
        CorrectiveActionDescription: $('#createCorrectiveActionDescription').val(),
        TargetDate: $('#createCorrectiveActionTargetDate').val(),
        Responsible: $('#createCorrectiveActionResponsible').val()
    };

    fetch("/api/correctiveactions/", {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        console.log(response)
        renderCorrectiveActionsTable();
    })
    .catch(error => console.log(error));
});