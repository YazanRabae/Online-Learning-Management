var GetCourses = {
    OnStart: function () {
        GetCourses.GetData();
        $('#searchButton').on('click', function () {
            GetCourses.GetData();
        });
        $('#restFilters').on('click', function () {
            $('#courseName').val(null);
            $('#instructorEmail').val('').trigger('change');
            $('#pageNumber').val(null);
            GetCourses.GetData();
        });
        $('#nextPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage + 1);
            GetCourses.GetData();
        });
        $('#previousPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage - 1);
            GetCourses.GetData();
        });
    },
    GetData: function () {
        var courseName = $('#courseName').val();
        var instructorId = $('#instructorEmail').val();
        var pageNumber = $('#pageNumber').val();

        $.ajax({
            url: window.origin + '/Admin/GetCourses',
            type: 'GET',
            dataType: 'json',
            data: {
                courseName: courseName,
                instructorId: instructorId,
                pageNumber: pageNumber,
            },
            success: function (data) {
                var tbody = $('#courseTableBody');
                tbody.empty();

                $.each(data.courses, function (index, course) {
                    var startDate = new Date(course.startDate).toLocaleDateString();
                    var endDate = new Date(course.endDate).toLocaleDateString();

                    var row = '<tr>' +
                        '<td>' + ((data.pageSize * (data.pageIndex - 1)) + (index + 1)) + '</td>' +
                        '<td>' + course.title + '</td>' +
                        '<td>' + course.instructorName + '</td>' +
                        '<td>' + startDate + '</td>' +
                        '<td>' + endDate + '</td>' +
                        '<td>' + course.maxStudents + '</td>' +
                        '<td>' + course.price.toFixed(2) + '</td>' +
                        '<td>' + course.courseTime + ' hours</td>' +
                        '<td><button class="btn btn-sm btn-info view-desc-btn" data-description="' + encodeURIComponent(course.description) + '">View</button></td>' +
                        '<td><button class="btn btn-sm btn-primary view-students-btn" data-course-id="' + course.id + '">View Students</button></td>' +
                        '</tr>';

                    tbody.append(row);
                });
                GetCourses.toggleButton('#nextPage', data.hasNextPage);
                GetCourses.toggleButton('#previousPage', data.hasPreviousPage);
                $('#pageNumber').val(data.pageIndex);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching courses:', error);
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
};

var GetInstructorsDrop = {
    OnStart: function () {
        GetInstructorsDrop.GetData();
    },
    GetData: function () {
        $.ajax({
            url: '/Admin/GetInstructors',
            type: 'GET',
            dataType: 'json',
            data: {
                getAll: true,
            },
            success: function (data) {
                var select = $('#instructorEmail');
                select.empty();
                select.append('<option value="" selected>Select Instructor</option>');
                $.each(data.instructors, function (index, instructor) {
                    select.append('<option value="' + instructor.id + '">' + instructor.name + '</option>');
                });

                // Make searchable
                select.select2({
                    placeholder: "Select Instructor",
                    allowClear: true,
                    width: '100%'
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching instructors:', error);
            }
        });
    }
};

// View Students Button
$(document).on("click", ".view-students-btn", function () {
    var courseId = $(this).data("course-id");

    $.ajax({
        url: '/Admin/GetStudentsByCourse?courseId=' + courseId,
        type: 'GET',
        success: function (students) {
            var $list = $("#studentsList");
            $list.empty();

            if (students.length === 0) {
                $list.append('<li class="list-group-item">No students enrolled.</li>');
            } else {
                let count = 1;
                students.forEach(function (student) {
                    $list.append('<li class="list-group-item">' + count + ') ' + student.name + ' (' + student.email + ')</li>');
                    count++;
                });
            }

            $("#studentsModal").modal("show");
        },
        error: function () {
            alert("Error loading students.");
        }
    });
});

// View Description Button
$(document).on("click", ".view-desc-btn", function () {
    var fullDescription = decodeURIComponent($(this).data("description"));
    $("#descriptionModalBody").text(fullDescription);
    var modal = new bootstrap.Modal(document.getElementById("descriptionModal"));
    modal.show();
});

// Initialize
$(document).ready(function () {
    GetCourses.OnStart();
    GetInstructorsDrop.OnStart();
});
