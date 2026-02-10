<%@ Page Title="Manage Breeds Types" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="managebreedstype.aspx.cs" Inherits="Breederapp.managebreedstype" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 60%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ManageBreedsTypes%>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-5 col-md-5 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <input type="hidden" id="hdfilter" runat="server" />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.TypeofBreed %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required" DataTextField="breedname" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSubmit" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;
                    </div>
                </div>
            </div>
            <div class="col-lg-7 col-md-7 col-sm-6 col-xs-12">
                <br />
                <table class="table datatable">
                    <thead>
                        <th width="20%">
                            <asp:Label ID="lblName" Text="<%$Resources:Resource,  Name%>" runat="server"></asp:Label></th>
                        <th width="20%">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, TypeofBreed %>" runat="server"></asp:Label></th>
                        <th width="10%">&nbsp;</th>
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

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js?101"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            process(1);
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetBreedTypes", pageIndex, filter, '', GetSetBreedDoc_Success);
        }

        function GetSetBreedDoc_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var id = record.find("id").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("name").text() + '</td>';
                    contents += '<td>' + record.find("breedname").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='GetBreedDocs(" + id + ");' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(" + id + ")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';

                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteBreedTypeCategory", id);
            var u = window.location.href;
            window.location = u;
        }

        //function deleteRecord(id) {
        //    deleteData("DeleteBreedCategory", id);
        //    var u = window.location.href;
        //    window.location = u;
        //}

        function GetBreedDocs(id) {
            $("#ContentPlaceHolder1_hdfilter").val('');
            $("#ContentPlaceHolder1_ddlType").val('');
            $("#ContentPlaceHolder1_hid_profile_pic").val('');

            $.ajax({
                type: "POST",
                url: "Services/getdata.asmx/GetBreedType",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id }),
                success: function (msg) {
                    if (msg != null && msg.d != null) {
                        var ppe = JSON.parse(msg.d);
                        $("#ContentPlaceHolder1_txtName").val(ppe.name);
                        $("#ContentPlaceHolder1_ddlType").val(ppe.categoryid);
                        $("#ContentPlaceHolder1_hdfilter").val(ppe.id);
                        window.scrollTo(0, 0);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var unknownerror = '<%= Resources.Resource.error %>';
                    alert(unknownerror);
                }
            });
        }
    </script>

    <script>
        $('#profile_pic').change(function () {
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
                    var hid = '#ContentPlaceHolder1_hid_profile_pic';
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
