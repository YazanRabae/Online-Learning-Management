var GetCoursesIns = {
    OnStart: function () {
        GetCoursesIns.GetData();
    },
    GetData: function () {
        $.ajax({
            url: window.origin + '/Instructors/GetAllCourses',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var tbody = $('#courseInsTableBody');
                tbody.empty();

                $.each(data, function (index, course) {
                    var startDate = new Date(course.startDate).toLocaleDateString();
                    var endDate = new Date(course.endDate).toLocaleDateString();

                    if (course.instructorName.includes('@')) {
                        course.instructorName = course.instructorName.split('@')[0];
                    }

                    var shortDesc = course.description.length > 30
                        ? course.description.substring(0, 30) + '...'
                        : course.description;

                    var descButton = '<button class="btn btn-sm btn-info view-desc-btn" data-description="' +
                        encodeURIComponent(course.description) + '">View</button>';

                    var row = '<tr>' +
                        '<td>' + (index + 1) + '</td>' +
                        '<td>' + course.title + '</td>' +
                        '<td>' + course.instructorName + '</td>' +
                        '<td>' + startDate + '</td>' +
                        '<td>' + endDate + '</td>' +
                        '<td>' + course.maxStudents + '</td>' +
                        '<td>' + course.price.toFixed(2) + '</td>' +
                        '<td>' + course.courseTime + ' hours</td>' +
                        '<td>' + descButton + '</td>' +
                        '<td><button class="btn btn-sm btn-primary view-students-btn" data-course-id="' + course.id + '">View Students</button></td>' +
                        '</tr>';

                    tbody.append(row);
                });

                // Attach click handler for the view buttons
                $('.view-desc-btn').on('click', function () {
                    var fullDescription = decodeURIComponent($(this).data('description'));
                    $('#descriptionModalBody').text(fullDescription);
                    var modal = new bootstrap.Modal(document.getElementById('descriptionModal'));
                    modal.show();
                });
            },
            error: function (xhr, status, error) {
                console.error('Error fetching courses:', error);
            }
        });
    }
};
// View Students Button
$(document).on("click", ".view-students-btn", function () {
    var courseId = $(this).data("course-id");

    $.ajax({
        url: '/Instructors/GetStudentsByCourse?courseId=' + courseId,
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