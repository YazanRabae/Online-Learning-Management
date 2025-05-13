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

    // Search button click handler
    $('#btnSearch').click(function () {
        const searchTitle = $('#searchTitle').val().toLowerCase();
        const searchInstructor = $('#searchInstructor').val().toLowerCase();
        const searchPrice = parseFloat($('#searchPrice').val()) || 0;

        // Filter courses based on search values
        $('.course-card').each(function () {
            const title = $(this).data('title').toLowerCase();
            const instructor = $(this).data('instructor').toLowerCase();
            const price = parseFloat($(this).data('price'));

            // Check if course matches search criteria
            if ((title.includes(searchTitle) || searchTitle === "") &&
                (instructor.includes(searchInstructor) || searchInstructor === "") &&
                (isNaN(searchPrice) || price <= searchPrice || searchPrice === 0)) {
                $(this).show();  // Show the course card
            } else {
                $(this).hide();  // Hide the course card
            }
        });
    });

    // Reset button click handler
    $('#btnReset').click(function () {
        $('#searchTitle').val('');
        $('#searchInstructor').val('');
        $('#searchPrice').val('');
        $('.course-card').show();  // Show all course cards
    });
});

