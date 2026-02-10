<%@ Page Title="VO Approval Edit" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="voapprovaledit.aspx.cs" Inherits="Breederapp.voapprovaledit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <style>
        .fstElement {
            width: 30%;
        }

        .fstResults {
            max-height: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, UsersEditUser %>" runat="server" CssClass="error_class"></asp:Label>
        </h5>
        <br />
        <br />
        <div class="login_form">
            <div class="form_horizontal">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form-group">
                    <div class="form_label col-lg-3 col-md-3 col-sm-3 col-xs-12">
                        <asp:Label ID="lblApprovalGroupLevel1" CssClass="form_label" runat="server"> <span><%= Resources.Resource.ApprovalGroupLevel1 %></span>&nbsp;*</asp:Label>
                    </div>

                    <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                        <select id="ddlUser" class="form_input multipleSelect" multiple runat="server" data-validate="required"></select>
                    </div>
                </div>
                <br />
                <div class="row form-group">
                    <div class="form_label col-lg-3 col-md-3 col-sm-3 col-xs-12">
                        <asp:Label ID="lblApprovalGroupLevel2" CssClass="form_label" runat="server"><span><%= Resources.Resource.ApprovalGroupLevel2 %></span>&nbsp;</asp:Label>
                    </div>

                    <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                        <select id="ddlUser2" class="form_input multipleSelect" multiple runat="server"></select>
                    </div>
                </div>
                <br />
                <div class="row form-group">
                    <div class="form_label col-lg-3 col-md-3 col-sm-3 col-xs-12">
                        <asp:Label ID="lblApprovalGroupLevel3" CssClass="form_label" runat="server"><span><%= Resources.Resource.ApprovalGroupLevel3 %></span>&nbsp;</asp:Label>
                    </div>

                    <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                        <select id="ddlUser3" class="form_input multipleSelect" multiple runat="server"></select>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnback" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
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
