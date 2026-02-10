<%@ Page Title="Manage Certificate Approval" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="managecertificateapproval.aspx.cs" Inherits="Breederapp.managecertificateapproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, ManageCertificateApproval %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="float-end">
            <div class="pull-right">
                <input type="hidden" id="hdfilter" runat="server" />
                <asp:Button ID="btnBulkApprove" runat="server" Text="<%$Resources:Resource, ApproveSelected %>" CssClass="form_button" OnClientClick="return OpenBulkAction('approve');" />&nbsp;
                <asp:Button ID="btnBulkReject" runat="server" Text="<%$Resources:Resource, RejectSelected %>" CssClass="form_button" OnClientClick="return OpenBulkAction('notapprove');" />
            </div>
        </div>
        <div class="clearfix"></div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblMessage" runat="server" Style="padding-left: 10px;"></asp:Label>
                <div class="table-responsive dt_container">
                    <input type="hidden" id="cplist" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                          <asp:Label ID="Label1" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblType" Text="<%$Resources:Resource, CertificateName %>" runat="server"></asp:Label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form_input" MaxLength="40"></asp:TextBox>
                        <asp:Button ID="btnApply" runat="server" Text="<%$Resources:Resource, Apply %>" CssClass="form_button" OnClick="btnApply_Click" />

                    </div>
                    <table class="table datatable">
                        <thead>
                            <th width="5%"></th>
                            <th width="30%">
                                <asp:Label ID="lblType1" Text="<%$Resources:Resource, CertificateName %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, UserName %>" runat="server"></asp:Label></th>
                            <th width="25%">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, Animal %>" runat="server"></asp:Label></th>
                            <th width="15%">&nbsp;</th>
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

    <div id="modal_notapprove" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label5" Text="<%$Resources:Resource, RejectCertificate %>" CssClass="error_class" runat="server"></asp:Label></h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label8" Text="<%$Resources:Resource, Comment %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtNotAppRemark" CssClass="form_input" runat="server" MaxLength="255" data-validate="maxlength-255" Rows="5" TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="Button3" runat="server" Text="<%$Resources:Resource, RejectCertificate %>" CssClass="form_button" OnClick="btnReject_Click" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label9" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
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
                        <asp:Label ID="Label4" Text="<%$Resources:Resource, ApproveCertificate %>" CssClass="error_class" runat="server"></asp:Label></h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Comment %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtAppRemark" CssClass="form_input" runat="server" MaxLength="255" data-validate="maxlength-255" Rows="5" TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="Button1" runat="server" Text="<%$Resources:Resource, ApproveCertificate %>" CssClass="form_button" OnClick="btnApproval_Click" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label6" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
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
            var totalcount = gettotalcount("GetAllCertificatesToApproveCount", filter);
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
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            $('.datatable tbody').html('');
            getdata3("GetAllCertificatesToApprove", pageIndex, filter, '', GetAllApproval);
        }

        function GetAllApproval(data) {
            $('.datatable tbody').html('');
            $('.call-checkbox-all').prop('checked', false);
            window.scrollTo(0, 0);
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);
                    var secureid = record.find("securedid").text();
                    var id = record.find("id").text();
                    var contents = '<tr>';
                    contents += '<td><input id="c' + id + '" class="call-checkbox" type="checkbox" value="' + id + '" /></td>';
                    contents += '<td>' + record.find("certificate_name").text() + ' - ' + record.find("certificate_typename").text() + '<div style="opacity:0.8;font-weight: 100; font-size: 13px;margin-top:3px;">' + record.find("procesed_startdate").text() + ' - ' + record.find("procesed_enddate").text() + '</div></td>';
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("name").text() + ' - ' + record.find("typename").text() + '</td>';

                    var f = record.find("certificate_file").text();
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='../app/viewdocument.aspx?file=" + f + "' target='_blank' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View Certificate' title='View Certificate'><i class='fa-solid fa-file'></i></a><a href='javascript:void(0);' onclick='singleApprove(" + id + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Approve' title='Approve '><i class='fa-regular fa-thumbs-up'></i></a><a href='javascript:void(0);' onclick='singleReject(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Reject' title='Reject'><i class='fa-regular fa-thumbs-down'></i></a></div></td>";

                    //contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='docs/" + record.find("certificate_file").text() + "' target='_blank' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View Certificate' title='View Certificate'><i class='fa-solid fa-file'></i></a><a href='javascript:void(0);' onclick='singleApprove(" + id + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Approve' title='Approve '><i class='fa-regular fa-thumbs-up'></i></a><a href='javascript:void(0);' onclick='singleReject(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Reject' title='Reject'><i class='fa-regular fa-thumbs-down'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function singleApprove(id) {
            $('.call-checkbox').prop('checked', false);
            var checkbox = $('#c' + id);
            $(checkbox).prop('checked', true);
            $("#ContentPlaceHolder1_cplist").val(id);
            OpenBulkAction('approve');
        }

        function singleReject(id) {
            $('.call-checkbox').prop('checked', false);
            var checkbox = $('#c' + id);
            $(checkbox).prop('checked', true);
            $("#ContentPlaceHolder1_cplist").val(id);
            OpenBulkAction('notapprove');
        }

        function OpenBulkAction(bulkAction) {
            if (bulkAction == "approve") {
                $("#modal_" + bulkAction).modal('show');
            }
            else {
                $("#modal_" + bulkAction).modal('show');
            }

            return false;
        }

        $(document).on('change', '.call-checkbox', function (e) {
            var listArray = [];
            $('input:checkbox.call-checkbox').each(function () {
                if ($(this).is(':checked')) {
                    listArray.push($(this).val());
                }
            });

            $("#ContentPlaceHolder1_cplist").val(listArray.join(';'));
        });
    </script>
</asp:Content>
