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
    .then(response => console.log(response))
    // .then(data => {
    //     console.log(data);
    //     renderConformitiesTable();
    // })
    .catch(error => console.log(error));
});