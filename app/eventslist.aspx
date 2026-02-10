<%@ Page Title="Event List" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="eventslist.aspx.cs" Inherits="Breederapp.eventslist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .event_img {
            width: auto;
            height: 50px;
        }

        .eventlink, .eventlink:hover {
            color: inherit;
            font-weight: 600;
            font-size: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, EventList %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <a href="eventsadd.aspx" class="add_form_btn btnborder">
                <i class="fa-solid fa-user-plus" aria-hidden="true"></i>
                &nbsp;<asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
            </a>
        </div>
        <div class="clearfix"></div>
        <br />

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />                    
                    <input type="hidden" id="cplist" runat="server" />
                    <input type="hidden" id="Hidden1" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:TextBox ID="txtTitle" runat="server" CssClass="form_input"></asp:TextBox>&nbsp;
                        <asp:DropDownList ID="ddlPastEvents" runat="server" CssClass="form_input"></asp:DropDownList>
                        <asp:LinkButton ID="LinkButton1" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                    <table class="table datatable">
                        <thead>
                            <th width="20%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Image %>" runat="server"></asp:Label></th>
                            <th width="45%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, Event %>" runat="server"></asp:Label>
                            </th>
                            <th width="10%">&nbsp;</th>
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
    </div>
    <div id="modal_deleteReason" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label3" Text="<%$Resources:Resource, DeleteCancellationReason %>" CssClass="error_class" runat="server"></asp:Label></h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Reason %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtReason" CssClass="form_input" runat="server" MaxLength="255" data-validate="maxlength-255" Rows="5" TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:Resource, Submit %>" CssClass="form_button" OnClientClick="return SubmitClick();" OnClick="btnSubmit_Click" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label8" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js?10"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var e_Managesponsor = '<%=  Resources.Resource.Managesponsor %>';
        var e_Managebrochures = '<%=  Resources.Resource.Managebrochures %>';
        var e_Viewevent = '<%=  Resources.Resource.Viewevent %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
			 var returnValue = $("#ContentPlaceHolder1_Hidden1").val();
             if (returnValue) {
                 alert(returnValue);
             }
			 
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetEventCount", filter);
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
            getdata3("GetEventDetails", pageIndex, filter, '', getAnimalNotesDetails_Success);
        }

        function getAnimalNotesDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var id1 = record.find("id").text();
                    var contents = '<tr>';
                    if (record.find("banner_image").text().length > 0) {
                        contents += '<td><img src="../images/image_loading.gif" class="lazy-img event_img" data-src="' + record.find(" banner_image").text() + '" class="equipment_img alt="x"></td>';
                    }
                    else {
                        contents += '<td><img src="../app/images/default_banner_image.png" class="event_img" alt="x"></td>';
                    }
                    contents += '<td><a href="eventview.aspx?id=' + id + '" class="eventlink" style="">' + record.find("title").text() + '<div style="opacity:0.8;font-weight: 100; font-size: 13px;margin-top:3px;">' + record.find("procesed_datetime").text() + '</div></td>';

                    contents += '<td><i class="fa-solid fa-heart" style="color:#d64651;"></i>&nbsp;&nbsp;' + record.find("registrationcount").text() + '</td>';

                    var action = '';
                    action += '<a href="brochurelist.aspx?eid=' + id + '" class="btn btn_edit">' + e_Managebrochures + '</a>';
                    action += '<a href="sponsorlist.aspx?eid=' + id + '" class="btn btn_edit">' + e_Managesponsor + '</a>';
                    action += '<a href="eventview.aspx?id=' + id + '" class="btn btn_edit">' + e_Viewevent + '</a>';
                    action += '<a href="eventedit.aspx?id=' + id + '" class="btn btn_edit" title="Edit"><i class="fa fa-pencil"></i></a>';

                    // if (parseInt(record.find("myregistrationcount").text()) > 0) action += '<a href="javascript:void(0);" class="btn btn_edit"><i class="fa-solid fa-heart"></i></a>';
                    // else action += '<a href="javascript:void(0);" class="btn btn_edit"><i class="fa-regular fa-heart"></i></a>';

                    action += '<a href="eventregistrationlist.aspx?id=' + id + '" class="btn btn_edit" title="Registration List"><i class="fa-solid fa-users"></i></a>';
                    action += '<a href="eventviewcustomfields.aspx?id=' + id + '" class="btn btn_edit" title="Custom Fields"><i class="fa-solid fa-circle-question"></i></a>';
                    action += '<a href="javascript:void(0);" onclick="deleteEvent(' + id1 + ');" class="btn btn_delete" title="Delete"><i class="fa fa-trash"></i></a>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'>" + action + "</div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
            loadLazyImages();
        }

        function deleteEvent(id1) {
            $("#ContentPlaceHolder1_cplist").val(id1);
            $("#modal_deleteReason").modal('show');           
        }
        function SubmitClick() {
            var reason = $("#ContentPlaceHolder1_txtReason").val().trim();
            confirm_msg = '<%=  Resources.Resource.Cancellationreasoncannotbeempty %>';
            if (reason.length == "" && reason.length <= 0) {
                alert(confirm_msg);
                return false;
            }
        }
    </script>

</asp:Content>
