using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSUserTable : cEAPWSTable
    {

        public cEAPWSUserTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "WSUsers";
            columns = "wsuserId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wsId INTEGER, userId INTEGER, userTypeId INTEGER";
            keys = ", FOREIGN KEY (wsId) REFERENCES WorkSpace(wsId), FOREIGN KEY (userTypeId) REFERENCES UserType(userTypeId)";
        }

        public EAP_STATUS addUser( int aWSId, int aUserId, int aUserTypeId )
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wsId, userId, userTypeId";
            string values;
            string wsId;
            string userId;
            string userTypeId;
            
            wsId = aWSId.ToString();
            userId = aUserId.ToString();
            userTypeId = aUserTypeId.ToString();

            values = wsId + ", " + userId + ", " + userTypeId ;
            status = this.addRow(fields, values);

         
            return status;
        }

    }
}

