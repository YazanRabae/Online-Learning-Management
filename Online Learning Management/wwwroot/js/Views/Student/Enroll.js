$(document).ready(function () {
    // Enroll button click handler
    $('.enroll-btn').click(function () {
        const courseId = $(this).data('course-id');
        $.ajax({
            url: '/Student/AddEnrollment?courseId=' + courseId,
            type: 'POST',
            contentType: 'application/json',
            success: function () {
                $('#successEnrollAlert').removeClass('d-none').text('Successfully enrolled!');
                setTimeout(function () {
                    $('#successEnrollAlert').addClass('d-none');
                    location.reload(); // reload to remove enrolled course
                }, 1200);
            },
            error: function () {
                $('#failEnrollAlert').removeClass('d-none').text('You are already enrolled or something went wrong.');
                setTimeout(function () {
                    $('#failEnrollAlert').addClass('d-none');
                }, 1200);
            }
        });
    });

    // Search functionality
    $('#btnSearch').click(function () {
        var title = $('#searchTitle').val().toLowerCase();
        var instructor = $('#searchInstructor').val().toLowerCase();
        var price = $('#searchPrice').val();

        // Filter the courses based on search criteria
        filterCourses(title, instructor, price);
    });

    // Reset functionality
    $('#btnReset').click(function () {
        // Clear input fields and dropdown
        $('#searchTitle').val('');
        $('#searchInstructor').val('');
        $('#searchPrice').val('');

        // Show all courses again
        filterCourses('', '', '', '');
    });

    function filterCourses(title, instructor, price) {
        // Convert input parameters to lowercase for case-insensitive comparison
        title = title.toLowerCase();
        instructor = instructor.toLowerCase();
        price = price.toLowerCase();

        // Iterate over each course card
        $('.course-card').each(function () {
            var card = $(this);
            var courseTitle = card.data('title').toLowerCase();
            var courseInstructor = card.data('instructor').toLowerCase();
            var coursePrice = card.data('price').toString().toLowerCase();

            // Check if the card matches the filter criteria
            var matchTitle = title === '' || courseTitle.includes(title);
            var matchInstructor = instructor === '' || courseInstructor.includes(instructor);
            var matchPrice = price === '' || coursePrice.includes(price);

            // Show or hide the card based on the match result
            card.toggle(matchTitle && matchInstructor && matchPrice);
        });
    }
});

