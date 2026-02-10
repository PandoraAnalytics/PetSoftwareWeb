<%@ Page Title="Manage Checklist" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="checklistmanage.aspx.cs" Inherits="Breederapp.checklistmanage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/fastselect.css" rel="stylesheet" />
    <style>
        .form_input {
            width: 65%;
        }

        .fstElement {
            width: 65%;
        }

        .form_input1 {
            width: 30% !important;
        }

        .members li {
            width: 33.33%;
            float: left;
            margin: 10px 0;
            list-style: none;
        }

        .linkicon .linkicon:hover {
            color: #d9082d;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label2" Text="<%$Resources:Resource, ManageChecklist %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <div class="row form_row">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form_input form_input1" data-validate="required" MaxLength="100"></asp:TextBox>
            </div>

        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Category %></span>&nbsp;*</asp:Label>
                <input type="text" list="ContentPlaceHolder1_datalist" class="form_input form_input1" runat="server" id="txtCategoryName" data-validate="required" maxlength="50" autocomplete="off" />
                <datalist runat="server" id="datalist"></datalist>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="Label12" Text="<%$Resources:Resource, ResponseType %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:DropDownList ID="ddlResponseType" runat="server" CssClass="form_input form_input1">
                    <asp:ListItem Text="<%$Resources:Resource, Public %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Resource, Private %>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>&nbsp;
                <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, SaveNext %>" runat="server" OnClick="btnSave_Click" />
            </div>
        </div>
        <asp:Panel ID="panelManageSchedule" runat="server" Visible="false">
            <br />
            <br />
            <h5>
                <asp:Label ID="Label3" Text="<%$Resources:Resource, Schedule %>" runat="server" CssClass="error_class"></asp:Label></h5>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblScheduleText" Text="-" runat="server"></asp:Label>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:LinkButton ID="lnkManageSchedule" runat="server" CssClass="edit_profile_link" OnClick="lnkManageSchedule_Click">
                        +&nbsp;<asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource, ManageSchedule %>"></asp:Label>
                    </asp:LinkButton>
                </div>
            </div>
            <asp:Panel ID="panelManageScheduleEdit" runat="server" Visible="false">
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
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource, Daily%>" CssClass="form_label"></asp:Label>
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
                                <asp:Label ID="Label9" runat="server" CssClass="form_label"><span><%= Resources.Resource.WeeklyOn %></span>&nbsp;*</asp:Label>
                                <asp:DropDownList ID="ddlDayOfWeek_Day" runat="server" CssClass="form_input" Style="width: 27%" data-validate="required">
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
                                &nbsp;<asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource, Dayof %>" CssClass=""></asp:Label>&nbsp;
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
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSaveSchedule" CssClass="form_button" Text="<%$Resources:Resource, SaveSchedule %>" runat="server" OnClick="btnSaveSchedule_Click" />&nbsp;
                        &nbsp;
                            <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panelMembers" runat="server" Visible="false">
            <br />
            <br />
            <h5>
                <asp:Label ID="Label8" Text="<%$Resources:Resource, Members %>" runat="server" CssClass="error_class"></asp:Label></h5>
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="members">
                        <asp:Repeater ID="repeaterMembers" runat="server">
                            <HeaderTemplate>
                                <li>
                                    <div>
                                        <a href='<%# string.Format("checklistassingusers.aspx?id={0}", BABusiness.BusinessSecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey)) %>' style="color: #777;">
                                            <i class="fa-solid fa-circle-plus"></i>&nbsp;&nbsp;
                                        <asp:Label ID="Label1" Text="<%$Resources:Resource, AddNewMember %>" runat="server"></asp:Label>
                                        </a>
                                        &nbsp;&nbsp;
                                    </div>
                                    <span style="opacity: 0.7; margin-top: 3px;">
                                        <%# Eval("email") %> 
                                    </span>
                                </li>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <div>
                                        <a href="javascript:void(0);" onclick='<%# string.Format("deleteRecord({0});", Eval("id")) %>'>
                                            <i class="fa-solid fa-circle-minus"></i>
                                        </a>
                                        &nbsp;&nbsp;
                                    <%# Eval("name") %>
                                    </div>
                                    <span style="opacity: 0.7; margin-top: 3px;">
                                        <%# Eval("email") %> 
                                    </span>
                                </li>
                            </ItemTemplate>

                        </asp:Repeater>
                    </ul>
                    <div class="clearfix"></div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            $('.multipleSelect').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
                 noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
             });
        });

        function deleteRecord(id) {
            deleteData("RemoveChecklistUser", id);
            var u = window.location.href;
            window.location = u;
        }

    </script>
</asp:Content>
