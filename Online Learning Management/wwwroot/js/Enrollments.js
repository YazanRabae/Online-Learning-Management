var GetEnrollments = {
    OnStart: function () {
        GetEnrollments.GetData();
    },
    GetData: function () {
        $.ajax({
            url: window.origin + '/Instructors/GetAllPendingEnrollments',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var tbody = $('#enrollmentTableBody'); // Target the table body
                tbody.empty(); // Clear any existing content

                // Iterate over the returned JSON data array
                $.each(data, function (index, enrollment) {

                    var enrollmentDate = new Date(enrollment.addDate).toLocaleDateString();
                    if (enrollment.studentName.includes('@')) {
                        var atIndex = enrollment.studentName.indexOf('@');
                        enrollment.studentName = enrollment.studentName.substring(0, atIndex);
                    }
                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +  // Row number
                        '<td>' + enrollment.studentName + '</td>' +    // Student Name
                        '<td>' + enrollment.courseName + '</td>' +     // Course Name
                        '<td>' + enrollmentDate + '</td>' +            // Enrollment Date
                        '<td>' + enrollment.price.toFixed(2) + '</td>'
                        ; // Price formatted to 2 decimals

                    // Log status to verify


                    // Check the enrollment status to determine if action buttons should be displayed
                    if (parseInt(enrollment.status) == 2) { // If not 'Accepted' (status 2)
                        row += '<td>' +
                            '<button class="btn btn-danger btn-sm" onclick="GetEnrollments.Reject(' + enrollment.id + ')">Reject</button>' +
                            '</td>';
                    } else if (parseInt(enrollment.status) == 1) {
                        row += '<td>' +
                            '<button class="btn btn-success btn-sm mr-2" onclick="GetEnrollments.Accept(' + enrollment.id + ')">Accept</button>' +
                            '</td>';
                    }
                    else {
                        row += '<td>' +
                            '<button class="btn btn-success btn-sm mr-2" onclick="GetEnrollments.Accept(' + enrollment.id + ')">Accept</button> | ' +
                            '<button class="btn btn-danger btn-sm" onclick="GetEnrollments.Reject(' + enrollment.id + ')">Reject</button>' +
                            '</td>';
                    }

                    row += '</tr>';

                    // Append the constructed row to the table body
                    tbody.append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching enrollments:', error);  // Log errors to the console
            }
        });
    },
    Accept: function (id) {
        $.ajax({
            url: window.origin + '/Instructors/Accept',
            type: 'POST',
            data: { id: id },
            success: function () {
                GetEnrollments.GetData();  // Refresh the table after accepting
            },
            error: function (xhr, status, error) {
                console.error('Error accepting enrollment:', error);  // Log errors to the console
            }
        });
    },
    Reject: function (id) {
        $.ajax({
            url: window.origin + '/Instructors/Reject',
            type: 'POST',
            data: { id: id },
            success: function () {
                GetEnrollments.GetData();  // Refresh the table after rejecting
            },
            error: function (xhr, status, error) {
                console.error('Error rejecting enrollment:', error);  // Log errors to the console
            }
        });
    }
};