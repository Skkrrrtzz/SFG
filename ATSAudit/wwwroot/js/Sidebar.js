function useStateLol() {
    $('.calendar-cell').on('click', (e) => {
        $('#calendar tbody > tr > td').removeClass('clicked');
        $(e.target).addClass('clicked');
        // $('#calendar tbody > tr > td').css('border', '');
        // $(e.target).css('border', '3px solid #0D6EFD');
        $('#sidepane').show();

        if (e.target.classList.contains('hasAuditPlan')) { 
            displayViewAuditPlan();
        } else if (!e.target.classList.contains('invalid')) {
            displayCreateAuditPlan();
        } else {
            $('#sidepane').hide();
        }
    });
}

function displayViewAuditPlan() {
    $('#readAuditPlan').css('display', 'block');
    $('#createAuditPlan').css('display', 'none');
}

function displayCreateAuditPlan() {
    $('#readAuditPlan').css('display', 'none');
    $('#createAuditPlan').css('display', 'block');
}

function keepClicked(cell) {
    $(cell).addClass('clicked');
}