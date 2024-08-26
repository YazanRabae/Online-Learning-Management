
var GetInstructors = {
    OnStart: function () {
        GetInstructors.GetData(); // Initial load without filters
        $('#applyFilters').on('click', function () {
            GetInstructors.GetData(); // Load data with current filters
        });
    },
    GetData: function () {
        // Get filter values
        var userNameFilter = $('#filterUserName').val();
        var emailFilter = $('#filterEmail').val();

        $.ajax({
            url: window.origin + '/Admin/GetInstructors',
            type: 'GET',
            dataType: 'json',
            data: {
                userName: userNameFilter,
                email: emailFilter
            },
            success: function (data) {
                var tbody = $('#InstructorsTableBody'); // Target the table body
                tbody.empty(); // Clear any existing content

                // Iterate over the returned JSON data array
                $.each(data, function (index, instructor) {
                    // Check if the username contains '@'
                    if (instructor.userName.includes('@')) {
                        // Extract the substring before '@'
                        var atIndex = instructor.userName.indexOf('@');
                        instructor.userName = instructor.userName.substring(0, atIndex);
                    }

                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +  // Row number
                        '<td>' + instructor.userName + '</td>' +         // Instructor Name
                        '<td>' + instructor.email + '</td>' +        // Instructor Email
                        '<td>' + (instructor.isActive ? 'Yes' : 'No') + '</td>' +  // IsActive
                        '<td><button class="btn btn-warning">Active</button></td>' + // Action button
                        '</tr>';

                    // Append the constructed row to the table body
                    tbody.append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching instructors:', error);  // Log errors to the console
            }
        });
    }
}
