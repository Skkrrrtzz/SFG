// Function to get query parameter by name from URL
function getQueryParam(name) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}

// Get the projectName and quotationCode from the URL query parameters
const projectName = getQueryParam("projectName");
const quotationCode = getQueryParam("quotationCode");
console.log(projectName);
$("#projectName").text(projectName);
// Function to make the GET request
function fetchRFQPartNumbers(projectName, quotationCode) {
  // Construct the URL with parameters
  const url = `/Sourcing/GetRFQPartNumbers?projectName=${projectName}&quotationCode=${quotationCode}`;

  // Make the GET request
  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      //   console.log(data);
      populateSelect(data);
    })
    .catch((error) => {
      // Handle errors
      console.error("Error:", error);
    });
}

function populateSelect(data) {
  var select = $("#partNumbers");
  $.each(data, function (index, item) {
    select.append(
      $("<option>", {
        value: index,
        text: item.customerPartNumber,
      })
    );
  });

  $("#partNumbers").change(function () {
    var selectedIndex = parseInt($(this).val());
    // console.log("Selected Index:", selectedIndex);
    if (!isNaN(selectedIndex)) {
      // Check if selectedIndex is a number
      var selectedPart = data[selectedIndex];
      //   console.log("Selected Part:", selectedPart);
      if (selectedPart) {
        $("#PartNumber").val(selectedPart.customerPartNumber);
        $("#Description").val(selectedPart.description);
        $("#MPN").val(selectedPart.origMPN);
        $("#Manufacturer").val(selectedPart.origMFR);
        $("#Commodity").val(selectedPart.commodity);
        $("#Qty").val(selectedPart.eqpa);
        $("#UOM").val(selectedPart.uoM);
        $("#Parts").val(selectedPart.status);
      } else {
        console.error("Selected part data is undefined.");
        // Clear input fields if selected part data is undefined
        $(
          "#PartNumber, #Description, #MPN, #Manufacturer, #Commodity, #Qty, #UOM, #Parts"
        ).val("");
      }
    } else {
      // Clear input fields if no part number is selected
      $(
        "#PartNumber, #Description, #MPN, #Manufacturer, #Commodity, #Qty, #UOM, #Parts"
      ).val("");
    }
  });
}
// Call the function with the projectName and quotationCode
fetchRFQPartNumbers(projectName, quotationCode);
