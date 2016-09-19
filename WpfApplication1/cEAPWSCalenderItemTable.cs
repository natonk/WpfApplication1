using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSCalenderItemTable : cEAPWSTable
    {

        public cEAPWSCalenderItemTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "calitems";
            columns = "calItemId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, calId INTEGER, wpId INTEGER, calItemEndTime DATETIME, calItemCreated DATETIME";
            keys = ", FOREIGN KEY (calId) REFERENCES calendars(calId), FOREIGN KEY (wpId) REFERENCES workpackage(wpId)";
        }

        public EAP_STATUS addCalenderItem(int aCalId, int aWPId, string date)
        {
            EAP_STATUS status = EAP_STATUS.OK;

            string fields = "calId, wpId, calItemEndTime, calItemCreated";
            string values;
            string created = "UTC_TIMESTAMP()";
            values = aCalId.ToString() + ", " + aWPId.ToString() + ", '" + date + "'," + created;
            status = this.addRow(fields, values);



            return status;
        }

    }
}

