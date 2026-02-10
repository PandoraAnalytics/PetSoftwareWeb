<%@ Page Title="Combo Add" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="comboadd.aspx.cs" Inherits="Breederapp.comboadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="lblComboAdd" Text="Combo Add" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-8 col-xs-12">
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblComboName" runat="server" CssClass="form_label"><span><%= Resources.Resource.Title %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtComboName" runat="server" CssClass="form_input" MaxLength="255" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label8" runat="server" CssClass="form_label"><span>Actual Cost</span> (<asp:Label ID="lblCostCurrency" runat="server"></asp:Label>)&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtCost" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label15" runat="server" CssClass="form_label"><span>Tax</span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlTax" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                        <input type="file" name="myfile" id="combo_pic" accept="image/*" />
                        <input type="hidden" runat="server" id="hid_combo_pic" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">                       
                            <input type="hidden" id="hdpfilter" runat="server" />
                            <input type="hidden" id="cplist" runat="server" />                          
                            <table class="table datatable" id="producttable" style="width: 90% !important;">
                                <thead>
                                    <tr>
                                        <th colspan="3" style="text-align: left; font-weight: bold; font-size: 1.2em;">
                                            <asp:Label ID="Label11" Text="Product List" runat="server" CssClass="error_class"></asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div class="dt_footer" style="width: 90% !important;">
                                <div class="pagination float-end" id="productpagination">
                                </div>
                            </div>                      
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">                       
                            <input type="hidden" id="hdsfilter" runat="server" />
                            <input type="hidden" id="cplist2" runat="server" />                           
                            <table class="table datatable" id="servicetable" style="width: 90% !important;">
                                <thead>
                                    <tr>
                                        <th colspan="3" style="text-align: left; font-weight: bold; font-size: 1.2em;">
                                            <asp:Label ID="Label1" Text="Service List" runat="server" CssClass="error_class"></asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div class="dt_footer" style="width: 90% !important;">
                                <div class="pagination float-end" id="servicepagination">
                                </div>
                            </div>                      
                    </div>
                </div>
                <br />
                <br />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                     <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var productfilter = '';
        var servicefilter = '';
        var costCurrency = '<%= this.GetCurrntBUCurrency() %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
        $(document).ready(function () {
            loadProduct();
            loadService();
        });

        function loadProduct() {
            productfilter = $('#ContentPlaceHolder1_hdpfilter').val();
            var totalcount = gettotalcount("GetProductForComboCount", productfilter);
            processproduct(1);
            $('#productpagination').bootpag({
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
                processproduct(num);
            });
        }

        function processproduct(pageIndex) {
            $('#producttable tbody').html('');
            getdata3("GetProductForComboDetails", pageIndex, productfilter, '', getProductDetails_Success);
        }

        function getProductDetails_Success(data) {
            $('#producttable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var array = $("#ContentPlaceHolder1_cplist").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var id1 = record.find("id").text();
                    var contents = '<tr>';
                    var inArray = jQuery.inArray(id1, array);
                    if (inArray >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" class="call-checkbox" value="' + id1 + '"></td>';
                    var main_desc = record.find("description").text();
                    if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                    contents += '<td width="80%">' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + main_desc + '</div></td>';
                    contents += '<td width="15%">' + costCurrency + ' ' + record.find("processed_cost").text() + '</div></td>';
                    contents += '</tr>';
                    $('#producttable > tbody:last').append(contents);
                });
            }
            else {
                $('#producttable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function loadService() {
            servicefilter = $('#ContentPlaceHolder1_hdsfilter').val();
            var totalcount = gettotalcount("GetServiceCount", servicefilter);
            processservice(1);
            $('#servicepagination').bootpag({
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
                processservice(num);
            });
        }

        function processservice(pageIndex) {
            $('#servicetable tbody').html('');
            getdata3("GetServicesDetails", pageIndex, servicefilter, '', getServicesDetails_Success);
        }

        function getServicesDetails_Success(data) {
            $('#servicetable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                var array2 = $("#ContentPlaceHolder1_cplist2").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var id2 = record.find("id").text();
                    var contents = '<tr>';
                    var inArray2 = jQuery.inArray(id2, array2);
                    if (inArray2 >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" class="call-checkbox" value="' + id2 + '"></td>';
                    var main_desc = record.find("description").text();
                    if (main_desc.length > 50) main_desc = '<span data-toggle="tooltip" data-placement="top" data-original-title="' + record.find("description").text().replace(/"/g, "&quot;") + '">' + main_desc.substring(0, 50) + '...' + '</span>';
                    contents += '<td width="80%">' + record.find("name").text() + '<div style="margin-top:5px;opacity:0.8">' + main_desc + '</div></td>';
                    contents += '<td width="15%">' + costCurrency + ' ' + record.find("processed_cost").text() + '</div></td>';
                    contents += '</tr>';
                    $('#servicetable > tbody:last').append(contents);
                });
            }
            else {
                $('#servicetable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        $(document).on('change', '#producttable .call-checkbox', function (e) {
            var listArray = $("#ContentPlaceHolder1_cplist").val().split(';');

            if ($(this).is(':checked')) {
                listArray.push($(this).val());
            }
            else {
                listArray.remove($(this).val());
            }
            $("#ContentPlaceHolder1_cplist").val(listArray.join(';'));
        });

        $(document).on('change', '#servicetable .call-checkbox', function (e) {
            var listArray2 = $("#ContentPlaceHolder1_cplist2").val().split(';');

            if ($(this).is(':checked')) {
                listArray2.push($(this).val());
            }
            else {
                listArray2.remove($(this).val());
            }
            $("#ContentPlaceHolder1_cplist2").val(listArray2.join(';'));
        });

        Array.prototype.remove = function (v) { this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1); }


        $('#combo_pic').change(function () {
            var f = $(this);
            var fid = $(f).attr('id');

            var fileData = $(f).prop("files")[0];
            var formData = new window.FormData();
            formData.append("file", fileData);
            formData.append("extns", "image");

            var id = 'per_lbl_' + fid;
            $('#' + id).remove();

            $.ajax({
                url: 'file_upload_docs.ashx',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            if (!isNaN(percentComplete)) {
                                percentComplete = percentComplete * 100;
                                if (percentComplete > 100) percentComplete = 100;
                                var id = 'per_lbl_' + fid;
                                $('#' + id).remove();
                                $(f).after("<span id='" + id + "'>Uploaded: " + parseFloat(percentComplete).toFixed(2) + "%</span>");
                            }
                        }
                    }, false);
                    return xhr;
                },
                success: function (data) {
                    var hid = '#ContentPlaceHolder1_hid_combo_pic';
                    var fileuploadedsuccessfully = '<%= Resources.Resource.Fileuploadedsuccessfully %>';
                    $(hid).val(data);
                    alert(fileuploadedsuccessfully);
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert(fileuploadingerror);

                }
            });
        });
    </script>
</asp:Content>
