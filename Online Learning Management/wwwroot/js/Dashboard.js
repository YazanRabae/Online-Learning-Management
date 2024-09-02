var Dashboard = {
    OnStart: function () {
        Dashboard.LoadCourses();
    },
    LoadCourses: function () {
        $.ajax({
            url: '/Student/GetAllCourses',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var coursesContainer = $('#coursesContainer');
                coursesContainer.empty();
                $.each(data, function (index, course) {
                    if (course.instructorName.includes('@')) {
                        var atIndex = course.instructorName.indexOf('@');
                        course.instructorName = course.instructorName.substring(0, atIndex);
                    }
                    var enrollButton = course.isEnrolled
                        ? `<button class="btn btn-secondary" disabled>Already Enrolled</button>`
                        : `<button class="btn btn-primary enroll-btn" data-course-id="${course.id}" onclick="Dashboard.Enrolle(${course.id})">Enroll</button>`;

                    var courseCard = `
                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card shadow-sm border-0 rounded">
                                <img src="data:image/jpeg;base64,${course.imageData}" class="card-img-top" alt="${course.title}">
                                <div class="card-body">
                                    <h5 class="card-title">${course.title}</h5>
                                    <p class="card-text">${course.description}</p>
                                    <p class="card-text">Instructor: ${course.instructorName}</p>
                                    <p class="card-text">Price: ${course.price} JD</p>
                                    <p class="card-text">Date: ${new Date(course.startDate).toLocaleDateString()}</p>
                                    <div class="d-flex justify-content-center">
                                        ${enrollButton}
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
                    coursesContainer.append(courseCard);
                });

            },
            error: function (xhr, status, error) {
                console.error('Error fetching courses:', error);
            }
        });
    },
    Enrolle: function (courseId) {
        $.ajax({
            url: '/Student/AddEnrollment?courseId=' + courseId,
            type: 'POST',
            contentType: 'application/json',
            success: function (data) {
                $('#successEnrollAlert').removeClass('d-none').text('Successfully enrolled in the course!');
                setTimeout(function () {
                    $('#successEnrollAlert').addClass('d-none');
                    Dashboard.LoadCourses(); // Reload courses to update button state
                }, 3000);
            },
            error: function () {
                $('#failEnrollAlert').removeClass('d-none').text('You are already enrolled in this course.');
                setTimeout(function () {
                    $('#failEnrollAlert').addClass('d-none');
                }, 1000);
            }
        });
    }
}
