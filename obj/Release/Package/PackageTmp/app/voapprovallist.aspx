<%@ Page Title="VO Approval Lis" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="voapprovallist.aspx.cs" Inherits="Breederapp.voapprovallist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, VOApprovalList %>" runat="server" CssClass="error_class"></asp:Label>
        </h5>
        <br />
        <div class="row">

            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="30%">
                                <asp:Label ID="lblLevel1" Text="<%$Resources:Resource, ApprovalGroupLevel1 %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="lblLevel2" Text="<%$Resources:Resource, ApprovalGroupLevel2 %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="lblLevel3" Text="<%$Resources:Resource, ApprovalGroupLevel3 %>" runat="server"></asp:Label></th>
                            <th width="10%"></th>
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
    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>    
        $(document).ready(function () {
            process();
        });

        function process() {
            $('.datatable tbody').html('');
            var xmldata = getdata("GetVOApprovalMatrix", 1, '');
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table1");
                records.each(function () {
                    var record = $(this);
                    var contents = '<tr>';
                    contents += '<td>' + record.find("matrix_name").text() + '</td>';
                    contents += '<td>' + record.find("matrix2_name").text() + '</td>';
                    contents += '<td>' + record.find("matrix3_name").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='voapprovaledit.aspx' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a></div></td>";
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                var contents = '<tr>';
                contents += '<td>-</td>';
                contents += '<td>-</td>';
                contents += '<td>-</td>';
                contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='voapprovaledit.aspx' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a></div></td>";
                contents += '</tr>';

                $('.datatable > tbody:last').append(contents);
            }
        }
    </script>
</asp:Content>
