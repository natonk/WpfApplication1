using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSCalenderTable : cEAPWSTable
    {
        public cEAPWSCalenderTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "Calenders";
            columns = "calId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wsId INTEGER, calName VARCHAR(50) UNIQUE, calDescription text, calCreated DATETIME, calModified DATETIME";
            keys = "";
        }

        public EAP_STATUS addCalender( int aWSId, string  aName, string aDescription, ref int newCalenderId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wsId, calName, calDescription, calCreated, calModified";
            string values;
            string wsId;
            string created;
            string modified;

            wsId = aWSId.ToString();
            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            values = wsId + ", " + aName + ", " + aDescription + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newCalenderId);
            
            return status;
        }
        
    }
}
