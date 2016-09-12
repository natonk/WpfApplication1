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
            columns = "wpId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpTypeId INTEGER, wpName VARCHAR(50) UNIQUE, wpDescription TEXT, wpCreated DATETIME, wpModified DATETIME";
            keys = ", FOREIGN KEY (wpTypeId) REFERENCES WPTypes(wpTypeId)";
        }

        public EAP_STATUS addWP( int aWPType, string aWPName, string aWPDescription, ref int newWPId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpTypeId, wpName, wpDescription, wpCreated, wpModified";
            string values;
            string wpTypeId;
            string wpName;
            string wpDescription;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            wpTypeId = aWPType.ToString();
            wpName = aWPName;
            wpDescription = aWPDescription;
            values = wpTypeId + ", " + wpName + ", " + wpDescription + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newWPId);

         
            return status;
        }

    }
}

