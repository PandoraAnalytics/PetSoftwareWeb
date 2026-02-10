using BADBUtils;
using System.Data;

namespace BABusiness
{
    public class BreederData : BusinessBase
    {
        public static DataTable GetBreedCategory()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from animal_categary where active=1";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetBreedTypes(string xiCategoryId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from animal_category_type where categoryid = " + Utils.ConvertToDBString(xiCategoryId, Utils.DataType.Integer) + " and active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetAllBreedTypes(object xiAssociation_BreedTypes)
        {
            string query = @"select act.id, act.[name], ac.breedname,(act.[name] + ' [' + ac.breedname + ']') as namewithbreedname 
from animal_category_type act inner join animal_categary ac on act.categoryid = ac.id where act.active = 1 
and ac.active = 1";
            if (xiAssociation_BreedTypes != null && xiAssociation_BreedTypes.ToString().Length > 0) query += " and act.id  in (" + Utils.ConvertToDBString(xiAssociation_BreedTypes, Utils.DataType.String) + ")";
            query += "order by ac.id, act.[name]";

            DBClass objdb = new DBClass();
            objdb.Connectdb();
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetContactProfessions()
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from contact_servicetype where active=1 order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }

        public static DataTable GetBUContactProfessions(int xiBUId)
        {
            DBClass objdb = new DBClass();
            objdb.Connectdb();
            string query = "select * from contact_servicetype where active=1 and (bu_id is null or bu_id = " + Utils.ConvertToDBString(xiBUId, Utils.DataType.Integer) + ") order by [name]";
            DataTable dataTable = objdb.ExecuteDataTable(objdb.con, query);
            objdb.Disconnectdb();

            return dataTable;
        }
    }
}
