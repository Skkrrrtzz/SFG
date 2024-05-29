async function renderConformitiesTable() {
    $('#conformitiesTableBody').empty();
    $('#conformitiesTable').show();
    $('.emptyTable').show();
    // $('#conformitiesTable').css('display', 'block');
    // $('.emptyTable').css('display', 'block');

    let conformities = await getConformitiesByPlanId($('#readAuditPlanId').val());

    conformities.forEach(conformity => {
        // console.log(conformity.planId)
                $('#conformitiesTable').append(
                    $('<tr>').append(
                        $('<td>').attr('hidden', true).text(conformity.conformityId),
                        $('<td>').attr('hidden', true).text(conformity.planId),
                        $('<td>').text(conformity.conformityDescription),
                        $('<td>').text(conformity.conformityAreaSection),
                        $('<td>').attr('data-conformityid', conformity.conformityId)
                                 .append(
                                            $('<button>', {type: 'button', class: 'btn btn-danger conformity-delete'}).append($('<i class="fa-solid fa-trash"></i>')),
                                            $('<button>', {type: 'button', class: 'btn btn-secondary'}).append($('<i class="fa-solid fa-pen-to-square"></i>'))
                                        )
                    )
                )
    });
    
    if (conformities.length > 0) {
        $('#conformitiesTable').show();
        $('.emptyTable').hide();
        // $('#conformitiesTable').css('display', 'block');
        // $('.emptyTable').css('display', 'none');
    } else {
        $('#conformitiesTable').hide();
        $('.emptyTable').show();
        // $('#conformitiesTable').css('display', 'hidden');
        // $('.emptyTable').css('display', 'none');
    }

    //TODO: This is all still broken. No idea where I should put the data for the ConformityId. Make a new column maybe.
    $('.conformity-delete').on('click', e => {
        let conformityId = e.currentTarget.parentNode.dataset.conformityid/* .split('-')[1] */;

        fetch('/api/conformities/' + conformityId, {
            method: "DELETE",
        })
        .then(response => response.json())
        .then(data => {
            renderConformitiesTable();
            // console.log(data);
        })
        .catch(error => console.log(error));
    });

    // $('.conformity-delete > *').click( e => e.stopPropagation());
}

function getConformitiesByPlanId(planId) {
    return fetch("/api/auditplans/" + planId + "/conformities", {
        method: "GET",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => response.json())
    .then(conformities => { 
        // if (conformities.length > 0) {
        //     $('#conformitiesTable').show();
        //     $('.emptyTable').hide();
        // } else {
        //     $('#conformitiesTable').hide();
        //     $('.emptyTable').show();
        // }

        return conformities;
    })
    .catch(error => console.log(error));
}

$('#conformitiesTab').on('click', e => renderConformitiesTable());


