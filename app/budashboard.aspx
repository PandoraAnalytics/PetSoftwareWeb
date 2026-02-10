<%@ Page Title="BU - Dashboard" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="budashboard.aspx.cs" Inherits="Breederapp.budashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <%--  <link href="css/dashboard.css" rel="stylesheet" />--%>
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

        .greentick {
            color: #0bb10b;
        }

        .redcross {
            color: #ff4040;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid dataviz" style="margin-top: 0px;">
        <input type="hidden" id="hid_filter" runat="server" />
        <asp:Panel ID="panelChecklist" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="card">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <h6 class="card-title mb-0">
                                <asp:Label ID="Label11" Text="Complete the setup steps below to proceed to the full functionality of the company. Also, refer to the ⚙️ icon above for additional settings & configurations." CssClass="error_class" runat="server"></asp:Label>
                            </h6>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="card" style="margin: 15px;">
                                    <div class="card-header d-flex justify-content-between align-items-center">
                                        <span class="card-title mb-0">
                                            <asp:Label ID="Label16" Text="Business User Checklist" CssClass="error_class" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <br />
                                    <div class="row" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkCurrency" runat="server" OnClick="lnkCurrency_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="currencyYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="currencyNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label0" runat="server" Text="Add Currency"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="lnkTax" runat="server" OnClick="lnkTax_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="taxYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="taxNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label12" runat="server" Text="Add Tax"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="lnkProductCatregory" runat="server" OnClick="lnkProductCatregory_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="categoryYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="categoryNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label14" runat="server" Text="Add Product Category"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="lnkProductBrand" runat="server" OnClick="lnkProductBrand_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="brandYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="brandNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label15" runat="server" Text="Add Product Brand"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="lnkServiceType" runat="server" OnClick="lnkServiceType_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="servicetypeYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="servicetypeNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label20" runat="server" Text="Add Service Type"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="card" style="margin: 15px;">
                                    <div class="card-header d-flex justify-content-between align-items-center">
                                        <span class="card-title mb-0">
                                            <asp:Label ID="Label17" Text="Other Checklist" CssClass="error_class" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                    <br />
                                    <div class="row" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkStaffDepartment" runat="server" OnClick="lnkStaffDepartment_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="departmentYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="departmentNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label18" runat="server" Text="Add Staff Department"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="lnkStaffjobrole" runat="server" OnClick="lnkStaffjobrole_Click" CssClass="edit_profile_link">
                                            <i class="fa-solid fa-circle-check greentick" id="jobroleYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="jobroleNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="Add Staff Jobrole"></asp:Label>
                                        </asp:LinkButton>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>       
        <br />
        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="sec_title mb-0">
                            <asp:Label ID="Label3" Text="Order By Status" CssClass="error_class" runat="server"></asp:Label>
                        </h5>

                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="showOrderByStatus();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>

                    </div>
                    <div class="card-body">
                        <canvas id="order_status_canvas" style="width: auto; height: 210px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="sec_title mb-0">
                            <asp:Label ID="Label4" Text="Item Usage" CssClass="error_class" runat="server"></asp:Label>
                        </h5>
                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="showOrderByType();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <div class="card-body">
                        <canvas id="order_type_canvas" style="width: auto; height: 210px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">
                            <asp:Label ID="Label1" Text="Today's Order" CssClass="error_class" runat="server"></asp:Label>
                        </h5>
                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="loadTodaysOrder();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <input type="hidden" id="hdpfilter" runat="server" />
                            <div class="table-responsive dt_container">
                                <table class="table datatable" id="ordertable">
                                    <thead>
                                        <th width="15%">
                                            <asp:Label ID="Label2" Text="Order No" runat="server"></asp:Label>
                                        </th>
                                        <th width="15%">
                                            <asp:Label ID="Label5" Text="Date" runat="server"></asp:Label>
                                        </th>
                                        <th width="20%">
                                            <asp:Label ID="Label6" Text="Customer" runat="server"></asp:Label>
                                            <th width="20%">
                                                <asp:Label ID="Label7" Text="Amount" runat="server"></asp:Label>
                                            </th>
                                            <th width="20%">
                                                <asp:Label ID="Label8" Text="Status" runat="server"></asp:Label>
                                            </th>
                                            <th width="10%">&nbsp;</th>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div class="dt_footer">
                                    <div class="pagination float-end" id="orderpagination">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">
                            <asp:Label ID="Label9" Text="Product running out of stock" CssClass="error_class" runat="server"></asp:Label>
                        </h5>
                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="loadProductStock();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <input type="hidden" id="hdpsfilter" runat="server" />
                            <div class="table-responsive dt_container">
                                <table class="table datatable" id="producttable">
                                    <thead>
                                        <th width="60%">
                                            <asp:Label ID="Label10" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                                        <th width="30%">
                                            <asp:Label ID="Label13" Text="Stock" runat="server"></asp:Label></th>
                                        <th width="10%">&nbsp;</th>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div class="dt_footer">
                                    <div class="pagination float-end" id="productpagination">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="https://www.chartjs.org/dist/2.7.2/Chart.bundle.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var colors = [
            "rgba(226, 137, 86, 1)",
            "rgba(226, 207, 86, 1)",
            "rgba(86, 174, 226, 1)",
            "rgba(104, 226, 86, 1)",
            "rgba(86, 226, 207, 1)",
            "rgba(174, 226, 86, 1)",
            "rgba(86, 104, 226, 1)",
            "rgba(138, 86, 226, 1)",
            "rgba(207, 86, 226, 1)",
            "rgba(226, 86, 174, 1)",
            "rgba(226, 86, 104, 1)"
        ];
    </script>
    <script>
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var filter = $('#ContentPlaceHolder1_hdpfilter').val();
        var productfilter = $('#ContentPlaceHolder1_hdpsfilter').val();
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            showOrderByStatus();
            showOrderByType();
            loadTodaysOrder();
            loadProductStock();
        });

        function showOrderByStatus() {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/BUDash_GetOrderByStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ page: 0, filter: '' }),
                success: function (msg) {
                    var xmldata = msg.d;
                    if (xmldata != null && xmldata.length > 2) {
                        var xmlDoc = $.parseXML(xmldata);
                        var xml = $(xmlDoc);

                        var labels = [];
                        var bgcolor = [];
                        var data = [];
                        var records2 = xml.find("Table");
                        records2.each(function (index, value) {
                            var record = $(this);
                            labels.push(record.find("status_name").text());
                            data.push(parseInt(record.find("ordercount").text()));
                            bgcolor.push(colors[index]);
                        });

                        var configPie2 = {
                            type: 'doughnut',
                            data: {
                                datasets: [{
                                    data: data,
                                    backgroundColor: bgcolor,
                                    label: 'Dataset 1'
                                }],
                                labels: labels
                            },
                            options: {
                                responsive: false,
                                maintainAspectRatio: true,
                                legend: {
                                    display: true,
                                    position: 'left',
                                    labels: {
                                        fontFamily: "'Nunito', sans-serif",
                                        fontStyle: "bold",
                                        fontColor: "#333",
                                        usePointStyle: true
                                    }
                                },
                                animation: {
                                    duration: 1,
                                    onComplete: function () {
                                        var chartInstance = this.chart,
                                            ctx = chartInstance.ctx;

                                        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                        ctx.textAlign = 'center';
                                        ctx.textBaseline = 'top';
                                        ctx.fillStyle = "#fff"
                                        this.data.datasets.forEach(function (dataset, i) {
                                            var meta = chartInstance.controller.getDatasetMeta(i);
                                            meta.data.forEach(function (pie, index) {
                                                var data = dataset.data[index];
                                                if (data > 0) {
                                                    var position = pie.tooltipPosition();
                                                    ctx.fillText(data, position.x, position.y - 15);
                                                }
                                            });
                                        });
                                    }
                                }
                            }
                        };
                        var ctx2 = document.getElementById('order_status_canvas').getContext('2d');
                        window.myHorizontalBar = new Chart(ctx2, configPie2);
                    }
                }
            });
        }

        function showOrderByType() {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/BUDash_GetOrderByType",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ page: 0, filter: '' }),
                success: function (msg) {
                    var xmldata = msg.d;
                    if (xmldata != null && xmldata.length > 2) {
                        var xmlDoc = $.parseXML(xmldata);
                        var xml = $(xmlDoc);

                        var labels = [];
                        var bgcolor = [];
                        var data = [];
                        var records2 = xml.find("Table");
                        records2.each(function (index, value) {
                            var record = $(this);
                            labels.push(record.find("item_type").text());
                            data.push(parseInt(record.find("item_count").text()));
                            bgcolor.push(colors[index]);
                        });

                        var configPie2 = {
                            type: 'doughnut',
                            data: {
                                datasets: [{
                                    data: data,
                                    backgroundColor: bgcolor,
                                    label: 'Dataset 1'
                                }],
                                labels: labels
                            },
                            options: {
                                responsive: false,
                                maintainAspectRatio: true,
                                legend: {
                                    display: true,
                                    position: 'left',
                                    labels: {
                                        fontFamily: "'Nunito', sans-serif",
                                        fontStyle: "bold",
                                        fontColor: "#333",
                                        usePointStyle: true
                                    }
                                },
                                animation: {
                                    duration: 1,
                                    onComplete: function () {
                                        var chartInstance = this.chart,
                                            ctx = chartInstance.ctx;

                                        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                        ctx.textAlign = 'center';
                                        ctx.textBaseline = 'top';
                                        ctx.fillStyle = "#fff"
                                        this.data.datasets.forEach(function (dataset, i) {
                                            var meta = chartInstance.controller.getDatasetMeta(i);
                                            meta.data.forEach(function (pie, index) {
                                                var data = dataset.data[index];
                                                if (data > 0) {
                                                    var position = pie.tooltipPosition();
                                                    ctx.fillText(data, position.x, position.y - 15);
                                                }
                                            });
                                        });
                                    }
                                }
                            }
                        };
                        var ctx2 = document.getElementById('order_type_canvas').getContext('2d');
                        window.myHorizontalBar = new Chart(ctx2, configPie2);
                    }
                }
            });
        }

        function loadTodaysOrder() {
            var totalcount = gettotalcount("GetTodayOrderCount", filter);
            processOrder(1);
            $('#orderpagination').bootpag({
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
                processOrder(num);
            });
        }

        function processOrder(pageIndex) {
            $('#ordertable tbody').html('');
            getdata3("GetTodayOrderDetails", pageIndex, filter, '', getTodayOrderDetails_Success);
        }

        function getTodayOrderDetails_Success(data) {
            $('#ordertable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var pad = "00000"
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var str = record.find("orderno").text();
                    var orderNo = pad.substring(0, pad.length - str.length) + str;
                    var contents = '<tr>';
                    contents += '<td>' + '#' + orderNo + '</td>';
                    contents += '<td>' + record.find("processed_date").text() + '</td>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + costCurrency + ' ' + record.find("processed_totalcost").text() + '</td>';
                    var status = "";
                    switch (record.find("status").text()) {
                        case '1':
                            status = "Processing";
                            break;

                        case '2':
                            status = "Completed";
                            break;

                        case '3':
                            status = "Deleted";
                            break;
                    }
                    contents += '<td>' + status + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buvieworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a></div></td>";
                    contents += '</tr>';
                    $('#ordertable > tbody:last').append(contents);
                });
            }
            else {
                $('#ordertable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }


        function loadProductStock() {
            var totalcount = gettotalcount("GetProductOutOfStockCount", productfilter);
            process(1);
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
                process(num);
            });
        }

        function process(pageIndex) {
            $('#producttable tbody').html('');
            getdata3("GetProductOutOfStockDetails", pageIndex, productfilter, '', getProductStock_Success);
        }

        function getProductStock_Success(data) {
            $('#producttable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("name").text() + '</td>';
                    var thvalue = parseInt(record.find("thresholdstockvalue").text());
                    var availableqty = parseInt(record.find("availablestock").text());

                    if (availableqty > thvalue) {
                        contents += '<td style="color:green";>' + record.find("availablestock").text() + '/' + record.find("totalstock").text() + '</td>';
                    }
                    else if (availableqty == thvalue) {
                        contents += '<td style="color:orange";>' + record.find("availablestock").text() + '/' + record.find("totalstock").text() + '</td>';
                    }
                    else if (availableqty < thvalue) {
                        contents += '<td style="color:red";>' + record.find("availablestock").text() + '/' + record.find("totalstock").text() + '</td>';
                    }
                    /*  contents += '<td>' + record.find("availablestock").text() + '/' + record.find("totalstock").text() + '</td>';*/
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='productview.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a></div></td>";
                    contents += '</tr>';
                    $('#producttable > tbody:last').append(contents);
                });
            }
            else {
                $('#producttable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

    </script>
</asp:Content>
