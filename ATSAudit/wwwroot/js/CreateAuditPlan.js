function createAuditPlan(args) {
    // $("#inputDay").val(args.day);
    // $('#inputMonth').val(args.month);
    // $('#inputYear').val(args.year);

    // $("#createAuditPlanModal").modal('show');
    $('#createAuditPlanDepartments').html('<option selected="" hidden="" value="">Select Department</option>');
    $("#createAuditPlanDate").val(args.date);

    fetch("/api/departments")
    .then(response => response.json())
    .then(data => {
        data.forEach(department => {
            let option = $('<option>').text(department).val(department);
            $('#createAuditPlanDepartments').append(option);
        });
    })
    .catch(error => console.log(error));

    $('#createAuditPlanSubmit').off('click');
    $('#createAuditPlanSubmit').on('click', e => {
        let timeStart = new Date(args.date + " " + $('#createAuditPlanTimeStart').val()).toLocaleTimeString("en-US").replace(/AM|PM/, '');
        // console.log(timeStart)

        let body = JSON.stringify({
            requestor: "Rizzlord",                    
            department: $('#createAuditPlanDepartments').val(),
            auditCategory: $('#createAuditPlanAuditCategory').val(),
            // year: args.year,
            // month: args.month+1,
            // day: args.day,
            timeStart: timeStart,
            timeEnd: new Date(args.date + " " + $('#createAuditPlanTimeEnd').val()).toLocaleTimeString("en-US"),
            targetDate: new Date(args.date + " " + timeStart)./* toLocaleDateString("en-US") */toISOString()
        });

        console.log(body);

        fetch("/api/auditplans", {
            method: "POST",
            cache: "no-cache",
            body: body,
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(response => response.json())
        .then(data => {
            viewAuditPlan({ plan: data[0] });
        })
        .then(() => {
            displayViewAuditPlan();
            // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth))
            renderCalendar(currentDate);
            // console.log('log');
        })
        .catch(error => console.log(error));


        // renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth));
    });

}