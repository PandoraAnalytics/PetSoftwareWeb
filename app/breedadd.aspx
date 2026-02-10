<%@ Page Title="Create Breed" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="breedadd.aspx.cs" Inherits="Breederapp.breedadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .rule {
            color: #19c5d4;
            font-size: 12px;
            display: block;
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

        .breed_list h5 {
            margin-top: 10px;
            margin-bottom: 0px;
        }

            .breed_list h5 a {
                color: #333;
            }

        .breed_list ul li {
            display: inline-block;
            width: 220px;
            margin: 0;
        }

        .list_image img {
            width: 50px;
            height: 50px;
            /* border-radius: 5px; */
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
            width: 180px;
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
    <h6>
        <asp:Label ID="Label13" Text="<%$Resources:Resource, Step1 %>" runat="server"></asp:Label></h6>
    <h5>
        <asp:Label ID="Label7" Text="<%$Resources:Resource, SelectCategory %>" runat="server" CssClass="error_class"></asp:Label></h5>
    <div class="breed_list">
        <ul>
            <asp:Repeater ID="repeaterBreeds" runat="server" OnItemCommand="repeaterBreeds_ItemCommand">
                <ItemTemplate>
                    <li>
                        <div class="list_wrapper">
                            <div class="list_image">
                                <asp:LinkButton ID="lnk1" runat="server" CommandArgument='<%# Eval("id") %>'>
                                    <img src='<%# string.Format("images/{0}", Eval("breedimage")) %>' alt="breed" class="breed" />
                                </asp:LinkButton>
                                <h5>
                                    <asp:LinkButton ID="lnk2" runat="server" CommandArgument='<%# Eval("id") %>'>
                                        <asp:Label ID="Label1" runat="server"><%# Eval("breedname") %></asp:Label>
                                    </asp:LinkButton></h5>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    <li>
                        <div class="list_wrapper">
                            <div class="list_image">
                                <h4>
                                    <asp:Label ID="Label8" Text="<%$Resources:Resource, Haveothercategory %>" runat="server"></asp:Label></h4>
                                <a href="javascript:void(0);" id="knowMore" data-bs-toggle="modal" data-bs-target="#modal_knowmore">
                                    <asp:Label ID="Label1" Text="<%$Resources:Resource, Clickheretoknowmore %>" runat="server"></asp:Label></a>
                            </div>
                        </div>
                    </li>
                </FooterTemplate>
            </asp:Repeater>
        </ul>
    </div>

    <asp:Panel ID="panelStep2" runat="server" Visible="false">
        <h6>
            <asp:Label ID="Label8" Text="<%$Resources:Resource, Step2 %>" runat="server"></asp:Label></h6>
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, AddAnimalDetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <div class="row form_row">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, CollarId %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:TextBox ID="txtCollarId" runat="server" CssClass="form_input" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.DOB %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>

                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.TypeofBreed %></span>&nbsp;*</asp:Label>
                            <input type="text" list="ContentPlaceHolder1_datalist1" class="form_input" runat="server" id="txtType" maxlength="50" autocomplete="off" data-validate="required" />
                            <datalist runat="server" id="datalist1"></datalist>
                        </div>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                        <asp:Label ID="Label14" Text="<%$Resources:Resource, Gender %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Male %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Female %>" Value="2"></asp:ListItem>
                           <%-- <asp:ListItem Text="<%$Resources:Resource, Other %>" Value="3"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label5" Text="<%$Resources:Resource, About %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAbout" runat="server" TextMode="MultiLine" CssClass="form_input" MaxLength="500" Rows="8" Width="83%"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnAddBreed" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAddBreed_Click" />
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
            </div>
        </div>
    </asp:Panel>

    <div id="modal_knowmore" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%; width: 300px;">
                <div class="modal-body">
                    <div class="text-end">
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="login_form">
                        <h4>
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, Thankyouforyourinterest %>" runat="server" CssClass="form_label"></asp:Label></h4>
                        <p>
                            <asp:Label ID="Label10" Text="<%$Resources:Resource, Wecurrentlysupportonlyhorsesanddogs %>" runat="server" CssClass="form_label"></asp:Label>
                        </p>
                        <div id="email">
                            <a href="mailto:example@email.com">
                                <asp:Label ID="Label12" Text="<%$Resources:Resource, exampleemailcom %>" runat="server" CssClass="form_label"></asp:Label></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>

    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });
    </script>
</asp:Content>
