<%@ Page Title="Customer Report" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucustomerreport.aspx.cs" Inherits="Breederapp.bucustomerreport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="form_horizontal">
        <h6>
            <asp:Label ID="Label11" Text="Customer Report" runat="server" CssClass="error_class"></asp:Label></h6>       
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Name"></asp:TextBox>&nbsp;&nbsp;
                         <asp:TextBox ID="txtEmail" runat="server" CssClass="form_input" MaxLength="40" placeholder="Email"></asp:TextBox>&nbsp;&nbsp;
                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable ">
                        <thead>
                            <th width="25%" sort-column-index="1">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="25%" sort-column-index="2">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="18%" sort-column-index="2">
                                <asp:Label ID="Label4" Text="<%$Resources:Resource, Mobile %>" runat="server"></asp:Label></th>
                              <th width="18%" sort-column-index="2">
                                <asp:Label ID="Label2" Text="Total Order Cost" runat="server"></asp:Label></th>
                            <th width="14%">&nbsp;</th>
                        </thead>

                        <tbody>
                        </tbody>
                    </table>
                    <div class="dt_footer">
                        <div class="pagination float-end">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetCustomerReportCount", filter);
            process(1);
            $('.pagination').bootpag({
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
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            window.scrollTo(0, 0);
            var xmldata = getdata("GetCustomerReportDetails", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var secureid = record.find("securedid").text();
                    var cid = record.find("customer_id").text();
                    var userid = record.find("user_id").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("first_name").text() + ' ' + record.find("last_name").text() + '</td>';
                    contents += '<td>' + record.find("user_email").text() + '</td>';  
                    contents += '<td>' + record.find("user_phone").text() + '</td>';  
                    contents += '<td>' + costCurrency + ' ' + record.find("processed_totalordercost").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='custreportorderdetails.aspx?cid=" + secureid + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View Order' title='View Order'>View Orders</a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
        }

       
    </script>
</asp:Content>