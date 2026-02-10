<%@ Page Title="Business Enquiry Form" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="buregistrationform.aspx.cs" Inherits="Breederapp.buregistrationform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
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
            <asp:Label ID="lblRegForm" Text="<%$Resources:Resource, BusinesEnquiryForm %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-8 col-xs-12">
                <asp:Panel ID="panelParent" runat="server" Visible="true">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                    <fieldset id="fs_Description" runat="server">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, PersonalInfo %>"></asp:Label></legend>
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
                                <asp:DropDownList ID="ddlPhoneCountryCode" runat="server" CssClass="form_input" Width="25%" data-validate="required" DataTextField="countrycode" DataValueField="id">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20" Width="38%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblAddress" runat="server" CssClass="form_label"><span><%= Resources.Resource.Address %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" data-validate="required" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>

                    <fieldset id="fs_CompanyInfo" runat="server">
                        <legend>
                            <asp:Label ID="lblCompanyInfo" runat="server" Text="<%$Resources:Resource, CompanyInfo %>"></asp:Label></legend>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblCompany" runat="server" CssClass="form_label"><span><%= Resources.Resource.Company %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtCompany" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblShortName" runat="server" CssClass="form_label"><span><%= Resources.Resource.CompanyShortName %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtShortName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblWebsite" runat="server" CssClass="form_label"><span><%= Resources.Resource.Website %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblBusinessType" runat="server" CssClass="form_label"><span><%= Resources.Resource.BusinessType %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblRegistration" runat="server" CssClass="form_label"><span><%= Resources.Resource.RegistrationNo %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                 <asp:Label ID="Label2" Text="Upload Business Documents" runat="server" CssClass="form_label"></asp:Label>
                                <input type="hidden" id="filenames" runat="server" />
                                <div id="uploader" style="width: 80%; display: inline-block;">
                                    <p>
                                        <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                                    </p>
                                </div>
                                <span class="rule">
                                    <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max20MBpnggif %>" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </fieldset>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSubmitRequest" CssClass="form_button" Text="<%$Resources:Resource, SubmitRequest %>" runat="server" OnClick="btnSubmitRequest_Click" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                    <h6><span>Thank you for registering your business with us. Your registration request has been successfully submitted.<br />
                        Our administrative team will review your application and contact you shortly for any additional information required to complete the process.</span></h6>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
     <script>
     var temp;
        $("#uploader").pluploadQueue({
            runtimes: 'html5,flash,browserplus,silverlight,gears',
            url: 'file_upload_docs.ashx',
            max_files: 5,
            rename: true,
            dragdrop: true,

            filters: {
                max_file_size: '20mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
                    { title: "Video files", extensions: "mp4" },
                    { title: "PDF files", extensions: "pdf" },
                    { title: "Text files", extensions: "txt,doc,docx,xls,xlsx" }
                ]
            },

            flash_swf_url: 'js/plupload/Moxie.swf',
            silverlight_xap_url: 'js/plupload/Moxie.xap',
            init: {
                FileUploaded: function (up, file, info) {
                    var val = $("#ContentPlaceHolder1_filenames").val();
                    temp = val;
                    if (val.length > 0) {
                        temp += ",";
                    }
                    temp += info.response;
                    $("#ContentPlaceHolder1_filenames").empty();
                    $("#ContentPlaceHolder1_filenames").val(temp);
                }
            },
        });
     </script>
</asp:Content>
