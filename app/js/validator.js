$(document).ready(function () {
    $("#my_form").submit(function (event) {
        var form = $(this);


        var currentBtn = $(this).find("input[type=submit]:focus");
        if (currentBtn.attr('validate') == 'no') return;
        var currentbtnVal = $(currentBtn).val();
        $(currentBtn).val('...');

        var error = '';
        var validateloop = null;
        if (currentBtn.attr('validate') == 'tab') {
            var parent = $(currentBtn).closest(".form_horizontal");
            validateloop = $(parent).find("input[type=text], input[type=password], input[type=hidden], select, textarea, input[type=file]").each(function (index, element) {
                var temperror = validateControl($(this));
                if (error.length == 0) error = temperror;
            });
        }
        else {
            validateloop = $("input[type=text], input[type=password], input[type=hidden], select, textarea, input[type=file]").each(function (index, element) {
                var temperror = validateControl($(this));
                if (error.length == 0) error = temperror;
            });
        }

        $(currentBtn).val(currentbtnVal);

        if (error.length > 0) {
            event.preventDefault();
            return false;
        }
        else {
            return true;
        }

        /*$.when(validateloop).then(function (x) {
            $(currentBtn).val(currentbtnVal);
            if (error.length > 0) {
                event.preventDefault();
                return false;
            }
            else {
                //document.getElementById("my_form").submit();
                //document.forms['my_form'].submit();
                return true;
            }

            return false;
        });*/
    });

    $('.phone').blur(function () {
        var val = $.trim($(this).val());
        var hasPlus = val.startsWith('+');
        var string = val.replace('(0)', '');
        string = string.replace(/[^0-9]/g, '');
        if (hasPlus) string = '+' + string;
        $(this).val(string);
    });
});

function validatePanel(panelid) {
    var error = '';
    var validateloop = $("#" + panelid).find("input[type=text], input[type=password], input[type=hidden], select, textarea").each(function (index, element) {
        var temperror = validateControl($(this));
        if (error.length == 0) error = temperror;
    });
    return (error.length == 0);
}

function validateControl(input) {
    var error = '';

    var attrs = $(input).attr('data-validate');
    if (attrs === undefined) return error;

    var id = 'err_lbl_' + $(input).attr('id');
    $('#' + id).remove();

    $.ajax({
        type: "POST",
        url: "Services/validation.asmx/Validate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ input: $(input).val(), attributes: attrs }),
        success: function (msg) {
            error = successAjax(input, msg);
            if (error.length > 0) {
                $(input).after("<span id='" + id + "' class='error_class'>" + error + "</span>");
                if ($(input).is(':hidden')) alert("Something went wrong. Please check all tabs to proceed.");
            }
        },
        error: function (jqXHR, exception) {
            failedAjax(jqXHR);
        }
    });

    return error;
}

function successAjax(input, msg) {
    var error = '';
    switch (msg.d[0]) {
        case 'required':
            error = "This field cannot be empty.";
            break;

        case 'email':
            error = "This field should be valid email address.";
            break;

        case 'phone':
            error = "This field should be valid phone number.";
            break;

        case 'minlength':
            error = "This field should be " + msg.d[1] + " characters minimum.";
            break;

        case 'maxlength':
            error = "This field cannot be more than " + msg.d[1] + " characters.";
            break;

        case 'minval':
            error = "This field should be greater than of equal to " + msg.d[1];
            break;

        case 'maxval':
            error = "This field should be less than of equal to " + msg.d[1];
            break;

        case 'pnumber':
            error = "This field should be valid positive number.";
            break;

        case 'number':
            error = "This field should be valid number.";
            break;

        case 'pdecimal':
            error = "This field should be valid positive decimal.";
            break;

        case 'decimal':
            error = "This field should be valid decimal.";
            break;

        case 'date':
            error = "This field should be valid date.";
            break;

        case 'time':
            error = "This field should be valid time.";
            break;
    }
    return error;

}

function failedAjax(xmlRequest) {
    //alert('error');
}