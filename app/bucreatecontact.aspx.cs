using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bucreatecontact : ERPBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtOtherContacts")).ToList();
            int i = 1;
            for (int k = 0; k < keys.Count; k = k + 2)
            {
                string key = keys[k];
                string[] s = key.Split('_');
                this.CreateTextBox(i, s[2], "", "");
                ViewState["indexVal"] = i;
                i++;
            }
        }

        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("id");
                ViewState["indexVal"] = 0;
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.ddlProfession.DataSource = BreederData.GetBUContactProfessions(this.ConvertToInteger(this.CompanyId));
            this.ddlProfession.DataBind();

            int id = this.ConvertToInteger(ViewState["id"]);
            if (id > 0)
            {
                NameValueCollection collection = AddressBA.GetContactDetail(id);
                if (collection != null)
                {
                    this.txtFirstName.Text = collection["fname"];
                    this.txtLastName.Text = collection["lname"];
                    this.txtEmailAddress.Text = collection["email"];
                    this.txtContactNumber.Text = collection["phone"];
                    this.txtAboutDiscription.Text = collection["note"];
                    this.ddlProfession.SelectedValue = collection["service_type"];
                    this.txtAddressOptional.Text = collection["addressoptional"];

                    this.lblFirstName.Text = collection["fname"];
                    this.lblLastName.Text = collection["lname"];
                    this.lblEmailAddress.Text = collection["email"];
                    this.lblContactNumber.Text = collection["phone"];
                    this.lblAboutDiscription.Text = collection["note"];
                    this.lblProfession.Text = collection["service_typename"];
                    this.lblAddressOptional.Text = collection["addressoptional"];

                    this.PopulateOtherContacts();
                }
            }
            else
            {
                this.panelView.Visible = false;
                this.lnkEdit.Visible = false;
                this.btnBack.Visible = false;
                this.panelEdit.Visible = true;
            }
        }

        private void PopulateOtherContacts()
        {
            this.pnlMatrixLabels.Controls.Clear();
            this.pnlMatrixTextBoxes.Controls.Clear();

            DataTable dataTable = AddressBA.GetOtherEmailPhoneInContact(ViewState["id"]);
            if (dataTable != null)
            {
                string[] types = { "Mobile", "Business", "Work", "Home", "Main", "Other" };
                int indxVal = 0;
                foreach (string typ in types)
                {
                    DataRow[] rows1 = dataTable.Select(string.Format("type='{0}' and (email is not null or len(email) > 0)", typ));
                    DataRow[] rows2 = dataTable.Select(string.Format("type='{0}' and (phone is not null or len(phone) > 0)", typ));

                    int max = Math.Max(rows1.Length, rows2.Length);
                    if (max == 0) continue;

                    for (int v = 0; v < max; v++)
                    {
                        string email = (rows1.Length > v) ? rows1[v]["email"].ToString() : string.Empty;
                        string phone = (rows2.Length > v) ? rows2[v]["phone"].ToString() : string.Empty;
                        if (email == string.Empty && phone == string.Empty) continue;

                        indxVal++;
                        this.CreateTextBox(indxVal, typ, email, phone);
                        this.CreateLabels(indxVal, typ, email, phone);
                    }
                }
            }
        }

        private void CreateTextBox(int xiIndexValue, string xiArgs, string xiEmailValue, string xiPhoneValue)
        {
            string textbox1 = "txtOtherContacts_Email" + xiIndexValue;
            string textbox2 = "txtOtherContacts_Phone" + xiIndexValue;

            TextBox chkTxt = this.pnlMatrixTextBoxes.FindControl(textbox1) as TextBox;
            if (chkTxt == null)
            {
                string labelname1 = "";
                string labelname2 = "";
                switch (xiArgs)
                {
                    case "Mobile":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Mobile + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Mobile + ")";
                        break;

                    case "Business":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Business + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Business + ")";
                        break;

                    case "Work":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Work + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Work + ")";
                        break;

                    case "Home":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Home + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Home + ")";
                        break;

                    case "Main":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Main + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Main + ")";
                        break;

                    case "Other":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Other + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Other + ")";
                        break;
                }

                Panel prow = new Panel();
                prow.CssClass = "row form_row";

                Panel pcell1 = new Panel();
                pcell1.CssClass = "col-lg-6 col-md-6 col-sm-6 col-xs-12";

                Label lbl1 = new Label();
                lbl1.ID = "lbl" + textbox1;
                lbl1.Text = labelname1;
                lbl1.CssClass = "form_label";
                pcell1.Controls.Add(lbl1);

                TextBox txt1 = new TextBox();
                txt1.CssClass = "form_input email";
                txt1.ID = "txtOtherContacts_Email_" + xiArgs + "_" + xiIndexValue;
                txt1.MaxLength = 100;
                if (!string.IsNullOrEmpty(xiEmailValue)) txt1.Text = xiEmailValue;
                txt1.Attributes.Add("data-validate", "email");
                txt1.Attributes.Add("data-type", xiArgs);
                pcell1.Controls.Add(txt1);

                Panel pcell2 = new Panel();
                pcell2.CssClass = "col-lg-6 col-md-6 col-sm-6 col-xs-12";

                Label lbl2 = new Label();
                lbl2.ID = "lbl" + textbox2;
                lbl2.Text = labelname2;
                lbl2.CssClass = "form_label";
                pcell2.Controls.Add(lbl2);

                TextBox txt2 = new TextBox();
                txt2.CssClass = "form_input phone";
                txt2.ID = "txtOtherContacts_Phone_" + xiArgs + "_" + xiIndexValue;
                txt2.MaxLength = 100;
                if (!string.IsNullOrEmpty(xiPhoneValue)) txt2.Text = xiPhoneValue;
                txt2.Attributes.Add("data-validate", "phone");
                txt2.Attributes.Add("data-type", xiArgs);
                pcell2.Controls.Add(txt2);

                prow.Controls.Add(pcell1);
                prow.Controls.Add(pcell2);

                this.pnlMatrixTextBoxes.Controls.Add(prow);

                ViewState["indexVal"] = xiIndexValue;
            }
        }

        private void CreateLabels(int xiIndexValue, string xiArgs, string xiEmailValue, string xiPhoneValue)
        {
            string label1 = "lblOtherContacts_Email" + xiIndexValue;
            string label2 = "lblOtherContacts_Phone" + xiIndexValue;

            Label chkTxt = this.pnlMatrixLabels.FindControl(label1) as Label;
            if (chkTxt == null)
            {
                string labelname1 = "";
                string labelname2 = "";
                switch (xiArgs)
                {
                    case "Mobile":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Mobile + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Mobile + ")";
                        break;

                    case "Business":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Business + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Business + ")";
                        break;

                    case "Work":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Work + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Work + ")";
                        break;

                    case "Home":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Home + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Home + ")";
                        break;

                    case "Main":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Main + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Main + ")";
                        break;

                    case "Other":
                        labelname1 = Resources.Resource.EmailAddress + " (" + Resources.Resource.Other + ")";
                        labelname2 = Resources.Resource.ContactNumber + " (" + Resources.Resource.Other + ")";
                        break;
                }

                Panel prow = new Panel();
                prow.CssClass = "row form_row";

                Panel pcell1 = new Panel();
                pcell1.CssClass = "col-lg-6 col-md-6 col-sm-6 col-xs-12";

                Label lbl1 = new Label();
                lbl1.ID = "lbl" + label1;
                lbl1.Text = labelname1;
                lbl1.CssClass = "form_label";
                pcell1.Controls.Add(lbl1);

                Label txt1 = new Label();
                txt1.ID = "lblOtherContacts_Email_" + xiArgs + "_" + xiIndexValue;
                if (!string.IsNullOrEmpty(xiEmailValue)) txt1.Text = xiEmailValue;
                pcell1.Controls.Add(txt1);

                Panel pcell2 = new Panel();
                pcell2.CssClass = "col-lg-6 col-md-6 col-sm-6 col-xs-12";

                Label lbl2 = new Label();
                lbl2.ID = "lbl" + label2;
                lbl2.Text = labelname2;
                lbl2.CssClass = "form_label";
                pcell2.Controls.Add(lbl2);

                Label txt2 = new Label();
                txt2.ID = "lblOtherContacts_Phone_" + xiArgs + "_" + xiIndexValue;
                if (!string.IsNullOrEmpty(xiPhoneValue)) txt2.Text = xiPhoneValue;
                pcell2.Controls.Add(txt2);

                prow.Controls.Add(pcell1);
                prow.Controls.Add(pcell2);

                this.pnlMatrixLabels.Controls.Add(prow);
            }
        }

        protected void GetControlsCollection(Control root, ref List<Control> AllControls, Func<Control, Control> filter)
        {
            foreach (Control child in root.Controls)
            {
                var childFiltered = filter(child);
                if (childFiltered != null) AllControls.Add(child);
                if (child.HasControls()) GetControlsCollection(child, ref AllControls, filter);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("firstname", this.txtFirstName.Text.Trim());
            collection.Add("lastname", this.txtLastName.Text.Trim());
            collection.Add("email", this.txtEmailAddress.Text.Trim());
            collection.Add("contact", this.txtContactNumber.Text.Trim());
            collection.Add("profession", this.ddlProfession.SelectedValue);
            collection.Add("about", this.txtAboutDiscription.Text.Trim());
            collection.Add("userid", this.UserId);
            collection.Add("companyid", this.CompanyId);
            collection.Add("addressoptional", this.txtAddressOptional.Text.Trim());

            AddressBA objBreed = new AddressBA();
            bool success = false;
            int contactid = this.ConvertToInteger(ViewState["id"]);
            if (contactid > 0)
            {
                success = objBreed.UpdateContact(collection, ViewState["id"]);
            }
            else
            {
                contactid = objBreed.AddContact(collection);
                success = (contactid > 0);
            }

            if (success)
            {
                objBreed.DeleteOtherEmailPhoneInContact(contactid);

                int txtboxCount = this.ConvertToInteger(ViewState["indexVal"]);
                if (txtboxCount >= 0)
                {
                    List<Control> resultControlList = new List<Control>();
                    this.GetControlsCollection(this.pnlMatrixTextBoxes, ref resultControlList, new Func<Control, Control>(ctr => (ctr is TextBox) ? ctr : null));

                    NameValueCollection collection2 = new NameValueCollection();
                    foreach (Control cnt in resultControlList)
                    {
                        TextBox txt = cnt as TextBox;
                        if (txt == null || txt.Text.Trim().Length == 0) continue;

                        collection2["email"] = string.Empty;
                        collection2["phone"] = string.Empty;

                        collection2["type"] = txt.Attributes["data-type"];
                        if (txt.ID.Contains("txtOtherContacts_Email_"))
                        {
                            collection2["email"] = txt.Text.Trim();
                        }
                        else if (txt.ID.Contains("txtOtherContacts_Phone_"))
                        {
                            collection2["phone"] = txt.Text.Trim();
                        }
                        objBreed.AddOtherEmailPhoneInContact(collection2, contactid);
                    }
                }

                Response.Redirect("bucontactlist.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            this.panelView.Visible = false;
            this.lnkEdit.Visible = false;
            this.btnBack.Visible = false;
            this.panelEdit.Visible = true;
            this.PopulateOtherContacts();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (this.ConvertToInteger(ViewState["id"]) > 0)
            {
                this.panelView.Visible = true;
                this.btnBack.Visible = true;
                this.lnkEdit.Visible = true;
                this.lnkEdit.Text = "<i class=\"fa-solid fa-pen-to-square\"></i>&nbsp;Edit";
                this.panelEdit.Visible = false;
                this.PopulateOtherContacts();
            }
            else
                Response.Redirect("bucontactlist.aspx");
        }

        protected void btnAddTxtBox_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int indxVal = this.ConvertToInteger(ViewState["indexVal"]);
            indxVal = indxVal + 1;
            this.CreateTextBox(indxVal, e.CommandArgument.ToString(), "", "");
        }

        protected void btnBack1_Click(object sender, EventArgs e)
        {
            Response.Redirect("bucontactlist.aspx");
        }
    }
}