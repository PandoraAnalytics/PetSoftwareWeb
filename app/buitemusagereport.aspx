<%@ Page Title="Item Usage Report" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buitemusagereport.aspx.cs" Inherits="Breederapp.buitemusagereport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6>
            <asp:label id="Label11" text="Item Usage Report" runat="server" cssclass="error_class"></asp:label>
        </h6>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                      <asp:label id="Label6" text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:label>
                        &nbsp;
                        <asp:textbox id="txtName" runat="server" cssclass="form_input" maxlength="40" placeholder="Name"></asp:textbox>
                        &nbsp;&nbsp;
                        <asp:linkbutton id="btnApply1" cssclass="form_search_button" runat="server" onclick="btnApply1_Click">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:linkbutton>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable ">
                        <thead>
                            <th width="80%">
                                <asp:label id="Label1" text="<%$Resources:Resource, Name %>" runat="server"></asp:label>
                            </th>
                            <th width="20%">
                                <asp:label id="Label9" text="Usage Count" runat="server"></asp:label>
                            </th>
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

    <script src="js/data.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            process();
        });

        function process() {
            $('.datatable tbody').html('');
            window.scrollTo(0, 0);
            var xmldata = getdata("BUDash_GetItemCountData", 1, filter);
            if (xmldata != null && xmldata.length > 2) {

                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var contents = '<tr>';
                    contents += '<td>' + record.find("productname").text() + '</td>';
                    contents += '<td>' + record.find("item_count").text() + '</td>';
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='10'>" + NoRecordsFound + "</td></tr>");
            }
        }
    </script>
</asp:Content>
