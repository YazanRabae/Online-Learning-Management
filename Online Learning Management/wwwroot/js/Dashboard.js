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
                var coursesContainer = $('#coursesContainer'); // Corrected ID
                coursesContainer.empty();
                debugger
                $.each(data, function (index, course) {

                    if (course.instructorName.includes('@')) {
                        // Extract the substring before '@'
                        var atIndex = course.instructorName.indexOf('@');
                        course.instructorName = course.instructorName.substring(0, atIndex);
                    }
                    var courseCard = `
                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card shadow-sm border-0 rounded">
                                <img src="data:image/jpeg;base64,${course.imageData}" class="card-img-top" alt="${course.title}">
                                <div class="card-body">
                                    <h5 class="card-title">${course.title}</h5>
                                    <p class="card-text">${course.description}</p>
                                         <p class="card-text">instructorName :${course.instructorName}</p>
                                         <p class="card-text">Price: ${course.price}JD</p>
                                    <p class="card-text">Date: ${new Date(course.startDate).toLocaleDateString()}</p>
                                    <div class="d-flex justify-content-center">
                                    <button class="btn btn-primary enroll-btn" data-course-id="${course.id}" onclick="Dashboard.Enrolle(${course.id})">Enroll</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
                    coursesContainer.append(courseCard);
                });

                //// Add click event listener for enroll buttons
                //$('.enroll-btn').on('click', function () {
                //    var courseId = $(this).data('course-id');
                //    // Simulate enrollment
                //    Dashboard.Enrolle(courseId);
                //});
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
            data: JSON.stringify({ CourseId: courseId }), // Send courseId in JSON format
            success: function (data) {
                $('#successEnrollAlert').removeClass('d-none').text('Successfully enrolled in the course!');
                setTimeout(function () {
                    $('#successEnrollAlert').addClass('d-none');
                }, 5000); // Hide alert after 3 seconds
            },
            error: function () {
                $('#failEnrollAlert').removeClass('d-none').text('You are already enrolled in this course.');
                setTimeout(function () {
                    $('#failEnrollAlert').addClass('d-none');
                }, 5000); // Hide alert after 3 seconds
            }
        });
    }
}

