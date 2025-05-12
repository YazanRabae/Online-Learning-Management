$(document).ready(function () {
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
});
