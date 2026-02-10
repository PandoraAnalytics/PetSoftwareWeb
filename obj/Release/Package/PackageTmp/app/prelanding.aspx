<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prelanding.aspx.cs" Inherits="Breederapp.prelanding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="icon" href="images/website/favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="images/website/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.2/css/all.min.css" />
    <link href="style/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="style/main.css" rel="stylesheet" type="text/css" />

    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
</head>
<body>
    <form id="my_form" runat="server">
        <div>
            <header>
                <div id="header-content">
                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                            <ul class="leftmenu">
                                <li>
                                   <img src="images/logo.png" alt="LOGO" style="max-height: 45px;" />
                                </li>
                            </ul>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12 text-end">
                            <ul class="rightmenu">
                                <li>
                                    <asp:Button ID="btnLogout" runat="server" CssClass="form_button" Text="<%$Resources:Resource, Logout %>" OnClick="btnLogout_Click"></asp:Button>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </header>
        </div>
        <div id="page-content">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <h4>
                        <asp:Label ID="lblHello" Text="<%$Resources:Resource, Hello %>" runat="server"></asp:Label>
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </h4>
                    <div style="opacity: 0.8;">
                        <asp:Label ID="Label1" Text="<%$Resources:Resource, Welcome %>" runat="server"></asp:Label>
                    </div>
                    <br />
                    <br />
                    <h3>
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, AddYourBreedMessage %>" runat="server"></asp:Label>
                    </h3>
                    <br />
                    <asp:Button ID="btnCreateNew" runat="server" CssClass="form_button" Text="<%$Resources:Resource, AddYourAnimal %>" OnClick="btnCreateNew_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnTransferRequest" runat="server" CssClass="form_button" Text="<%$Resources:Resource, TransferRequest %>" PostBackUrl="transferlist.aspx" Visible="false"></asp:Button>
                </div>
            </div>
            <br />
            <br />
            <br />
            <div class="row orange_section">
                <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                    <h4><asp:Label ID="Label4" Text="<%$Resources:Resource, Feature1 %>" runat="server"></asp:Label></h4>
                    <asp:Label ID="Label5" Text="<%$Resources:Resource, LoremIpsumissimplydummytext %>" runat="server"></asp:Label>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 text-center">
                    <img src="images/feature_dummy_image.png" alt="x" width="30%" />
                </div>
            </div>
            <div class="row white_section">
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 text-center">
                    <img src="images/feature_dummy_image.png" alt="x" width="30%" />
                </div>
                <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                    <h4><asp:Label ID="Label6" Text="<%$Resources:Resource, Feature2 %>" runat="server"></asp:Label></h4>
                    <asp:Label ID="Label7" Text="<%$Resources:Resource, LoremIpsumissimplydummytext2 %>" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row orange_section">
                <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                    <h4><asp:Label ID="Label8" Text="<%$Resources:Resource, Feature3 %>" runat="server"></asp:Label></h4>
                    <asp:Label ID="Label9" Text="<%$Resources:Resource, LoremIpsumissimplydummytext3 %>" runat="server"></asp:Label>
                    </div>
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 text-center">
                    <img src="images/feature_dummy_image.png" alt="x" width="30%" />
                </div>
            </div>
            <div class="row white_section">
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 text-center">
                    <img src="images/feature_dummy_image.png" alt="x" width="30%" />
                </div>
                <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                     <h4><asp:Label ID="Label10" Text="<%$Resources:Resource, Feature4 %>" runat="server"></asp:Label></h4>
                    <asp:Label ID="Label12" Text="<%$Resources:Resource, LoremIpsumissimplydummytext4 %>" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row orange_section">
                <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                     <h4><asp:Label ID="Label11" Text="<%$Resources:Resource, Feature5 %>" runat="server"></asp:Label></h4>
                    <asp:Label ID="Label13" Text="<%$Resources:Resource, LoremIpsumissimplydummytext5 %>" runat="server"></asp:Label>

                </div>
                <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12 text-center">
                    <img src="images/feature_dummy_image.png" alt="x" width="30%" />
                </div>
            </div>
            <br />
            <div class="text-center">
            <asp:Button ID="btnCreateNew2" runat="server" CssClass="form_button" Text="<%$Resources:Resource, AddYourAnimal %>" OnClick="btnCreateNew_Click"></asp:Button></div>
        </div>
    </form>
</body>
</html>
