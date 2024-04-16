$("#SRFQTbl").dataTable({
  responsive: true,
});

// Add click event listener to the edit buttons
$(".btnEdit").click(function () {
  var id = $(this).data("id");
  // Send AJAX request to fetch data for editing
  $.ajax({
    url: FindIdUrl,
    method: "GET",
    data: { id: id },
    success: function (data) {
      $("#editCustomerPartnumber").val(data.customerPartNumber);
      $("#editRev").val(data.rev);
      $("#editDescription").val(data.description);
      $("#editOrigMPN").val(data.origMPN);
      $("#editOrigMFR").val(data.origMFR);
      $("#editCommodity").val(data.commodity);
      $("#editEqpa").val(data.eqpa);
      $("#editUoM").val(data.uoM);
      $("#editStatus").val(data.status);
    },
    error: function () {
      alert("Failed to fetch data for editing.");
    },
  });
});
