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
            columns = "wpId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wsId INTEGER, wpTypeId INTEGER, wpName VARCHAR(50) UNIQUE, wpDescription TEXT, wpCreated DATETIME, wpModified DATETIME";
            keys = ", FOREIGN KEY (wsId) REFERENCES WorkSpace(wsId), FOREIGN KEY (wpTypeId) REFERENCES WPTypes(wpTypeId)";
        }

        public EAP_STATUS addWP( int aWSId, int aWPType, string aWPName, string aWPDescription, ref int newWPId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wsId, wpTypeId, wpName, wpDescription, wpCreated, wpModified";
            string values;
            string wsId;
            string wpTypeId;
            string wpName;
            string wpDescription;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            wsId = aWSId.ToString();
            wpTypeId = aWPType.ToString();
            wpName = aWPName;
            wpDescription = aWPDescription;
            values = wsId + ", " + wpTypeId + ", " + wpName + ", " + wpDescription + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newWPId);

         
            return status;
        }

    }
}

