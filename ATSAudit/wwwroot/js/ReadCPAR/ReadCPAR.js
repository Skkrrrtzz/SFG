// renderCorrectionsTable();
let cparId = $("#cparId").val();

$('#readCPARCorrectionsTable').load(`?handler=Corrections&cparId=${cparId}`, () => {
    $('#readCPARCorrectionsTable .uploadEvidenceButton').on('click', e => {
        $('#readCPAR').modal('toggle');

        //Setting form values so I don't have to render the whole form from the client side
        uploadEvidenceData( "CPARs",
                            "Corrections",
                            e.currentTarget.dataset.correctionId);
    });
});
$('#readCPARCorrectiveActionsTable').load(`?handler=CorrectiveActions&cparId=${cparId}`, () => {
    $('#readCPARCorrectiveActionsTable .uploadEvidenceButton').on('click', e => {
        $('#readCPAR').modal('toggle');

        //Setting form values so I don't have to render the whole form from the client side
        uploadEvidenceData( "CPARs",
                            "CorrectiveActions",
                            e.currentTarget.dataset.correctiveActionId);
    });
});
$('#readCPARPreventiveActionsTable').load(`?handler=PreventiveActions&cparId=${cparId}`);