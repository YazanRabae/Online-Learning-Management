
var GetStudents = {
    onStart: function () {
        GetStudents.getdata();
        $('#applyFilters').on('click', function () {
            GetStudents.getdata();
        });
        $('#restFilters').on('click', function () {
            $('#filterName').val(null);
            $('#filterEmail').val(null);
            $('#pageNumber').val(null);
            GetStudents.getdata();
        });
        $('#nextPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage + 1);
            GetStudents.getdata();
        });
        $('#previousPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage - 1);
            GetStudents.getdata();
        });
    },
    getdata: function () {
        // get filter values
        var usernamefilter = $('#filterName').val();
        var emailfilter = $('#filterEmail').val();
        var pageNumber = $('#pageNumber').val();

        $.ajax({
            url: window.origin + '/admin/getstudents',
            type: 'get',
            datatype: 'json',
            data: {
                name: usernamefilter,
                email: emailfilter,
                pageNumber : pageNumber,
            },
            success: function (data) {
                var tbody = $('#StudentsTableBody');
                tbody.empty();

                $.each(data.students, function (index, students) {

                    // construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + ((data.pageSize * (data.pageIndex - 1)) + (index + 1)) + '</td>' +
                        '<td>' + students.name + '</td>' +
                        '<td>' + students.email + '</td>' +
                        '<td><a type="button" href="/Admin/CreateUser?roleName=' + encodeURIComponent("Student") + '&id=' + students.id + '" class="btn btn-primary"><i class="fas fa-edit"></i></a></td>' +
                        '</tr>';


                    tbody.append(row);
                });
                GetStudents.toggleButton('#nextPage', data.hasNextPage);
                GetStudents.toggleButton('#previousPage', data.hasPreviousPage);
                $('#pageNumber').val(data.pageIndex);
                $('#totalPage').text('Total Pages: ' + data.totalPages);
                $('#divPageNumber').text('Page Number: ' + data.pageIndex);
            },
            error: function (xhr, status, error) {
                console.error('error fetching students:', error);
            }
        });
    },
    toggleButton(selector, enabled) {
        $(selector).toggleClass('disabled', !enabled)
            .css({
                'pointer-events': enabled ? 'auto' : 'none',
                'opacity': enabled ? '1' : '0.6'
            });
    }
}
