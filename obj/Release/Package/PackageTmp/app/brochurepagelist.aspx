<%@ Page Title="Brochure Page List" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="brochurepagelist.aspx.cs" Inherits="Breederapp.brochurepagelist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .file, .file:hover {
            border: solid 1px #efefef;
            padding: 3px 8px;
            border-radius: 5px;
            display: inline-block;
            color: #333;
            background: #f8f8f8;
        }

        .error_class {
            display: inline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Pagelist %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;<asp:Label ID="lblBrochureNM" Text="" runat="server"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, Addpages %>" runat="server"></asp:Label>
                </asp:LinkButton>&nbsp;
                <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
            </div>
        </div>
    <br />
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <input type="hidden" id="hdsort" runat="server" />
            <table class="table datatable">
                <thead>
                    <th width="10%">
                        <asp:Label ID="Label5" Text="<%$Resources:Resource, Sequenceno %>" runat="server"></asp:Label></th>
                    <th width="20%">
                        <asp:Label ID="Label1" Text="<%$Resources:Resource, Pagename %>" runat="server"></asp:Label></th>
                    <th width="20%">
                        <asp:Label ID="Label3" Text="<%$Resources:Resource, Brochurename %>" runat="server"></asp:Label></th>
                    <th width="20%">
                        <asp:Label ID="Label4" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                    <th width="20%">
                        <asp:Label ID="Label16" Text="<%$Resources:Resource, Contenttype %>" runat="server"></asp:Label></th>
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

    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '<%= this.BrochureId %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {

            var totalcount = gettotalcount("GetBrochurePageCount", filter);
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
            getdata3("GetBrochurePageDetails", pageIndex, filter, '', getDetails_Success);
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
                    contents += '<td>' + record.find("sequence_no").text() + '</td>';
                    contents += '<td>' + record.find("pagename").text() + '</td>';
                    contents += '<td>' + record.find("brochurename").text() + '</td>';
                    contents += '<td>' + record.find("procesed_date").text() + '</td>';
                    var cType = record.find("content_type").text();
                    var typeName = '';
                    switch (cType) {
                        case '1':
                            typeName = '<%=  Resources.Resource.TextEditor %>'; 
                            break

                        case '2':
                            typeName = '<%=  Resources.Resource.AnimalList %>'; 
                            break

                        case '3':
                            typeName = '<%=  Resources.Resource.OwnerList %>';
                            break

                        case '4':
                            typeName = '<%=  Resources.Resource.Sponsorlist %>';
                            break
                    }
                    contents += '<td>' + typeName + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='brochurepageadd.aspx?bid=" + '<%= this.BrochureId %>' + "&bpid=" + id + "'class='btn btn_edit' title='Edit'><i class='fa fa-pencil'></i></a> <a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";

                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteBrochurePage", id);
            process(1);
        }
    </script>
</asp:Content>
