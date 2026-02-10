using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class bucustomfieldsedit : ERPBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            string questionId = DecryptQueryString();

            List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtColumnVal")).ToList();
            int i = 1;
            foreach (string key in keys)
            {
                int columnId = AnimalBA.GetColumnId(questionId, key);
                this.CreateTextBox(i, columnId, string.Empty);
                i++;
            }
        }
        override protected void Page_Load(object sender, EventArgs e)
        {
            this.IsAssociationAccess = true;
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = CustomFields.GetCustomFields(ViewState["id"]);
            if (collection == null) Response.Redirect("bucustomfields.aspx");

            string associationbreedertype = (Session["associationbreedertype"] != null) ? this.ConvertToString(Session["associationbreedertype"]) : string.Empty;
            if (!string.IsNullOrEmpty(associationbreedertype) && !string.IsNullOrEmpty(collection["breedtype"]))
            {
                string[] a_breedTypes1 = associationbreedertype.Split(',');
                string[] breedTypes1 = collection["breedtype"].Split(',');

                string[] intersect = a_breedTypes1.Intersect(breedTypes1).ToArray();
                if (intersect == null || intersect.Length == 0) Response.Redirect("bucustomfields.aspx");
            }

            this.ddlBreedType.DataSource = BreederData.GetAllBreedTypes(associationbreedertype);
            this.ddlBreedType.DataBind();

            this.txtQTitle.Text = collection["title"];
            ViewState["type"] = collection["type"];
            this.ddlIsMandatory.SelectedValue = collection["ismandatory"];

            string[] breedTypes = collection["breedtype"].Split(',');
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                item.Selected = breedTypes.Contains(item.Value);
            }

            switch (collection["type"])
            {
                case "1":
                    this.lblType.Text = Resources.Resource.OneLine;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;

                case "2":
                    this.lblType.Text = Resources.Resource.Paragraph;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;

                case "3":
                    this.lblType.Text = Resources.Resource.List;
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    this.PopulateQuestionOptions();
                    break;

                case "4":
                    this.lblType.Text = Resources.Resource.SingleSelect;
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    this.PopulateQuestionOptions();
                    break;

                case "5":
                    this.lblType.Text = Resources.Resource.MultiSelect;
                    this.panelList.Visible = true;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    this.PopulateQuestionOptions();
                    break;

                case "6":
                    this.lblType.Text = Resources.Resource.FileUpload;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;

                case "7":
                    this.lblType.Text = Resources.Resource.Range;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = true;
                    this.panelMatrix.Visible = false;
                    this.txtRangeMinVal.Text = collection["minval"];
                    this.txtRangeMaxVal.Text = collection["maxval"];
                    break;

                case "8":
                    this.lblType.Text = Resources.Resource.Matrix;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = true;
                    this.txtRowsValue.Text = collection["rowvalue"];
                    this.PopulateMatrixColumn();
                    break;

                case "9":
                    this.lblType.Text = Resources.Resource.Date;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;

                case "10":
                    this.lblType.Text = Resources.Resource.Time;
                    this.panelList.Visible = false;
                    this.panelRange.Visible = false;
                    this.panelMatrix.Visible = false;
                    break;
            }
        }
        private void PopulateMatrixColumn()
        {
            DataTable dataTable = AnimalBA.GetCustomFieldsOptions(ViewState["id"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                AnimalBA.AdjustOptionKeys(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    this.CreateTextBox((i + 1), this.ConvertToInteger(dataTable.Rows[i]["id"]), this.ConvertToString(dataTable.Rows[i]["optiontext"]));
                }
            }
        }

        private void PopulateQuestionOptions()
        {
            //DataTable dataTable = CustomFields.GetCustomFieldsOptions(ViewState["id"]);
            DataTable dataTable = AnimalBA.GetCustomFieldsOptions(ViewState["id"]);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.lblError.Text = "";

            string fieldId = this.ConvertToString(ViewState["id"]);

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
                        if (value.Length > 0) // insert
                        {
                            NameValueCollection col = new NameValueCollection();
                            col.Add("fieldid", fieldId);
                            col.Add("optiontext", value);

                            insertList.Add(col);
                        }
                    }
                    else
                    {
                        if (value.Length > 0) // update
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
            else if (selectedtype == (int)CustomFields.enum_customfieldsType.Range)
            {
                int minrange = this.ConvertToInteger(this.txtRangeMinVal.Text.Trim());
                int maxrange = this.ConvertToInteger(this.txtRangeMaxVal.Text.Trim());

                if (minrange >= maxrange)
                {
                    lblError.Text = "Min range value should be less than max range value";
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
                        if (value.Length > 0) // insert
                        {
                            NameValueCollection col = new NameValueCollection();
                            col.Add("fieldid", fieldId);
                            col.Add("optiontext", value);
                            col.Add("optionkey", txt.UniqueID);
                            insertList.Add(col);
                        }
                    }
                    else
                    {
                        if (value.Length > 0) // update
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

            NameValueCollection collection = new NameValueCollection();
            collection.Add("title", this.txtQTitle.Text.Trim());
            collection.Add("ismandatory", this.ddlIsMandatory.SelectedValue);
            collection.Add("isallowchar", "1");
            collection.Add("isallownumber", "1");
            collection.Add("isallowspchar", "1");
            collection.Add("sortorder", "1");
            collection.Add("moreinfo", "");
            collection.Add("moreinfo_link", "");
            string breedtypesIds = string.Empty;
            foreach (ListItem item in this.ddlBreedType.Items)
            {
                if (item.Selected == false) continue;
                if (breedtypesIds.Length > 0) breedtypesIds += ",";
                breedtypesIds += item.Value;
            }
            collection["breedtype"] = breedtypesIds;

            if (selectedtype == (int)CustomFields.enum_customfieldsType.Matrix)
            {
                collection.Add("rowvalue", this.txtRowsValue.Text.Trim());
            }
            if (selectedtype == (int)CustomFields.enum_customfieldsType.Range)
            {
                collection.Add("minval", this.txtRangeMinVal.Text.Trim());
                collection.Add("maxval", this.txtRangeMaxVal.Text.Trim());
            }
            else
            {
                collection.Add("minval", "");
                collection.Add("maxval", "");
            }
            collection.Add("fileextension", "");

            CustomFields objCustomFields = new CustomFields();
            bool success = objCustomFields.Update(collection, fieldId);

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
                Response.Redirect("bucustomfields.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("bucustomfields.aspx");
        }
        private void CreateTextBox(int xiIndexValue, int xiColumnId, string xiValue)
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
                lbl.CssClass = "form_label";
                this.pnlMatrixTextBoxes.Controls.Add(lbl);
                div1.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.CssClass = "form_input";
                txt.ID = "txtColumnVal" + xiIndexValue;
                txt.MaxLength = 50;
                if (xiColumnId > 0) txt.Attributes.Add("data-id", xiColumnId.ToString());
                txt.Text = xiValue;
                div2.Controls.Add(txt);

                if (xiIndexValue == 1)
                {
                    Button btnAddColumn = new Button();
                    btnAddColumn.Text = "Add More Columns";
                    btnAddColumn.CssClass = "form_button top_btn";
                    btnAddColumn.Attributes.Add("validate", "no");
                    btnAddColumn.Click += btnAddTxtBox_Click;
                    div2.Controls.Add(btnAddColumn);
                }

                mainDiv.Controls.Add(div1);
                mainDiv.Controls.Add(div2);
                this.pnlMatrixTextBoxes.Controls.Add(mainDiv);

                ViewState["indexval"] = xiIndexValue;
            }
        }

        protected void btnAddTxtBox_Click(object sender, EventArgs e)
        {
            int indxVal = this.ConvertToInteger(ViewState["indexval"]);
            indxVal = indxVal + 1;
            this.CreateTextBox(indxVal, int.MinValue, string.Empty);
        }
    }
}