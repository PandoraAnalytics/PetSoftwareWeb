<%@ Page Title="CheckPoint" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="checkpoint.aspx.cs" Inherits="Breederapp.checkpoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, CheckPoint %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <a href="checkpointmanage.aspx" class="edit_profile_link">+&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
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
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_input" placeholder="CheckPoint"></asp:TextBox>&nbsp;                    
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
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Title %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lblType" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label></th>
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
        //$(document).ready(function () {
        //    getCheckpoint();
        //});

        //function getCheckpoint() {
        //    var obj = {};
        //    obj['ischecklistype'] = 1;
        //    obj['associationbreedertype'] = '';
        //    obj['title'] = $('#ContentPlaceHolder1_txtName').val();
        //    filter = JSON.stringify(obj);
        //    $('.datatable tbody').html('');
        //    getdata3("GetAllCustomFields", 1, filter, '', GetAllCustomFields);
        //}
        $(document).ready(function () {
            getCheckpoint();
        });


        function accessFilter() {
            var obj = {};
            obj['ischecklistype'] = 1;
            obj['associationbreedertype'] = '';
            obj['title'] = $('#ContentPlaceHolder1_txtName').val();
            filter = JSON.stringify(obj);
        }

        function getCheckpoint() {
            accessFilter();

            var totalcount = gettotalcount("GetAllCustomFieldsCount", filter);
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



            //$('.datatable tbody').html('');
            //getdata3("GetAllCustomFields", 1, filter, '', GetAllCustomFields);
        }

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetAllCustomFields", pageIndex, filter, '', GetAllCustomFields);
        }

        /*  below original code*/

        function GetAllCustomFields(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var typeValue = '';

                    switch (record.find("type").text()) {
                        case '1':
                            typeValue = 'One Line';
                            break;
                        case '2':
                            typeValue = 'Paragraph'
                            break;
                        case '3':
                            typeValue = 'List'
                            break;
                        case '4':
                            typeValue = 'Single-Select'
                            break;
                        case '5':
                            typeValue = 'Multi-Select'
                            break;
                        case '6':
                            typeValue = 'File Upload'
                            break;
                        case '7':
                            typeValue = 'Range'
                            break;
                        case '8':
                            typeValue = 'Matrix'
                            break;
                        case '9':
                            typeValue = 'Date'
                            break;
                        case '10':
                            typeValue = 'Time'
                            break;
                    }
                    var contents = '<tr>';
                    contents += '<td>' + record.find("title").text() + '</td>';
                    contents += '<td>' + typeValue + '</td>';
                    /*  contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checkpointmanage.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";*/
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checkpointedit.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' tilte='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteCustomFields", id);
            var u = window.location.href;
            window.location = u;
        }
    </script>
</asp:Content>
