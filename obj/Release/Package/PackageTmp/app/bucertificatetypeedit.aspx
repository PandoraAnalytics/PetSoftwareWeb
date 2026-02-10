<%@ Page Title="Certificate Type Edit" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucertificatetypeedit.aspx.cs" Inherits="Breederapp.bucertificatetypeedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <style>
        .fstElement {
            width: 50%;
        }

        .form_input {
            width: 50%;
        }

        .top_btn {
            margin-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label2" Text="<%$Resources:Resource, CertificateTypeEdit %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <div class="row form_row">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.CertificateType %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtCertificateType" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>

            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label7" Text="<%$Resources:Resource, TypeofBreed %>" runat="server" CssClass="form_label"></asp:Label>
                <select id="ddlBreedType" class="multipleSelect" multiple runat="server" datatextfield="namewithbreedname" datavaluefield="id"></select>
                <span class="rule">
                    <asp:Label ID="Label3" Text="<%$Resources:Resource, Ifyouwanttoapplyforalltypes %>" runat="server" CssClass="form_label"></asp:Label></span>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="lblMandatory" runat="server" CssClass="form_label"><span><%= Resources.Resource.Mandatory %></span>&nbsp;*</asp:Label>
                <asp:DropDownList ID="ddlMandatory" runat="server" CssClass="form_input" data-validate="required">
                    <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.NeedAssociationApproval %></span>&nbsp;*</asp:Label>
                <asp:DropDownList ID="ddlApproval" runat="server" CssClass="form_input" data-validate="required">
                    <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
                <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        $('.multipleSelect').fastselect({
            placeholder: '<%=  Resources.Resource.ChooseOption %>',
            noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
        });
    </script>
</asp:Content>

