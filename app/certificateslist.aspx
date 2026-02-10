<%@ Page Title="Certificates List" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="certificateslist.aspx.cs" Inherits="Breederapp.Certificateslist" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, CertificateList %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, AddCertificate %>" runat="server"></asp:Label>
                </asp:LinkButton>
            </div>
        </div>
        <br />
        <asp:Panel ID="panelWarning" runat="server" Visible="false">
            <div class="alert alert-danger" role="alert">
                <asp:Label ID="lblWarning" runat="server">
                    <asp:Label ID="Label5" Text="<%$Resources:Resource, Youmustprovidefollowing %>" runat="server"></asp:Label>
                </asp:Label>
                <asp:Label ID="lblMandatoryCertificateNames" runat="server"></asp:Label>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="100" placeholder="<%$Resources:Resource, Name %>"></asp:TextBox>&nbsp;
                         <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                             <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                         </asp:LinkButton>
                    </div>
                </div>
                <input type="hidden" id="hdsort" runat="server" />
                <table class="table datatable">
                    <thead>
                        <th width="35%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, CertificateName %>" runat="server"></asp:Label></th>
                        <th width="10%">
                            <asp:Label ID="Label16" Text="<%$Resources:Resource, StartDate %>" runat="server"></asp:Label></th>
                        <th width="10%">
                            <asp:Label ID="Label18" Text="<%$Resources:Resource, ExpiryDate %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, FileName %>" runat="server"></asp:Label></th>
                        <th width="5%">&nbsp;</th>
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
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetAnimalCertificateCount", filter);
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
            getdata3("GetAnimalCertificateDetails", pageIndex, filter, '', getDetails_Success);
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
                    contents += '<td>' + record.find("certificate_name").text() + ' - ' + record.find("certificate_typename").text() + '</td>';
                    contents += '<td>' + record.find("start_date").text() + '</td>';
                    contents += '<td>' + record.find("end_date").text() + '</td>';

                    var status = '';
                    switch (record.find("status").text()) {
                        case '-1':
                            status = '<%=  Resources.Resource.Rejected %>' + '<div style="opacity:0.8;font-weight: 100; font-size: 13px;margin-top:3px;">[' + record.find("comments").text() + ']<div>';
                            break;

                        case '0':
                            status = '<%=  Resources.Resource.AwaitingApproval %>';
                            break;

                        case '1':
                            status = '<%=  Resources.Resource.Approved %>';
                            break;
                    }
                    contents += '<td>' + status + '</td>';

                    var f = record.find("certificate_file").text();

                    contents += '<td><a href="../app/viewdocument.aspx?file=' + f + '" target="_blank" class="file">' + f.substring(f.indexOf("_") + 1) + '</a></td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteCertificates", id);
            process(1);
        }
    </script>
</asp:Content>
