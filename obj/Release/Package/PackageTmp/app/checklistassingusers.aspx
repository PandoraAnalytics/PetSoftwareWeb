<%@ Page Title="Checklist Assign Members" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="checklistassingusers.aspx.cs" Inherits="Breederapp.checklistassingusers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, AssignMembers %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <h6>&nbsp;-&nbsp;&nbsp;(<asp:Label ID="lblTitle" runat="server"></asp:Label>)</h6>

        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" placeholder="<%$Resources:Resource, Name %>" MaxLength="40"></asp:TextBox>
                        <asp:LinkButton ID="btnApply1" CssClass="form_element form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                        &nbsp;
                        <a href=".aspx">
                            <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click">
                                <asp:Label ID="Label6" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label>
                            </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <input type="hidden" id="cplist" runat="server" />
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="5%">&nbsp;</th>
                            <th width="20%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lbladdress" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="lblPhone" Text="<%$Resources:Resource, ContactNumber %>" runat="server"></asp:Label></th>
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

        <div class="row form-group">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" OnClick="btnSubmit_Click" />
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

            var totalcount = gettotalcount("GetUserCount", filter);
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
            getdata3("GetUserDetails", pageIndex, filter, '', GetAllAssociate);
        }

        function GetAllAssociate(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var itemArray = $("#ContentPlaceHolder1_cplist").val().split(';');

                records.each(function () {
                    var record = $(this);
                    var id1 = record.find("id").text();
                    var inArray = jQuery.inArray(id1, itemArray);

                    var typeValue = '';
                    var contents = '<tr>';
                    if (inArray >= 0) contents += "<td valign='top'><input id='checkbox_id_" + id1 + "' class='call-checkbox' checked type='checkbox' value='" + id1 + "' />";
                    else contents += "<td valign='top'><input id='checkbox_id_" + id1 + "' class='call-checkbox' type='checkbox' value='" + id1 + "' />";
                    contents += '<td>' + record.find("fname").text() + ' ' + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("phone").text() + '</td>';

                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='4'>" + NoRecordsFound + "</td></tr>");
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

