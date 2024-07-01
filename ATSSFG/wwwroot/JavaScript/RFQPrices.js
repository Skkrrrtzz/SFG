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
  const url = `/Sourcing/SourcingRFQPrices?handler=RFQPartNumbers&projectName=${encodeURIComponent(
    projectName
  )}&quotationCode=${encodeURIComponent(quotationCode)}`;
  fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
  })
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
        Swal.fire({
          title: data.message,
          icon: "info",
          toast: true,
          width: 400,
          position: "top-end",
          showCloseButton: true,
          showConfirmButton: false,
        });
        populateSelect(data.data);
      } else {
        // Display a SweetAlert2 warning message
        showInfoAlert(data.message);
      }
    })
    .catch((error) => {
      // Handle errors
      console.error("Error:", error);
    });
}

function findPartNumber(projectName, partNumber) {
  showLoading();
  // console.log(projectName, partNumber);
  fetch("/Sourcing/SourcingRFQPrices?handler=GetPrices", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
    body: JSON.stringify({ projectName, partNumber }),
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        appendSupplierCards(data.data);
        Swal.close();
      } else {
        showErrorAlert(data.message);
      }
    })
    .catch((error) => {
      console.error("Fetch Error:", error);
      showErrorAlert(
        "An error occurred while fetching data. Please try again later."
      );
    });
}
function getOrdinalNumber(n) {
  const s = ["th", "st", "nd", "rd"];
  const v = n % 100;
  return n + (s[(v - 20) % 10] || s[v] || s[0]);
}
function createSupplierCard(supplierDetail, index) {
  // Define card color based on the index
  let cardColorClass;

  switch (index) {
    case 0:
      cardColorClass = "best1stPriceColor";
      break;
    case 1:
      cardColorClass = "best2ndPriceColor";
      break;
    default:
      cardColorClass = "best3rdPriceColor";
      break;
  }
  const ordinalNumber = getOrdinalNumber(index + 1);

  const card = $(`
        <div class="col-12 mb-2">
            <div class="card ${cardColorClass} text-white" id="card${
    index + 1
  }">
                <div class="row">
                  <div class="col-12 d-flex justify-content-between align-items-center">
                    <h4 class="ms-2">${ordinalNumber} BEST PRICE</h4>
                    <div class="form-check form-check-reverse me-2">
                        <input class="form-check-input supplier-checkbox" type="checkbox" value="${
                          index + 1
                        }" id="flexCheck${index + 1}">
                    </div>
                  </div>
                </div>
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

  const checkbox = card.find("#flexCheck" + (index + 1));
  const cardNo = card.find("#card" + (index + 1));
  let suggestedSupplier = $("#SuggestedSupplier").text();
  let comments = $("#Comments").val();

  // Check the checkbox if it matches the suggestedSupplier
  if ((index + 1).toString() === suggestedSupplier) {
    checkbox.prop("checked", true);
    cardNo.addClass("border border-5 selectedPriceColor");
    $("#SuggestedSupplier").text(index + 1);
    $("#Comments").val(comments);
  }

  checkbox.change(function () {
    if ($(this).is(":checked")) {
      // Uncheck other checkboxes
      $(".supplier-checkbox").prop("checked", false);
      $(".card").removeClass("border border-5 selectedPriceColor");

      $(this).prop("checked", true);

      cardNo.addClass("border border-5 selectedPriceColor");
      $("#SuggestedSupplier").text(index + 1);
    } else {
      // Clear the SuggestedSupplier element if no checkbox is selected
      $("#SuggestedSupplier").text("");
      cardNo.removeClass("border border-5 selectedPriceColor");
    }
  });

  return card;
}

function createInputField(name, value) {
  // Adjust column classes for Tooling Sourcing Remarks
  const columnClasses =
    name === "Tooling Sourcing Remarks" ? "col-lg-4" : "col-lg-2";

  return `
        <div class="col-6 col-md-3 ${columnClasses}">
            <div class="form-floating">
                <input type="text" class="form-control bg-light" name="${name}" id="${name}" value="${
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

  // Create and append the supplier cards
  data.supplierDetails.forEach((supplierDetail, index) => {
    const card = createSupplierCard(supplierDetail, index);
    container.append(card);
  });
}

function populateSelect(data) {
  var select = $("#partNumbers");
  // Clear existing options
  select.empty();

  $.each(data, function (index, item) {
    select.append(
      $("<option>", {
        value: index,
        text: item.customerPartNumber,
        class: item.suggestedSupplier ? "highlight" : "",
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
        $("#SuggestedSupplier").text(selectedPart.suggestedSupplier);
        $("#Comments").val(selectedPart.comments);
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

function saveSupplierAndComments() {
  $("#saveButton").prop("disabled", true);
  // showLoading();
  let partNumber = $("#PartNumber").val();
  let suggestedSupplier = $("#SuggestedSupplier").text();
  let comments = $("#Comments").val();
  let projectName = $("#projectName").text();
  // console.log(partNumber, suggestedSupplier, comments, projectName);
  fetch("/Sourcing/SourcingRFQPrices?handler=SaveSupplierAndComments", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
    body: JSON.stringify({
      partNumber,
      projectName,
      suggestedSupplier,
      comments,
    }),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((response) => {
      if (response.success) {
        showSuccessAlert(response.message).then(() => {
          fetchRFQPartNumbers(projectName, quotationCode);
        });
        // fetchRFQPartNumbers(projectName, quotationCode);
      } else {
        showErrorAlert(response.message);
      }
    });
}
updateSaveButtonState();
function updateSaveButtonState() {
  let partNumber = $("#PartNumber").val();
  let suggestedSupplier = $("#SuggestedSupplier").text();

  // Check if partNumber is empty or suggestedSupplier is empty
  if (!partNumber || !suggestedSupplier) {
    $("#saveButton").prop("disabled", true);
  } else {
    $("#saveButton").prop("disabled", false);
  }
}
// Update save button state when the part number changes
$("#PartNumber").change(function () {
  updateSaveButtonState();
});

// Create a MutationObserver to observe changes to the suggested supplier element
const observer = new MutationObserver(function () {
  updateSaveButtonState();
});

// Start observing changes to the suggested supplier element
const suggestedSupplierElement = document.getElementById("SuggestedSupplier");
observer.observe(suggestedSupplierElement, {
  subtree: true,
  characterData: true,
  childList: true,
});

$("#saveButton").click(function (e) {
  e.preventDefault();

  saveSupplierAndComments();
});
