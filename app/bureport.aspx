<%@ Page Title="BU - Reports" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bureport.aspx.cs" Inherits="Breederapp.bureport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/dashboard.css" rel="stylesheet" />
    <style>
        .box {
            width: 30px;
            height: 30px;
            display: inline-block;
            border-radius: 3px;
            text-align: center;
            color: #fff;
            line-height: 30px;
        }

        .reload-icon {
            color: #007bff;
            cursor: pointer;
        }

            .reload-icon:hover {
                color: #0056b3;
            }

        .table-fixed tbody {
            display: block;
            height: 200px;
            overflow-y: auto;
            width: 100%;
        }

            .table-fixed thead, .table-fixed tbody tr {
                display: table;
                width: 100%;
                table-layout: fixed;
            }

        .table-fixed thead {
            width: calc(100% - 1em);
            background-color: #f8f9fa;
        }

            .table-fixed thead th {
                font-weight: bold;
                padding: 10px;
                border-bottom: 2px solid #dee2e6;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid dataviz" style="margin-top: 0px;">
        <input type="hidden" id="hid_filter" runat="server" />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Panel ID="pnlMessage" runat="server" Visible="true" HorizontalAlign="Center">
                    <h4><span>Reports Coming Soon...<br />
                        <br />
                        A smarter way to manage your reports are on its way!.</span></h4>
                </asp:Panel>
            </div>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-datepicker.js"></script>
</asp:Content>

