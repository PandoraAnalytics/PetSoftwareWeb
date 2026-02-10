<%@ Page Title="My Company" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="custallcompanylist.aspx.cs" Inherits="Breederapp.custallcompanylist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .password_rules {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #eff2f7;
            /*margin-top: 8px;*/
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .rule {
            color: #777;
        }

            .rule.active {
                color: #000 !important;
                font-weight: 600;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="My Company" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <input type="hidden" id="hid" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Company Name"></asp:TextBox>&nbsp;&nbsp;
                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
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
                            <th width="20%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lbladdress" Text="Short Name" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label3" Text="Business Type" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label1" Text="Website" runat="server"></asp:Label></th>
                            <%--<th width="10%">&nbsp;</th>--%>
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
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetCompanyForCustomerCount", filter);
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
            getdata3("GetCompanyForCustomer", pageIndex, filter, '', GetCompanyForCustomer);
        }

        function GetCompanyForCustomer(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);
                    var secureid = record.find("securedid").text();
                    var id = record.find("id").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("companyname").text() + '</td>';
                    contents += '<td>' + record.find("companyshortname").text() + '</td>';
                    contents += '<td>' + record.find("businesstypename").text() + '</td>';
                    contents += '<td>' + record.find("website").text() + '</td>';
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }
    </script>
</asp:Content>

