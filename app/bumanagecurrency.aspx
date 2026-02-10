<%@ Page Title="Manange Currency" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bumanagecurrency.aspx.cs" Inherits="Breederapp.bumanagecurrency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 60%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="Manage Currency" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <input type="hidden" id="hdfilter" runat="server" />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSubmit" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;
                    </div>
                </div>
            </div>
            <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                <br />
                <table class="table datatable">
                    <thead>
                        <th width="20%">
                            <asp:Label ID="lblName" Text="<%$Resources:Resource,  Name%>" runat="server"></asp:Label></th>
                        <th width="10%">&nbsp;</th>
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

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js?101"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            process(1);
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetAllBUCurrency", pageIndex, filter, '', GetBUCurrency_Success);
        }

        function GetBUCurrency_Success(data) {
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
                    contents += '<td>' + record.find("currencyname").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='GetCurrency(" + id + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(" + id + ")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteBUCurrency", id);
            var u = window.location.href;
            window.location = u;
        }

        function GetCurrency(id) {
            $("#ContentPlaceHolder1_hdfilter").val('');
            $("#ContentPlaceHolder1_txtName").val('');

            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/GetBUCurrency",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                success: function (msg) {
                    if (msg != null && msg.d != null) {
                        var ppe = JSON.parse(msg.d);
                        $("#ContentPlaceHolder1_txtName").val(ppe.name);
                        $("#ContentPlaceHolder1_hdfilter").val(ppe.id);
                        window.scrollTo(0, 0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var unknownerror = '<%= Resources.Resource.error %>';
                    alert(unknownerror);
                }
            });
        }
    </script>

</asp:Content>

