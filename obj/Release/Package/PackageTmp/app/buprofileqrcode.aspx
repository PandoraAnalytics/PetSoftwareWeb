<%@ Page Title="QR Code" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buprofileqrcode.aspx.cs" Inherits="Breederapp.buprofileqrcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_submit_btn, .form_submit_btn:hover {
            color: #fff;
        }

        @media print {
            body * {
                visibility: hidden;
            }

            #qrcodediv, #qrcodediv * {
                visibility: visible;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <div id="qrcodediv">
            <div style="border: 1px solid #ddd; text-align: center; width: 400px; height: 500px; margin: auto; background: #fff; padding: 20px 0;">
                <br />
                <br />
                <div>
                    <h4>
                        <asp:Label ID="lblCompany" runat="server"></asp:Label></h4>
                </div>
                <br />
                <p>Scan this QR code in pets.software app</p>
                <img src="/docs/" alt="alt" id="img_qrcode" runat="server" />
            </div>
        </div>
        <br />
        <div style="text-align: center;">
            <a href="javascript:void(0);" onclick="window.print();" runat="server" class="form_button">Print QR Code</a>
        </div>
    </div>
    <script>
        function printQRCode() {
            var divToPrint = document.getElementById('qrcodediv');
            var newWin = window.open('', 'Print-Window');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 10);
        }
    </script>
</asp:Content>

