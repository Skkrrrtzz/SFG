var readCPARToggle = false;

$('#readCPAREdit').click(e => {
    // $('#readCPAR .modal-footer').hide();
    if (readCPARToggle) {
        $('#readCPAR .modal-footer').attr('hidden', false);
        $('#readCPARFormsRC [readonly]').attr('readonly', false);
        readCPARToggle = true;
    } else {
        $('#readCPAR .modal-footer').attr('hidden', true);
        $('#readCPARFormsRC [readonly]').attr('readonly', true);
        readCPARToggle = false;
    }

});