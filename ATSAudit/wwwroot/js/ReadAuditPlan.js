var isManager = true;

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
            body: JSON.stringify("Closed"),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => response.json())
        .then(data => { })
        .then(() => {
            // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth))
            renderCalendar(currentDate);
            viewAuditPlan(args)
        })
        .catch(error => console.log(error));
    });

    $('#auditPlanManagerApproveAuditPlan').off('click');
    $('#auditPlanManagerApproveAuditPlan').click(e => {
        fetch(`/api/auditplans/${$('#readAuditPlanId').val()}`, {
            method: 'POST',
            body: JSON.stringify("Open"),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => response.json())
        .then(data => {
            // console.log(data)
            viewAuditPlan(args);
        })
        .then(() => {
            // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth));
            renderCalendar(currentDate);
        })
        .catch(error => console.log(error));
    });
    // $('#readAuditPlanModal .btn-close').click(() => {
    //     // $("#readAuditPlanModal").modal('hide');
    //     $('#conformitiesTable tbody').empty();
    // });
}

// async function renderConformitiesTable() {
//     $('#conformitiesTableBody').empty();

//     let conformities = await getConformitiesByPlanId($('#readAuditPlanId').val());

//     conformities.forEach(conformity => {
//         console.log(conformity.planId)
//                 $('#conformitiesTable').append(
//                     $('<tr>').append(
//                         $('<td>').attr("hidden", true).text(conformity.conformityId),
//                         $('<td>').attr("hidden", true).text(conformity.planId),
//                         $('<td>').text(conformity.conformityDescription),
//                         $('<td>').text(conformity.conformityAreaSection),
//                         $('<td>').data('conformityid', conformity.conformityId)
//                                  .append(
//                                     $('<button>', {type: 'button', class: 'btn btn-danger conformity-delete'}).append($('<i class="fa-solid fa-trash"></i>')),
//                                     $('<button>', {type: 'button', class: 'btn btn-secondary'}).append($('<i class="fa-solid fa-pen-to-square"></i>'))
//                                 )
//                     )
//                 )
//     });
    
//     if (conformities.length <= 0) {
//         $('#conformitiesTable').hide();
//         $('.emptyTable').show();
//     } else {
//         $('#conformitiesTable').show();
//         // $('#conformitiesTable').addClass('col-lg');
//         $('.emptyTable').hide();
//     }

//     //TODO: This is all still broken. No idea where I should put the data for the ConformityId. Make a new column maybe.
//     $('.conformity-delete').on('click', e => {
//         let conformityId = e.target.parentNode.dataset.conformityid/* .split('-')[1] */;

//         fetch('/api/conformities/' + conformityId, {
//             method: "DELETE",
//         })
//         .then(response => response.json())
//         .then(data => {
//             renderConformitiesTable();
//             console.log(data);
//         })
//         .catch(error => console.log(error));

//     });
// }


// function getConformitiesByPlanId(planId) {
//     return fetch("/api/auditplans/" + planId + "/conformities", {
//         method: "GET",
//         cache: "no-cache",
//         headers: {
//             "Content-Type": "application/json"
//         }
//     })
//     .then(response => response.json())
//     .then(data => { 
//         // console.log(data);
//         return data;
//     })
//     .catch(error => console.log(error));
// }

// $('#createConformitySubmit').on('click', e => {
//     // e.preventDefault();

//     let formData = {
//         PlanId: $('#readAuditPlanId').val(),
//         Description: $('#createConformityDescription').val(),
//         AreaSection: $('#createConformityAreaSection').val()
//     };

//     fetch("/api/conformities/", {
//         method: "POST",
//         body: JSON.stringify(formData),
//         headers: {
//             "Content-Type": "application/json"
//         }
//     })
//     .then(response => response.json())
//     // .then(data => console.log(data))
//     // .then(renderConformitiesTable())
//     .then(data => {
//         console.log(data);
//         renderConformitiesTable();
//     })
//     .catch(error => console.log(error));

//     // renderConformitiesTable();
// });

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
    })
    // .then(data => alert(data))
    .catch(error => console.log(error));

});


// $('#auditPlanManagerCloseAuditPlan').click(e => {
//     fetch(`/api/auditplans/${$('#readAuditPlanId').val()}`, {
//         method: 'POST',
//         body: JSON.stringify("Closed"),
//         headers: {
//             "Content-Type": "application/json"
//         }
//     })
//     .then(response => response.json())
//     .then(data => {
//         // viewAuditPlan(args);
//     })
//     .then(
//         renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth))
//     )
//     .catch(error => console.log(error));
// });

// $('#auditPlanManagerApproveAuditPlan').click(e => {
//     fetch(`/api/auditplans/${$('#readAuditPlanId').val()}`, {
//         method: 'POST',
//         body: JSON.stringify("Open"),
//         headers: {
//             "Content-Type": "application/json"
//         }
//     })
//     .then(response => response.json())
//     .then(data => {
//         // viewAuditPlan(args);
//     })
//     .then(() => {
//         renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth));
//     })
//     .catch(error => console.log(error));
// });

// $('#conformitiesTab').on('click', e => {
//     renderConformitiesTable();
// });

