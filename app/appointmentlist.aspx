<%@ Page Title="Appointment List" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="appointmentlist.aspx.cs" Inherits="Breederapp.appointmentlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_rdbDeleteList tr td {
            padding: 8px;
        }

            #ContentPlaceHolder1_rdbDeleteList tr td label {
                margin-left: 8px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Appointment %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, CreateAppointment %>" runat="server"></asp:Label>
                </asp:LinkButton>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <input type="hidden" id="hdsort" runat="server" />

                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form_input"></asp:DropDownList>&nbsp;
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form_input"></asp:DropDownList>&nbsp;

                        <asp:DropDownList ID="ddlProfession" runat="server" CssClass="form_input" DataTextField="name" DataValueField="id">
                        </asp:DropDownList>&nbsp;

                        <asp:TextBox ID="txtContact" runat="server" CssClass="form_input" placeholder="<%$Resources:Resource, Contacts %>"></asp:TextBox>&nbsp;
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form_input" placeholder="<%$Resources:Resource, Description %>"></asp:TextBox>&nbsp;

                        <asp:LinkButton ID="btnApply" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <table class="table datatable">
                    <thead>
                        <th width="20%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                        <th width="35%">
                            <asp:Label ID="Label18" Text="<%$Resources:Resource, ContactDetails %>" runat="server"></asp:Label></th>
                        <th width="35%">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, ContactNumber %>" runat="server"></asp:Label></th>
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

    <div id="modal_event" class="modal fade">
        <input type="hidden" id="hddateid" runat="server" />
        <div class="modal-dialog" style="max-width: 40%;">
            <div class="modal-content" style="margin-top: 7%;">
                <div class="modal-body">
                    <div class="login_form">
                        <div class="row form_action">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <asp:RadioButtonList ID="rdbDeleteList" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                                    <asp:ListItem Text="<%$Resources:Resource, Thisoccurrence %>" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Thisandfollowingoccurrences %>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Alloccurrences %>" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="form_button" OnClick="btnDelete_Click" />&nbsp;&nbsp;&nbsp;
                            <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Close %>" runat="server"></asp:Label></a>
                            </div>
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
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetAnimalAppointmentCount", filter);
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
            getdata3("GetAnimalAppointmentDetails", pageIndex, filter, '', getAppointment_Success);
        }

        function getAppointment_Success(data) {
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
                    contents += '<td>' + record.find("procesed_datetime").text() + '</td>';
                    contents += '<td>' + record.find("profession_name").text() + ' - ' + record.find("contact_name").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + ' - ' + record.find("phone").text() + '</td>';

                    var actioncontents = "<a href='appointmentview.aspx?id=" + id + "' target='_blank' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a>";
                    if (parseInt(record.find("ispastappointment").text()) == 0) {
                        actioncontents += "<a href='editappointment.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' tilte='Edit'><i class='fa fa-pencil'></i></a>";
                    }
                    actioncontents += "<a href='javascript:void(0);'onclick='showdeletemodal(\"" + id + "\");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a>";
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'>" + actioncontents + "</div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function showdeletemodal(dateid) {
            $('#ContentPlaceHolder1_hddateid').val(dateid);
            $('#modal_event').modal('show');
            return false;
        }

    </script>
</asp:Content>
