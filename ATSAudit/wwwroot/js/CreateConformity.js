$('#createConformitySubmit').on('click', e => {
    let formData = {
        PlanId: $('#readAuditPlanId').val(),
        Description: $('#createConformityDescription').val(),
        AreaSection: $('#createConformityAreaSection').val()
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