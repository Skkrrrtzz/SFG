// Function to get query parameter by name from URL
function getQueryParam(name) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}

// Get the projectName and quotationCode from the URL query parameters
const projectName = getQueryParam("projectName");
const quotationCode = getQueryParam("quotationCode");
// console.log(projectName);
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
      // console.log(data);
      if (data.success) {
        // Display a SweetAlert2 warning message
        const Toast = Swal.mixin({
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 4000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          },
        });
        Toast.fire({
          icon: "info",
          title: data.message,
        });
        populateSelect(data.data);
      } else {
        // Display a SweetAlert2 warning message
        const Toast = Swal.mixin({
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 4000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          },
        });
        Toast.fire({
          icon: "info",
          title: data.message,
        });
      }
    })
    .catch((error) => {
      // Handle errors
      console.error("Error:", error);
    });
}

function findPartNumber(projectName, partNumber) {
  console.log(projectName, partNumber);
  fetch("/Sourcing/GetPrices", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ projectName, partNumber }),
  })
    .then((response) => response.json())
    .then((data) => {
      console.log(data);
      // Call the function with the sample data
      appendSupplierCards(data.data);
    })
    .catch((error) => {
      console.error("Error:", error);
    });
}

function createSupplierCard(supplierDetail, index) {
  // Define card color based on the index
  let cardColorClass;
  let cardTitleColor;
  switch (index) {
    case 0:
      cardColorClass = "bg-secondary";
      cardTitleColor = "text-white";
      break;
    case 1:
      cardColorClass = "bg-secondary-subtle";
      cardTitleColor = "text-dark";
      break;
    default:
      cardColorClass = "bg-light";
      cardTitleColor = "text-dark";
      break;
  }

  // Create card element using jQuery
  const card = $(`
        <div class="col-12 mb-2">
            <div class="card ${cardTitleColor} ${cardColorClass}">
                <h4 class="ms-2">${index + 1} BEST PRICE</h4>
                <div class="row g-2 mx-2 mb-2">
                    ${createUnitCostFields(supplierDetail.unitCosts)}
                    ${createInputField("Currency", supplierDetail.currency)}
                    ${createInputField("Supplier", supplierDetail.supplier)}
                    ${createInputField("MOQ", supplierDetail.moq)}
                    ${createInputField("SPQ", supplierDetail.spq)}
                    ${createInputField(
                      "Purchasing UOM",
                      supplierDetail.purchasingUOM
                    )}
                    ${createInputField(
                      "Parts LT",
                      supplierDetail.leadTimeWeeks
                    )}
                    ${createInputField("Location", supplierDetail.location)}
                    ${createInputField(
                      "Quote Validity",
                      supplierDetail.quoteValidity
                    )}
                    ${createInputField(
                      "Sourcing Remarks",
                      supplierDetail.sourcingRemarks
                    )}
                    ${createInputField(
                      "Tooling Cost",
                      supplierDetail.toolingCost
                    )}
                    ${createInputField(
                      "Tooling LT",
                      supplierDetail.toolingLeadTimeWeeks
                    )}
                    ${createInputField(
                      "Tooling Sourcing Remarks",
                      supplierDetail.toolingSourcingRemarks
                    )}
                </div>
            </div>
        </div>
    `);
  return card;
}

function createInputField(name, value) {
  // Adjust column classes for Tooling Sourcing Remarks
  const columnClasses =
    name === "Tooling Sourcing Remarks" ? "col-lg-4" : "col-lg-2";

  return `
        <div class="col-6 col-md-3 ${columnClasses}">
            <div class="form-floating">
                <input type="text" class="form-control bg-white" name="${name}" id="${name}" value="${
    value ?? ""
  }" readonly>
                <label class="form-label fw-bold text-black" for="${name}">${name}</label>
            </div>
        </div>
    `;
}

function createUnitCostFields(unitCosts) {
  let unitCostFields = "";
  for (const [quantity, cost] of Object.entries(unitCosts)) {
    unitCostFields += `
            <div class="col-6 col-md-3 col-lg-2">
                <div class="form-floating">
                    <input type="text" class="form-control bg-white" name="UnitCost_${quantity}" id="UnitCost_${quantity}" value="${cost}" readonly>
                    <label class="form-label fw-bold text-black" for="UnitCost_${quantity}">Unit Cost x${quantity}</label>
                </div>
            </div>
        `;
  }
  return unitCostFields;
}

function appendSupplierCards(data) {
  const container = $("#prices-container");
  container.empty(); // Clear any existing content

  // Sort suppliers by the lowest unit cost for 1 unit
  // data.supplierDetails.sort((a, b) => a.unitCosts[1] - b.unitCosts[1]);

  // Create and append the supplier cards
  data.supplierDetails.forEach((supplierDetail, index) => {
    const card = createSupplierCard(supplierDetail, index);
    container.append(card);
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
    let selectedIndex = parseInt($(this).val());
    let projectName = $("#projectName").text();
    // console.log("Selected Index:", selectedIndex);
    if (!isNaN(selectedIndex)) {
      // Check if selectedIndex is a number
      let selectedPart = data[selectedIndex];

      // console.log("Selected Part:", selectedPart);
      if (selectedPart) {
        $("#PartNumber").val(selectedPart.customerPartNumber);
        $("#Description").val(selectedPart.description);
        $("#MPN").val(selectedPart.origMPN);
        $("#Manufacturer").val(selectedPart.origMFR);
        $("#Commodity").val(selectedPart.commodity);
        $("#Qty").val(selectedPart.eqpa);
        $("#UOM").val(selectedPart.uoM);
        $("#Parts").val(selectedPart.status);
        findPartNumber(projectName, selectedPart.customerPartNumber);
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
