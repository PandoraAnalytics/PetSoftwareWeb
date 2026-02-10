<%@ Page Title="User List" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="ticketuserlist.aspx.cs" Inherits="Breederapp.ticketuserlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, UserList %>" runat="server" CssClass="error_class"></asp:Label>
        </h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                          <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>
                        &nbsp;-&nbsp;
                          
                        <asp:Label ID="lblName" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label>
                        &nbsp;
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_element form_input" MaxLength="50"></asp:TextBox>
                        &nbsp;
                        
                         <asp:LinkButton ID="btnApply1" CssClass="form_button" runat="server" OnClick="btnApply_Click">
                             <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                         </asp:LinkButton>
                    </div>
                </div>
                <table class="table datatable">
                    <thead>
                        <th width="20%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label>
                        </th>
                        <th width="30%">
                            <asp:Label ID="Label5" Text="<%$Resources:Resource, Permission %>" runat="server"></asp:Label>
                        </th>
                        <th width="15%">&nbsp;</th>

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
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetTicketUserCount", filter);
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
            getdata3("GetTicketUsers", pageIndex, filter, '', getDetails_Success);
        }

        function getDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("primaryuserid").text();
                    var id1 = record.find("id").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + ' (' + record.find("email").text() + ')'+ '</td>';
                    contents += '<td>' + record.find("status").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='ticketuseredit.aspx?" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

    </script>
</asp:Content>
