var isManager = true; //TODO: Replace with role

function viewAuditPlan(args) {
    $('#readAuditPlanTables').hide();
    $('#auditPlanManagerApproval').hide();
    $('#auditPlanManagerCloseAuditPlan').hide();
    $('#auditPlanManagerApproveAuditPlan').hide();

    $('#readAuditPlanId').val(args.plan.planId);
    $("#readAuditPlanStatus").val(
        args.plan.status == 0 ? "For Approval" :
        args.plan.status == 1 ? "Open" :
        args.plan.status == 2 ? "Closed" :
        "Error"
    );
    $("#readAuditPlanDepartment").val(args.plan.department);
    $("#readAuditPlanAuditCategory").val(args.plan.auditCategory);
    $("#readAuditPlanTargetDate").val(new Date(args.plan.targetDate).toLocaleDateString());
    $("#readAuditPlanTimeStart").val(new Date(args.plan.targetDate).toLocaleTimeString());
    $("#readAuditPlanAuditorApproved").val(args.plan.auditorApproved ? "Approved" : "Not Approved");
    $("#readAuditPlanAuditeeApproved").val(args.plan.auditeeApproved ? "Approved" : "Not Approved");

    if (args.plan.status == 2) { //Closed
        $('#readAuditPlanTables').show();
        renderConformitiesTable();
        renderCPARsTable();
    } else {
        if (isManager) {
            $('#auditPlanManagerApproval').show();
            if (args.plan.status == 0) { //For Approval
                $('#auditPlanManagerApproveAuditPlan').show();
            } else if (args.plan.status == 1) { //Open
                $('#auditPlanManagerCloseAuditPlan').show();
            }
        }
    }

    $('#auditPlanManagerCloseAuditPlan').off('click');
    $('#auditPlanManagerCloseAuditPlan').click(e => {
        fetch(`/api/auditplans/${$('#readAuditPlanId').val()}`, {
            method: 'POST',
            body: JSON.stringify({ 
                status: "Closed",
                actualAuditDate: new Date()
             }),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => response.json())
        // .then(data => { })
        .then(() => {
            // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth))
            displayViewAuditPlan();
            renderCalendar(currentDate);
            viewAuditPlan(args)
        })
        .catch(error => console.log(error));
    });

    $('#auditPlanManagerApproveAuditPlan').off('click');
    $('#auditPlanManagerApproveAuditPlan').click(e => {
        fetch(`/api/auditplans/${$('#readAuditPlanId').val()}`, {
            method: 'POST',
            body: JSON.stringify({ status: "Open" }),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => response.json())
        // .then(data => console.log(data))
        .then(() => {
            renderCalendar(currentDate);
            viewAuditPlan(args)
        })
        .catch(error => console.log(error));
    });
}

$('#readAuditPlanDelete').on('click', e => {
    console.log($('#readAuditPlanId').val());
    fetch("/api/auditplans/" +  $('#readAuditPlanId').val(), {
        method: "DELETE",
    })
    .then(response => {
        console.log(response);
        response.json();
    })
    .then(() => {
        // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth))
        renderCalendar(currentDate);
        viewAuditPlan(args)
    })
    // .then(data => alert(data))
    .catch(error => console.log(error));

});

