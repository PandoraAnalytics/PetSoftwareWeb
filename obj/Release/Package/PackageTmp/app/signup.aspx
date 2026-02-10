<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="Breederapp.signup" %>

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
                <div class="col-lg-5 col-md-5 col-sm-6 d-none d-sm-block offset-lg-1 offset-md-1">
                    <div class="horse_img"></div>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 offset-lg-1 offset-md-1 wrapper_form">
                    <div style="width: 300px; margin: auto;">
                        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                        <br />
                        <center>
                            <a href="https://pets.software/">
                                <img src="images/logo.png" alt="LOGO" style="max-height: 75px;" />
                            </a>
                        </center>
                        <br />
                        <div class="form_row">
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="30" data-validate="required" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                    <asp:Label ID="Label13" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" MaxLength="30" data-validate="required" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form_row">
                            <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form_input" MaxLength="100" data-validate="required email"></asp:TextBox>
                        </div>
                        <div class="form_row">
                            <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.MobileNumber %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form_input" MaxLength="20" data-validate="required"></asp:TextBox>
                        </div>

                        <div class="form_row">
                            <asp:Label ID="Label11" runat="server" CssClass="form_label"><span><%= Resources.Resource.NewPassword %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form_input" TextMode="Password" MaxLength="20" data-validate="required"></asp:TextBox>
                        </div>

                        <div class="form_row">
                            <asp:Label ID="Label12" runat="server" CssClass="form_label"><span><%= Resources.Resource.ReenterPassword %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtNewPassword2" runat="server" CssClass="form_input" TextMode="Password" MaxLength="20" data-validate="required"></asp:TextBox>
                        </div>
                        <div class="form_row">
                            <asp:Button ID="btnLogin" CssClass="form_button" Text="<%$Resources:Resource, Createyouraccount %>" runat="server" OnClick="btnLogin_Click" />
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

    <script>
        var typingTimer1;
        $('#ContentPlaceHolder1_txtNewPassword').on('input propertychange paste', function () {
            clearTimeout(typingTimer1);
            typingTimer1 = setTimeout(validatepassword, 800);
        });

        function validatepassword() {
            $('#rule_min_length').removeClass('active');
            $('#rule_isnumberrequired').removeClass('active');
            $('#rule_islowercaserequired').removeClass('active');
            $('#rule_isuppercaserequried').removeClass('active');
            $('#rule_isnumberspecialcharrequired').removeClass('active');

            var pwd = $.trim($("#ContentPlaceHolder1_txtNewPassword").val());
            if (pwd.length == 0) return;

            $.ajax({
                type: "POST",
                url: "changepassword.aspx/CheckPasswordKeys",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'password': pwd }),
                success: function (msg) {
                    if (msg && msg.d && msg.d.length > 0) {
                        var keys = msg.d.split(',');
                        for (var x = 0; x < keys.length; x++) {
                            $('#rule_' + keys[x] + '').addClass('active');
                        }
                    }
                }
            });
        }
    </script>

</body>
</html>
