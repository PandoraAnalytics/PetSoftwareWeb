<%@ Page Title="Breed - Manage Notes" Language="C#" MasterPageFile="bubreeder.master" AutoEventWireup="true" CodeBehind="bunotesdetails.aspx.cs" Inherits="Breederapp.bunotesdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .form_input {
            width: 30%;
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
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, NoteDetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <br />
        <asp:Panel ID="panelView" runat="server">
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Date %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblDate" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, Description  %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblDescription" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Panel ID="panel1" runat="server">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <ul class="photos">
                                    <asp:Repeater ID="repNotesPhotos" runat="server" OnItemCommand="repeaterFiles_ItemCommand">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkdelete" runat="server" CssClass="cross" OnClientClick="return confirm_delete();" CommandName="delete" CommandArgument='<%# Eval("file") %>' data-toggle='tooltip' data-placement='top' data-original-title='Delete' Text="<i class='fa fa-times'></i>"></asp:LinkButton>
                                                <%--<a href='<%# string.Format("docs/{0}", Eval("file")) %>' target="doc" class="photowrapper">
                                                    <div class="photo" style='<%# Eval("file_type").ToString() == "1" ? "background-image:url(images/document.png)": string.Format("background-image:url(docs/{0})", Eval("file")) %>'>
                                                        &nbsp;
                                                    </div>
                                                    <br />
                                                    <%#  Eval("file").ToString().Substring(Eval("file").ToString().IndexOf("_") + 1) %>
                                                </a>--%>

                                                <a href='<%# string.Format("../app/viewdocument.aspx?file={0}", Eval("file")) %>' target="_blank">
                                                    <img src='../images/image_loading.gif' class="lazy-img" data-src='<%# Eval("file") %>' alt="photo" />
                                                    <br />
                                                    &nbsp;<%#  Eval("file").ToString().Substring(Eval("file").ToString().IndexOf("_") + 1) %> &nbsp;
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
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="panelEdit" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.Date %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form_input date-picker" data-validate="required date" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtNotes" data-validate="required" runat="server" TextMode="MultiLine" CssClass="form_input" MaxLength="500" Rows="8" Width="100%"></asp:TextBox>
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
                                <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max2MBpng %>" runat="server"></asp:Label></span>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                            <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                </div>
            </div>
        </asp:Panel>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>

    <script>
        $(document).ready(function () {
            loadLazyImages();
        });

        $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });

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

        function confirm_delete() {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        }
    </script>
</asp:Content>

