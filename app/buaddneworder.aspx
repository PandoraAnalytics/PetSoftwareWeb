<%@ Page Title="Add New Order" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buaddneworder.aspx.cs" Inherits="Breederapp.buaddneworder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />  
    <style>
        #ContentPlaceHolder1_rdbPaymentOption tr td {
            padding: 8px;
        }

            #ContentPlaceHolder1_rdbPaymentOption tr td label {
                font-weight: normal;
                margin-left: 8px;
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
                        <asp:Label ID="lblHeading" Text="Add New Order" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 text-end">
                    <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
                </div>
            </div>
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
            <fieldset id="fs_CustomerInfo" runat="server">
                <legend>
                    <asp:Label ID="Label3" runat="server" Text="Customer Details"></asp:Label>
                </legend>
                <div class="row">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblCustomerName" runat="server" CssClass="form_label"><span>Customer Name</span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form_input" data-validate="required" DataTextField="fullname" DataValueField="id">
                            </asp:DropDownList>
                            
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label15" runat="server" CssClass="form_label"><span>Order No</span>&nbsp;</asp:Label>
                            <div style="display: inline-flex">
                                <asp:Label ID="lblOrderNo" runat="server" CssClass="form_label"></asp:Label>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label1" runat="server" CssClass="form_label"><span>Order Date</span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, SaveNext %>" runat="server" OnClick="btnSave_Click" />
                            &nbsp;
                        </div>
                    </div>
                </div>
            </fieldset>

            <fieldset id="fsAddItems" runat="server" visible="false">
                <legend>
                    <asp:Label ID="Label9" runat="server" Text="Add Cart Items"></asp:Label>
                </legend>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="lblProductError" runat="server" CssClass="error_class"></asp:Label>
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblProduct" runat="server" CssClass="form_label"><span>Item Name</span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form_input" DataTextField="ItemNameType" DataValueField="id2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label ID="lblQuantity" runat="server" CssClass="form_label"><span>Quantity</span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form_input" placeholder="Quantity" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <br />
                                <asp:LinkButton ID="lnkAdd" CssClass="add_form_btn btnborder" runat="server" OnClick="lnkAdd_Click" OnClientClick="getproductdata();">
                                    <i class="fa-solid fa-plus"></i>&nbsp;
                                <asp:Label ID="Label5" Text="Add to Cart" runat="server"></asp:Label>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset id="fsProductServiceInoTable" runat="server" visible="false">
                <legend>
                    <asp:Label ID="Label4" runat="server" Text="Cart Details"></asp:Label>
                </legend>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="table-responsive dt_container">
                            <input type="hidden" id="hdfilter" runat="server" />
                            <asp:Repeater ID="rptOrderItems" runat="server" OnItemCommand="rptOrderItems_ItemCommand">
                                <HeaderTemplate>
                                    <table class="table datatable">
                                        <thead>
                                            <tr>
                                                <th width="20%">Item</th>
                                                <th width="10%">Quantity</th>
                                                <th width="15%">Unit Cost</th>
                                                <th width="15%">Unit Tax</th>
                                                <th width="15%">Unit Price (with Tax)</th>
                                                <th width="15%">Amount</th>
                                                <th width="10%">&nbsp;</th>
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
                                        <td>
                                            <asp:LinkButton ID="btnRemove" runat="server" CommandArgument='<%# Eval("id") %>'
                                                CommandName="Remove" class="btn btn_delete"> <i class='fa fa-trash'></i></asp:LinkButton>
                                        </td>
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
                    <div class="row">
                        <asp:Label ID="lblHeadError" runat="server" CssClass="error_class"></asp:Label>
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                            <%--<asp:Button ID="btnHead" runat="server" Text="Add New Head" CssClass="add_form_btn btnborder" validate="no" OnClientClick="return addheadmodal();return false;" Visible="false" />--%>
                            <asp:LinkButton ID="btnHead" CssClass="add_form_btn btnborder" validate="no" runat="server" OnClientClick="return addheadmodal();return false;" Visible="false">
                                <asp:Label ID="Label8" Text="Add New Head" runat="server"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                            <div class="table-responsive dt_container">
                                <table class="table datatable">
                                    <thead>
                                        <tr>
                                            <th colspan="3"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Sub Total</td>
                                            <td class="text-right">
                                                <asp:Label ID="lblSubTotalCurrency" runat="server"></asp:Label>
                                                &nbsp;<asp:Label ID="lblSubTotal" runat="server"></asp:Label></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>Total Tax</td>
                                            <td class="text-right">
                                                <asp:Label ID="lblTotalTaxCurrency" runat="server"></asp:Label>
                                                &nbsp;<asp:Label ID="lblTotalTax" runat="server"></asp:Label></td>
                                            <td></td>
                                        </tr>
                                        <asp:Repeater ID="rptDynamicCharges" runat="server" OnItemCommand="rptDynamicCharges_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("orderhead") %></td>
                                                    <td class="text-right">
                                                        <%# this.ConvertToInteger(Eval("isnegative")) == 1 ? this.GetCurrntBUCurrency() + " (-) " + (Convert.ToDecimal(Eval("cost"))).ToString("0.00").Replace(",", ".") : this.GetCurrntBUCurrency() +" (+) "+ (Convert.ToDecimal(Eval("cost"))).ToString("0.00").Replace(",", ".") %>
                                                    </td>
                                                    <td class="text-danger text-right">
                                                        <asp:LinkButton ID="btnRemove" runat="server" CommandArgument='<%# Eval("id") %>'
                                                            CommandName="Remove" class="btn btn_delete"><i class='fa fa-trash'></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td><strong>TOTAL</strong></td>
                                            <td class="text-right"><strong>
                                                <asp:Label ID="lblTotalCurrency" runat="server"></asp:Label>
                                                &nbsp;<asp:Label ID="lblTotalAmount" runat="server"></asp:Label></strong></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="dt_footer">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </fieldset>
            <br />
            <fieldset id="panelOtherInfo" runat="server" visible="false">
                <legend>
                    <asp:Label ID="Label14" runat="server" Text="Payment Details"></asp:Label>
                </legend>
                <div class="row">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label10" runat="server" CssClass="form_label"><span>Payment Options</span>&nbsp;*</asp:Label>
                            <asp:RadioButtonList ID="rdbPaymentOption" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Width="100%" CellPadding="5">
                                <asp:ListItem Value="1" Text="Cash"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Credit Card"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Debit Card"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Bank Transfer"></asp:ListItem>
                                <asp:ListItem Value="5" Text="E-Wallet"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Mobile"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Prepaid Cards"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                            <asp:Label ID="Label12" runat="server" CssClass="form_label"><span>Attach Reference File</span>&nbsp;</asp:Label>
                            <input type="file" name="myfile" id="reference_pic" accept="image/*" />
                            <input type="hidden" runat="server" id="hid_reference_pic" />
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                            <asp:Label ID="Label11" runat="server" CssClass="form_label"><span>Terms and Conditions</span>&nbsp;</asp:Label>
                            <asp:TextBox ID="txtTermCondition" runat="server" CssClass="form_input" data-validate="required" MaxLength="500" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSaveDraft" CssClass="form_button" Text="Save Draft" runat="server" validate="no" OnClick="btnSaveDraft_Click" />
                        &nbsp;&nbsp;
                  <asp:Button ID="btnFinishOrder" CssClass="form_button" Text="Finish Order" runat="server" validate="no" OnClick="btnFinishOrder_Click" />
                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <div id="modal_addhead" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content form_horizontal" style="margin-top: 40%;">
                <div class="modal-body">
                    <h6>
                        <span id="Label56" class="error_class">
                            <asp:Label ID="lblHead" Text="Add New Head" class="error_class" runat="server"></asp:Label>
                        </span></h6>
                    <br />
                    <br />
                    <div class="row form_row">
                        <div class="form_label col-lg-3 col-md-3 col-sm-5 col-xs-12">
                            <asp:Label ID="Label6" runat="server" CssClass="form_label"><span>Head Name</span>&nbsp;*</asp:Label>
                        </div>
                        <div class="col-lg-9 col-md-9 col-sm-7 col-xs-12">
                            <asp:TextBox ID="txtHeadName" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="form_label col-lg-3 col-md-3 col-sm-5 col-xs-12">
                            <asp:Label ID="Label7" runat="server" CssClass="form_label"><span>Amount</span>&nbsp;*</asp:Label>
                        </div>
                        <div class="col-lg-9 col-md-9 col-sm-7 col-xs-12">
                            <asp:DropDownList ID="ddlIsNegative" runat="server" CssClass="form_input" Width="20%">
                                <asp:ListItem Text="+" Value="0"></asp:ListItem>
                                <asp:ListItem Text="-" Value="1" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;<asp:TextBox ID="txtHeadCost" runat="server" CssClass="form_input" data-validate="pdecimal" MaxLength="18"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnAddHead" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" UseSubmitBehavior="false" validate="no" OnClick="btnAddHead_Click" />
                            &nbsp;
                                   <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                       <asp:Label ID="Label13" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                   </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });           
        });

        function addheadmodal() {
            $('#modal_addhead').modal('show');
            return false;
        }

        $('#reference_pic').change(function () {
            var f = $(this);
            var fid = $(f).attr('id');

            var fileData = $(f).prop("files")[0];
            var formData = new window.FormData();
            formData.append("file", fileData);
            formData.append("extns", "image");

            var id = 'per_lbl_' + fid;
            $('#' + id).remove();

            $.ajax({
                url: 'file_upload_docs.ashx',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            if (!isNaN(percentComplete)) {
                                percentComplete = percentComplete * 100;
                                if (percentComplete > 100) percentComplete = 100;
                                var id = 'per_lbl_' + fid;
                                $('#' + id).remove();
                                $(f).after("<span id='" + id + "'>Uploaded: " + parseFloat(percentComplete).toFixed(2) + "%</span>");
                            }
                        }
                    }, false);
                    return xhr;
                },
                success: function (data) {
                    var hid = '#ContentPlaceHolder1_hid_reference_pic';
                    var fileuploadedsuccessfully = '<%= Resources.Resource.Fileuploadedsuccessfully %>';
                    $(hid).val(data);
                    alert(fileuploadedsuccessfully);
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert(fileuploadingerror);

                }
            });
        });

        $("#ContentPlaceHolder1_ddlProduct").change(function () {
            $('#ContentPlaceHolder1_lblProductError').text('');
            getproductdata();
        });

        function getproductdata() {
            var spanid = 'err_lbl_' + $('#ContentPlaceHolder1_ddlProduct').attr('id');
            $('#' + spanid).remove();

            var pid = parseInt($('#ContentPlaceHolder1_ddlProduct').val());
            if (pid > 0) {
                $.ajax({
                    type: "POST",
                    url: "buaddneworder.aspx/GetProductData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ 'pid': pid }),
                    success: function (msg) {
                        if (msg && msg.d.length > 0) {
                            var datajson = JSON.parse(msg.d);
                            var datatoshow = 'Item Cost: ' + datajson.cost + '&nbsp;&nbsp; | &nbsp; Tax: ' + datajson.taxname + ' (' + datajson.taxpercentage + '%)&nbsp;&nbsp;' + ' | &nbsp;Available:' + datajson.availablestock;
                            $('#ContentPlaceHolder1_ddlProduct').after("<span id='" + spanid + "' class='rule'>" + datatoshow + "</span>");
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>

