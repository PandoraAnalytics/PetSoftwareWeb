using BABusiness;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Breederapp
{
    public partial class tickethistory : PageBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                PopulateControls();
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
            DataTable dataTable1 = Ticket.GetApplicationsForTicket();
            if (dataTable1 != null)
            {
                DataRow row = dataTable1.NewRow();
                row["id"] = int.MinValue;
                row["name"] = Resources.Resource.SelectApplication;
                dataTable1.Rows.InsertAt(row, 0);

                this.ddlApplication.DataSource = dataTable1;
                this.ddlApplication.DataBind();
            }


            NameValueCollection collection = Session["ticketsearch"] as NameValueCollection;
            if (collection != null)
            {
                this.txtSearch.Text = collection["keyword"];
                this.txtTicketNo.Text = collection["ticketno"];
                this.ddlApplication.SelectedValue = collection["application"];
                this.ddlBug.SelectedValue = collection["bug"];
                this.hdsort.Value = collection["hdsort"];
            }

            Session["ticketsearch"] = null;
            this.ApplyFilter();
        }
        private void ApplyFilter()
        {

            NameValueCollection collection = new NameValueCollection();
            if (this.ConvertToInteger(this.ddlApplication.SelectedValue) > 0) collection.Add("application", this.ddlApplication.SelectedValue);
            collection.Add("keyword", this.txtSearch.Text.Trim());
            collection.Add("ticketno", this.txtTicketNo.Text.Trim());
            if (this.ConvertToInteger(this.ddlBug.SelectedValue) >= 0) collection.Add("bug", this.ddlBug.SelectedValue);
            if (this.ConvertToInteger(this.ddlVOneeded.SelectedValue) >= 0) collection.Add("voneeded", this.ddlVOneeded.SelectedValue);
            Ticket objTicket = new Ticket();
            string filter = objTicket.TicketSearch(collection);
            this.hidfilter.Value = (filter.Length > 0) ? BASecurity.Encrypt(filter, PageBase.HashKey) : ""; ;
            objTicket = null;
            collection["hdsort"] = this.hdsort.Value;
            Session["ticketsearch"] = collection;

        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            this.ApplyFilter();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string filter = BASecurity.Decrypt(this.hidfilter.Value, PageBase.HashKey);

            int totalPages = Ticket.GetTicketsHistoryCount(filter);
            if (totalPages > 35)
            {
                this.lblError.Text = "Please use filter to reduce no of pages.";
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
            cell1.SetCellValue("Ticket No");
            workSheet.SetColumnWidth(0, 30 * 256);

            ICell cell11 = headerRow1.CreateCell(1, CellType.String);
            cell11.CellStyle = fontStyle;
            cell11.SetCellValue("Header");
            workSheet.SetColumnWidth(1, 30 * 256);

            ICell cell2 = headerRow1.CreateCell(2, CellType.String);
            cell2.CellStyle = fontStyle;
            cell2.SetCellValue("Application");
            workSheet.SetColumnWidth(2, 30 * 256);

            ICell cell3 = headerRow1.CreateCell(3, CellType.String);
            cell3.CellStyle = fontStyle;
            cell3.SetCellValue("EDT (hours)");
            workSheet.SetColumnWidth(3, 30 * 256);

            ICell cell4 = headerRow1.CreateCell(4, CellType.String);
            cell4.CellStyle = fontStyle;
            cell4.SetCellValue("Status");
            workSheet.SetColumnWidth(4, 30 * 256);

            ICell cell5 = headerRow1.CreateCell(5, CellType.String);
            cell5.CellStyle = fontStyle;
            cell5.SetCellValue("Is Bug");
            workSheet.SetColumnWidth(5, 30 * 256);

            ICell cell6 = headerRow1.CreateCell(6, CellType.String);
            cell6.CellStyle = fontStyle;
            cell6.SetCellValue("Priority");
            workSheet.SetColumnWidth(6, 30 * 256);

            ICell cell7 = headerRow1.CreateCell(7, CellType.String);
            cell7.CellStyle = fontStyle;
            cell7.SetCellValue("VO Needded");
            workSheet.SetColumnWidth(7, 30 * 256);

            ICell cell8 = headerRow1.CreateCell(8, CellType.String);
            cell8.CellStyle = fontStyle;
            cell8.SetCellValue("Created");
            workSheet.SetColumnWidth(8, 30 * 256);

            ICell cell9 = headerRow1.CreateCell(9, CellType.String);
            cell9.CellStyle = fontStyle;
            cell9.SetCellValue("Created By");
            workSheet.SetColumnWidth(9, 30 * 256);

            ICell cell10 = headerRow1.CreateCell(10, CellType.String);
            cell10.CellStyle = fontStyle;
            cell10.SetCellValue("Updated");
            workSheet.SetColumnWidth(10, 30 * 256);

            ICell cell111 = headerRow1.CreateCell(11, CellType.String);
            cell111.CellStyle = fontStyle;
            cell111.SetCellValue("Updated By");
            workSheet.SetColumnWidth(11, 30 * 256);

            IDataFormat dataFormatCustom = workBook.CreateDataFormat();
            int rowindex = 1;
            DataSet itemDs = null;
            int pageno = 1;
            do
            {
                itemDs = Ticket.GetTicketsHistory(pageno, filter, this.hdsort.Value);
                if (itemDs == null || itemDs.Tables.Count == 0 || itemDs.Tables[0].Rows.Count == 0) break;

                foreach (DataRow row in itemDs.Tables[0].Rows)
                {
                    IRow excelRow = workSheet.CreateRow(rowindex++);
                    ICell cell21 = excelRow.CreateCell(0, CellType.String);
                    cell21.CellStyle = cellStyle;
                    cell21.SetCellValue(this.ConvertToString(row["ticketno"]).PadLeft(5, '0'));

                    ICell cell22 = excelRow.CreateCell(1, CellType.String);
                    cell22.CellStyle = cellStyle;
                    cell22.SetCellValue(this.ConvertToString(row["header"]));

                    ICell cell23 = excelRow.CreateCell(2, CellType.String);
                    cell23.CellStyle = cellStyle;
                    cell23.SetCellValue(this.ConvertToString(row["appname"]));

                    if (!string.IsNullOrEmpty(row["processed_completiondate"].ToString()))
                    {
                        ICell cell24 = excelRow.CreateCell(3, CellType.String);
                        cell24.CellStyle = cellStyle;
                        cell24.SetCellValue(this.ConvertToString(row["etd"]) + "(CD:" + row["processed_completiondate"].ToString() + ")");
                    }
                    else
                    {
                        ICell cell24 = excelRow.CreateCell(3, CellType.String);
                        cell24.CellStyle = cellStyle;
                        cell24.SetCellValue(this.ConvertToString(row["etd"]));
                    }

                    ICell cell25 = excelRow.CreateCell(4, CellType.String);
                    cell25.CellStyle = cellStyle;
                    cell25.SetCellValue(this.ConvertToString(row["statusname"]));


                    ICell cell26 = excelRow.CreateCell(5, CellType.String);
                    cell26.CellStyle = cellStyle;
                    string isBug = this.ConvertToString(row["isbug"]);

                    switch (isBug)
                    {
                        case "1":
                            cell26.SetCellValue("Yes");
                            break;

                        case "0":
                            cell26.SetCellValue("No");
                            break;

                        default:
                            cell26.SetCellValue("-");
                            break;
                    }

                    ICell cell27 = excelRow.CreateCell(6, CellType.String);
                    cell27.CellStyle = cellStyle;
                    cell27.SetCellValue(this.ConvertToString(row["priorityname"]));


                    ICell cell28 = excelRow.CreateCell(7, CellType.String);
                    cell28.CellStyle = cellStyle;
                    string voNeeded = this.ConvertToString(row["voneeded"]);

                    switch (voNeeded)
                    {
                        case "1":
                            cell28.SetCellValue("Yes");
                            break;

                        case "0":
                            cell28.SetCellValue("No");
                            break;

                        default:
                            cell28.SetCellValue("-");
                            break;
                    }


                    ICell cell29 = excelRow.CreateCell(8, CellType.String);
                    cell29.CellStyle = cellStyle;
                    cell29.SetCellValue(this.ConvertToString(row["created"]));

                    ICell cell30 = excelRow.CreateCell(9, CellType.String);
                    cell30.CellStyle = cellStyle;
                    cell30.SetCellValue(this.ConvertToString(row["createdfn"]) + " " + this.ConvertToString(row["createdln"]));

                    ICell cell31 = excelRow.CreateCell(10, CellType.String);
                    cell31.CellStyle = cellStyle;
                    cell31.SetCellValue(this.ConvertToString(row["lastmodified"]));

                    ICell cell32 = excelRow.CreateCell(11, CellType.String);
                    cell32.CellStyle = cellStyle;
                    cell32.SetCellValue(this.ConvertToString(row["updatedfn"]) + " " + this.ConvertToString(row["updatedln"]));

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

            string filename = "ticket_history" + BusinessBase.Now.ToString("dd_MM_yyyy_HH_mm") + ".xls";
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