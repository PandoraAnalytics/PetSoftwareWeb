<%@ Page Title="Breed - Manage Certificate" Language="C#" MasterPageFile="bubreeder.master" AutoEventWireup="true" CodeBehind="bucertificateadd.aspx.cs" Inherits="Breederapp.bucertificateadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AddCertificate %>" runat="server" CssClass="error_class"></asp:Label></h5>

        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <asp:Panel ID="panelWarning" runat="server" Visible="false">
                    <div class="alert alert-danger" role="alert">
                        <asp:Label ID="lblWarning" runat="server" Text="<%$Resources:Resource, Pleasenotethatthiscertificateneeds %>"></asp:Label>
                    </div>
                </asp:Panel>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.CertificateName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtCertificateName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblType" runat="server" CssClass="form_label"><span><%= Resources.Resource.Type %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required" DataTextField="type" DataValueField="id" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label9" runat="server" CssClass="form_label"><span><%= Resources.Resource.StartDate %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form_input date-picker" data-validate="required date" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label11" Text="<%$Resources:Resource, ExpiryDate %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form_input date-picker" data-validate="date" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label12" runat="server" CssClass="form_label"><span><%= Resources.Resource.FileUpload %></span>&nbsp;*</asp:Label>
                        <input type="file" name="myfile" id="certificate_pic" accept="image/*" />
                        <input type="hidden" runat="server" id="hid_certificate_pic" />
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAddCertificate_Click" />
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
        $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });

        $('#certificate_pic').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_certificate_pic';
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

