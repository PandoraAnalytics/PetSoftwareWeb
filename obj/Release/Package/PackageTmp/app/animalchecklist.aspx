<%@ Page Title="Animal Checklist" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="animalchecklist.aspx.cs" Inherits="Breederapp.animalchecklist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AssignChecklist %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
        </div>
        <div class="clearfix"></div>
        <div class="alert alert-info" role="alert">
            <asp:Label ID="lblWarning" runat="server" Text="<%$Resources:Resource, Checklistsdisabled %>"></asp:Label>
        </div>       
        <br />
        <br />
        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label11" Text="<%$Resources:Resource, Checklist %>" runat="server" CssClass="error_class"></asp:Label></h6>

                <input type="hidden" id="hidfilter" runat="server" />
                 <input type="hidden" id="hidfiler1" runat="server" />
                <input type="hidden" id="cplist" runat="server" />
                <table class="table datatable" id="checklistable" style="width: 90% !important;">
                    <thead>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="dt_footer" style="width: 90% !important;">
                    <div class="pagination float-end">
                    </div>
                </div>
                <br />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnChecklist" runat="server" Text="<%$Resources:Resource, AssignChecklist %>" CssClass="form_button top_btn" OnClick="btnChecklist_Click" validate="no" />
                    </div>
                </div>
            </div>
            <div class="col-lg-8 col-md-8 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="main_container">
                    <div class="row form_row">
                        <asp:Panel ID="panelNoQuestion" runat="server">
                            <asp:Label ID="lbldropithere" Text="<%$Resources:Resource, Selectchecklisttoassignanimal %>" runat="server"></asp:Label>
                        </asp:Panel>

                        <asp:Panel ID="panelQuestions" runat="server" Visible="false">
                            <table class="table datatable" id="assignedtable">
                                <thead>
                                    <th width="50%">
                                        <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                                    <th width="20%">
                                        <asp:Label ID="lblType" Text="<%$Resources:Resource, Category %>" runat="server"></asp:Label></th>
                                    <th width="30%">&nbsp;</th>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div class="dt_footer">
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>

    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetChecklistCount", filter);
            showallchecklist(1);
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
                showallchecklist(num);
            });

            showassignedchecklist();
        });

        function showallchecklist(pageIndex) {
            $('#checklistable tbody').html('');
            getdata3("GetAllChecklists", pageIndex, filter, '', GetAllChecklist_Success);
        }

        function GetAllChecklist_Success(data) {
            $('#checklistable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var array = $("#ContentPlaceHolder1_cplist").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id1 = record.find("id").text();
                    var checkpointcount = record.find("checkpointcount").text();
                    var contents = '<tr>';
                    var inArray = jQuery.inArray(id1, array);
                    if (checkpointcount <= 0) {
                        contents += '<td title="Checkpoints are not available." width="5%" valign="top"><input type="checkbox" disabled></td>';
                    } else if (inArray >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" class="call-checkbox" value="' + id1 + '"></td>';
                    contents += '<td width="95%">' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + record.find("categoryname").text() + '</div></td>';
                    contents += '</tr>';

                    $('#checklistable > tbody:last').append(contents);
                });
            }
            else {
                $('#checklistable tbody').html("<tr><td colspan='2'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function showassignedchecklist() {
            var animalid = '<%= ViewState["id"]%>';
            $('#assignedtable tbody').html('');
            getdata3("GetAnimalChecklist", 1, animalid, '', GetAssignedChecklist_Success);
        }

        var m_viewresponse = '<%=  Resources.Resource.ViewResponse %>';
        function GetAssignedChecklist_Success(data) {
            $('#assignedtable tbody').html('');
            var xmldata = data.d;
            var animalid = '<%= ViewState["id"]%>';
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    contents = '<tr>';
                    contents += '<td>' + record.find("name").text() + '</td>';
                    contents += '<td>' + record.find("categoryname").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='checklistviewresponse.aspx?id=" + record.find("securedchecklistid").text() + "&aid=" + animalid + "' class='btn btn_edit'>" + m_viewresponse + "</a><a href='javascript:void(0);' onclick='confirm_delete(\"" + record.find("securedid").text() + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";
                    contents += '</tr>';

                    $('#assignedtable > tbody:last').append(contents);
                });
            }
            else {
                $('#assignedtable tbody').html("<tr><td colspan='2'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function confirm_delete(id) {
            var confirm_msg = '<%=  Resources.Resource.DeleteRecordConfirmationMessage %>';
            var answer = confirm(confirm_msg);
            if (answer) {
                $.ajax({
                    type: "POST",
                    url: "Services/getdata.asmx/UnassignedAnimalChecklist",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ id: id }),
                    success: function (msg) {
                        showassignedchecklist();
                        showallchecklist(1);
                    }
                });
            }
        }

        $(document).on('change', '.call-checkbox', function (e) {
            var listArray = $("#ContentPlaceHolder1_cplist").val().split(';');

            if ($(this).is(':checked')) {
                listArray.push($(this).val());
            }
            else {
                listArray.remove($(this).val());
            }
            $("#ContentPlaceHolder1_cplist").val(listArray.join(';'));
        });

        Array.prototype.remove = function (v) { this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1); }
    </script>
</asp:Content>
