   <!-- ATTENDANCE CHART-->
   <script>
       // Initialize the chart when the modal is shown
       $('#AllAttendance').on('shown.bs.modal', function() {
           // Initialize default interval as 'daily'
           let currentInterval = 'daily';

           // Function to toggle the interval between 'daily' and 'weekly'
           function toggleInterval() {
               const toggleWeekly = document.getElementById('toggleWeekly').checked;

               if (toggleWeekly) {
                   currentInterval = 'weekly';
               } else {
                   currentInterval = 'daily';
               }


               // Update the chart data based on the new interval and department
               let newData;
               if (currentInterval === 'daily') {
                   newData = <?php echo json_encode($cableDailyData); ?>;
               } else if (currentInterval === 'weekly') {
                   newData = <?php echo json_encode($cableWeeklyData); ?>;
               }

               Attchart.data.labels = Object.keys(newData);
               Attchart.data.datasets[0].data = Object.values(newData);

               // Update the label and data for the main department
               if (currentInterval === 'daily') {
                   newData = <?php echo json_encode($mainDailyData); ?>;
               } else if (currentInterval === 'weekly') {
                   newData = <?php echo json_encode($mainWeeklyData); ?>;
               }

               Attchart.data.datasets[1].data = Object.values(newData);
               Attchart.update();
           }

           // Chart data
           const cableData = <?php echo json_encode($cableDailyData); ?>;
           const mainData = <?php echo json_encode($mainDailyData); ?>;
           const cableWeeklyData = <?php echo json_encode($cableWeeklyData); ?>;
           const mainWeeklyData = <?php echo json_encode($mainWeeklyData); ?>;

           // Create the chart
           const ctx = document.getElementById('att_chart').getContext('2d');
           const Attchart = new Chart(ctx, {
               type: 'bar',
               data: {
                   datasets: [{
                       label: 'Cable',
                       data: cableData,
                       backgroundColor: 'rgba(255, 177, 193)',
                       borderColor: 'rgba(255,99,132,255)',
                       borderWidth: 2
                   }, {
                       label: 'Main',
                       data: mainData,
                       backgroundColor: 'rgba(154,208,245,255)',
                       borderColor: 'rgba(65,167,236,255)',
                       borderWidth: 2
                   }]
               },
               options: {
                   plugins: {
                       legend: {
                           display: true,
                           labels: {
                               color: 'black',
                               font: {
                                   weight: 'bold',
                               },
                           },
                       },
                       tooltip: {
                           enabled: true,
                           callbacks: {
                               label: function(context) {
                                   return context.dataset.label + ': ' + context.parsed.y + '%';
                               },
                           },
                       },
                       annotation: {
                           annotations: {
                               targetLine: {
                                   type: 'line',
                                   yMin: 95, // Target attendance rate (96%)
                                   yMax: 95, // Target attendance rate (96%)
                                   borderColor: 'rgba(255, 174, 66)',
                                   borderWidth: 2,
                                   label: {
                                       enabled: true,
                                       content: 'Target: 95%', // The label content
                                       position: 'end', // Position of the label relative to the target line (start, center, end)
                                   },
                               },
                           },
                       },
                   },
                   maintainAspectRatio: true,
                   responsive: true,
                   scales: {
                       y: {
                           beginAtZero: true,
                           ticks: {
                               callback: function(value) {
                                   return value + '%';
                               },
                           },
                       },
                   },
               },
           });

           // Add event listener to handle click on the chart
           document.getElementById('att_chart').addEventListener('click', function(event) {
               const activePoints = Attchart.getElementsAtEventForMode(event, 'nearest', {
                   intersect: true
               }, true);
               if (activePoints.length > 0) {
                   const clickedDatasetIndex = activePoints[0].datasetIndex;
                   const clickedIndex = activePoints[0].index;

                   // Perform action based on the clicked dataset and index
                   if (clickedDatasetIndex === 0 && clickedIndex === 0) {
                       // Bar for dataset 'Cable' and index 0 was clicked
                       window.location.href = 'Generate Reports/cable_attendance_summary.php';
                   } else if (clickedDatasetIndex === 1 && clickedIndex === 0) {
                       // Bar for dataset 'Main' and index 0 was clicked
                       window.location.href = 'Generate Reports/module_attendance_summary.php';
                   }
               }
           });

           document.getElementById('toggleWeekly').addEventListener('change', toggleInterval);
       });
   </script>

   <!-- ALL ATTENDANCE CHART-->
   <script>
       // Initialize default interval as 'daily'
       let currentAllInterval = 'daily';

       // Function to toggle the interval between 'daily', 'weekly'
       function toggleAllInterval() {
           const toggleAllWeekly = document.getElementById('toggleAllWeekly').checked;
           const toggleAllMonthly = document.getElementById('toggleAllMonthly').checked;

           if (toggleAllWeekly) {
               currentAllInterval = 'weekly';
           } else if (toggleAllMonthly) {
               currentAllInterval = 'monthly';
           } else {
               currentAllInterval = 'daily';
           }

           // Update the chart data based on the new interval
           let newData;
           if (currentAllInterval === 'daily') {
               newData = <?php echo json_encode($OverAllDailyData); ?>;
           } else if (currentAllInterval === 'weekly') {
               newData = Object.fromEntries(Object.entries(<?php echo json_encode($OverAllWeeklyData); ?>).map(([key, value]) => [`Week ${key}`, value]));
           } else if (currentAllInterval === 'monthly') {
               newData = <?php echo json_encode($monthlyAttendanceRatesWithDays); ?>;
           }

           // Update chart labels and data
           Att.data.labels = Object.keys(newData);
           Att.data.datasets[0].data = Object.values(newData);
           Att.update();
       }

       // Chart data
       const OvrAllDailyAttendance = <?php echo json_encode($OverAllDailyData); ?>;
       const attendanceValues = Object.values(OvrAllDailyAttendance);

       // Create the chart
       const All_Att = document.getElementById('all_att_chart').getContext('2d');
       const Att = new Chart(All_Att, {
           type: 'bar',
           data: {
               labels: Object.keys(OvrAllDailyAttendance),
               datasets: [{
                   label: 'Production',
                   data: attendanceValues,
                   backgroundColor: 'rgba(227, 246, 245, 1)',
                   borderColor: 'rgba(44, 105, 141, 255)',
                   borderWidth: 2
               }]
           },
           options: {
               plugins: {
                   legend: {
                       display: true,
                       labels: {
                           color: 'black',
                           font: {
                               weight: 'bold',
                           },
                       },
                   },
                   tooltip: {
                       enabled: true,
                       callbacks: {
                           label: function(context) {
                               return context.dataset.label + ': ' + context.parsed.y + '%';
                           },
                       },
                   },
                   annotation: {
                       annotations: {
                           targetLine: {
                               type: 'line',
                               yMin: 95, // Target attendance rate 
                               yMax: 95,
                               borderColor: 'rgba(255, 174, 66)',
                               borderWidth: 2,
                               label: {
                                   enabled: true,
                                   content: 'Target: 95%',
                                   position: 'end',
                               },
                           },
                       },
                   },
                   datalabels: {
                       anchor: 'top',
                       align: 'top',
                       formatter: function(value, context) {
                           if (value !== null && value !== undefined) {
                               return value + '%';
                           } else {
                               console.log('Null or undefined value detected:', value);
                               return 'N/A';
                           }
                       },
                       color: 'black',
                       font: {
                           weight: 'bold',
                           size: 14
                       }
                   },
               },
               maintainAspectRatio: true,
               responsive: true,
               scales: {
                   y: {
                       beginAtZero: true,
                       ticks: {
                           callback: function(value) {
                               return value + '%';
                           },
                       },
                   },
               },
           },
           plugins: [ChartDataLabels]
       });

       // Get the bar chart canvas element by ID
       const chartCanvas = document.getElementById('all_att_chart');

       chartCanvas.addEventListener('click', function(event) {
           const activePoints = Att.getElementsAtEventForMode(event, 'nearest', {
               intersect: true
           }, false);

           if (activePoints.length > 0) {
               const firstPoint = activePoints[0];
               const clickedDate = Att.data.labels[firstPoint.index];

               if (currentAllInterval === 'daily') {
                   // Use jQuery AJAX to fetch attendance data from PROD_dashboard.php
                   $.ajax({
                       url: 'PROD_dashboard.php',
                       method: 'GET',
                       data: {
                           clickedDate: encodeURIComponent(clickedDate)
                       },
                       dataType: 'json',
                       success: function(response) {
                           // console.log(response);
                           const cableAttendance = response.cable;
                           const prodAttendance = response.prod;
                           const cablePresent = response.cable_present;
                           const cableAbsent = response.cable_abs;
                           const prodPresent = response.prod_present;
                           const prodAbsent = response.prod_abs;
                           const dates = response.dates;

                           // Update the HTML elements with the cable attendance data
                           $('#cablePresentBadge').text(`Present: ${cablePresent}`);
                           $('#cableAbsentBadge').text(`Absent: ${cableAbsent}`);
                           $('#prodPresentBadge').text(`Present: ${prodPresent}`);
                           $('#prodAbsentBadge').text(`Absent: ${prodAbsent}`);
                           $('#datesBadge').text(`${dates}`);

                           // Update the modal content with the fetched data
                           updateModalContent(response);

                           // Show the modal
                           const modal = new bootstrap.Modal(document.getElementById('AllAttModal'));
                           modal.show();
                       },
                       error: function(xhr, status, error) {
                           console.error('Request failed:', error);
                       }
                   });
               } else if (currentAllInterval === 'monthly') {
                   window.location.href = 'Generate Reports/cable_attendance_summary.php';
               }
           }
       });

       function updateModalContent(response) {
           const cableTableBody = $('#cableTable tbody');
           const mainTableBody = $('#mainTable tbody');
           const allAbsentTableBody = $('#absTable tbody');

           // Clear the existing table rows
           cableTableBody.empty();
           mainTableBody.empty();
           allAbsentTableBody.empty();

           // Populate the tables with attendance data
           $.each(response.cable, function(index, entry) {
               const row = `
            <tr>
                <td>${entry.Name}</td>
                <td>${entry.Time_In}</td>
            </tr>`;
               cableTableBody.append(row);
           });

           $.each(response.prod, function(index, entry) {
               const row = `
            <tr>
                <td>${entry.Name}</td>
                <td>${entry.Time_In}</td>
            </tr>`;
               mainTableBody.append(row);
           });

           $.each(response.all_abs, function(index, entry) {
               const row = `
            <tr>
                <td>${entry.Name}</td>
            </tr>`;
               allAbsentTableBody.append(row);
           });
       }
   </script>

   <!-- <script>
        const ctx1 = document.getElementById("eff_chart").getContext("2d");
        const chart1 = new Chart(ctx1, {
            type: 'bar',
            data: {
                labels: ['<?php echo $date; ?>'],
                datasets: [{
                        label: 'Cable',
                        data: [<?php echo $operator_eff; ?>],
                        backgroundColor: 'rgba(255, 177, 193)',
                        borderColor: 'rgba(255,99,132,255)',
                        borderWidth: 2
                    },
                    {
                        label: 'Main',
                        data: [<?php echo $OVR_EFF; ?>],
                        backgroundColor: 'rgba(154,208,245,255)',
                        borderColor: 'rgba(65,167,236,255)',
                        borderWidth: 2
                    }
                ]
            },
            options: {
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: true,
                        labels: {
                            color: 'black',
                            font: {
                                weight: 'bold'
                            }
                        }
                    },
                    tooltip: {
                        enabled: true,
                        callbacks: {
                            label: function(context) {
                                return context.dataset.label + ': ' + context.parsed.y + '%';
                            }
                        }
                    },
                    annotation: {
                        annotations: {
                            targetLine: {
                                type: 'line',
                                yMin: 96, // Target attendance rate (96%)
                                yMax: 96, // Target attendance rate (96%)
                                borderColor: 'rgba(255, 174, 66)',
                                borderWidth: 2,
                                label: {
                                    enabled: true,
                                    content: 'Target: 96%', // The label content
                                    position: 'end', // Position of the label relative to the target line (start, center, end)
                                },
                            },
                        },
                    }
                },
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function(value) {
                                return value + '%';
                            }
                        }
                    }
                }
            }
        });
        // Add event listener to handle click on the chart
        document.getElementById('eff_chart').addEventListener('click', function(event) {
            const activePoints1 = chart1.getElementsAtEventForMode(event, 'nearest', {
                intersect: true
            }, true);
            if (activePoints1.length > 0) {
                const clickedDatasetIndex1 = activePoints1[0].datasetIndex;
                const clickedIndex1 = activePoints1[0].index;

                // Perform action based on the clicked dataset and index
                if (clickedDatasetIndex1 === 0 && clickedIndex1 === 0) {
                    // Bar for dataset 'Cable' and index 0 was clicked
                    window.location.href = 'Generate Reports/cable_efficiency_summary.php?linkTitle=CABLE EFFICIENCY SUMMARY';
                } else if (clickedDatasetIndex1 === 1 && clickedIndex1 === 0) {
                    // Bar for dataset 'Main' and index 0 was clicked
                    window.location.href = 'Generate Reports/Main_Efficiency_Summary.php?linkTitle=MAIN EFFICIENCY SUMMARY';
                }
            }
        });
    </script> -->

   <!-- ALL EFFICIENCY CHART-->
   <script>
       // Initialize default interval as 'weekly'
       let currentEff = 'weekly';

       // Function to toggle the interval between 'daily', 'weekly', and 'monthly'
       function toggleEfficiency() {
           const toggleDailyEff = document.getElementById('toggleDailyEff').checked;
           const toggleMonthlyEff = document.getElementById('toggleMonthlyEff').checked;

           // Update the chart data based on the new interval
           let newData;
           let newDate;

           if (toggleDailyEff) {
               currentEff = 'daily';
           } else if (toggleMonthlyEff) {
               currentEff = 'monthly';
           } else {
               currentEff = 'weekly';
           }

           if (currentEff === 'daily') {
               newDate = <?php echo json_encode(array_keys($efficiencyData["daily"])); ?>;
               newData = <?php echo json_encode(array_values($efficiencyData["daily"])); ?>;
           } else {
               newDate = <?php $weekNumbers = array_keys($efficiencyData["weekly"]);
                            $formattedWeeks = array_map(function ($weekNumber) {
                                return "Week $weekNumber";
                            }, $weekNumbers);
                            echo json_encode($formattedWeeks); ?>;
               newData = <?php echo json_encode(array_values($efficiencyData["weekly"])); ?>;
           }
           // Update chart labels and data
           chart1.data.labels = newDate;
           chart1.data.datasets[0].data = newData;
           chart1.update();
       }


       // Set the initial chart data based on the default interval ('weekly')
       const recordDatesWeekly = <?php $weekNumbers = array_keys($efficiencyData["weekly"]);
                                    $formattedWeeks = array_map(function ($weekNumber) {
                                        return "Week $weekNumber";
                                    }, $weekNumbers);
                                    echo json_encode($formattedWeeks); ?>;
       const ovrEfficiencyWeekly = <?php echo json_encode(array_values($efficiencyData["weekly"])); ?>;

       const ctx1 = document.getElementById("eff_chart").getContext("2d");
       const chart1 = new Chart(ctx1, {
           type: 'bar',
           data: {
               labels: recordDatesWeekly, // Initial labels
               datasets: [{
                   label: 'Overall Efficiency',
                   data: ovrEfficiencyWeekly, // Initial data
                   backgroundColor: 'rgba(255, 177, 193)',
                   borderColor: 'rgba(255, 99, 132, 255)',
                   borderWidth: 2
               }]
           },
           options: {
               responsive: true,
               maintainAspectRatio: true,
               plugins: {
                   legend: {
                       display: true,
                       labels: {
                           color: 'black',
                           font: {
                               weight: 'bold'
                           }
                       }
                   },
                   tooltip: {
                       enabled: true,
                       callbacks: {
                           label: function(context) {
                               return context.dataset.label + ': ' + context.parsed.y + '%';
                           }
                       }
                   },
                   annotation: {
                       annotations: {
                           targetLine: {
                               type: 'line',
                               yMin: 96, // Target attendance rate (96%)
                               yMax: 96, // Target attendance rate (96%)
                               borderColor: 'rgba(255, 174, 66)',
                               borderWidth: 2,
                               label: {
                                   enabled: true,
                                   content: 'Target: 96%', // The label content
                                   position: 'end', // Position of the label relative to the target line (start, center, end)
                               },
                           },
                       },
                   },
                   datalabels: {
                       anchor: 'top',
                       align: 'top',
                       formatter: function(value, context) {
                           if (value !== null && value !== undefined) {
                               return value + '%';
                           } else {
                               console.log('Null or undefined value detected:', value);
                               return 'N/A';
                           }
                       },
                       color: 'black',
                       font: {
                           weight: 'bold',
                           size: 14
                       }
                   },
               },
               scales: {
                   y: {
                       beginAtZero: true,
                       ticks: {
                           callback: function(value) {
                               return value + '%';
                           }
                       }
                   }
               }
           },
           plugins: [ChartDataLabels]
       });

       // Add event listener to handle click on the chart
       document.getElementById('eff_chart').addEventListener('click', function(event) {
           const activePoints1 = chart1.getElementsAtEventForMode(event, 'nearest', {
               intersect: true
           }, true);
           if (activePoints1.length > 0) {
               const clickedDatasetIndex1 = activePoints1[0].datasetIndex;
               const clickedIndex1 = activePoints1[0].index;

               // Perform action based on the clicked dataset and index
               if (clickedDatasetIndex1 === 0 && clickedIndex1 === 0) {
                   // Bar for dataset 'Cable' and index 0 was clicked
                   window.location.href = 'Generate Reports/cable_efficiency_summary.php?linkTitle=CABLE EFFICIENCY SUMMARY';
               } else if (clickedDatasetIndex1 === 1 && clickedIndex1 === 0) {
                   // Bar for dataset 'Main' and index 0 was clicked
                   window.location.href = 'Generate Reports/Main_Efficiency_Summary.php?linkTitle=MAIN EFFICIENCY SUMMARY';
               }
           }
       });
   </script>

   <!-- EFFICIENCY CHART-->
   <script>
       // Initialize the chart when the modal is shown
       $('#AllEff').on('shown.bs.modal', function() {
           // Initialize default interval as 'daily'
           let currentInterval = 'daily';

           function toggleInterval() {
               const toggleWeekly = document.getElementById('toggleWeeklyEff').checked;

               if (toggleWeekly) {
                   currentInterval = 'weekly';
               } else {
                   currentInterval = 'daily';
               }

               // Update the chart data based on the new interval
               let newData;
               if (currentInterval === 'daily') {
                   newData = <?php echo json_encode($EffDailyData); ?>;
               } else if (currentInterval === 'weekly') {
                   newData = <?php echo json_encode($EffWeeklyData); ?>;
               }

               // Extract the necessary data from newData and set labels accordingly
               let labels;
               if (currentInterval === 'weekly') {
                   labels = Object.keys(newData).map(weekNumber => `Week ${weekNumber}`);
               } else {
                   labels = Object.keys(newData);
               }
               const cableEfficiency = Object.values(newData).map(entry => entry.cable_dailyEff || entry.cable_efficiency); // Use 'cable_dailyEff' for daily or 'cable_efficiency' for weekly
               const mainEfficiency = Object.values(newData).map(entry => entry.tech_dailyEff || entry.tech_efficiency); // Use 'tech_dailyEff' for daily or 'tech_efficiency' for weekly

               Effchart.data.labels = labels;
               Effchart.data.datasets[0].data = cableEfficiency;
               Effchart.data.datasets[1].data = mainEfficiency;

               Effchart.update();
           }

           // Add an event listener to the checkbox to call toggleInterval when it changes
           const checkbox = document.getElementById('toggleWeeklyEff');
           checkbox.addEventListener('change', toggleInterval);

           // Initialize the chart with daily data as the default
           const effdailyData = <?php echo json_encode($EffDailyData); ?>;
           const dates = Object.keys(effdailyData);
           const operatorEfficiency = Object.values(effdailyData).map(entry => entry.cable_dailyEff);
           const technicianEfficiency = Object.values(effdailyData).map(entry => entry.tech_dailyEff);

           // Create the chart
           const ctx = document.getElementById('eff_chart_per').getContext('2d');
           const Effchart = new Chart(ctx, {
               type: 'bar',
               data: {
                   labels: dates, // Use the dates as labels
                   datasets: [{
                           label: 'Cable (Operator Efficiency)',
                           data: operatorEfficiency, // Use operator efficiency data
                           backgroundColor: 'rgba(255, 177, 193)',
                           borderColor: 'rgba(255, 99, 132, 255)',
                           borderWidth: 2
                       },
                       {
                           label: 'Main (Technician Efficiency)',
                           data: technicianEfficiency, // Use technician efficiency data
                           backgroundColor: 'rgba(154, 208, 245)',
                           borderColor: 'rgba(65, 167, 236)',
                           borderWidth: 2
                       }
                   ]
               },
               options: {
                   plugins: {
                       legend: {
                           display: true,
                           labels: {
                               color: 'black',
                               font: {
                                   weight: 'bold',
                               },
                           },
                       },
                       tooltip: {
                           enabled: true,
                           callbacks: {
                               label: function(context) {
                                   return context.dataset.label + ': ' + context.parsed.y + '%';
                               },
                           },
                       },
                       annotation: {
                           annotations: {
                               targetLine: {
                                   type: 'line',
                                   yMin: 95, // Target attendance rate (96%)
                                   yMax: 95, // Target attendance rate (96%)
                                   borderColor: 'rgba(255, 174, 66)',
                                   borderWidth: 2,
                                   label: {
                                       enabled: true,
                                       content: 'Target: 95%', // The label content
                                       position: 'end', // Position of the label relative to the target line (start, center, end)
                                   },
                               },
                           },
                       },
                       datalabels: {
                           anchor: 'top',
                           align: 'top',
                           formatter: function(value, context) {
                               if (value !== null && value !== undefined) {
                                   return value + '%';
                               } else {
                                   console.log('Null or undefined value detected:', value);
                                   return 'N/A';
                               }
                           },
                           color: 'black',
                           font: {
                               weight: 'bold',
                               size: 14
                           }
                       },
                   },
                   // maintainAspectRatio: false,
                   responsive: true,
                   scales: {
                       y: {
                           beginAtZero: true,
                           ticks: {
                               callback: function(value) {
                                   return value + '%';
                               },
                           },
                       },
                   },
               },
               plugins: [ChartDataLabels]
           });

           document.getElementById('toggleWeeklyEff').addEventListener('change', toggleInterval);
       });
   </script>

   <!-- LEAD TIME CHART-->
   <script>
       var batchNumbers = <?php echo json_encode($batchNumbers); ?>;
       var noDays = <?php echo json_encode($noDays); ?>;
       var dateReceived = <?php echo json_encode($dateReceived); ?>;

       // Create an array to store the background colors
       var backgroundColors = [];
       var backgroundBorders = [];

       // Loop through the noDays array and set colors based on the condition
       for (var i = 0; i < noDays.length; i++) {
           if (noDays[i] > 9) {
               // Color for overdue
               backgroundColors.push('rgb(234, 68, 68, 1)'); // Red
               backgroundBorders.push('rgb(238, 238, 228)');
           } else {
               // Color for on-time or not overdue
               backgroundColors.push('rgb(39, 172, 35, 1)'); // Green
               backgroundBorders.push('rgb(238, 238, 228)');
           }
       }

       // Create the chart using Chart.js
       var leadTime = document.getElementById('Leadtime').getContext('2d');
       var batchChart = new Chart(leadTime, {
           type: 'bar',
           axis: 'y',
           data: {
               labels: batchNumbers,
               datasets: [{
                   label: 'Number of Days per Batch number',
                   data: noDays,
                   backgroundColor: backgroundColors,
                   borderColor: backgroundBorders,
                   borderWidth: 1
               }]
           },
           options: {
               responsive: true,
               maintainAspectRatio: false,
               indexAxis: 'y',
               scales: {
                   y: {
                       beginAtZero: true
                   }
               },
               plugins: {
                   tooltip: {
                       callbacks: {
                           label: function(context) {
                               var dateLabel = 'Date Started: ' + dateReceived[context.dataIndex];
                               var daysLabel = 'Number of Days: ' + noDays[context.dataIndex];
                               var remarks = 'Remarks: ';
                               return [dateLabel, daysLabel, remarks];
                           }
                       }
                   }
               }
           }
       });

       // var batchNumbers = <?php echo json_encode($batchNumbers); ?>;
       // var noDays = <?php echo json_encode($noDays); ?>;
       // var dateReceived = <?php echo json_encode($dateReceived); ?>; // Add this line to get dateReceived

       // // Create the chart using Chart.js
       // var leadTime = document.getElementById('Leadtime').getContext('2d');
       // var batchChart = new Chart(leadTime, {
       //     type: 'bar',
       //     axis: 'y',
       //     data: {
       //         labels: batchNumbers,
       //         datasets: [{
       //             label: 'Number of Days per Batch number',
       //             data: noDays,
       //             backgroundColor: [
       //                 'rgb(115, 147, 179)',
       //                 'rgb(54, 69, 79)',
       //                 'rgb(169, 169, 169)',
       //                 'rgb(96, 130, 182)',
       //                 'rgb(128, 128, 128)',
       //                 'rgb(129, 133, 137)'
       //             ],
       //             borderColor: [
       //                 'rgb(115, 147, 179)',
       //                 'rgb(54, 69, 79)',
       //                 'rgb(169, 169, 169)',
       //                 'rgb(96, 130, 182)',
       //                 'rgb(128, 128, 128)',
       //                 'rgb(129, 133, 137)'
       //             ],
       //             borderWidth: 1
       //         }]
       //     },
       //     options: {
       //         responsive: true,
       //         // maintainAspectRatio: false,
       //         indexAxis: 'y',
       //         scales: {
       //             y: {
       //                 beginAtZero: true
       //             }
       //         },
       //         plugins: {
       //             tooltip: {
       //                 callbacks: {
       //                     label: function(context) {
       //                         var dateLabel = 'Date Started: ' + dateReceived[context.dataIndex];
       //                         var daysLabel = 'Number of Days: ' + noDays[context.dataIndex];
       //                         return [dateLabel, daysLabel];
       //                     }
       //                 }
       //             }
       //         }
       //     }
       // });
   </script>

   <!-- BUILD STATUS CHART-->
   <script>
       // Extract batch numbers, SUB_ASSY, and MAIN_ASSY from the data
       var batchNumbers = <?php echo json_encode(array_column($build_status_data, 'batch_no')); ?>;
       var subAssyPercentages = <?php echo json_encode(array_column($build_status_data, 'sub_bp')); ?>;
       var mainAssyPercentages = <?php echo json_encode(array_column($build_status_data, 'mainassy')); ?>;
       var testingPercentages = <?php echo json_encode(array_column($build_status_data, 'testing')); ?>;
       var totalPercentages = <?php echo json_encode(array_column($build_status_data, 'total_count')); ?>;
       // List of colors in RGBA format
       var colorsRgba = [
           '#126ba3',
           '#106093',
           '#0e5682',
           '#0d4b72',
           '#0b4062'
       ];

       // Create the Chart.js chart
       var ctx5 = document.getElementById("buildStatusChart").getContext("2d");
       var myChart = new Chart(ctx5, {
           type: 'bar',
           data: {
               labels: batchNumbers,
               datasets: [{
                   label: 'Build Status',
                   data: totalPercentages,
                   backgroundColor: colorsRgba,
                   borderWidth: 1,
               }, ]
           },
           options: {
               responsive: true,
               maintainAspectRatio: false,
               scales: {
                   y: {
                       beginAtZero: true,
                       ticks: {
                           callback: function(value) {
                               return value + '%';
                           }
                       }
                   }
               },
               plugins: {
                   tooltip: {
                       enabled: true,
                       mode: 'index', // Set tooltip mode to index for stacking
                       intersect: false,
                       callbacks: {
                           label: function(context) {
                               return context.dataset.label + ': ' + context.parsed.y + '%';
                           },
                           title: function(context) {
                               return 'Batch Number: ' + context[0].label;
                           },
                           footer: function(tooltipItems) {
                               var dataIndex = tooltipItems[0].dataIndex;
                               var subAssyPercentage = subAssyPercentages[dataIndex];
                               var mainAssyPercentage = mainAssyPercentages[dataIndex];
                               var testingPercentage = testingPercentages[dataIndex];

                               // Format the tooltip footer with new lines for each percentage
                               return [
                                   'ASSEMBLY: ' + subAssyPercentage + '%',
                                   'INTEGRATION: ' + mainAssyPercentage + '%',
                                   'TESTING: ' + testingPercentage + '%'
                               ];
                           }
                       }
                   }
               },
           }
       });
   </script>

   <!-- MAIN SKILLS MATRIX CHART-->
   <script>
       const smatrix_main = document.getElementById("skill_matrix_main").getContext("2d");
       const labels = <?php echo json_encode($shortcutNames); ?>;
       const datasets_main = [{
           label: 'Level 3',
           data: <?php echo json_encode(array_values($mainlevel3Values)); ?>,
           backgroundColor: 'rgba(84,130,53,255)',
           order: 1
       }, {
           label: 'Level 2',
           data: <?php echo json_encode(array_values($mainlevel2Values)); ?>,
           backgroundColor: 'rgba(255,255,0,255)',
           order: 1
       }, {
           label: 'Level 1',
           data: <?php echo json_encode(array_values($mainlevel1Values)); ?>,
           borderColor: 'rgba(240, 255, 0)',
           backgroundColor: 'rgba(46,117,182,255)',
           order: 1
       }, {
           label: 'Target',
           data: <?php echo json_encode($maintargetValues); ?>,
           borderColor: 'rgba(255, 0, 0)',
           backgroundColor: 'rgba(255, 0, 0)',
           type: 'line',
           stacked: 'combined',
           order: 0
       }];


       new Chart(smatrix_main, {
           type: 'bar',
           data: {
               labels: labels,
               datasets: datasets_main
           },
           options: {
               plugins: {
                   title: {
                       display: true,
                       text: 'Technician`s Skill Level Per Module - <?php echo $month; ?>',
                       color: 'black'
                   }
               },
               responsive: true,
               maintainAspectRatio: false,
               scales: {
                   x: {
                       stacked: true,
                   },
                   y: {
                       stacked: true
                   }
               }
           }
       });
   </script>

   <!-- CABLE SKILLS MATRIX CHART-->
   <script>
       const smatrix_cable = document.getElementById("skill_matrix_cable").getContext("2d");
       const labels_cable = ['Manual Cutting', 'Manual Stripping', 'Manual Crimping', 'Semi-Auto Wire Crimp', 'Machine set Up', 'Soldering', 'Molding', 'Wire Harnessing', 'Final Assy', 'Machine Change-over', 'Labelling', 'Electrical Testing', 'VI', 'Pre Blocking', 'Taping'];
       const datasets_cable = [{
           label: 'Level 3',
           data: <?php echo json_encode(array_values($cablelevel3Values)); ?>,
           backgroundColor: 'rgba(84,130,53,255)',
           order: 1
       }, {
           label: 'Level 2',
           data: <?php echo json_encode(array_values($cablelevel2Values)); ?>,
           backgroundColor: 'rgba(255,255,0,255)',
           order: 1
       }, {
           label: 'Level 1',
           data: <?php echo json_encode(array_values($cablelevel1Values)); ?>,
           borderColor: 'rgba(240, 255, 0)',
           backgroundColor: 'rgba(46,117,182,255)',
           order: 1
       }];


       new Chart(smatrix_cable, {
           type: 'bar',
           data: {
               labels: labels_cable,
               datasets: datasets_cable
           },
           options: {
               plugins: {
                   title: {
                       display: true,
                       text: 'Operator`s Skill Level Per Station - <?php echo $month; ?>',
                       color: 'black'
                   }
               },
               responsive: true,
               maintainAspectRatio: false,
               scales: {
                   x: {
                       stacked: true,
                   },
                   y: {
                       stacked: true
                   }
               }
           }
       });
   </script>