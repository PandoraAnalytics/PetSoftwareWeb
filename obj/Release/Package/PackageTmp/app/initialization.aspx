<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="initialization.aspx.cs" Inherits="Breederapp.initialization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Switch Company</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/custom.css" rel="stylesheet" />
    <style>
        body {
            background: url('images/bg_init.png') no-repeat center center fixed;
            background-size: cover;
            overflow-x: hidden;
        }

        .header_custom_nav {
            float: right;
            padding: 20px 50px 0 0;
        }

            .header_custom_nav a {
                color: #fff;
            }

        .form_input ul li {
            display: inline-block;
            width: 250px;
            margin: 0 2%;
        }

        input[type=radio].css-checkbox {
            /* position: absolute;*/
            z-index: -1000;
            left: -1000px;
            overflow: hidden;
            clip: rect(0 0 0 0);
            height: 12px;
            width: 12px;
            margin: -1px;
            padding: 0;
            border: 0;
        }

            input[type=radio].css-checkbox + label.css-label {
                padding-left: 0;
                height: 21px;
                display: block;
                line-height: 21px;
                background-repeat: no-repeat;
                background-position: 0 0;
                font-size: 18px;
                vertical-align: middle;
                cursor: pointer;
                text-align: center;
                width: 100%;
                /*position: absolute;*/
                left: 86%;
                /* top: 5px; */
                bottom: 8px;
                z-index: 2;
            }

            input[type=radio].css-checkbox:checked + label.css-label {
                background-position: 0 -26px;
            }

        .list_image img {
            object-fit: scale-down;
            height: 90px;
            width: auto;
            max-width: 100%;
            cursor: pointer;
        }

        label.css-label {
            background-image: url(images/fullscreen.png);
            background-repeat: no-repeat;
            display: block;
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            font-weight: normal;
            margin-bottom: 0px;
        }

        .list_wrapper {
            text-align: center;
            margin: 5px 5px;
            position: relative;
        }

        .list_data {
            margin: 50px 0;
            font-size: 20px;
        }

        .list_image {
            background: #fff;
            padding: 10px;
            width: 230px;
            margin: 0 auto;
            height: 130px;
            display: table-cell;
            vertical-align: middle;
            box-shadow: 1px 0 20px rgba(0,0,0,.08);
            border-radius: 5px;
        }

        .error_class {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 30px;">
            <div class="text-center">
                <a href="budashboard.aspx" class="logo_link">
                    <img src="images/logo.png" alt="logo" style="max-height: 70px;" /></a>
            </div>
            <br />
            <div class="container">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 ">
                        <p class="text-center list_data">
                            <asp:Label ID="lblWelcome" Text="Welcome to pets.software" runat="server" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="lblhaveaccess" Text="You have access to the following businesses." runat="server" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="lblselectabu" Text="Please select a business from the following list and click 'Proceed' to continue" runat="server" Font-Bold="true"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>

            <div class="container">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                        <div class="form_input align_center">
                            <ul>
                                <asp:Repeater ID="repeaterCompany" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div class="list_wrapper">
                                                <div class="list_image">
                                                    <%--<img src='<%# string.Format("docs/{0}", Eval("companylogo")) %>' alt="client" for='<%# string.Format("repeaterCompany_radio1_{0}", Container.ItemIndex) %>' />--%>
                                                    <img src='../app/images/image_loading.gif' class="lazy-img" data-src='<%# (string.IsNullOrEmpty(Eval("companylogo").ToString()))? "defcomplogo2.png" :Eval("companylogo") %>' alt="company" for='<%# string.Format("repeaterCompany_radio1_{0}", Container.ItemIndex) %>' />
                                                    <br />
                                                    <br />
                                                    <div class="row form_row">
                                                        <div class="col-lg-10 col-md-10 col-sm-12 col-xs-12 ">
                                                            <label for='<%# string.Format("repeaterCompany_radio1_{0}", Container.ItemIndex) %>' class="css-label radGroup1"></label>
                                                            <asp:Label ID="lblCompName" runat="server" Text='<%# (Eval("status").ToString())=="0"?Eval("companyname")+ " (Not Approved)": Eval("companyname") %>' class="css-label radGroup1"></asp:Label>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12 ">
                                                            <input type="radio" name="radiog_lite" id="radio1" visible='<%# (Eval("status").ToString())=="0"?false:true %>' class="css-checkbox" runat="server" value='<%# Eval("id") %>' />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <br />
                            <br />
                            <asp:Button ID="btnSwitch" CssClass="form_submit_btn submit_btn_sm" Text="Proceed" runat="server" OnClick="btnSwitch_Click" />
                            <br />
                            <br />
                            <asp:LinkButton ID="lnkBack" runat="server" Text="<%$Resources:Resource, Back %>" Visible="false" OnClick="lnkBack_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="js/jquery.js"></script>
    <script src="js/data.js"></script>
    <script type="text/javascript">
        localStorage.clear();
        $(document).ready(function () {
            loadLazyImages();

            $('input[type=radio]').click(function () {
                var cname = $(this).attr('class');
                $('.' + cname).each(function () {
                    $(this).prop('checked', false);
                });
                $(this).prop('checked', true);
            });

            $('.list_image img').click(function () {
                $('.css-checkbox').each(function () {
                    $(this).prop('checked', false);
                });
                var checkid = $(this).attr('for');
                $('#' + checkid).prop('checked', true);
            });
        });


    </script>

</body>
</html>
