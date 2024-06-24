// Function to set up a month filter and charts
function setupMonthFilter(month1, month2, chart1, chart2) {
  const monthFilter1 = $(month1);
  const monthFilter2 = $(month2);
  const currentDate = new Date();
  const currentMonthYear = `${currentDate.getFullYear()}-${(
    currentDate.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}`;

  monthFilter1.val(currentMonthYear);
  monthFilter2.val(currentMonthYear);

  monthFilter1.on("change", function () {
    const selectedMonth = $(this).val();
    fetchData(selectedMonth, chart1, "myChart1");
  });

  monthFilter2.on("change", function () {
    const selectedMonth = $(this).val();
    fetchData(selectedMonth, chart2, "myChart2");
  });

  fetchData(currentMonthYear, chart1, "myChart1");
  fetchData(currentMonthYear, chart2, "myChart2");
}

function fetchData(yearMonth, chart, chartID) {
  // console.log(`Fetching data for ${yearMonth}`);
  fetch("/Dashboard/Dashboard?handler=SummaryRFQperMonth", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
    body: JSON.stringify(yearMonth),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((responseData) => {
      const data = responseData.data;
      if (Array.isArray(data)) {
        updateChart(data, chart, chartID);
        if (chartID === "myChart2") {
          const tatValues = data.map((item) => item.TAT ?? 0);
          const averageTAT = calculateAverage(tatValues);
          updateAverageTAT(averageTAT);
        }
      } else {
        console.error("Expected an array but received:", data);
      }
    })
    .catch((error) => {
      console.error("Error:", error);
    });
}
// Function to calculate average
function calculateAverage(values) {
  const sum = values.reduce((acc, val) => acc + val, 0);
  return (sum / values.length).toFixed(2);
}

// Function to update the average TAT in the HTML
function updateAverageTAT(averageTAT) {
  $("#averageTAT").text(`Average TAT: ${averageTAT}`);
}
// Function to update chart data
function updateChart(data, chart, chartID) {
  if (chartID === "myChart1") {
    const labels = data.map((item) => item.QuotationCode);
    const uniquePartsData = data.map((item) => item.UniqueParts);
    const commonPartsData = data.map((item) => item.CommonParts);

    chart.data.labels = labels;
    chart.data.datasets[0].data = uniquePartsData;
    chart.data.datasets[1].data = commonPartsData;
  } else if (chartID === "myChart2") {
    const labels = data.map((item) => item.QuotationCode);
    const quotationsCodeData = data.map((item) => item.TAT);
    const stdTATData = data.map((item) => item.StdTAT ?? 0);

    chart.data.labels = labels;
    chart.data.datasets[0].data = quotationsCodeData;

    chart.options.plugins.annotation.annotations = stdTATData.map(
      (target, index) => ({
        type: "line",
        scaleID: "y",
        value: target,
        borderColor: "rgba(255, 0, 0, 1)",
        borderWidth: 2,
        label: {
          display: true,
          content: `Target: ${target}`,
          position: "end",
        },
      })
    );
  }

  chart.update();
}

// Initialize charts
const myChart1 = new Chart(
  document.getElementById("myChart1").getContext("2d"),
  {
    type: "bar",
    data: {
      labels: [],
      datasets: [
        {
          label: "Unique Parts",
          data: [],
          backgroundColor: "rgba(58, 91, 160, 1)",
          borderWidth: 1,
        },
        {
          label: "Common Parts",
          data: [],
          backgroundColor: "rgba(41, 58, 128, 1)",
          borderWidth: 1,
        },
      ],
    },
    options: {
      scales: {
        x: {
          stacked: true,
        },
        y: {
          stacked: true,
          beginAtZero: true,
        },
      },
    },
  }
);

const myChart2 = new Chart(
  document.getElementById("myChart2").getContext("2d"),
  {
    type: "bar",
    data: {
      labels: [],
      datasets: [
        {
          label: "RFQ TAT",
          data: [],
          backgroundColor: "rgba(58, 91, 160, 1)",
          borderWidth: 1,
          order: 1,
        },
      ],
    },
    options: {
      responsive: true,
      scales: {
        y: {
          beginAtZero: true,
        },
      },
      plugins: {
        annotation: {
          annotations: {}, // Initialize as an empty object
        },
      },
    },
  }
);

// Set up month filters for both charts
setupMonthFilter("#monthFilter1", "#monthFilter2", myChart1, myChart2);
const actualData = [100, 180, 120, 80, 140, 170, 160];
const targetData = [110, 150, 140, 100, 120, 110, 170];

new Chart("chart", {
  type: "bar",
  data: {
    labels: ["KPI 1", "KPI 2", "KPI 3", "KPI 4", "KPI 5", "KPI 6", "KPI 7"],
    datasets: [
      {
        label: "Actual",
        backgroundColor: "rgba(0, 0, 255, 0.2)",
        data: actualData,
        xAxisID: "x-axis-actual",
      },
      {
        label: "Target",
        backgroundColor: "rgba(255, 0, 128, 1)",
        data: targetData.map((v) => [v - 1, v + 1]),
        xAxisID: "x-axis-target",
      },
    ],
  },
  options: {
    tooltips: {
      callbacks: {
        label: (tooltipItem, data) => {
          const v =
            data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
          return Array.isArray(v) ? (v[1] + v[0]) / 2 : v;
        },
      },
    },
    scales: {
      xAxes: [
        {
          id: "x-axis-target",
          stacked: true,
        },
        {
          display: false,
          offset: true,
          stacked: true,
          id: "x-axis-actual",
          gridLines: {
            offsetGridLines: true,
          },
        },
      ],
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
          },
        },
      ],
    },
  },
});
