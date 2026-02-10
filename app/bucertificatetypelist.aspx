<%@ Page Title="Certificate Type List" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucertificatetypelist.aspx.cs" Inherits="Breederapp.bucertificatetypelist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, CertificateTypeList %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <a href="bucertificatetypeadd.aspx" class="add_form_btn btnborder">+&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
            </a>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblName" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label>
                        <asp:TextBox ID="txtType" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, Mandatory %>" runat="server"></asp:Label>
                        <asp:DropDownList ID="ddlMandatory" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClientClick="loadQuestions(); return false;">
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
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lblType" Text="<%$Resources:Resource, Mandatory %>" runat="server"></asp:Label></th>
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

    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            accessFilter();
            process(1);
        });

        function accessFilter() {
            var obj = {};
            obj['type'] = $.trim($('#ContentPlaceHolder1_txtType').val());
            obj['mandatory'] = $.trim($('#ContentPlaceHolder1_ddlMandatory').val());
            obj['companyid'] = '<%= this.CompanyId %>';
            filter = JSON.stringify(obj);
        }

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetAllCertificateType", pageIndex, filter, '', GetAllCertificateType);
        }

        function GetAllCertificateType(data) {
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
                    var type = record.find("ismandatory").text();
                    var typeValue = '';
                    var contents = '<tr>';
                    contents += '<td>' + record.find("type").text() + '</td>';

                    switch (type) {
                        case '0':
                            typeValue = '<%=  Resources.Resource.No %>';
                            break;
                        case '1':
                            typeValue = '<%=  Resources.Resource.Yes %>';
                            break;
                    }

                    contents += '<td> ' + typeValue + '</td>';

                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='bucertificatetypeedit.aspx?" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";

                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("Deletecertificatetype", id);
            process(1);
        }
    </script>
</asp:Content>
