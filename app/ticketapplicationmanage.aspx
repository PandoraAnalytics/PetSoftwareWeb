<%@ Page Title="Manage Application" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="ticketapplicationmanage.aspx.cs" Inherits="Breederapp.ticketapplicationmanage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ManageApplication %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Application %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAdd_Click" />&nbsp;&nbsp;
                         <asp:LinkButton ID="btnCancel" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnCancel_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js?123"></script>
</asp:Content>
