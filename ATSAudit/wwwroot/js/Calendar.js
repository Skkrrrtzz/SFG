const todaysDate = new Date();
let currentDate = new Date();
let thisYear = todaysDate.getFullYear();
let thisMonth = todaysDate.getMonth();
let thisFirstDay = getFirstDay(thisYear, thisMonth);
let thisTotalDays = getLastDayOfMonth(thisYear, thisMonth);

let auditPlanData;
let monthSpecifier = document.getElementById('monthSpecifier');

function getLastDayOfMonth(year, month) {
    const lastDayOfPreviousMonth = new Date(year, month + 1, 0);
    const lastDayOfMonth = lastDayOfPreviousMonth.getDate();

    return lastDayOfMonth;
}

function getFirstDay(year, month) {
    return new Date(year, month, 1).getDay();
}

function selectMonth(operation) {
    if (operation === '-') thisMonth--;
    else thisMonth++;

    renderCalendar(thisYear, thisMonth, getFirstDay(thisYear, thisMonth), getLastDayOfMonth(thisYear, thisMonth));
}

$('#decrementMonth').click(() => {
    currentDate.setDate(currentDate.getDate() - 30);
    renderCalendar(currentDate);
});

$('#incrementMonth').click(() => {
    currentDate.setDate(currentDate.getDate() + 30);
    renderCalendar(currentDate);
});

async function getAuditPlansByMonth(month) {
    return await fetch('/api/auditplans/' + (month+1), {
        method: "GET",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json"
        },
        // body: JSON.stringify({ month: month }) 
    })
    .then(response => response.json())
    .catch(error => console.log("Error: ", error)); 
}

async function renderCalendar(date) {
    let year = date.getFullYear();
    let month = date.getMonth();
    let firstDay = getFirstDay(year, month);
    let totalDays = getLastDayOfMonth(year, month);

    // Clear cell 
    $('#calendar tbody > tr > td').text('')
                                  .click(() => {})
                                  .removeClass()
                                  .removeAttr('style');

    let plans = await getAuditPlansByMonth(month);
    let currentDayCount = 1;
    let monthSetter = new Date(year, month);
    let displayMonth = monthSetter.toLocaleString('default', { month: 'long'});
    let displayYear = monthSetter.getFullYear();
    monthSpecifier.innerHTML = displayMonth + " " + displayYear;

    $('#calendar tbody > tr').each((i, row) => {
        // console.log('row');
        const week = i;
        $(row).find('td').each((j, cell) => {
            const day = j;

            const cellDate = new Date(year, month, currentDayCount);
            const plansByDate = plans.filter(plan => new Date(plan.targetDate).setHours(0,0,0) == cellDate.setHours(0,0,0));
            const currentDateFlag = cellDate.setHours(0,0,0,0) == todaysDate.setHours(0,0,0,0);
            let invalidDate = false;

            //Current Day
            cell.classList.add('calendar-cell');                

            // All other invalid dates 
            //Days outside of calendar days
            if ((week == 0 && day < firstDay) || (currentDayCount > totalDays)) {
                cell.style.backgroundColor = '#9a9a9a'; 
                invalidDate = true;
                cell.classList.add('invalid');
                return;
            } 
            
            //Weekend
            if ( cellDate.getDay() == 0 || cellDate.getDay() == 6) {
                // cell.classList.add('text-danger');
                cell.classList.add('fw-bolder');
                cell.style.backgroundColor = '#9a9a9a'; 
                cell.style.color = '#7c7c7c';
                invalidDate = true;
                cell.classList.add('invalid');
            } 

            //Grey out previousDay divs
            if (cellDate < todaysDate && !invalidDate) {
                cell.style.backgroundColor = '#cacaca'; 
                cell.style.color = '#6d6d6d';
                invalidDate = true;
                cell.classList.add('invalid');
            }

            //Every other cell is valid and either has no audit plan or has audit plan
            if (plansByDate.length > 0) { //Has Audit Plan
                const status = plansByDate[0].status;
                switch (status) {
                    case 0: // For Approval
                        cell.style.backgroundColor = '#0D6EFD';
                        cell.style.color = '#a2ecff';
                        break;
                    case 1: // Open
                        cell.style.backgroundColor = '#FFC107';
                        cell.style.color = '#9e0000';
                        break;
                    case 2: // Closed
                        cell.style.backgroundColor = '#00BA5A';
                        cell.style.color = '#b1ff89';
                        break;
                    default:
                        break;
                }

                //TODO: Don't know if I should pass the responsibility to Sidebar.js by using .hasAuditPlan and .noAuditPlan
                cell.classList.add('hasAuditPlan');
                cell.onclick = () => {
                    viewAuditPlan({ 
                        plan: plansByDate[0],
                    });
                };
            } else if (!invalidDate) {
                cell.onclick = () => {
                    createAuditPlan({ 
                        date: cellDate.toLocaleDateString('en-US')
                    });
                }; 
            }
            
            if (currentDateFlag) {
                // invalidDate = true;
                cell.classList.add('invalid');
                // cell.classList.add('today');
                cell.style.border = '3px solid #c41414';
                cell.style.color = '#c41414';
                cell.style.fontWeight = 'bold';
            }

            // Has Audit Plan

            //I don't know if cellClickHandler() should do the isEmpty check
            //or renderCalendar() should- brain hurtie :(

            //Base condition(?)
            if (currentDayCount <= totalDays) {
                cell.textContent = currentDayCount;
                currentDayCount++;
            }

        });
    });

    useStateLol();
}

renderCalendar(currentDate);