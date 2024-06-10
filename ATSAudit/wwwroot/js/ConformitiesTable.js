$('#conformitiesTab').on('click', () => { 
    renderConformitiesTable();
});

async function renderConformitiesTable() {
    $('#conformitiesTabPane').empty();
    $('#conformitiesTabPane').load(`?handler=Conformities&planId=${$('#readAuditPlanId').val()}`, () => { 
        $('.conformity-delete').on('click', e => {
            let conformityId = e.currentTarget.parentNode.dataset.conformityid/* .split('-')[1] */;

            fetch('/api/conformities/' + conformityId, {
                method: "DELETE",
            })
            .then(response => response.json())
            .then(data => { renderConformitiesTable() })
            .catch(error => console.log(error));
        });
    });
}

$('#createConformitySubmit').on('click', e => {
    let formData = {
        PlanId: $('#readAuditPlanId').val(),
        ConformityDescription: $('#createConformityDescription').val(),
        ConformityAreaSection: $('#createConformityAreaSection').val()
    };

    fetch("/api/conformities/", {
        method: "POST",
        body: JSON.stringify(formData),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => response.json())
    .then(data => {
        console.log(data);
        renderConformitiesTable();
    })
    .catch(error => console.log(error));
});