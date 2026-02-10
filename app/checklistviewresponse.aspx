<%@ Page Title="View Checklist Responses" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="checklistviewresponse.aspx.cs" Inherits="Breederapp.checklistviewresponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .childrow {
            display: none;
        }

        .childtable {
            background-color: #efefef !important;
        }

            .childtable thead {
                background: inherit;
            }

            .childtable tr th, .childtable tr td {
                border-left: 0;
                border-right: 0;
                border-bottom: solid 1px #ddd;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ViewResponse %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <h6>
            <asp:Label ID="lblChecklistName" Text="" runat="server"></asp:Label></h6>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdsort" runat="server" />
                    <table class="table datatable" id="maintable">
                        <thead>
                            <th width="10%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                            <th width="60%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, ResponseRate %>" runat="server"></asp:Label>
                            </th>
                            <th width="30%">&nbsp;</th>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="dt_footer">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            process();
        });

        function process() {
            $('#maintable tbody').html('');

            var formData = {};
            formData['checklistid'] = '<%= ViewState["id"]%>';
            formData['animalid'] = '<%= ViewState["animalid"]%>';
            var filter = JSON.stringify(formData);
            getdata3("GetChecklistSchedules", 1, filter, '', getSchedule_Success);
        }

        function getSchedule_Success(data) {
            $('#maintable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("id").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("procesed_date").text() + '</td>';

                    var assigned_usercount = parseInt(record.find("assigned_usercount").text());
                    var responsecount = parseInt(record.find("responsecount").text());
                    if (isNaN(responsecount)) responsecount = 0;

                    if (isNaN(assigned_usercount) || assigned_usercount == 0) {
                        contents += '<td>No user assigned</td>';
                    }
                    else {
                        var per = parseFloat((responsecount / assigned_usercount) * 100);
                        contents += '<td>' + per.toFixed(2) + '%' + ' - [' + responsecount + '/' + assigned_usercount + ']' + '</td>';
                    }

                    var action_contents = "<a href='javascript:void(0);' data-val='" + id + "' class='btn btn_edit btn_expand'>Expand</a>";
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'>" + action_contents + "</div></td>";

                    contents += '</tr>';
                    contents += '<tr class="childrow"><td>&nbsp;</td><td colspan="6" style="padding:0;"><table id="childtable_' + id + '" class="table datatable childtable"><thead><tr><th width="50%">Name</th><th width="30%">Date</th><th width="20%">&nbsp;</th></tr></thead><tbody></tbody></table></td></tr>';

                    $('#maintable > tbody:last').append(contents);
                });
            }
            else {
                $('#maintable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        $(document).on('click', '.btn_expand', function (event) {
            event.stopPropagation();

            var csdid = $(this).attr('data-val');
            var childrow = $(this).parents('tr').next(".childrow");
            if ($(childrow).is(':hidden') == false) {
                $(this).html('Expand');
                $(childrow).slideToggle('fast');
                return;
            }
            $(this).html('Collapse');
            $(childrow).slideToggle('fast');

            var formData = {};
            formData['scheduledateid'] = csdid;
            formData['animalid'] = '<%= ViewState["animalid"]%>';
            var filter = JSON.stringify(formData);

            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/GetChecklistScheduleResponses",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ page: 1, filter: filter }),
                success: function (msg) {
                    var xmldata = msg.d;
                    var $table = $('#childtable_' + csdid); //  $(this).parents('tr').next(".childrow").find('.childtable tbody');
                    $table.find('tbody').html('');
                    if (xmldata != null && xmldata.length > 2) {
                        var xmlDoc = $.parseXML(xmldata);
                        var xml = $(xmlDoc);
                        var records = xml.find("Table");
                        records.each(function () {
                            var record = $(this);
                            var contents = '<tr>';
                            contents += '<td width="50%">' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                            if (record.find("responseid").text().length > 0) {
                                contents += '<td width="30%">' + record.find("procesed_date").text() + '</td>';
                                contents += "<td width='20%'><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistresponse.aspx?id=" + record.find("securedid").text() + "' target='r' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View Response'><i class='fa-solid fa-file-lines'></i></a></div></td>"
                            }
                            else {
                                contents += '<td width="30%">&nbsp;</td>';
                                contents += '<td width="20%">&nbsp;</td>';
                            }
                            contents += '</tr>';
                            $table.find('tbody').append(contents);
                        });
                    }
                    else {
                        $table.find('tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
                    }
                }
            });
        });
    </script>
</asp:Content>
