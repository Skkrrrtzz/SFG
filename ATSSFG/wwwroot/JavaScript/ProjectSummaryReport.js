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
fetch("/Dashboard/ProjectSummaryReport?handler=RFQSummaryReport")
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    let projectSummaryTbl = $("#projectSummaryTbl").DataTable({
      responsive: true,
      data: data.data,
      dom: '<"row m-1 d-flex justify-content-between"<"col-sm-8"Bl><"col-sm-4"f>>t<"row"<"col-sm-6"i><"col-sm-6"p>>',
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
        {
          data: "ActualCompletionDate",
          render: function (data, type, row) {
            return formatDate(data);
          },
        },
        { data: "StdTAT" },
        { data: "TAT" },
        { data: "NoItems" },
        { data: "UniqueParts" },
        { data: "CommonParts" },
      ],
      buttons: [
        {
          extend: "copyHtml5",
          text: '<i class="fas fa-copy "></i> Copy',
          className: "btn btn-sm bg-main3 mb-1",
          exportOptions: {
            columns: ":visible",
          },
        },
        {
          extend: "excelHtml5",
          text: '<i class="fas fa-file-excel"></i> Excel',
          className: "btn btn-sm bg-main3 mb-1",
          exportOptions: {
            columns: ":visible",
          },
        },
      ],
    });
  })
  .catch((error) => {
    alert("Error: " + error.message);
  });
