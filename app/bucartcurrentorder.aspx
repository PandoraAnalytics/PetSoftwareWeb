<%@ Page Title="Cart Current Order" Language="C#" MasterPageFile="~/app/bu.Master" AutoEventWireup="true" CodeBehind="bucartcurrentorder.aspx.cs" Inherits="PetsSoftware.app.bucartcurrentorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
            /*color: var(--bs-nav-pills-link-active-color) !important;*/
            /*background-color: var(--bs-nav-pills-link-active-bg);*/
            background-color: #f28c0e;
        }

        .nav-pills .nav-link {
            border-radius: 0 !important;
        }

        .nav-link:focus, .nav-link:hover {
            color: #ffffff !important;
        }

        .nav-link {
            color: #ffffff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h6 class="page_title">
                    <asp:Label ID="lblHeading" Text="Cart Current Order" CssClass="error_class" runat="server"></asp:Label></h6>
            </div>
        </div>
        <br />
        <div class="dt_tab_pages">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                        <li id="tablnkBasic" class="nav-item" role="presentation">
                            <button class="nav-link active" id="basic_details-tab" data-bs-toggle="pill" data-bs-target="#tab_basic_details" type="button" role="tab" aria-controls="tab_basic_details"
                                aria-selected="true">
                                <asp:Label ID="lblBasic" runat="server" Text="Current Orders"></asp:Label></button>
                        </li>
                        <li id="tablnkImages" class="nav-item" role="presentation">
                            <button class="nav-link" id="images-tab" data-bs-toggle="pill" data-bs-target="#tab_images" type="button" role="tab" aria-controls="tab_images"
                                aria-selected="false">
                                <asp:Label ID="lblImages" runat="server" Text="POS Current Orders"></asp:Label></button>
                        </li>
                    </ul>

                    <div class="tab-content clearfix" id="pills-tabContent">
                        <!------------ tab_1_current_orders ------------>
                        <div class="tab-pane fade show active" role="tabpanel" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0">
                            <div class="form_horizontal basic_deatils_form" style="line-height: 25px;">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <h6>
                                            <asp:Label ID="Label10" Text="Processing Orders" runat="server" CssClass="error_class"></asp:Label></h6>
                                        <div class="table-responsive dt_container">
                                            <input type="hidden" id="hdfilter" runat="server" />
                                            <table class="table datatable" id="processtable">
                                                <thead>
                                                    <th width="25%">
                                                        <asp:Label ID="Label1" Text="Order No" runat="server"></asp:Label></th>
                                                    <th width="25%">
                                                        <asp:Label ID="Label9" Text="Date" runat="server"></asp:Label></th>
                                                    <th width="20%">
                                                        <asp:Label ID="Label16" Text="Customer" runat="server"></asp:Label>
                                                        <th width="20%">
                                                            <asp:Label ID="Label2" Text="Amount" runat="server"></asp:Label></th>
                                                        <th width="10%">&nbsp;</th>
                                                </thead>

                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div class="dt_footer">
                                                <div class="pagination float-end" id="processpagination">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <h6>
                                            <asp:Label ID="Label12" Text="Today's Order(Closed)" runat="server" CssClass="error_class"></asp:Label></h6>
                                        <div class="table-responsive dt_container">
                                            <input type="hidden" id="hdpfilter" runat="server" />
                                            <table class="table datatable" id="closetable">
                                                <thead>
                                                    <th width="25%">
                                                        <asp:Label ID="Label3" Text="Order No" runat="server"></asp:Label></th>
                                                    <th width="25%">
                                                        <asp:Label ID="Label5" Text="Date" runat="server"></asp:Label></th>
                                                    <th width="20%">
                                                        <asp:Label ID="Label6" Text="Customer" runat="server"></asp:Label>
                                                        <th width="20%">
                                                            <asp:Label ID="Label7" Text="Amount" runat="server"></asp:Label></th>
                                                        <th width="10%">&nbsp;</th>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>

                                            <div class="dt_footer">
                                                <div class="pagination float-end" id="closepagination">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_2_pos_current_orders ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_images" aria-labelledby="images-tab" tabindex="0">
                            <div class="form_horizontal images_form">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <h6>
                                            <asp:Label ID="Label4" Text="Processing Orders" runat="server" CssClass="error_class"></asp:Label></h6>
                                        <div class="table-responsive dt_container">
                                            <input type="hidden" id="hdfilterpos" runat="server" />
                                            <table class="table datatable" id="processpostable">
                                                <thead>
                                                    <th width="25%">
                                                        <asp:Label ID="Label8" Text="Order No" runat="server"></asp:Label></th>
                                                    <th width="25%">
                                                        <asp:Label ID="Label11" Text="Date" runat="server"></asp:Label></th>
                                                    <th width="20%">
                                                        <asp:Label ID="Label13" Text="Customer" runat="server"></asp:Label>
                                                        <th width="20%">
                                                            <asp:Label ID="Label14" Text="Amount" runat="server"></asp:Label></th>
                                                        <th width="10%">&nbsp;</th>
                                                </thead>

                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div class="dt_footer">
                                                <div class="pagination float-end" id="processpospagination">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <h6>
                                            <asp:Label ID="Label15" Text="Today's Order(Closed)" runat="server" CssClass="error_class"></asp:Label></h6>
                                        <div class="table-responsive dt_container">
                                            <input type="hidden" id="hdpfilterpos" runat="server" />
                                            <table class="table datatable" id="closepostable">
                                                <thead>
                                                    <th width="25%">
                                                        <asp:Label ID="Label17" Text="Order No" runat="server"></asp:Label></th>
                                                    <th width="25%">
                                                        <asp:Label ID="Label18" Text="Date" runat="server"></asp:Label></th>
                                                    <th width="20%">
                                                        <asp:Label ID="Label19" Text="Customer" runat="server"></asp:Label>
                                                        <th width="20%">
                                                            <asp:Label ID="Label20" Text="Amount" runat="server"></asp:Label></th>
                                                        <th width="10%">&nbsp;</th>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>

                                            <div class="dt_footer">
                                                <div class="pagination float-end" id="closepospagination">
                                                </div>
                                            </div>
                                        </div>
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
    <script src="js/jquery.bootpag.min.js"></script>

    <script>
        var orderId = '<%=  ViewState["orderid"] %>';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var filter = $('#ContentPlaceHolder1_hdfilter').val();
        var filter2 = $('#ContentPlaceHolder1_hdpfilter').val();
        var filter3 = $('#ContentPlaceHolder1_hdfilterpos').val();
        var filter4 = $('#ContentPlaceHolder1_hdpfilterpos').val();
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            loadProcessingOrder();
            loadClosedOrder();
            loadPosProcessingOrder();
            loadPosClosedOrder();
        });

        function loadProcessingOrder() {
            var totalcount = gettotalcount("GetProcessingOrderCount", filter);
            processorder(1);
            $('#processpagination').bootpag({
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
                processorder(num);
            });
        }

        function processorder(pageIndex) {
            $('#processtable tbody').html('');
            getdata3("GetProcessingOrderDetails", pageIndex, filter, '', getProcessingDetails_Success);
        }

        function getProcessingDetails_Success(data) {
            $('#processtable tbody').html('');
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
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buaddneworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('#processtable > tbody:last').append(contents);
                });
            }
            else {
                $('#processtable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("Deleteorder", id);
            loadProcessingOrder(1);
        }

        function loadClosedOrder() {
            var totalcount = gettotalcount("GetTodayClosedOrderCount", filter2);
            process(1);
            $('#closepagination').bootpag({
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
            $('#closetable tbody').html('');
            getdata3("GetTodayClosedOrderDetails", pageIndex, filter2, '', getTodayClosedOrderDetails_Success);
        }

        function getTodayClosedOrderDetails_Success(data) {
            $('#closetable tbody').html('');
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
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buvieworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('#closetable > tbody:last').append(contents);
                });
            }
            else {
                $('#closetable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("Deleteorder", id);
            loadProcessingOrder(1);
        }

        //posorder
        function loadPosProcessingOrder() {
            var totalcount = gettotalcount("GetProcessingOrderCount", filter3);
            processposorder(1);
            $('#processpospagination').bootpag({
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
                processposorder(num);
            });
        }

        function processposorder(pageIndex) {
            $('#processpostable tbody').html('');
            getdata3("GetProcessingOrderDetails", pageIndex, filter3, '', getPosProcessingDetails_Success);
        }

        function getPosProcessingDetails_Success(data) {
            $('#processpostable tbody').html('');
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
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buposaddneworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deletePosRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('#processpostable > tbody:last').append(contents);
                });
            }
            else {
                $('#processpostable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deletePosRecord(id) {
            deleteData("Deleteorder", id);
            loadPosProcessingOrder(1);
        }

        function loadPosClosedOrder() {
            var totalcount = gettotalcount("GetTodayClosedOrderCount", filter4);
            processpos(1);
            $('#closepospagination').bootpag({
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
                processpos(num);
            });
        }

        function processpos(pageIndex) {
            $('#closepostable tbody').html('');
            getdata3("GetTodayClosedOrderDetails", pageIndex, filter4, '', getPosTodayClosedOrderDetails_Success);
        }

        function getPosTodayClosedOrderDetails_Success(data) {
            $('#closepostable tbody').html('');
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
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buposvieworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deletePosRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('#closepostable > tbody:last').append(contents);
                });
            }
            else {
                $('#closepostable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deletePosRecord(id) {
            deleteData("Deleteorder", id);
            loadPosProcessingOrder(1);
        }

    </script>

</asp:Content>
