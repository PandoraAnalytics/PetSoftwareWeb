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
    public partial class buposvieworder : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["orderid"] = this.DecryptQueryString("id");
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            NameValueCollection collection = BUOrderManagement.GetOrderDetails(ViewState["orderid"], this.CompanyId);
            if (collection != null)
            {
                this.lblCustomerName.Text = collection["cfname"] + " " + collection["clname"];
                this.lblOrderNo.Text = "#" + collection["orderno"].PadLeft(5, '0');
                if (string.IsNullOrEmpty(collection["orderdate"]) == false)
                {
                    DateTime dtTempDateTime = DateTime.Parse(collection["orderdate"]);
                    this.lblOrderDate.Text = dtTempDateTime.ToString(this.DateFormat);
                }
                switch (collection["status"])
                {
                    case "1":
                        this.lblStatus.Text = "Pending";
                        this.btnGenerateReport.Visible = false;
                        break;

                    case "2":
                        this.lblStatus.Text = "Completed";
                        this.btnGenerateReport.Visible = true;
                        break;

                    case "3":
                        this.lblStatus.Text = "Deleted";
                        this.btnGenerateReport.Visible = true;
                        break;
                }
                switch (collection["paymentoption"])
                {
                    case "1":
                        this.lblPaymentOption.Text = "Cash";
                        break;

                    case "2":
                        this.lblPaymentOption.Text = "Credit Card";
                        break;

                    case "3":
                        this.lblPaymentOption.Text = "Debit Card";
                        break;

                    case "4":
                        this.lblPaymentOption.Text = "Bank Transfer";
                        break;

                    case "5":
                        this.lblPaymentOption.Text = "E-Wallet";
                        break;

                    case "6":
                        this.lblPaymentOption.Text = "Mobile";
                        break;

                    case "7":
                        this.lblPaymentOption.Text = "Prepaid Cards";
                        break;
                }
                this.lblTermCondition.Text = collection["termscondition"];

                if (!string.IsNullOrEmpty(collection["referencefile"]))
                {
                    this.lnkFile.HRef = "../app/viewdocument.aspx?file=" + collection["referencefile"].ToString() + ""; //"docs/" + collection["referencefile"];
                }

                this.lblSubTotalCurrency.Text = this.GetCurrntBUCurrency();
                this.lblTotalTaxCurrency.Text = this.GetCurrntBUCurrency();
                this.lblTotalCurrency.Text = this.GetCurrntBUCurrency();

                PopulateOrderDetails();
            }
            else
            {
                Response.Redirect("buposcurrentorder.aspx");
            }
        }

        private void PopulateOrderDetails()
        {
            double dynamicTotal = 0;

            if (!string.IsNullOrEmpty(this.ConvertToString(ViewState["orderid"])))
            {               
                this.rptOrderItems.DataSource = BUOrderManagement.GetOrderItems(ViewState["orderid"]);
                this.rptOrderItems.DataBind();

                NameValueCollection costcollection = BUOrderManagement.GetCurrentOrderCost(ViewState["orderid"]);
                if (costcollection != null)
                {                    
                    this.lblSubTotal.Text = (Convert.ToDecimal(costcollection["total_item_cost"]).ToString("0.00")).Replace(",", ".");//costcollection["total_item_cost"].Replace(",", ".");
                    this.lblTotalTax.Text = (Convert.ToDecimal(costcollection["total_tax_amount"]).ToString("0.00")).Replace(",", ".");//costcollection["total_tax_amount"].Replace(",", ".");
                    this.lblTotalAmount.Text = (Convert.ToDecimal(costcollection["total_cost_with_tax"]).ToString("0.00")).Replace(",", "."); //costcollection["total_cost_with_tax"].Replace(",", ".");
                    ViewState["ordertotalamount"] = this.lblTotalAmount.Text;
                }
                else
                {
                    this.lblSubTotal.Text = "-";
                    this.lblTotalTax.Text = "-";
                    this.lblTotalAmount.Text = "-";
                }

                DataTable headTable = BUOrderManagement.GetHeadDetails(ViewState["orderid"]);
                if (costcollection != null && headTable != null && headTable.Rows.Count > 0)
                {
                    foreach (DataRow row in headTable.Rows)
                    {
                        double cost = this.ConvertToDouble(row["cost"].ToString().Replace(",", "."));
                        int isNegative = this.ConvertToInteger(row["isnegative"]);

                        if (isNegative == 1)
                        {
                            dynamicTotal -= cost;
                        }
                        else if (isNegative == 0)
                        {
                            dynamicTotal += cost;
                        }
                    }
                   
                    double d = this.ConvertToDouble(costcollection["total_cost_with_tax"].Replace(",", ".")) + this.ConvertToDouble(dynamicTotal);
                    this.lblTotalAmount.Text = (Convert.ToDecimal(d).ToString("0.00")).Replace(",", ".");
                    ViewState["ordertotalamount"] = d.ToString().Replace(",", ".");
                }
                this.rptDynamicCharges.DataSource = headTable;
                this.rptDynamicCharges.DataBind();
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("buposcurrentorder.aspx");
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = this.ProcessPrintOrderDetails(ViewState["orderid"]);
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
    }
}