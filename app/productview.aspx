<%@ Page Title="Product View" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="productview.aspx.cs" Inherits="Breederapp.productview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
            /*color: var(--bs-nav-pills-link-active-color) !important;*/
            /*background-color: var(--bs-nav-pills-link-active-bg);*/
            background-color: #f28c0e;
        }

        .nav-pills .nav-link {
            border-radius: 0 !important;
        }

        .nav-link:focus, .nav-link:hover {
            color: #ffffff !important;
        }

        .nav-link {
            color: #ffffff !important;
        }

        .photos {
            /*margin-bottom: 25px;*/
        }

            .photos li {
                display: inline-block;
                padding-right: 10px;
                position: relative;
            }

                .photos li a {
                    display: inline-block;
                    min-width: 120px;
                    border: solid 1px #ddd;
                    text-align: center;
                }

                .photos li img {
                    width: 100%;
                    object-fit: cover;
                    height: 90px;
                    padding: 2px;
                    max-width: 100%;
                }

                .photos li .cross {
                    position: absolute;
                    top: -5px;
                    right: 5px;
                    border: 0;
                    border-radius: 50%;
                    min-width: 0;
                    background-color: #d64651;
                    color: #fff;
                    padding: 1px 6px;
                }

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

        /* .form_input {
            width: 50%;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h5 class="page_title">
                    <asp:Label ID="lblHeading" Text="Product View" CssClass="error_class" runat="server"></asp:Label></h5>
            </div>
        </div>
        <br />
        <div class="dt_tab_pages">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                        <li id="tablnkBasic" class="nav-item" role="presentation">
                            <button class="nav-link active" id="basic_details-tab" data-bs-toggle="pill" data-bs-target="#tab_basic_details" type="button" role="tab" aria-controls="tab_basic_details"
                                aria-selected="true">
                                <asp:Label ID="lblBasic" runat="server" Text="<%$Resources:Resource, BasicDetails %>"></asp:Label></button>
                        </li>
                        <li id="tablnkImages" class="nav-item" role="presentation">
                            <button class="nav-link" id="images-tab" data-bs-toggle="pill" data-bs-target="#tab_images" type="button" role="tab" aria-controls="tab_images"
                                aria-selected="false">
                                <asp:Label ID="lblImages" runat="server" Text="Images"></asp:Label></button>
                        </li>
                    </ul>

                    <div class="tab-content clearfix" id="pills-tabContent">
                        <!------------ tab_1 ------------>
                        <div class="tab-pane fade show active" role="tabpanel" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0">
                            <asp:Panel ID="panelAction" runat="server">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit Product" CssClass="form_button" OnClick="btnEdit_Click" validate="no" Visible="true" />&nbsp;&nbsp;
                                         <a href="productlist.aspx">
                                             <asp:Label ID="lblBack" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label></a>
                                    </div>
                                </div>
                                <br />
                            </asp:Panel>
                            <div class="form_horizontal basic_deatils_form" style="line-height: 25px;">
                                <div class="row">
                                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                    <fieldset id="fs_Description" runat="server">
                                        <legend>
                                            <asp:Label ID="Label6" runat="server" Text="Basic Info"></asp:Label></legend>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label1" runat="server" CssClass="form_label" Text="Name"></asp:Label>
                                                    <asp:Label ID="lblName" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label20" Text="Category" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblCategory" Text="-" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label22" Text="Brand" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblBrand" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label9" Text="Material Type" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblType" Text="-" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label14" Text="Status" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblStatus" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label03" runat="server" CssClass="form_label" Text="Product Tag"></asp:Label>
                                                    <asp:Label ID="lblproducttag" Text="-" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row form_row">                                               
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label11" Text="Actual Cost" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblCostCurrency" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblCost" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label25" runat="server" CssClass="form_label" Text="Threshold Stock Value"></asp:Label>
                                                    <asp:Label ID="lblThresholdStockValue" Text="-" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label21" runat="server" CssClass="form_label" Text="Size"></asp:Label>
                                                    <asp:Label ID="lblSize" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label24" runat="server" CssClass="form_label" Text="Color"></asp:Label>
                                                    <asp:Label ID="lblColor" Text="-" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <%-- <div class="row form_row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label25" runat="server" CssClass="form_label" Text="Original Quantity"></asp:Label>
                                                <asp:Label ID="lblOriginalQuentity" Text="-" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                <asp:Label ID="Label27" runat="server" CssClass="form_label" Text="Stock Quantity"></asp:Label>
                                                <asp:Label ID="lblStockQuentity" Text="-" runat="server"></asp:Label>
                                            </div>
                                        </div>--%>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label23" runat="server" CssClass="form_label" Text="Weight"></asp:Label>
                                                    <asp:Label ID="lblWeight" Text="-" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblWeightType" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label2" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Panel ID="panelProfiePic" runat="server" Visible="false">
                                                        <a href="javascript:void(0);" id="lnkProductProfilePic" runat="server" target="_blank">View Profile Pic</a>
                                                    </asp:Panel>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label15" Text="<%$Resources:Resource, Description %>" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblAboutDiscription" Text="-" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label5" Text="Tax" runat="server" CssClass="form_label"></asp:Label>
                                                    <asp:Label ID="lblTax" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <br />
                                    <br />
                                    <br />
                                    <fieldset id="fs_StockInfo" runat="server">
                                        <input type="hidden" id="hdfilter" runat="server" />
                                        <legend>
                                            <asp:Label ID="lblOtherInfo" runat="server" Text="Stock Info"></asp:Label></legend>

                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-left">
                                                <asp:Label ID="Label26" runat="server" CssClass="form_label"><span>Available Stock</span>&nbsp;</asp:Label>
                                                <asp:Label ID="lblAvailableStock" Text="-" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                            </div>
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">

                                                <asp:LinkButton ID="lnkAdd" CssClass="add_form_btn btnborder" runat="server" OnClick="lnkAdd_Click">
                                                    <i class="fa-solid fa-plus"></i>&nbsp;
                                                        <asp:Label ID="Label7" Text="Add New Stock" runat="server"></asp:Label>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="table-responsive dt_container">
                                                    <input type="hidden" id="hdsort" runat="server" />
                                                    <table class="table datatable">
                                                        <thead>
                                                            <th width="20%">
                                                                <asp:Label ID="Label13" Text="Date" runat="server"></asp:Label></th>
                                                            <th width="20%">
                                                                <asp:Label ID="Label16" Text="Quantity" runat="server"></asp:Label></th>
                                                            <th width="20%">
                                                                <asp:Label ID="Label17" Text="PO" runat="server"></asp:Label></th>
                                                            <th width="30%">
                                                                <asp:Label ID="Label19" Text="Added By" runat="server"></asp:Label></th>
                                                            <th width="10%">&nbsp;</th>
                                                        </thead>

                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                    <div class="dt_footer">
                                                    </div>
                                                </div>
                                                <br />
                                                <h6>
                                                    <asp:Label ID="lblTotalQuantity1" Text="Total Quantity" runat="server"></asp:Label>
                                                    <label id="lblTotalQuantity" style="padding-right: 180px;"></label>
                                                </h6>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <asp:Panel ID="panelAddStock" runat="server" Visible="false">
                                            <h6>
                                                <span id="Label56" class="error_class">
                                                    <asp:Label ID="lblStock" Text="Add New Stock" class="error_class" runat="server"></asp:Label>
                                                </span></h6>
                                            <br />
                                            <asp:Label ID="lblStockError" runat="server" CssClass="error_class"></asp:Label>
                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label18" runat="server" CssClass="form_label"><span>Date</span>&nbsp;*</asp:Label>
                                                    <asp:TextBox ID="txtStockDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label10" runat="server" CssClass="form_label"><span>Quantity</span>&nbsp;*</asp:Label>
                                                    <asp:TextBox ID="txtStockQuantity" runat="server" data-validate="required pnumber" CssClass="form_input" MaxLength="5"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row form_row">
                                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                                    <asp:Label ID="Label8" runat="server" CssClass="form_label"><span>PO Number</span>&nbsp;</asp:Label>
                                                    <asp:TextBox ID="txtPONumber" runat="server" CssClass="form_input" MaxLength="25"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="btnAddStock" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" OnClick="btnAddStock_Click" />
                                                    &nbsp;
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                    <br />
                                </div>
                            </div>
                        </div>
                        <!------------ tab_2 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_images" aria-labelledby="images-tab" tabindex="0">
                            <div class="form_horizontal images_form">
                                <div class="row">
                                    <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                        <h6 class="page_title">
                                            <asp:Label ID="Label4" Text="Images" CssClass="error_class" runat="server"></asp:Label></h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <ul class="photos">
                                        <asp:Repeater ID="repPhotos" runat="server">
                                            <ItemTemplate>
                                                <li>                                                   
                                                    <a href='<%# string.Format("../app/viewdocument.aspx?file={0}", Eval("gallery_file")) %>' target="_blank">
                                                        <img src='../images/image_loading.gif' class="lazy-img" data-src='<%# Eval("gallery_file") %>' alt="photo" />
                                                    </a>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <br />
                                                &nbsp;
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/data.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/validator.js" type="text/javascript"></script>
    <script>
        var filter = '<%= ViewState["id"] %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            loadLazyImages();
            process(1);

        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetAllProductStock", pageIndex, filter, '', GetAllProductStock);
        }

        function GetAllProductStock(data) {
            $('.datatable tbody').html('');
            $("#lblTotalQuantity").html('-');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var quan = 0;
                records.each(function () {
                    var record = $(this);
                    var id1 = record.find("id").text();
                    var id = record.find("securedid").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("formatteddate").text() + '</td>';
                    contents += '<td>' + record.find("stockquan").text() + '</td>';
                    contents += '<td>' + record.find("ponumber").text() + '</td>';
                    contents += '<td>' + record.find("addedby").text() + '</td>';
                    quan += parseInt(record.find("stockquan").text());
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='editstock(" + id1 + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deletestock(\"" + id1 + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";

                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                    $("#lblTotalQuantity").html(quan);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        //function deleteRecord(id) {
        //    deleteData("DeleteProductStock", id);
        //    process(1);
        //}

        function editstock(id) {
            __doPostBack('editstock', id);
        }

        function deletestock(id) {
            __doPostBack('deletestock', id);
        }

       <%-- function GetProductStock(id) {
            $("#ContentPlaceHolder1_hdfilter").val('');
            $("#ContentPlaceHolder1_txtStockDate").val('');
            $("#ContentPlaceHolder1_txtStockQuantity").val('');
            $("#ContentPlaceHolder1_txtPONumber").val('');

            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/GetProductStock",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                success: function (msg) {
                    if (msg != null && msg.d != null) {
                        var ppe = JSON.parse(msg.d);
                        $("#ContentPlaceHolder1_txtStockDate").val(ppe.date);
                        $("#ContentPlaceHolder1_txtStockQuantity").val(ppe.stockquan);
                        $("#ContentPlaceHolder1_txtPONumber").val(ppe.ponumber);
                        $("#ContentPlaceHolder1_hdfilter").val(ppe.id);
                        window.scrollTo(0, 0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var unknownerror = '<%= Resources.Resource.error %>';
                    alert(unknownerror);
                }
            });
        }--%>


    </script>
    <script>

    <%--        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
            $(document).ready(function () {
                filter = $('#ContentPlaceHolder1_hdfilter').val();
                var totalcount = gettotalcount("GetProductCount", filter);
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
                getdata3("GetProductDetails", pageIndex, filter, '', getProductDetails_Success);
            }

            function getProductDetails_Success(data) {
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
                        var main_desc = record.find("description").text();
                        if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                        contents += '<td style="word-break:break-all;">' + main_desc + '</td>';
                        /* contents += '<td>' + record.find("description").text() + '</td>';*/
                        contents += '<td>' + record.find("cost").text() + '</td>';
                        contents += '<td>' + record.find("material_type").text() + '</td>';
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='productview.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                        contents += '</tr>';
                        $('.datatable > tbody:last').append(contents);
                    });
                }
                else {
                    $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
                }
            }

            function deleteRecord(id) {
                deleteData("Deleteproduct", id);
                process(1);
            }--%>
    </script>
</asp:Content>
