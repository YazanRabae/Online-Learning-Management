var GetCoursesIns = {
    OnStart: function () {
        this.GetData();
        $(function () {
            $('#startDateRange').daterangepicker({
                opens: 'left',
                locale: {
                    format: 'YYYY-MM-DD',
                    cancelLabel: 'Clear'
                }
            }, function (start, end, label) {
                
            });
        });
        $(function () {
            $('#endDateRange').daterangepicker({
                opens: 'left',
                locale: {
                    format: 'YYYY-MM-DD'
                }
            }, function (start, end, label) {
                
            });
        });
        $('#btnSearch').click(function () {
            GetCoursesIns.GetData();
        });

        $('#btnReset').click(function () {
            $('#searchTitle').val(null);
            $('#pageNumber').val(null);
            GetCoursesIns.GetData();
        });

        $('#nextPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage + 1);
            GetCoursesIns.GetData();
        });

        $('#previousPage').on('click', function () {
            var currentPage = parseInt($('#pageNumber').val(), 10) || 1;
            $('#pageNumber').val(currentPage - 1);
            GetCoursesIns.GetData();
        });

        $(document).on("click", ".view-desc-btn", function () {
            var fullDescription = decodeURIComponent($(this).data("description"));
            $("#descriptionModalBody").text(fullDescription);
            var modal = new bootstrap.Modal(document.getElementById("descriptionModal"));
            modal.show();
        });

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
                                actions = `<button class="btn btn-sm btn-danger ms-3 reject-btn" data-student-id="${student.id}" data-course-id="${courseId}">Reject</button>`;
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

        $(document).on("click", ".reject-btn", function () {
            const studentId = $(this).data("student-id");
            const courseId = $(this).data("course-id");

            if (!confirm("Are you sure you want to reject this student?")) return;

            $.ajax({
                url: '/Instructors/RejectStudent',
                type: 'POST',
                data: {
                    studentId: studentId,
                    courseId: courseId
                },
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
    },

    GetData: function () {
        var searchTitle = $('#searchTitle').val();
        var pageNumber = $('#pageNumber').val();
        var startDateFrom = null;
        var startDateTo = null;
        var endDateFrom = null;
        var endDateTo = null;

        var startPicker = $('#startDateRange').data('daterangepicker');
        if (startPicker) {
            startDateFrom = startPicker.startDate.format('YYYY-MM-DD');
            startDateTo = startPicker.endDate.format('YYYY-MM-DD');
        }

        var endPicker = $('#endDateRange').data('daterangepicker');
        if (endPicker) {
            endDateFrom = endPicker.startDate.format('YYYY-MM-DD');
            endDateTo = endPicker.endDate.format('YYYY-MM-DD');
        }

        $.ajax({
            url: window.origin + '/Instructors/GetAllCourses',
            type: 'GET',
            dataType: 'json',
            data: {
                title: searchTitle,
                startDateFrom: startDateFrom,
                startDateTo: startDateTo,
                endDateFrom: endDateFrom,
                endDateTo: endDateTo,
                pageNumber: pageNumber,
            },
            success: function (data) {
                var tbody = $('#courseInsTableBody');
                tbody.empty();

                $.each(data.courses, function (index, course) {
                    var startDate = new Date(course.startDate).toLocaleDateString();
                    var endDate = new Date(course.endDate).toLocaleDateString();

                    var descButton = '<button class="btn btn-sm btn-info view-desc-btn" data-description="' +
                        encodeURIComponent(course.description) + '">View</button>';

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
                GetCoursesIns.toggleButton('#nextPage', data.hasNextPage);
                GetCoursesIns.toggleButton('#previousPage', data.hasPreviousPage);
                $('#pageNumber').val(data.pageIndex);
                // 📘 Description Modal
                
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
