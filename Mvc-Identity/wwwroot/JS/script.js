// ----------------------- DropdownList of countries ----------------------- \\

$(document).ready(function () {
    $.ajax({
        url: '/JSON/GetCountries',
        method: "GET",
        data: '{}',
        success: function (data) {
            var options = '';
            for (var i = 0; i < data.length; i++) {
                options += '<option value="' + data[i].name + '">' + data[i].name + '</option>';
            }
            $('#countryDropDownList').append(options);
        },
        error: function (status) {
            if (status === 'notfound') {
                alert("Your choice was not found. Please refresh.");
            }
            else if (status === 'badrequest') {
                alert("You did something wrong. Please refresh and try again.");
            }
            else {
                alert('error: ' + status.statusText);
            }
        }
    });
});

// ----------------------- DropdownList of cities ----------------------- \\

$('#countryDropDownList').on("change", function () {
    var country = $('#countryDropDownList').val();

    $.post('/JSON/GetCities',
        {
            countryName: country
        },

        function (data, status) {

            if (status.toString() == "success" || data.length > 0) {

                $('#cityDropDownList').html('');
                var options = '';

                for (var i = 0; i < data.length; i++) {
                    options += '<option value="' + data[i].id + '">' + data[i].name + '</option>';
                }
                $('#cityDropDownList').append(options);
            }
            if (status.toString() == "notfound") {
                alert("notfound");
            }
            else if (status.toString() == "badrequest") {
                alert("badrequest");
            }
            else {
                //alert("error: " + status.toString());
            }
        })
});