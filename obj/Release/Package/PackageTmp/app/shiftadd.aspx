<%@ Page Title="Manage Shift" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="shiftadd.aspx.cs" Inherits="Breederapp.shiftadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/bootstrap-timepicker.min.css" rel="stylesheet" />
    <style>
        .form_input {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="form_horizontal">
            <div class="row">
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                    <h5 class="page_title">
                        <asp:Label ID="lblHeading" Text="Manage Shift" CssClass="error_class" runat="server"></asp:Label></h5>
                </div>
            </div>
            <div class="tab-content clearfix" id="pills-tabContent">
                <!------------ tab_1 ------------>
                <div class="tab-pane fade show active" role="tabpanel" runat="server" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0">
                    <asp:Panel ID="panelAction" runat="server">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                <asp:Button ID="lnkEdit1" class="form_button" Text="Edit Shift" runat="server" OnClick="lnkEdit_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack1_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="panelView" runat="server">
                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label1" runat="server" CssClass="form_label" Text="Name"></asp:Label>
                                        <asp:Label ID="lblName" Text="-" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label15" Text="Break Time Duration" runat="server" CssClass="form_label"></asp:Label>
                                        <asp:Label ID="lblbreaktimeduration" Text="-" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label14" Text="Status" runat="server" CssClass="form_label"></asp:Label>
                                        <asp:Label ID="lblStatus" Text="-" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label11" Text="Type" runat="server" CssClass="form_label"></asp:Label>
                                        <asp:Label ID="lblType" Text="-" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label12" Text="Start Time" runat="server" CssClass="form_label"></asp:Label>
                                        <asp:Label ID="lblStartTime" Text="-" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label9" Text="End Time" runat="server" CssClass="form_label"></asp:Label>
                                        <asp:Label ID="lblEndTime" Text="-" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="panelEdit" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span>Name</span>&nbsp;*</asp:Label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="80" data-validate="required"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label13" runat="server" CssClass="form_label"><span>Break Time Duration</span>&nbsp;*</asp:Label>
                                        <asp:TextBox ID="txtBreakTimeDuration" runat="server" CssClass="form_input" MaxLength="50" data-validate="required pnumber"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label7" runat="server" CssClass="form_label"><span>Type</span>&nbsp;*</asp:Label>
                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required">
                                            <asp:ListItem Text="Night" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Day" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label10" runat="server" CssClass="form_label"><span>Status</span>&nbsp;*</asp:Label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_input" data-validate="required">
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form_row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.StartTime %></span>&nbsp;*</asp:Label>
                                        <asp:DropDownList ID="ddlStartTime" runat="server" CssClass="form_input" data-validate="required"></asp:DropDownList>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                        <asp:Label ID="Label8" runat="server" CssClass="form_label"><span>End Time</span>&nbsp;*</asp:Label>
                                        <asp:DropDownList ID="ddlEndTime" runat="server" CssClass="form_input" data-validate="required"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form_row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Button ID="btnSave" CssClass="form_button" Text="Save" runat="server" OnClick="btnSave_Click" />&nbsp;                                     
                                        <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Back"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/bootstrap-timepicker.min.js"></script>
    <script>        
        $(document).ready(function () {
            $(".time-picker").timepicker({ showInputs: false, showMeridian: false });
        });
    </script>
</asp:Content>
