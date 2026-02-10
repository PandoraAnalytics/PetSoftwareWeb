<%@ Page Title="Checklist" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="checklist.aspx.cs" Inherits="Breederapp.checklist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Checklist %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <a href="checklistmanage.aspx" class="add_form_btn btnborder">+&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
            </a>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_input" placeholder="<%$Resources:Resource, Checklist %>"></asp:TextBox>&nbsp;
                        <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="name" DataValueField="id" CssClass="form_input"></asp:DropDownList>&nbsp;
                        <asp:LinkButton ID="btnApply1" CssClass="form_element form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
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
                            <th width="60%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lblType" Text="<%$Resources:Resource, Category %>" runat="server"></asp:Label></th>
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
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetChecklistCount", filter);
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
            getdata3("GetAllChecklists", pageIndex, filter, '', GetAllChecklist_Success);
        }

        function GetAllChecklist_Success(data) {
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
                    contents += '<td>' + record.find("categoryname").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistmanage.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='checklistassingotherfields.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Other Fields' title='Other Fields'><i class='fa fa-circle-question'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteChecklist", id);
            var u = window.location.href;
            window.location = u;
        }
    </script>
</asp:Content>

