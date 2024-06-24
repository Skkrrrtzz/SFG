// renderCorrectionsTable();

$('#readCPARCorrectionsTable').load(`?handler=Corrections&cparId=14`, () => {
    $('.uploadEvidenceButton').on('click', e => {
        $('#readCPAR').modal('toggle');
        $("#uploadEvidenceId").val(e.currentTarget.dataset.correctionId);
    });
});
$('#readCPARCorrectiveActionsTable').load(`?handler=CorrectiveActions&cparId=14`);
$('#readCPARPreventiveActionsTable').load(`?handler=PreventiveActions&cparId=14`);