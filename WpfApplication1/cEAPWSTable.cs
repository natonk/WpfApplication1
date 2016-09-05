using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    abstract class cEAPWSTable
    {
        MySqlConnection connection;
        protected string tableName ="";
        protected string columns = "";
        protected string keys = "";

        protected cEAPWSTable(MySqlConnection aConnection)
        {
            connection = aConnection;
        }

        public EAP_STATUS createTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + tableName +"(" + columns + keys +")";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                status = EAP_STATUS.CREATE_TABLE_FAILED;
            }
            return status;
        }

        public abstract EAP_STATUS stdPopulateTable();
        
        protected EAP_STATUS addRow(string fields, string values)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO " + tableName + " (" + fields +") VALUES (" + values + ")";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                status = EAP_STATUS.ADD_ROW_FAILED;
            }
            return status;
        }

    }
}
