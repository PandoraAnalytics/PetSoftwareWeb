<%@ Page Title="Ticket History" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="tickethistory.aspx.cs" Inherits="Breederapp.tickethistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/bootstrap-multiselect.css" rel="stylesheet" />
    <style>
        .fa-exclamation {
            color: red;
        }

        .flags_bug {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #FF0000;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .flags_voneeded {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #90EE90;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .flags_feature {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #FFFF00;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .rule {
            color: #777;
        }

            .rule.active {
                color: #000 !important;
                font-weight: 600;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, TicketHistory %>" runat="server" CssClass="error_class"></asp:Label></h5>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="hidden" id="hidfilter" runat="server" />
                <input type="hidden" id="hdsort" runat="server" />
                <div class="filter_section">
                    <i class="fa-solid fa-filter"></i>&nbsp;
                          <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;-&nbsp;

                           <asp:Label ID="lblDate" runat="server" Text="<%$Resources:Resource, TicketNo %>" CssClass="form_element"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtTicketNo" runat="server" CssClass="form_element form_input" MaxLength="7"></asp:TextBox>&nbsp;

                        <asp:Label ID="lblKeywords" Text="<%$Resources:Resource, Keywords %>" runat="server"></asp:Label>&nbsp;
                         <asp:TextBox ID="txtSearch" runat="server" CssClass="form_element form_input" MaxLength="100"></asp:TextBox>&nbsp;

                    <asp:Label ID="Label2" Text="<%$Resources:Resource, Application %>" runat="server"></asp:Label>&nbsp;
                        <asp:DropDownList ID="ddlApplication" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                        </asp:DropDownList>&nbsp;
                      <br />
                    <br />
                    <asp:Label ID="Label7" Text="<%$Resources:Resource, Bug %>" runat="server"></asp:Label>&nbsp;
                        <asp:DropDownList ID="ddlBug" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>&nbsp;

                    <asp:Label ID="Label1" Text="<%$Resources:Resource, VONeeded %>" runat="server"></asp:Label>
                    <asp:DropDownList ID="ddlVOneeded" runat="server" CssClass="form_input">
                        <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                    </asp:DropDownList>&nbsp;

                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    &nbsp;
                </div>
            </div>
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="form_element form_search_button text-end" Text="<%$Resources:Resource, Exporttoexcel %>" OnClick="btnExcel_Click" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable sorttable">
                        <thead>
                            <th width="6%" sort-column-index="1">
                                <asp:Label ID="lblNo_" Text="<%$Resources:Resource, TicketNo %>" runat="server"></asp:Label></th>
                            <th width="24%" sort-column-index="2">
                                <asp:Label ID="lblHeader" Text="<%$Resources:Resource, Header %>" runat="server"></asp:Label></th>
                            <th width="10%" sort-column-index="3">
                                <asp:Label ID="lblApplication1" Text="<%$Resources:Resource, Application %>" runat="server"></asp:Label></th>
                            <th width="10%" sort-column-index="9">
                                <asp:Label ID="lblEDT" Text="<%$Resources:Resource, EDT %>" runat="server"></asp:Label></th>
                            <th width="6" sort-column-index="10">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, VONeeded %>" runat="server"></asp:Label></th>
                            <th width="6%" sort-column-index="5">
                                <asp:Label ID="lblPriority" Text="<%$Resources:Resource, Priority %>" runat="server"></asp:Label></th>
                            <th width="6%" sort-column-index="6">
                                <asp:Label ID="lblStatus1" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label></th>
                            <th width="13%" sort-column-index="7">
                                <asp:Label ID="lblCreated" Text="<%$Resources:Resource, Created %>" runat="server"></asp:Label></th>
                            <th width="13%" sort-column-index="8">
                                <asp:Label ID="lblUpdated" Text="<%$Resources:Resource, Updated %>" runat="server"></asp:Label></th>
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
    </div>
    <%--  <script src="js/data.js?123"></script>--%>
    <script src="js/data.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script src="js/bootstrap-multiselect.js"></script>
    <script>        
        var filter = '';
        var totalcount = 0;
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        var t_bug = '<%=  Resources.Resource.Bug %>';
        var t_feature = '<%=  Resources.Resource.Feature %>';

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetTicketsHistoryCount", filter);
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

            var order = $('#ContentPlaceHolder1_hdsort').val();
            if (order.length > 0) {
                var tokens = order.split('_');
                var $col = $('[sort-column-index="' + tokens[0] + '"]');
                if ($col.length > 0) {
                    $('.fa-long-arrow-up').remove();
                    $('.fa-long-arrow-down').remove();

                    var thdata = $col.html();
                    if (tokens[1] == '1') {
                        $col.html(thdata + ' <i class="fa fa-long-arrow-up"></i>');
                    }
                    else if (tokens[1] == '0') {
                        $col.html(thdata + ' <i class="fa fa-long-arrow-down"></i>');
                    }
                }
            }
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            window.scrollTo(0, 0);
            var xmldata = getdata("GetTicketsHistory", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("ticket_no").text() + '</td>';
                    var header = record.find("header").text();
                    if (parseInt(record.find("commentscount").text()) > 0) header += '&nbsp;&nbsp;<i class="fa fa-comments" aria-hidden="true"></i>';
                    if (parseInt(record.find("doccount").text()) > 0) header += '&nbsp;&nbsp;<i class="fa fa-paperclip" aria-hidden="true"></i>';

                    var flags = '';
                    if (parseInt(record.find("isbug").text()) > 0) flags = "<span class='flags_bug'>" + t_bug + "</span>";
                    else {
                        flags = "<span class='flags_feature'>" + t_feature + "</span>";
                        if (parseInt(record.find("voneeded").text()) == 1) flags += "&nbsp;&nbsp;<span class='flags_voneeded'>Vo Needed</span>";
                    }

                    contents += '<td>' + header + '<div style="margin-top:5px;">' + flags + '</div></td>';
                    contents += '<td>' + record.find("appname").text() + '</td>';
                    var cd = (record.find("processed_completiondate").text().length > 0) ? '<div style="font-size:12px;">(CD:' + record.find("processed_completiondate").text() + ')</div>' : '';
                    contents += '<td>' + record.find("etd").text() + cd + '</td>';
                    var voneeded = '';
                    switch (record.find("voneeded").text()) {
                        default:
                            voneeded = '-';
                            break;
                        case '0':
                            voneeded = 'No';
                            break;
                        case '1':
                            voneeded = 'Yes';
                            break;
                    }
                    contents += '<td>' + voneeded + '</td>';
                    contents += '<td>' + record.find("priorityname").text() + '</td>';
                    contents += '<td>' + record.find("statusname").text() + '</td>';
                    contents += '<td>' + record.find("processed_created").text() + '<br>' + record.find("createdfn").text() + ' ' + record.find("createdln").text() + '</td>';
                    contents += '<td>' + record.find("processed_updatedate").text() + '<br>' + record.find("updatedfn").text() + ' ' + record.find("updatedln").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='ticketview.aspx?" + id + "' class='btn btn_view' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-pencil'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
        }

        //$('.chkbox_dropdown').multiselect({
        //    includeSelectAllOption: true,
        //    numberDisplayed: 0
        //});
    </script>

</asp:Content>
