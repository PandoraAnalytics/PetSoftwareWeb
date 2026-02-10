<%@ Page Title="Service View" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="serviceview.aspx.cs" Inherits="Breederapp.serviceview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h5 class="page_title">
                    <asp:Label ID="lblHeading" Text="Service View" CssClass="error_class" runat="server"></asp:Label></h5>
            </div>
        </div>
        <br />
        <div class="dt_tab_pages">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                        <li id="tablnkBasic" class="nav-item" role="presentation">
                            <button class="nav-link active" id="basic_details-tab" data-bs-toggle="pill" data-bs-target="#tab_basic_details" type="button" role="tab" aria-controls="tab_basic_details"
                                aria-selected="true">
                                <asp:Label ID="lblBasic" runat="server" Text="<%$Resources:Resource, BasicDetails %>"></asp:Label></button>
                        </li>
                        <li id="tablnkImages" class="nav-item" role="presentation">
                            <button class="nav-link" id="images-tab" data-bs-toggle="pill" data-bs-target="#tab_images" type="button" role="tab" aria-controls="tab_images"
                                aria-selected="false">
                                <asp:Label ID="lblImages" runat="server" Text="Images"></asp:Label></button>
                        </li>
                    </ul>

                    <div class="tab-content clearfix" id="pills-tabContent">
                        <!------------ tab_1 ------------>
                        <div class="tab-pane fade show active" role="tabpanel" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0">
                            <asp:Panel ID="panelAction" runat="server">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit Service" CssClass="form_button" OnClick="btnEdit_Click" validate="no" Visible="true" />&nbsp;&nbsp;
                                         <a href="serviceslist.aspx">
                                             <asp:Label ID="lblBack" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label></a>
                                    </div>
                                </div>
                                <br />
                            </asp:Panel>
                            <div class="form_horizontal basic_deatils_form" style="line-height: 25px;">
                                <div class="row">
                                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label1" runat="server" CssClass="form_label" Text="Name"></asp:Label>
                                                <asp:Label ID="lblName" Text="-" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label9" Text="Type" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblType" Text="-" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label14" Text="Status" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblStatus" Text="-" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label03" runat="server" CssClass="form_label" Text="Service Tag"></asp:Label>
                                                <asp:Label ID="lblServicetag" Text="-" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form_row">
                                            <%--<div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label12" Text="Actual Cost" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblFinalCostCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblFinalCost" Text="-" runat="server"></asp:Label>
                                            </div>--%>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label11" Text="Actual Cost" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblCostCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblCost" Text="-" runat="server"></asp:Label>
                                            </div>
                                           <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label5" Text="Tax" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblTax" runat="server"></asp:Label>
                                            </div>  
                                        </div>

                                         <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label15" Text="<%$Resources:Resource, Description %>" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Label ID="lblAboutDiscription" Text="-" runat="server"></asp:Label>
                                            </div>   
                                             <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label2" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                                                <asp:Panel ID="panelProfiePic" runat="server" Visible="false">
                                                    <a href="javascript:void(0);" id="lnkServiceProfilePic" runat="server" target="_blank">View Profile Pic</a>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_2 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_images" aria-labelledby="images-tab" tabindex="0">
                            <div class="form_horizontal images_form">
                                <div class="row">
                                    <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                        <h6 class="page_title">
                                            <asp:Label ID="Label4" Text="Images" CssClass="error_class" runat="server"></asp:Label></h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <ul class="photos">
                                        <asp:Repeater ID="repPhotos" runat="server">
                                            <ItemTemplate>
                                                <li>
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script>
        $(document).ready(function () {
            loadLazyImages();
        });
    </script>
</asp:Content>
