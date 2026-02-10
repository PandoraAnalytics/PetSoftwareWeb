<%@ Page Title="My Profile" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="Breederapp.profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, MyProfile %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label3" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="30" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" data-validate="required" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Phone %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlPhoneCountryCode" runat="server" CssClass="form_input" Width="25%" data-validate="required" DataTextField="countrycode" DataValueField="id">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20" Width="38%"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Language %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form_input" data-validate="required">
                            <asp:ListItem Text="<%$Resources:Resource, Lang_English %>" Value="en-us"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Lang_German %>" Value="de-DE"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                        <asp:Label ID="Label29" runat="server" CssClass="form_label"><span><%= Resources.Resource.Country %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form_input" data-validate="required" DataTextField="fullname" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
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
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, Timezone %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlTimezone" runat="server" CssClass="form_input" DataValueField="name" DataTextField="timezone">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label8" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label10" Text="Profile Image" runat="server" CssClass="form_label"></asp:Label>
                        <input type="file" name="myfile" id="profile_logo" accept="image/*" />
                        <input type="hidden" runat="server" id="hid_profile_logo" />&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" id="lnkProfileLogo" runat="server" target="_blank">View Profile Image</a>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });

        $('#profile_logo').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_profile_logo';
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
</asp:Content>
