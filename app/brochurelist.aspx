<%@ Page Title="Brochure List" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="brochurelist.aspx.cs" Inherits="Breederapp.brochurelist" %>

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
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Brochurelist %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;<asp:Label ID="lblEventNM" Text="" runat="server" CssClass="h6"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, Addbrochure %>" runat="server"></asp:Label>
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
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_element" MaxLength="100"></asp:TextBox>&nbsp;
                         <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                             <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                         </asp:LinkButton>&nbsp;<asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
                    </div>
                </div>
                <input type="hidden" id="hdsort" runat="server" />
                <table class="table datatable">
                    <thead>
                        <th width="20%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label16" Text="<%$Resources:Resource, Description %>" runat="server"></asp:Label></th>
                         <th width="10%">
                            <asp:Label ID="Label5" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label></th>
                        <th width="25%">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, FileName %>" runat="server"></asp:Label></th>
                        <th width="25%">&nbsp;</th>

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

        var b_Managepages = '<%=  Resources.Resource.Managepages %>';
        var b_PDFReport = '<%=  Resources.Resource.BrochurePreview %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        var b_Indraft = '<%=  Resources.Resource.InDraft %>';
        var b_Publish = '<%=  Resources.Resource.Publish %>';

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetEventBrochureCount", filter);
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
            getdata3("GetEventBrochureDetails", pageIndex, filter, '', getDetails_Success);
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
                    var publishButton = "";
                    var status = "";
                    switch (record.find("status").text())
                    {
                        case '1':
                            status = b_Indraft;
                            publishButton = "<a href='javascript:void(0);' onclick='publishEventBrochure(\"" + id + "\",2)' class='btn btn_edit'>" + b_Publish +"</a>";
                                  break;
                        case '2':
                            status = b_Publish;
                            publishButton = "<a href='javascript:void(0);' onclick='publishEventBrochure(\"" + id + "\",1)' class='btn btn_edit'>" + b_Indraft +"</a>";
                                  break;
                    }
                    contents += '<td>' + status + '</td>';
                    var f = record.find("brochure_file").text();
                    if (f.length > 0)
                        contents += '<td><a href="../app/viewdocument.aspx?file=' + f + '" target="_blank" class="file">' + f.substring(f.indexOf("_") + 1) + '</a></td>';
                    else
                        contents += '<td>Image not available.</td>';

                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='brochureadd.aspx?eid=" + eventId + "&bid=" + id + "'class='btn btn_edit' title='Edit'><i class='fa fa-pencil'></i></a> <a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' tilte='Delete'><i class='fa-regular fa-trash-can'></i></a><a href='brochurepagelist.aspx?bid=" + id + "' class='btn btn_edit'>" + b_Managepages + "</a>" + publishButton +"<a href='exportbrochure.aspx?bid=" + id + "' target='_blank' class='btn btn_edit'>" + b_PDFReport + "</a></div></td>";



                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteEventBrochure", id);
            process(1);
        }

        function publishEventBrochure(id, statusVal) {
            var confirm_msg = '';
            if (statusVal == 2) {
                confirm_msg = '<%=  Resources.Resource.PublishEventBrochureConfirmationMessage %>';
            }
            else if (statusVal == 1){
                confirm_msg = '<%=  Resources.Resource.InDraftEventBrochureConfirmationMessage %>';
            }        
            var answer = confirm(confirm_msg);
            if (answer) {
                $.ajax({
                    type: "POST",
                    url: "Services/getdata.asmx/PublishEventBrochure",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ id: id, newstatus: statusVal }),
                    success: function (msg) {
                        process(1);
                    }
                });
            }
        }
    </script>
</asp:Content>
