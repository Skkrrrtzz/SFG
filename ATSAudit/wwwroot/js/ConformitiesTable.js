$('#conformitiesTab').on('click', () => { 
    renderConformitiesTable();
});

async function renderConformitiesTable() {
    $('#conformitiesTabPane').empty();
    $('#conformitiesTabPane').load(`/Conformities?planId=${$('#readAuditPlanId').val()}`, () => { 
        $('.conformity-delete').on('click', e => {
            let conformityId = e.currentTarget.parentNode.dataset.conformityid/* .split('-')[1] */;

            fetch('/api/conformities/' + conformityId, {
                method: "DELETE",
            })
            .then(response => response.json())
            .then(data => { renderConformitiesTable() })
            .catch(error => console.log(error));
        });
    });
}