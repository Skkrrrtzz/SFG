$('#readCPARCorrectionsTable > loading').hide();

async function renderCorrectionsTable() {
    $('#readCPARCorrectionsTable').load(`?handler=Corrections&cparId=${$('#readCPARId').val()}`, () => { 
        $('.uploadEvidenceButton').on('click', e => {
            $('#readCPAR').modal('toggle');

            //Setting form values so I don't have to render the whole form from the client side
            uploadEvidenceData( "CPARs",
                                "Corrections",
                                e.currentTarget.dataset.correctionId);

            // viewEvidence(   "CPARs",
            //                 "Corrections",
            //                 e.currentTarget.dataset.correctionId);
        });

        $('.viewEvidenceButton').on('click', e => {
            //Setting form values so I don't have to render the whole form from the client side
            viewEvidence(   "CPARs",
                            "Corrections",
                            e.currentTarget.dataset.correctionId);
        });

        $("#correctionsTable").DataTable({ 
            pageLength: 15, 
            lengthChange: false
        });
    });
}

function createCorrection() {
    renderCorrectionsTable();
}

function closeCreateCorrection() {
    console.log("ilovepimes")
    $("#createCorrection").modal('hide');
}


// $('#createCorrectionButton').on('click', e => {
//     $('#createCorrectionCPARId').val($('#readCPARId').val());
//     $('#readCPAR').modal('toggle');
// });

// $('#createCorrection button.btn-close').on('click', e => {
//     $('#readCPAR').modal('toggle');
// });

// $('#createCorrectionSubmit').on('click', e => {
//     $('#createCorrection').modal('toggle');

//     let formData = {
//         CPARId: $('#createCorrectionCPARId').val(),
//         CorrectionDescription: $('#createCorrectionDescription').val(),
//         EscapeCause: $('#createCorrectionEscapeCause').val(),
//         Action: $('#createCorrectionAction').val()
//     };

//     fetch("/api/corrections/", {
//         method: "POST",
//         body: JSON.stringify(formData),
//         headers: {
//             "Content-Type": "application/json"
//         }
//     })
//     .then(response => {
//         console.log(response)
//         renderCorrectionsTable();
//     })
//     .catch(error => console.log(error));
// });

