<%@ Page Title="Company List" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="custcompanylist.aspx.cs" Inherits="Breederapp.custcompanylist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .password_rules {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #eff2f7;
            /*margin-top: 8px;*/
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
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="Company List" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <input type="hidden" id="hid" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Company Name"></asp:TextBox>&nbsp;&nbsp;
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
                            <th width="20%">
                                <asp:Label ID="lblName1" Text="<%$Resources:Resource, Name %>" runat="server"></asp:Label></th>
                            <th width="15%">
                                <asp:Label ID="lbladdress" Text="Short Name" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label3" Text="Business Type" runat="server"></asp:Label></th>
                            <th width="20%">
                                <asp:Label ID="Label1" Text="Website" runat="server"></asp:Label></th>
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

    <div id="modal_addCustomer" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="lblCust" Text="Add Other Info" CssClass="error_class" runat="server"></asp:Label>
                    </h6>
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Gender %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form_input" data-validate="required">
                                    <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Male %>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Female %>" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label5" Text="<%$Resources:Resource, DOB %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input date-picker" data-validate="date" MaxLength="10" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, AlternateContactNo %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtAlterContactNo" runat="server" CssClass="form_input" data-validate="phone" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblMembershipType" runat="server" CssClass="form_label"><span>Membership Type</span>&nbsp;</asp:Label>
                                <asp:DropDownList ID="ddlMembershipType" runat="server" CssClass="form_input" data-validate="required">
                                    <asp:ListItem Text="Gold" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Silver" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Platinum" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="form_button" OnClick="btnSave_Click" />
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

    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetCompanyForCustomerCount", filter);
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
            getdata3("GetCompanyForCustomer", pageIndex, filter, '', GetCompanyForCustomer);
        }

        function GetCompanyForCustomer(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);
                    var secureid = record.find("securedid").text();
                    var id = record.find("id").text();
                    //var buid = record.find("buid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("companyname").text() + '</td>';
                    contents += '<td>' + record.find("companyshortname").text() + '</td>';
                    contents += '<td>' + record.find("businesstypename").text() + '</td>';
                    contents += '<td>' + record.find("website").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='AddCustomer(" + id + ");' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='JOIN' title='JOIN'>JOIN AS CUSTOMER</a></div></td>";
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function AddCustomer(id) {
            $("#ContentPlaceHolder1_hid").val(id);
            $('#modal_addCustomer').modal('show');
        }
    </script>
</asp:Content>
