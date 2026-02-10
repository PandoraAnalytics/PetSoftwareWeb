<%@ Page Title="Add Appointment" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="createappointment.aspx.cs" Inherits="Breederapp.createappointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/fastselect.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />

    <style>
        .fstElement {
            width: 65%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Addappointment %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label15" runat="server" CssClass="form_label"><span><%= Resources.Resource.Date %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off"></asp:TextBox>
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
                        <asp:Label ID="lblRepeat" Text="<%$Resources:Resource, Repeat %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlRepeat" runat="server" AutoPostBack="true" CssClass="form_input" OnSelectedIndexChanged="ddlRepeat_SelectedIndexChanged">
                            <asp:ListItem Text="<%$Resources:Resource, NoRepeat %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Daily %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Weekly %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Monthly %>" Value="3"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, DayOfWeeks %>" Value="4"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Annually %>" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:Panel ID="panelRepeat" runat="server" Visible="false">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Panel ID="panelDaily" runat="server" Visible="false">
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, Daily%>" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form_input" ReadOnly="true" Text="<%$Resources:Resource, RepeatDaily%>" Style="background: #ddd;"></asp:TextBox>
                            </asp:Panel>
                            <asp:Panel ID="panelWeekly" runat="server" Visible="false">
                                <asp:Label ID="Label13" runat="server" CssClass="form_label"><span><%= Resources.Resource.WeeklyOn %></span>&nbsp;*</asp:Label>
                                <select id="ddlWeekly" multiple runat="server" class="form_input multipleSelect" data-validate="required">
                                    <option value="Monday" title="<%$Resources:Resource, Monday %>"></option>
                                    <option value="Tuesday" title="<%$Resources:Resource,  Tuesday%>"></option>
                                    <option value="Wednesday" title="<%$Resources:Resource, Wednesday %>"></option>
                                    <option value="Thursday" title="<%$Resources:Resource, Thursday %>"></option>
                                    <option value="Friday" title="<%$Resources:Resource, Friday %>"></option>
                                    <option value="Saturday" title="<%$Resources:Resource, Saturday %>"></option>
                                    <option value="Sunday" title="<%$Resources:Resource, Sunday %>"></option>
                                </select>
                            </asp:Panel>
                            <asp:Panel ID="panelMonthly" runat="server" Visible="false">
                                <asp:Label ID="Label14" runat="server" CssClass="form_label"><span><%= Resources.Resource.Monthly %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlMonthDays" runat="server" data-validate="required" CssClass="form_input"></asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="panelDayOfWeeks" runat="server" Visible="false">
                                <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.WeeklyOn %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlDayOfWeek_Day" runat="server" CssClass="form_input" Style="width: 27%">
                                    <asp:ListItem Text="<%$Resources:Resource, First %>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Second %>" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Third %>" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Fourth %>" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource, Fifth %>" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
               
                                   <asp:DropDownList ID="ddlDayOfWeek_Week" runat="server" CssClass="form_input" Style="width: 33%" data-validate="required">
                                       <asp:ListItem Text="<%$Resources:Resource, selectdays %>" Value=""></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Monday %>" Value="1"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Tuesday %>" Value="2"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Wednesday %>" Value="3"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Thursday %>" Value="4"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Friday %>" Value="5"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Saturday %>" Value="6"></asp:ListItem>
                                       <asp:ListItem Text="<%$Resources:Resource, Sunday %>" Value="0"></asp:ListItem>
                                   </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="panelAnnually" runat="server" Visible="false">
                                <asp:Label ID="Label11" runat="server" CssClass="form_label"><span><%= Resources.Resource.Annually %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlAnnualyOn_Date" runat="server" CssClass="form_input" Style="width: 20%" data-validate="required">
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                    <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                    <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                    <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                    <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                    <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                    <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:Label ID="Label3" Text="<%$Resources:Resource, Dayof %>" runat="server" CssClass=""></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlAnnualyOn_Month" runat="server" Style="width: 25%" CssClass="form_input" data-validate="required">
                                <asp:ListItem Text="<%$Resources:Resource, Jan%>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Feb%>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Mar%>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Apr%>" Value="4"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, May%>" Value="5"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Jun%>" Value="6"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Jul%>" Value="7"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Aug%>" Value="8"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Sept%>" Value="9"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Oct%>" Value="10"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Nov%>" Value="11"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Dec%>" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            </asp:Panel>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Panel ID="panelEndsOn" runat="server" Visible="false">
                                <asp:Label ID="lblEndsOn" runat="server" CssClass="form_label"><span><%= Resources.Resource.EndsOn %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form_input date-picker" data-validate="required date" autocomplete="off"></asp:TextBox>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
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
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, ReminderMeBefore %>" runat="server" CssClass="form_label"></asp:Label>
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
                        <input type="hidden" id="filenames" runat="server" />
                        <div id="uploader" style="width: 100%; display: inline-block;">
                            <p>
                                <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                            </p>
                        </div>
                        <span class="rule">
                            <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max2MBpng %>" runat="server"></asp:Label></span>
                    </div>
                </div>


                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnCreateAppointment" CssClass="form_button" Text="<%$Resources:Resource, CreateAppointment %>" runat="server" OnClick="btnCreateAppointment_Click" />&nbsp;
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
                                <asp:Label ID="Label16" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                                <asp:TextBox ID="txtModalFirstName" runat="server" CssClass="form_input modal_element" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="Label17" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
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
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>

    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
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

        var temp;
        $("#uploader").pluploadQueue({
            runtimes: 'html5,flash,browserplus,silverlight,gears',
            url: 'file_upload_docs.ashx',
            max_files: 5,
            rename: true,
            dragdrop: true,

            filters: {
                max_file_size: '2mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
                    { title: "PDF files", extensions: "pdf" },
                ]
            },

            flash_swf_url: 'js/plupload/Moxie.swf',
            silverlight_xap_url: 'js/plupload/Moxie.xap',
            init: {
                FileUploaded: function (up, file, info) {
                    var val = $("#ContentPlaceHolder1_filenames").val();
                    temp = val;
                    if (val.length > 0) {
                        temp += ",";
                    }
                    temp += info.response;
                    $("#ContentPlaceHolder1_filenames").empty();
                    $("#ContentPlaceHolder1_filenames").val(temp);
                }
            },
        });




    </script>
</asp:Content>
