<%@ Page Title="Transfer List" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="transferlist.aspx.cs" Inherits="Breederapp.transferlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .doclist li {
            display: inline-block;
            padding-right: 20px;
            padding-bottom: 8px !important;
            word-break: break-all;
        }

            .doclist li a, .doclist li a:hover {
                border: solid 1px #efefef;
                padding: 3px 8px;
                border-radius: 5px;
                display: inline-block;
                color: #333;
                background: #f8f8f8;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Transfer %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="dt_ctrl_panal" style="display: none;">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <ul class="dt_filters pull-left">
                        <li>
                            <label><span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label3" Text="<%$Resources:Resource, Filters %>" runat="server" CssClass="error_class"></asp:Label></label>
                        </li>
                        <li>
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Site %>" runat="server" CssClass="error_class"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtName" runat="server" CssClass="form_element" MaxLength="40"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Button ID="btnApply" runat="server" Text="<%$Resources:Resource, Apply %>" CssClass="form_submit_btn submit_btn_sm" OnClick="btnApply_Click" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="10%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, TransferredBy %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="Label18" Text="<%$Resources:Resource, FileName %>" runat="server"></asp:Label></th>
                            <th width="15%"></th>
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

    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
		 var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetAnimalTransferCount", filter);
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
            getdata3("GetAnimalTransferDetails", pageIndex, filter, '', getDetails_Success);
        }

        function getDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("id").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("processed_transferdate").text() + '</td>';
                    contents += '<td>' + record.find("sender_full_name").text() + '</td>';
                    contents += '<td>' + record.find("animalname").text() + ' - ' + record.find("typename").text() + '</td>';
                    // contents += '<td>' + record.find("quantity").text() + ' ' + record.find("unit").text() + '</td>';
                    var files = record.find("files").text();
                    if (files.length > 0) {
                        contents += '<td style="vertical-align: top;"><ul class="doclist">';
                        var file_spilt = files.split(',');
                        if (file_spilt != null && file_spilt.length > 0) {
                            for (d = 0; d < file_spilt.length; d++) {
                                contents += '<li><a href="../app/viewdocument.aspx?file=' + file_spilt[d] + '" target="_blank">' + file_spilt[d].substring(file_spilt[d].indexOf("_") + 1) + '</a></li>';
                            }
                        }
                        contents += '</ul></td>';
                    }
                    else {
                        contents += '<td>&nbsp;</td>';
                    }

                    if (record.find("status").text() == "0") {
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='singleApprove(\"" + id + "\")' class='btn btn_edit'>Approve</a><a href='javascript:void(0);' onclick='singleReject(\"" + id + "\")' class='btn btn_edit'>Reject</a></div></td>";
                    }
                    else if (record.find("status").text() == "1") {
                        contents += "<td>Approved</td>";
                    } else if (record.find("status").text() == "1") {
                        contents += "<td>Rejected</td>";
                    }
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function singleApprove(id) {
            var confirm_msg = '<%=  Resources.Resource.Approvetherecord %>';
            var answer = confirm(confirm_msg);
           /* var answer = confirm("Are you sure you want to approve the record?")*/
            if (answer) __doPostBack('approve', id);
        }

        function singleReject(id) {
            var confirm_msg = '<%=  Resources.Resource.Rejecttherecord %>';
            var answer = confirm(confirm_msg);
           /* var answer = confirm("Are you sure you want to reject the record?")*/
            if (answer) __doPostBack('reject', id);
        }
    </script>
</asp:Content>
