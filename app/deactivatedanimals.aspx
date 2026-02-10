<%@ Page Title="Deactivated Animals" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="deactivatedanimals.aspx.cs" Inherits="Breederapp.deactivatedanimals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .profilepic {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            border: 1px solid #ddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, DeactivatedAnimals %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="hidden" id="hdfilter" runat="server" />
                <input type="hidden" id="hdsort" runat="server" />
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="10%">&nbsp;</th>
                            <th width="30%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, TypeofBreed %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, Reason %>" runat="server"></asp:Label></th>
                            <th width="10%">&nbsp;</th>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="dt_footer">
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
            var totalcount = gettotalcount("GetMyAnimalsCount", filter);
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
            getdata3("GetMyAnimals", pageIndex, filter, '', getAnimalDetails_Success);
        }

        function getAnimalDetails_Success(data) {
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
                    if (record.find("profilepic_file").text().length > 0) contents += '<td><img src="docs/' + record.find("profilepic_file").text() + '" alt="p" class="profilepic" /></td>';
                    else contents += '<td><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/></td>';

                    contents += '<td>' + record.find("name").text() + '</td>';
                    contents += '<td>' + record.find("typename").text() + '</td>';
                    contents += '<td>' + record.find("reasonofdelete").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='active_account(\"" + id + "\")' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Activate'><i class='fa-solid fa-check-double'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function active_account(id) {
            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/ActivateAnimal",
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
