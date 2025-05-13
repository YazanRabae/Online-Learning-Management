$(document).ready(function () {
    // Search functionality
    $('#btnSearch').click(function () {
        var title = $('#searchTitle').val().toLowerCase();
        var instructor = $('#searchInstructor').val().toLowerCase();
        var price = $('#searchPrice').val();
        var status = $('#courseStatusFilter').val();

        // Filter the courses based on search criteria
        filterCourses(title, instructor, price, status);
    });

    // Reset functionality
    $('#btnReset').click(function () {
        // Clear input fields and dropdown
        $('#searchTitle').val('');
        $('#searchInstructor').val('');
        $('#searchPrice').val('');
        $('#courseStatusFilter').val('');

        // Show all courses again
        filterCourses('', '', '', '');
    });

    function filterCourses(title, instructor, price, status) {
        $('#pendingCoursesSection, #activeCoursesSection, #finishedCoursesSection, #rejectedCoursesSection').each(function () {
            var section = $(this);
            var sectionId = section.attr('id');

            if (status && sectionId.toLowerCase().indexOf(status.toLowerCase()) === -1) {
                section.hide(); 
            } else {
                section.show();
                section.find('.card').each(function () {
                    var card = $(this);
                    var courseTitle = card.find('.card-title').text().toLowerCase();
                    var courseInstructor = card.find('.card-text:contains("Instructor:")').text().toLowerCase();
                    var coursePrice = card.find('.card-text:contains("Price:")').text().toLowerCase();

                    var matchTitle = title === '' || courseTitle.includes(title);
                    var matchInstructor = instructor === '' || courseInstructor.includes(instructor);
                    var matchPrice = price === '' || coursePrice.includes(price);

                    card.toggle(matchTitle && matchInstructor && matchPrice);
                });
            }
        });
    }
});
