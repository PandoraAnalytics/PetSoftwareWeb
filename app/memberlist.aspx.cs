using BABusiness;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class memberlist : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                ViewState["id"] = DecryptQueryString("aid");
                this.ApplyFilter();
                this.PopulateControls();
            }
            else
            {
                switch (this.Request["__EVENTTARGET"])
                {
                    case "invite":
                        this.SendInvite(this.Request["__EVENTARGUMENT"]);
                        break;

                    case "add":
                        this.ProcessAdmin(this.Request["__EVENTARGUMENT"], "1");
                        break;

                    case "delete":
                        this.ProcessAdmin(this.Request["__EVENTARGUMENT"], "0");
                        break;
                }

            }

            this.lnkAddMember.HRef = "addmember.aspx?aid=" + BABusiness.BASecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey);
            this.lnkImportMember.HRef = "importmembers.aspx?aid=" + BABusiness.BASecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey);
        }

        private void PopulateControls()
        {
            string aId = this.ConvertToString(ViewState["id"]);
            if (!string.IsNullOrEmpty(aId))
            {
                NameValueCollection collection = UserBA.GetAssociation(aId);
                if (collection != null) this.lblAssociationTitle.Text = "(" + collection["name"] + ")";
            }
            else Response.Redirect("landing.aspx");
        }

        private void SendInvite(string xiValue)
        {
            string[] valArray = xiValue.Split('*');
            if (!string.IsNullOrEmpty(valArray[0]) && !string.IsNullOrEmpty(valArray[1]))
            {               
                BreederMail.SendEmail(BreederMail.MessageType.SENDUSERJOINREQUEST, valArray[1]);
                Member.UpdateInviteStatus(valArray[0], ViewState["id"]);
            }
        }

        private void ProcessAdmin(string xiMemberId, string xiType)
        {
            if (!string.IsNullOrEmpty(xiMemberId) && !string.IsNullOrEmpty(xiType))
            {
                Member.UpdateISAdminStatus(xiType, xiMemberId, ViewState["id"]);
            }
        }

        private void ApplyFilter()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", this.txtName.Text.Trim());
            collection.Add("associationid", this.ConvertToString(ViewState["id"]));
            if (this.ddlMember.SelectedValue == "3") collection.Add("type", string.Empty);
            else collection.Add("type", this.ddlMember.SelectedValue);

            this.hidfilter.Value = Member.SearchMember(collection);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("manageassociation.aspx");
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();

            int totalPages = Member.GetMemberListCount(this.hidfilter.Value);
            if (totalPages > 35)
            {
                return;
            }

            HSSFWorkbook workBook = new HSSFWorkbook();
            HSSFSheet workSheet = workBook.CreateSheet("Sheet1") as HSSFSheet;
            workSheet.SetColumnWidth(0, 30 * 256);

            IFont font = workBook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle fontStyle = workBook.CreateCellStyle();
            fontStyle.SetFont(font);
            fontStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            fontStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            fontStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            fontStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            fontStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            fontStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;

            ICellStyle cellStyle = workBook.CreateCellStyle();
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
            cellStyle.WrapText = true;
            IFont font1 = workBook.CreateFont();
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
            cellStyle.SetFont(font1);

            workSheet.SetDefaultColumnStyle(0, cellStyle);

            Table excelTable = new Table();

            IRow headerRow1 = workSheet.CreateRow(0);
            ICell cell1 = headerRow1.CreateCell(0, CellType.String);
            cell1.CellStyle = fontStyle;
            cell1.SetCellValue("Member No");
            workSheet.SetColumnWidth(0, 30 * 256);

            ICell cell11 = headerRow1.CreateCell(1, CellType.String);
            cell11.CellStyle = fontStyle;
            cell11.SetCellValue("Name");
            workSheet.SetColumnWidth(1, 30 * 256);

            ICell cell2 = headerRow1.CreateCell(2, CellType.String);
            cell2.CellStyle = fontStyle;
            cell2.SetCellValue("Email Address");
            workSheet.SetColumnWidth(2, 30 * 256);

            ICell cell3 = headerRow1.CreateCell(3, CellType.String);
            cell3.CellStyle = fontStyle;
            cell3.SetCellValue("Mobile");
            workSheet.SetColumnWidth(3, 30 * 256);

            IDataFormat dataFormatCustom = workBook.CreateDataFormat();
            int rowindex = 1;
            DataSet itemDs = null;
            int pageno = 1;
            do
            {
                itemDs = Member.GetMemberList(pageno, this.hidfilter.Value);
                if (itemDs == null || itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0) break;

                foreach (DataRow row in itemDs.Tables[0].Rows)
                {
                    IRow excelRow = workSheet.CreateRow(rowindex++);
                    ICell cell21 = excelRow.CreateCell(0, CellType.String);
                    cell21.CellStyle = cellStyle;
                    cell21.SetCellValue(this.ConvertToString(row["memberno"]));

                    ICell cell22 = excelRow.CreateCell(1, CellType.String);
                    cell22.CellStyle = cellStyle;
                    cell22.SetCellValue(this.ConvertToString(row["fname"]) + " " + this.ConvertToString(row["lname"]));

                    ICell cell23 = excelRow.CreateCell(2, CellType.String);
                    cell23.CellStyle = cellStyle;
                    cell23.SetCellValue(this.ConvertToString(row["email"]));

                    ICell cell24 = excelRow.CreateCell(3, CellType.String);
                    cell24.CellStyle = cellStyle;
                    cell24.SetCellValue(this.ConvertToString(row["mobile"]));

                }
                pageno++;
            }
            while (itemDs != null && itemDs.Tables.Count > 0 && itemDs.Tables[0].Rows.Count > 0);

            ICellStyle cellStyle1 = workBook.CreateCellStyle();
            cellStyle1.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle1.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            workSheet.SetDefaultColumnStyle(0, cellStyle1);

            FileStream fileStream = null;
            bool download = false;

            string filename = "memberlist" + BusinessBase.Now.ToString("dd_MM_yyyy_HH_mm") + ".xls";
            try
            {
                string strFileName = this.FileUploadTempPath + filename;
                fileStream = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                workBook.Write(fileStream);

                download = true;
            }
            catch { }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Dispose();
                    fileStream.Close();
                }
                fileStream = null;
                if (download)
                {
                    Response.ContentType = "application/ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    Response.WriteFile(this.FileUploadTempPath + filename);
                    Response.End();
                }
            }
        }


    }
}