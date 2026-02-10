<%@ Page Title="Staff Report" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bustaffreport.aspx.cs" Inherits="Breederapp.bustaffreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6>
            <asp:Label ID="Label11" Text="Staff Report" runat="server" CssClass="error_class"></asp:Label></h6>
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
                    <table class="table datatable">
                        <thead>
                            <th width="40%" sort-column-index="1">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="30%" sort-column-index="3">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, JobTitle %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label4" Text="Total Order Cost" runat="server"></asp:Label></th>
                            <th width="10%">&nbsp;</th>
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
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        var st_Active = '<%=  Resources.Resource.Active %>';
        var st_Resigned = '<%=  Resources.Resource.Resigned %>';
        var st_Terminate = '<%=  Resources.Resource.Terminate %>';
        var st_OnLeave = '<%=  Resources.Resource.OnLeave %>';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetStaffReportCount", filter);
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
            var xmldata = getdata("GetStaffReportDetails", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var secureid = record.find("securedid").text();
                    var sid = record.find("staff_id").text();
                    var userid = record.find("user_id").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("first_name").text() + ' ' + record.find("last_name").text() + '</td>';
                    contents += '<td>' + record.find("jobtitle").text() + '</td>';
                    contents += '<td>' + costCurrency + ' ' + record.find("processed_totalordercost").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='staffreportorderdetails.aspx?sid=" + secureid + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View Order' title='View Order'>View Orders</a></div></td>";
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

