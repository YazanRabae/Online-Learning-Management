
var GetInstructors = {
    OnStart: function () {
        GetInstructors.GetData(); // Initial load without filters
        $('#applyFilters').on('click', function () {
            GetInstructors.GetData(); // Load data with current filters
        });
        $('#restFilters').on('click', function () {
            $('#filterUserName').val(null);
            $('#filterEmail').val(null);
            $('#pageNumber').val(null);
            GetInstructors.GetData();
        });
        $('#nextPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage + 1);
            GetInstructors.GetData();
        });
        $('#previousPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage - 1);
            GetInstructors.GetData();
        });
    },
    GetData: function () {
        // Get filter values
        var nameFilter = $('#filterUserName').val();
        var emailFilter = $('#filterEmail').val();
        var pageNumber = $('#pageNumber').val();

        $.ajax({
            url: window.origin + '/Admin/GetInstructors',
            type: 'GET',
            dataType: 'json',
            data: {
                name: nameFilter,
                email: emailFilter,
                pageNumber: pageNumber,
            },
            success: function (data) {
                var tbody = $('#InstructorsTableBody');
                tbody.empty();

                $.each(data.instructors, function (index, instructor) {

                    // Construct each table row
                    var row = '<tr>' +
                        '<td scope="row">' + ((data.pageSize * (data.pageIndex - 1)) + (index + 1)) + '</td>' +  
                        '<td>' + instructor.name + '</td>' +
                        '<td>' + instructor.email + '</td>' +
                        '<td><a type="button" href="/Admin/CreateUser?roleName=' + encodeURIComponent("Instructor") + '&id=' + instructor.id + '" class="btn btn-primary"><i class="fas fa-edit"></i></a></td>' +
                        '</tr>';

                    tbody.append(row);
                });
                GetInstructors.toggleButton('#nextPage', data.hasNextPage);
                GetInstructors.toggleButton('#previousPage', data.hasPreviousPage);
                $('#pageNumber').val(data.pageIndex);
                $('#totalPage').text('Total Pages: ' + data.totalPages);
                $('#divPageNumber').text('Page Number: ' + data.pageIndex);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching instructors:', error); 
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
