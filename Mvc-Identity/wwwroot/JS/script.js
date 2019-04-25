// ----------------------- Rewrite the new AJAX code down here --------------------------------------

function CountryDropDownList() {
    $.ajax({
        method: "GET",
        url: '/JSON/GetCountries',
        data: '{}',
        success: function (data) {
            var options = '<option value"Select">Select one</option>';
            for (var i = 0; i < data.length; i++) {
                options += '<option value="' + data[i].id + '">' + data[i].name + '</option>';
            }
            $('#countriesDropDownList').append(options);
        },
        error: function (status) {
            if (status === 'notfound') {
                alert("Your choice was not found. Please refresh.");
            }
            else if (status === 'badrequest') {
                alert("You did something wrong. Please refresh and try again.");
            }
            else {
                alert('error: ' + status);
            }
        }
    });
}


if (URL = "/Person/CreatePerson") {

    $(function () {

        AjaxCall('/JSON/GetCountries', null).done(function (response) {
            if (response.length > 0) {
                $('#countryDropDownList').html('');
                var options = '';
                options += '<option value="Select">Select one</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value"' + response[i].id + '">' + response[i].name + '</option>';
                }
                $('#countryDropDownList').append(options);

            }
        }).fail(function (error) {
            alert(error.StatusText + "what do you want from me");
        });

        $('#countryDropDownList').on("change", function () {
            var country = $('#countryDropDownList').val();
            var obj = country;
            AjaxCall('/JSON/GetCities', obj).done(function (result) {
                if (result.length > 0) {
                    $('#cityDropDownList').html('');
                    var options = '';
                    options += '<option value="Select">Select one</option>';
                    for (var i = 0; i < result.length; i++) {
                        options += 'option value"' + result[i].id + '">' + result[i].name + '</option>';
                    }
                    $('#cityDropDownList').append(options);

                }
            }).fail(function (error) {
                console.log(error.statusText);
                alert(error.StatusText + " Hey :)");
            });
        });
    });
}
function AjaxCall(url, data) {
    return $.ajax({
        url: url,
        type: 'GET',
        data: { country: data },
    });
}
