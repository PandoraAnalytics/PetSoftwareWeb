<%@ Page Title="Ticket - Dashboard" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="ticketdashboard.aspx.cs" Inherits="Breederapp.ticketdashboard" %>

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
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="sec_title mb-0">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, TicketByStatus %>" CssClass="error_class" runat="server"></asp:Label>
                        </h5>

                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="showTicketByStatus();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>

                    </div>
                    <div class="card-body">
                        <canvas id="ticket_status_canvas" style="width: auto; height: 210px;"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="sec_title mb-0">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, TicketByPriority %>" CssClass="error_class" runat="server"></asp:Label></h5>
                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="showTicketByPriority();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <div class="card-body">
                        <canvas id="ticket_priority_canvas" style="width: auto; height: 210px;"></canvas>
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
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, TicketByApplication %>" CssClass="error_class" runat="server"></asp:Label>
                        </h5>
                        <ul class="list-inline mb-0 ml-auto">
                            <li class="list-inline-item">
                                <asp:Label ID="Label7" CssClass="error_class" runat="server"><span><%= Resources.Resource.From %></span> :&nbsp;
                                    <asp:TextBox ID="txtDate1" runat="server" CssClass="form_element date-picker form_input" MaxLength="10" Width="70%"></asp:TextBox>
                                </asp:Label>
                            </li>
                            <li class="list-inline-item">
                                <asp:Label ID="Label8" CssClass="error_class" runat="server"><span><%= Resources.Resource.To %></span> :&nbsp;
                                    <asp:TextBox ID="txtDate2" runat="server" CssClass="form_element date-picker form_input" MaxLength="10" Width="70%"></asp:TextBox>
                                </asp:Label>
                            </li>
                            <li class="list-inline-item">
                                <a href="javascript:void(0);" onclick="showTicketByCategory();" class="reload-icon">
                                    <i class="fas fa-sync-alt"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <div style="padding: 20px;">
                                <canvas id="ticket_category_canvas" style="width: auto; height: 210px;"></canvas>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <table id="tickettable" class="table table-fixed">
                                <thead>
                                    <tr>
                                        <th width="30%">
                                            <asp:Label ID="lblWorkforce" Text="<%$Resources:Resource, Application %>" runat="server"></asp:Label>
                                        </th>
                                        <th width="23%">
                                            <asp:Label ID="Label4" Text="<%$Resources:Resource, TotalTickets %>" runat="server"></asp:Label>
                                        </th>
                                        <th width="23%">
                                            <asp:Label ID="Label5" Text="<%$Resources:Resource, PendingEDT %>" runat="server"></asp:Label>
                                        </th>
                                        <th width="24%">
                                            <asp:Label ID="Label6" Text="<%$Resources:Resource, ApprovedEDT %>" runat="server"></asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="../js/fastselect.standalone.min.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-datepicker.js"></script>
    <script src="https://www.chartjs.org/dist/2.7.2/Chart.bundle.js"></script>
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
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });

            showTicketByStatus();
            showTicketByPriority();
            showTicketByCategory();
        });

        function showTicketByStatus() {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/TicketDash_GetTicketByStatus",
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
                            labels.push(record.find("name").text());
                            data.push(parseInt(record.find("ticketcount").text()));
                            bgcolor.push(colors[index]);
                        });

                        var configPie2 = {
                            type: 'pie',
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
                                }
                            }
                        };
                        var ctx2 = document.getElementById('ticket_status_canvas').getContext('2d');
                        window.myHorizontalBar = new Chart(ctx2, configPie2);
                    }
                }
            });
        }

        function showTicketByPriority() {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/TicketDash_GetTicketByPriority",
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
                            labels.push(record.find("priorityname").text());
                            data.push(parseInt(record.find("ticketcount").text()));
                            bgcolor.push(colors[index]);
                        });

                        var configPie2 = {
                            type: 'pie',
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
                                }
                            }
                        };
                        var ctx2 = document.getElementById('ticket_priority_canvas').getContext('2d');
                        window.myHorizontalBar = new Chart(ctx2, configPie2);
                    }
                }
            });
        }

        var ticket_category_canvas_chart = null;
        function showTicketByCategory() {
            var filter = $('#ContentPlaceHolder1_txtDate1').val() + ',' + $('#ContentPlaceHolder1_txtDate2').val();
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/TicketDash_GetTicketByCategory",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ page: 0, filter: filter }),
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
                            labels.push(record.find("appname").text());
                            data.push(parseInt(record.find("ticketcount").text()));
                            bgcolor.push(colors[index]);
                        });

                        var configPie2 = {
                            type: 'pie',
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
                                }
                            }
                        };

                        var ctx2 = document.getElementById('ticket_category_canvas').getContext('2d');
                        if (ticket_category_canvas_chart != null) {
                            ticket_category_canvas_chart.destroy();
                            ticket_category_canvas_chart = null;
                        }

                        ticket_category_canvas_chart = new Chart(ctx2, configPie2);
                        window.myHorizontalBar = ticket_category_canvas_chart;

                    }
                }
            });


            $('#tickettable tbody').html('');
            var xmldata = getdata2("GetTicketOverview", 0, filter);
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var allcontent = '';
                records.each(function () {
                    var record = $(this);
                    var contents = "<tr>";
                    contents += "<td width='30%'>" + record.find("appname").text() + "</td>";
                    contents += "<td width='23%'>" + record.find("totaltickets").text() + "</td>";
                    contents += "<td width='23%'>" + record.find("notapprovedtickets").text() + "</td>";
                    contents += "<td width='24%'>" + record.find("approvedtickets").text() + "</td>";
                    contents += "</tr>";
                    allcontent += contents;
                });
                $('#tickettable tbody').html(allcontent);
            }
            else {
                $('#tickettable tbody').html("<tr><td colspan='4'>No Records Found</td></tr>");
            }

        }
    </script>
</asp:Content>
