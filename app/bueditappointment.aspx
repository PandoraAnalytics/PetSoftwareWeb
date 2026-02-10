<%@ Page Title="Edit Appointment" Language="C#" MasterPageFile="bubreeder.master" AutoEventWireup="true" CodeBehind="bueditappointment.aspx.cs" Inherits="Breederapp.bueditappointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />

    <style>
        .fstElement {
            width: 65%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Editappointment%>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, Date%>" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblStartTime" runat="server" CssClass="form_label"><span><%= Resources.Resource.StartTime %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlStartTime" runat="server" CssClass="form_input " data-validate="required"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Category %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlProfession" runat="server" CssClass="form_input" data-validate="required" AutoPostBack="true" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="ddlProfession_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label8" runat="server" CssClass="form_label"><span><%= Resources.Resource.ContactDetails %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlContact" runat="server" CssClass="form_input" data-validate="required" DataTextField="full_name" DataValueField="id">
                        </asp:DropDownList>
                        <a href="javascript:void(0);" onclick="opencontactform();">
                            <asp:Label ID="lbl" runat="server" Text="<%$Resources:Resource, AddContact %>"></asp:Label></a>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, SetReminder %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlReminder" runat="server" AutoPostBack="true" CssClass="form_input" OnSelectedIndexChanged="ddlReminder_SelectedIndexChanged" data-validate="required pnumber">
                            <asp:ListItem Text="<%$Resources:Resource, None %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Set %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Panel ID="panelReminder" runat="server" Visible="false">
                            <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.ReminderMeBefore %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtReminderNumber" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="2" Width="27%"></asp:TextBox>
                            <asp:DropDownList ID="ddlReminderText" runat="server" CssClass="form_input" Width="33%">
                                <asp:ListItem Text="<%$Resources:Resource, Days %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Hours  %>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:Panel>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnEditAppointment" CssClass="form_button" Text="<%$Resources:Resource, Updatethisoccurrence  %>" runat="server" OnClick="btnEditAppointment_Click" />&nbsp;
                        <asp:Button ID="btnEditAppointmentFollowing" CssClass="form_button" Text="<%$Resources:Resource, Updatethisnfollowingoccurrences  %>" runat="server" OnClick="btnEditAppointmentFollowing_Click" />&nbsp;
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
            </div>
        </div>
    </div>

    <div id="modal_addcontact" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label9" Text="<%$Resources:Resource, AddContact %>" CssClass="error_class" runat="server"></asp:Label></h6>
                    <div class="login_form">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label10" Text="<%$Resources:Resource, Category %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:DropDownList ID="ddlModalProfession" runat="server" CssClass="form_input modal_element" DataTextField="name" DataValueField="id" Width="84%"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, FirstName %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtModalFirstName" runat="server" CssClass="form_input modal_element" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label17" Text="<%$Resources:Resource, LastName %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtModalLastName" runat="server" CssClass="form_input modal_element" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Button ID="btnAddContact" runat="server" Text="<%$Resources:Resource, AddContact %>" CssClass="form_button" OnClick="btnAddContact_Click" validate="no" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label12" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.multipleSelect').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
                noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
            });
        });
        var RequriedMsg = '<%=  Resources.Resource.RequriedMsg %>';
        function opencontactform() {
            $('#modal_addcontact').modal('show');
        }
        $("#ContentPlaceHolder1_btnAddContact").click(function () {
            var success = true;

            var cls = '.modal_element';

            $(cls).each(function () {
                var id = 'err_lbl_' + $(this).attr('id');
                $('#' + id).remove();
                if ($.trim($(this).val()).length == 0) {
                    $(this).after("<span id='" + id + "' class='error_class'>" + RequriedMsg + "</span>");
                    success = false;
                }
            });

            return success;
        });
    </script>
</asp:Content>

