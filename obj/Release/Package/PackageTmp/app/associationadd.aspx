<%@ Page Title="Add Association" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="associationadd.aspx.cs" Inherits="Breederapp.associationadd" %>

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
            <asp:Label ID="Label2" Text="<%$Resources:Resource, AssociationAdd %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <div class="row form_row">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label4"  runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label7" Text="<%$Resources:Resource, TypeofBreed %>" runat="server" CssClass="form_label"></asp:Label>
                <select id="ddlBreedType" class="multipleSelect" multiple runat="server" datatextfield="namewithbreedname" datavaluefield="id"></select>
                <span class="rule">
                    <asp:Label ID="Label6" Text="<%$Resources:Resource, Ifyouwanttoapplyforalltypes %>" runat="server" CssClass="form_label"></asp:Label></span>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.ContactNumber %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label5" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtEmailAddress" runat="server" data-validate="required email" CssClass="form_input" MaxLength="255"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="lblMandatory" runat="server" CssClass="form_label"><span><%= Resources.Resource.Address %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" data-validate="required"  TextMode="Multiline" MaxLength="255"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label1" Text="<%$Resources:Resource, Website %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form_input" MaxLength="255"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
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
