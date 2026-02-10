using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class buaddneworder : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["orderid"] = this.DecryptQueryString("id"); // edit case
                PopulateControls();
                PopulateOrderDetails();
            }
        }

        private void PopulateControls()
        {
            DataTable dtcustomer = BUOrderManagement.GetAllBUCustomer(this.CompanyId);
            if (dtcustomer != null)
            {
                DataRow row = dtcustomer.NewRow();
                row["id"] = int.MinValue;
                row["fullname"] = Resources.Resource.Select;
                dtcustomer.Rows.InsertAt(row, 0);
            }
            this.ddlCustomer.DataSource = dtcustomer;
            this.ddlCustomer.DataBind();

            DataTable dtproduct = BUOrderManagement.GetAllBUItem(this.CompanyId);
            if (dtproduct != null)
            {
                DataRow row = dtproduct.NewRow();
                row["id2"] = int.MinValue;
                row["ItemNameType"] = "- Select -";
                dtproduct.Rows.InsertAt(row, 0);
            }
            this.ddlProduct.DataSource = dtproduct;
            this.ddlProduct.DataBind();

            if (!string.IsNullOrEmpty(this.ConvertToString(ViewState["orderid"]))) // EDIT 
            {
                this.fsAddItems.Visible = true;
                if (!this.fsProductServiceInoTable.Visible) this.btnHead.Visible = false;

                NameValueCollection collection = BUOrderManagement.GetOrderDetails(ViewState["orderid"], this.CompanyId);
                if (collection != null)
                {
                    this.lblOrderNo.Text = "#" + collection["orderno"].PadLeft(5, '0');
                    this.ddlCustomer.SelectedValue = collection["customerid"];
                    if (this.ConvertToInteger(collection["paymentoption"]) > 0)
                    {
                        this.rdbPaymentOption.SelectedValue = collection["paymentoption"];
                    }
                    this.txtTermCondition.Text = collection["termscondition"];

                    if (string.IsNullOrEmpty(collection["orderdate"]) == false)
                    {
                        DateTime dtTempDateTime = DateTime.Parse(collection["orderdate"]);
                        this.txtOrderDate.Text = dtTempDateTime.ToString(this.DateFormat);
                    }
                }
                this.ddlCustomer.Enabled = false;
                this.txtOrderDate.Enabled = false;
                this.btnSave.Visible = false;
            }
            else
            {
                NameValueCollection nocollection = BUOrderManagement.GetCurrentOrderNo();
                this.lblOrderNo.Text = "#" + nocollection["currentorderno"].PadLeft(4, '0');
                this.txtOrderDate.Text = BusinessBase.Now.ToString(this.DateFormat);

                NameValueCollection bucollection = UserBA.GetBusinessUserDetail(this.CompanyId);
                if (bucollection != null)
                {
                    this.txtTermCondition.Text = bucollection["termscondition"];
                }
            }

            this.lblSubTotalCurrency.Text = this.GetCurrntBUCurrency();
            this.lblTotalTaxCurrency.Text = this.GetCurrntBUCurrency();
            this.lblTotalCurrency.Text = this.GetCurrntBUCurrency();
        }

        private void PopulateOrderDetails()
        {
            this.lblError.Text = string.Empty;
            double dynamicTotal = 0;

            if (string.IsNullOrEmpty(this.ConvertToString(ViewState["orderid"]))) return;

            this.rptOrderItems.DataSource = BUOrderManagement.GetOrderItems(ViewState["orderid"]);
            this.rptOrderItems.DataBind();

            NameValueCollection costcollection = BUOrderManagement.GetCurrentOrderCost(ViewState["orderid"]);
            if (costcollection != null)
            {
                this.panelOtherInfo.Visible = true;
                this.fsProductServiceInoTable.Visible = true;

                this.lblSubTotal.Text = (Convert.ToDecimal(costcollection["total_item_cost"]).ToString("0.00")).Replace(",", ".");//costcollection["total_item_cost"].Replace(",", ".");
                this.lblTotalTax.Text = (Convert.ToDecimal(costcollection["total_tax_amount"]).ToString("0.00")).Replace(",", ".");//costcollection["total_tax_amount"].Replace(",", ".");
                this.lblTotalAmount.Text = (Convert.ToDecimal(costcollection["total_cost_with_tax"]).ToString("0.00")).Replace(",", "."); //costcollection["total_cost_with_tax"].Replace(",", ".");
                ViewState["ordertotalamount"] = costcollection["total_cost_with_tax"].Replace(",", ".");
                this.btnHead.Visible = true;
            }
            else
            {
                this.lblSubTotal.Text = "-";
                this.lblTotalTax.Text = "-";
                this.lblTotalAmount.Text = "-";
                this.fsProductServiceInoTable.Visible = false;
                this.btnHead.Visible = false;
                this.panelOtherInfo.Visible = false;
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

                double d = this.ConvertToDouble(costcollection["total_cost_with_tax"]) + this.ConvertToDouble(dynamicTotal);
                this.lblTotalAmount.Text = (Convert.ToDecimal(d).ToString("0.00")).Replace(",", ".");
                ViewState["ordertotalamount"] = d.ToString().Replace(",", ".");
            }
            this.rptDynamicCharges.DataSource = headTable;
            this.rptDynamicCharges.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;

            NameValueCollection collection = new NameValueCollection();
            collection.Add("customerid", this.ddlCustomer.SelectedValue);
            collection.Add("orderdate", this.txtOrderDate.Text.Trim());
            collection.Add("status", "1");
            collection.Add("totalcost", "0");
            collection.Add("staffid", this.UserId);
            collection.Add("companyid", this.CompanyId);
            collection.Add("termscondition", this.txtTermCondition.Text.Trim());

            bool success = false;

            int orderid = BUOrderManagement.AddBUOrder(collection);
            success = (orderid > 0);
            if (success)
            {
                ViewState["orderid"] = orderid.ToString();
                PopulateControls();
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }

            // Response.Redirect("buaddneworder.aspx");
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            this.lblProductError.Text = string.Empty;
            int productid = this.ConvertToInteger(this.ddlProduct.SelectedValue);
            if (productid <= 0)
            {
                this.lblProductError.Text = "select product from the list";
                return;
            }

            int quantity = this.ConvertToInteger(this.txtQuantity.Text.Trim());
            if (quantity <= 0)
            {
                //this.ddlProduct.SelectedValue = null;
                this.lblProductError.Text = "Please enter quatity.";
                return;
            }

            NameValueCollection productcollection = BUOrderManagement.GetBUItem(this.ddlProduct.SelectedValue);
            if (productcollection == null)
            {
                this.lblProductError.Text = "Invalid product";
                return;
            }

            if (productcollection["itemtype"] == "P")
            {
                int availableStock = this.ConvertToInteger(productcollection["availablestock"]);
                if (quantity > availableStock)
                {
                    //this.ddlProduct.SelectedValue = null;
                    this.lblProductError.Text = "Quantity you have entered is not in stock";
                    return;
                }
            }


            double totalcost = this.ConvertToDouble(productcollection["cost"]) + this.ConvertToDouble(productcollection["taxamount"]);

            NameValueCollection collection = new NameValueCollection();
            collection.Add("orderid", ViewState["orderid"].ToString());
            collection.Add("itemid", this.ddlProduct.SelectedValue);
            collection.Add("productid", string.Empty);
            collection.Add("serviceid", string.Empty);
            collection.Add("comboid", string.Empty);

            switch (productcollection["itemtype"])
            {
                case "P":
                    collection["productid"] = productcollection["id"];
                    break;

                case "S":
                    collection["serviceid"] = productcollection["id"];
                    break;

                case "C":
                    collection["comboid"] = productcollection["id"];
                    break;
            }

            collection.Add("quan", quantity.ToString());
            collection.Add("itemcost", productcollection["cost"]);
            collection.Add("taxid", productcollection["taxid"]);
            collection.Add("taxpercentage", productcollection["taxpercentage"]);
            collection.Add("totalcost", totalcost.ToString());
            collection.Add("staffid", this.UserId);
            int itemid = BUOrderManagement.AddBUOrderItem(collection);
            if (itemid > 0)
            {
                this.fsProductServiceInoTable.Visible = true;
                this.ddlProduct.SelectedValue = null;
                this.txtQuantity.Text = string.Empty;
                this.btnHead.Visible = true;
                this.PopulateOrderDetails();
            }
            else
            {
                this.lblProductError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnAddHead_Click(object sender, EventArgs e)
        {
            this.lblHeadError.Text = "";
            if (string.IsNullOrEmpty(this.txtHeadName.Text.Trim()))
            {
                this.lblHeadError.Text = "Please select head name.";
                return;
            }
            if (string.IsNullOrEmpty(this.txtHeadCost.Text.Trim()))
            {
                this.lblHeadError.Text = "Please select head cost.";
                return;
            }

            if (this.ConvertToInteger(this.ddlIsNegative.SelectedValue) < 0)
            {
                this.lblHeadError.Text = "Please select to continue";
                return;
            }

            NameValueCollection headcollection = new NameValueCollection();
            headcollection["orderid"] = ViewState["orderid"].ToString();
            headcollection["orderhead"] = this.txtHeadName.Text.Trim();
            headcollection["headcost"] = this.txtHeadCost.Text.Trim();
            headcollection["isnegative"] = this.ddlIsNegative.SelectedValue;
            headcollection["staffid"] = this.UserId;

            bool success = false;

            double totamount = this.ConvertToDouble(ViewState["ordertotalamount"]);
            double headcostVal = this.ConvertToDouble(this.txtHeadCost.Text.Trim());
            if (headcostVal > totamount)
            {
                this.lblHeadError.Text = "Entered head cost should not be greater than 'Total Cart Value'";
                return;
            }

            int headid = BUOrderManagement.AddBUOrderHead(headcollection);
            success = (headid > 0);
            if (success)
            {
                this.ddlIsNegative.SelectedValue = null;
                this.txtHeadName.Text = string.Empty;
                this.txtHeadCost.Text = string.Empty;
                PopulateOrderDetails();
            }
            else
            {
                this.lblHeadError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ConvertToString(ViewState["ordertotalamount"])))
            {
                this.lblError.Text = "Please add items into the order";
                return;
            }

            NameValueCollection draftcollection = new NameValueCollection();

            if (string.IsNullOrEmpty(rdbPaymentOption.SelectedValue))
            {
                this.lblError.Text = "Payment option cannot blank";
                return;
            }

            if (!string.IsNullOrEmpty(this.hid_reference_pic.Value))
            {
                draftcollection.Add("referencefile", this.hid_reference_pic.Value);
            }
            else
            {
                draftcollection.Add("referencefile", string.Empty);
            }

            draftcollection.Add("paymentoption", this.rdbPaymentOption.SelectedItem.Value);
            draftcollection.Add("termscondition", this.txtTermCondition.Text.Trim());
            draftcollection.Add("status", "1");
            draftcollection.Add("totalcost", ViewState["ordertotalamount"].ToString());
            bool success = BUOrderManagement.UpdateBUOrder(draftcollection, ViewState["orderid"]);

            if (success)
            {
                Response.Redirect("bucurrentorder.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnFinishOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ConvertToString(ViewState["ordertotalamount"])))
            {
                this.lblError.Text = "Please add items into the order";
                return;
            }

            NameValueCollection finishcollection = new NameValueCollection();
            if (string.IsNullOrEmpty(rdbPaymentOption.SelectedValue))
            {
                this.lblError.Text = "Payment option cannot blank";
                return;
            }

            double orderamount = this.ConvertToDouble(ViewState["ordertotalamount"].ToString().Replace(",", "."));
            if (orderamount <= 0)
            {
                this.lblError.Text = "Order Amount Can not be negative";
                return;
            }

            if (!string.IsNullOrEmpty(this.hid_reference_pic.Value))
            {
                finishcollection.Add("referencefile", this.hid_reference_pic.Value);
            }
            else
            {
                finishcollection.Add("referencefile", string.Empty);
            }
            finishcollection.Add("paymentoption", this.rdbPaymentOption.SelectedItem.Value);
            finishcollection.Add("termscondition", this.txtTermCondition.Text.Trim());
            finishcollection.Add("status", "2");
            finishcollection.Add("totalcost", ViewState["ordertotalamount"].ToString());

            bool success = BUOrderManagement.UpdateBUOrder(finishcollection, ViewState["orderid"]);

            if (success)
            {
                Response.Redirect("bucurrentorder.aspx");
            }
            else
            {
                this.lblError.Text = Resources.Resource.error;
                return;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("bucurrentorder.aspx");
        }

        protected void rptDynamicCharges_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int headid = this.ConvertToInteger(e.CommandArgument);
            if (headid <= 0) return;

            BUOrderManagement.DeleteOrderHead(headid);
            this.PopulateOrderDetails();
        }

        protected void rptOrderItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int itemid = this.ConvertToInteger(e.CommandArgument);
            if (itemid <= 0) return;

            bool success = BUOrderManagement.DeleteBUOrderItem(itemid);
            if (success)
            {
                BUOrderManagement.UpdateBUOrderItemQuan(itemid);
            }

            this.PopulateOrderDetails();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetProductData(string pid)
        {
            NameValueCollection collection = BUOrderManagement.GetBUItem(pid);
            if (collection == null) return string.Empty;

            string bucurrency = string.Empty;
            NameValueCollection currencycollection = UserBA.GetBusinessUserDetail(System.Web.HttpContext.Current.Session["companyid"]);
            if (currencycollection != null) bucurrency = currencycollection["currencyname"];
            currencycollection = null;
            if (string.IsNullOrEmpty(bucurrency)) bucurrency = "EUR";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["cost"] = bucurrency + " " + Convert.ToDecimal(collection["cost"]).ToString("0.00").Replace(",", ".");//collection["cost"].Replace(",", ".");
            dictionary["taxname"] = collection["taxname"];
            dictionary["taxpercentage"] = collection["taxpercentage"];

            string availablestock = string.Empty;
            if (!string.IsNullOrEmpty(collection["availablestock"]))
            {
                availablestock = collection["availablestock"];
            }
            else
            {
                availablestock = " - ";
            }

            dictionary["availablestock"] = availablestock;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(dictionary);
        }


    }
}