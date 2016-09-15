using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSWSTable : cEAPWSTable
    {

        public cEAPWSWSTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "WorkSpace";
            columns = "wsId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wsName VARCHAR(50), wsDescription TEXT, wsCreated DATETIME, wsModified DATETIME";
            keys = "";
        }

        public EAP_STATUS addWS( string aWSName, string aWSDescription, ref int newWSId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wsName, wsDescription, wsCreated, wsModified";
            string values;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

           values = aWSName + ", " + aWSDescription + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newWSId);

         
            return status;
        }

    }
}

