<%@ Page Title="Service List" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="serviceslist.aspx.cs" Inherits="Breederapp.serviceslist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .greentick {
            color: #0bb10b;
        }

        .redcross {
            color: #ff4040;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="Service List" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="Add Service" runat="server"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <asp:Panel ID="panelChecklist" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="card">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <span class="card-title mb-0">
                                <asp:Label ID="Label7" Text="Just finish the checklist before adding your first service. It'll only take a moment!(if it's Already done? Go ahead and add your services!)" CssClass="error_class" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div class="row" style="padding: 10px;">
                            <asp:LinkButton ID="lnkServiceType" runat="server" OnClick="lnkServiceType_Click" CssClass="edit_profile_link">
                                <i class="fa-solid fa-circle-check greentick" id="servicetypeYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="servicetypeNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label20" runat="server" Text="Add Service Type"></asp:Label>
                            </asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton ID="lnkTax" runat="server" OnClick="lnkTax_Click" CssClass="edit_profile_link">
                                <i class="fa-solid fa-circle-check greentick" id="taxYes" runat="server" visible="false"></i><i class="fa-solid fa-circle-xmark redcross" id="taxNo" runat="server" visible="false"></i>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label12" runat="server" Text="Add Tax"></asp:Label>
                            </asp:LinkButton>
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
                <br />
            </div>
        </asp:Panel>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Name"></asp:TextBox>&nbsp;&nbsp;
                         <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" DataTextField="name" DataValueField="id">
                         </asp:DropDownList>&nbsp;&nbsp;
                        <asp:LinkButton ID="btnApply" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
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
                    <table class="table datatable">
                        <thead>
                            <th width="25%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label9" Text="Description" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label16" Text="Actual Cost" runat="server"></asp:Label>
                                (<asp:Label ID="lblCostCurrency" runat="server"></asp:Label>)</th>
                            <th width="20%">
                                <asp:Label ID="Label4" Text="Service Type" runat="server"></asp:Label></th>
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
            var totalcount = gettotalcount("GetServiceCount", filter);
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
            getdata3("GetServicesDetails", pageIndex, filter, '', getServicesDetails_Success);
        }

        function getServicesDetails_Success(data) {
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
                    contents += '<td>' + record.find("name").text() + '</td>';
                    var main_desc = record.find("description").text();
                    if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                    contents += '<td style="word-break:break-all;">' + main_desc + '</td>';
                    /* contents += '<td>' + record.find("description").text() + '</td>';*/
                    contents += '<td>' + record.find("processed_cost").text() + '</td>';
                    contents += '<td>' + record.find("typename").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='serviceview.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteService", id);
            process(1);
        }
    </script>
</asp:Content>
