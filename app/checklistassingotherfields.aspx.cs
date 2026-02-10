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
    public partial class checklistassingotherfields : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtColumnVal")).ToList();
            int i = 1;
            foreach (string key in keys)
            {
                if (i == 1)
                {
                    ViewState["indexval"] = 1;
                }
                else
                {
                    this.CreateTextBox(i);
                }
                i++;
            }
        }
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("id");
                ViewState["indexval"] = 1;
                this.PopulateControls();

                Control divMenu = Master.FindControl("checklistmainmenu");
                Control divUserMenu = Master.FindControl("usermenu");
                divUserMenu.Visible = false;
                divMenu.Visible = true;
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = Checklist.GetChecklist(ViewState["id"]);
            if (collection == null) Response.Redirect("checklist.aspx");

            this.lblTitle.Text = collection["name"];

            this.ddlType.Items.Add(new ListItem(Resources.Resource.Select, ""));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.OneLine, "1"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.Paragraph, "2"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.List, "3"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.SingleSelect, "4"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.MultiSelect, "5"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.FileUpload, "6"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.Range, "7"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.Matrix, "8"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.Date, "9"));
            this.ddlType.Items.Add(new ListItem(Resources.Resource.Time, "10"));

            this.PopulateAssignedOtherInfo();
        }

        private void PopulateAssignedOtherInfo()
        {
            DataTable dataTable = Checklist.GetAssignedOtherFields(ViewState["id"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.panelQuestions.Visible = true;
                this.panelNoQuestion.Visible = false;

                this.repPhotos.DataSource = dataTable;
                this.repPhotos.DataBind();

                string[] array = (dataTable != null) ? dataTable.Rows.Cast<DataRow>().Select(row => row["id"].ToString()).ToArray() : null;
                this.cplist1.Value = (array != null && array.Length > 0) ? string.Join(";", array) : string.Empty;
                array = null;
            }
            else
            {
                this.panelNoQuestion.Visible = true;
                this.panelQuestions.Visible = false;

                this.cplist1.Value = "";
                this.repPhotos.DataBind();
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ddlType.SelectedValue)
            {
                default:
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;

                case "3":
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;
                case "4":
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;
                case "5":
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;
                case "7":
                    this.panelList.Visible = false;
                    this.panelRange.Visible = true;
                    this.panelMatrix.Visible = false;
                    break;
                case "8":
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = true;
                    break;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            List<string> myList = new List<string>(9);
            List<string> uniqueIdList = new List<string>(0);

            int questionType = this.ConvertToInteger(this.ddlType.SelectedValue);

            if (questionType == (int)CustomFields.enum_customfieldsType.List || questionType == (int)CustomFields.enum_customfieldsType.Singleselect || questionType == (int)CustomFields.enum_customfieldsType.Multiselect)
            {
                bool chktxtval = false;
                string[] listValue = new string[7];

                for (int i = 1; i <= 8; i++)
                {
                    TextBox txt = (TextBox)panelList.FindControl("txtVal" + i);
                    if (txt != null)
                    {
                        if (!string.IsNullOrEmpty(txt.Text.Trim()))
                        {
                            chktxtval = true;
                            myList.Add(txt.Text.Trim());
                        }
                    }
                }
                if (!chktxtval)
                {
                    lblError.Text = Resources.Resource.enter2optionstocontinue;
                    return;
                }

                if (myList.Count < 2)
                {
                    lblError.Text = Resources.Resource.enter2optionstocontinue;
                    return;
                }
            }
            else if (questionType == (int)CustomFields.enum_customfieldsType.Range)
            {
                int minrange = this.ConvertToInteger(this.txtRangeMinVal.Text.Trim());
                int maxrange = this.ConvertToInteger(this.txtRangeMaxVal.Text.Trim());

                if (minrange >= maxrange)
                {
                    lblError.Text = "Min range value should be less than max range value";
                    //return;
                }
            }
            else if (questionType == (int)CustomFields.enum_customfieldsType.Matrix)
            {
                int txtboxCount = this.ConvertToInteger(ViewState["indexval"]);
                if (txtboxCount < 0) txtboxCount = 0;
                txtboxCount = txtboxCount + 1;

                myList = new List<string>(txtboxCount);
                uniqueIdList = new List<string>(txtboxCount);

                myList.Add(this.txtColumnVal1.Text.Trim());
                uniqueIdList.Add(this.txtColumnVal1.UniqueID.Trim());

                for (int i = 2; i <= txtboxCount; i++)
                {
                    TextBox txt = this.pnlMatrixTextBoxes.FindControl("txtColumnVal" + i) as TextBox;
                    if (txt != null && txt.Text.Trim().Length > 0)
                    {
                        myList.Add(txt.Text.Trim());
                        uniqueIdList.Add(txt.UniqueID.Trim());
                    }
                }
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("type", questionType.ToString());
            collection.Add("title", this.txtQTitle.Text.Trim());
            collection.Add("ismandatory", this.ddlIsMandatory.SelectedValue);
            collection.Add("sortorder", "1");
            collection["breedtype"] = "checklist";
            collection.Add("fileextension", "");
            collection.Add("createdby", this.UserId);
            if (questionType == (int)CustomFields.enum_customfieldsType.Matrix)
            {
                collection.Add("rowvalue", this.txtRowsValue.Text.Trim());
            }
            if (questionType == (int)CustomFields.enum_customfieldsType.Range)
            {
                collection.Add("minval", this.txtRangeMinVal.Text.Trim());
                collection.Add("maxval", this.txtRangeMaxVal.Text.Trim());
            }
            else
            {
                collection.Add("minval", "");
                collection.Add("maxval", "");
            }

            CustomFields objCustomFields = new CustomFields();
            int fieldId = objCustomFields.Add(collection);
            if (fieldId <= 0)
            {
                lblError.Text = Resources.Resource.error;
                return;
            }

            if (fieldId > 0 && myList.Count > 0)
            {
                for (int i = 0; i < myList.Count; i++)
                {
                    NameValueCollection collection2 = new NameValueCollection();
                    collection2.Add("fieldid", fieldId.ToString());
                    collection2.Add("optiontext", myList[i]);

                    if (uniqueIdList.Count > i)
                    {
                        collection2.Add("optionkey", uniqueIdList[i]);
                    }
                    else
                    {
                        collection2.Add("optionkey", string.Empty);
                    }

                    objCustomFields.AddOptions(collection2);
                }
            }

            this.ddlIsMandatory.SelectedIndex = 0;
            this.txtQTitle.Text = string.Empty;

            this.txtVal1.Text = string.Empty;
            this.txtVal2.Text = string.Empty;
            this.txtVal3.Text = string.Empty;
            this.txtVal4.Text = string.Empty;
            this.txtVal5.Text = string.Empty;
            this.txtVal6.Text = string.Empty;
            this.txtVal7.Text = string.Empty;
            this.txtVal8.Text = string.Empty;

            this.ddlType.SelectedValue = string.Empty;
            this.ddlType_SelectedIndexChanged(null, null);

            this.txtRangeMinVal.Text = string.Empty;
            this.txtRangeMaxVal.Text = string.Empty;
            this.txtRowsValue.Text = string.Empty;
            this.txtColumnVal1.Text = string.Empty;
            this.pnlMatrixTextBoxes.Controls.Clear();

            this.PopulateAssignedOtherInfo();
        }

        protected void btnAssignOtherInfo_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            if (this.cplist.Value.Length == 0)
            {
                this.lblError.Text = "Please select at least one other info from the list.";
                return;
            }

            string ckList = this.cplist.Value;
            if (ckList.StartsWith(";")) ckList = ckList.Substring(1);
            string[] ckListItems = ckList.Split(';');
            if (ckListItems == null || ckListItems.Length == 0)
            {
                this.lblError.Text = "Please select at least one other info from the list.";
                return;
            }

            Checklist obj = new Checklist();

            NameValueCollection collection = new NameValueCollection();
            collection["checklistid"] = ViewState["id"].ToString();
            foreach (string val in ckListItems)
            {
                if (string.IsNullOrEmpty(val)) continue;

                collection["fieldid"] = val;
                obj.AssignOtherFields(collection);
            }
            obj = null;
            cplist.Value = string.Empty;
            this.PopulateAssignedOtherInfo();
        }

        protected void repPhotos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Checklist obj = new Checklist();

            NameValueCollection collection = new NameValueCollection();
            collection["checklistid"] = ViewState["id"].ToString();
            collection["fieldid"] = e.CommandArgument.ToString();
            obj.UnAssignOtherFields(collection);
            obj = null;

            this.PopulateAssignedOtherInfo();
        }
        protected void btnAddTxtBox_Click(object sender, EventArgs e)
        {
            int indxVal = this.ConvertToInteger(ViewState["indexval"]);
            indxVal = indxVal + 1;
            this.CreateTextBox(indxVal);
        }
        private void CreateTextBox(int xiIndexValue)
        {
            string txtID = "txtColumnVal" + xiIndexValue;
            TextBox chkTxt = this.pnlMatrixTextBoxes.FindControl(txtID) as TextBox;
            if (chkTxt == null)
            {
                Panel mainDiv = new Panel();
                mainDiv.Attributes.Add("class", "row form_row");

                Panel div1 = new Panel();
                div1.Attributes.Add("class", "col-lg-12 col-md-12 col-sm-12 col-xs-12");

                Panel div2 = new Panel();
                div2.Attributes.Add("class", "col-lg-12 col-md-12 col-sm-12 col-xs-12");

                Label lbl = new Label();
                lbl.ID = "lblCol" + xiIndexValue;
                lbl.Text = "Column " + xiIndexValue + " Name <span>:</span>";
                lbl.CssClass = "form_label";//new
                this.pnlMatrixTextBoxes.Controls.Add(lbl);
                div1.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.CssClass = "form_input";
                txt.ID = "txtColumnVal" + xiIndexValue;
                txt.MaxLength = 50;

                div2.Controls.Add(txt);

                mainDiv.Controls.Add(div1);
                mainDiv.Controls.Add(div2);
                this.pnlMatrixTextBoxes.Controls.Add(mainDiv);

                ViewState["indexval"] = xiIndexValue;
            }
        }
    }
}