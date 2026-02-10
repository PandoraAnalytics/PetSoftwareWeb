<%@ Page Title="Customer List" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="customerlist.aspx.cs" Inherits="Breederapp.customerlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="Customer List" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="Add Customer" runat="server"></asp:Label>
            </asp:LinkButton>
            &nbsp;&nbsp;
                 <a href="buimportcustomer.aspx" runat="server" id="lnkImportCustomer" class="add_form_btn btnborder">
                     <i class="fa fa-upload" aria-hidden="true"></i>
                     <asp:Label ID="Label5" Text="Import Customer" runat="server"></asp:Label>
                 </a>
        </div>
        <div class="clearfix"></div>
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
                    <input type="hidden" id="hdsort" runat="server" />
                    <table class="table datatable sorttable">
                        <thead>
                            <th width="30%" sort-column-index="1">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="30%" sort-column-index="2">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="30%" sort-column-index="3">
                                <asp:Label ID="Label16" Text="Membership Type" runat="server"></asp:Label></th>
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
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetCustomerCount", filter);
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

            order = $('#ContentPlaceHolder1_hdsort').val();
            if (order.length > 0) {
                var tokens = order.split('_');
                var $col = $('[sort-column-index="' + tokens[0] + '"]');
                if ($col.length > 0) {
                    $('.fa-long-arrow-up').remove();
                    $('.fa-long-arrow-down').remove();

                    var thdata = $col.html();
                    if (tokens[1] == '1') {
                        $col.html(thdata + ' <i class="fa fa-long-arrow-up"></i>');
                    }
                    else if (tokens[1] == '0') {
                        $col.html(thdata + ' <i class="fa fa-long-arrow-down"></i>');
                    }
                }
            }
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            window.scrollTo(0, 0);
            var xmldata = getdata("GetCustomerDetails", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var secureid = record.find("securedid").text();
                    var id = record.find("id").text();
                    var userid = record.find("userid").text();
                    var contents = '<tr>';
                    var contents = '<tr>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    var type = '';
                    switch (record.find("membershiptype").text()) {
                        case '1':
                            type = "Gold";
                            break;
                        case '2':
                            type = "Silver";
                            break;
                        case '3':
                            type = "Platinum";
                            break;
                        default:
                            type = "NA";
                            break;

                    }
                    contents += '<td>' + type + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='customerview.aspx?cid=" + secureid + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + secureid + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a><a href='bucustomerlanding.aspx?cid=" + secureid + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Manage' title='Manage'>Manage</a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteCustomer", id);
            process(1);
        }
    </script>
</asp:Content>
