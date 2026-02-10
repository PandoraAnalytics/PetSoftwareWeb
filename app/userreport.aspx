<%@ Page Title="User Report" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="userreport.aspx.cs" Inherits="Breederapp.userreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css?101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label112" Text="<%$Resources:Resource, userreport %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="row">
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="Label7" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label>:
                         <asp:TextBox ID="txtName" runat="server" CssClass="form_input"></asp:TextBox>&nbsp;
                            <asp:Label ID="Label10" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label>:
                         <asp:TextBox ID="txtEmail" runat="server" CssClass="form_input"></asp:TextBox>&nbsp;
                           <asp:Label ID="Label5" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label>:
                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, Usertype %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AnimalBreeder %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AnimalOwner %>" Value="2"></asp:ListItem>
                        </asp:DropDownList>&nbsp;
                        &nbsp;
                            <asp:LinkButton ID="LinkButton1" CssClass="form_button" runat="server" OnClick="btnApply_Click">
                                <asp:Label ID="Label2" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                            </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnExportToExcel" CssClass="form_button" runat="server" OnClick="btnExportToExcel_Click">
                            <asp:Label ID="Label11" Text="<%$Resources:Resource, ExportToExcel %>" runat="server"></asp:Label>
                        </asp:LinkButton>

                    </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="table-responsive dt_container">
                <input type="hidden" id="hdsort" runat="server" />
                <table class="table datatable">
                    <thead>
                        <th width="25%">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, EmailAddress %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, ContactNumber %>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label8" Text="<%$Resources:Resource, Type %>" runat="server"></asp:Label></th>
                        <th width="15%">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
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

    <script src="js/data.js?123"></script>
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
            getdata3("GetUserDetails", pageIndex, filter, '', getUserDetails_Success);
        }

        function getUserDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var type = record.find("type").text();
                    var typeValue = '';

                    var contents = '<tr>';
                    switch (type) {
                        case '1':
                            typeValue = '<%=  Resources.Resource.AnimalBreeder %>';
                            break;
                        case '2':
                            typeValue = '<%=  Resources.Resource.AnimalOwner %>'; 
                            break;
                    }
                    contents += '<td>' + record.find("fname").text() + " " + record.find("lname").text() + '</td>';
                    contents += '<td>' + record.find("email").text() + '</td>';
                    contents += '<td>' + record.find("phone").text() + '</td>';
                    contents += '<td> ' + typeValue + '</td>';
                    contents += '<td>' + record.find("procesed_datetime").text() + '</td>';

                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }


    </script>
</asp:Content>
