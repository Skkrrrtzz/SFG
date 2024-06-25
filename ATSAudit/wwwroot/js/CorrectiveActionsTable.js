async function renderCorrectiveActionsTable() {
    $('#readCPARCorrectiveActionsTable').load(`?handler=CorrectiveActions&cparId=${$('#readCPARId').val()}`, () => { 
        // $('.uploadEvidenceButton').on('click', e => {
        //     $('#readCPAR').modal('toggle');

        //     //Setting form values so I don't have to render the whole form from the client side
        //     uploadEvidenceData( "CPARs",
        //                         "CorrectiveActions",
        //                         e.currentTarget.dataset.correctiveActionId);
        // });

        $('.correctiveAction-delete').on('click', e => {
            let correctiveActionId = e.currentTarget.parentNode.dataset.correctiveactionid/* .split('-')[1] */;

            fetch('/api/correctiveactions/' + correctiveActionId, {
                method: "DELETE",
            })
            .then(response => response.json())
            .then(data => { renderCorrectiveActionsTable() })
            .catch(error => console.log(error));
        });


        $('.correctiveAction-close').on('click', e => {
            let correctiveActionId = e.currentTarget.parentNode.dataset.correctiveactionid/* .split('-')[1] */;

            fetch('/api/correctiveactions/' + correctiveActionId, {
                method: "PATCH",
            })
            .then(response => response.json())
            .then(data => { renderCorrectiveActionsTable() })
            .catch(error => console.log(error));
        });
    });

}

$('#createCorrectiveActionButton').on('click', e => {
    $('#createCorrectiveActionCPARId').val($('#readCPARId').val());
    $('#readCPAR').modal('toggle');
});

$('#createCorrectiveAction button.btn-close').on('click', e => {
    $('#readCPAR').modal('toggle');
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