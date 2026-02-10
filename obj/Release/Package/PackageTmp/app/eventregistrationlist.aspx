<%@ Page Title="Event Registration List" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="eventregistrationlist.aspx.cs" Inherits="Breederapp.eventregistrationlist" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, EventRegistrationList %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <a href="eventslist.aspx" class="edit_profile_link">
                <asp:Label ID="Label4" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label>
            </a>
        </div>
        <div class="clearfix"></div>
        <br />
        <h5>
            <asp:Label ID="lblEventTitle" Text="" runat="server" CssClass="error_class"></asp:Label></h5>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <table class="table datatable">
                        <thead>
                            <th width="30%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, ContactNumber %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                            <th width="10%">
                                <asp:Label ID="Label3" Text="" runat="server"></asp:Label></th>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="dt_footer">
                        <div class="pagination float-end">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_otherinfo" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label6" Text="<%$Resources:Resource, Eventotherinfo %>" CssClass="error_class" runat="server"></asp:Label></h6>
                    <div class="login_form" style="overflow-y:scroll;height:400px;">
                        <div class="row form_row">
                            <div id="questions"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js?10"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {

            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetEventRegistrationsCount", filter);
            process(1);
            $('.pagination').bootpag({
                total: totalcount,
                page: 1,
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on("page", function (event, num) {
                process(num);
            });
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetEventRegistrationsDetails", pageIndex, filter, '', getDetails_Success);
        }

        function getDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;

            var eventid = '<%= ViewState["id"].ToString() %>';

            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("fname").text() + " " + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("phone").text() + '</td>';
                    contents += '<td>' + record.find("procesed_date").text() + '</td>';
                    contents += '<td><div class="btn-group btn-group-sm dt_action_btns"><a href="javascript:void(0);" onclick="opendetails(' + record.find("eventregistrationid").text() + ')" class="btn btn_edit"><i class="fa-solid fa-list"></i></a></div></td>';
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }


        function opendetails(eventid) {
            $('#modal_otherinfo').modal('show');
            loadQuestions(eventid);
        }

        function loadQuestions(registrationid) {
            $('#questions').html('Please wait...');
            getdata3("GetEventRegistrationOtherDetails", 1, registrationid, '', getOtherField_Success);
        }

        function getOtherField_Success(data) {
            $('#questions').html('');
            var xmldata = data.d;

            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function (index) {
                    index = index + 1;

                    var record = $(this);
                    var type = record.find("type").text();

                    var contents = '<div class="question_section"><div class="widget_heading clearfix">';
                    contents += '<h3 class="sec_title">' + index + '. ' + record.find("title").text() + '</h3>';
                    contents += '</div>';
                    contents += '<div class="response_text">';
                    if (type == '3' || type == '4' || type == '5') {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div>' + record.find("optiontext").text() + '</div>';
                        else contents += '<div>-</div>'
                    }
                    else if (type == '6') {
                        if (record.find("fieldvalue").text().length > 0) contents += '<div><a href="../docs/' + record.find("fieldvalue").text() + '" target="docs">' + record.find("fieldvalue").text() + '</a></div>';
                        else contents += '<div>-</div>'
                    }
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
                    contents += '</div>';
                    contents += '</div>';
                    $('#questions').append(contents);
                });
            }
            else {
                $('#questions').html("-");
            }
        }
    </script>

</asp:Content>
