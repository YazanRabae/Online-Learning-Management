//var GetCoursesIns = {
//    OnStart: function () {
//        this.GetData();

//        // Filter logic
//        $('#btnSearch').click(function () {
//            const title = $('#searchTitle').val().toLowerCase();
//            const startDate = $('#searchStartDate').val();
//            const endDate = $('#searchEndDate').val();

//            $('#courseInsTableBody tr').each(function () {
//                const row = $(this);
//                const rowTitle = row.find('td:eq(1)').text().toLowerCase();
//                const rowStart = row.find('td:eq(3)').text(); // already in locale date string
//                const rowEnd = row.find('td:eq(4)').text();

//                const matchTitle = !title || rowTitle.includes(title);
//                const matchStart = !startDate || new Date(rowStart) >= new Date(startDate);
//                const matchEnd = !endDate || new Date(rowEnd) <= new Date(endDate);

//                row.toggle(matchTitle && matchStart && matchEnd);
//            });
//        });

//        // Reset logic
//        $('#btnReset').click(function () {
//            $('#searchTitle').val('');
//            $('#searchStartDate').val('');
//            $('#searchEndDate').val('');
//            $('#courseInsTableBody tr').show(); // Show all rows again
//        });
//    },

//    GetData: function () {
//        $.ajax({
//            url: window.origin + '/Instructors/GetAllCourses',
//            type: 'GET',
//            dataType: 'json',
//            success: function (data) {
//                var tbody = $('#courseInsTableBody');
//                tbody.empty();

//                $.each(data, function (index, course) {
//                    var startDate = new Date(course.startDate).toLocaleDateString();
//                    var endDate = new Date(course.endDate).toLocaleDateString();

//                    if (course.instructorName.includes('@')) {
//                        course.instructorName = course.instructorName.split('@')[0];
//                    }

//                    var descButton = '<button class="btn btn-sm btn-info view-desc-btn" data-description="' +
//                        encodeURIComponent(course.description) + '">View</button>';

//                    var row = '<tr>' +
//                        '<td>' + (index + 1) + '</td>' +
//                        '<td>' + course.title + '</td>' +
//                        '<td>' + course.instructorName + '</td>' +
//                        '<td>' + startDate + '</td>' +
//                        '<td>' + endDate + '</td>' +
//                        '<td>' + course.maxStudents + '</td>' +
//                        '<td>' + course.price.toFixed(2) + '</td>' +
//                        '<td>' + course.courseTime + ' hours</td>' +
//                        '<td>' + descButton + '</td>' +
//                        '<td><button class="btn btn-sm btn-primary view-students-btn" data-course-id="' + course.id + '">View Students</button></td>' +
//                        '</tr>';

//                    tbody.append(row);
//                });

//                // Attach click handler for course description buttons
//                $('.view-desc-btn').on('click', function () {
//                    var fullDescription = decodeURIComponent($(this).data('description'));
//                    $('#descriptionModalBody').text(fullDescription);
//                    var modal = new bootstrap.Modal(document.getElementById('descriptionModal'));
//                    modal.show();
//                });
//            },
//            error: function (xhr, status, error) {
//                console.error('Error fetching courses:', error);
//            }
//        });
//    }
//};

//// View Students Modal Handler (outside object)
//$(document).on("click", ".view-students-btn", function () {
//    var courseId = $(this).data("course-id");

//    $.ajax({
//        url: '/Instructors/GetStudentsByCourse?courseId=' + courseId,
//        type: 'GET',
//        success: function (students) {
//            var $list = $("#studentsList");
//            $list.empty();

//            if (students.length === 0) {
//                $list.append('<li class="list-group-item">No students enrolled.</li>');
//            } else {
//                students.forEach(function (student, index) {
//                    $list.append('<li class="list-group-item">' + (index + 1) + ') ' + student.name + ' (' + student.email + ')</li>');
//                });
//            }

//            $("#studentsModal").modal("show");
//        },
//        error: function () {
//            alert("Error loading students.");
//        }
//    });
//});

var GetCoursesIns = {
    OnStart: function () {
        this.GetData();

        // 🔍 Filter logic
        $('#btnSearch').click(function () {
            const title = $('#searchTitle').val().toLowerCase();
            const startDate = $('#searchStartDate').val();
            const endDate = $('#searchEndDate').val();

            $('#courseInsTableBody tr').each(function () {
                const row = $(this);
                const rowTitle = row.find('td:eq(1)').text().toLowerCase();
                const rowStart = row.find('td:eq(3)').text();
                const rowEnd = row.find('td:eq(4)').text();

                const matchTitle = !title || rowTitle.includes(title);
                const matchStart = !startDate || new Date(rowStart) >= new Date(startDate);
                const matchEnd = !endDate || new Date(rowEnd) <= new Date(endDate);

                row.toggle(matchTitle && matchStart && matchEnd);
            });
        });

        // 🔄 Reset logic
        $('#btnReset').click(function () {
            $('#searchTitle').val('');
            $('#searchStartDate').val('');
            $('#searchEndDate').val('');
            $('#courseInsTableBody tr').show();
        });
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

                // 📘 Description Modal
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

// 👥 View Students Modal Handler
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
                students.forEach(function (student, index) {
                    let statusBadge = '';
                    let actions = '';

                    if (student.status === "Rejected") {
                        statusBadge = `<span class="badge bg-danger ms-2">Rejected</span>`;
                    } else {
                        actions = `<button class="btn btn-sm btn-danger ms-3 reject-btn" data-student-id="${student.studentId}" data-course-id="${courseId}">Reject</button>`;
                    }

                    $list.append(`
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <div>
                                ${index + 1}) ${student.name} (${student.email}) ${statusBadge}
                            </div>
                            <div>${actions}</div>
                        </li>
                    `);
                });
            }

            $("#studentsModal").modal("show");
        },
        error: function () {
            alert("Error loading students.");
        }
    });
});

// ❌ Reject Student Handler
$(document).on("click", ".reject-btn", function () {
    const studentId = $(this).data("student-id");
    const courseId = $(this).data("course-id");

    if (!confirm("Are you sure you want to reject this student?")) return;

    $.ajax({
        url: '/Instructors/RejectStudent',
        type: 'POST',
        data: { studentId: studentId },
        success: function () {
            alert("Student rejected successfully.");
            // Refresh student list
            $(".view-students-btn[data-course-id='" + courseId + "']").click();
        },
        error: function () {
            alert("Error rejecting student.");
        }
    });
});

// 🔁 Initialize on document ready
$(document).ready(function () {
    GetCoursesIns.OnStart();
});
