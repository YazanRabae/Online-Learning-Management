document.addEventListener("DOMContentLoaded", function () {
    var ctx = document.getElementById('courseChart').getContext('2d');

    // Get data from data attributes
    var titles = JSON.parse(document.getElementById('courseChart').dataset.titles);
    var counts = JSON.parse(document.getElementById('courseChart').dataset.counts);

    var courseChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: titles,
            datasets: [{
                label: 'Enrollment Count',
                data: counts,
                backgroundColor: 'rgba(54, 162, 235, 0.5)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                x: {
                    barPercentage: 0.7,
                    categoryPercentage: 1.0,
                    ticks: {
                        autoSkip: false,
                        maxRotation: 45,
                        minRotation: 45
                    }
                },
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            },
            responsive: true,
            maintainAspectRatio: false
        }
    });
});
