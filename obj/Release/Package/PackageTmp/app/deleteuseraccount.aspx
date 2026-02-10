<%@ Page Title="Delete My Account" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="deleteuseraccount.aspx.cs" Inherits="Breederapp.deleteuseraccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="Delete My Account" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <h6>
                    <asp:Label ID="Label4" Text="You are about to delete your account. The data will no longer be available and will be permanently deleted. This action cannot be undone" runat="server"></asp:Label>
                </h6>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div style="opacity: 0.7">
                    <asp:Label ID="Label1" Text="Please type DELETE in the box below to continue" runat="server"></asp:Label>

                </div>
                <asp:TextBox ID="txtDelete" runat="server" CssClass="form_input" MaxLength="6" data-validate="required" Width="30%"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Reason %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtReason" runat="server" CssClass="form_input" MaxLength="500" data-validate="required maxlength-500" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="checkbox" id="chkconfirmation" runat="server" />&nbsp;&nbsp;&nbsp;
                <label for="ContentPlaceHolder1_chkconfirmation">
                    <asp:Label ID="Label6" Text="I understand that deleting my account will remove all my data permanently" runat="server"></asp:Label>
                </label>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Button ID="btnSubmit" CssClass="form_button" Text="<%$Resources:Resource, Delete %>" runat="server" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>

    <script src="js/validator.js?100" type="text/javascript"></script>


    <script>
        $("#ContentPlaceHolder1_btnSubmit").click(function () {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        });

    </script>
</asp:Content>
