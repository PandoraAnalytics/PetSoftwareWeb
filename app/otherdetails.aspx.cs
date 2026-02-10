using BABusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class otherdetails : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                // ViewState["id"] = DecryptQueryString();
                ViewState["id"] = this.ReadQueryString("id");
                (Page.Master as breeder).AnimalId = ViewState["id"].ToString();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string animalId = this.ReadQueryString("id");

            NameValueCollection collection = AnimalBA.GetAnimalDetail(animalId);
            if (collection == null) Response.Redirect("landing.aspx");
            ViewState["breedtype"] = collection["breedtype"];

            string hidedit = Request.QueryString["edit"];
            if (hidedit == "1")
            {
                this.panelView.Visible = false;
                this.lnkEdit.Visible = false;
                this.panelEdit.Visible = true;

                this.PopulateCustomFieldsEdit(collection["breedtype"], animalId);
            }
            else
            {
                this.panelView.Visible = true;
                this.lnkEdit.Visible = true;
                this.panelEdit.Visible = false;

                //this.PopulateCustomFieldsReadonly(collection["breedtype"], animalId);
            }
        }

        private void PopulateCustomFieldsReadonly(string xiBreedType, string xiAnimalId)
        {
            DataTable dataTable = AnimalBA.GetCustomFields(xiBreedType, xiAnimalId);
            if (dataTable == null || dataTable.Rows.Count == 0) return;

            string controlhtml1 = "<div class='row form-group'><div class='form_label col-lg-2 col-md-2 col-sm-12 col-xs-12'><label>{0} <span>:</span></label></div><div class='col-lg-8 col-md-8 col-sm-12 col-xs-12'><span>{1}</span></div><div class='col-lg-2 col-md-2 col-sm-12 col-xs-12'><a href='javascript:void(0);' onclick='addcomments({2})'>Comments ({3})</a></div></div>";

            string controlhtml = "<div class='col-lg-6 col-md-6 col-sm-6 col-xs-12'><span class='form_label'>{0}</span><span>{1}</span></div>";

            string temp = string.Empty;
            for (int i = 0; i < dataTable.Rows.Count; i = i + 2)
            {
                if (i >= dataTable.Rows.Count) break;

                DataRow row1 = dataTable.Rows[i];

                string value1 = this.ConvertToString(row1["optiontext"]);
                if (string.IsNullOrEmpty(value1)) value1 = this.ConvertToString(row1["fieldvalue"]);
                if (!string.IsNullOrEmpty(value1))
                {
                    var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    foreach (Match m in linkParser.Matches(value1))
                    {
                        value1 = value1.Replace(m.Value, "<a href='" + m.Value + "' target='link_1'>" + m.Value + "</a>");
                    }
                    value1 = this.nl2br(value1);
                }
                else value1 = "-";
                temp += string.Format(controlhtml, row1["title"], value1);

                if (i + 1 < dataTable.Rows.Count)
                {
                    DataRow row2 = dataTable.Rows[i + 1];

                    string value2 = this.ConvertToString(row2["optiontext"]);
                    if (string.IsNullOrEmpty(value2)) value2 = this.ConvertToString(row2["fieldvalue"]);
                    if (!string.IsNullOrEmpty(value2))
                    {
                        var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        foreach (Match m in linkParser.Matches(value2))
                        {
                            value2 = value2.Replace(m.Value, "<a href='" + m.Value + "' target='link_1'>" + m.Value + "</a>");
                        }

                        value2 = this.nl2br(value2);
                    }
                    else value2 = "-";
                    temp += string.Format(controlhtml, row2["title"], value2);
                }
                this.lit.Text += "<div class='row form_row'>" + temp + "</div>";

                temp = string.Empty;
            }
            temp = string.Empty;
        }


        protected void PopulateCustomFieldsEdit(string xiBreedType, string xiAnimalId)
        {
            DataTable dataTable = AnimalBA.GetCustomFields(xiBreedType, xiAnimalId);
            if (dataTable == null || dataTable.Rows.Count == 0) return;

            Control ctrl = null;
            ArrayList validation = new ArrayList();
            int index = 0;
            int totalRows = dataTable.Rows.Count;

            for (int indexi = 0; indexi < totalRows; indexi = indexi + 2)
            {
                string rule = string.Empty;

                Panel mainDiv = new Panel();
                mainDiv.Attributes.Add("class", "row form_row");

                for (int j = indexi; j < indexi + 2; j++)
                {
                    if (j >= totalRows) break;

                    validation.Clear();

                    DataRow row = dataTable.Rows[j];
                    index = j + 1;

                    int queType = this.ConvertToInteger(row["type"]);
                    int mandatory = this.ConvertToInteger(row["ismandatory"]);

                    if (mandatory == 2) validation.Add("required");

                    Panel div1 = new Panel();
                    div1.Attributes.Add("class", "col-lg-6 col-md-6 col-sm-6 col-xs-12");

                    /*string moreinfo = this.ConvertToString(row["moreinfo"]);
                    string moreinfocontrol = string.Empty;
                    if (!string.IsNullOrEmpty(moreinfo))
                    {
                        string moreinfo_link = this.ConvertToString(row["moreinfo_link"]);

                        if (!string.IsNullOrEmpty(moreinfo_link)) moreinfo_link = "<a href='" + moreinfo_link + "' target='ext'>View Supportive Document</a>";
                        moreinfocontrol = string.Format("<div class='custom_tooltip'><i class='fa fa-info-circle' aria-hidden='true'></i><span><p>{0}</p>{1}</span></div>", moreinfo, moreinfo_link);
                    }

                    string star = (mandatory == 2) ? "*" : "";*/

                    Literal lit = new Literal();
                    lit.Text = string.Format("<span class='form_label'>{0}</span>", row["title"]);
                    div1.Controls.Add(lit);

                    switch (queType)
                    {
                        case (int)AnimalBA.enum_customfieldsType.OneLine:
                            TextBox txt = new TextBox();
                            txt.ID = "custom_id_" + index;
                            txt.MaxLength = (row["maxlength"] != DBNull.Value) ? this.ConvertToInteger(row["maxlength"]) : 500;
                            txt.CssClass = "form_input custom_element";
                            if (validation.Count > 0) txt.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));
                            txt.Attributes.Add("data-id", row["id"].ToString());
                            txt.Text = this.ConvertToString(row["fieldValue"]);
                            ctrl = txt;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Paragraph:
                            TextBox paragraph = new TextBox();
                            paragraph.ID = "custom_id_" + index;
                            paragraph.TextMode = TextBoxMode.MultiLine;
                            paragraph.MaxLength = (row["maxlength"] != DBNull.Value) ? this.ConvertToInteger(row["maxlength"]) : 1000;
                            paragraph.CssClass = "form_input";

                            if (validation.Count > 0) paragraph.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            paragraph.Attributes.Add("data-id", row["id"].ToString());
                            paragraph.Text = this.ConvertToString(row["fieldValue"]);
                            ctrl = paragraph;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.List:
                            DropDownList list = new DropDownList();
                            list.ID = "custom_id_" + index;
                            list.CssClass = "form_input custom_element";
                            list.Items.Add(new ListItem(Resources.Resource.Select, ""));
                            DataTable optionsTable1 = AnimalBA.GetCustomFieldsOptions(row["id"]);
                            if (optionsTable1 != null)
                            {
                                foreach (DataRow optionRow in optionsTable1.Rows)
                                {
                                    if (!string.IsNullOrEmpty(this.ConvertToString(optionRow["optiontext"]))) list.Items.Add(new ListItem(optionRow["optiontext"].ToString(), optionRow["id"].ToString()));
                                }
                            }

                            if (validation.Count > 0) list.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            list.Attributes.Add("data-id", row["id"].ToString());
                            list.SelectedValue = this.ConvertToString(row["fieldValue"]);

                            ctrl = list;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Singleselect:
                            RadioButtonList rbdlist = new RadioButtonList();
                            rbdlist.ID = "custom_id_" + index;
                            rbdlist.CssClass = "table datatable customtable";
                            rbdlist.RepeatDirection = RepeatDirection.Horizontal;
                            rbdlist.RepeatColumns = 3;

                            DataTable optionsTable2 = AnimalBA.GetCustomFieldsOptions(row["id"]);
                            if (optionsTable2 != null)
                            {
                                foreach (DataRow optionRow in optionsTable2.Rows)
                                {
                                    if (!string.IsNullOrEmpty(this.ConvertToString(optionRow["optiontext"]))) rbdlist.Items.Add(new ListItem(optionRow["optiontext"].ToString(), optionRow["id"].ToString()));
                                }
                            }

                            if (validation.Count > 0) rbdlist.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            rbdlist.Attributes.Add("data-id", row["id"].ToString());
                            rbdlist.SelectedValue = this.ConvertToString(row["fieldValue"]);

                            ctrl = rbdlist;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Multiselect:
                            CheckBoxList chklist = new CheckBoxList();
                            chklist.ID = "custom_id_" + index;
                            chklist.CssClass = "table datatable customtable";
                            chklist.RepeatDirection = RepeatDirection.Horizontal;
                            chklist.RepeatColumns = 3;

                            DataTable optionsTable3 = AnimalBA.GetCustomFieldsOptions(row["id"]);
                            if (optionsTable3 != null)
                            {
                                foreach (DataRow optionRow in optionsTable3.Rows)
                                {
                                    if (!string.IsNullOrEmpty(this.ConvertToString(optionRow["optiontext"]))) chklist.Items.Add(new ListItem(optionRow["optiontext"].ToString(), optionRow["id"].ToString()));
                                }
                            }

                            if (validation.Count > 0) chklist.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            chklist.Attributes.Add("data-id", row["id"].ToString());

                            string[] values = this.ConvertToString(row["fieldvalue"]).Split(',');
                            if (values != null)
                            {
                                foreach (ListItem item in chklist.Items)
                                {
                                    item.Selected = values.Contains(item.Value);
                                }
                            }
                            ctrl = chklist;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Fileupload:
                            FileUpload upload = new FileUpload();
                            upload.ID = "custom_id_" + index;
                            upload.CssClass = "filestyle form_input file_caption custome_file_upload";
                            string extensionRule = string.Empty;

                            string[] allExtensions = new string[6];
                            allExtensions[0] = "Image";
                            allExtensions[1] = "Document";
                            allExtensions[2] = "Excel";
                            allExtensions[3] = "PDF";
                            allExtensions[4] = "Powerpoint";
                            allExtensions[5] = "Text File";

                            string fileextension = this.ConvertToString(row["fileextension"]);

                            upload.Attributes.Add("data-extensions", fileextension);
                            upload.Attributes.Add("data-id", row["id"].ToString());
                            if (validation.Count > 0) upload.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            ctrl = upload;

                            HtmlInputHidden hidfile = new HtmlInputHidden();
                            hidfile.ID = "file_" + upload.ID;
                            hidfile.Value = this.ConvertToString(row["fieldValue"]);
                            div1.Controls.Add(hidfile);

                            break;

                        case (int)AnimalBA.enum_customfieldsType.Range:
                            TextBox txt1 = new TextBox();
                            txt1.ID = "custom_id_" + index;
                            txt1.MaxLength = (row["maxlength"] != DBNull.Value) ? this.ConvertToInteger(row["maxlength"]) : 100;
                            txt1.CssClass = "form_input custom_element";

                            validation.Add("pnumber");
                            validation.Add("minval-" + row["minval"]);
                            validation.Add("maxval-" + row["maxval"]);

                            if (validation.Count > 0) txt1.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            txt1.Attributes.Add("data-id", row["id"].ToString());
                            txt1.Text = this.ConvertToString(row["fieldValue"]);

                            ctrl = txt1;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Date:
                            TextBox datetxt = new TextBox();
                            datetxt.ID = "custom_id_" + index;
                            datetxt.MaxLength = 10;
                            datetxt.CssClass = "form_input custom_element date-picker";

                            validation.Add("date");
                            if (validation.Count > 0) datetxt.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            datetxt.Attributes.Add("data-id", row["id"].ToString());
                            datetxt.Text = this.ConvertToString(row["fieldValue"]);

                            ctrl = datetxt;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Time:
                            TextBox timetxt = new TextBox();
                            timetxt.ID = "custom_id_" + index;
                            timetxt.MaxLength = 5;
                            timetxt.CssClass = "form_input custom_element time-picker bootstrap-timepicker";

                            validation.Add("time");
                            if (validation.Count > 0) timetxt.Attributes.Add("data-validate", string.Join(" ", validation.ToArray()));

                            timetxt.Attributes.Add("data-id", row["id"].ToString());
                            timetxt.Text = this.ConvertToString(row["fieldValue"]);
                            ctrl = timetxt;
                            break;

                        case (int)AnimalBA.enum_customfieldsType.Matrix:
                            Table mTable = new Table();
                            mTable.ID = "custom_id_" + index;
                            mTable.CssClass = "table datatable tblDynamic";
                            mTable.Attributes.Add("data-id", row["id"].ToString());

                            int rowValue = this.ConvertToInteger(row["rowvalue"]);
                            DataTable colTable = AnimalBA.GetCustomFieldsOptions(row["id"]);
                            if (rowValue > 0 && colTable != null)
                            {
                                int totalcols = colTable.Rows.Count;

                                double width = (100 / totalcols);

                                TableRow tblRow = new TableRow();

                                for (int j1 = 0; j1 < totalcols; j1++)
                                {
                                    TableHeaderCell tblCellHeader = new TableHeaderCell();
                                    tblCellHeader.Text = this.ConvertToString(colTable.Rows[j1]["optiontext"]);
                                    tblCellHeader.Width = Unit.Percentage(width);
                                    tblRow.Cells.Add(tblCellHeader);
                                }
                                mTable.Rows.Add(tblRow);

                                string jsonString = row["fieldvalue"].ToString();
                                List<List<string>> matrixData = null;
                                var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(jsonString);

                                //for (int i = 0; i <= rowValue; i++)
                                for (int i = 0; i < rowValue; i++)
                                {
                                    TableRow tblRow2 = new TableRow();
                                    for (int j1 = 0; j1 < totalcols; j1++)
                                    {
                                        TextBox colText = new TextBox();
                                        colText.ID = "txtColumnVal" + ((i * totalcols) + j1);
                                        colText.MaxLength = 100;
                                        colText.CssClass = "form_input";
                                        if (jsonData != null && jsonData.TryGetValue("data", out matrixData))
                                        {
                                            if (matrixData != null && i < matrixData.Count && j1 < matrixData[i].Count)
                                            {
                                                colText.Text = matrixData[i][j1];
                                            }
                                        }

                                        TableCell tblCellValue = new TableCell();
                                        tblCellValue.Controls.Add(colText);
                                        tblRow2.Cells.Add(tblCellValue);
                                    }
                                    mTable.Rows.Add(tblRow2);
                                }
                            }

                            Panel matrixdiv = new Panel();
                            matrixdiv.Attributes.Add("class", "table-responsive");
                            matrixdiv.Controls.Add(mTable);

                            ctrl = matrixdiv;
                            break;

                    }

                    div1.Controls.Add(ctrl);


                    mainDiv.Controls.Add(div1);
                }

                this.pnlCustomFieldsEdit.Controls.Add(mainDiv);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("otherdetails.aspx?id=" + ViewState["id"] + "&edit=1");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("otherdetails.aspx?id=" + ViewState["id"]);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            NameValueCollection customCollection = new NameValueCollection();
            customCollection["animalid"] = ViewState["id"].ToString();
            customCollection["userid"] = this.UserId;

            for (int i = 1; i < 80; i++)
            {
                Control ctrl = this.Master.FindControl("ContentPlaceHolder1").FindControl("custom_id_" + i);
                if (ctrl == null) continue;

                Type ctrlType = ctrl.GetType();

                if (ctrlType == typeof(TextBox))
                {
                    TextBox temp = ctrl as TextBox;

                    customCollection["fieldid"] = temp.Attributes["data-id"];
                    customCollection["fieldvalue"] = temp.Text.Trim();
                }
                else if (ctrlType == typeof(DropDownList) || ctrlType == typeof(CheckBoxList) || ctrlType == typeof(RadioButtonList))
                {
                    ListControl temp = ctrl as ListControl;

                    string value = string.Empty;
                    foreach (ListItem item in temp.Items)
                    {
                        if (!item.Selected) continue;

                        if (value.Length > 0) value += ",";
                        value += item.Value;
                    }

                    customCollection["fieldid"] = temp.Attributes["data-id"];
                    customCollection["fieldvalue"] = value;
                }
                else if (ctrlType == typeof(FileUpload))
                {
                    FileUpload postedFile = ctrl as FileUpload;

                    string id = "file_" + postedFile.ID;
                    HtmlInputHidden hidfile = this.Master.FindControl("ContentPlaceHolder1").FindControl(id) as HtmlInputHidden;
                    string value = (hidfile != null) ? hidfile.Value : string.Empty;

                    customCollection["fieldid"] = postedFile.Attributes["data-id"];
                    customCollection["fieldvalue"] = value;
                }
                else if (ctrlType == typeof(Panel))
                {
                    Panel p = ctrl as Panel;
                    foreach (Control cntr in p.Controls)
                    {
                        if (cntr.GetType() == typeof(CheckBoxList))
                        {
                            ListControl temp = cntr as ListControl;

                            string value = string.Empty;
                            foreach (ListItem item in temp.Items)
                            {
                                if (!item.Selected) continue;

                                if (value.Length > 0) value += ",";
                                value += item.Value;
                            }

                            customCollection["fieldid"] = p.Attributes["data-id"];
                            customCollection["fieldvalue"] = value;
                        }
                    }
                }
                else if (ctrlType == typeof(Table))
                {
                    ArrayList array1 = new ArrayList();
                    string value = string.Empty;

                    Table table = ctrl as Table;
                    for (int i1 = 0; i1 < table.Rows.Count; i1++)
                    {
                        ArrayList array2 = new ArrayList();

                        for (int j2 = 0; j2 < table.Rows[i1].Cells.Count; j2++)
                        {
                            int id = (i1 * (table.Rows[i1].Cells.Count)) + j2;

                            Control ctrl2 = table.Rows[i1].FindControl("txtColumnVal" + id);
                            if (ctrl2 == null || ctrl2.GetType() != typeof(TextBox)) continue;

                            array2.Add((ctrl2 as TextBox).Text.Trim());
                        }
                        if (array2.Count > 0) array1.Add(array2);
                    }

                    if (array1.Count > 0)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic["data"] = array1;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        value = serializer.Serialize(dic);
                        dic = null;
                    }

                    customCollection["fieldid"] = table.Attributes["data-id"];
                    customCollection["fieldvalue"] = value;
                }

                AnimalBA.SaveCustomFields(customCollection);
            }

            NameValueCollection logCollection = new NameValueCollection();
            logCollection["animalid"] = ViewState["id"].ToString();
            logCollection["key"] = Common.AnimalLogKey.EDITOTHERINFO.ToString();
            logCollection["category"] = Common.AnimalLogCategory.OTHERINFO.ToString();
            logCollection["description"] = null;
            logCollection["userid"] = this.UserId;
            Common.SaveAnimalLog(logCollection);

            Response.Redirect("otherdetails.aspx?id=" + ViewState["id"]);
        }
    }
}