<%@ Page Title="Edit Member" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="editmember.aspx.cs" Inherits="Breederapp.editmember" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="css/datepicker3.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .form_input {
            width: 70%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AddMember %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblMemberNo" runat="server" CssClass="form_label"><span><%= Resources.Resource.MemberNo %></span>&nbsp;*</asp:Label>
                       <asp:TextBox ID="txtMemberNo2" runat="server" CssClass="form_input" data-validate="required" MaxLength="20"></asp:TextBox>

                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.FirstName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.LastName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAddress" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" MaxLength="200" ></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblCity" runat="server" CssClass="form_label"><span><%= Resources.Resource.City %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblvCountry" runat="server" CssClass="form_label"><span><%= Resources.Resource.Country %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form_input" MaxLength="200" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblZipCode" runat="server" CssClass="form_label"><span><%= Resources.Resource.Postcode %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="form_input" data-validate="required pnumber" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblEmailAddress" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form_input email"  data-validate="required email" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblMobile" runat="server" CssClass="form_label"><span><%= Resources.Resource.Mobile %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form_input phone" data-validate="required pnumber" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblFax" Text="<%$Resources:Resource, Fax %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtFax" runat="server" CssClass="form_input" MaxLength="20"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblEntryDate" runat="server" CssClass="form_label"><span><%= Resources.Resource.EntryDate %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEntryDate" runat="server" CssClass="form_input date-picker" data-validate="required date" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblExitDate" Text="<%$Resources:Resource, ExitDate %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtExitDate" runat="server" CssClass="form_input date-picker"  autocomplete="off"></asp:TextBox>
                          <asp:Label ID="lblExitDateMsg" runat="server" CssClass="error_class"></asp:Label>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblExitReason" Text="<%$Resources:Resource, ExitReason %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtExitReason" runat="server" CssClass="form_input" MaxLength="200"></asp:TextBox>
                         <asp:Label ID="lblExitReasonMsg" runat="server" CssClass="error_class"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblMembershipType" runat="server" CssClass="form_label"><span><%= Resources.Resource.MembershipType %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtMembershipType" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblfamilyfullmember" Text="<%$Resources:Resource, familyfullmember %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtfamilyfullmember" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblPositioninRegion" Text="<%$Resources:Resource, PositioninRegion %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtPositioninRegion" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblRegion" Text="<%$Resources:Resource, Region %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtRegion" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblIsBREEDERYesorNo" Text="<%$Resources:Resource, IsBreeder %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlIsBREEDERYesorNo" runat="server" CssClass="form_input">
                            <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblfamilymemberof" Text="<%$Resources:Resource, familymemberof %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtfamilymemberof" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblRemark" Text="<%$Resources:Resource, Remark %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblpaymentmethod" Text="<%$Resources:Resource, paymentmethod %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtpaymentmethod" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAccountnumber" Text="<%$Resources:Resource, Accountnumber %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAccountnumber" runat="server" CssClass="form_input" MaxLength="30" data-validate="pnumber"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAccountownerName" Text="<%$Resources:Resource, AccountownerName %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAccountownerName" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblBankName" Text="<%$Resources:Resource, BankName %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblBankcode" Text="<%$Resources:Resource, Bankcode %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtBankcode" runat="server" CssClass="form_input" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblBreedtype" Text="<%$Resources:Resource, Breedtype %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtBreedtype" runat="server" CssClass="form_input" MaxLength="50" data-validate="required"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAnimalName" runat="server" CssClass="form_label"><span><%= Resources.Resource.AnimalName %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtAnimalName" runat="server" CssClass="form_input" MaxLength="100" data-validate="required"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblAnimalBirthdate" Text="<%$Resources:Resource, AnimalBirthdate %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtAnimalBirthdate" runat="server" CssClass="form_input date-picker" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblCollarId" Text="<%$Resources:Resource, CollarId %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtCollarId" runat="server" CssClass="form_input"  MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblPhone" Text="<%$Resources:Resource, Phone %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" CssClass="form_input phone" data-validate="pnumber"></asp:TextBox>

                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        <asp:Label ID="lblStreet" Text="<%$Resources:Resource, Street %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtStreet" runat="server" CssClass="form_input" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnCancel_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>

    <script>
        $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
    </script>
</asp:Content>
