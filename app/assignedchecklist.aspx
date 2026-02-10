<%@ Page Title="Checklist Assigned To Me" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="assignedchecklist.aspx.cs" Inherits="Breederapp.assignedchecklist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .profilepic {
            border-radius: 50%;
            width: 24px;
            height: 24px;
            border-radius: 50%;
            border: 1px solid #ddd;
        }
        .equipment_img_parent {
            width: 60px;
            height: 60px;
            text-align: center;
            padding: 0;
            line-height: 40px;
            text-indent: 0;
        }

        .equipment_img {
            width: 15%;
            height: 100%;
            border-radius: 50%;
            border: 1px solid #efefef;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ChecklistAssigned %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
        </div>
        <div class="clearfix"></div>
        <div class="alert alert-info" role="alert">
            <asp:Label ID="lblWarning" runat="server" Text="<%$Resources:Resource, Checklistsuptotoday %>"></asp:Label>
        </div>       
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="main_container">
                    <div class="row form_row">
                        <table class="table datatable" id="assignedtable">
                            <thead>
                                <th width="20%">
                                    <asp:Label ID="lblName1" Text="<%$Resources:Resource, Animal %>" runat="server"></asp:Label></th>
                                <th width="20%">
                                    <asp:Label ID="lblType" Text="<%$Resources:Resource, Checklist %>" runat="server"></asp:Label></th>
                                <th width="15%">
                                    <asp:Label ID="Label1" Text="<%$Resources:Resource, AssignedDate %>" runat="server"></asp:Label></th>
                                <th width="15%">
                                    <asp:Label ID="Label2" Text="<%$Resources:Resource, ResponseDate %>" runat="server"></asp:Label></th>
                                <th width="15%">
                                    <asp:Label ID="Label3" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label></th>
                                <th width="15%">&nbsp;</th>
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
    </div>

    <script src="js/data.js"></script>
    <script>
        var filter = '';
        var c_Draft = '<%=  Resources.Resource.Draft %>';
        var c_Submitted = '<%=  Resources.Resource.Submitted %>';
        var c_NotSubmitted = '<%=  Resources.Resource.Notsubmitted %>';
        var c_SubmitResponse = '<%=  Resources.Resource.Submitresponse %>';
        var c_ViewResponse = '<%=  Resources.Resource.ViewResponse %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            showassignedchecklist();
        });

        function showassignedchecklist() {
            var userid = '<%= this.UserId %>';
            $('#assignedtable tbody').html('');
            getdata3("GetAllChecklistsForUsers", 1, userid, '', GetAssignedChecklist_Success);
        }

        var m_viewresponse = '<%=  Resources.Resource.ViewResponse %>';
        function GetAssignedChecklist_Success(data) {
            $('#assignedtable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    contents = '<tr>';

                    if (record.find("profilepic_file").text().length > 0) contents += '<td><img src="../images/image_loading.gif" class="lazy-img equipment_img" data-src="' + record.find(" profilepic_file").text() + '" class="equipment_img alt="x">&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';
                    else contents += '<td><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/>&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';

                    //if (record.find("profilepic_file").text().length > 0) contents += '<td><img src="docs/' + record.find("profilepic_file").text() + '" alt="p" class="profilepic" />&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';
                    //else contents += '<td><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/>&nbsp;' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("animaltype").text() + '</div></td>';
                    contents += '<td>' + record.find("checklistname").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("categoryname").text() + '</div></td>';
                    contents += '<td>' + record.find("procesed_scheduledate").text() + '</td>';
                    contents += '<td>' + record.find("procesed_responsedate").text() + '</td>';

                    if (record.find("responseid").text().length > 0) {
                        if (record.find("isdraft").text() == '1') {
                            contents += '<td>' + c_Draft + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistsubmitresponse.aspx?id=" + record.find("securedscheduleid").text() + "&aid=" + record.find("securedanimalid").text() + "'>" + c_SubmitResponse + "</a></div></td>";
                        }
                        else {
                            contents += '<td>' + c_Submitted + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistresponse.aspx?id=" + record.find("securedid").text() + "' target='r'>" + c_ViewResponse + "</a></div></td>";
                        }
                    }
                    else {
                        contents += '<td>' + c_NotSubmitted + '</td>';
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistsubmitresponse.aspx?id=" + record.find("securedscheduleid").text() + "&aid=" + record.find("securedanimalid").text() + "'>" + c_SubmitResponse + "</a></div></td>";
                    }
                    contents += '</tr>';

                    $('#assignedtable > tbody:last').append(contents);
                });
            }
            else {
                $('#assignedtable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
            loadLazyImages();
        }

        function confirm_delete() {
            var confirm_msg = '<%=  Resources.Resource.DeleteRecordConfirmationMessage %>';
            var answer = confirm(confirm_msg);
            if (answer) {
                $.ajax({
                    type: "POST",
                    url: "Services/getdata.asmx/UnassignedAnimalChecklist",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ id: id }),
                    success: function (msg) {
                        showassignedchecklist();
                    }
                });
            }
        }

        $(document).on('change', '.call-checkbox', function (e) {
            var listArray = $("#ContentPlaceHolder1_cplist").val().split(';');

            if ($(this).is(':checked')) {
                listArray.push($(this).val());
            }
            else {
                listArray.remove($(this).val());
            }
            $("#ContentPlaceHolder1_cplist").val(listArray.join(';'));
        });

        Array.prototype.remove = function (v) { this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1); }
    </script>
</asp:Content>
