<%@ Page Title="Combo View" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="comboview.aspx.cs" Inherits="Breederapp.comboview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .photos {
            /*margin-bottom: 25px;*/
        }

            .photos li {
                display: inline-block;
                padding: 10px;
                position: relative;
            }

                .photos li .photowrapper {
                    display: inline-block;
                    /* min-width: 120px; */
                    border: solid 1px #ddd;
                    text-align: center;
                    padding: 5px 10px;
                    color: #333;
                    border-radius: 5px;
                    height: 100px;
                    width: auto;
                }

                .photos li .photo {
                    width: 130px;
                    background-repeat: no-repeat;
                    background-size: contain;
                    background-repeat: no-repeat;
                    background-position: center;
                    height: auto !important;
                    height: 100%;
                    min-height: 100%;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="lblComboView" Text="Combo View" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <asp:Panel ID="panelAction" runat="server">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                        <asp:Button ID="btnEdit" runat="server" Text="Edit Combo" CssClass="form_button" OnClick="btnEdit_Click" validate="no" Visible="true" />&nbsp;&nbsp; 
                          <a href="combolist.aspx">
                              <asp:Label ID="lblBack" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label></a>
                    </div>
                </div>
                <br />
            </asp:Panel>
            <div class="col-lg-12 col-md-12 col-sm-8 col-xs-12">
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblName" Text="<%$Resources:Resource, Title %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblComboName" Text="-" runat="server"></asp:Label>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblCostt" Text="Actual Cost" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblCostCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblCost" Text="-" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label5" Text="Tax" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblTax" runat="server"></asp:Label>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:Panel ID="panelProfiePic" runat="server" Visible="false">
                            <a href="javascript:void(0);" id="lnkComboProfilePic" runat="server" target="_blank">View Profile Pic</a>
                        </asp:Panel>
                    </div>
                </div>
                <br />
                <div class="row">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <input type="hidden" id="hdpfilter" runat="server" />
                        <input type="hidden" id="cplist" runat="server" />
                        <table class="table datatable" id="producttable" style="width: 90% !important;">
                            <thead>
                                <tr>
                                    <th colspan="3" style="text-align: left; font-weight: bold; font-size: 1.2em;">
                                        <asp:Label ID="Label11" Text="Product List" runat="server" CssClass="error_class"></asp:Label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div class="dt_footer" style="width: 90% !important;">
                            <div class="pagination float-end" id="productpagination">
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <input type="hidden" id="hdsfilter" runat="server" />
                        <input type="hidden" id="cplist2" runat="server" />
                        <table class="table datatable" id="servicetable" style="width: 90% !important;">
                            <thead>
                                <tr>
                                    <th colspan="3" style="text-align: left; font-weight: bold; font-size: 1.2em;">
                                        <asp:Label ID="Label1" Text="Service List" runat="server" CssClass="error_class"></asp:Label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div class="dt_footer" style="width: 90% !important;">
                            <div class="pagination float-end" id="servicepagination">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var productfilter = '';
        var servicefilter = '';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            loadProduct();
            loadService();
        });

        function loadProduct() {
            productfilter = $('#ContentPlaceHolder1_hdpfilter').val();
            var totalcount = gettotalcount("GetProductForComboCount", productfilter);
            processproduct(1);
            $('#productpagination').bootpag({
                total: totalcount,
                page: 1,
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on("page", function (event, num) {
                processproduct(num);
            });
        }

        function processproduct(pageIndex) {
            $('#producttable tbody').html('');
            getdata3("GetProductForComboDetails", pageIndex, productfilter, '', getProductDetails_Success);
        }

        function getProductDetails_Success(data) {
            $('#producttable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var array = $("#ContentPlaceHolder1_cplist").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var id1 = record.find("id").text();
                    var contents = '<tr>';
                    var inArray = jQuery.inArray(id1, array);
                    if (inArray >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" disabled></td>';
                    var main_desc = record.find("description").text();
                    if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                    contents += '<td width="80%">' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + main_desc + '</div></td>';
                    contents += '<td width="15%">' + costCurrency + ' ' + record.find("processed_cost").text() + '</div></td>';
                    contents += '</tr>';
                    $('#producttable > tbody:last').append(contents);
                });
            }
            else {
                $('#producttable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function loadService() {
            servicefilter = $('#ContentPlaceHolder1_hdsfilter').val();
            var totalcount = gettotalcount("GetServiceCount", servicefilter);
            processservice(1);
            $('#servicepagination').bootpag({
                total: totalcount,
                page: 1,
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on("page", function (event, num) {
                processservice(num);
            });
        }

        function processservice(pageIndex) {
            $('#servicetable tbody').html('');
            getdata3("GetServicesDetails", pageIndex, servicefilter, '', getServicesDetails_Success);
        }

        function getServicesDetails_Success(data) {
            $('#servicetable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var array2 = $("#ContentPlaceHolder1_cplist2").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var id2 = record.find("id").text();
                    var contents = '<tr>';
                    var inArray2 = jQuery.inArray(id2, array2);
                    if (inArray2 >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" disabled></td>';
                    var main_desc = record.find("description").text();
                    if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                    contents += '<td width="80%">' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + main_desc + '</div></td>';
                    contents += '<td width="15%">' + costCurrency + ' ' + record.find("processed_cost").text() + '</div></td>';
                    contents += '</tr>';
                    $('#servicetable > tbody:last').append(contents);
                });
            }
            else {
                $('#servicetable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }
    </script>
</asp:Content>
