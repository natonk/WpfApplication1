using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSJobsTable : cEAPWSTable
    {
        
        public cEAPWSJobsTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "Jobs";
            columns = "jobId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpId INTEGER, compId INTEGER, jobComment VARCHAR(255), jobCreated DATETIME, jobModified DATETIME";
            keys = ", FOREIGN KEY (compId) REFERENCES Competences(compId), FOREIGN KEY (wpId) REFERENCES WorkPackage(wpId)";
        }

        public EAP_STATUS addJob(int aWPId, int aCompId, string  aComment, ref int newJobId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpId, compId, jobComment, jobCreated, jobModified";
            string values;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            values = aWPId.ToString() + ", " + aCompId.ToString() + ", " + aComment + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newJobId);
            
            return status;
        }
    }
}
