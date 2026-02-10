<%@ Page Title="Report - All Animals" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="reportallanimals.aspx.cs" Inherits="Breederapp.reportallanimals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />

    <style>
        .profilepic {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            border: 1px solid #ddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, ReportAllAnimals%>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="clearfix"></div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                     
                           <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="<%$Resources:Resource, Name %>"></asp:TextBox>&nbsp;
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form_input" MaxLength="40" placeholder="<%$Resources:Resource, Username %>"></asp:TextBox>&nbsp;

                       <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form_input">
                           <asp:ListItem Text="<%$Resources:Resource, AnimalCateogy %>" Value=""></asp:ListItem>
                           <asp:ListItem Text="<%$Resources:Resource, Horse %>" Value="1"></asp:ListItem>
                           <asp:ListItem Text="<%$Resources:Resource, Dog %>" Value="2"></asp:ListItem>
                       </asp:DropDownList>&nbsp;

                        <asp:DropDownList ID="ddlBreedType" runat="server" CssClass="form_input" DataTextField="namewithbreedname" DataValueField="id">
                             <asp:ListItem Text="<%$Resources:Resource, SelectBreed %>" Value=""></asp:ListItem>
                        </asp:DropDownList>&nbsp;

                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, Usertype %>" Value=""></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AnimalBreeder %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AnimalOwner %>" Value="2"></asp:ListItem>
                        </asp:DropDownList>&nbsp;


                        <asp:LinkButton ID="btnApply1" CssClass="form_search_button" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <table class="table datatable">
                        <thead>
                            <th width="10%">&nbsp;</th>
                            <th width="30%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, TypeofBreed %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, DOB %>" runat="server"></asp:Label></th>
                            <th width="30%">
                                <asp:Label ID="Label4" Text="<%$Resources:Resource, Username %>" runat="server"></asp:Label></th>
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

    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';


        $(document).ready(function () {

            filter = $('#ContentPlaceHolder1_hdfilter').val();

            var totalcount = gettotalcount("GetAllAnimalReportCount", filter);
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
            getdata3("GetAllAnimalReport", pageIndex, filter, '', GetAllReport);
        }

        function GetAllReport(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var contents = '<tr>';
                    if (record.find("profilepic_file").text().length > 0) contents += '<td><img src="docs/' + record.find("profilepic_file").text() + '" alt="p" class="profilepic" /></td>';
                    else contents += '<td><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/></td>';


                    contents += '<td>' + record.find("name").text() + '</td>';

                    contents += '<td>' + record.find("categoryname").text() + ' - ' + record.find("typename").text() + '</td>';
                    contents += '<td>' + record.find("procesed_dob").text() + '</td>';

                    var type = '';
                    switch (record.find("type").text()) {
                        case '1':
                            type = '<%=  Resources.Resource.AnimalBreeder %>'; 
                            break;

                        case '2':
                            type = '<%=  Resources.Resource.AnimalOwner %>'; 
                            break;
                    }

                    contents += '<td>' + record.find("username").text() + ' - ' + type + '<div style="margin-top:5px;opacity:0.7">' + record.find("email").text() + ' - ' + record.find("phone").text() + '</div></td>';

                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("Deleteassociation", id);
            process(1);
        }
    </script>
</asp:Content>
