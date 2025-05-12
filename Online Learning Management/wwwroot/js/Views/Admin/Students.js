
var GetStudents = {
    onStart: function () {
        GetStudents.getdata();
        $('#applyFilters').on('click', function () {
            GetStudents.getdata();
        });
        $('#restFilters').on('click', function () {
            $('#filterName').val(null);
            $('#filterEmail').val(null);
            GetStudents.getdata();
        });
    },
    getdata: function () {
        // get filter values
        var usernamefilter = $('#filterName').val();
        var emailfilter = $('#filterEmail').val();

        $.ajax({
            url: window.origin + '/admin/getstudents',
            type: 'get',
            datatype: 'json',
            data: {
                name: usernamefilter,
                email: emailfilter
            },
            success: function (data) {
                var tbody = $('#StudentsTableBody');
                tbody.empty();


                $.each(data, function (index, students) {

                    // construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + (index + 1) + '</td>' +
                        '<td>' + students.name + '</td>' +
                        '<td>' + students.email + '</td>' +
                        '</tr>';


                    tbody.append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('error fetching students:', error);
            }
        });
    }
}
