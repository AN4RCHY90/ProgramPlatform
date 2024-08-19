$(document).ready(function () {
    // Set IsAdmin to false by default
    $('input[name="IsAdmin"][value="false"]').prop('checked', true);
    
    // Function to filter user types based on IsAdmin selection
    function filterUserTypes() {
        var isAdmin = $('input[name="IsAdmin"]:checked').val();
        var userTypeListContainer = $('#userTypeListContainer');
        var userTypeList = $('#UserTypeList');
        
        if (isAdmin === 'true') {
            userTypeListContainer.show();
            userTypeList.find('option').each(function () {
                var userTypeValue = $(this).val();
                if (isAdmin === 'true') {
                    if (userTypeValue.includes('Admin')) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                }
            });
        } else {
            userTypeListContainer.hide();
        }
    }

    // Initial call to filter on page load
    filterUserTypes();

    // Bind filter function to IsAdmin radio button change event
    $('input[name="IsAdmin"]').change(function () {
        filterUserTypes();
    });
});