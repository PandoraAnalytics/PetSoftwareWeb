<%@ Page Title="Sponsor - Manage Sponsor" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="sponsoradd.aspx.cs" Inherits="Breederapp.sponsoradd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .fstElement {
            width: 50%;
        }

        .form_input {
            width: 50%;
        }     

        .error_class {
            display: inline;
        }

        .password_rules {
            border-radius: 4px;
            border: 1px solid #ddd;
            padding: 8px;
            background: #eff2f7;
            margin-top: 8px;
            width: 400px;
            max-width: 100%;
        }

        .rule {
            color: #777;
            padding-bottom: 8px;
        }

            .rule.active {
                color: #000 !important;
                font-weight: 600;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Managesponsor %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;<asp:Label ID="lblEventNM" Text="" runat="server" CssClass="h6"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                        <asp:Label ID="lblType" runat="server" CssClass="form_label"><span><%= Resources.Resource.Type %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Gold %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Silver %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Bronze %>" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, UploadPicture %>" runat="server" CssClass="form_label"></asp:Label>
                        <input type="file" name="myfile" id="sponsor_pic" accept="image/*" />
                        <input type="hidden" runat="server" id="hid_sponsor_pic" />

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="password_rules">
                                    <div class="rule">
                                        <b>
                                            <asp:Label ID="Label8" Text="<%$Resources:Resource, BrochureImageSpecification %>" runat="server" CssClass=""></asp:Label></b>
                                    </div>
                                    <div class="rule">
                                        <asp:Label ID="Label9" Text="<%$Resources:Resource, Bronzetypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                    </div>
                                    <div class="rule">
                                        <asp:Label ID="Label10" Text="<%$Resources:Resource, Silvertypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                    </div>
                                    <div class="rule">
                                        <asp:Label ID="Label11" Text="<%$Resources:Resource, Goldtypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form_input" MaxLength="2000" data-validate="required maxlength-2000" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>

                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAdd_Click" />&nbsp;&nbsp;
                         <asp:Button ID="btnBack" CssClass="form_button" Text="<%$Resources:Resource, Back %>" runat="server" OnClick="btnBack_Click" validate="no" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script>

        $('#sponsor_pic').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_sponsor_pic';
                    var fileuploadedsuccessfully = '<%= Resources.Resource.Fileuploadedsuccessfully %>';

                    $(hid).val(data);
                    alert(fileuploadedsuccessfully); 
                    
                  /*  alert("The file has been uploaded successfully.");*/
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);

                    else alert(fileuploadingerror); 
                   /* else alert("There was a problem uploading the file. Please try again.");*/
                }
            });
        });

    </script>
</asp:Content>
