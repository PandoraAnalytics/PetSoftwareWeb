<%@ Page Title="Sponsor List" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="sponsorlist.aspx.cs" Inherits="Breederapp.sponsorlist" %>

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
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Sponsorlist %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;<asp:Label ID="lblEventNM" Text="" runat="server" CssClass="h6"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, Addsponsor %>" runat="server"></asp:Label>
                </asp:LinkButton>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:TextBox ID="txtSponsorName" runat="server" CssClass="form_element" MaxLength="100"></asp:TextBox>&nbsp;
                         <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                             <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                         </asp:LinkButton>&nbsp;
                <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
                    </div>
                </div>
                <input type="hidden" id="hdsort" runat="server" />
                <table class="table datatable">
                    <thead>
                        <th width="15%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                        <th width="30%">
                            <asp:Label ID="Label16" Text="<%$Resources:Resource, Description %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label33" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label></th>
                        <th width="25%">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, FileName %>" runat="server"></asp:Label></th>
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
        var filter = '';
        var eventId = '<%= this.EventId %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetEventSponsorCount", filter);
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
            getdata3("GetEventSponsorDetails", pageIndex, filter, '', getDetails_Success);
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
                    contents += '<td>' + record.find("name").text() + '</td>';
                    contents += '<td style="word-break: break-all;">' + record.find("description").text() + '</td>';

                    var type = "";
                    switch (record.find("type").text()) {
                        case "1":
                            type = "<%=  Resources.Resource.Gold %>";
                            break;

                        case "2":
                            type = "<%=  Resources.Resource.Silver %>";
                            break;

                        case "3":
                            type = "<%=  Resources.Resource.Bronze %>";
                            break;

                    }
                    contents += '<td>' + type + '</td>';
                    if (record.find("sponsor_file").text().length > 0) {
                        var f = record.find("sponsor_file").text();
                        contents += '<td><a href="../app/viewdocument.aspx?file=' + f + '" target="_blank" class="file">' + f.substring(f.indexOf("_") + 1) + '</a></td>';                       
                    }
                    else contents += '<td>' + "-" + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='sponsoradd.aspx?eid=" + eventId + "&sid=" + id + "'class='btn btn_edit' title='Edit'><i class='fa fa-pencil'></i></a> <a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";

                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteEventSponsor", id);
            process(1);
        }
    </script>
</asp:Content>
