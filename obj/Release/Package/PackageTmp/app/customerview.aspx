<%@ Page Title="Customer View" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="customerview.aspx.cs" Inherits="Breederapp.customerview_aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h5 class="page_title">
                    <asp:Label ID="lblHeading" Text="Customer View" CssClass="error_class" runat="server"></asp:Label></h5>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Panel ID="panelAction" runat="server">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                            <asp:Button ID="btnEdit" runat="server" Text="Edit Customer" CssClass="form_button" OnClick="btnEdit_Click" validate="no" Visible="true" />&nbsp;&nbsp;
                             <a href="customerlist.aspx">
                                 <asp:Label ID="lblBack" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label></a>
                        </div>
                    </div>
                    <br />
                </asp:Panel>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, FirstName %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblFname" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label15" Text="<%$Resources:Resource, LastName %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblLname" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblEmail" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label5" Text="<%$Resources:Resource, Phone %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblPhone" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblAddress" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label10" Text="<%$Resources:Resource, Country %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblCountry" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblCit" Text="<%$Resources:Resource, City %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblCity" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, Postcode %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblPostcode" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, Gender %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblGender" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label11" Text="<%$Resources:Resource, DOB %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblDob" Text="-" runat="server"></asp:Label>
                            </div>

                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label13" Text="<%$Resources:Resource, AlternateContactNo %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblAlternatecontact" Text="-" runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label16" Text="Membership Type" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblMembershipType" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
