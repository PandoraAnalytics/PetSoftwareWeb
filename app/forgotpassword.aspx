<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpassword.aspx.cs" Inherits="Breederapp.forgotpassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="shortcut icon" href="images/website/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="images/blogo.ico" type="image/x-icon" />
    <link href="style/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="style/main.css" rel="stylesheet" type="text/css" />
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
                <div class="col-lg-5 col-md-5 col-sm-6 d-none d-sm-block offset-lg-1 offset-md-1">
                    <div class="horse_img"></div>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 offset-lg-1 offset-md-1 wrapper_form">
                    <div style="width: 300px; margin: auto;">
                        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                        <br />
                        <center>
                            <img src="images/logo.png" alt="LOGO" style="max-height: 75px;" /></center>
                        <br />
                        <div class="form_row">
                            <asp:Label ID="lblEmail" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form_input" data-validate="required email"></asp:TextBox>
                        </div>
                        <div class="form_row">
                            <asp:Button ID="btnSend" CssClass="form_button" Text="<%$Resources:Resource, Send %>" runat="server" Width="100%" OnClick="btnSend_Click" />
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
                <a href="javascript:void(0)">
                    <asp:Label ID="Label7" Text="<%$Resources:Resource, Terms %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="javascript:void(0)">
                    <asp:Label ID="Label8" Text="<%$Resources:Resource, Privacy %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="javascript:void(0)">
                    <asp:Label ID="Label9" Text="<%$Resources:Resource, MobileAppAndroid %>" runat="server" CssClass="form_label"></asp:Label></a>
                <a href="javascript:void(0)">
                    <asp:Label ID="Label10" Text="<%$Resources:Resource, MobileAppiPhone %>" runat="server" CssClass="form_label"></asp:Label></a>
            </div>
        </div>
    </form>
</body>
</html>
