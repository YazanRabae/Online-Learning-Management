var GetEnrollments = {
    OnStart: function () {
        GetEnrollments.GetData();
        $('#applyFilters').on('click', function () {
            GetEnrollments.GetData();
        });
        $('#restFilters').on('click', function () {
            $('#courseNameFilter').val(null);
            $('#studentNameFilter').val(null);
            $('#pageNumber').val(null);
            GetEnrollments.GetData();
        });
        $('#nextPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage + 1);
            GetEnrollments.GetData();
        });
        $('#previousPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage - 1);
            GetEnrollments.GetData();
        });
    },
    GetData: function () {
        var courseNameFilter = $('#courseNameFilter').val();
        var studentNameFilter = $('#studentNameFilter').val();
        var pageNumber = $('#pageNumber').val();

        $.ajax({
            url: window.origin + '/Instructors/GetAllPendingEnrollments',
            type: 'GET',
            dataType: 'json',
            data: {
                courseName: courseNameFilter,
                studentName: studentNameFilter,
                pageNumber: pageNumber,
            },
            success: function (data) {
                var tbody = $('#enrollmentTableBody'); // Target the table body
                tbody.empty(); // Clear any existing content

                GetEnrollments.toggleButton('#nextPage', data.hasNextPage);
                GetEnrollments.toggleButton('#previousPage', data.hasPreviousPage);
                $('#pageNumber').val(data.pageIndex);

                if (data.enrollments.length === 0) {
                    var row = '<tr><td colspan="6" class="text-center">No Pending Enrollments</td></tr>';
                    tbody.append(row);
                    return;
                }
                // Iterate over the returned JSON data array
                $.each(data.enrollments, function (index, enrollment) {

                    var enrollmentDate = new Date(enrollment.addDate).toLocaleDateString();
                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + ((data.pageSize * (data.pageIndex - 1)) + (index + 1)) + '</td>' +  // Row number
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
    toggleButton(selector, enabled) {
        $(selector).toggleClass('disabled', !enabled)
            .css({
                'pointer-events': enabled ? 'auto' : 'none',
                'opacity': enabled ? '1' : '0.6'
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