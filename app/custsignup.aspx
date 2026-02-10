<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="custsignup.aspx.cs" Inherits="Breederapp.custsignup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Welcome</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="icon" href="images/website/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="images/blogo.ico" type="image/x-icon" />
    <link href="style/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="style/main.css" rel="stylesheet" type="text/css" />
    <link href="css/datepicker3.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>

    <style>
        .wrapper {
            padding-top: 120px;
            transition: all 0.5s ease;
        }

        .wrapper_form {
            margin-left: -16px;
            background: #fff6ec;
        }

        .horse_img {
            background-image: url('images/login_bg.png');
            width: 100%;
            background-size: cover;
            height: 480px;
            background-repeat: no-repeat;
            overflow-x: hidden;
        }

        .otptext {
            text-align: center;
            color: #999;
            position: relative;
        }

            .otptext::before {
                position: absolute;
                content: '';
                border-top: 1px solid #ddd;
                top: 9px;
                width: 85px;
                left: 0px;
            }

            .otptext::after {
                position: absolute;
                content: '';
                border-top: 1px solid #ddd;
                top: 9px;
                width: 85px;
                right: 0px;
            }

        .form_input {
            width: 100%;
        }

        .other_links a, .other_links a:hover {
            color: #777;
            display: inline-block;
            margin-right: 20px;
        }

        .hor_line1 {
            border-top: 1px solid #ddd;
            flex: 1 1 auto;
            margin-top: 8px;
            min-width: 1px;
            padding-top: 8px;
        }

        #rdbType tr td {
            padding: 8px;
        }

            #rdbType tr td label {
                font-weight: normal;
                margin-left: 8px;
            }

        .lang_select {
            border: 1px solid #777;
            width: auto;
            margin-right: 20px;
            padding: 6px;
            height: auto;
            background-color: #fff;
            border-radius: 5px;
        }

        @media (max-width:640px) {
            .wrapper_form {
                margin-left: 0;
            }
        }
    </style>
</head>
<body>
    <form id="my_form" method="post" runat="server">
        <div class="container">
            <div class="row wrapper wrapper">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 offset-lg-1 offset-md-1 wrapper_form">
                    <div style="width: 700px; margin: auto;">
                        <asp:Panel ID="panelInfo" runat="server" Visible="false">
                            <div class="alert alert-info">
                                <asp:Literal ID="literalInfo" runat="server"></asp:Literal>
                            </div>
                        </asp:Panel>
                        <center>
                            <a href="https://pets.software/">
                                <img src="images/logo.png" id="companylogo" alt="LOGO" runat="server" style="max-height: 75px;" />
                            </a>
                            <h3>
                                <asp:Label ID="lblCompanyName" runat="server" CssClass="error_class"></asp:Label></h3>
                        </center>
                        <br />
                        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                        <br />
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblFirstName" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="255" data-validate="required"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblLastName" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" data-validate="required" MaxLength="255"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblEmailAddress" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form_input email" data-validate="required email" MaxLength="255"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblPhone" runat="server" CssClass="form_label"><span><%= Resources.Resource.Phone %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlPhoneCountryCode" runat="server" CssClass="form_input" Width="30%" data-validate="required" DataTextField="countrycode" DataValueField="id">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20" Width="66%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblAddress" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <asp:Label ID="Label29" runat="server" CssClass="form_label"><span><%= Resources.Resource.Country %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form_input" data-validate="required" DataTextField="fullname" DataValueField="id">
                                </asp:DropDownList>
                                <br />
                                <br />
                                <asp:Label ID="Label6" Text="<%$Resources:Resource, City %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Postcode %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtPincode" runat="server" CssClass="form_input" data-validate="pnumber" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Gender %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form_input" data-validate="required">
                                    <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Male %>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Female %>" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, DOB %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>
                                <%--                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>--%>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label11" Text="<%$Resources:Resource, AlternateContactNo %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtAlterContactNo" runat="server" CssClass="form_input" data-validate="phone" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblMembershipType" runat="server" CssClass="form_label"><span>Membership Type</span>&nbsp;</asp:Label>
                                <asp:DropDownList ID="ddlMembershipType" runat="server" CssClass="form_input" data-validate="required">
                                    <asp:ListItem Text="Gold" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Silver" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Platinum" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form_row">
                            <asp:Button ID="btnLogin" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnLogin_Click" />
                            &nbsp;<a href="signin.aspx"><asp:Label ID="Label4" Text="<%$Resources:Resource, BacktoLogin %>" runat="server"></asp:Label></a>&nbsp;
                        </div>

                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="other_links text-center">
                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="lang_select" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                    <asp:ListItem Text="<%$Resources:Resource, Lang_English %>" Value="en-us"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Resource, Lang_German %>" Value="de-DE"></asp:ListItem>
                </asp:DropDownList>
                <a href="https://pets.software/">
                    <asp:Label ID="Label5" Text="pets.software" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="">
                    <asp:Label ID="Label7" Text="<%$Resources:Resource, Terms %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="">
                    <asp:Label ID="Label8" Text="<%$Resources:Resource, Privacy %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="">
                    <asp:Label ID="Label9" Text="<%$Resources:Resource, MobileAppAndroid %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="">
                    <asp:Label ID="Label10" Text="<%$Resources:Resource, MobileAppiPhone %>" runat="server" CssClass="form_label"></asp:Label></a>
            </div>
        </div>
    </form>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });
    </script>
</body>
</html>
