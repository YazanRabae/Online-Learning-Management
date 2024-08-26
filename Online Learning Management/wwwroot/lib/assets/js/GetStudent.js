
var GetStudents = {
    OnStart: function () {
        GetStudents.GetData(); 
        $('#applyFilters').on('click', function () {
            GetStudents.GetData(); 
        });
    },
    GetData: function () {
        // Get filter values
        var userNameFilter = $('#filterName').val();
        var emailFilter = $('#filterEmail').val();

        $.ajax({
            url: window.origin + '/Admin/GetStudents',
            type: 'GET',
            dataType: 'json',
            data: {
                userName: userNameFilter,
                email: emailFilter
            },
            success: function (data) {
                var tbody = $('#StudentsTableBody'); 
                tbody.empty(); 

         
                $.each(data, function (index, Students) {
                  
                    if (Students.userName.includes('@')) {
                        var atIndex = Students.userName.indexOf('@');
                        Students.userName = Students.userName.substring(0, atIndex);
                    }

                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +  
                        '<td>' + Students.userName + '</td>' +         
                        '<td>' + Students.email + '</td>' +      
                        '<td>' + (Students.isActive ? 'Yes' : 'No') + '</td>' +  
                        '<td><button class="btn btn-warning">Active</button></td>' + 
                        '</tr>';

      
                    tbody.append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching Students:', error);  
            }
        });
    }
}
