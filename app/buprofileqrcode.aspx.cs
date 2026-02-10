using BABusiness;
using Gma.QrCodeNet.Encoding;
using System;
using System.Collections.Specialized;

namespace Breederapp
{
    public partial class buprofileqrcode : ERPBase
    {
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!this.IsPostBack)
            {
                 ViewState["id"] = DecryptQueryString();
                this.PopulateControls();
            }
        }

        private void PopulateControls()
        {
            this.lblError.Text = string.Empty;
            NameValueCollection collection = UserBA.GetBusinessUserDetail(this.CompanyId);
            if (collection == null)
            {
                this.lblError.Text = "The company is either deleted or not available.";
                return;
            }

            this.lblCompany.Text = collection["companyname"];

            int companyId = this.ConvertToInteger(this.CompanyId);
            string qrCodeData = "companyqr_" + (companyId).ToString();            
			string stringtowrite = BreederMail.PageURL + "app/custsignup.aspx?token=" + BASecurity.Encrypt(this.CompanyId, BusinessBase.FixedSaltKey);
           // string stringtowrite = BreederMail.PageURL + "custsignup.aspx";

            this.GenerateBarcode(stringtowrite, qrCodeData);
            this.img_qrcode.Src = "docs/" + qrCodeData + ".png";
        }

        protected void GenerateBarcode(string xiDataToWrite, string xiName)
        {
            string barcodePath = this.FileUploadPath + xiName + ".png";
            if (System.IO.File.Exists(barcodePath)) return;
            int size = 210;//changed 510 
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = qrEncoder.Encode(xiDataToWrite);

            var multiplier = (double)size / qrCode.Matrix.Width;
            var image = new System.Drawing.Bitmap(size, size);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var originalX = Math.Min(qrCode.Matrix.Width - 1, (int)(x / multiplier));
                    var originalY = Math.Min(qrCode.Matrix.Height - 1, (int)(y / multiplier));

                    if (qrCode.Matrix.InternalArray[originalX, originalY])
                        image.SetPixel(x, y, System.Drawing.Color.Black);
                    else
                        image.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            image.Save(barcodePath, System.Drawing.Imaging.ImageFormat.Png);

        }
    }
}