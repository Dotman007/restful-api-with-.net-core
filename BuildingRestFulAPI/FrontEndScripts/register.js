$(document).ready(function () {
    $("#register").click(function () {
        let customer = {
            firstname: $('#txtFirstName').val(),
            lastname: $('#txtLastName').val(),
            email: $('.txtEmail').val(),
            dob: $('#txtDob').val(),
            telephone: $('#txtTelephone').val(),
            fax: $('#txtFax').val(),
            password: $('#txtPassword').val(),
            confirmpassword: $('#txtConfirmPassword').val(),
            gender: $('input[name="gender"]:checked').val()
        }
        if (customer.password !== customer.confirmpassword) {
            alert("Password and confirm password do not match");
            return;
        }
        $.ajax({
            url: 'https://localhost:44371/api/Customers/RegisterCustomer',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(customer),
            dataType: 'json',
            success: function (result) {
                alert("Customer Added Successfully");
            },
            error: function (err) {
                alert(err.responseText);
            }
        });
    });
});
function loading() {
    $(".loader").fadeOut(2000, function () {
    });
}

function hiding() {
    $(".loader").hide();
}