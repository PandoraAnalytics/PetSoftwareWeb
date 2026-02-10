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
    public partial class checkpointmanage : PageBase
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
            //this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = this.DecryptQueryString("id");
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

            int checkpointid = this.ConvertToInteger(ViewState["id"]);
            if (checkpointid > 0)
            {
                NameValueCollection collection = CustomFields.GetCustomFields(ViewState["id"]);
                if (collection == null) Response.Redirect("checkpoint.aspx");

                this.txtQTitle.Text = collection["title"];
                ViewState["type"] = collection["type"];
                this.ddlIsMandatory.SelectedValue = collection["ismandatory"];
                this.ddlType.SelectedValue = collection["type"];


                switch (collection["type"])
                {
                    case "1":
                        this.panelList.Visible = false;
                        break;

                    case "2":
                        this.panelList.Visible = false;
                        break;

                    case "3":
                        this.panelList.Visible = true;
                        this.PopulateQuestionOptions();
                        break;

                    case "4":
                        this.panelList.Visible = true;
                        this.PopulateQuestionOptions();
                        break;

                    case "5":
                        this.panelList.Visible = true;
                        this.PopulateQuestionOptions();
                        break;

                    case "6":
                        this.panelList.Visible = false;
                        break;

                    case "7":
                        this.panelList.Visible = false;
                        this.panelRange.Visible = true;
                        this.txtRangeMinVal.Text = collection["minval"];
                        this.txtRangeMaxVal.Text = collection["maxval"];
                        break;

                    case "8":
                        this.panelList.Visible = false;
                        this.panelMatrix.Visible = true;
                        this.txtRowsValue.Text = collection["rowvalue"];
                        break;

                    case "9":
                        this.panelList.Visible = false;
                        break;

                    case "10":
                        this.panelList.Visible = false;
                        break;

                }
            }

        }

        private void PopulateQuestionOptions()
        {
            DataTable dataTable = CustomFields.GetCustomFieldsOptions(ViewState["id"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = i + 1;
                    TextBox txt = panelList.FindControl("txtVal" + j) as TextBox;
                    if (txt != null)
                    {
                        string id = BASecurity.Encrypt(dataTable.Rows[i]["id"].ToString(), PageBase.HashKey);
                        txt.Attributes.Add("data-id", id);
                        txt.Text = this.ConvertToString(dataTable.Rows[i]["optiontext"]);
                    }
                }
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
            List<string> uniqueIdList = new List<string>(9);

            int questionType = this.ConvertToInteger(this.ddlType.SelectedValue);

            if (questionType == (int)CustomFields.enum_customfieldsType.List || questionType == (int)CustomFields.enum_customfieldsType.Multiselect || questionType == (int)CustomFields.enum_customfieldsType.Singleselect)
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
                    return;
                }
            }
            else if (questionType == (int)CustomFields.enum_customfieldsType.Matrix)
            {
                int txtboxCount = this.ConvertToInteger(ViewState["indexval"]);
                if (txtboxCount < 0) txtboxCount = 0;
                txtboxCount = txtboxCount + 1;

                myList = new List<string>(txtboxCount); // reintialize
                uniqueIdList = new List<string>(txtboxCount);

                myList.Add(this.txtColumnVal1.Text.Trim());
                uniqueIdList.Add(this.txtColumnVal1.UniqueID.Trim());

                for (int i = 2; i <= txtboxCount; i++) // start from 2, 1st is always visible
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

            string fieldId1 = this.ConvertToString(ViewState["id"]);

            if (this.ConvertToInteger(fieldId1) > 0)
            {
                List<NameValueCollection> insertList = new List<NameValueCollection>(7);
                List<NameValueCollection> updateList = new List<NameValueCollection>(7);
                List<NameValueCollection> deleteList = new List<NameValueCollection>(7);
                int selectedtype = this.ConvertToInteger(ViewState["type"]);
                if (selectedtype == (int)CustomFields.enum_customfieldsType.List || selectedtype == (int)CustomFields.enum_customfieldsType.Singleselect || selectedtype == (int)CustomFields.enum_customfieldsType.Multiselect)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        TextBox txt = panelList.FindControl("txtVal" + i) as TextBox;
                        if (txt == null) continue;

                        string id = txt.Attributes["data-id"];
                        string value = txt.Text.Trim();
                        if (string.IsNullOrEmpty(id))
                        {
                            if (value.Length > 0)
                            {
                                NameValueCollection col = new NameValueCollection();
                                col.Add("fieldid", fieldId1);
                                col.Add("optiontext", value);

                                insertList.Add(col);
                            }
                        }
                        else
                        {
                            if (value.Length > 0)
                            {
                                NameValueCollection col = new NameValueCollection();
                                col["id"] = BASecurity.Decrypt(id, PageBase.HashKey);
                                col["optiontext"] = value;

                                updateList.Add(col);
                            }
                            else
                            {
                                NameValueCollection col = new NameValueCollection();
                                col["id"] = BASecurity.Decrypt(id, PageBase.HashKey);

                                deleteList.Add(col);
                            }
                        }
                    }

                    int count = insertList.Count + updateList.Count + deleteList.Count;
                    if (count < 2)
                    {
                        this.lblError.Text = Resources.Resource.enter2optionstocontinue;
                        return;
                    }
                }

                else if (selectedtype == (int)CustomFields.enum_customfieldsType.Matrix)
                {
                    int txtboxCount = this.ConvertToInteger(ViewState["indexval"]);

                    insertList = new List<NameValueCollection>(txtboxCount);
                    updateList = new List<NameValueCollection>(txtboxCount);
                    deleteList = new List<NameValueCollection>(txtboxCount);

                    for (int i = 1; i <= txtboxCount; i++)
                    {
                        TextBox txt = this.pnlMatrixTextBoxes.FindControl("txtColumnVal" + i) as TextBox;
                        if (txt == null) continue;

                        string id = txt.Attributes["data-id"];
                        string value = txt.Text.Trim();
                        if (string.IsNullOrEmpty(id))
                        {
                            if (value.Length > 0)
                            {
                                NameValueCollection col = new NameValueCollection();
                                col.Add("fieldid", fieldId1);
                                col.Add("optiontext", value);
                                col.Add("optionkey", txt.UniqueID);
                                insertList.Add(col);
                            }
                        }
                        else
                        {
                            if (value.Length > 0)
                            {
                                NameValueCollection col = new NameValueCollection();
                                col["id"] = id;
                                col["optiontext"] = value;

                                updateList.Add(col);
                            }
                            else
                            {
                                NameValueCollection col = new NameValueCollection();
                                col["id"] = id;

                                deleteList.Add(col);
                            }
                        }
                    }
                }

                bool success = objCustomFields.Update(collection, fieldId1);
                if (success)
                {
                    foreach (NameValueCollection col1 in insertList)
                    {
                        success = objCustomFields.AddOptions(col1);
                    }

                    foreach (NameValueCollection col2 in updateList)
                    {
                        success = objCustomFields.UpdateOptions(col2);
                    }

                    foreach (NameValueCollection col3 in deleteList)
                    {
                        success = objCustomFields.DeleteOption(col3["id"]);
                    }
                }
                if (success)
                {
                    Response.Redirect("checkpoint.aspx");
                }
            }
            else
            {
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

            this.pnlMatrixTextBoxes.Controls.Clear();

            this.txtRangeMinVal.Text = string.Empty;
            this.txtRangeMaxVal.Text = string.Empty;

            Response.Redirect("checkpoint.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("checkpoint.aspx");
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