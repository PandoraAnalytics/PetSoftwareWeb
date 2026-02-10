<%@ Page Title="Brochure - Manage Brochure" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="brochureadd.aspx.cs" Inherits="Breederapp.brochureadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
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

        .error_class {
            display: inline;
        }

        .password_rules {
            font-size: 12px;
            margin-top: 8px;
            max-width: 100%;
            color: #777;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Managebrochures %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;
            <asp:Label ID="lblEventNM" Text="" CssClass="h6" runat="server"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="200"></asp:TextBox>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, UploadTitleImage %>" runat="server" CssClass="form_label"></asp:Label>
                        <input type="file" name="myfile" id="brochure_pic" accept="image/*" />
                        <input type="hidden" runat="server" id="hid_brochure_pic" />
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label8" Text="<%$Resources:Resource, RecommendedLogoSize %>" runat="server" CssClass="password_rules"></asp:Label>
                                <asp:Label ID="lblFileUploadvalidation" CssClass="password_rules" Text="<%$Resources:Resource, Max2MBpng %>" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Headertext %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtHeader" runat="server" CssClass="form_input" data-validate="required" MaxLength="250"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Footertext %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtFooter" runat="server" CssClass="form_input" data-validate="required" MaxLength="250"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form_input" MaxLength="2000" data-validate="required maxlength-2000" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAdd_Click" />
                       <asp:LinkButton ID="btnBack2" runat="server" OnClick="btnBack_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>

    <script>

        $('#brochure_pic').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_brochure_pic';
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
