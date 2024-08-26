var GetInstructorsDrop = {
    OnStart: function () {
        GetInstructorsDrop.GetData();
    },
    GetData: function () {
        $.ajax({
            url: '/Admin/GetInstructors',  // Adjust the URL to match your route
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var select = $('select[name="instructorId"]');
                select.empty(); // Clear any existing options
                select.append('<option  value="" disabled hidden selected>Select Instructor</option>');

                // Populate the dropdown with the instructor data
                $.each(data, function (index, instructor) {
                    select.append('<option value="' + instructor.userName + '">' + instructor.userName + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching instructors:', error);
            }
        });
    }
}
var GetInstructorsDrop = {
    OnStart: function () {
        GetInstructorsDrop.GetData();
    },
    GetData: function () {
        $.ajax({
            url: '/Admin/GetInstructors',  // Adjust the URL to match your route
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var select = $('select[name="instructorId"]');
                select.empty(); // Clear any existing options
                select.append('<option value="" selected >Select Instructor</option>')

                // Populate the dropdown with the instructor data
                $.each(data, function (index, instructor) {
                    select.append('<option value="' + instructor.userName + '">' + instructor.userName + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching instructors:', error);
            }
        });
    }
}
