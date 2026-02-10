<%@ Page Title="Product Edit" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="productedit.aspx.cs" Inherits="Breederapp.productedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .form_input {
            width: 70%;
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

        .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
            /*color: var(--bs-nav-pills-link-active-color) !important;*/
            /*background-color: var(--bs-nav-pills-link-active-bg);*/
            background-color: #f28c0e;
        }

        .nav-pills .nav-link {
            border-radius: 0 !important;
        }

        .nav-link:focus, .nav-link:hover {
            color: #ffffff !important;
        }

        .nav-link {
            color: #ffffff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h5 class="page_title">
                    <asp:Label ID="lblHeading" Text="Product Edit" CssClass="error_class" runat="server"></asp:Label></h5>
            </div>
        </div>
        <br />
        <div class="dt_tab_pages">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                        <li id="tablnkBasic" class="nav-item" role="presentation" runat="server">
                            <button class="nav-link active" id="basic_details_tab" data-bs-toggle="pill" data-bs-target="#tab_basic_details" type="button" role="tab" aria-controls="tab_basic_details"
                                aria-selected="true" runat="server">
                                <asp:Label ID="lblBasic" runat="server" Text="<%$Resources:Resource, BasicDetails %>"></asp:Label></button>
                        </li>
                        <li id="tablnkImages" class="nav-item" role="presentation" runat="server">
                            <button class="nav-link" id="images_tab" data-bs-toggle="pill" data-bs-target="#tab_images" type="button" role="tab" aria-controls="tab_images" aria-selected="false" runat="server">
                                <asp:Label ID="lblImages" runat="server" Text="Images"></asp:Label></button>
                        </li>
                    </ul>

                    <div class="tab-content clearfix" id="pills-tabContent">
                        <!------------ tab_1 ------------>
                        <div class="tab-pane fade show active" role="tabpanel" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0" runat="server">
                            <div class="form_horizontal basic_deatils_form" style="line-height: 25px;">
                                <div class="row">
                                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label2" runat="server" CssClass="form_label"><span>Name</span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label31" runat="server" CssClass="form_label"><span>Category</span>&nbsp;</asp:Label>
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form_input" DataTextField="name" DataValueField="id">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label32" runat="server" CssClass="form_label"><span>Brand</span>&nbsp;</asp:Label>
                                                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form_input" DataTextField="name" DataValueField="id">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label1" runat="server" CssClass="form_label"><span>Material Type</span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtMaterialType" runat="server" data-validate="required" CssClass="form_input" MaxLength="45"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label10" runat="server" CssClass="form_label"><span>Status</span>&nbsp;*</asp:Label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_input">
                                                    <asp:ListItem Text="Deactivate" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Activate" Value="1" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label30" runat="server" CssClass="form_label"><span>Tag</span>&nbsp;</asp:Label>
                                                <asp:TextBox ID="txtproducttags" runat="server" CssClass="form_input" MaxLength="90"></asp:TextBox>
                                                <span class="rule">
                                                    <asp:Label ID="Label17" Text="[Tags can be separated with commas. For example, you can type tag1,tag2,tag3 to create three independent tags]" runat="server"></asp:Label></span>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <%--<div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label6" runat="server" CssClass="form_label"><span>Actual Cost</span> (<asp:Label ID="lblFinalCostCurrency" runat="server"></asp:Label>)&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtFinalCost" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="20"></asp:TextBox>
                                            </div>--%>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label8" runat="server" CssClass="form_label"><span>Actual Cost</span> (<asp:Label ID="lblCostCurrency" runat="server"></asp:Label>)&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtCost" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label3" runat="server" CssClass="form_label"><span>Threshold Stock Value</span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtThresholdStockValue" runat="server" data-validate="required pnumber" CssClass="form_input" MaxLength="5"></asp:TextBox>
                                                <span class="rule">
                                                    <asp:Label ID="Label5" Text="[System will show warning when stock reaches to this value]" runat="server"></asp:Label></span>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label26" runat="server" CssClass="form_label"><span>Size</span>&nbsp;</asp:Label>
                                                <asp:TextBox ID="txtSize" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label28" runat="server" CssClass="form_label"><span>Color</span>&nbsp;</asp:Label>
                                                <asp:TextBox ID="txtColor" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                                            </div>
                                        </div>

                                        <%-- <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label33" runat="server" CssClass="form_label"><span>Original Quantity </span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtOriginalQuentity" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="10"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label34" runat="server" CssClass="form_label"><span>Stock Quantity</span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtStockQuentity" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </div>--%>

                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label29" runat="server" CssClass="form_label"><span>Weight</span> (<asp:Label ID="lblWeightType" runat="server"></asp:Label>)&nbsp;</asp:Label>
                                                <asp:TextBox ID="txtWeight" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label12" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                                                <input type="file" name="myfile" id="product_pic" accept="image/*" />
                                                <input type="hidden" runat="server" id="hid_product_pic" />&nbsp;&nbsp;<a href="javascript:void(0);" id="lnkProductProfilePic" runat="server" target="_blank">View Profile Pic</a>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                             <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label13" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                                                <asp:TextBox ID="txtAboutDiscription" runat="server" TextMode="MultiLine" data-validate="required" CssClass="form_input" MaxLength="455" Rows="8"></asp:TextBox>
                                            </div>

                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label15" runat="server" CssClass="form_label"><span>Tax</span>&nbsp;*</asp:Label>
                                                <asp:DropDownList ID="ddlTax" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                                                </asp:DropDownList>
                                            </div>
                                        </div>                                       

                                        <div class="row form-group">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:Button ID="btnSaveProduct" CssClass="form_button" Text="Save and Next" runat="server" OnClick="btnSaveProduct_Click" />&nbsp;                                                 
                                                <asp:Button ID="btnNext" CssClass="form_button" Text="Next" runat="server" OnClick="btnNext_Click" />&nbsp;   
                                                  <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_2 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_images" aria-labelledby="images-tab" tabindex="0" runat="server">
                            <div class="form_horizontal images_form">
                                <div class="row">
                                    <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                        <h6 class="page_title">
                                            <asp:Label ID="Label4" Text="Images" CssClass="error_class" runat="server"></asp:Label></h6>
                                    </div>
                                </div>
                                <asp:Label ID="lblPhotoError" runat="server" CssClass="error_class"></asp:Label>
                                <asp:Panel ID="PanelViewPhoto" runat="server" Visible="true">
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
                                </asp:Panel>
                                <asp:Panel ID="panelAddPhoto" runat="server" Visible="true">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <asp:Label ID="Label19" runat="server" CssClass="error_class"></asp:Label>
                                            <div class="row form_row">
                                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                                    <input type="hidden" id="filenames" runat="server" />
                                                    <div id="uploader" style="width: 100%; display: inline-block;">
                                                        <p>
                                                            <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                                                        </p>
                                                    </div>
                                                    <span class="rule">
                                                        <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max20MBpnggif %>" runat="server"></asp:Label></span>
                                                </div>
                                            </div>
                                            <div class="row form_row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="btnPrevious" CssClass="form_button" Text="Previous" runat="server" OnClick="btnPrevious_Click" />&nbsp;   
                                                    <asp:Button ID="btnSavePhoto" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSavePhoto_Click" />&nbsp;                                          
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script src="js/validator.js?3" type="text/javascript"></script>
    <script src="js/jquery.bootpag.min.js"></script>

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
                max_file_size: '20mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
                    { title: "Video files", extensions: "mp4" },
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

        $('#product_pic').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_product_pic';
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
