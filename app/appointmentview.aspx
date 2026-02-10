<%@ Page Title="Appointment View" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="appointmentview.aspx.cs" Inherits="Breederapp.appointmentview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
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
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Appointment %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <br />

        <div class="row">
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, Name%>" CssClass="form_label"></asp:Label>
                    <asp:Label ID="lblAnimal" runat="server"></asp:Label>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label9" Text="<%$Resources:Resource, Date %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:Label ID="lblDate" Text="" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource, ContactDetails%>" CssClass="form_label"></asp:Label>
                    <asp:Label ID="lblContact" runat="server"></asp:Label>
                </div>

                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblMeditation" runat="server" Text="<%$Resources:Resource, Description  %>" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtMeditation" runat="server" CssClass="form_input" MaxLength="500" data-validate="maxlength-500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                </div>

                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, TODO%>" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtToDo" runat="server" CssClass="form_input" MaxLength="500" data-validate="maxlength-500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>

            <asp:Panel ID="panelView" runat="server">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <ul class="photos">
                            <asp:Repeater ID="repAppPhotos" runat="server" OnItemCommand="repeaterFiles_ItemCommand">
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="lnkdelete" runat="server" CssClass="cross" OnClientClick="return confirm_delete();" CommandName="delete" CommandArgument='<%# Eval("filename") %>' data-toggle='tooltip' data-placement='top' data-original-title='Delete' Text="<i class='fa fa-times'></i>"></asp:LinkButton>                                       
                                        <a href='<%# string.Format("../app/viewdocument.aspx?file={0}", Eval("filename")) %>' target="_blank">
                                            <img src='../images/image_loading.gif' class="lazy-img" data-src='<%# Eval("filename") %>' alt="photo" />
                                            <br />
                                            &nbsp;<%#  Eval("filename").ToString().Substring(Eval("filename").ToString().IndexOf("_") + 1) %> &nbsp;
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
                </div>
            </asp:Panel>

            <%--<div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="photos">
                        <asp:Repeater ID="repPhotos" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource, Image%>" CssClass="form_label"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <a href='<%# string.Format("docs/{0}", Eval("filename")) %>' target="doc">
                                        <%#  Eval("filename").ToString().Substring(Eval("filename").ToString().IndexOf("_") + 1) %>
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
            </div>--%>
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <input type="hidden" id="filenames" runat="server" />
                    <div id="uploader" style="width: 100%; display: inline-block;">
                        <p>
                            <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                        </p>
                    </div>
                    <span class="rule">
                        <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max2MBpng %>" runat="server"></asp:Label></span>
                </div>
            </div>
        </div>

        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
               <%-- <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click">Back</asp:LinkButton>--%>
                <asp:LinkButton ID="lnkBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="lnkBack_Click"></asp:LinkButton>
            </div>
        </div>

    </div>
    <script src="js/validator.js?100" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script>
        $(document).ready(function () {
            loadLazyImages();
        });

        var temp;
        $("#uploader").pluploadQueue({
            runtimes: 'html5,flash,browserplus,silverlight,gears',
            url: 'file_upload_docs.ashx',
            max_files: 5,
            rename: true,
            dragdrop: true,

            filters: {
                max_file_size: '2mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
                    { title: "PDF files", extensions: "pdf" },
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

        function confirm_delete() {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        }
    </script>
</asp:Content>
