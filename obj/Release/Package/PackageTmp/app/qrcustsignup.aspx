<%@ Page Title="Customer Add" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="qrcustsignup.aspx.cs" Inherits="Breederapp.qrcustsignup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="lblCustomerAdd" Text="Customer Add" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblFirstName" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="255" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblLastName" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblEmailAddress" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form_input email" data-validate="required email" MaxLength="255"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblPhone" runat="server" CssClass="form_label"><span><%= Resources.Resource.Phone %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAddress" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                        <asp:Label ID="Label29" runat="server" CssClass="form_label"><span><%= Resources.Resource.Country %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form_input" data-validate="required" DataTextField="fullname" DataValueField="id">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Label ID="Label6" Text="<%$Resources:Resource, City %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label9" Text="<%$Resources:Resource, Postcode %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtPincode" runat="server" CssClass="form_input" data-validate="pnumber" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.Gender %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form_input" data-validate="required">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Male %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Female %>" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, DOB %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label4" Text="<%$Resources:Resource, AlternateContactNo %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAlterContactNo" runat="server" CssClass="form_input" data-validate="phone" MaxLength="20"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblMembershipType" runat="server" CssClass="form_label"><span>Membership Type</span>&nbsp;</asp:Label>
                        <asp:DropDownList ID="ddlMembershipType" runat="server" CssClass="form_input" data-validate="required">
                            <asp:ListItem Text="Gold" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Silver" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Platinum" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                     <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });
    </script>
</asp:Content>
