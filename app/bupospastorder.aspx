<%@ Page Title="POS Past Orders" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bupospastorder.aspx.cs" Inherits="Breederapp.bupospastorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6>
            <asp:Label ID="Label11" Text="POS Past Orders" runat="server" CssClass="error_class"></asp:Label></h6>

        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="hidden" id="hdfilter" runat="server" />
                <div class="filter_section">
                    <label><span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label3" Text="<%$Resources:Resource, Filters %>" runat="server"></asp:Label></label>
                    <asp:TextBox ID="txtStartDate" runat="server" placeholder="Start Date" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>&nbsp;
                
                        <asp:TextBox ID="txtEndDate" runat="server" placeholder="End Date" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>&nbsp;
                    
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form_input" DataTextField="fullname" DataValueField="id">
                        </asp:DropDownList>&nbsp;
                    
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_input">
                            <asp:ListItem Text="- Status -" Value=""></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Deleted" Value="3"></asp:ListItem>
                        </asp:DropDownList>&nbsp;                        
                      
                         <asp:TextBox ID="txtOrderNo" runat="server" placeholder="Order No" CssClass="form_element form_input" data-validate="pnumber" MaxLength="100"></asp:TextBox>&nbsp;                              
                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                            <asp:Label ID="Label8" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    &nbsp;
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable" id="closetable">
                        <thead>
                            <th width="20%">
                                <asp:Label ID="Label1" Text="Order No" runat="server"></asp:Label></th>
                            <th width="10%">
                                <asp:Label ID="Label2" Text="Status" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label9" Text="Date" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label16" Text="Customer" runat="server"></asp:Label>
                                <th width="20%">
                                    <asp:Label ID="Label4" Text="Amount" runat="server"></asp:Label></th>
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
    <script src="js/data.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        var orderId = '<%=  ViewState["orderid"] %>';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var filter = $('#ContentPlaceHolder1_hdfilter').val();
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            loadClosedOrder();
        });

        function loadClosedOrder() {
            var totalcount = gettotalcount("GetClosedOrderCount", filter);
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
            getdata3("GetClosedOrderDetails", pageIndex, filter, '', getClosedOrderDetails_Success);
        }

        function getClosedOrderDetails_Success(data) {
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
                    var status = "";
                    switch (record.find("status").text()) {
                        case '2':
                            status = "Completed";
                            break;

                        case '3':
                            status = "Deleted";
                            break;
                    }
                    contents += '<td>' + status + '</td>';
                    contents += '<td>' + record.find("processed_date").text() + '</td>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + costCurrency + ' ' + record.find("processed_totalcost").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='buposvieworder.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
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

    </script>
</asp:Content>
