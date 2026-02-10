<%@ Page Title="Event Other Info" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="eventviewcustomfields.aspx.cs" Inherits="Breederapp.eventviewcustomfields" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 80%;
        }

        #ContentPlaceHolder1_panelNoQuestion {
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 10px;
            margin-top: 20px;
        }

        ul.photos li {
            list-style: none;
            padding: 4px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Eventotherinfo %>" runat="server" CssClass="error_class"></asp:Label></h5>
         <div class="float-end">
            <a href="eventslist.aspx" class="edit_profile_link">
                <asp:Label ID="Label3" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label>
            </a>
        </div>
        <div class="clearfix"></div>
        <br />
        <div class="row">

            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label11" Text="<%$Resources:Resource, OtherFields %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <table class="table datatable" id="otherinfotable" style="width: 90% !important;">
                    <thead>
                        <tr>
                            <td width="100%" colspan="2">
                                <a href="customfieldsadd.aspx" class="edit_profile_link">+&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label></a>
                            </td>
                        </tr>
                    </thead>
                    <tbody style="display: block; height: 350px; overflow: auto;">
                    </tbody>
                </table>
                <div class="dt_footer" style="width: 90% !important;">
                </div>
                <br />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnAssignOtherInfo" runat="server" Text="<%$Resources:Resource, Assigntoevent %>" CssClass="form_button top_btn" OnClick="btnAssignOtherInfo_Click" validate="no" />

                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <h6>
                    <asp:Label ID="lblTitle" runat="server" CssClass="error_class"></asp:Label></h6>
                <div class="main_container">
                    <div class="row form_row">
                        <asp:Panel ID="panelNoQuestion" runat="server">
                            <asp:Label ID="lbldropithere" Text="<%$Resources:Resource, Selectotherfieldsfromlefthandsidetoassignwiththisevent %>" runat="server"></asp:Label>
                        </asp:Panel>

                        <asp:Panel ID="panelQuestions" runat="server">
                            <ul class="photos">
                                <asp:Repeater ID="repPhotos" runat="server" OnItemCommand="repPhotos_ItemCommand">
                                    <ItemTemplate>
                                        <li>
                                            <span>
                                                <%# Eval("title") %>
                                            </span>
                                            &nbsp;
                                            <asp:LinkButton ID="lnkdelete" runat="server" CssClass="cross" CommandName="delete" CommandArgument='<%# Eval("id") %>' data-toggle='tooltip' data-placement='top' data-original-title='Delete' OnClientClick="return confirm_delete()" Text="<i class='fa fa-times'></i>"></asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="col-lg-1 col-md-1 col-sm-1 col-xs-12">
            </div>
            <div class="col-lg-3 col-md-3 col-sm-5 col-xs-12">
                <input type="hidden" id="cplist" runat="server" />
                 <input type="hidden" id="cplist1" runat="server" />
                <h6>
                    <asp:Label ID="Label1" Text="<%$Resources:Resource, AddOtherEdit %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <asp:Panel ID="panelOtherInfo" runat="server">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Type %></span>&nbsp;*</asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtQTitle" CssClass="form_input" runat="server" data-validate="required" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label8" Text="<%$Resources:Resource, IsMandatory %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:DropDownList ID="ddlIsMandatory" runat="server" CssClass="form_input">
                                <asp:ListItem Text="<%$Resources:Resource, No %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:Panel ID="panelList" runat="server" Visible="false">
                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption1" Text="<%$Resources:Resource, Option1 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal1" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption2" Text="<%$Resources:Resource, Option2 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal2" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption3" Text="<%$Resources:Resource, Option3 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal3" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption4" Text="<%$Resources:Resource, Option4 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal4" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption5" Text="<%$Resources:Resource, Option5 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal5" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption6" Text="<%$Resources:Resource, Option6 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal6" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption7" Text="<%$Resources:Resource, Option7 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal7" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:Label ID="lblOption8" Text="<%$Resources:Resource, Option8 %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtVal8" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                     <asp:Panel ID="panelRange" runat="server" Visible="false">
                        <div class="row form_row">
                            <div class="form_label col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="lblMinValue" CssClass="form_label" runat="server"><span><%= Resources.Resource.MinValue %></span>&nbsp;* </asp:Label>
                                <asp:TextBox ID="txtRangeMinVal" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row form_row">
                            <div class="form_label col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="lblMaxValue" CssClass="form_label" runat="server"><span><%= Resources.Resource.MaxValue %></span>&nbsp;* </asp:Label>
                                <asp:TextBox ID="txtRangeMaxVal" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="panelMatrix" runat="server" Visible="false">
                        <div class="row form_row">
                            <div class=" form_label col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.NumberofRows %></span>&nbsp;* 
                                </asp:Label>
                                <asp:TextBox ID="txtRowsValue" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label9" runat="server" CssClass="form_label"><span><%= Resources.Resource.ColumnName %></span>&nbsp;* 
                                </asp:Label>
                                <asp:TextBox ID="txtColumnVal1" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>&nbsp;
                                                <asp:Button ID="btnAddColumn" runat="server" Text="Add More Column" CssClass="form_button top_btn" validate="no" OnClick="btnAddTxtBox_Click" />
                            </div>
                        </div>

                        <asp:Panel ID="pnlMatrixTextBoxes" runat="server">
                        </asp:Panel>

                    </asp:Panel>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSaveOtherInfo" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button top_btn" OnClick="btnSubmit_Click" />

                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>


    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js"></script>
    <script>
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            getOtherInfo();
        });

        function getOtherInfo() {
            var obj = {};
            obj['iseventtype'] = 1;
            obj['associationbreedertype'] = '';
            filter = JSON.stringify(obj);

            $('#otherinfotable tbody').html('');
            getdata3("GetAllCustomFields", 1, filter, '', GetAllCustomFields);
        }

        function GetAllCustomFields(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var array = $("#ContentPlaceHolder1_cplist1").val().split(';');
                records.each(function () {
                    var record = $(this);
                    var id1 = record.find("id").text();
                    var typeValue = '';

                    switch (record.find("type").text()) {
                        case '1':
                            typeValue = 'One Line';
                            break;
                        case '2':
                            typeValue = 'Paragraph'
                            break;
                        case '3':
                            typeValue = 'List'
                            break;
                        case '4':
                            typeValue = 'Single-Select'
                            break;
                        case '5':
                            typeValue = 'Multi-Select'
                            break;
                        case '6':
                            typeValue = 'File Upload'
                            break;
                        case '7':
                            typeValue = 'Range'
                            break;
                        case '8':
                            typeValue = 'Matrix'
                            break;
                        case '9':
                            typeValue = 'Date'
                            break;
                        case '10':
                            typeValue = 'Time'
                            break;
                    }

                    

                    var contents = '<tr>';
                    var inArray = jQuery.inArray(id1, array);
                    if (inArray >= 0) contents += '<td width="5%" valign="top"><input type="checkbox" disabled checked></td>';
                    else contents += '<td width="5%" valign="top"><input type="checkbox" class="call-checkbox" value="' + id1 + '"></td>';
                    contents += '<td width="95%"><div>' + record.find("title").text() + '<div style="margin-top:5px;opacity:0.8">' + typeValue + '</div></div></td>';
                    contents += '</tr>';

                    $('#otherinfotable > tbody:last').append(contents);
                });
            }
            else {
                $('#otherinfotable tbody').html("<tr><td colspan='2'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function confirm_delete() {
            var confirm_msg = '<%=  Resources.Resource.DeleteRecordConfirmationMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        }

        $(document).on('change', '.call-checkbox', function (e) {
            var listArray = $("#ContentPlaceHolder1_cplist").val().split(';');

            if ($(this).is(':checked')) {
                listArray.push($(this).val());
            }
            else {
                listArray.remove($(this).val());
            }
            $("#ContentPlaceHolder1_cplist").val(listArray.join(';'));
        });

        Array.prototype.remove = function (v) { this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1); }
    </script>
</asp:Content>
