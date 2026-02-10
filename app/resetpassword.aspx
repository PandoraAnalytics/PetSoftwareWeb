<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resetpassword.aspx.cs" Inherits="Breederapp.resetpassword" UICulture="en" Culture="en-US" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />    
    <title>Reset Password - Permit To Work</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/custom.css" rel="stylesheet" />
    <style>
        body {
            background: url('images/bg_init.png') no-repeat center center fixed;
            background-size: cover;
            overflow-x: hidden;
        }

        #wrapper {
            padding-top: 80px;
            transition: all 0.5s ease;
        }

        .login-page {
            box-shadow: 0 2px 2px 0 rgba(49, 46, 46, 0.7);
            border: none;
            margin: 0;
            background: #fff;
            padding: 30px;
            border-radius: 8px;
        }

        .login_label {
            display: block;
            margin-left: 20%;
            text-align: left;
        }

        .login_text {
            border: 1px solid #ccc;
            padding: 12px 15px !important;
            color: #000;
            width: 60%;
            padding: 10px;
            margin: 5px 20%;
            font-weight: normal;
        }

        .error_class {
            display: block;
            color: red;
            font-weight: normal !important;
            text-align: left;
            margin: 0 20%;
        }

        .sign_btn {
            padding: 10px;
            border-radius: 5px;
            color: #fff;
            width: 25%;
            padding: 8px 10px;
            margin: 5px 0%;
            font-size: 13px;
            border: 1px solid #0265b3;
            background: #0265b3;
        }

            .sign_btn:hover, .sign_btn:active {
                background: #0265b3;
                box-shadow: 0 0 0 2px #0199dc;
            }

        .login_form div {
            margin-bottom: 10px;
        }

        .page_title {
            margin-bottom: 20px;
        }

        .password_rule {
            color: #00a4ed;
            font-size: 12px;
            display: block;
        }
    </style>
</head>

<body>
    <div id="wrapper">
        <div class="container">
            <div class="row">
                <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12 col-xs-12">
                    <div class="login-page">
                        <div class="form">
                            <div class="form_input align_center">
                                <div>
                                    <a href="index.aspx" class="logo_link">
                                        <img src="images/logo.png" alt="logo"  style="max-height: 75px;"/></a>
                                </div>
                                <br />
                                <br />
                                <h1 class="page_title align_center"><asp:Label ID="Label3" Text="<%$Resources:Resource, ResetYourPassword %>" runat="server"></asp:Label></h1>
                                <form class="login_form" enctype="multipart/form-data" id="Form1" name="my_form" method="post" runat="server">
                                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                    <asp:Panel ID="panelExpiry" runat="server" Visible="false">
                                        <p>
                                            <asp:Label ID="lbllinkhasalreadybeenused" Text="<%$Resources:Resource, linkhasalreadybeenused %>" runat="server"></asp:Label>
                                        </p>
                                       <%-- <asp:Label ID="lblGobackto" Text="<%$Resources:Resource, Gobackto %>" runat="server"></asp:Label> <a href="index.aspx"><asp:Label ID="lblhomepage" Text="<%$Resources:Resource, homepage %>" runat="server"></asp:Label></a>.--%>
                                          <asp:Label ID="lblGobackto" Text="<%$Resources:Resource, Gobackto %>" runat="server"></asp:Label> <a href="app/landing.aspx"><asp:Label ID="lblhomepage" Text="<%$Resources:Resource, homepage %>" runat="server"></asp:Label></a>.
                                    </asp:Panel>


                                    <asp:Panel ID="panelReset" runat="server" Visible="false">
                                        <div>
                                            <label class="login_label"><asp:Label ID="Label4" runat="server"><span><%= Resources.Resource.NewPassword %></span>&nbsp;*</asp:Label><span>:</span> </label>
                                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="login_text" data-validate="required" TextMode="Password" MaxLength="15"></asp:TextBox>
                                            <span class="password_rule"><asp:Label ID="Label5" Text="<%$Resources:Resource, Password615inlength %>" runat="server"></asp:Label></span>
                                        </div>
                                        <div>
                                            <label class="login_label"><asp:Label ID="Label6" runat="server"><span><%= Resources.Resource.ReenterPassword %></span>&nbsp;*</asp:Label><span>:</span> </label>
                                            <asp:TextBox ID="txtNewPassword2" runat="server" CssClass="login_text" data-validate="required" TextMode="Password" MaxLength="15"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Button ID="btnSubmit" runat="server"  Text="<%$Resources:Resource, ResetPassword %>" CssClass="action_btn sign_btn" OnClick="btnSubmit_Click" />
                                        </div>
                                    </asp:Panel>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="page_loading_box" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content" style="margin: 40% 0 0 30%; padding: 20px; width: 105px;">
                Just a sec.
            </div>
        </div>
    </div>


    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // $('#page_loading_box').modal('show');
        });

        function closePopup() {
            //$('#page_loading_box').modal('hide');
        }
    </script>
</body>
</html>
