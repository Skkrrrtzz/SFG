//Close the modal and let Bootstrap Modal logic handle the rest
$("#uploadEvidenceSubmit").on('click', () => {
    $("#uploadEvidence").modal('toggle');
});

//Triggers whenever the form is submitted (currently via Unobtrusive AJAX)
function uploadEvidenceComplete(xhr, status) {
    alert(xhr.responseText);
    
    //Clear form
    $("#uploadEvidenceForm")[0].reset();
    $("#attachedFiles").empty();

    //Re-render sub-tables
    renderCPARSubtables();
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
        anchor.textContent = file.name;
        anchor.target = "_blank";

        anchor.dataset.bsToggle = "tooltip";
        anchor.title =  "Click to preview file";
        document.querySelector("#attachedFiles").appendChild(anchor);
    }
});

//Setting form values so I don't have to render the whole form from the client side
//Experimental, as this approach assumes that the form is rendered server-side ann only the values need to be set
function uploadEvidenceData(form, subform, id) {
    // $("#uploadEvidenceForCPAR").val(cparId);
    $("#uploadEvidenceForForm").val(form);
    $("#uploadEvidenceForSubform").val(subform);
    $("#uploadEvidenceForId").val(id);
}

function viewEvidence(form, subform, id) {
    const attachedFiles = document.querySelector("#viewAttachedFiles");
    attachedFiles.innerHTML = "";

    //Set data
    // $("#viewEvidenceForForm").val(form);
    // $("#viewEvidenceForSubform").val(subform);
    // $("#viewEvidenceForId").val(id);

    $('input[name=form]').val(form);
    $('input[name=subform]').val(subform);
    $('input[name=id]').val(id);
    
    fetch(`?handler=Evidences&Form=${form}&Subform=${subform}&Id=${id}`)
    .then(response => response.json())
    .then(data => {
        data.forEach(file => {
            const div = document.createElement("div");

            //Anchor
            const anchor = document.createElement("a");
            // anchor.href = new Blob([file], { type: image });
            // anchor.href = `${cparId}/${subform}/${id}/${file}`;
            anchor.href = `${subform}/${id}/${file}`;
            anchor.textContent = file;
            anchor.target = "_blank";
            anchor.dataset.bsToggle = "tooltip";
            anchor.title =  "Click to preview file";
            
            //Delete
            const button = document.createElement("button")
            button.classList.add("fa", "fa-x", "deleteEvidenceButton");
            // button.textContent = "X";
            button.addEventListener('click', e => {
                e.preventDefault();
                deleteEvidence( $("#viewEvidenceForForm").val(),
                                $("#viewEvidenceForSubform").val(),
                                $("#viewEvidenceForId").val(),
                                anchor.textContent
                                )
            });
            
            div.appendChild(anchor);
            div.appendChild(button);

            attachedFiles.appendChild(div);
        });
    });
}

function deleteEvidence(form, subform, id, filename) {
    // fetch(`?handler=DeleteEvidence&Form=${form}&Subform=${subform}&Id=${id}&filename=${filename}`, { 
    fetch(`?handler=Evidence`, { 
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": $('#deleteEvidence input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify({
            form: form,
            subform: subform,
            id: id,
            filename: filename
        })
    })
    .then(response => response.json())
    .then(data => viewEvidence(form, subform, id));
}


//Failed attempt to make upload ACID-compliant (not compatible with Unobtrusive AJAX)
//Unless...we make the method itself do ACID-compliant requests- which is weird
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