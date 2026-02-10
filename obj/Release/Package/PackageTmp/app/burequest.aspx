<%@ Page Title="Business User Request" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="burequest.aspx.cs" Inherits="Breederapp.burequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        legend {
            display: block;
            width: 100%;
            padding: 0;
            margin-bottom: 20px;
            font-size: 17px;
            line-height: inherit;
            color: #333;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
        }

        .photos {
            /*margin-bottom: 25px;*/
        }

            .photos li {
                display: inline-block;
                padding: 10px;
                position: relative;
            }

                .photos li .photowrapper {
                    display: inline-block;
                    /* min-width: 120px; */
                    border: solid 1px #ddd;
                    text-align: center;
                    padding: 5px 10px;
                    color: #333;
                    border-radius: 5px;
                    height: 100px;
                    width: auto;
                }

                .photos li .photo {
                    width: 130px;
                    background-repeat: no-repeat;
                    background-size: contain;
                    background-repeat: no-repeat;
                    background-position: center;
                    height: auto !important;
                    height: 100%;
                    min-height: 100%;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="lblUserRequest" Text="<%$Resources:Resource, BusinessUserRequest %>" runat="server" CssClass="error_class"></asp:Label>
        </h6>
        <input type="hidden" id="hdfilter" runat="server" />
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblMessage" runat="server" Style="padding-left: 10px;"></asp:Label>
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdsort" runat="server" />
                    <input type="hidden" id="cplist" runat="server" />
                    <input type="hidden" id="hid" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                          <asp:Label ID="lblFilter" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>
                        &nbsp;
                            <asp:Label ID="lblFilterName" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                        &nbsp;
                         <asp:Label ID="lblFilterCompany" Text="<%$Resources:Resource, Company %>" runat="server"></asp:Label>
                        &nbsp;
                         <asp:TextBox ID="txtComapny" runat="server" CssClass="form_element form_input" MaxLength="100"></asp:TextBox>
                        &nbsp;
                         <asp:Label ID="lblStatus" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label>
                        &nbsp;
                       <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_input">
                           <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                           <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                           <asp:ListItem Text="Approved" Value="3"></asp:ListItem>
                           <asp:ListItem Text="<%$Resources:Resource, SelectAll %>" Value=""></asp:ListItem>
                       </asp:DropDownList>
                        &nbsp;
                        <asp:Button ID="btnApply" runat="server" Text="<%$Resources:Resource, Apply %>" CssClass="form_button" OnClick="btnApply_Click" />

                    </div>
                    <div class="table-responsive dt_container">
                        <table class="table datatable sorttable" id="userTable">
                            <thead>
                                <th width="20%" sort-column-index="1">
                                    <asp:Label ID="lblName" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label>
                                </th>
                                <th width="20%" sort-column-index="2">
                                    <asp:Label ID="lblEmail" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label>
                                </th>
                                <th width="20%" sort-column-index="3">
                                    <asp:Label ID="lblCompany" Text="<%$Resources:Resource, Company %>" runat="server"></asp:Label>
                                </th>
                                <th width="20%" sort-column-index="5">
                                    <asp:Label ID="lblType" Text="<%$Resources:Resource, Status %>" runat="server"></asp:Label>
                                </th>
                                <th width="20%">&nbsp;</th>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div class="dt_footer">
                        <div class="pagination float-end" id="userTablepagination">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_notapprove" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="lblReject" Text="<%$Resources:Resource, Reject %>" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="lblComment" Text="<%$Resources:Resource, Comment %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtNotAppRemark" CssClass="form_input" runat="server" MaxLength="255" data-validate="maxlength-255" Rows="5" TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="Button3" runat="server" Text="<%$Resources:Resource, Reject %>" CssClass="form_button" OnClick="btnReject_Click" />
                                &nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="lblClose" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_approve" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="lblApprove" Text="<%$Resources:Resource, Approve %>" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="lblApproveText" Text="<%$Resources:Resource, Approverequestmessage %>" runat="server" CssClass="form_label"></asp:Label>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="Button1" runat="server" Text="<%$Resources:Resource, Approve %>" CssClass="form_button" OnClick="btnApproval_Click" />
                                &nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="lblClose2" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_view" class="modal fade">
        <div class="modal-dialog" style="width: 100%">
            <div class="modal-content" style="margin-top: 10%; width: 100%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label1" Text="User Details" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                    <br />
                    <input type="hidden" id="BUId" runat="server" />
                    <div class="login_form">
                        <fieldset id="fs_Description" runat="server">
                            <legend>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, PersonalInfo %>"></asp:Label>
                            </legend>
                            <div class="row form_row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label5" Text="<%$Resources:Resource, FirstName %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="lblName2" Text="<%$Resources:Resource, LastName %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblLastName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row form_row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label6" Text="<%$Resources:Resource, EmailAddress %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label8" Text="<%$Resources:Resource, Phone %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                </div>
                            </div>
                        </fieldset>

                        <fieldset id="fs_CompanyInfo" runat="server">
                            <legend>
                                <asp:Label ID="lblCompanyInfo" runat="server" Text="<%$Resources:Resource, CompanyInfo %>"></asp:Label>
                            </legend>

                            <div class="row form_row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label10" Text="<%$Resources:Resource, Company %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label11" Text="<%$Resources:Resource, CompanyShortName %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblShortName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row form_row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label4" Text="<%$Resources:Resource, Website %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblWebsite" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label12" Text="<%$Resources:Resource, BusinessType %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblBusinessType" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="row form_row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label13" Text="<%$Resources:Resource, RegistrationNo %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblRegistration" runat="server"></asp:Label>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:Label ID="Label9" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                </div>
                            </div>
                        </fieldset>

                        <fieldset id="fs_Document" runat="server">
                            <legend>
                                <asp:Label ID="Label16" runat="server" Text="Business Documents"></asp:Label>
                            </legend>
                            <div class="table-responsive dt_container">
                                <table class="table datatable" id="docTable">
                                    <thead>
                                        <tr>
                                        </tr>
                                    </thead>
                                    <tbody id="docTableBody">
                                    </tbody>
                                </table>
                                <div class="dt_footer">
                                </div>
                            </div>
                        </fieldset>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;" class="add_form_btn btnborder">
                                    <asp:Label ID="Label3" Text="<%$Resources:Resource, Close %>" runat="server"></asp:Label>
                                </a>
                                &nbsp;&nbsp;  
                                <a href="javascript:void(0);" style="display: inline-block;" class="add_form_btn btnborder" id="linkViewMore" target="_blank">
                                    <asp:Label ID="lbl" Text="View Business Profile" runat="server"></asp:Label>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_rejectReason" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label7" Text="Reject Reason" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label14" Text="<%$Resources:Resource, Comment %>" runat="server" CssClass="form_label">:</asp:Label><br />
                                <div style="border: 1px solid #ccc; padding: 10px; border-radius: 5px; background-color: #f9f9f9; max-height: 200px; overflow-y: auto;">
                                    <asp:Label ID="lblRejectReason" Text="-" runat="server" CssClass="form_label"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;" class="add_form_btn btnborder">
                                    <asp:Label ID="Label15" Text="<%$Resources:Resource, Close %>" runat="server"></asp:Label>
                                </a>
                            </div>
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

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetBusinessUserRequestApproveCount", filter);
            process(1);
            $('#userTablepagination').bootpag({
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

            order = $('#ContentPlaceHolder1_hdsort').val();
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
            $('#userTable tbody').html('');
            window.scrollTo(0, 0);
            var xmldata = getdata("GetBusinessUserRequestApprove", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var secureid = record.find("securedid").text();
                    var id = record.find("id").text();
                    var buid = record.find("buid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("companyname").text() + '</td>';

                    var status = record.find("status").text();
                    switch (status) {
                        case "1"://pending
                            contents += '<td>' + "Pending" + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='viewDetails(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='singleApprove(" + id + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Approve' title='Approve '><i class='fa-regular fa-thumbs-up'></i></a><a href='javascript:void(0);' onclick='singleReject(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Reject' title='Reject'><i class='fa-regular fa-thumbs-down'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + secureid + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                            break;

                        case "2"://rejected                       
                            contents += '<td>' + "Rejected" + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='viewDetails(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='rejectReason(\"" + record.find("reason").text() + "\");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Reason' title='Reject Reason'>Reject Reason</a></div></td>";
                            break;

                        case "3"://approved
                            contents += '<td>' + "Approved" + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='viewDetails(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a></div></td>";
                            break;

                        default:
                            contents += '<td>' + record.find("status").text() + '</td>';
                            contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='viewDetails(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a></div></td>";
                            break;
                    }
                    contents += '</tr>';
                    $('#userTable > tbody:last').append(contents);
                });
            }
            else {
                $('#userTable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteBusinessEnquiry", id);
            process(1);
        }

        function singleApprove(id) {
            $("#ContentPlaceHolder1_cplist").val(id);
            $("#ContentPlaceHolder1_hid").val(id);
            OpenBulkAction('approve');
        }

        function singleReject(id) {
            $("#ContentPlaceHolder1_cplist").val(id);
            $("#ContentPlaceHolder1_hid").val(id);
            OpenBulkAction('notapprove');
        }

        function viewDetails(id) {
            $("#ContentPlaceHolder1_lblFirstName").text('');
            $("#ContentPlaceHolder1_lblLastName").text('');
            $("#ContentPlaceHolder1_lblEmailAddress").text('');
            $("#ContentPlaceHolder1_lblPhone").text('');
            $("#ContentPlaceHolder1_lblAddress").text('');
            $("#ContentPlaceHolder1_lblCompanyName").text('');
            $("#ContentPlaceHolder1_lblShortName").text('');
            $("#ContentPlaceHolder1_lblWebsite").text('');
            $("#ContentPlaceHolder1_lblBusinessType").text('');
            $("#ContentPlaceHolder1_lblRegistration").text('');
            $("#ContentPlaceHolder1_lblCurrentStatus").text('');
            $("#ContentPlaceHolder1_hid").val(id);
            $("#ContentPlaceHolder1_BUId").val('');


            $.ajax({
                type: "POST",
                url: "burequest.aspx/GetBusinessEnquiryDetail",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'enquiryid': id }),
                success: function (msg) {
                    if (msg && msg.d && msg.d.length > 0) {
                        var enquiryobj = JSON.parse(msg.d);
                        $("#ContentPlaceHolder1_lblFirstName").text(enquiryobj.fname);
                        $("#ContentPlaceHolder1_lblLastName").text(enquiryobj.lname);
                        $("#ContentPlaceHolder1_lblEmailAddress").text(enquiryobj.email);
                        $("#ContentPlaceHolder1_lblPhone").text(enquiryobj.phone);
                        $("#ContentPlaceHolder1_lblAddress").text(enquiryobj.address);
                        $("#ContentPlaceHolder1_lblCompanyName").text(enquiryobj.companyname);
                        $("#ContentPlaceHolder1_lblShortName").text(enquiryobj.companyshortname);
                        $("#ContentPlaceHolder1_lblWebsite").text(enquiryobj.website);
                        $("#ContentPlaceHolder1_lblBusinessType").text(enquiryobj.businesstype);
                        $("#ContentPlaceHolder1_lblRegistration").text(enquiryobj.registrationno);
                        $("#ContentPlaceHolder1_BUId").val(enquiryobj.buid);
                        if (enquiryobj.docValue.length > 0) {
                            var arr = enquiryobj.docValue.split(",").map(function (item) {
                                return item.trim();
                            });
                            var tbody = $("#docTableBody");
                            tbody.empty();

                            arr.forEach(function (fileName) {
                                var link = "<a href='../app/viewdocument.aspx?file=" + fileName + "' target='_blank'>" + fileName.substring(fileName.indexOf('_') + 1) + "</a>";//"<a href='docs/" + fileName + "' target='doc'>" + fileName + "</a>";
                                var row = "<tr><td>" + link + "</td></tr>";
                                tbody.append(row);
                            });
                        }
                        if (parseInt(enquiryobj.buid) > 0) {
                            $("#linkViewMore").show();
                            $("#linkViewMore").attr('href', 'buprofileview.aspx?buid=' + enquiryobj.buid);
                        }
                        else $("#linkViewMore").hide();

                        $('#modal_view').modal('show');
                    }
                }
            });
            return false;
        }

        function rejectReason(reason) {
            $("#ContentPlaceHolder1_lblRejectReason").text(reason);
            $('#modal_rejectReason').modal('show');
        }

        function OpenBulkAction(bulkAction) {
            if (bulkAction == "approve") {
                $("#modal_" + bulkAction).modal('show');
            }
            else if (bulkAction == "notapprove") {
                $("#modal_" + bulkAction).modal('show');
            }
            else {
                $("#modal_" + bulkAction).modal('show');
            }

            return false;
        }

    </script>
</asp:Content>
