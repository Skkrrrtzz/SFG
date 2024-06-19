function formatDate(dateString) {
  if (!dateString) {
    return "---";
  }
  const date = new Date(dateString);
  if (isNaN(date)) {
    return "---";
  }
  const options = { month: "2-digit", day: "2-digit", year: "numeric" };
  return date.toLocaleDateString("en-US", options);
}
fetch("/Dashboard/CheckingPartNumber?handler=CheckingPartNumber")
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    console.log(data);

    // Add a 'No' property to each item in the data array
    data.data.forEach((item, index) => {
      item.No = index + 1;
    });

    let checkingPartNumber = $("#checkingPartNumberTbl").DataTable({
      responsive: true,
      data: data.data,
      columns: [
        { data: "No" },
        { data: "ForeignName" },
        { data: "PartNumber" },
        { data: "ItemDescription" },
        { data: "GWRLQty" },
        { data: "Unit" },
        {
          data: "LastPurchasedDate",
          render: function (data, type, row) {
            return formatDate(data);
          },
        },
        { data: "LastPurchasedUSDPrice" },
        { data: "CustomerVendorName" },
      ],
      // Add createdRow function to add numbering
      createdRow: function (row, data, dataIndex) {
        $("td:eq(0)", row).html(dataIndex + 1); // Use 1-based indexing for display
      },
      columnDefs: [
        { targets: 0, searchable: false }, // Disable searching on the No column
      ],
    });
  })
  .catch((error) => {
    alert("Error: " + error.message);
  });

// Function to format date (example)
function formatDate(date) {
  return new Date(date).toLocaleDateString();
}
