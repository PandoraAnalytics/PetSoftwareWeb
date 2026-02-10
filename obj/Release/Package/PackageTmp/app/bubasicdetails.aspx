<%@ Page Title="Customer Breed - Basic Details" Language="C#" MasterPageFile="bubreeder.Master" AutoEventWireup="true" CodeBehind="bubasicdetails.aspx.cs" Inherits="Breederapp.bubasicdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, BasicInfo %>" runat="server" CssClass="error_class"></asp:Label>
        </h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <br />
        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <asp:Panel ID="panelView" runat="server">
            <div class="row">
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Name %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblName" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, DOB %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblDOB" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label27" Text="<%$Resources:Resource, Age %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblAge" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label20" Text="<%$Resources:Resource, Gender %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblGender" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label22" Text="<%$Resources:Resource, Weight %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblWeight" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, TypeofBreed %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblType" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, CollarId %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblCollarId" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label21" Text="<%$Resources:Resource, Height %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblHeight" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label24" Text="<%$Resources:Resource, CoatColor %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblSpanCoat" Text="-" runat="server"></asp:Label>

                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label5" Text="<%$Resources:Resource, About %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblAbout" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="panelEdit" runat="server" Visible="false">
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label8" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.TypeofBreed %></span>&nbsp;*</asp:Label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required" DataTextField="name" DataValueField="id">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.DOB %></span>&nbsp;*</asp:Label>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label9" Text="<%$Resources:Resource, CollarId %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtCollarId" runat="server" CssClass="form_input" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label13" Text="<%$Resources:Resource, Gender %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form_input">
                        <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Male %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Female %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label14" Text="<%$Resources:Resource, Height %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtHeight" runat="server" CssClass="form_input" data-validate="pdecimal"></asp:TextBox>
                </div>
            </div>

            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label15" Text="<%$Resources:Resource, Weight %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtWeight" runat="server" CssClass="form_input" data-validate="pdecimal"></asp:TextBox>

                </div>

                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label17" Text="<%$Resources:Resource, CoatColor %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtSpanCoat" runat="server" MaxLength="50" CssClass="form_input"></asp:TextBox>

                </div>

            </div>

            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Label ID="Label10" Text="<%$Resources:Resource, About %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtAbout" runat="server" TextMode="MultiLine" CssClass="form_input" MaxLength="500" Rows="8" Width="83%"></asp:TextBox>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label12" Text="<%$Resources:Resource, ProfilePic %>" runat="server" CssClass="form_label"></asp:Label>
                    <input type="file" name="myfile" id="profile_pic" accept="image/*" />
                    <input type="hidden" runat="server" id="hid_profile_pic" />
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="btnSaveBreed" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />
                    &nbsp;
                    <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>

    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });

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
