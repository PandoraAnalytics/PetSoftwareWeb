<%@ Page Title="Accept User" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="acceptassociate.aspx.cs" Inherits="Breederapp.acceptassociate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, ApproveRejectList %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="clearfix"> </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="hidden" id="hdfilter" runat="server" />
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="45%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                                <th width="25%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
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
              <%-- <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click">Back</asp:LinkButton>--%>
                 <asp:LinkButton ID="lnkBack" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="lnkBack_Click"></asp:LinkButton>
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
            var totalcount = gettotalcount("GetAllRequestCount", filter);
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
            getdata3("GetAllRequest", pageIndex, filter, '', GetAllAssociateRequest);
        }

        function GetAllAssociateRequest(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("id").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("username").text() + '</td>';
                    contents += '<td>' + record.find("procesed_datetime").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='AcceptRequest(\"" + id + "\");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Accept' title='Accept'><i class='fa-regular fa-thumbs-up'></i></a><a href='javascript:void(0);' onclick='DeclineRequest(\"" + id + "\");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Decline' title='Decline'><i class='fa-regular fa-thumbs-down'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function AcceptRequest(id) {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/AcceptRequest",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ id: id }),
                success: function (msg) {
                    var u = window.location.href;
                    window.location = u;
                }
            });
        }

        function DeclineRequest(id) {
                $.ajax({
                    type: "POST",
                    url: "Services/getdata.asmx/DeclineRequest",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ id: id }),
                    success: function (msg) {
                        var u = window.location.href;
                        window.location = u;
                    }
                });
            }
    </script>

</asp:Content>
