using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Services;

namespace Breederapp.Services
{
    [WebService(Namespace = "http://pandora-ict.de/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    [System.Web.Script.Services.ScriptService]
    public class validation : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public string[] Validate(string input, string attributes)
        {
            string[] returnString = new string[2];

            if (string.IsNullOrEmpty(attributes)) return returnString;

            string[] validationArray = attributes.Split(' ');
            if (validationArray == null || validationArray.Length < 0) return returnString;

            if (input == null || input == (Int16.MinValue).ToString() || input == (int.MinValue).ToString() || input == (decimal.MinValue).ToString() || input == Guid.Empty.ToString()) input = string.Empty;
            input = input.Trim();

            bool valid = true;
            for (int i = 0; i < validationArray.Length; i++)
            {
                string[] pieces = validationArray[i].Split('-');
                if (pieces == null || pieces.Length < 0) break;

                switch (pieces[0])
                {
                    case "required":
                        valid = this.RequiredValidation(input);
                        break;

                    case "email":
                        valid = this.EmailValidation(input);
                        break;

                    case "phone":
                        valid = this.PhoneValidation(input);
                        break;

                    case "minlength":
                        if (pieces.Length < 2) break;
                        valid = this.MinLengthValidation(input, Convert.ToInt32(pieces[1]));
                        break;

                    case "maxlength":
                        if (pieces.Length < 2) break;
                        valid = this.MaxLengthValidation(input, Convert.ToInt32(pieces[1]));
                        break;

                    case "minval":
                        if (pieces.Length < 2) break;
                        valid = this.MinValueValidation(input, Convert.ToInt32(pieces[1]));
                        break;

                    case "maxval":
                        if (pieces.Length < 2) break;
                        valid = this.MaxValueValidation(input, Convert.ToInt32(pieces[1]));
                        break;

                    case "pnumber":
                        valid = this.PositiveNumberValidation(input);
                        break;

                    case "number":
                        valid = this.NumberValidation(input);
                        break;

                    case "pdecimal":
                        valid = this.PositiveDecimalValidation(input);
                        break;

                    case "decimal":
                        valid = this.DecimalValidation(input);
                        break;

                    case "date":
                        valid = this.DateValidation(input);
                        break;

                    case "time":
                        valid = this.TimeValidation(input);
                        break;
                }

                if (valid == false)
                {
                    returnString = new string[2];
                    returnString[0] = pieces[0];
                    returnString[1] = (pieces.Length > 1) ? pieces[1] : "0";
                    break;
                }
            }

            return returnString;
        }

        private bool RequiredValidation(string xiInputString)
        {
            return (xiInputString.Length > 0);
        }

        private bool EmailValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            /*string emailPattern = @"^([0-9a-zA-Z]" + @"([\+\-_\.][0-9a-zA-Z]+)*" + @")+" + @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
            Regex regex = new Regex(emailPattern);
            Match match = regex.Match(xiInputString);

            return match.Success;*/

            if (xiInputString.Trim().EndsWith(".")) return false; // suggested by @TK-421

            try
            {
                var addr = new System.Net.Mail.MailAddress(xiInputString);
                return addr.Address == xiInputString;
            }
            catch
            {
                return false;
            }
        }

        private bool PhoneValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            xiInputString = Regex.Replace(xiInputString, @"[-()\s]+", "");

            Regex regex = new Regex(@"^\+?\d*$");
            Match match = regex.Match(xiInputString);

            return match.Success;
        }

        private bool MinLengthValidation(string xiInputString, int xiMinLength)
        {
            if (xiInputString.Length == 0) return true;

            return (xiInputString.Length >= xiMinLength);
        }

        private bool MaxLengthValidation(string xiInputString, int xiMaxLength)
        {
            if (xiInputString.Length == 0) return true;

            return (xiInputString.Length <= xiMaxLength);
        }

        private bool MinValueValidation(string xiInputString, int xiMinLength)
        {
            if (xiInputString.Length == 0) return true;

            try
            {
                int d = int.MinValue;
                bool parsed = (int.TryParse(xiInputString, out d));
                if (d != int.MinValue && parsed)
                {
                    return (d >= xiMinLength);
                }
            }
            catch { }

            return true;
        }

        private bool MaxValueValidation(string xiInputString, int xiMaxLength)
        {
            if (xiInputString.Length == 0) return true;

            try
            {
                int d = int.MinValue;
                bool parsed = (int.TryParse(xiInputString, out d));
                if (d != int.MinValue && parsed)
                {
                    return (d <= xiMaxLength);
                }
            }
            catch { }

            return true;
        }

        private bool PositiveNumberValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            Regex regex = new Regex(@"^[0-9]\d*$");
            Match match = regex.Match(xiInputString);

            return match.Success;
        }

        private bool NumberValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            xiInputString = xiInputString.Replace(',', '.');

            int d = int.MinValue;
            bool parsed = (int.TryParse(xiInputString, out d));

            return (d != int.MinValue && parsed);
        }

        private bool PositiveDecimalValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            // xiInputString = xiInputString.Replace(',', '.');

            Regex regex1 = new Regex(@"^[0-9]*\.[0-9]{1,4}$");
            Match match1 = regex1.Match(xiInputString);

            Regex regex2 = new Regex(@"^[0-9]\d*$");
            Match match2 = regex2.Match(xiInputString);

            return (match1.Success || match2.Success);
        }

        private bool DecimalValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            xiInputString = xiInputString.Replace(',', '.');

            decimal d = decimal.MinValue;
            bool parsed = (decimal.TryParse(xiInputString, out d));

            return (d != decimal.MinValue && parsed);
        }

        private bool DateValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            Thread.CurrentThread.CurrentCulture = BABusiness.BusinessBase.GetCulture();
            DateTime tempDate = Convert.ToDateTime(xiInputString, CultureInfo.CurrentCulture);
            if (tempDate == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        private bool TimeValidation(string xiInputString)
        {
            if (xiInputString.Length == 0) return true;

            Regex regex = new Regex(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$");
            Match match = regex.Match(xiInputString);

            return match.Success;
        }


    }
}
