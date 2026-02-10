<%@ Page Title="POS Order View" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buposvieworder.aspx.cs" Inherits="Breederapp.buposvieworder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rule {
            color: #999b9c !important;
        }

        legend {
            display: block;
            width: 100%;
            padding: 0;
            font-weight: 600;
            margin-bottom: 20px;
            font-size: 15px;
            line-height: inherit;
            color: #333;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="form_horizontal">
            <div class="row">
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                    <h6 class="page_title">
                        <asp:Label ID="lblHeading" Text="POS Order View" CssClass="error_class" runat="server"></asp:Label></h6>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 text-end">
                    <asp:LinkButton ID="btnGenerateReport" runat="server" Text="Invoice PDF" CssClass="add_form_btn btnborder" OnClick="btnGenerateReport_Click" Visible="false"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
                </div>
            </div>
            <fieldset id="fs_CustomerInfo" runat="server">
                <legend>
                    <asp:Label ID="Label3" runat="server" Text="Customer Details"></asp:Label></legend>
                <div class="row">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label15" runat="server" CssClass="form_label"><span>Order No</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblCustomer" runat="server" CssClass="form_label"><span>Customer Name</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label5" runat="server" CssClass="form_label"><span>Status</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label1" runat="server" CssClass="form_label"><span>Order Date</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset id="fsProductServiceInoTable" runat="server">
                <legend>
                    <asp:Label ID="Label4" runat="server" Text="Cart Details"></asp:Label></legend>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="table-responsive dt_container">
                            <input type="hidden" id="hdfilter" runat="server" />
                            <asp:Repeater ID="rptOrderItems" runat="server">
                                <HeaderTemplate>
                                    <table class="table datatable">
                                        <thead>
                                            <tr>
                                                <th width="30%">Item</th>
                                                <th width="10%">Quantity</th>
                                                <th width="15%">Unit Cost</th>
                                                <th width="15%">Unit Tax</th>
                                                <th width="15%">Unit Price (with Tax)</th>
                                                <th width="15%">Amount</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ItemNameType") %></td>
                                        <td><%# Eval("quan") %></td>
                                        <td><%# this.GetCurrntBUCurrency() + " " + (Convert.ToDecimal(Eval("itemcost"))).ToString("0.00").Replace(",", ".") %></td>
                                        <td><%# Eval("taxname") %> (<%# Eval("tpercentage") %>%)</td>
                                        <td><%# this.GetCurrntBUCurrency() + " " + (Convert.ToDecimal(Eval("totalcost"))).ToString("0.00").Replace(",", ".") %></td>
                                        <td><%# this.GetCurrntBUCurrency() + " " + (Convert.ToDecimal(Eval("itemtotalcost"))).ToString("0.00").Replace(",", ".") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                        </table>   
                                    <div class="dt_footer">
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-6col-xs-12">
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <div class="table-responsive dt_container">
                            <table class="table datatable">
                                <thead>
                                    <tr>
                                        <th colspan="2"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Sub Total</td>
                                        <td class="text-right">
                                            <asp:Label ID="lblSubTotalCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblSubTotal" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Total Tax</td>
                                        <td class="text-right">
                                            <asp:Label ID="lblTotalTaxCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblTotalTax" runat="server"></asp:Label></td>

                                    </tr>
                                    <asp:Repeater ID="rptDynamicCharges" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("orderhead") %></td>
                                                <td class="text-right">
                                                    <%# this.ConvertToInteger(Eval("isnegative")) == 1 ? this.GetCurrntBUCurrency() + " (-) " + (Convert.ToDecimal(Eval("cost"))).ToString("0.00").Replace(",", ".") : this.GetCurrntBUCurrency() +" (+) "+ (Convert.ToDecimal(Eval("cost"))).ToString("0.00").Replace(",", ".") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td><strong>TOTAL</strong></td>
                                        <td class="text-right"><strong>
                                            <asp:Label ID="lblTotalCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblTotalAmount" runat="server"></asp:Label></strong></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="dt_footer">
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset id="panelOtherInfo" runat="server">
                <legend>
                    <asp:Label ID="Label14" runat="server" Text="Payment Details"></asp:Label></legend>
                <div class="row">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label10" runat="server" CssClass="form_label"><span>Payment Option</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblPaymentOption" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label12" runat="server" CssClass="form_label"><span>Reference File</span>&nbsp;</asp:Label>
                            <a href="javascript:void(0);" id="lnkFile" runat="server" target="_blank">View Reference File</a>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label11" runat="server" CssClass="form_label"><span>Terms and Conditions</span>&nbsp;</asp:Label>
                            <asp:Label ID="lblTermCondition" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>      
        $(document).ready(function () {
        });
    </script>
</asp:Content>
