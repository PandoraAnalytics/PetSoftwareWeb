<%@ Page Title="Breed - Manage Contact" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="createcontact.aspx.cs" Inherits="Breederapp.createcontact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ContactDetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
            &nbsp;&nbsp;
             <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack1_Click"></asp:LinkButton>

        </div>
        <div class="clearfix"></div>
        <br />
        <asp:Panel ID="panelView" runat="server">
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, FirstName %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblFirstName" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, LastName %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblLastName" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblEmailAddress" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label16" Text="<%$Resources:Resource, ContactNumber %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblContactNumber" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <asp:Panel ID="pnlMatrixLabels" runat="server">
                    </asp:Panel>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label18" Text="<%$Resources:Resource, Category %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblProfession" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label20" Text="<%$Resources:Resource, AboutDiscription %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblAboutDiscription" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label10" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblAddressOptional" Text="-" runat="server"></asp:Label>
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
                            <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label7" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form_input email" data-validate="email" MaxLength="100"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label8" Text="<%$Resources:Resource, ContactNumber %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form_input phone" data-validate="phone" MaxLength="20"></asp:TextBox>
                            &nbsp;
                            <a href="javascript:void(0);" class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                <asp:Label ID="Label15" Text="<%$Resources:Resource, AddMore %>" runat="server"></asp:Label>
                            </a>
                            <ul class="dropdown-menu profile_menu" aria-labelledby="profilemenu">
                                <li>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="Mobile" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label19" Text="<%$Resources:Resource, Mobile%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandArgument="Business" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label22" Text="<%$Resources:Resource, Business%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Work" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label11" Text="<%$Resources:Resource, Work%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Home" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label14" Text="<%$Resources:Resource, Home%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Main" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label17" Text="<%$Resources:Resource, Main%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument="Other" OnCommand="btnAddTxtBox_Click">
                                        <asp:Label ID="Label21" Text="<%$Resources:Resource, Other%>" runat="server"></asp:Label>
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <asp:Panel ID="pnlMatrixTextBoxes" runat="server">
                    </asp:Panel>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label12" runat="server" CssClass="form_label"><span><%= Resources.Resource.Category %></span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlProfession" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label13" Text="<%$Resources:Resource, AboutDiscription %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtAboutDiscription" runat="server" CssClass="form_input" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtAddressOptional" runat="server" CssClass="form_input" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
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
</asp:Content>
