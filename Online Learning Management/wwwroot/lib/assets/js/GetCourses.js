﻿var GetCourses = {
    OnStart: function () {
        GetCourses.GetData();
        $('#searchButton').on('click', function () {
            GetCourses.GetData();
        });
    },
    GetData: function () {
        var courseName = $('#courseName').val();
        var instructorId = $('#instructorId').val();
        $.ajax({
            url: window.origin + '/Admin/GetCourses',
            type: 'GET',
            dataType: 'json',
            data: {
                courseName: courseName,
                instructorId: instructorId
            },
            success: function (data) {

                var tbody = $('#courseTableBody'); // Target the table body
                tbody.empty(); // Clear any existing content

                // Iterate over the returned JSON data array
                $.each(data, function (index, course) {


                    // Parse dates if necessary
                    var startDate = new Date(course.startDate).toLocaleDateString();
                    var endDate = new Date(course.endDate).toLocaleDateString();

                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +  // Row number
                        '<td>' + course.title + '</td>' +             // Course Title
                        '<td>' + course.description + '</td>' +       // Course Description
                        '<td>' + course.instructorName + '</td>' +    // Instructor Name
                        '<td>' + startDate + '</td>' +                // Start Date
                        '<td>' + endDate + '</td>' +                  // End Date
                        '<td>' + course.maxStudents + '</td>' +       // Max Students
                        '<td>' + course.price.toFixed(2) + '</td>' +  // Price, formatted to 2 decimals
                        '<td>' + course.courseTime + ' hours</td>' +  // Course Time in hours
                        '</tr>';

                    // Append the constructed row to the table body
                    tbody.append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching courses:', error);  // Log errors to the console
            }
        });
    }

}

