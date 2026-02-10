<%@ Page Title="View Response" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="checklistresponse.aspx.cs" Inherits="Breederapp.checklistresponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5>
        <asp:Label ID="lblChecklist" runat="server" CssClass="error_class"></asp:Label></h5>
    <h6>
        <asp:Label ID="lblResponseBy" Text="" runat="server"></asp:Label></h6>

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div id="questions"></div>
        </div>
    </div>

    <script src="js/data.js"></script>

    <script>
        $(document).ready(function () {
            loadQuestions();
        });

        var resId = '<%=  ViewState["id"] %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        var c_Draft = '<%=  Resources.Resource.Draft %>';
        var c_Submitted = '<%=  Resources.Resource.Submitted %>';
        var c_Notsubmitted = '<%=  Resources.Resource.Notsubmitted %>';
        var c_Submitresponce = '<%=  Resources.Resource.Submitresponse %>';
        var c_Viewresponse = '<%=  Resources.Resource.ViewResponse %>';

        function loadQuestions() {
            $('#questions').html('Please wait...');
            getdata3("GetCheckListResponseAnswers", 1, resId, '', GetQuestions_Success);
        }

        function GetQuestions_Success22222(data) {
            $('#questions').html('Please wait...');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    contents = '<tr>';
                    if (record.find("profilepic_file").text().length > 0) contents += '<td><img src="docs/' + record.find("profilepic_file").text() + '" alt="p" class="profilepic" />&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';
                    else contents += '<td><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/>&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';
                    contents += '<td>' + record.find("checklistname").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("categoryname").text() + '</div></td>';
                    contents += '<td>' + record.find("procesed_scheduledate").text() + '</td>';
                    contents += '<td>' + record.find("procesed_responsedate").text() + '</td>';

                    if (record.find("responseid").text().length > 0) {
                        if (record.find("isdraft").text() == '1') {
                            contents += '<td>' + c_Draft + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistsubmitresponse.aspx?id=" + record.find("securedscheduleid").text() + "'>" + c_Submitresponce + "</a></div></td>";
                        }
                        else {
                            contents += '<td>' + c_Submitted + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistresponse.aspx?id=" + record.find("securedid").text() + "'>" + c_Viewresponse + "</a></div></td>";
                        }
                    }
                    else {
                        contents += '<td>' + c_Notsubmitted + '</td>';
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistsubmitresponse.aspx?id=" + record.find("securedscheduleid").text() + "'>" + c_Submitresponce + "</a></div></td>";
                    }
                    contents += '</tr>';

                    $('#assignedtable > tbody:last').append(contents);
                });
            }
            else {
                $('#assignedtable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
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
                    if (type == '3' || type == '4' || type == '5') {
                        if (record.find("optiontext").text().length > 0) contents += '<div>' + record.find("optiontext").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '6') {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div><a href="docs/' + record.find("fieldvalue").text() + '" target="docs">' + record.find("fieldvalue").text().substring(record.find("fieldvalue").text().indexOf("_") + 1) + '</a></div>';
                        else contents += '<div>-</div>'
                    }                   
                    //else {
                    //    if (record.find("fieldvalue").text().length > 0) contents += '<div>' + record.find("fieldvalue").text() + '</div>';
                    //    else contents += '<div>-</div>'
                    //}
                    /* matrix new*/
                    else if (type == '8') {
                        var col_content = "<table class='table datatable' width='100%'>";

                        var col_body = "";
                        var col_header = "";
                        var rowValue = record.find("rowvalue").text(); // no of rows of matrix
                        if (rowValue > 0) {
                            var xmldata_col = getdata("GetChecklistQuestionOptions", 0, record.find("id").text());
                            if (xmldata_col != null && xmldata_col.length > 2) {
                                var xmlDoc = $.parseXML(xmldata_col);
                                var xml = $(xmlDoc);
                                var records = xml.find("Table");

                                col_header += "<thead>";
                                col_header = "<tr>";
                                // col_header += "<th width='30%'>&nbsp;</th>";
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
                            // console.log(userdata);

                            for (var i = 0; i < rowValue; i++) { // Row loop
                                var parsedArray;
                                if (userdata && userdata.data && userdata.data.length > i) parsedArray = userdata.data[i];
                                console.log(parsedArray);

                                col_body += "<tr>";
                                if (xmldata_col != null && xmldata_col.length > 2) {
                                    var xmlDoc_col = $.parseXML(xmldata_col);
                                    var xml_col = $(xmlDoc_col);
                                    var records_col = xml_col.find("Table");

                                    // var testval = (parsedArray && parsedArray.length > 0) ? parsedArray[0] : '-';
                                    // if (!testval || testval.length == 0) testval = '-';
                                    // col_body += "<td>" + testval + "</td>";

                                    records_col.each(function (index, value) {
                                        //var j = index + 1;
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
                        }
                    }
                    else {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div>' + record.find("fieldvalue").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    /*  end*/
                    contents += '</div>';
                    /*if (record.find("additionalcomments").text().length > 0) {
                        contents += '<div class="sub_comments">'
                        contents += record.find("additionalcomments").text();
                        var files = record.find("files").text();
                        if (files.length > 0) {
                            contents += '<ul class="photos">';
                            var file_spilt = files.split(',');
                            if (file_spilt != null && file_spilt.length > 0) {
                                for (d = 0; d < file_spilt.length; d++) {
                                    contents += '<li><div><a href="../viewdocument.aspx?file=' + file_spilt[d] + '" target="_blank"><img src="../images/image_loading.gif" class="lazy-img" data-src="' + file_spilt[d] + '" alt="photo" /></a></div><li>';
                                }
                                contents += '</ul>';
                            }
                            contents += '</div>';
                        }
                    }*/
                    contents += '</div>';
                    $('#questions').append(contents);
                });
            }
            else {
                $('#questions').html("<div class='sec_title'>" + NoRecordsFound + "<div>");
            }
        }
    </script>
</asp:Content>
