var order = '';

function getdata(functionname, page, filter, sort) {
    if ($('#page_loading_box').length > 0) $('#page_loading_box').modal('show');
    var xmldata = null;

    var inputdata = JSON.stringify({ page: page, filter: filter });
    if (sort === undefined) sort = $('#ContentPlaceHolder1_hdsort').val();
    if (sort !== undefined) inputdata = JSON.stringify({ page: page, filter: filter, order: sort });

    $.ajax({
        type: "POST",
        url: "Services/getdata.asmx/" + functionname,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: inputdata,
        success: function (msg) {
            xmldata = msg.d;
        }
    });
    if ($('#page_loading_box').length > 0) $('#page_loading_box').modal('hide');
    return xmldata;
}

function getdata2(functionname, page, filter, sort) {
    var xmldata = null;
    var inputdata = JSON.stringify({ page: page, filter: filter });
    if (sort === undefined) sort = $('#ContentPlaceHolder1_hdsort').val();
    if (sort !== undefined) inputdata = JSON.stringify({ page: page, filter: filter, order: sort });
    $.ajax({
        type: "POST",
        url: "Services/getdata.asmx/" + functionname,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: inputdata,
        success: function (msg) {
            xmldata = msg.d;
        }
    });
    return xmldata;
}

function getdata3(functionname, page, filter, sort, callback) {
    
    var inputdata = JSON.stringify({ page: page, filter: filter });
    if (sort && sort.length > 0) inputdata = JSON.stringify({ page: page, filter: filter, order: sort });
    $.ajax({
        type: "POST",
        url: "Services/getdata.asmx/" + functionname,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: inputdata,
        success: callback
    });
}

function gettotalcount(functionname, filter) {
    var count = 0;
    var inputdata = {};
    if (filter !== undefined) inputdata = JSON.stringify({ filter: filter });
    $.ajax({
        type: "POST",
        url: "Services/getdata.asmx/" + functionname,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: inputdata,
        success: function (msg) {
            count = msg.d;
        }
    });

    return count;
}

function deleteData(functionname, id) {
    var valid = false;  
    var answer = confirm("Are you sure you want to delete the record?")
    if (answer) {
        $.ajax({
            type: "POST",
            url: "Services/getdata.asmx/" + functionname,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ id: id }),
            success: function (msg) {
                if (msg.d == false) {
                    alert("Some unknown error has occurred. Please try again.");
                }
                if (msg.d == true) {
                    valid = true;
                }
                else {
                    alert(msg.d);
                }
                valid = true;
            }
        });
    }

    return valid;
}

String.prototype.nl2br = function () {
    return this.replace(/\n/g, "<br />");
}

$('.sorttable thead th').click(function () {
    $('.fa-long-arrow-up').remove();
    $('.fa-long-arrow-down').remove();

    var thdata = $(this).html();
    if (thdata == "&nbsp;" || thdata.length == 0) return;

    var tempindex = $(this).attr('sort-column-index');
    if (tempindex === undefined || tempindex.length == 0) return;

    var order = $('#ContentPlaceHolder1_hdsort').val();
    if (order.length == 0) {
        order = tempindex + '_0';
        $(this).html(thdata + ' <i class="fa fa-long-arrow-down"></i>');
    }
    else {
        if (order == tempindex + '_0') {
            order = tempindex + '_1';
            $(this).html(thdata + ' <i class="fa fa-long-arrow-up"></i>');
        }
        else {
            order = tempindex + '_0';
            $(this).html(thdata + ' <i class="fa fa-long-arrow-down"></i>');
        }
    }
    $('#ContentPlaceHolder1_hdsort').val(order);
    process(1);
    $('.pagination').bootpag({ page: 1 });
});

$('.custome_file_upload').change(function () {
    var f = $(this);
    var fid = $(f).attr('id').replace('ContentPlaceHolder1_', '');
    var extns = $(f).attr('data-extensions');

    var fileData = $(f).prop("files")[0];
    console.log(fileData);
    var formData = new window.FormData();
    formData.append("file", fileData);
    formData.append("extns", extns);

    $.ajax({
        url: '../generalfileupload.ashx',
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                    if (!isNaN(percentComplete)) {
                        percentComplete = percentComplete * 100;
                        if (percentComplete > 100) percentComplete = 100;
                        var id = 'per_lbl_' + fid;
                        $('#' + id).remove();
                        $(f).after("<span id='" + id + "'>Uploaded: " + percentComplete + "%</span>");
                    }
                }
            }, false);
            return xhr;
        },
        success: function (data) {
            var hid = '#ContentPlaceHolder1_file_' + fid;
            $(hid).val(data);
            alert("The file has been uploaded successfully.");
        },
        error: function (evt, error) {
            if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
            else alert("There was a problem uploading the file. Please try again.");
        }
    });
});

function loadLazyImages() {
    $('.lazy-img').each(function () {
        var img = $(this);
        $.ajax({
            type: "POST",
            url: "../app/Services/getdata.asmx/getimageurl",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'filename': $(this).attr('data-src') }),
            success: function (msg) {
                // alert(msg.d);
                $(img).attr('src', msg.d)
            }
        });
    });
}
