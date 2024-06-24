$("#uploadEvidenceSubmit").on('click', () => {
    $("#uploadEvidence").modal('toggle');
});

function uploadEvidence(xhr, status) {
    alert(xhr.responseText);
    $("#uploadEvidenceForm")[0].reset();
    $("#attachedFiles").empty();
}

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