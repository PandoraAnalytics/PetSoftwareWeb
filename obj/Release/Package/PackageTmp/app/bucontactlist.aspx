<%@ Page Title="Breed - Contact List" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucontactlist.aspx.cs" Inherits="Breederapp.bucontactlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, ContactList %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">            
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, AddContact %>" runat="server"></asp:Label>
                </asp:LinkButton>
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
                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                        &nbsp;
                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="form_element form_search_button" Text="<%$Resources:Resource, Exporttoexcel %>" OnClick="btnExportToExcel_Click" />&nbsp;
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdsort" runat="server" />
                    <table class="table datatable">
                        <thead>
                            <th width="25%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, ContactNumber %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label18" Text="<%$Resources:Resource, Category %>" runat="server"></asp:Label></th>
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

    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetContactCount", filter);
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
            getdata3("GetContactDetails", pageIndex, filter, '', getContactDetails_Success);
        }

        function getContactDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("fname").text() + " " + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("phone").text() + '</td>';
                    contents += '<td>' + record.find("service_typename").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='bucreatecontact.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteContact", id);
            process(1);
        }
    </script>
</asp:Content>

