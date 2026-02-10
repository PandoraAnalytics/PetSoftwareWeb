<%@ Page Title="Business Profile" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buprofile.aspx.cs" Inherits="Breederapp.buprofile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
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

        .photos {
            /*margin-bottom: 25px;*/
        }

            .photos li {
                display: inline-block;
                padding-right: 10px;
                position: relative;
            }

                .photos li a {
                    display: inline-block;
                    min-width: 120px;
                    border: solid 1px #ddd;
                    text-align: center;
                }

                .photos li img {
                    width: 100%;
                    object-fit: cover;
                    height: 90px;
                    padding: 2px;
                    max-width: 100%;
                }

                .photos li .cross {
                    position: absolute;
                    top: -5px;
                    right: 5px;
                    border: 0;
                    border-radius: 50%;
                    min-width: 0;
                    background-color: #d64651;
                    color: #fff;
                    padding: 1px 6px;
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
                            <a href="buprofileqrcode.aspx" runat="server" target="_blank"><i class='fa fa-qrcode' aria-hidden='true'></i>&nbsp;Generate QR Code</a>
                        </div>
                    </div>
                </fieldset>
                <br />
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
                            <asp:Label ID="Label17" Text="Business Email" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtBUEmailAddress" runat="server" CssClass="form_input email" data-validate="email" MaxLength="255"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label18" Text="Business Contact" runat="server" CssClass="form_label"></asp:Label>
                            <asp:DropDownList ID="ddlBUPhoneCountryCode" runat="server" CssClass="form_input" Width="25%" DataTextField="countrycode" DataValueField="id">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                                <asp:TextBox ID="txtBUPhone" runat="server" CssClass="form_input" data-validate="phone" MaxLength="20" Width="38%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <asp:Label ID="Label29" runat="server" CssClass="form_label"><span><%= Resources.Resource.Country %></span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form_input" data-validate="required" DataTextField="fullname" DataValueField="id">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, City %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <asp:Label ID="Label13" Text="<%$Resources:Resource, Postcode %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="form_input" data-validate="pnumber" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblRegistration" runat="server" CssClass="form_label"><span><%= Resources.Resource.RegistrationNo %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                            <asp:Label ID="Label14" runat="server" CssClass="form_label"><span>Currency</span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label15" runat="server" CssClass="form_label"><span>Tax</span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlTax" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblAddress" runat="server" CssClass="form_label"><span><%= Resources.Resource.Address %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" data-validate="required" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label16" runat="server" CssClass="form_label"><span>Order Terms and Conditiions</span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtTermCondition" runat="server" CssClass="form_input" data-validate="required" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label19" Text="Business Documents" runat="server" CssClass="form_label"></asp:Label>
                            <div class="row">
                                <ul class="photos">
                                    <asp:Repeater ID="repPhotos" runat="server" OnItemCommand="repeaterFiles_ItemCommand">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkdelete" runat="server" CssClass="cross" OnClientClick="return confirm_delete();" CommandName="delete" CommandArgument='<%# Eval("gallery_file") %>' data-toggle='tooltip' data-placement='top' data-original-title='Delete' Text="<i class='fa fa-times'></i>"></asp:LinkButton>
                                                <a href='<%# string.Format("../app/viewdocument.aspx?file={0}", Eval("gallery_file")) %>' target="_blank">
                                                    <img src='../images/image_loading.gif' class="lazy-img" data-src='<%# Eval("gallery_file") %>' alt="photo" />
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <br />
                                            &nbsp;
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <input type="hidden" id="filenames" runat="server" />
                            <div id="uploader" style="width: 60%; display: inline-block;">
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
                <br />
                <fieldset id="fs_OtherInfo" runat="server">
                    <legend>
                        <asp:Label ID="lblOtherInfo" runat="server" Text="<%$Resources:Resource, OtherInfo %>"></asp:Label></legend>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label7" Text="<%$Resources:Resource, DateOfIncorporation %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtIncorporationDate" runat="server" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, TinNo %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtTinNo" runat="server" CssClass="form_input" MaxLength="255"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, LicenceNo %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form_input" MaxLength="255"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, EmployerIdNo %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtEmployerId" runat="server" CssClass="form_input" MaxLength="255"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label11" Text="<%$Resources:Resource, AboutBusiness %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtAboutBusiness" runat="server" CssClass="form_input" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label12" Text="<%$Resources:Resource, CompanyLogo %>" runat="server" CssClass="form_label"></asp:Label>
                            <input type="file" name="myfile" id="company_logo" accept="image/*" />
                            <input type="hidden" runat="server" id="hid_company_logo" />&nbsp;&nbsp;<a href="javascript:void(0);" id="lnkCompanyLogo" runat="server" target="_blank">View Company Logo</a>
                        </div>
                    </div>
                </fieldset>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            loadLazyImages();
        });

        $('#company_logo').change(function () {
            var f = $(this);
            var fid = $(f).attr('id');

            var fileData = $(f).prop("files")[0];
            var formData = new window.FormData();
            formData.append("file", fileData);
            formData.append("extns", "image");

            var id = 'per_lbl_' + fid;
            $('#' + id).remove();

            $.ajax({
                url: 'file_upload_docs.ashx',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            if (!isNaN(percentComplete)) {
                                percentComplete = percentComplete * 100;
                                if (percentComplete > 100) percentComplete = 100;
                                var id = 'per_lbl_' + fid;
                                $('#' + id).remove();
                                $(f).after("<span id='" + id + "'>Uploaded: " + parseFloat(percentComplete).toFixed(2) + "%</span>");
                            }
                        }
                    }, false);
                    return xhr;
                },
                success: function (data) {
                    var hid = '#ContentPlaceHolder1_hid_company_logo';
                    var fileuploadedsuccessfully = '<%= Resources.Resource.Fileuploadedsuccessfully %>';
                    $(hid).val(data);
                    alert(fileuploadedsuccessfully);
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert(fileuploadingerror);
                }
            });
        });
    </script>
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
