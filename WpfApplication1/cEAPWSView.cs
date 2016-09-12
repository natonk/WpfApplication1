using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    abstract class cEAPWSView
    {
        MySqlConnection connection;
        protected string viewName ="";
        protected string cmdString = "";
        
        protected cEAPWSView(MySqlConnection aConnection)
        {
            connection = aConnection;
        }

        public virtual EAP_STATUS createView()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE VIEW " + viewName +" AS (" + cmdString + ")";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                status = EAP_STATUS.CREATE_VIEW_FAILED;
            }
            return status;
        }
    }
}
