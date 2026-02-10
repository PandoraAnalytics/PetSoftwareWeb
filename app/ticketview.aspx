<%@ Page Title="Ticket View" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="ticketview.aspx.cs" Inherits="Breederapp.ticketview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .flags_bug {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #FF0000;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .flags_voneeded {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #90EE90;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .flags_feature {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #FFFF00;
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 11px;
            font-weight: bold;
        }

        .file_rule {
            color: #19c5d4;
            font-size: 13px;
            margin-top: 5px;
            display: block;
        }

        .top_btn {
            margin-right: 5px;
        }

        #ContentPlaceHolder1_btnTesting, #ContentPlaceHolder1_btnTesting:hover, #ContentPlaceHolder1_btnTesting:focus, #ContentPlaceHolder1_btnTesting:visited {
            /* color: #fff !important;*/
            font-size: 15px;
            text-decoration: none;
        }

        /* #btnTesting ~ .dropdown-menu {
            left: 4px;
        }*/



        #btnTesting ~ .dropdown-menu li {
            margin: 5px 0;
        }

        .dropdown-item {
            font-size: 13px !important;
        }

        .radio-toolbar label {
            margin-left: 5px;
        }

        #custom_alert {
            display: none;
        }

            #custom_alert .cus-overlay {
                position: fixed;
                top: 0px;
                right: 0px;
                left: 0px;
                height: 100%;
                width: 100%;
                background-color: #fff;
                opacity: 0.4;
                z-index: 10000;
            }

            #custom_alert .cus-dialog {
                width: 480px;
                margin-left: -240px;
                min-height: 100px;
                position: fixed;
                top: 28%;
                left: 50%;
                z-index: 10001;
                background-color: #fff;
                opacity: 1;
                text-align: center;
                padding: 15px;
                border-radius: 5px;
                border: 1px solid #ddd;
            }

            #custom_alert .message {
                margin: 10px 0 30px 0;
            }

            #custom_alert .yes {
                color: #ffffff;
                cursor: pointer;
                outline: none;
                background: #0265b3;
                border-radius: 5px;
                border: 1px solid #0265b3;
                min-width: 60px;
                height: 30px;
                display: inline-block;
                text-align: center;
                line-height: 30px;
            }

            #custom_alert .no {
                color: #333;
                cursor: pointer;
                outline: none;
                background: #ddd;
                border: 1px solid #efefef;
                border-radius: 5px;
                min-width: 60px;
                height: 30px;
                display: inline-block;
                text-align: center;
                line-height: 30px;
            }

        .nav-pills .nav-link.active, .nav-pills .show > .nav-link {
            /*color: var(--bs-nav-pills-link-active-color) !important;*/
            /*background-color: var(--bs-nav-pills-link-active-bg);*/
            background-color: #f28c0e;
        }

        .nav-pills .nav-link {
            border-radius: 0 !important;
        }

        .nav-link:focus, .nav-link:hover {
            color: #ffffff !important;
        }

        .nav-link {
            color: #ffffff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                <h5 class="page_title">
                    <asp:Label ID="lblHeading" Text="<%$Resources:Resource, TicketView %>" CssClass="error_class" runat="server"></asp:Label></h5>
            </div>
        </div>
        <br />
        <div class="dt_tab_pages">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                        <li id="tablnkBasic" class="nav-item" role="presentation">
                            <button class="nav-link active" id="basic_details-tab" data-bs-toggle="pill" data-bs-target="#tab_basic_details" type="button" role="tab" aria-controls="tab_basic_details"
                                aria-selected="true">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, BasicDetails %>"></asp:Label></button>
                        </li>

                        <li id="tablnkComment" class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-comment-tab" data-bs-toggle="pill" data-bs-target="#tab_comments" type="button" role="tab" aria-controls="tab_comments"
                                aria-selected="false" onclick="commentClicked();">
                                <asp:Label ID="lblComments" runat="server" Text="<%$Resources:Resource, Comments %>"></asp:Label></button>
                        </li>

                        <li id="tablnkDocument" class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-document-tab" data-bs-toggle="pill" data-bs-target="#tab_documents" type="button" role="tab" aria-controls="tab_documents"
                                aria-selected="false" onclick="documentClicked();">
                                <asp:Label ID="Label18" runat="server" Text="<%$Resources:Resource, Documents %>"></asp:Label></button>
                        </li>

                        <li id="tablnkLogs" class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-log-tab" data-bs-toggle="pill" data-bs-target="#tab_logs" type="button" role="tab" aria-controls="tab_logs"
                                aria-selected="false" onclick="LogsClicked();">
                                <asp:Label ID="Label19" runat="server" Text="<%$Resources:Resource, Logs %>"></asp:Label></button>

                        </li>
                    </ul>

                    <div class="tab-content clearfix" id="pills-tabContent">
                        <!------------ tab_1 ------------>
                        <div class="tab-pane fade show active" role="tabpanel" id="tab_basic_details" aria-labelledby="basic_details-tab" tabindex="0">
                            <asp:Panel ID="panelAction" runat="server">

                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                        <asp:Button ID="btnEdit2" runat="server" Text="<%$Resources:Resource, EditTicket %>" CssClass="form_button" OnClick="btnEdit2_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnOpenTicket" runat="server" Text="<%$Resources:Resource, OpenTicket %>" CssClass="form_button" validate="no" OnClientClick="return openticketmodal();return false;" Visible="false" />
                                        <asp:Button ID="btnDeferred" runat="server" Text="<%$Resources:Resource, Defer %>" CssClass="form_button" OnClick="btnDeferred_Click" validate="no" Visible="false" />
                                        <%--  <asp:Button ID="btnApprovedfordevelopment" runat="server" Text="<%$Resources:Resource, Approvefordevelopment %>" CssClass="form_button" OnClick="btnApprovedfordevelopment_Click" validate="tab" Visible="false" />--%>
                                        <asp:Button ID="btnApprovedfordevelopment" runat="server" Text="<%$Resources:Resource, Approvefordevelopment %>" CssClass="form_button" ToolTip="You can't approve the ticket as approvals are pending from L2/L3" OnClick="btnApprovedfordevelopment_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnApprovedfordevelopmentVO" runat="server" Text="<%$Resources:Resource, Approvefordevelopment %>" CssClass="form_button" validate="no" Visible="false" />
                                        <asp:Button ID="btnApprovedfordevelopmentVO2" runat="server" Text="<%$Resources:Resource, Approvefordevelopment %>" CssClass="form_button" Enabled="false" Visible="false" />
                                        <asp:Button ID="btnDevelopment" runat="server" Text="<%$Resources:Resource, Development %>" CssClass="form_button" OnClick="btnDevelopment_Click" validate="tab" Visible="false" />
                                        <div style="position: relative; display: inline-block;">
                                            <a id="btnTesting" runat="server" visible="false" href="javascript:void(0);" class="form_submit_btn submit_btn_sm topbtn dropdown-toggle top_btn" data-bs-toggle="dropdown">
                                                <asp:Label ID="Label12" Text="<%$Resources:Resource, Markastesting %>" runat="server"></asp:Label>
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="<%$Resources:Resource, LiveEnvironment %>" class="dropdown-item" OnCommand="btnMarkastesting_Command" CommandArgument="Live"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" Text="<%$Resources:Resource, DevEnvironment %>" class="dropdown-item" OnCommand="btnMarkastesting_Command" CommandArgument="Dev"></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>

                                        <asp:Button ID="btnApproved" runat="server" Text="<%$Resources:Resource, Approve %>" CssClass="form_button" OnClick="btnApproved_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnReject" runat="server" Text="<%$Resources:Resource, Reject %>" CssClass="form_button" validate="tab" Visible="false" />
                                        <asp:Button ID="btnRolledout" runat="server" Text="<%$Resources:Resource, Rollout %>" CssClass="form_button" OnClick="btnRolledout_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnClosed" runat="server" Text="<%$Resources:Resource, Close %>" CssClass="form_button" OnClick="btnClosed_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnEstimateEDT" runat="server" Text="<%$Resources:Resource, EstimateEDT %>" CssClass="form_button" OnClick="btnEstimateEDT_Click" validate="tab" Visible="false" />
                                        <asp:Button ID="btnWaitingfordevelopmentapproval" runat="server" Text="<%$Resources:Resource, WaitingforDevelopmentApproval %>" CssClass="form_button" OnClick="btnWaitingfordevelopmentapproval_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnApproveRollout" runat="server" Text="<%$Resources:Resource, ApproveRolledout %>" CssClass="form_button" OnClick="btnApproveRollout_Click" validate="no" Visible="false" />
                                        <asp:Button ID="btnPDF" runat="server" Text="<%$Resources:Resource, PDFReport %>" CssClass="form_button" OnClick="btnGenerateReport_Click" validate="no" />
                                    </div>
                                </div>
                                <br />
                            </asp:Panel>
                            <div class="form_horizontal basic_deatils_form" style="line-height: 25px;">
                                <div class="row">
                                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                                    <div class="col-lg-9 col-md-9 col-sm-6 col-xs-12">
                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblTicketNo1" Text="<%$Resources:Resource, TicketNo %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblTicketNo" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblCreated1" Text="<%$Resources:Resource, Created %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblCreated" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblUpdated1" Text="<%$Resources:Resource, Updated %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblUpdated" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblCurrentStatus2" Text="<%$Resources:Resource, CurrentStatus %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblCurrentStatus" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblApplication1" Text="<%$Resources:Resource, Application %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblApplication" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblHeader1" Text="<%$Resources:Resource, Header %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblDescription1" Text="<%$Resources:Resource, Description %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblEDT1" Text="<%$Resources:Resource, EDT %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblEDT" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <asp:Panel ID="panelEDTEdit" runat="server" Visible="false">
                                            <div class="row form-group">
                                                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                    <asp:Label ID="lblEDT2" Text="<%$Resources:Resource, EDT %>" runat="server"></asp:Label><span>:</span>
                                                </div>

                                                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                    <asp:TextBox ID="txtEDT" runat="server" CssClass="form_element" MaxLength="6" data-validate="pnumber"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="panelCompletionDate" runat="server" Visible="false">
                                            <div class="row form-group">
                                                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                    <asp:Label ID="Label7" Text="<%$Resources:Resource, CompletionDate %>" runat="server"></asp:Label><span>:</span>
                                                </div>

                                                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                    <asp:TextBox ID="txtCompletionDate" runat="server" CssClass="form_element date-picker" autocomplete="off" MaxLength="10" data-validate="required date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md- col-sm-12 col-xs-12">
                                                <asp:Label ID="lblPriority1" Text="<%$Resources:Resource, Priority %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblPriority" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row form-group">
                                            <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="Label10" Text="<%$Resources:Resource, VONeeded %>" runat="server"></asp:Label><span>:</span>
                                            </div>

                                            <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblVONeeded" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <asp:Panel ID="panelOptionalEmail" runat="server" Visible="false">
                                            <div class="row form-group">
                                                <div class="form_label col-lg-3 col-md-3 col-sm-12 col-xs-12">
                                                    <asp:Label ID="Label17" Text="<%$Resources:Resource, AdditionalEmailAddresses %>" runat="server"></asp:Label>
                                                    <span>:</span>
                                                </div>

                                                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12">
                                                    <div style="word-break: break-all;">
                                                        <asp:Label ID="lblOptionalEmails" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="row form-group">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <a href="ticketlist.aspx">
                                                    <asp:Label ID="lblBack" Text="<%$Resources:Resource, Back %>" runat="server"></asp:Label></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                        <asp:Literal ID="lit1" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_2 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_comments" aria-labelledby="pills-comment-tab" tabindex="0">
                            <div class="form_horizontal comment_form">
                                <asp:Label ID="lblcommentloading" Text="Loading" runat="server"></asp:Label>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                        <div class="float-end" style="margin-bottom: 10px;">
                                            <a href="javascript:void(0);" id="lnkAddComments" runat="server" class="add_form_btn btnborder" data-bs-toggle="modal" data-bs-target="#manage_log" data-bs-backdrop="static" data-keyboard="false">+&nbsp;<asp:Label ID="lblAddComment" Text="<%$Resources:Resource, AddComment %>" runat="server"></asp:Label></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="table-responsive dt_container">
                                    <table class="table datatable" id="commenttable">
                                        <thead>
                                            <th width="20%">
                                                <asp:Label ID="lblAuthor" Text="<%$Resources:Resource, Author %>" runat="server"></asp:Label></th>
                                            <th width="10%">
                                                <asp:Label ID="lblDate" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                                            <th width="50%">
                                                <asp:Label ID="lblComment" Text="<%$Resources:Resource, Comment %>" runat="server"></asp:Label></th>
                                            <th width="10%">&nbsp;</th>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                    <div class="dt_footer">
                                        <ul class="pagination pull-right" id="commentpagination">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_3 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_documents" aria-labelledby="pills-document-tab" tabindex="0">
                            <div class="form_horizontal document_form">
                                <asp:Label ID="lbldocumentloading" Text="Loading" runat="server"></asp:Label>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                                        <div class="float-end" style="margin-bottom: 10px;">
                                            <a href="javascript:void(0);" id="lnkAddDocuments" class="add_form_btn btnborder" runat="server" onclick="openDocument();">+&nbsp;<asp:Label ID="Label3" Text="<%$Resources:Resource, AddDocument %>" runat="server"></asp:Label></a>

                                        </div>
                                    </div>
                                </div>

                                <div class="table-responsive dt_container">
                                    <table class="table datatable" id="documenttable">
                                        <thead>
                                            <th width="20%">
                                                <asp:Label ID="Label4" Text="<%$Resources:Resource, Author %>" runat="server"></asp:Label></th>
                                            <th width="10%">
                                                <asp:Label ID="Label5" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                                            <th width="50%">
                                                <asp:Label ID="Label6" Text="<%$Resources:Resource, Comment %>" runat="server"></asp:Label></th>
                                            <th width="20%">&nbsp;</th>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                    <div class="dt_footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------ tab_4 ------------>
                        <div class="tab-pane fade" role="tabpanel" id="tab_logs" aria-labelledby="pills-log-tab" tabindex="0">
                            <div class="form_horizontal work_desc_form">
                                <div class="row form-group">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:Label ID="lbllogloading" Text="Loading" runat="server"></asp:Label>
                                        <div class="table-responsive dt_container">
                                            <table class="table datatable" id="logtable">
                                                <thead>
                                                    <th width="20%">
                                                        <asp:Label ID="Label38" Text="<%$Resources:Resource, CreatedAt %>" runat="server"></asp:Label></th>
                                                    <th width="50%">
                                                        <asp:Label ID="Label39" Text="<%$Resources:Resource, Action %>" runat="server"></asp:Label></th>
                                                    <th width="30%">
                                                        <asp:Label ID="Label40" Text="<%$Resources:Resource, CreatedBy %>" runat="server"></asp:Label></th>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div class="dt_footer">
                                                <ul class="pagination pull-right" id="logpagination">
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="manage_log" class="modal fade">
        <div class="modal-dialog" style="width: 70%;">
            <div class="modal-content form_horizontal">
                <div class="modal-body">
                    <h6>
                        <span id="Label55" class="error_class">
                            <asp:Label ID="Label21" Text="<%$Resources:Resource, Comment %>" runat="server"></asp:Label></span></h6>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <input id="hdfilter1" runat="server" type="hidden" />
                            <asp:TextBox ID="txtComments" CssClass="form_element" runat="server" Width="100%" MaxLength="8000" data-validate="required maxlength-8000" Rows="10" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnComment" runat="server" Text="Post Comment" CssClass="form_button" OnClick="btnComment_Click" validate="no" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label20" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="manage_document" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content form_horizontal" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <asp:Label ID="Label25" class="error_class" Text="<%$Resources:Resource, UploadDocuments %>" runat="server"></asp:Label></h6>

                    <div id="panelDoc">
                        <div class="row form-group">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <input type="file" name="fileUpload" id="fileUpload" />
                                <input type="hidden" runat="server" id="hid_document" />
                                <span class="file_rule">
                                    <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max5MBpng %>" runat="server"></asp:Label></span>
                            </div>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <input id="hdfilter2" runat="server" type="hidden" />
                            <asp:TextBox ID="txtDocComments" CssClass="form_element" runat="server" Width="90%" MaxLength="200" data-validate="required maxlength-200" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnDocument" runat="server" Text="<%$Resources:Resource, UploadDocuments %>" OnClientClick="return validateDocument();" CssClass="form_button" OnClick="btnDocument_Click" validate="no" />&nbsp;
                                    <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                        <asp:Label ID="Label24" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                                    </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_voneeded" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content form_horizontal" style="margin-top: 40%;">
                <div class="modal-body">
                    <h6>
                        <span id="Label56" class="error_class">
                            <asp:Label ID="lblputthispermitonhold" Text="<%$Resources:Resource, OpenTicket %>" class="error_class" runat="server"></asp:Label></span></h6>
                    <%-- <button type="button" class="close popup_close_btn" data-dismiss="modal" aria-hidden="true">&times;</button>--%>
                    <br />
                    <br />
                    <div class="row form-group">
                        <div class="form_label col-lg-3 col-md-3 col-sm-5 col-xs-12">
                            <asp:Label ID="Label23" class="error_class" runat="server"><span><%= Resources.Resource.VONeeded %></span>&nbsp;*</asp:Label>
                        </div>

                        <div class="col-lg-9 col-md-9 col-sm-7 col-xs-12">
                            <asp:DropDownList ID="ddlVOneeded" runat="server" CssClass="form_input" Width="70%">
                                <asp:ListItem Text="<%$Resources:Resource, Select %>" Value=""></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, No %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnVONeeded" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" UseSubmitBehavior="false" validate="no" OnClick="btnVONeeded_Click" />&nbsp;
                                   <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                       <asp:Label ID="Label13" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_reject" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content form_horizontal" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6>
                        <span>
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Reject %>" class="error_class" runat="server"></asp:Label></span>
                    </h6>

                    <div class="row form-group">
                        <div class="form_label col-lg-3 col-md-3 col-sm-5 col-xs-12">
                            <asp:Label ID="Label9" Text="<%$Resources:Resource, Remark %>" runat="server"></asp:Label>
                            <span>*:</span>
                        </div>
                        <div class="col-lg-9 col-md-9 col-sm-7 col-xs-12">
                            <asp:TextBox ID="txtRejectComments" CssClass="form_element" runat="server" MaxLength="500" data-validate="maxlength-500" Rows="5" TextMode="MultiLine" Width="80%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnReject2" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" UseSubmitBehavior="true" validate="no" OnClick="btnReject2_Click" />&nbsp;
                                
                            <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" style="display: inline-block;">
                                <asp:Label ID="Label8" Text="<%$Resources:Resource, Close %>" runat="server" CssClass="form_label"></asp:Label>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_approvefordevelopmentvo" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content form_horizontal" style="margin-top: 30%;">
                <div class="modal-body">
                    <h6><span>
                        <asp:Label ID="Label14" Text="<%$Resources:Resource, Approvefordevelopment %>" class="error_class" runat="server"></asp:Label></span>
                        <%-- <button type="button" class="close popup_close_btn" data-dismiss="modal" aria-hidden="true">&times;</button>--%>
                    </h6>
                    <br />
                    <div class="row form-group">
                        <asp:Panel ID="panelL1" runat="server">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <span>
                                    <asp:Label ID="Label15" Text="<%$Resources:Resource, Areyousureperformthisaction %>" class="error_class" runat="server"></asp:Label></span>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="panelL3" runat="server" Visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 radio-toolbar">
                                <asp:RadioButton ID="rdbConfirmApproveForDev" runat="server" Text="<%$Resources:Resource, Iaccepttheagreement %>" class="error_class" GroupName="approvefordev" />
                                <div style="margin-bottom: 5px; opacity: 0.8;">
                                    <%-- I hereby confirm the commissioning and payment of the stated development effort. The number of hours in the EDT and the previously agreed hourly rate are recognized.--%>
                                    <asp:Label ID="Label11" Text="<%$Resources:Resource, Confirmthecommissioning %>" runat="server"></asp:Label>
                                </div>
                                <br />
                                <br />
                                <asp:RadioButton ID="rdbCancelApproveForDev" runat="server" Text="<%$Resources:Resource, Idonotaccepttheagreement %>" class="error_class" GroupName="approvefordev" />
                                <div style="margin-bottom: 5px; opacity: 0.8;">
                                    <%-- The stated development effort as shown in the ticket is not agreed on. The ticket will be set to status "closed".--%>
                                    <asp:Label ID="Label22" Text="<%$Resources:Resource, Developmenteffortintheticketisnotagreed %>" runat="server"></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                        <br />
                    </div>
                    <div class="row form-group">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnConfirmApproveForDev" runat="server" Text="<%$Resources:Resource, Save %>" CssClass="form_button" UseSubmitBehavior="false" validate="no" OnClick="btnApprovedfordevelopmentVO_Click" />&nbsp;
                                   <a href="javascript:void(0);" data-bs-dismiss="modal" aria-label="Close" aria-hidden="true">
                                       <asp:Label ID="Label16" Text="<%$Resources:Resource, Close %>" runat="server"></asp:Label></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="custom_alert">
        <div class="cus-overlay"></div>
        <div class="cus-dialog">
            <div class="message"></div>
            <div class="yes">OK</div>
            &nbsp;&nbsp;&nbsp;
            <div class="no">Cancel</div>
        </div>
    </div>

    <script src="js/data.js"></script>
    <script src="js/validator.js?3" type="text/javascript"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script type="text/javascript" src="js/bootstrap-datepicker.js"></script>
    <script>
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
        });

        $("#ContentPlaceHolder1_btnReject").click(function () {
            $('#modal_reject').modal('show');
            return false;
        });

        $("#ContentPlaceHolder1_btnReject2").click(function () {
            var txt = $("#ContentPlaceHolder1_txtRejectComments");
            var remarks = $.trim(txt.val());
            if (remarks == '' || remarks.length == 0) {
                var id = 'err_lbl_rejectcomments';
                $('#' + id).remove();
                $(txt).after("<span id='" + id + "' class='error_class'>This field cannot be empty</span>");
                return false;
            }
        });

        $("#ContentPlaceHolder1_btnApprovedfordevelopmentVO").click(function () {
            $('#modal_approvefordevelopmentvo').modal('show');
            return false;
        });

        //$("#ContentPlaceHolder1_btnConfirmApproveForDev").click(function (e) {
        //    if (e.target.getAttribute('custom_alert') == '1') return true;

        //    customalert("Are you sure you perform this action? This action cannot be undone", onconfirmapprovesuccess);
        //    return false;
        //});

        function onconfirmapprovesuccess() {
            console.log("onconfirmapprovesuccess called");
            $("#ContentPlaceHolder1_btnConfirmApproveForDev").attr("custom_alert", "1");
            $("#ContentPlaceHolder1_btnConfirmApproveForDev").click();
        }

        //$("#ContentPlaceHolder1_btnVONeeded").click(function (e) {
        //    if (e.target.getAttribute('custom_alert') == '1') return true;

        //    var ddl = $("#ContentPlaceHolder1_ddlVOneeded");
        //    var voneeded = $.trim(ddl.val());
        //    if (voneeded == '' || isNaN(voneeded) || voneeded < 0) {
        //        var id = 'err_lbl_voneeded';
        //        $('#' + id).remove();
        //        $(ddl).after("<span id='" + id + "' class='error_class'>This field cannot be empty</span>");
        //        return false;
        //    }

        //    customalert("Are you sure you perform this action? This action cannot be undone", onvoneededsuccess);
        //      return false;
        //});

        function onvoneededsuccess() {
            console.log("onvoneededsuccess called");
            $("#ContentPlaceHolder1_btnVONeeded").attr("custom_alert", "1");
            $("#ContentPlaceHolder1_btnVONeeded").click();
        }
        function openticketmodal() {
            $('#modal_voneeded').modal('show');
            return false;
        }

        function customalert(msg, callback) {
            var confirmBox = $("#custom_alert");
            confirmBox.find(".message").text(msg);
            var myYes = confirmBox.find(".yes");
            $(myYes).unbind().click(function () {
                confirmBox.hide();
                callback();
            });

            var myNo = confirmBox.find(".no");
            $(myNo).unbind().click(function () {
                confirmBox.hide();
                return false;
            });


            confirmBox.show();
            return false;
        }


        var filter = '';
        function commentClicked() {
            filter = '<%= TicketId %>';
            $('#ContentPlaceHolder1_lblcommentloading').show();
            $('#commenttable tbody').html('');
            setTimeout(loadComments, 1);
        }

        function loadComments() {
            var totalcount = gettotalcount("GetTicketCommentsCount", filter);
            processComment(1);
            $('#commentpagination').bootpag({
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
                processComment(num);
            });
        }

        function processComment(pageIndex) {
            $('#commenttable tbody').html('');
            var xmldata = getdata("GetTicketComments", pageIndex, filter);
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var createdby = '<%= UserId %>';
                records.each(function () {
                    var record = $(this);

                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("author").text() + '</td>';
                    contents += '<td>' + record.find("createddate").text() + '</td>';
                    contents += '<td>' + String(record.find("comment").text()).nl2br() + '</td>';
                    if (record.find("createdby").text() == createdby)
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='javascript:void(0);' onclick='editComment(\"" + id + "\")' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteComment(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td>";
                    else
                        contents += "<td>&nbsp;</td>";
                    contents += '</tr>';
                    $('#commenttable > tbody:last').append(contents);
                });
            }
            else {
                $('#commenttable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
            $('#ContentPlaceHolder1_lblcommentloading').hide();
        }

        function editComment(id) {
            $("#ContentPlaceHolder1_txtComments").val('');
            $("#ContentPlaceHolder1_hdfilter1").val(id);
            var xmldata = getdata("GetTicketComment", 0, id);
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    $("#ContentPlaceHolder1_txtComments").val(record.find("comment").text());
                });
            }
            $("#manage_log").modal('show');
        }

        function deleteComment(id) {
            deleteData("DeleteTicketComment", id);
            commentClicked();
        }

        function documentClicked() {
            filter = '<%= TicketId %>';
            $('#ContentPlaceHolder1_lbldocumentloading').show();
            $('#documenttable tbody').html('');
            setTimeout(processDocument);
        }

        function processDocument() {
            $('#documenttable tbody').html('');
            var xmldata = getdata("GetTicketDocuments", 0, filter); // no pagination
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var createdby = '<%= UserId %>';
                records.each(function () {
                    var record = $(this);

                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("author").text() + '</td>';
                    contents += '<td>' + record.find("createddate").text() + '</td>';
                    contents += '<td>' + String(record.find("comment").text()).nl2br() + '</td>';
                    if (record.find("createdby").text() == createdby)
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href = '../app/viewdocument.aspx?file=" + record.find("filename").text() + "' target = '_blank' class='btn btn_view' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye' style='color: #d64651;'></i></a><a href='javascript:void(0);' onclick='editDocument(\"" + id + "\")' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteDocument(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa-regular fa-trash-can'></i></a></div></td> ";
                    else
                        contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href = '../app/viewdocument.aspx?file=" + record.find("filename").text() + "' target = '_blank' class='btn btn_view' data-toggle='tooltip' data-placement='top' data-original-title='View' title='View'><i class='fa fa-eye' style='color: #d64651;'></i></a></div></td>";

                    contents += '</tr>';
                    $('#documenttable > tbody:last').append(contents);
                });
            }
            else {
                $('#documenttable tbody').html("<tr><td colspan='4'>" + NoRecordsFound + "</td></tr>");
            }
            $('#ContentPlaceHolder1_lbldocumentloading').hide();
        }

        function openDocument() {
            $('#panelDoc').show();
            $("#ContentPlaceHolder1_txtDocComments").val('');
            $("#ContentPlaceHolder1_hdfilter2").val('');
            $("#manage_document").modal('show');
        }

        function editDocument(id) {
            $('#panelDoc').hide();
            $("#ContentPlaceHolder1_txtDocComments").val('');
            $("#ContentPlaceHolder1_hdfilter2").val(id);
            var xmldata = getdata("GetTicketDocument", 0, id);
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    $("#ContentPlaceHolder1_txtDocComments").val(record.find("comment").text());
                });
            }
            $("#manage_document").modal('show');
        }

        function deleteDocument(id) {
            deleteData("DeleteTicketDocument", id);
            documentClicked()
        }

        function validateDocument() {
            if ($("#ContentPlaceHolder1_hdfilter2").val().length > 0) return true;

            var currentBtn = $(this);
            currentBtn.hide();

            $('.error_class').remove();
            var fileinput = $("#fileUpload");
            var filename = $(fileinput).val();
            var extension = filename.replace(/^.*\./, '');
            if (extension == filename) {
                extension = '';
            } else {
                extension = extension.toLowerCase();
            }

            if (extension != 'jpg' && extension != 'jpeg' && extension != 'pdf' && extension != 'png' && extension != 'doc' && extension != 'docx' && extension != 'xls' && extension != 'xlsx' && extension != 'eml') {
                $(fileinput).after("<span class='error_class'>File is either empty or with invalid extension.</span>");
                currentBtn.show();
                return false;
            }
            return true;
        }

        $('#fileUpload').change(function () {
            if ($("#ContentPlaceHolder1_hdfilter2").val().length > 0) return true;

            var f = $(this);
            var fid = $(f).attr('id');

            var fileData = $(f).prop("files")[0];
            console.log(fileData.name);

            var extension = fileData.name.replace(/^.*\./, '');
            if (extension == fileData.name) {
                extension = '';
            } else {
                extension = extension.toLowerCase();
            }

            if (extension != 'jpg' && extension != 'jpeg' && extension != 'pdf' && extension != 'png' && extension != 'doc' && extension != 'docx' && extension != 'xls' && extension != 'xlsx' && extension != 'eml') {
                alert("Error: File is either empty or with invalid extension");
                return false;
            }

            var fivembsize = 5 * 1024 * 1024;
            if (fileData.size > fivembsize) {
                alert("Error: File is too large");
                return false;
            }

            var formData = new window.FormData();
            formData.append("file", fileData);

            var id = 'per_lbl_' + fid;
            $('#' + id).remove();

            $.ajax({
                /* url: '../generalfileupload.ashx',*/
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
                    var hid = '#ContentPlaceHolder1_hid_document';
                    $(hid).val(data);
                },
                error: function (evt, error) {
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert("There was a problem uploading the file. Please try again.");
                }
            });
        });

        function LogsClicked() {
            ticId = '<%= TicId %>';

            $('#ContentPlaceHolder1_lbllogloading').show();
            $('#logtable tbody').html('');
            setTimeout(loadLog, 1);
        }

        function loadLog() {
            var totalcount = gettotalcount("GetTicketLogCount", ticId);
            processLog(1);

            $('#logpagination').bootpag({
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
                processLog(num);
            });
        }

        function processLog(pageIndex) {
            $('#logtable tbody').html('');
            var xmldata = getdata2("GetTicketLogs", pageIndex, ticId);
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                records.each(function () {
                    var record = $(this);

                    var contents = '<tr>';
                    contents += '<td>' + record.find("processed_date").text() + '</td>';
                    contents += '<td>' + record.find("processed_remark").text() + '</td>';

                    var iswf = (parseInt(record.find("isworkforce").text()) == 1) ? " (workforce)" : "";
                    contents += '<td>' + record.find("processed_user").text() + iswf + '</td>';
                    contents += '</tr>';

                    $('#logtable > tbody:last').append(contents);
                });
            }
            else {
                $('#logtable tbody').html("<tr><td colspan='3'>" + NoRecordsFound + "</td></tr>");
            }
            $('#ContentPlaceHolder1_lbllogloading').hide();
        }
    </script>

</asp:Content>
