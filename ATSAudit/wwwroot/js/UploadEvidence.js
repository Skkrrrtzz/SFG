//Close the modal and let Bootstrap Modal logic handle the rest
$("#uploadEvidenceSubmit").on('click', () => {
    $("#uploadEvidence").modal('toggle');
});

//Triggers whenever the form is submitted (currently via Unobtrusive AJAX)
function uploadEvidenceComplete(xhr, status) {
    alert(xhr.responseText);
    $("#uploadEvidenceForm")[0].reset();
    $("#attachedFiles").empty();
}

//When files are uploaded, create anchor tags to preview (like Schoolbook hehe)
$("#uploadEvidenceInput").on('change', () => {
    const attachedFiles = document.querySelector("#attachedFiles");
    const uploadInput = document.querySelector("#uploadEvidenceInput");
    const currFiles = uploadInput.files;

    attachedFiles.innerHTML = '<label class="form-text"><i class="fa fa-file"></i> Files to Upload</label>';

    for (const file of currFiles) {
        const anchor = document.createElement("a");
        anchor.href = URL.createObjectURL(file);
        anchor.textContent = anchor.title = file.name;
        anchor.target = "_blank";

        anchor.dataset.bsToggle = "tooltip";
        anchor.title =  "Click to preview file";
        document.querySelector("#attachedFiles").appendChild(anchor);
    }
});

//Setting form values so I don't have to render the whole form from the client side
//Experimental, as this approach assumes that the form is rendered server-side ann only the values need to be set
function uploadEvidenceData(formName, subformName, id) {
    $("#uploadEvidenceForForm").val(formName);
    $("#uploadEvidenceForSubform").val(subformName);
    $("#uploadEvidenceForId").val(id);
}

//Failed attempt to make upload ACID-compliant (not compatible with Unobtrusive AJAX)
// $("#uploadEvidenceSubmit").on('click', () => {
//     const uploadInput = document.querySelector("#uploadEvidenceInput");
//     const currFiles = uploadInput.files;

//     if (currFiles.length > 0) {
//         for (let i = 0; i < currFiles.length; i++) {
//             fetch(`?handler=UploadEvidence`, {
//                 method: "POST",
//                 body: new FormData().append('evidence', currFiles[i])
//             })
//             .then(response => alert(`${response.status}: ${response.text}`))
//             .catch(response => alert(`${response.status}: ${response.text}`));
//         };
//     } else {
//         alert("No files uploaded!");
//     }
// });