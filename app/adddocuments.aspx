<%@ Page Title="Add Document" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="adddocuments.aspx.cs" Inherits="Breederapp.adddocuments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <link href="css/fastselect.css" rel="stylesheet" />

    <style>
        .rank {
            color: #656565;
            font-weight: normal;
        }

        .rule {
            color: #19c5d4;
            font-size: 12px;
            display: block;
        }

        .checkbox {
            margin: 0 0 0 20px;
        }

            .checkbox tr td {
                border: none;
                border: 0;
                padding: 5px 0;
            }

        #divreported {
            display: none;
        }

        .fstElement {
            width: 30%;
        }

        h6 {
            color: #999;
        }

        h5 {
            margin-bottom: 15px;
        }

        .breed_list h4 {
            margin-top: 10px;
        }

            .breed_list h4 a {
                color: #333;
            }

        .breed_list ul li {
            display: inline-block;
            width: 250px;
            margin: 0;
        }


        .list_image img {
            max-width: 70%;
            height: auto;
            cursor: pointer;
        }


        .list_wrapper {
            text-align: center;
            margin: 10px 0px;
            position: relative;
        }

        .list_image {
            background: #fff;
            padding: 10px;
            width: 230px;
            margin: 0 auto;
            height: 100px;
            display: table-cell;
            vertical-align: middle;
            box-shadow: 1px 0 20px rgba(0,0,0,.08);
            border-radius: 5px;
        }

        #modal_knowmore h4 {
            color: #db6516;
        }

        #email {
            padding: 12px;
            box-shadow: 0px 10px 20px rgba(30, 30, 30, 0.15);
            border-radius: 15px;
        }

        .form_input {
            width: 65%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h5>
        <asp:Label ID="Label11" Text="<%$Resources:Resource, AddDocument %>" runat="server" CssClass="error_class"></asp:Label></h5>
    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label1" Text="<%$Resources:Resource, Nameofdocument %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtnotes" runat="server" TextMode="MultiLine" CssClass="form_input" MaxLength="255" Rows="8" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="row form-group">
                <div class="col-lg-10 col-md-10 col-sm-12 col-xs-12">
                    <asp:Label ID="lblUploadDocument" Text="<%$Resources:Resource, UploadDocument %>" runat="server"></asp:Label><\br>
                    <input type="hidden" id="filenames" runat="server" />
                    <div id="uploader" style="width: 84%; display: inline-block;">
                        <p>
                            <asp:Label ID="lblYourbrowser" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                        </p>
                    </div>
                    <span class="rule"><asp:Label ID="Label2" Text="<%$Resources:Resource, Max20MBpng %>" runat="server"></asp:Label></span>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="btnAddBreed" CssClass="form_button" Text="<%$Resources:Resource, SaveYourNotes %>" runat="server" OnClick="btnAddNote_Click" />
                </div>
            </div>
        </div>
        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
        </div>
    </div>
    <script src="js/data.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script>
        $('.multipleSelect').fastselect({
            placeholder: '<%=  Resources.Resource.ChooseOption %>',
            noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
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
                    { title: "Zip files", extensions: "zip" },
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
