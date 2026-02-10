<%@ Page Title="Association List" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="manageassociation.aspx.cs" Inherits="Breederapp.manageassociation" %>

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
            <asp:Label ID="Label11" Text="<%$Resources:Resource, AssociationList %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <a href="associationadd.aspx" class="add_form_btn btnborder">+&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
            </a>
            <%-- <a href="associationsearch.aspx" class="edit_profile_link">&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource, AssociationSearch %>"></asp:Label>
            </a>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Name"></asp:TextBox>&nbsp;&nbsp;
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
                            <th width="25%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="35%">
                                <asp:Label ID="lbladdress" Text="<%$Resources:Resource, TypeofBreed %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, ManageRequest %>" runat="server"></asp:Label></th>
                            <th width="20%">&nbsp;</th>
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
        var a_Owner = '<%=  Resources.Resource.Owner %>';
        var a_Admin = '<%=  Resources.Resource.Admin %>';
        var a_ExitMembership = '<%=  Resources.Resource.ExitMembership %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetAllMyAssociationCount", filter);
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
            getdata3("GetAllMyAssociation", pageIndex, filter, '', GetAllAssociations);
        }

        function GetAllAssociations(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);
                    var id1 = record.find("id").text();
                    var id = record.find("securedid").text();
                    var email = record.find("email").text();
                    var isApprove = record.find("isapprove").text();
                    var typeValue = '';
                    var contents = '<tr>';
                    var manageJoinRequestText = '<%= Resources.Resource.ManageJoinRequest %>' + " (" + record.find("totalpendingmembers").text() + ")";
                    var isownercheck = record.find("isowner").text();
                    var isAdmin = record.find("isadmin").text();

                    if (isownercheck == 1)
                        contents += '<td><div class="assocation_title">' + record.find("name").text() + ' <span class="password_rules">' + a_Owner +'</span> </div></td>';
                    else {
                        if (isAdmin == 1)
                            contents += '<td><div class="assocation_title">' + record.find("name").text() + ' <span class="password_rules">' + a_Admin +'</span> </div></td>';
                        else
                            contents += '<td><div class="assocation_title">' + record.find("name").text() + '</div></td>';
                    }

                    if (record.find("breednames").text().length == 0) contents += '<td>-</td>';
                    else contents += '<td>' + record.find("breednames").text() + '</td>';

                    var joinrequestaction = "<a href='memberrequest.aspx?aid=" + id + "' class='btn btn_edit'>" + manageJoinRequestText + "</a>";
                    var exitmemberaction = "<a href='existmember.aspx?mid=" + id + "'  data-toggle='tooltip' data-placement='top' data-original-title='Exit Member' class='btn btn_edit'>" + a_ExitMembership + "</a>";

                    var editdeleteactions = "<a href='associationedit.aspx?" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete'><i class='fa-regular fa-trash-can'></i></a>";
                    var otheractions = "<a href='memberlist.aspx?aid=" + id + "' class='btn btn_edit'  data-toggle='tooltip' data-placement='top' data-original-title='Manage Member'><i class='fa-solid fa-user-plus'></i></a><a href='inviteassociation.aspx?id=" + id + "' class='btn btn_edit'  data-toggle='tooltip' data-placement='top' data-original-title='Invite Members'><i class='fa-solid fa-share-nodes'></i></a>";

                    var adminmemberactions = '&nbsp;';
                    var associationowneractions = '&nbsp;';
                   
                    if (isownercheck == 1) {
                        adminmemberactions = joinrequestaction;
                        if (isApprove == 0) {
                            associationowneractions = editdeleteactions;
                        }
                        else if (isApprove == 2) { // Reject
                            associationowneractions = editdeleteactions;
                        }
                        else if (isApprove == 1) { // Accept or approve
                            associationowneractions = editdeleteactions + otheractions;
                        }
                    } else {
                        if (isAdmin == 1) {
                            adminmemberactions = joinrequestaction + exitmemberaction;
                            associationowneractions = otheractions;
                        } else {
                            adminmemberactions = exitmemberaction;
                            associationowneractions = '&nbsp;';
                        }
                    }
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'>" + adminmemberactions + "</div></td>";
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'>" + associationowneractions + "</div></td>";
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("Deleteassociation", id);
            process(1);
        }

        function AdminProcess(value, type) {
            __doPostBack("delete", value);
        }

    </script>
</asp:Content>
