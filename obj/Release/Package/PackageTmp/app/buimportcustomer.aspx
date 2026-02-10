<%@ Page Title="Import Customers" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="buimportcustomer.aspx.cs" Inherits="Breederapp.buimportcustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        .resultdiv {
            background-color: #eaeaea;
            padding: 5px 10px;
            border-radius: 4px;
        }

        #ContentPlaceHolder1_gridResult tr td, #ContentPlaceHolder1_gridResult tr th {
            border: none;
        }

            #ContentPlaceHolder1_gridResult tr td a {
                color: #333;
            }

        .form-group {
            margin-bottom: 40px;
        }

        .form_input1 {
            width: 30% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"><div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="Import Customers" runat="server" CssClass="error_class"></asp:Label></h5>
        <br />
        <div class="login_form">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
            <div class="row form-group">
                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                    <asp:Label ID="lblLeadSoucre1" Text="<%$Resources:Resource, Downloadtemplate %>" runat="server"></asp:Label>
                    <span>:</span>
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">                  
                    Download the template on your computer to create the import file. Following columns are mandatory --- First & Last Name, Email Address,Gender,Mobile
                    <br />
                    <a href="docs/files/template_bu_customers.xlsx" download="template_bu_customers.xlsx">
                        <asp:Label ID="Label5" Text="<%$Resources:Resource, Downloadtemplate %>" runat="server"></asp:Label></a>
                </div>
            </div>

            <div class="row form-group">
                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                    <asp:Label ID="Label1" Text="<%$Resources:Resource, UploadDocument %>" runat="server"></asp:Label>
                    <span>:</span>
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                    <asp:FileUpload ID="fileUpload" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" multiple="false" runat="server" data-buttontext="Browse file" />
                </div>
            </div>
            <div class="row form-group">
                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, Handleduplicaterecords %>" runat="server"></asp:Label>
                    <span>:</span>
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                    <asp:DropDownList ID="ddlDuplicateAction" runat="server" CssClass="form_input form_input1">
                        <asp:ListItem Text="<%$Resources:Resource, Skip %>" Value="skip"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Overwrite %>" Value="overwrite"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row form-group">
                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                    <asp:Label ID="Label3" Text="<%$Resources:Resource, ColumnToCheckDuplicateRecords %>" runat="server"></asp:Label>
                    <span>:</span>
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                    <asp:DropDownList ID="ddlDuplicateColumn" runat="server" CssClass="form_input form_input1">
                        <asp:ListItem Text="<%$Resources:Resource, EmailAddress %>" Value="email"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row form_action">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" OnClick="btnSubmit_Click" />&nbsp;
                        <asp:LinkButton ID="btnBack1" runat="server" Text="<%$Resources:Resource, Back %>" OnClick="btnBack_Click"></asp:LinkButton>
                </div>
            </div>

            <asp:Panel ID="panelResult" runat="server" Visible="false">
                <br />
                <br />
                <div class="resultdiv">
                    <h5>
                        <asp:Label ID="lblResultSummary" runat="server"></asp:Label></h5>
                    <div>
                        <asp:GridView ID="gridResult" runat="server" BorderWidth="0" BorderStyle="None" ShowHeader="false" CssClass="table datatable" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="30%" HeaderText="Customer" HeaderStyle-CssClass="header" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <a href='<%# string.Format("customerview.aspx?id={0}", Eval("customerid")) %>'><%# string.Format("[Row:{0}]&nbsp;{1}",Eval("rowindex"), Eval("customername")) %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="action" HeaderText="Action" ItemStyle-Width="20%" HeaderStyle-CssClass="header" HeaderStyle-Font-Bold="true" />
                                <asp:BoundField DataField="message" HeaderText="Message" ItemStyle-Width="50%" HeaderStyle-CssClass="header" HeaderStyle-Font-Bold="true" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
    </script>
</asp:Content>

