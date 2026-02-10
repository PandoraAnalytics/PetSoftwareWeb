using BABusiness;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Breederapp.Services
{
    [ScriptService]
    [WebService(Namespace = "http://pandora-ict.de/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class getdata : WebService
    {
        public getdata()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = BusinessBase.GetCulture();
        }

        private int ConvertToInt(object xiId)
        {
            int num = int.MinValue;
            try
            {
                num = Convert.ToInt32(xiId);
            }
            catch
            {
            }
            return num;
        }

        #region User

        [WebMethod(EnableSession = true)]
        public int GetUserCount(string filter)
        {
            return UserBA.GetUserCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetUserDetails(int page, string filter)
        {
            DataSet dataSet = UserBA.GetUserDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteUser(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return UserBA.DeleteUser(idInt);
        }

        #endregion

        #region User_Animal

        [WebMethod(EnableSession = true)]
        public int GetMyAnimalsCount(string filter)
        {
            return UserBA.GetUser_AnimalCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetMyAnimals(int page, string filter)
        {
            DataSet dataSet = UserBA.GetUser_AnimalDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetCustomerAnimalsCount(string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary == null) return 0;

            return UserBA.GetUser_AnimalCount(dictionary["filter"], dictionary["customerid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomerAnimals(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary == null) return string.Empty;


            DataSet dataSet = UserBA.GetUser_AnimalDetails(page, dictionary["filter"], dictionary["customerid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteAssetContract(string id)
        {
            return UserBA.DeleteUser_Animal(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region Animal

        [WebMethod(EnableSession = true)]
        public int GetAnimalCount(string filter)
        {
            return AnimalBA.GetAnimalCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalChilds(int page, string filter)
        {
            DataTable dataTable = AnimalBA.GetAnimalChilds(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.DataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalParents(int page, string filter)
        {
            DataTable dataTable = AnimalBA.GetAnimalParents(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.DataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool ActivateAnimal(string id)
        {
            AnimalBA obj = new AnimalBA();
            bool success = obj.ActivateAnimal(BASecurity.Decrypt(id, PageBase.HashKey));
            obj = null;
            return success;
        }

        #endregion

        #region Animal_Certificates

        [WebMethod(EnableSession = true)]
        public int GetAnimalCertificateCount(string filter)
        {
            return Certificate.GetCertificateCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalCertificateDetails(int page, string filter)
        {
            DataSet dataSet = Certificate.GetCertificateDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteCertificates(string id)
        {
            return Certificate.DeleteCertificates(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region Animal_Food

        [WebMethod(EnableSession = true)]
        public int GetAnimalFoodCount(string filter)
        {
            return AnimalBA.GetAnimalFoodCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalFoodDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalFoodDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteAnimalFood(string id)
        {
            return AnimalBA.DeleteAnimalFood(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region Animal_Transfer

        [WebMethod(EnableSession = true)]
        public int GetAnimalTransferCount(string filter)
        {
            return AnimalBA.GetAnimalTransferCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalTransferDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalTransferDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #endregion

        #region Animal_Notes

        [WebMethod(EnableSession = true)]
        public int GetAnimalNotesCount(string filter)
        {
            return AnimalBA.GetAnimalNotesCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalNotesDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalNotesDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteAnimalNotes(string id)
        {
            return AnimalBA.DeleteAnimalNotes(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region Animal_Appointment

        [WebMethod(EnableSession = true)]
        public int GetAnimalAppointmentCount(string filter)
        {
            return AnimalBA.GetAnimalAppointmentCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalAppointmentDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalAppointmentDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "dateid");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetCustomerAnimalAppointmentCount(string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary == null) return 0;

            return AnimalBA.GetAnimalAppointmentCount(dictionary["filter"], dictionary["customerid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomerAnimalAppointmentDetails(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary == null) return string.Empty;


            DataSet dataSet = AnimalBA.GetAnimalAppointmentDetails(page, dictionary["filter"], dictionary["customerid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "dateid");

                dataSet.Tables[0].Columns.Add("secureduserid", typeof(string));
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    row["secureduserid"] = BASecurity.Encrypt(row["userid"].ToString(), PageBase.HashKey);
                }
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteAnimalAppointment(string id)
        {
            return AnimalBA.DeleteAnimalAppointment(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalAppointmentDetailsByMonth(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary != null)
            {
                DateTime date = new DateTime(this.ConvertToInt(dictionary["year"]), this.ConvertToInt(dictionary["month"]), 1);

                NameValueCollection collection = new NameValueCollection();

                collection.Add("startdate", date.ToString("dd.MM.yyyy"));
                collection.Add("enddate", date.AddMonths(1).AddDays(-1).ToString("dd.MM.yyyy"));

                filter = AnimalBA.AppointmentSearch(collection);

                collection = null;
                dictionary = null;

                DataSet dataSet = AnimalBA.GetAnimalAppointmentDetails(page, filter, Session["userid"]);
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "dateid");
                    return dataSet.GetXml();
                }

            }
            return string.Empty;
        }

        #endregion

        #region Event

        [WebMethod(EnableSession = true)]
        public int GetEventCount(string filter)
        {
            return EventBA.GetEventCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetEventDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetEventDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteEvent(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return EventBA.DeleteEvent(idInt);
        }

        [WebMethod(EnableSession = true)]
        public string GetEventDetailsByMonth(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary != null)
            {
                DateTime date = new DateTime(this.ConvertToInt(dictionary["year"]), this.ConvertToInt(dictionary["month"]), 1);

                NameValueCollection collection = new NameValueCollection();

                collection.Add("startdate", date.ToString("dd.MM.yyyy"));
                collection.Add("enddate", date.AddMonths(1).AddDays(-1).ToString("dd.MM.yyyy"));
                collection.Add("userid", Session["userid"].ToString());

                filter = EventBA.EventSearch(collection);

                collection = null;
                dictionary = null;

                DataSet dataSet = EventBA.GetEventDetails(page, filter, Session["userid"]);
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                    return dataSet.GetXml();
                }

            }
            return string.Empty;
        }

        #endregion

        #region Event_Registrations

        [WebMethod(EnableSession = true)]
        public int GetEventRegistrationsCount(string filter)
        {
            return EventBA.GetRegisteredUserCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetEventRegistrationsDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetRegisteredUsers(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetEventRegistrationOtherDetails(int page, string filter)
        {
            DataTable dataTable = EventBA.GetRegisteredOtherFields(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }

        // Event Brochure

        [WebMethod(EnableSession = true)]
        public int GetEventBrochureCount(string filter)
        {
            return EventBA.GetEventBrochureCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetEventBrochureDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetEventBrochureDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteEventBrochure(string id)
        {
            return EventBA.DeleteEventBrochure(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public bool PublishEventBrochure(string id, string newstatus)
        {
            return EventBA.PublishBrochure(BASecurity.Decrypt(id, PageBase.HashKey), newstatus);
        }

        [WebMethod(EnableSession = true)]
        public int GetBrochurePageCount(string filter)
        {
            return EventBA.GetBrochurePageCount(BASecurity.Decrypt(filter, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public string GetBrochurePageDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetBrochurePageListDetails(page, BASecurity.Decrypt(filter, PageBase.HashKey));
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteBrochurePage(string id)
        {
            return EventBA.DeleteBrochurePage(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        //Event Sponsor 18 dec 2023 nilesh
        [WebMethod(EnableSession = true)]
        public int GetEventSponsorCount(string filter)
        {
            return EventBA.GetEventSponsorCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetEventSponsorDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetEventSponsorDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteEventSponsor(string id)
        {
            return EventBA.DeleteEventSponsor(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region Address

        [WebMethod(EnableSession = true)]
        public int GetAddressCount(string filter)
        {
            return AddressBA.GetAddressCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAddressDetails(int page, string filter)
        {
            DataSet dataSet = AddressBA.GetAddressDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteAddress(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return AddressBA.DeleteAddress(idInt);
        }

        #endregion

        #region Contact

        [WebMethod(EnableSession = true)]
        public int GetContactCount(string filter)
        {
            return AddressBA.GetContactCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetContactDetails(int page, string filter)
        {
            DataSet dataSet = AddressBA.GetContactDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteContact(string id)
        {
            return AddressBA.DeleteContact(BASecurity.Decrypt(id, PageBase.HashKey));
            //int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            //return AddressBA.DeleteContact(idInt);
        }

        #endregion

        #region BreedCType

        [WebMethod(EnableSession = true)]
        public string GetBreedTypes(int page, string filter)
        {
            DataSet allMeasures = AnimalBA.GetBreedTypes(filter);
            if (allMeasures != null && allMeasures.Tables.Count > 0 && allMeasures.Tables[0].Rows.Count > 0)
                return allMeasures.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBreedType(string id)
        {
            NameValueCollection measure = AnimalBA.GetBreedType(id);
            if (measure == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = measure["name"];
            dictionary["categoryid"] = measure["categoryid"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        //[WebMethod(EnableSession = true)]
        //public string DeleteBreedType(int id)
        //{
        //    return AnimalBA.DeleteBreedType(id);
        //}

        #endregion

        #region BreedCategory

        [WebMethod(EnableSession = true)]

        public string GetBreedCategories(int page, string filter)
        {
            DataSet allMeasures = AnimalBA.GetBreedCategories(filter);
            if (allMeasures != null && allMeasures.Tables.Count > 0 && allMeasures.Tables[0].Rows.Count > 0)
                return allMeasures.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBreedCategory(string id)
        {
            NameValueCollection measure = AnimalBA.GetBreedCategory(id);
            if (measure == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["breedname"] = measure["breedname"];
            dictionary["breedimage"] = measure["breedimage"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteBreedTypeCategory(string id)
        {

            return AnimalBA.DeleteBreedCategory(id);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteAnimalTypeCategory(string id)
        {

            return AnimalBA.DeleteAnimalCategory(id);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteBreedCategory(int id)
        {
            return AnimalBA.DeleteBreedCategory(id);
        }

        #endregion

        #region Profession

        [WebMethod(EnableSession = true)]

        public string GetProfessions(int page, string filter)
        {
            DataSet allMeasures = AnimalBA.GetProfessions(filter);
            //DataSet allMeasures = AnimalBA.GetBUProfessions(filter);
            if (allMeasures != null && allMeasures.Tables.Count > 0 && allMeasures.Tables[0].Rows.Count > 0)
                return allMeasures.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetProfession(string id)
        {
            NameValueCollection measure = AnimalBA.GetProfession(id);
            if (measure == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = measure["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProfession(int id)
        {
            return AnimalBA.DeleteProfession(id);
        }

        #endregion

        #region CustomFields

        [WebMethod(EnableSession = true)]
        public int GetAllCustomFieldsCount(string filter)
        {
            string active = "1";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary != null)
            {
                NameValueCollection collection = new NameValueCollection();
                collection["title"] = (dictionary != null && dictionary.Keys.Contains("title")) ? dictionary["title"] : string.Empty;
                collection["ischecklistype"] = (dictionary != null && dictionary.Keys.Contains("ischecklistype")) ? dictionary["ischecklistype"] : string.Empty;
                collection["iseventtype"] = (dictionary != null && dictionary.Keys.Contains("iseventtype")) ? dictionary["iseventtype"] : string.Empty;
                string brtype = (Session["associationbreedertype"] != null) ? Session["associationbreedertype"].ToString() : string.Empty;
                collection["association_breedtype"] = (dictionary != null && dictionary.Keys.Contains("associationbreedertype")) ? dictionary["associationbreedertype"] : brtype;
                collection["companyid"] = (dictionary != null && dictionary.Keys.Contains("companyid")) ? dictionary["companyid"] : string.Empty;
                filter = CustomFields.Search(collection);
                collection = null;
                dictionary = null;
            }

            //DataSet dataSet = CustomFields.GetAllCustomFieldsCount(filter, active);
            //if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            //{
            //    BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
            //    return dataSet.GetXml();
            //}
            //return string.Empty;
            return CustomFields.GetAllCustomFieldsCount(filter, active);
        }


        [WebMethod(EnableSession = true)]
        public string GetAllCustomFields(int page, string filter)
        {
            string active = "1";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary != null)
            {
                NameValueCollection collection = new NameValueCollection();
                collection["title"] = (dictionary != null && dictionary.Keys.Contains("title")) ? dictionary["title"] : string.Empty;
                collection["ischecklistype"] = (dictionary != null && dictionary.Keys.Contains("ischecklistype")) ? dictionary["ischecklistype"] : string.Empty;
                collection["iseventtype"] = (dictionary != null && dictionary.Keys.Contains("iseventtype")) ? dictionary["iseventtype"] : string.Empty;
                string brtype = (Session["associationbreedertype"] != null) ? Session["associationbreedertype"].ToString() : string.Empty;
                collection["association_breedtype"] = (dictionary != null && dictionary.Keys.Contains("associationbreedertype")) ? dictionary["associationbreedertype"] : brtype;

                collection["companyid"] = (dictionary != null && dictionary.Keys.Contains("companyid")) ? dictionary["companyid"] : string.Empty;

                filter = CustomFields.Search(collection);
                collection = null;
                dictionary = null;
            }

            DataSet dataSet = CustomFields.GetAllCustomFields(page, filter, active);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetValuesByCustomFields(int page, string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);

            DataSet dataSet = CustomFields.GetValueByCustomFields(filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteCustomFields(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return CustomFields.DeleteCustomFields(idInt);
        }

        [WebMethod(EnableSession = true)]
        public bool ShowHideCustomFields(string id, int show)
        {
            bool isvisible = (show == 1) ? true : false;
            return CustomFields.ShowHideCustomField(this.ConvertToInt((object)BASecurity.Decrypt(id, PageBase.HashKey)), isvisible);
        }

        #endregion

        [WebMethod(EnableSession = true)]
        public string GetAllCertificateType(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
            if (dictionary != null)
            {
                NameValueCollection collection = new NameValueCollection();
                collection["mandatory"] = (dictionary != null && dictionary.Keys.Contains("mandatory")) ? dictionary["mandatory"] : string.Empty;
                collection["searchtext"] = (dictionary != null && dictionary.Keys.Contains("type")) ? dictionary["type"] : string.Empty;
                collection["association_breedtype"] = (Session["associationbreedertype"] != null) ? Session["associationbreedertype"].ToString() : string.Empty;
                collection["companyid"] = (dictionary != null && dictionary.Keys.Contains("companyid")) ? dictionary["companyid"] : string.Empty;
                filter = Certificate.SearchType(collection);
                collection = null;
                dictionary = null;
            }

            DataSet dataSet = Certificate.GetAllCertificatetype(filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string Deletecertificatetype(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return Certificate.DeleteCertificatetype(idInt);
        }

        [WebMethod(EnableSession = true)]
        public int GetAllCertificatesToApproveCount(string filter)
        {
            return Certificate.GetAllCertificatesToApproveCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllCertificatesToApprove(int page, string filter)
        {
            DataSet dataSet = Certificate.GetAllCertificatesToApprove(page, filter);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #region Association

        [WebMethod(EnableSession = true)]
        public int GetAllAssociationCount(string filter)
        {
            return UserBA.GetAllAssociationCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllAssociation(int page, string filter)
        {
            DataSet dataSet = UserBA.GetAllAssociation(page, filter, Session["userid"], Session["email"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }


        [WebMethod(EnableSession = true)]
        public int GetAllMyAssociationCount(string filter)
        {
            return UserBA.GetAllMyAssociationCount(filter, Session["userid"], Session["email"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllMyAssociation(int page, string filter)
        {
            DataSet dataSet = UserBA.GetAllMyAssociation(page, filter, Session["userid"], Session["email"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string Deleteassociation(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return UserBA.DeleteAssociation(idInt);
        }

        [WebMethod(EnableSession = true)]
        public string GetAssociateDetails(int page, string filter)
        {
            DataSet dataSet = EventBA.GetEventDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool SendRequest(string id)
        {
            return UserBA.AddRequest(BASecurity.Decrypt(id, PageBase.HashKey), Convert.ToInt32(Session["userid"]));
        }

        [WebMethod(EnableSession = true)]
        public bool AcceptRequest(string id)
        {
            return UserBA.AcceptRequest(id, Convert.ToInt32(Session["userid"]));
        }

        [WebMethod(EnableSession = true)]
        public bool DeclineRequest(string id)
        {
            return UserBA.DeclineRequest(id);
        }

        [WebMethod(EnableSession = true)]
        public int GetAllRequestCount(string filter)
        {
            return UserBA.GetAllRequestCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllRequest(int page, string filter)
        {
            DataSet dataSet = UserBA.GetAllRequest(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetAllAssociationToApproveCount(string filter)
        {
            return UserBA.GetAllAssociationToApproveCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllAssociationToApprove(int page, string filter)
        {
            DataSet dataSet = UserBA.GetAllAssociationToApprove(page, filter, Session["userid"], Session["email"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }



        #endregion

        #region Ticket

        #region Ticket Priority

        [WebMethod(EnableSession = true)]
        public int GetTicketPriorityCount(string filter)
        {
            return Ticket.GetTicketPriorityCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketPriorityDetails(int page, string filter)
        {
            DataSet dataSet = Ticket.GetTicketPriorityDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteTicketPriority(string id)
        {
            return Ticket.DeleteTicketPriority(BASecurity.Decrypt(id, PageBase.HashKey));
        }
        #endregion

        //new 21 jun 2024 nilesh
        #region Tickets Application

        [WebMethod(EnableSession = true)]
        public int GetTicketApplicationCount(string filter)
        {
            return Ticket.GetTicketApplicationCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketApplicationDetails(int page, string filter)
        {
            DataSet dataSet = Ticket.GetTicketApplicationDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteTicketApplication(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return Ticket.DeleteTicketApplication(idInt);
        }

        #endregion
        //new 21 june 2024 nilesh
        #region Ticket Users
        [WebMethod(EnableSession = true)]
        public string GetTicketUsers(int page, string filter)
        {
            DataSet allUsers = Ticket.GetAllUsers(page, filter);
            if (allUsers != null && allUsers.Tables.Count > 0 && allUsers.Tables[0].Rows.Count > 0)
                return allUsers.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetTicketUserCount(string filter)
        {
            return Ticket.GetAllUsersCount(filter);
        }
        #endregion
        //new 21 june 2024 nilesh
        #region New Ticket

        [WebMethod(EnableSession = true)]
        public int GetTicketsCount(string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            return Ticket.GetTicketsCount(filter);
        }

        //final working with sort
        [WebMethod(EnableSession = true)]
        public string GetTickets(int page, string filter, string order)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            DataSet tickets = Ticket.GetTickets(page, filter, order);
            if (tickets != null && tickets.Tables.Count > 0 && tickets.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref tickets, 0, "id");
                return tickets.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetTicketsHistoryCount(string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            return Ticket.GetTicketsHistoryCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketsHistory(int page, string filter, string order)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            DataSet ticketsHistory = Ticket.GetTicketsHistory(page, filter, order);
            if (ticketsHistory != null && ticketsHistory.Tables.Count > 0 && ticketsHistory.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref ticketsHistory, 0, "id");
                return ticketsHistory.GetXml();
            }

            return string.Empty;
        }

        // new for ticket dashboard
        [WebMethod(EnableSession = true)]
        public string TicketDash_GetTicketByStatus(int page, string filter)
        {
            DataSet dataSet = Ticket.GetTicketByStatus();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string TicketDash_GetTicketByPriority(int page, string filter)
        {
            DataSet dataSet = Ticket.GetTicketByPriority();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string TicketDash_GetTicketByCategory(int page, string filter)
        {
            if (string.IsNullOrEmpty(filter)) return string.Empty;

            string[] dates = filter.Split(',');
            if (dates == null || dates.Length < 2) return string.Empty;

            DataSet dataSet = Ticket.GetTicketByCategory(dates[0], dates[1]);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketOverview(int page, string filter)
        {
            if (string.IsNullOrEmpty(filter)) return string.Empty;

            string[] dates = filter.Split(',');
            if (dates == null || dates.Length < 2) return string.Empty;

            DataSet dashboardOverview = Ticket.GetTicketOverview(dates[0], dates[1]);
            if (dashboardOverview != null && dashboardOverview.Tables.Count > 0 && dashboardOverview.Tables[0].Rows.Count > 0)
                return dashboardOverview.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetTicketCommentsCount(string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            return Ticket.GetTicketCommentsCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketComments(int page, string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            DataSet comments = Ticket.GetTicketComments(page, filter);
            if (comments != null && comments.Tables.Count > 0 && comments.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref comments, 0, "id");
                return comments.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketComment(int page, string filter)
        {
            DataSet comment = Ticket.GetComment(this.ConvertToInt((object)BASecurity.Decrypt(filter, PageBase.HashKey)));
            if (comment != null && comment.Tables.Count > 0 && comment.Tables[0].Rows.Count > 0)
                return comment.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketDocuments(int page, string filter)
        {
            filter = BASecurity.Decrypt(filter, PageBase.HashKey);
            DataSet documents = Ticket.GetTicketDocuments(page, filter);
            BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref documents, 0, "id");
            if (documents != null && documents.Tables.Count > 0 && documents.Tables[0].Rows.Count > 0)
                return documents.GetXml();
            return string.Empty;
        }
        [WebMethod(EnableSession = true)]
        public string GetTicketDocument(int page, string filter)
        {
            DataSet document = Ticket.GetDocument(this.ConvertToInt((object)BASecurity.Decrypt(filter, PageBase.HashKey)));
            if (document != null && document.Tables.Count > 0 && document.Tables[0].Rows.Count > 0)
                return document.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteTicketComment(string id)
        {
            return Ticket.DeleteTicketComment(this.ConvertToInt((object)BASecurity.Decrypt(id, PageBase.HashKey)), Convert.ToInt32(this.Session["userid"]));
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteTicketDocument(string id)
        {
            return Ticket.DeleteTicketDocument(this.ConvertToInt((object)BASecurity.Decrypt(id, PageBase.HashKey)), Convert.ToInt32(this.Session["userid"]));
        }

        #endregion

        #region Ticket Log
        [WebMethod(EnableSession = true)]
        public int GetTicketLogCount(string filter)
        {
            return Ticket.GetTicketLogCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTicketLogs(int page, string filter)
        {
            DataSet dataSet = Ticket.GetTicketLogs(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }
        #endregion

        //new 01 august 2024 nilesh

        #region Ticket VO           


        [WebMethod(EnableSession = true)]
        public string GetVOEmails(int page, string filter)
        {
            DataSet allMeasures = Ticket.GetVOEmails(filter);
            if (allMeasures != null && allMeasures.Tables.Count > 0 && allMeasures.Tables[0].Rows.Count > 0)
                return allMeasures.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetVOEmail(string id)
        {
            NameValueCollection measure = Ticket.GetVOEmail(id);
            if (measure == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["emailaddress"] = measure["emailaddress"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteVOEmail(int id)
        {
            return Ticket.DeleteVOEmail(id);
        }


        [WebMethod(EnableSession = true)]
        public string GetVOApprovalMatrix(int page, string filter)
        {
            NameValueCollection collection = Ticket.GetVOApprovalMatrix();
            if (collection == null) return string.Empty;

            DataTable datatable = new DataTable();
            foreach (string key in collection.Keys)
            {
                datatable.Columns.Add(key);
            }

            DataRow row = datatable.NewRow();
            foreach (string key in collection.Keys)
            {
                row[key] = collection[key];
            }
            datatable.Rows.Add(row);

            DataSet ds = new DataSet();
            ds.Tables.Add(datatable);
            return ds.GetXml();
        }

        #endregion

        #endregion

        #region Report

        [WebMethod(EnableSession = true)]
        public int GetAllAnimalReportCount(string filter)
        {
            return AnimalBA.GetAllAnimalReportCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllAnimalReport(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAllAnimalReport(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #endregion

        #region Animal Log
        [WebMethod(EnableSession = true)]
        public int GetAnimalLogCount(string filter)
        {
            return AnimalBA.GetAnimalLogCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalLogDetails(int page, string filter)
        {
            DataSet dataSet = AnimalBA.GetAnimalLogDetails(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet);
                return dataSet.GetXml();
            }
            return string.Empty;
        }
        #endregion

        #region Checklist

        [WebMethod(EnableSession = true)]
        public int GetChecklistCount(string filter)
        {
            return Checklist.GetChecklistCount(filter, Session["userid"]);
        }

        [WebMethod(EnableSession = true)]
        public string GetAllChecklists(int page, string filter)
        {
            DataSet dataSet = Checklist.GetAllChecklists(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteChecklist(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return Checklist.DeleteChecklist(idInt);
        }

        [WebMethod(EnableSession = true)]
        public string GetChecklistOtherDetails(int page, string filter)
        {
            DataTable dataTable = Checklist.GetAssignedOtherFields(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool RemoveChecklistUser(string id)
        {
            return Checklist.RemoveChecklistUser(id);
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalChecklist(int page, string filter)
        {
            DataTable dataTable = AnimalBA.GetAnimalChecklist(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());

                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref ds);

                ds.Tables[0].Columns.Add("securedchecklistid", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["securedchecklistid"] = BASecurity.Encrypt(row["checklistid"].ToString(), PageBase.HashKey);
                }

                return ds.GetXml();


            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool UnassignedAnimalChecklist(string id)
        {
            int idInt = ConvertToInt(BASecurity.Decrypt(id, PageBase.HashKey));
            return AnimalBA.UnassignedChecklist(idInt);
        }

        [WebMethod(EnableSession = true)]
        public string GetChecklistSchedules(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

            DataSet dataSet = Checklist.GetChecklistSchedules(dictionary["checklistid"], dictionary["animalid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetChecklistScheduleResponses(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

            DataSet dataSet = Checklist.GetChecklistScheduleResponses(dictionary["scheduledateid"], dictionary["animalid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "responseid");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetCheckListResponseAnswers(int page, string filter)
        {
            DataTable dataTable = Checklist.GetCheckListResponseAnswers(filter);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetCheckListResponseReadOnlyAnswers(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

            DataTable dataTable = Checklist.GetCheckListResponseAnswers(dictionary["scheduledateid"], dictionary["userid"], dictionary["animalid"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetAnimalDetailsReadonly(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

            DataTable dataTable = AnimalBA.GetCustomFields(dictionary["breedtype"], dictionary["animalid"]);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBUAnimalDetailsReadonly(int page, string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> dictionary = serializer.Deserialize(filter, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

            string companyid = (dictionary != null && dictionary.Keys.Contains("companyid")) ? dictionary["companyid"] : string.Empty;


            DataTable dataTable = AnimalBA.GetBUCustomFields(dictionary["breedtype"], dictionary["animalid"], companyid);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable.Copy());
                return ds.GetXml();
            }
            return string.Empty;
        }


        [WebMethod(EnableSession = true)]
        public string GetAllChecklistsForUsers(int page, string filter)
        {
            DataSet dataSet = Checklist.GetAllChecklistsForUsers(null, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "responseid");

                dataSet.Tables[0].Columns.Add("securedscheduleid", typeof(string));
                dataSet.Tables[0].Columns.Add("securedanimalid", typeof(string));
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    row["securedscheduleid"] = BASecurity.Encrypt(row["scheduleid"].ToString(), PageBase.HashKey);
                    row["securedanimalid"] = BASecurity.Encrypt(row["animalid"].ToString(), PageBase.HashKey);
                }

                return dataSet.GetXml();
            }
            return string.Empty;
        }

        //new 9 july 2024 nilesh
        [WebMethod(EnableSession = true)]
        public string GetChecklistQuestionOptions(int page, string filter)
        {
            if (string.IsNullOrEmpty(filter)) return null;

            DataTable colTable = AnimalBA.GetCustomFieldsOptions(filter);
            if (colTable == null || colTable.Rows.Count == 0) return string.Empty;

            DataTable newTable = colTable.Copy();
            colTable = null;
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(newTable);
            return dataSet.GetXml();
        }
        //end

        #endregion

        #region Checklist Category

        [WebMethod(EnableSession = true)]

        public string GetChecklist_Category(int page, string filter)
        {
            DataSet allMeasures = Checklist.GetChecklistCategories(filter);
            if (allMeasures != null && allMeasures.Tables.Count > 0 && allMeasures.Tables[0].Rows.Count > 0)
                return allMeasures.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetChecklistCategory(string id)
        {
            NameValueCollection measure = Checklist.GetChecklistCategory(id);
            if (measure == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = measure["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteChecklistCategory(int id)
        {
            return Checklist.DeleteChecklistCategory(id);
        }

        #endregion

        #region Member Details

        [WebMethod(EnableSession = true)]
        public int GetMemberListCount(string filter)
        {
            return Member.GetMemberListCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetMemberList(int page, string filter)
        {
            DataSet dataSet = Member.GetMemberList(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetAssociationJoinMembershipListCount(string filter)
        {
            //return AnimalBA.GetAnimalTransferCount(filter, Session["userid"]);

            return Member.GetAssociationJoinMembershipListCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetAssociationJoinMembershipList(int page, string filter)
        {
            DataSet dataSet = Member.GetAssociationJoinMembershipList(page, filter, Session["userid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #endregion

        #region Animal ClassType

        [WebMethod(EnableSession = true)]
        public string GetTypeClassDetails(int page, string filter)
        {
            DataSet dsTypeClass = AnimalBA.GetTypeClassDetails(filter);
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetTypeClass(string id)
        {
            NameValueCollection collection = AnimalBA.GetTypeClass(id);
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            dictionary["categoryid"] = collection["categoryid"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteTypeClass(string id)
        {
            return AnimalBA.DeleteTypeClass(id);
        }

        #endregion

        #region Business User

        [WebMethod(EnableSession = true)]
        public int GetBusinessUserRequestApproveCount(string filter)
        {
            return UserBA.GetBusinessUserRequestApproveCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetBusinessUserRequestApprove(int page, string filter, string order)
        {
            DataSet dataSet = UserBA.GetBusinessUserRequestApprove(page, filter, order);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteBusinessEnquiry(string id)
        {
            return UserBA.DeleteBusinessEnquiry(BASecurity.Decrypt(id, PageBase.HashKey));
        }
       
        [WebMethod(EnableSession = true)]
        public int GetCompanyForCustomerCount(string filter)
        {
            return UserBA.GetCompanyForCustomerCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetCompanyForCustomer(int page, string filter)
        {
            DataSet dataSet = UserBA.GetCompanyForCustomer(page, filter);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #region Services

        [WebMethod(EnableSession = true)]
        public int GetServiceCount(string filter)
        {
            return BuServices.GetServicesCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetServicesDetails(int page, string filter)
        {
            DataSet dataSet = BuServices.GetServicesDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteService(string id)
        {
            return BuServices.DeleteServices(BASecurity.Decrypt(id, PageBase.HashKey), Session["userid"].ToString());

        }

        #endregion

        #region Product

        [WebMethod(EnableSession = true)]
        public int GetProductCount(string filter)
        {
            return BUProduct.GetProductCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetProductDetails(int page, string filter)
        {
            DataSet dataSet = BUProduct.GetProductDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetProductOutOfStockCount(string filter)
        {
            return BUProduct.GetProductOutOfStockCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetProductOutOfStockDetails(int page, string filter)
        {
            DataSet dataSet = BUProduct.GetProductOutOfStockDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string Deleteproduct(string id)
        {
            return BUProduct.DeleteProduct(BASecurity.Decrypt(id, PageBase.HashKey), Session["userid"].ToString());

        }

        [WebMethod(EnableSession = true)]
        public int GetComboCount(string filter)
        {
            return BUProduct.GetComboCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetComboDetails(int page, string filter)
        {
            DataSet dataSet = BUProduct.GetComboDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteCombo(string id)
        {
            return BUProduct.DeleteCombo(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public int GetProductForComboCount(string filter)
        {
            return BUProduct.GetProductForComboCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetProductForComboDetails(int page, string filter)
        {
            DataSet dataSet = BUProduct.GetProductForComboDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetServiceForComboCount(string filter)
        {
            return BuServices.GetServiceForComboCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetServicesForComboDetails(int page, string filter)
        {
            DataSet dataSet = BuServices.GetServicesForComboDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetAllProductStock(int page, string filter)
        {
            DataSet dataSet = BUProduct.GetAllProductStock(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteProductStock(string id)
        {
            return BUProduct.DeleteProductStock(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public string GetProductStock(string id)
        {
            NameValueCollection collection = BUProduct.GetProductStock(id);
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            DateTime date = Convert.ToDateTime(collection["date"]);
            if (date != DateTime.MinValue) dictionary["date"] = date.ToString("dd.MM.yyyy");           
            dictionary["stockquan"] = collection["stockquan"];
            dictionary["ponumber"] = collection["ponumber"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }


        #endregion

        #region Business Type

        [WebMethod(EnableSession = true)]
        public string GetBusinessTypes(int page, string filter)
        {
            DataSet dataSet = UserBA.GetBusinessTypes(filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                return dataSet.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBusinessType(string id)
        {
            NameValueCollection type = UserBA.GetBusinessType(id);
            if (type == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = type["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteBusinessType(int id)
        {
            return UserBA.DeleteBusinessType(id);
        }
        #endregion

        #region Service Type

        [WebMethod(EnableSession = true)]
        public string GetServiceTypeDetails(int page, string filter)
        {
            DataSet dsTypeClass = BuServices.GetServiceTypeDetails(filter, Session["companyid"].ToString());
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetServiceType(string id)
        {
            NameValueCollection collection = BuServices.GetServiceType(id, Session["companyid"].ToString());
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteServiceType(string id)
        {
            return BuServices.DeleteServiceType(id);
        }

        #endregion

        #region Product Category 

        [WebMethod(EnableSession = true)]
        public string GetProductCategoryDetails(int page, string filter)
        {
            DataSet dsTypeClass = BUProduct.GetProductCategoryDetails(filter, Session["companyid"].ToString());
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetProductCategory(string id)
        {
            NameValueCollection collection = BUProduct.GetProductCategory(id, Session["companyid"].ToString());
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProductCategory(string id)
        {
            return BUProduct.DeleteProductCategory(id);
        }

        #endregion

        #region Product Brand 

        [WebMethod(EnableSession = true)]
        public string GetProductBrandDetails(int page, string filter)
        {
            DataSet dsTypeClass = BUProduct.GetProductBrandDetails(filter, Session["companyid"].ToString());
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetProductBrand(string id)
        {
            NameValueCollection collection = BUProduct.GetProductBrands(id, Session["companyid"].ToString());
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProductBrand(string id)
        {
            return BUProduct.DeleteProductBrand(id);
        }

        #endregion

        #region Staff

        [WebMethod(EnableSession = true)]
        public int GetStaffCount(string filter)
        {
            return BUStaff.GetStaffCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetStaffDetails(int page, string filter, string order)
        {
            DataSet dataSet = BUStaff.GetStaffDetails(page, filter, order);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteStaff(string id)
        {
            return BUStaff.DeleteStaff(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public string GetStaffDepartments(int page, string filter)
        {
            DataSet dataSet = BUStaff.GetStaffDepartments(filter, Session["companyid"].ToString());
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                return dataSet.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetStaffDepartment(string id)
        {
            NameValueCollection department = BUStaff.GetStaffDepartment(id, Session["companyid"].ToString());
            if (department == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = department["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteStaffDepartment(int id)
        {
            return BUStaff.DeleteStaffDepartment(id);
        }

        [WebMethod(EnableSession = true)]

        public string GetStaffJobRoles(int page, string filter)
        {
            DataSet dataSet = BUStaff.GetStaffJobRoles(filter, Session["companyid"].ToString());
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                return dataSet.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetStaffJobRole(string id)
        {
            NameValueCollection jobrole = BUStaff.GetStaffJobRole(id, Session["companyid"].ToString());
            if (jobrole == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = jobrole["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteStaffJobRole(int id)
        {
            return BUStaff.DeleteStaffJobRole(id);
        }

        #endregion

        #region Shift

        [WebMethod(EnableSession = true)]
        public int GetShiftCount(string filter)
        {
            return Shift.GetShiftCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetShiftDetails(int page, string filter)
        {
            DataSet dataSet = Shift.GetShiftDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteShift(string id)
        {
            return Shift.DeleteShift(BASecurity.Decrypt(id, PageBase.HashKey));

        }

        #endregion

        #region Customer

        [WebMethod(EnableSession = true)]
        public int GetCustomerCount(string filter)
        {
            return BUCustomer.GetCustomerCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomerDetails(int page, string filter, string order)
        {
            DataSet dataSet = BUCustomer.GetCustomerDetails(page, filter, order);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteCustomer(string id)
        {
            return BUCustomer.DeleteCustomer(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        #endregion

        #region BU Tax

        [WebMethod(EnableSession = true)]
        public string GetAllBUTax(int page, string filter)
        {
            DataSet dsTypeClass = BUProduct.GetBUTaxDetails(filter, Session["companyid"].ToString());
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBUTax(string id)
        {
            NameValueCollection collection = BUProduct.GetBUTax(id, Session["companyid"].ToString());
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            dictionary["percentage"] = collection["percentage"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteBUTax(string id)
        {
            return BUProduct.DeleteBUTax(id);
        }

        #endregion

        #region BU Currency

        [WebMethod(EnableSession = true)]
        public string GetAllBUCurrency(int page, string filter)
        {
            DataSet dsTypeClass = BUProduct.GetBUCurrencyDetails(filter, Session["companyid"].ToString());
            if (dsTypeClass != null && dsTypeClass.Tables.Count > 0 && dsTypeClass.Tables[0].Rows.Count > 0)
                return dsTypeClass.GetXml();
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBUCurrency(string id)
        {
            NameValueCollection collection = BUProduct.GetBUCurrency(id, Session["companyid"].ToString());
            if (collection == null) return string.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["id"] = id;
            dictionary["name"] = collection["name"];
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteBUCurrency(string id)
        {
            return BUProduct.DeleteBUCurrency(id);
        }

        #endregion

        #endregion

        #region Order Management

        // new for bu dashboard
        [WebMethod(EnableSession = true)]
        public string BUDash_GetOrderByStatus(int page, string filter)
        {

            DataSet dataSet = BUOrderManagement.GetOrderByStatus(Session["companyid"]);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        // new for bu dashboard
        [WebMethod(EnableSession = true)]
        public string BUDash_GetOrderByType(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetOrderByType(Session["companyid"]);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string BUDash_GetItemCountData(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetItemCountData(Session["companyid"]);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public string GetBUOrderItems(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetBUOrderItems(filter, Session["companyid"]);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteBUOrderItem(string id)
        {
            return BUOrderManagement.DeleteBUOrderItem(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public int GetProcessingOrderCount(string filter)
        {
            return BUOrderManagement.GetProcessingOrderCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetProcessingOrderDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetProcessingOrderDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public bool Deleteorder(string id)
        {
            return BUOrderManagement.DeleteOrder(BASecurity.Decrypt(id, PageBase.HashKey));
        }

        [WebMethod(EnableSession = true)]
        public int GetTodayClosedOrderCount(string filter)
        {
            return BUOrderManagement.GetTodayClosedOrderCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTodayClosedOrderDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetTodayClosedOrderDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetTodayOrderCount(string filter)
        {
            return BUOrderManagement.GetTodayOrderCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetTodayOrderDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetTodayOrderDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetClosedOrderCount(string filter)
        {
            return BUOrderManagement.GetClosedOrderCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetClosedOrderDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetClosedOrderDetails(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #endregion

        #region Reports

        [WebMethod(EnableSession = true)]
        public int GetCustomerReportCount(string filter)
        {
            return BUOrderManagement.GetCustomerReportCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomerReportDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetCustomerReportDetails(page, filter);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "customer_id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }


        [WebMethod(EnableSession = true)]
        public int GetStaffReportCount(string filter)
        {
            return BUOrderManagement.GetStaffReportCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetStaffReportDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetStaffReportDetails(page, filter);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "user_id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetCustReportCount(string filter)
        {
            return BUOrderManagement.GetCustReportCount(filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustReportList(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetCustReportList(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

 
        [WebMethod(EnableSession = true)]
        public int GetStaffReportsCount(string filter)
        {
            return BUOrderManagement.GetStaffReportsCount(filter);
        }


        [WebMethod(EnableSession = true)]
        public string GetStaffReportsList(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetStaffReportsList(page, filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        public int GetCustomerOrderCount(string filter)
        {
            return BUOrderManagement.GetCustomerOrderCount(Session["userid"], filter);
        }

        [WebMethod(EnableSession = true)]
        public string GetCustomerOrderDetails(int page, string filter)
        {
            DataSet dataSet = BUOrderManagement.GetCustomerOrderDetails(page, Session["userid"], filter);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                BusinessBase.SecurePrimaryKey(PageBase.HashKey, ref dataSet, 0, "id");
                return dataSet.GetXml();
            }
            return string.Empty;
        }

        #endregion

        #region Common 

        [WebMethod(EnableSession = true)]
        public string getimageurl(string filename)
        {
            string returnPath = string.Empty;

            try
            {
                string extension = System.IO.Path.GetExtension(filename);
                if (!string.IsNullOrEmpty(extension)) extension = extension.ToLower();

                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                {
                    returnPath = BusinessBase.GetBase64ImageString(filename);
                }
                else if (extension == ".pdf")
                {
                    returnPath = "../app/images/icon_pdf_file.png";
                }
                else
                {
                    returnPath = "../app/images/icon_files.png";
                }
            }
            catch { }
            finally
            {
                if (string.IsNullOrEmpty(returnPath)) returnPath = "../images/blank.png";
            }
            return returnPath;
        }


        #endregion

    }
}