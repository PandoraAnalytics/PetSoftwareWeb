<%@ Page Title="Transfer Animal" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="animaltransfer.aspx.cs" Inherits="Breederapp.animaltransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .fstElement {
            width: 65%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AnimalTransfer %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label15" runat="server" CssClass="form_label"><span><%= Resources.Resource.Date %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblStartTime" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form_input" data-validate="required email" MaxLength="100"></asp:TextBox>
                        <div class="rule">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, Enteremailaddressoftransferee %>" CssClass="form_label"></asp:Label></div>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <input type="hidden" id="filenames" runat="server" />
                        <div id="uploader" style="width: 100%; display: inline-block;">
                            <p>
                                <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                            </p>
                        </div>
                        <span class="rule">
                            <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max4MBpng %>" runat="server"></asp:Label></span>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnTransferAnimal" CssClass="form_button" Text="<%$Resources:Resource, Transfer %>" runat="server" OnClick="btnTransferAnimal_Click" />
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

        $("#ContentPlaceHolder1_btnTransferAnimal").click(function () {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        });

        var temp;
        $("#uploader").pluploadQueue({
            runtimes: 'html5,flash,browserplus,silverlight,gears',
            url: 'file_upload_docs.ashx',
            max_files: 5,
            rename: true,
            dragdrop: true,

            filters: {
                max_file_size: '4mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
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
