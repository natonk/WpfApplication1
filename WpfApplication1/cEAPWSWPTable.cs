using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSWPTable : cEAPWSTable
    {

        public cEAPWSWPTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "WorkPackage";
            columns = "wpId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpType INTEGER, description TEXT, created DATETIME, modified DATETIME";
            keys = ", FOREIGN KEY (wpType) REFERENCES WPTypes(wpType)";
        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpType, description, created, modified";
            string values;
            string wpType;
            string description;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            wpType = "1";
            description = "'RFID identification of accessories'";
            values = wpType + ", " + description + ", " + created + ", " + modified;
            status = this.addRow(fields, values);

         
            return status;
        }

    }
}

