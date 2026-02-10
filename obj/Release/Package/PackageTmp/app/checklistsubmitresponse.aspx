<%@ Page Title="Submit Response" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="checklistsubmitresponse.aspx.cs" Inherits="Breederapp.checklistsubmitresponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/bootstrap-timepicker.min.css" rel="stylesheet" />
    <style>
        .customtable tr td {
            border: 0 !important;
        }

            .customtable tr td label {
                font-weight: normal;
                margin-left: 8px;
            }

        .form_input {
            width: 65%;
        }

        .form_input, .fstElement {
            width: 45%;
        }
        /*  new*/
        h3.sec_title {
            color: #333;
            font-size: 14px;
            font-weight: 500;
            margin: 0;
            line-height: 23px;
        }

        .response_text {
            padding: 10px;
            background: #fff;
        }

        .question_section {
            border: solid 1px #e3e3e3;
            border-radius: 5px;
            margin: 15px 15px 25px 0;
            padding: 4px 8px;
        }

        table.table.datatable tr td, table.table.datatable tr th {
            border: 1px solid #ddd !important;
        }

        /*  end*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="lblHeading" Text="<%$Resources:Resource, Submitresponse %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <a href="assignedchecklist.aspx">
                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, Back %>"></asp:Label>
            </a>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <h6>
            <asp:Label ID="lblAnimal" Text="" runat="server"></asp:Label></h6>
        <br />
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <asp:Panel ID="panelView" runat="server">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div id="questions"></div>
            </div>
            <asp:Literal ID="lit" runat="server" Visible="false"></asp:Literal>
        </asp:Panel>
        <asp:Panel ID="panelEdit" runat="server" Visible="false">
            <asp:Panel ID="pnlCustomFieldsEdit" runat="server">
            </asp:Panel>

            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="btnSaveDraft" CssClass="form_button" Text="<%$Resources:Resource, SaveDraft %>" runat="server" OnClick="btnSaveDraft_Click" />&nbsp;
                    <asp:Button ID="btnSubmit" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;
                    <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/bootstrap-timepicker.min.js"></script>
    <script src="js/data.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            $(".time-picker").timepicker({ showInputs: false, showMeridian: false });
            loadQuestions();
        });



        $("#ContentPlaceHolder1_btnSubmit").click(function () {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        });

        $('.custome_file_upload').change(function () {
            var f = $(this);
            var fid = $(f).attr('id').replace('ContentPlaceHolder1_', '');
            // var extns = $(f).attr('data-extensions');

            var fileData = $(f).prop("files")[0];
            console.log(fileData);
            var formData = new window.FormData();
            formData.append("file", fileData);
            // formData.append("extns", extns);

            $.ajax({
                url: 'file_upload_docs.ashx',
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
                    var fileuploadedsuccessfully = '<%= Resources.Resource.Fileuploadedsuccessfully %>';
                    $(hid).val(data);
                    alert(fileuploadedsuccessfully);
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert(fileuploadingerror);
                }
            });
        });
        /* new for readonly*/
        function loadQuestions() {
            $('#questions').html('Please wait...');
            var formData = {};
            formData['scheduledateid'] = '<%= ViewState["id"]%>';
            formData['animalid'] = '<%= ViewState["animalid"]%>';
            formData['userid'] = '<%= this.UserId %>';

            var filter = JSON.stringify(formData);
            getdata3("GetCheckListResponseReadOnlyAnswers", 1, filter, '', GetQuestions_Success);
        }

        function GetQuestions_Success(data) {
            $('#questions').html('Please wait...');

            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                $('#questions').html('');
                var index = 0;
                records.each(function () {
                    var record = $(this);
                    index = index + 1;

                    var id = record.find("id").text();
                    var type = record.find("type").text();

                    var contents = '<div class="question_section"><div class="widget_heading clearfix">';
                    contents += '<h3 class="sec_title">' + index + '. ' + record.find("title").text() + '</h3>';
                    contents += '</div>';
                    contents += '<div class="response_text">';
                    if (type == '3' ) {
                        if (record.find("optiontext").text().length > 0) contents += '<div>' + record.find("optiontext").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '4') {
                        if (record.find("optiontext").text().length > 0) contents += '<div>' + record.find("optiontext").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '5') {
                        if (record.find("optiontext").text().length > 0) contents += '<div>' + record.find("optiontext").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '6') {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div><a href="docs/' + record.find("fieldvalue").text() + '" target="docs">' + record.find("fieldvalue").text().substring(record.find("fieldvalue").text().indexOf("_") + 1) + '</a></div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '8') {

                        var fieldvalue = record.find("fieldvalue").text();
                        if (fieldvalue && fieldvalue.length > 0) {

                            var col_content = "<table class='table datatable' width='100%'>";

                            var col_body = "";
                            var col_header = "";
                            var rowValue = record.find("rowvalue").text();
                            if (rowValue > 0) {
                                var xmldata_col = getdata("GetChecklistQuestionOptions", 0, record.find("id").text());
                                if (xmldata_col != null && xmldata_col.length > 2) {
                                    var xmlDoc = $.parseXML(xmldata_col);
                                    var xml = $(xmlDoc);
                                    var records = xml.find("Table");

                                    col_header += "<thead>";
                                    col_header = "<tr>";

                                    var width = parseFloat(100 / records.length);
                                    records.each(function () {
                                        var record = $(this);
                                        col_header += "<th width='" + width + "%'>" + record.find("optiontext").text() + "</th>";
                                    });
                                    col_header += "</tr>";
                                    col_header += "</thead>";
                                }

                                col_content += col_header;

                                col_body += "<tbody>";
                                var json = record.find("fieldvalue").text();

                                var userdata = JSON.parse(json);

                                for (var i = 0; i < rowValue; i++) {
                                    var parsedArray;
                                    if (userdata && userdata.data && userdata.data.length > i) parsedArray = userdata.data[i];
                                    console.log(parsedArray);

                                    col_body += "<tr>";
                                    if (xmldata_col != null && xmldata_col.length > 2) {
                                        var xmlDoc_col = $.parseXML(xmldata_col);
                                        var xml_col = $(xmlDoc_col);
                                        var records_col = xml_col.find("Table");

                                        records_col.each(function (index, value) {
                                            var testval = (parsedArray && parsedArray.length > index) ? parsedArray[index] : '-';
                                            if (!testval || testval.length == 0) testval = '-';
                                            col_body += "<td>" + testval + "</td>";
                                        });
                                    }
                                    col_body += "</tr>";
                                }
                                col_body += "</tbody>";

                                col_content += col_body;

                                col_content += "</table>";
                                contents += col_content;
                                /*  contents += '<div>' + col_content + '</div>';*/
                            }
                        }
                        else {
                            contents += '<div>-</div>'
                        }
                    }
                    else
                    {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div>' + record.find("fieldvalue").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    contents += '</div>';

                    contents += '</div>';

                    $('#questions').append(contents);
                });
            }
            else {
                $('#questions').html("<div class='sec_title'>" + NoRecordsFound + "<div>");
            }
        }
      /*  end*/
    </script>
</asp:Content>

