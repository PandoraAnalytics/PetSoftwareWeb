using BABusiness;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Breederapp
{
    public partial class ticketview : PageBase
    {
        public string TicketId
        {
            get { return BASecurity.Encrypt(this.ConvertToString(ViewState["id"]), PageBase.HashKey); }
        }

        public string TicId
        {
            get { return this.ConvertToString(ViewState["id"]); }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
            }
            Control divMenu = Master.FindControl("ticketmainmenu");
            divMenu.Visible = true;
            Control divUserMenu = Master.FindControl("usermenu");
            divUserMenu.Visible = false;
            Control divChecklistMenu = Master.FindControl("checklistmainmenu");
            divChecklistMenu.Visible = false;
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], this.UserId);
            if (collection == null) Response.Redirect("ticketlist.aspx?access=false");

            DateTime tempDate1 = Convert.ToDateTime(collection["createddate"]);
            if (tempDate1 != DateTime.MinValue) this.lblCreated.Text = tempDate1.ToString(this.DateTimeFormat) + " / " + collection["author"];

            DateTime tempDate2 = Convert.ToDateTime(collection["updateddate"]);
            if (tempDate2 != DateTime.MinValue) this.lblUpdated.Text = tempDate2.ToString(this.DateTimeFormat) + " / " + collection["updatedbyauthor"];

            this.lblTicketNo.Text = collection["ticketno"].PadLeft(5, '0');
            this.lblHeading.Text = "[" + collection["ticketno"].PadLeft(5, '0') + "] " + collection["header"];

            string header = collection["header"];
            if (this.ConvertToInteger(collection["isbug"]) == 1) header += "&nbsp;[Bug]";
            if (this.ConvertToInteger(collection["commentscount"]) > 0) header += "&nbsp;&nbsp;<i class=\"fa fa-comments\" aria-hidden=\"true\"></i>";
            if (this.ConvertToInteger(collection["doccount"]) > 0) header += "&nbsp;&nbsp;<i class=\"fa fa-paperclip\" aria-hidden=\"true\"></i>";
            this.lblHeader.Text = header;
            this.lblDescription.Text = this.nl2br(collection["description"]);
            this.lblApplication.Text = collection["appname"];

            this.lblEDT.Text = collection["etd"];
            this.txtEDT.Text = collection["etd"];
            ViewState["edt"] = collection["etd"];

            if (!string.IsNullOrEmpty(collection["completiondate"]))
            {
                DateTime tempDate3 = Convert.ToDateTime(collection["completiondate"]);
                if (tempDate3 != DateTime.MinValue) this.lblEDT.Text += "&nbsp;[CD: " + tempDate3.ToString(this.DateFormat) + "]";
            }

            ViewState["authoremail"] = collection["authoremail"];

            this.lblPriority.Text = collection["priorityname"];
            switch (collection["voneeded"])
            {
                case "0":
                    this.lblVONeeded.Text = "No";
                    break;

                case "1":
                    this.lblVONeeded.Text = "Yes";
                    break;

                default:
                    this.lblVONeeded.Text = "-";
                    break;
            }

            this.lblCurrentStatus.Text = collection["statusname"];

            this.panelOptionalEmail.Visible = !string.IsNullOrEmpty(collection["optionalemails"]);
            if (this.panelOptionalEmail.Visible) this.lblOptionalEmails.Text = collection["optionalemails"].Replace(",", ", ");

            int isowner = this.ConvertToInteger(collection["isowner"]);

            if (isowner == 0) // readonly user has no permission
            {
                this.lnkAddComments.Visible = false;
                this.lnkAddDocuments.Visible = false;

                return;
            }

            int isbug = this.ConvertToInteger(collection["isbug"]);
            int status = this.ConvertToInteger(collection["status"]);
            int voneeded = this.ConvertToInteger(collection["voneeded"]);

            if (isbug == 0 && voneeded == 1)
            {
                this.PopulateApproveComments();
            }

            this.panelEDTEdit.Visible = false;
            this.panelCompletionDate.Visible = false;
            this.btnEdit2.Visible = false;
            this.btnOpenTicket.Visible = false;
            //this.btnOpenTicket.Visible = true;//need to change --false

            bool islogauthor = (ConvertToInteger(Session["userid"]) == ConvertToInteger(collection["createdby"]));

            bool isadmin = (isowner == 1);

            if (isbug == 0)
            {
                if (voneeded != 0 && voneeded != 1)
                {
                    switch (status)
                    {
                        case (int)Ticket.Status.NEW:
                            this.btnEdit2.Visible = (islogauthor || isadmin);
                            string userEmail = this.ConvertToString(Session["Email"]).ToLower();
                            //this.btnOpenTicket.Visible = (userEmail == "sumit.deshpande10@gmail.com" || userEmail == "j.mueller@pandora-ict.de");
                            this.btnOpenTicket.Visible = (isadmin);
                           //this.btnOpenTicket.Visible = (userEmail == "sumit.deshpande10@gmail.com" || userEmail == "amitmanekar19@gmail.com" || userEmail == "nileshbhamare2011@gmail.com");
                            break;

                        case (int)Ticket.Status.CLOSED:
                            this.btnClosed.Visible = false;
                            break;
                    }

                    return;
                }
            }

            int[] statusArray = null;
            DataTable statusTable = Ticket.GetUserStatuses(this.UserId);
            if (statusTable != null && statusTable.Rows.Count > 0) statusArray = statusTable.Rows.Cast<DataRow>().Select(row => this.ConvertToInteger(row["status"])).ToArray();
            statusTable = null;
            if (statusArray == null) statusArray = new int[0];

            this.btnClosed.Visible = (statusArray.Contains((int)Ticket.Status.CLOSED));
            switch (status)
            {
                case (int)Ticket.Status.NEW:
                    this.btnEdit2.Visible = (islogauthor || isadmin);
                    if (isbug == 1)
                    {
                        this.btnApprovedfordevelopment.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEDFORDEVELOPMENT));
                        if (this.btnApprovedfordevelopment.Visible) this.panelEDTEdit.Visible = true;
                    }
                    else
                    {
                        this.btnDeferred.Visible = (statusArray.Contains((int)Ticket.Status.DEFERRED));
                        this.btnEstimateEDT.Visible = (statusArray.Contains((int)Ticket.Status.ESTIMATEEDT));
                    }
                    break;

                case (int)Ticket.Status.DEFERRED:
                    this.btnEstimateEDT.Visible = (statusArray.Contains((int)Ticket.Status.ESTIMATEEDT));
                    break;

                case (int)Ticket.Status.ESTIMATEEDT:
                    this.btnEdit2.Visible = (islogauthor || isadmin);
                    this.btnDeferred.Visible = (statusArray.Contains((int)Ticket.Status.DEFERRED));
                    if (isbug == 1)
                    {
                        this.btnApprovedfordevelopment.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEDFORDEVELOPMENT));
                        if (this.btnApprovedfordevelopment.Visible) this.panelEDTEdit.Visible = true;
                    }
                    else
                    {
                        this.btnWaitingfordevelopmentapproval.Visible = (statusArray.Contains((int)Ticket.Status.WAITINGFORDEVAPPROVAL));
                        if (this.btnWaitingfordevelopmentapproval.Visible)
                        {
                            this.panelEDTEdit.Visible = true;
                            if (voneeded == 1) this.btnWaitingfordevelopmentapproval.Text = "Waiting for customer approval";
                        }
                    }
                    break;

                case (int)Ticket.Status.WAITINGFORDEVAPPROVAL:
                    //this.btnEdit2.Visible = (islogauthor || isadmin);

                    //this.btnApprovedfordevelopment.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEDFORDEVELOPMENT));
                    //if (this.btnApprovedfordevelopment.Visible) this.panelEDTEdit.Visible = true;

                    //break;

                    this.btnEdit2.Visible = (islogauthor || isadmin);
                    if (isbug == 0 && voneeded == 1)
                    {
                        this.PopulateVOApprovedButton();
                        this.panelEDTEdit.Visible = false;
                        this.lblCurrentStatus.Text = "Waiting for customer approval";
                    }
                    else
                    {
                        this.btnApprovedfordevelopment.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEDFORDEVELOPMENT));
                        if (this.btnApprovedfordevelopment.Visible) this.panelEDTEdit.Visible = true;
                    }
                    break;

                case (int)Ticket.Status.APPROVEDFORDEVELOPMENT:
                    this.btnDevelopment.Visible = (statusArray.Contains((int)Ticket.Status.DEVELOPMENT));
                    if (this.btnDevelopment.Visible) this.panelCompletionDate.Visible = true;

                    if (!string.IsNullOrEmpty(collection["log_comments"])) this.lblCurrentStatus.Text += ", " + collection["log_comments"];
                    break;

                case (int)Ticket.Status.DEVELOPMENT:
                    this.btnTesting.Visible = (statusArray.Contains((int)Ticket.Status.TESTING));

                    if (!string.IsNullOrEmpty(collection["log_comments"])) this.lblCurrentStatus.Text += ", " + collection["log_comments"];
                    break;

                case (int)Ticket.Status.TESTING:
                    this.btnApproved.Visible = (statusArray.Contains((int)Ticket.Status.APPROVED));
                    this.btnApproved.Text = Resources.Resource.Approve;
                    this.btnReject.Visible = (statusArray.Contains((int)Ticket.Status.APPROVED));
                    break;

                case (int)Ticket.Status.APPROVED:// need to change
                    this.btnTesting.Visible = (statusArray.Contains((int)Ticket.Status.TESTING)) && (isowner == 1);  // Admin can put ticket back to testing in case wrongly approved
                    this.btnRolledout.Visible = (statusArray.Contains((int)Ticket.Status.ROLLEDOUT));
                    break;

                case (int)Ticket.Status.ROLLEDOUT:// need to change
                    //this.btnApproved.Visible = (statusArray.Contains((int)Ticket.Status.APPROVED)) && (isowner == 1);
                    //if (this.btnApproved.Visible) this.btnApproved.Text = "Back to Approve";
                    this.btnApproved.Visible = (statusArray.Contains((int)Ticket.Status.APPROVED)) && (isowner == 1);
                    if (this.btnApproved.Visible) this.btnApproved.Text = "Back to Approve";
                    this.btnApproveRollout.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEROLLEDOUT));
                    break;
                //this.btnApproveRollout.Visible = (statusArray.Contains((int)Ticket.Status.APPROVEROLLEDOUT));
                //if (this.btnApproved.Visible) this.btnApproved.Text = "Back to Approve";
                // break;

                case (int)Ticket.Status.APPROVEROLLEDOUT:
                    this.btnClosed.Visible = (statusArray.Contains((int)Ticket.Status.CLOSED));
                    break;

                case (int)Ticket.Status.CLOSED:
                    this.btnClosed.Visible = false;
                    break;
            }
        }

        private bool SaveStatus(int xiStatus, string xiComments = "")
        {
            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], this.UserId);
            if (collection == null)
            {
                this.lblError.Text = "Invalid";
                return false;
            }

            int status = this.ConvertToInteger(collection["status"]);
            if (status == xiStatus || status == (int)Ticket.Status.CLOSED)
            {
                return false;
            }

            collection["status"] = xiStatus.ToString();
            collection["userid"] = this.UserId;

            Ticket objTicket = new Ticket();
            bool success = objTicket.UpdateStatus(collection, ViewState["id"]);
            if (!success)
            {
                this.lblError.Text = Resources.Resource.error;
                return false;
            }

            collection = new NameValueCollection();
            collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
            collection["userid"] = this.UserId;
            collection["messageid"] = xiStatus.ToString();
            collection["oldentry"] = xiComments;
            collection["newentry"] = "-";
            objTicket.AddLog(collection);

            objTicket = null;

            return true;
        }

        private void SendEmailAndBackToList(int xiStatus)
        {
            NameValueCollection emailDetailsCollection = new NameValueCollection();
            emailDetailsCollection.Add("ticketid", ConvertToString(ViewState["id"]));
            emailDetailsCollection.Add("userid", this.UserId);
            BreederMail.SendEmail(BreederMail.MessageType.TICKETCHANGESTATUS, emailDetailsCollection);
            Response.Redirect("ticketlist.aspx");
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            string comments = this.txtComments.Text.Trim();
            if (comments.Length == 0) return;

            NameValueCollection collection = new NameValueCollection();
            collection["ticketid"] = ConvertToString(ViewState["id"]);
            collection["comment"] = this.txtComments.Text.Trim();
            collection["userid"] = this.UserId;

            string commentId = this.hdfilter1.Value;

            Ticket objTicket = new Ticket();
            if (string.IsNullOrEmpty(commentId))
            {
                bool success = objTicket.AddTicketComment(collection);
                if (success)
                {
                    collection = new NameValueCollection();
                    collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                    collection["userid"] = this.UserId;
                    collection["messageid"] = (int)Ticket.Status.COMMENT + "";
                    collection["oldentry"] = "-";
                    collection["newentry"] = "-";
                    objTicket.AddLog(collection);

                    if (comments.Length > 255) comments = comments.Substring(0, 255);

                    NameValueCollection emailDetailsCollection = new NameValueCollection();
                    emailDetailsCollection.Add("authoremail", ConvertToString(ViewState["authoremail"]));
                    emailDetailsCollection.Add("ticketid", ConvertToString(ViewState["id"]));
                    emailDetailsCollection.Add("ticketno", this.lblTicketNo.Text);
                    emailDetailsCollection.Add("username", ConvertToString(Session["username"]));
                    emailDetailsCollection.Add("header", this.lblHeader.Text);
                    emailDetailsCollection.Add("comment", comments);
                    BreederMail.SendEmail(BreederMail.MessageType.TICKETADDNEWCOMMENT, new NameValueCollection(emailDetailsCollection));
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }
            else
            {
                commentId = BASecurity.Decrypt(commentId, PageBase.HashKey);
                bool success = objTicket.UpdateComment(collection, commentId);
                if (success)
                {
                    collection = new NameValueCollection();
                    collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                    collection["userid"] = this.UserId;
                    collection["messageid"] = (int)Ticket.Status.UPDATECOMMENT + "";
                    collection["oldentry"] = "-";
                    collection["newentry"] = "-";
                    objTicket.AddLog(collection);
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }

            this.hdfilter1.Value = string.Empty;
            this.txtComments.Text = string.Empty;
        }

        protected void btnDocument_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            string documentId = this.hdfilter2.Value;
            if (string.IsNullOrEmpty(documentId))
            {
                string extension = Path.GetExtension(this.hid_document.Value);
                if (extension != null) extension = extension.ToLower();
                ArrayList extensionArray = new ArrayList();
                extensionArray.Add(".jpg");
                extensionArray.Add(".png");
                extensionArray.Add(".jpeg");
                extensionArray.Add(".pdf");
                extensionArray.Add(".doc");
                extensionArray.Add(".docx");
                extensionArray.Add(".xls");
                extensionArray.Add(".xlsx");
                extensionArray.Add(".eml");

                if (extensionArray.Contains(extension) == false)
                {
                    this.lblError.Text = "Invalid file";
                    return;
                }
            }

            NameValueCollection collection = new NameValueCollection();
            collection["ticketid"] = ConvertToString(ViewState["id"]);
            collection["comment"] = this.txtDocComments.Text.Trim();
            collection["userid"] = this.UserId;

            Ticket objTicket = new Ticket();
            if (string.IsNullOrEmpty(documentId))
            {
                collection.Add("filename", this.hid_document.Value);

                bool success = objTicket.AddDocument(collection);

                if (success)
                {
                    collection = new NameValueCollection();
                    collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                    collection["userid"] = this.UserId;
                    collection["messageid"] = (int)Ticket.Status.ADDDOCUMENT + "";
                    collection["oldentry"] = "-";
                    collection["newentry"] = "-";
                    objTicket.AddLog(collection);

                    NameValueCollection emailDetailsCollection = new NameValueCollection();
                    emailDetailsCollection.Add("authoremail", ConvertToString(ViewState["authoremail"]));
                    emailDetailsCollection.Add("ticketid", ConvertToString(ViewState["id"]));
                    emailDetailsCollection.Add("ticketno", this.lblTicketNo.Text);
                    emailDetailsCollection.Add("username", ConvertToString(Session["username"]));
                    emailDetailsCollection.Add("header", this.lblHeader.Text);
                    BreederMail.SendEmail(BreederMail.MessageType.TICKETADDNEWDOCUMENT, new NameValueCollection(emailDetailsCollection));
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }
            else
            {
                documentId = BASecurity.Decrypt(documentId, PageBase.HashKey);
                bool success = objTicket.UpdateDocument(collection, documentId);

                if (success)
                {
                    collection = new NameValueCollection();
                    collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                    collection["userid"] = this.UserId;
                    collection["messageid"] = (int)Ticket.Status.UPDATEDOCUMENT + "";
                    collection["oldentry"] = "-";
                    collection["newentry"] = "-";
                    objTicket.AddLog(collection);
                }
                else
                {
                    this.lblError.Text = Resources.Resource.error;
                    return;
                }
            }

            this.hdfilter2.Value = string.Empty;
            this.hid_document.Value = string.Empty;
            this.txtDocComments.Text = string.Empty;
        }

        protected void btnDeferred_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.DEFERRED);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.DEFERRED);
        }

        protected void btnApprovedfordevelopment_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], this.UserId);
            if (collection == null)
            {
                this.lblError.Text = "Invalid";
                return;
            }

            int isbug = this.ConvertToInteger(collection["isbug"]);
            int edt = int.MinValue;
            if (isbug == 0)
            {
                edt = this.ConvertToInteger(this.txtEDT.Text.Trim());
                if (edt <= 0)
                {
                    this.lblError.Text = "EDT should greater than zero";
                    return;
                }
            }
            if (edt < 0) edt = 0;

            int oldedt = this.ConvertToInteger(ViewState["edt"]);
            if (oldedt != edt)
            {
                collection = new NameValueCollection();
                collection["etd"] = edt.ToString();
                collection["userid"] = this.UserId;

                Ticket objTicket = new Ticket();
                bool success1 = objTicket.UpdateETD(collection, ViewState["id"]);
                if (!success1) return;

                collection = new NameValueCollection();
                collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                collection["userid"] = this.UserId;
                collection["messageid"] = (int)Ticket.Status.EDTCHANGE + "";
                collection["oldentry"] = oldedt.ToString();
                collection["newentry"] = "-";

                objTicket.AddLog(collection);
                objTicket = null;
            }

            bool success = this.SaveStatus((int)Ticket.Status.APPROVEDFORDEVELOPMENT);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.APPROVEDFORDEVELOPMENT);
        }

        protected void btnDevelopment_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (this.panelCompletionDate.Visible)
            {
                if (this.txtCompletionDate.Text.Trim().Length == 0)
                {
                    this.lblError.Text = "Please enter completion date";
                    return;
                }

                NameValueCollection collection = new NameValueCollection();
                collection["completiondate"] = this.txtCompletionDate.Text.Trim();
                collection["userid"] = this.UserId;

                Ticket objTicket = new Ticket();
                bool success1 = objTicket.UpdateCompletionDate(collection, ViewState["id"]);
                objTicket = null;

                if (!success1) return;
            }

            bool success = this.SaveStatus((int)Ticket.Status.DEVELOPMENT);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.DEVELOPMENT);
        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.APPROVED);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.APPROVED);
        }

        protected void btnRolledout_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.ROLLEDOUT);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.ROLLEDOUT);
        }

        protected void btnClosed_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.CLOSED);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.CLOSED);
        }

        protected void btnEstimateEDT_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.ESTIMATEEDT);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.ESTIMATEEDT);
        }

        protected void btnWaitingfordevelopmentapproval_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], this.UserId);
            if (collection == null)
            {
                this.lblError.Text = "Invalid";
                return;
            }

            int edt = int.MinValue;
            int isbug = this.ConvertToInteger(collection["isbug"]);
            if (isbug == 0)
            {
                edt = this.ConvertToInteger(this.txtEDT.Text.Trim());
                if (edt <= 0)
                {
                    this.lblError.Text = "EDT shuld greater than zero";
                    return;
                }
            }
            if (edt < 0) edt = 0;

            collection = new NameValueCollection();
            collection["etd"] = edt.ToString();
            collection["userid"] = this.UserId;

            Ticket objTicket = new Ticket();
            bool success = objTicket.UpdateETD(collection, ViewState["id"]);
            if (!success) return;

            objTicket = null;

            success = this.SaveStatus((int)Ticket.Status.WAITINGFORDEVAPPROVAL);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.WAITINGFORDEVAPPROVAL);
        }

        protected void btnApproveRollout_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.APPROVEROLLEDOUT);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.APPROVEROLLEDOUT);
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = this.ProcessPrintTicket(ViewState["id"], this.UserId);
                if (string.IsNullOrEmpty(fileName)) return;

                fileName = fileName + ".pdf";
                string pdfFileName = this.FileUploadTempPath + fileName;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream file = new FileStream(pdfFileName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] fileBytes = new byte[file.Length];
                        file.Read(fileBytes, 0, (int)file.Length);
                        ms.Write(fileBytes, 0, (int)file.Length);
                    }
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Buffer = true;
                    Response.Clear();
                    var byteArray = ms.ToArray();
                    Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                    Response.OutputStream.Flush();
                }

                File.Delete(pdfFileName);
            }
            catch { }
        }

        protected void btnEdit2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ticketedit.aspx?" + this.TicketId);
        }

        protected void btnReject2_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (this.txtRejectComments.Text.Trim().Length == 0)
            {
                this.lblError.Visible = true;
                this.lblError.Text = "Please enter remark to continue";
                return;
            }

            string remark = this.txtRejectComments.Text.Trim();
            if (remark.Length > 500) remark = remark.Substring(0, 500);

            bool success = this.SaveStatus((int)Ticket.Status.DEVELOPMENT, remark);
            if (!success) return;

            this.SendEmailAndBackToList((int)Ticket.Status.DEVELOPMENT);
        }

        protected void btnMarkastesting_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            this.lblError.Text = string.Empty;

            bool success = this.SaveStatus((int)Ticket.Status.TESTING, "Solution uploaded to " + e.CommandArgument + " platform");
            if (!success) return;

            NameValueCollection emailDetailsCollection = new NameValueCollection();
            emailDetailsCollection.Add("ticketid", ConvertToString(ViewState["id"]));
            emailDetailsCollection.Add("userid", this.UserId);
            emailDetailsCollection.Add("platform", e.CommandArgument.ToString());
            BreederMail.SendEmail(BreederMail.MessageType.TICKETCHANGESTATUS, emailDetailsCollection);
            Response.Redirect("ticketlist.aspx");
        }

        protected void btnVONeeded_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.ConvertToInteger(this.ddlVOneeded.SelectedValue) < 0)
            {
                this.lblError.Text = "Please select VO Needed to continue";
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection["voneeded"] = this.ddlVOneeded.SelectedValue;
            collection["userid"] = this.UserId;

            Ticket objTicket = new Ticket();
            bool success = objTicket.UpdateVONeeded(collection, ViewState["id"]);

            if (success)
            {
                string vo = "-";
                switch (this.ddlVOneeded.SelectedValue)
                {
                    case "0":
                        vo = "No";
                        break;

                    case "1":
                        vo = "Yes";
                        break;
                }

                collection = new NameValueCollection();
                collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                collection["userid"] = this.UserId;
                collection["messageid"] = (int)Ticket.Status.VONEEDEDUPDATED + "";
                collection["oldentry"] = vo;
                collection["newentry"] = "-";
                objTicket.AddLog(collection);

                objTicket = null;

                this.PopulateControls();
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        private void PopulateApproveComments()
        {
            this.lit1.Text = string.Empty;

            string controlhtml = "<div class='col-lg-12 col-md-12 col-sm-12 col-xs-12'><div>{0}&nbsp;{1}</div><div style='margin-top:5px;opacity:0.8;'>{2}</div></div>";

            NameValueCollection matrixcollection = Ticket.GetTicketVOApprovalMatrix(ViewState["id"]);
            if (matrixcollection == null) return;

            //string status = "<span class='status_btn st_onhold'>?</span>";
            string status = "<span class='flags_feature'>?</span>";

            ArrayList replaces = new ArrayList();

            if (!string.IsNullOrEmpty(matrixcollection["matrix"]) && matrixcollection["matrix"].Length > 0)
            {
                string[] replace = new string[4];
                replace[0] = "L1";
                replace[1] = status;
                replace[2] = matrixcollection["matrix_name"];
                replace[3] = "1";
                replaces.Add(replace);
            }

            if (!string.IsNullOrEmpty(matrixcollection["matrix2"]) && matrixcollection["matrix2"].Length > 0)
            {
                string[] replace = new string[4];
                replace[0] = "L2";
                replace[1] = status;
                replace[2] = matrixcollection["matrix2_name"];
                replace[3] = "2";
                replaces.Add(replace);
            }

            if (!string.IsNullOrEmpty(matrixcollection["matrix3"]) && matrixcollection["matrix3"].Length > 0)
            {
                string[] replace = new string[4];
                replace[0] = "L3";
                replace[1] = status;
                replace[2] = matrixcollection["matrix3_name"];
                replace[3] = "3";
                replaces.Add(replace);
            }

            DataTable approvedTable = Ticket.GetTicketVOApprovedComments(ViewState["id"]);
            if (approvedTable != null && approvedTable.Rows.Count > 0)
            {
                for (int level = 1; level <= 3; level++)
                {
                    DataRow[] foundRows = approvedTable.Select("approvelevel=" + level);
                    if (foundRows != null && foundRows.Length > 0)
                    {
                        string lastactiondate = this.ConvertToString(foundRows[0]["last_action"]);
                        if (!string.IsNullOrEmpty(lastactiondate))
                        {
                            foreach (string[] replace in replaces)
                            {
                                if (replace[3] == level.ToString())
                                {
                                    //replace[1] = "<span class='status_btn st_open'>OK</span>";
                                    replace[1] = "<span class='flags_voneeded'>OK</span>";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            foreach (string[] replace in replaces)
            {
                this.lit1.Text += "<div class='row form-group'>" + string.Format(controlhtml, replace) + "</div>";
            }
        }

        private void PopulateVOApprovedButton()
        {
            this.btnApprovedfordevelopment.Visible = false;
            this.btnApprovedfordevelopmentVO.Visible = false;
            this.btnApprovedfordevelopmentVO2.Visible = false;

            this.panelL1.Visible = false;
            this.panelL3.Visible = false;

            NameValueCollection matrixcollection = Ticket.GetTicketVOApprovalMatrix(ViewState["id"]);
            if (matrixcollection == null) return;

            string userId = this.UserId;

            string[] slices = (!string.IsNullOrEmpty(matrixcollection["matrix"])) ? matrixcollection["matrix"].Split(',') : null;
            string[] slices2 = (!string.IsNullOrEmpty(matrixcollection["matrix2"])) ? matrixcollection["matrix2"].Split(',') : null;
            string[] slices3 = (!string.IsNullOrEmpty(matrixcollection["matrix3"])) ? matrixcollection["matrix3"].Split(',') : null;

            bool isLevel1 = (slices != null) ? slices.Contains(userId) : false;
            bool isLevel2 = (slices2 != null) ? slices2.Contains(userId) : false;
            bool isLevel3 = (slices3 != null) ? slices3.Contains(userId) : false;

            if (!isLevel1 && !isLevel2 && !isLevel3) return;

            bool approvedbyL1 = false;
            bool approvedbyL2 = false;
            bool approvedbyL3 = false;

            DataTable approvedTable = Ticket.GetTicketVOApprovedComments(ViewState["id"]);

            if (slices != null && slices.Length > 0) // we found matrix1, check if L1 has approved
            {
                DataRow[] foundRows = (approvedTable != null && approvedTable.Rows.Count > 0) ? approvedTable.Select("approvelevel=1") : null;
                approvedbyL1 = (foundRows != null && foundRows.Length > 0);
            }
            else
            {
                approvedbyL1 = true; // L1 is not set
            }


            if (slices2 != null && slices2.Length > 0) // we found matrix1, check if L2 has approved
            {
                DataRow[] foundRows = (approvedTable != null && approvedTable.Rows.Count > 0) ? approvedTable.Select("approvelevel=2") : null;
                approvedbyL2 = (foundRows != null && foundRows.Length > 0);
            }
            else
            {
                approvedbyL2 = true; // L2 is not set
            }


            if (slices3 != null && slices3.Length > 0) // we found matrix1, check if L3 has approved
            {
                DataRow[] foundRows = (approvedTable != null && approvedTable.Rows.Count > 0) ? approvedTable.Select("approvelevel=3") : null;
                approvedbyL3 = (foundRows != null && foundRows.Length > 0);
            }
            else
            {
                approvedbyL3 = true; // L3 is not set
            }


            if (isLevel1)
            {
                if (!approvedbyL1) this.btnApprovedfordevelopmentVO.Visible = true;
            }
            else if (isLevel2)
            {
                if (!approvedbyL1) this.btnApprovedfordevelopmentVO2.Visible = true; // L1 is pending, L2 can't do anything 
                else if (!approvedbyL2) this.btnApprovedfordevelopmentVO.Visible = true;
            }
            else if (isLevel3)
            {
                if (!approvedbyL1 || !approvedbyL2) this.btnApprovedfordevelopmentVO2.Visible = true; // L1/L2 is pending, L3 can't do anything 
                else if (!approvedbyL3) this.btnApprovedfordevelopmentVO.Visible = true;
            }

            if (this.btnApprovedfordevelopmentVO.Visible)
            {
                if (isLevel1)  // you are level1
                {
                    // if there is level2 and/or 3, then show L1 panel else L3 panel
                    if ((slices2 != null && slices2.Length > 0) || (slices3 != null && slices3.Length > 0)) this.panelL1.Visible = true;
                    else this.panelL3.Visible = true;
                }
                else if (isLevel2) // you are level2
                {
                    // if there is level3, then show L1 panel else L3 panel
                    if (slices3 != null && slices3.Length > 0) this.panelL1.Visible = true;
                    else this.panelL3.Visible = true;
                }
                else if (isLevel3) // you are level3
                {
                    this.panelL3.Visible = true; // shiw L3 panel
                }
            }
        }


        protected void btnApprovedfordevelopmentVO_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection matrixcollection = Ticket.GetTicketVOApprovalMatrix(ViewState["id"]);
            if (matrixcollection == null)
            {
                this.lblError.Text = "Access denied";
                return;
            }

            if (this.panelL3.Visible)
            {
                if (!this.rdbConfirmApproveForDev.Checked && !this.rdbCancelApproveForDev.Checked)
                {
                    this.lblError.Text = "Please select at least one value to continue";
                    return;
                }

                if (this.rdbCancelApproveForDev.Checked)
                {
                    bool success1 = this.SaveStatus((int)Ticket.Status.CLOSED, "because agreement of the VO was not accepted");
                    if (!success1)
                    {
                        this.lblError.Text = Resources.Resource.error;
                        return;
                    }
                    this.SendEmailAndBackToList((int)Ticket.Status.CLOSED);
                    return;
                }
            }

            string userId = this.UserId;

            string[] slices = (!string.IsNullOrEmpty(matrixcollection["matrix"])) ? matrixcollection["matrix"].Split(',') : null;
            string[] slices2 = (!string.IsNullOrEmpty(matrixcollection["matrix2"])) ? matrixcollection["matrix2"].Split(',') : null;
            string[] slices3 = (!string.IsNullOrEmpty(matrixcollection["matrix3"])) ? matrixcollection["matrix3"].Split(',') : null;

            bool isLevel1 = (slices != null) ? slices.Contains(userId) : false;
            bool isLevel2 = (slices2 != null) ? slices2.Contains(userId) : false;
            bool isLevel3 = (slices3 != null) ? slices3.Contains(userId) : false;

            int level = int.MinValue;
            if (isLevel1) level = 1;
            else if (isLevel2) level = 2;
            else if (isLevel3) level = 3;

            if (level != 1 && level != 2 && level != 3)
            {
                this.lblError.Text = "Access denied";
                return;
            }

            NameValueCollection collection = Ticket.GetTicket(ViewState["id"], userId);
            if (collection == null)
            {
                this.lblError.Text = "Invalid";
                return;
            }

            int edt = int.MinValue;
            int isbug = this.ConvertToInteger(collection["isbug"]);
            if (isbug == 0)
            {
                edt = this.ConvertToInteger(this.txtEDT.Text.Trim());
                if (edt <= 0)
                {
                    this.lblError.Text = "EDT should greater than zero";
                    return;
                }
            }
            if (edt < 0) edt = 0;

            Ticket objTicket = new Ticket();

            int oldedt = this.ConvertToInteger(ViewState["edt"]);
            if (oldedt != edt)
            {
                collection = new NameValueCollection();
                collection["etd"] = edt.ToString();
                collection["userid"] = this.UserId;


                bool success1 = objTicket.UpdateETD(collection, ViewState["id"]);
                if (!success1) return;

                collection = new NameValueCollection();
                collection["ticket_id"] = this.ConvertToString(ViewState["id"]);
                collection["userid"] = this.UserId;
                collection["messageid"] = (int)Ticket.Status.EDTCHANGE + "";
                collection["oldentry"] = oldedt.ToString();
                collection["newentry"] = "-";

                objTicket.AddLog(collection);
            }

            NameValueCollection collection1 = new NameValueCollection();
            collection1["userid"] = this.UserId;
            collection1["level"] = level.ToString();

            bool success = objTicket.SaveVOApproval(collection1, ViewState["id"]);

            if (!success)
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            bool approvedbyL1 = false;
            bool approvedbyL2 = false;
            bool approvedbyL3 = false;
            DataTable approvedTable = Ticket.GetTicketVOApprovedComments(ViewState["id"]);
            if (approvedTable != null && approvedTable.Rows.Count > 0)
            {
                if (slices != null && slices.Length > 0) // we found matrix1, check if L1 has approved
                {
                    foreach (string uid in slices)
                    {
                        DataRow[] foundRows = approvedTable.Select(string.Format("user_id={0} and approvelevel=1", uid));
                        if (foundRows != null && foundRows.Length > 0) { approvedbyL1 = true; break; }
                    }
                }
                else
                {
                    approvedbyL1 = true; // L1 is not set
                }

                if (approvedbyL1) // if L1 is false dont check further
                {
                    if (slices2 != null && slices2.Length > 0) // we found matrix1, check if L2 has approved
                    {
                        foreach (string uid in slices2)
                        {
                            DataRow[] foundRows = approvedTable.Select(string.Format("user_id={0} and approvelevel=2", uid));
                            if (foundRows != null && foundRows.Length > 0) { approvedbyL2 = true; break; }
                        }
                    }
                    else
                    {
                        approvedbyL2 = true; // L2 is not set
                    }

                    if (approvedbyL2) // if L2 is false dont check further
                    {
                        if (slices3 != null && slices3.Length > 0) // we found matrix1, check if L3 has approved
                        {
                            foreach (string uid in slices3)
                            {
                                DataRow[] foundRows = approvedTable.Select(string.Format("user_id={0} and approvelevel=3", uid));
                                if (foundRows != null && foundRows.Length > 0) { approvedbyL3 = true; break; }
                            }
                        }
                        else
                        {
                            approvedbyL3 = true; // L3 is not set
                        }
                    }
                }
            }

            // one of the Level has not approved yet
            if (!approvedbyL1 || !approvedbyL2 || !approvedbyL3) Response.Redirect("ticketlist.aspx");

            if (this.panelL3.Visible)
            {
                if (this.rdbConfirmApproveForDev.Checked)
                {
                    success = this.SaveStatus((int)Ticket.Status.APPROVEDFORDEVELOPMENT, "VO approved by customer");
                    if (!success)
                    {
                        this.lblError.Text = Resources.Resource.error;
                        return;
                    }
                    this.SendEmailAndBackToList((int)Ticket.Status.APPROVEDFORDEVELOPMENT);
                }
            }
        }
    }
}