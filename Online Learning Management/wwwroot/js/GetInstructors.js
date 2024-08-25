var GetInstructors = {
    OnStart: function () {
        GetInstructors.GetData();
    },
    GetData: function () {
        $.ajax({
            url: window.origin + '/Admin/GetInstructors',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var tbody = $('#InstructorsTableBody'); // Target the table body
                tbody.empty(); // Clear any existing content

                // Iterate over the returned JSON data array
                $.each(data, function (index, instructor) {
                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +  // Row number
                        '<td>' + instructor.userName + '</td>' +         // Instructor Name
                        '<td>' + instructor.email + '</td>' +        // Instructor Email
                        '<td>' + (instructor.isActive ? 'Yes' : 'No') + '</td>' +  // IsActive
                        '<td><button class="btn btn-warning">Edit</button></td>' + // Action button
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
