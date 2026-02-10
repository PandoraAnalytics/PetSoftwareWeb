<%@ Page Title="Users - Edit User" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="ticketuseredit.aspx.cs" Inherits="Breederapp.ticketuseredit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <style>
        .form_input, .fstElement {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, UsersEditUser %>" runat="server" CssClass="error_class"></asp:Label>
         <%--   &nbsp;<asp:Label ID="lblEventNM" Text="" runat="server" CssClass="h6"></asp:Label>--%></h5>
      <%--  <br />--%>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblName" Text="<%$Resources:Resource, Name %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblFullName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" Text="<%$Resources:Resource, Permission %>" runat="server" CssClass="form_label"></asp:Label>
                        <select id="ddlPermission" multiple runat="server" class="form_input multipleSelect" data-validate="required" datatextfield="name" datavaluefield="id"></select>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAdd_Click" />
                        &nbsp;&nbsp;
                         <asp:LinkButton ID="btnCancel" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnCancel_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js?123"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script>
        $('.multipleSelect').fastselect({
            placeholder: '<%=  Resources.Resource.ChooseOption %>',
            noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
        });
    </script>
</asp:Content>
