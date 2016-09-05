using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSEstTimePeriodTable : cEAPWSTable
    {

        public cEAPWSEstTimePeriodTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "EstTimePeriod";
            columns = "timePeriodId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, description CHAR(15) UNIQUE";
            keys = "";
        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            return status;
        }

        public override EAP_STATUS createTable()
        {
           
            EAP_STATUS status = EAP_STATUS.OK;

            base.createTable();

            string fields = "description";
            string values;
            string description;

            description = "'M1'";
            values = description;
            status = this.addRow(fields, values);

            return status;
        }


}
}

