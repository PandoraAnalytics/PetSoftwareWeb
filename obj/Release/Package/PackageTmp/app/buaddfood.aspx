<%@ Page Title="Breed - Manage Food" Language="C#" MasterPageFile="bubreeder.master" AutoEventWireup="true" CodeBehind="buaddfood.aspx.cs" Inherits="Breederapp.buaddfood" %>

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
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, FoodDetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <br />
        <asp:Panel ID="panelView" runat="server">
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Food %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblFood" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label11" Text="<%$Resources:Resource, FoodType %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblFoodType" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, Days %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblDay" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Timesperday %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblTimesperday" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, Amount %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblAmount" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="panelEdit" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label13" runat="server" CssClass="form_label"><span><%= Resources.Resource.Food %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtFood" runat="server" CssClass="form_input" MaxLength="30" data-validate="required"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label12" Text="" runat="server" CssClass="form_label"><span><%= Resources.Resource.FoodType %></span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlFoodType" runat="server" CssClass="form_input">
                                <asp:ListItem Text="<%$Resources:Resource, Dry %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Wet %>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Days %></span>&nbsp;*</asp:Label>
                            <select id="ddlDay" multiple runat="server" class="form_input multipleSelect" data-validate="required">
                            </select>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Timesperday %></span>&nbsp;*</asp:Label>
                            <select id="ddlTimesperday" multiple runat="server" class="form_input multipleSelect" data-validate="required">
                            </select>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label8" Text="" runat="server" CssClass="form_label"><span><%= Resources.Resource.Amount %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form_input" MaxLength="10" data-validate="required pnumber"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label10" Text="" runat="server" CssClass="form_label"><span><%= Resources.Resource.Unit %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtUnit" runat="server" CssClass="form_input" MaxLength="20" data-validate="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                            <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                </div>
            </div>
        </asp:Panel>
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

