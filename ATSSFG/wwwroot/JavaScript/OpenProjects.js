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
fetch("/Dashboard/OpenProjects?handler=OpenProjectsSummary")
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    $("#openProjectsTbl").DataTable({
      responsive: true,
      data: data.data,
      columns: [
        { data: "ProjectName" },
        { data: "QuotationCode" },
        {
          data: "RequestDate",
          render: function (data, type, row) {
            return formatDate(data);
          },
        },
        {
          data: "RequiredDate",
          render: function (data, type, row) {
            return formatDate(data);
          },
        },
        { data: "NoItems" },
        { data: "ItemsW_Price" },
        { data: "ItemsWO_Price" },
      ],
    });
  })
  .catch((error) => {
    alert("Error: " + error.message);
  });
