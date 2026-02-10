<%@ Page Title="Ticket Edit" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="ticketedit.aspx.cs" Inherits="Breederapp.ticketedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .form_input {
            width: 80%;
        }

        #ContentPlaceHolder1_txtOptionalEmails {
            height: 50px;
            min-height: 50px;
        }

        .rule {
            color: #19c5d4;
            font-size: 12px;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, EditTicket %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Header %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtHeader" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblApplication" runat="server" CssClass="form_label"><span><%= Resources.Resource.Application %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlApplication" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <asp:Panel ID="panelBug" runat="server">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Bug %></span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlBug" runat="server" CssClass="form_input" data-validate="required">
                                <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form_input" MaxLength="10000" data-validate="required maxlength-10000" Rows="10" TextMode="MultiLine" Width="91%"></asp:TextBox>
                    </div>
                </div>

                <asp:Panel ID="panelOptionalEmail" runat="server" Visible="false">
                    <div class="row form-form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, AdditionalEmailAddresses %>" CssClass="form_label" runat="server"></asp:Label>
                            <asp:TextBox ID="txtOptionalEmails" runat="server" CssClass="form_input" MaxLength="1000" Rows="2" TextMode="MultiLine" Width="91%"></asp:TextBox>
                            <span class="rule"><%= Resources.Resource.AdditionalEmailAddressesRule %></span>
                        </div>
                    </div>
                </asp:Panel>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;&nbsp;
                          <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="../js/fastselect.standalone.min.js"></script>
</asp:Content>
