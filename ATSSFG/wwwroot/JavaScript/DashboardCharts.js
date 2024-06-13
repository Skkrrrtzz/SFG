// Function to set up a month filter
function setupMonthFilter(inputSelector) {
  const monthFilter = $(inputSelector);
  const currentDate = new Date();
  const currentMonthYear = `${currentDate.getFullYear()}-${(
    currentDate.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}`;

  monthFilter.val(currentMonthYear);

  // Fetch data initially
  fetchData(currentMonthYear);

  // Fetch data when the month filter is changed
  monthFilter.on("change", function () {
    const selectedMonth = $(this).val();
    fetchData(selectedMonth);
  });
}

// Function to fetch data based on month
function fetchData(monthYear) {
  // Implement your data fetching logic here
  console.log(`Fetching data for ${monthYear}`);
}

// Set up month filters
setupMonthFilter("#monthFilter1");
setupMonthFilter("#monthFilter2");

const ctx1 = document.getElementById("myChart1");

new Chart(ctx1, {
  type: "bar",
  data: {
    labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
    datasets: [
      {
        label: "# of Votes",
        data: [12, 19, 3, 5, 2, 3],
        borderWidth: 1,
      },
    ],
  },
  options: {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  },
});
const ctx2 = document.getElementById("myChart2");

new Chart(ctx2, {
  type: "bar",
  data: {
    labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
    datasets: [
      {
        label: "# of Votes",
        data: [12, 19, 3, 5, 2, 3],
        borderWidth: 1,
      },
    ],
  },
  options: {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  },
});
