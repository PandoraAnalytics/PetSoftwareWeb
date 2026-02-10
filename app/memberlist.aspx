<%@ Page Title="Association - Member List" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="memberlist.aspx.cs" Inherits="Breederapp.memberlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Memberlist %>" runat="server"></asp:Label>&nbsp;
            <asp:Label ID="lblAssociationTitle" Text="" runat="server"></asp:Label>
        </h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <a href="importmembers.aspx" runat="server" id="lnkImportMember" class="add_form_btn btnborder">
                    <i class="fa fa-upload" aria-hidden="true"></i>
                    <asp:Label ID="Label5" Text="<%$Resources:Resource, ImportMembers %>" runat="server"></asp:Label>
                </a>
                &nbsp;&nbsp;
                 <a runat="server" id="lnkAddMember" href="addmember.aspx" class="add_form_btn btnborder">
                     <i class="fa-solid fa-user-plus" aria-hidden="true"></i>
                     <asp:Label ID="lblAddProcess" Text="<%$Resources:Resource, AddMember %>" runat="server"></asp:Label>
                 </a>

            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;-&nbsp;
                        <asp:Label ID="Label4" Text="<%$Resources:Resource, Name %>" runat="server" CssClass="form_element"></asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_element form_input" MaxLength="40"></asp:TextBox>&nbsp;&nbsp;
                          <asp:Label ID="Label3" Text="<%$Resources:Resource, Members %>" runat="server" CssClass="form_element"></asp:Label>
                        <asp:DropDownList ID="ddlMember" runat="server" CssClass="form_element form_input">
                            <asp:ListItem Text="<%$Resources:Resource, CurrentMember %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, PastMember %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AllMember %>" Value="3"></asp:ListItem>

                        </asp:DropDownList>

                        <asp:Button ID="btnApply" CssClass="form_element form_search_button" Text="<%$Resources:Resource, Search %>" runat="server" OnClick="btnApply_Click" />&nbsp;
                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="form_element form_search_button" Text="<%$Resources:Resource, Exporttoexcel %>" OnClick="btnExportToExcel_Click" />&nbsp;
                        <asp:LinkButton ID="lnkBack" CssClass="form_element" runat="server" OnClick="lnkBack_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
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
                            <th width="10%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, MemberNo %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="Label18" Text="<%$Resources:Resource, Mobile %>" runat="server"></asp:Label></th>
                            <th width="25%">&nbsp;</th>
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
        var m_SendInvite = '<%=  Resources.Resource.SendInvite %>';
        var m_SetasAdmin = '<%=  Resources.Resource.SetasAdmin %>';
        var m_ExitasAdmin = '<%=  Resources.Resource.ExitasAdmin %>';

        $(document).ready(function () {

            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetMemberListCount", filter);
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
            getdata3("GetMemberList", pageIndex, filter, '', getDetails_Success);
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
                    var id = record.find("securedid").text();

                    var contents = '<tr>';
                    var idVal = record.find("id").text();
                    // var isExistVal = parseInt(record.find("isExist").text());
                    var isInviteSendChk = parseInt(record.find("isinvitesend").text());
                    var isAdminChk = parseInt(record.find("isadmin").text());
                    var isExitDt = record.find("exitdate").text();

                    var sendinvitetxt = '';
                    if (isInviteSendChk != 1) {
                        var email_Id = idVal + "*" + record.find("email").text();
                        sendinvitetxt = "<a onclick='sendInvite(\"" + email_Id + "\")' href='javascript:void(0);' class='btn btn_edit'>" + m_SendInvite +"</a>";
                    }

                    contents += '<td>' + record.find("memberno").text() + '</td>';
                    contents += '<td>' + record.find("fname").text() + " " + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("mobile").text() + '</td>';
                    if (isExitDt != '' && isExitDt != null) {
                        contents += "<td>&nbsp;</td>";
                    }
                    else {
                        if (isAdminChk == 1)
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='editmember.aspx?" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a onclick='AdminProcess(\"" + idVal + "\",0)' href='javascript:void(0);' class='btn btn_edit'>" + m_ExitasAdmin +"</a>" + sendinvitetxt + "</div></td>";
                        else
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='editmember.aspx?" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a onclick='AdminProcess(\"" + idVal + "\",1)' href='javascript:void(0);' class='btn btn_edit'>" + m_SetasAdmin +"</a>" + sendinvitetxt + "</div></td>";
                    }
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

        function sendInvite(email) {
            __doPostBack('invite', email);
        }

        function AdminProcess(value, type) {
            if (type == 0)
                __doPostBack("delete", value);
            else if (type == 1)
                __doPostBack("add", value);
        }



    </script>
</asp:Content>
