<%@ Page Title="Business Profile View" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="buprofileview.aspx.cs" Inherits="Breederapp.buprofileview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        legend {
            display: block;
            width: 100%;
            padding: 0;
            margin-bottom: 20px;
            font-size: 17px;
            line-height: inherit;
            color: #333;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, BusinessProfile %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <fieldset id="fs_Description" runat="server">
                    <legend>
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, PersonalInfo %>"></asp:Label></legend>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label10" Text="<%$Resources:Resource, Name %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label8" Text="<%$Resources:Resource, Phone %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblPhone" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset id="fs_CompanyInfo" runat="server">
                    <legend>
                        <asp:Label ID="lblCompanyInfo" runat="server" Text="<%$Resources:Resource, CompanyInfo %>"></asp:Label></legend>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                              <asp:Label ID="lblCompany1" Text="<%$Resources:Resource, Company %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblCompany" runat="server"></asp:Label>                           
                           
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                              <asp:Label ID="Label14" Text="<%$Resources:Resource, CompanyShortName %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblShortName" runat="server"></asp:Label> 
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                             <asp:Label ID="Label15" Text="<%$Resources:Resource, Website %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblWebsite" runat="server"></asp:Label> 
                           
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                             <asp:Label ID="Label16" Text="<%$Resources:Resource, BusinessType %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblBusinessType" runat="server"></asp:Label>                            
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                             <asp:Label ID="Label17" Text="<%$Resources:Resource, Country %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblCountry" runat="server"></asp:Label> 
                            
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, City %>" runat="server" CssClass="form_label"></asp:Label>
                             <asp:Label ID="lblCity" runat="server"></asp:Label> 
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <asp:Label ID="Label13" Text="<%$Resources:Resource, Postcode %>" runat="server" CssClass="form_label"></asp:Label>
                             <asp:Label ID="lblPostcode" runat="server"></asp:Label> 
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                             <asp:Label ID="Label18" Text="<%$Resources:Resource, RegistrationNo %>" runat="server" CssClass="form_label"></asp:Label>
                             <asp:Label ID="lblRegistrationNo" runat="server"></asp:Label>                          
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                             <asp:Label ID="Label19" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                             <asp:Label ID="lblAddress" runat="server"></asp:Label>                          
                        </div>
                    </div>
                </fieldset>
                <br />
                <fieldset id="fs_OtherInfo" runat="server">
                    <legend>
                        <asp:Label ID="lblOtherInfo" runat="server" Text="<%$Resources:Resource, OtherInfo %>"></asp:Label></legend>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label7" Text="<%$Resources:Resource, DateOfIncorporation %>" runat="server" CssClass="form_label"></asp:Label>
                           <asp:Label ID="lblDateOfIncorporation" runat="server"></asp:Label> 
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, TinNo %>" runat="server" CssClass="form_label"></asp:Label>
                           <asp:Label ID="lblTinNo" runat="server"></asp:Label> 
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, LicenceNo %>" runat="server" CssClass="form_label"></asp:Label>
                           <asp:Label ID="lblLicenceNo" runat="server"></asp:Label> 
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, EmployerIdNo %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblEmployerId" runat="server"></asp:Label> 
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label11" Text="<%$Resources:Resource, AboutBusiness %>" runat="server" CssClass="form_label"></asp:Label>
                           <asp:Label ID="lblAboutBusiness" runat="server"></asp:Label> 
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label12" Text="<%$Resources:Resource, CompanyLogo %>" runat="server" CssClass="form_label"></asp:Label>
                            <a href="javascript:void(0);" id="lnkCompanyLogo" runat="server" target="_blank">View Company Logo</a>
                        </div>
                    </div>
                </fieldset>              
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
           
        });       
    </script>
</asp:Content>
