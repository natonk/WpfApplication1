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
        UInt16[] monthEst = new UInt16[36];

        public cEAPWSJobsTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "Jobs";
            columns = "jobId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpId INTEGER, competenceId INTEGER, comment VARCHAR(255), created DATETIME, modified DATETIME";
            keys = ", FOREIGN KEY (competenceId) REFERENCES Competences(competenceId), FOREIGN KEY (wpId) REFERENCES WorkPackage(wpId)";
        }

        public EAP_STATUS addJob(int aWPId, int aCompId, string  aComment, ref int newJobId)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpId, competenceId, comment, created, modified";
            string values;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            values = aWPId.ToString() + ", " + aCompId.ToString() + ", " + aComment + ", " + created + ", " + modified;
            status = this.addRow(fields, values, ref newJobId);
            
            return status;
        }
        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpId, competenceId, comment, created, modified";
            string values;
            string wpId;
            string competenceId;
            string comment;
            string created;
            string modified;

            created = "UTC_TIMESTAMP()";
            modified = "UTC_TIMESTAMP()";

            if (EAP_STATUS.OK == status)
            {
                wpId = "1";
                competenceId = "1";
                comment = "'Very rough estimates see also kalle.doc'";
                this.distributeEstimate(200, 6, distributionEnum.EVEN);
                values = wpId + ", " + competenceId + ", " + comment + ", " + created + ", " + modified;
                status = this.addRow(fields, values);
            }

            return status;
        }

        public EAP_STATUS distributeEstimate(UInt16 hours, UInt16 month, distributionEnum dist)
        {
            switch (dist)
            {
                case distributionEnum.NORMAL:
                // Fall through
                case distributionEnum.EARLY:
                // Fall through
                case distributionEnum.LATE:
                // Fall through
                case distributionEnum.EVEN:
                    UInt16 permonth = checked((UInt16)(hours / month));
                    UInt16 permonthmod = checked((UInt16)(hours % month));
                    UInt16 sum = 0;
                    for (int i = 35; i >= 0; i--)
                    {
                        if (i < month)
                        {
                            monthEst[i] = permonth;
                            sum += permonth;
                        }
                        else
                        {
                            monthEst[i] = 0;

                        }
                        if (i < permonthmod)
                        {
                            monthEst[i] += 1;
                            sum += 1;
                        }

                    }
                    break;
                default:
                    break;
            }
            return EAP_STATUS.OK;
        }

        public enum distributionEnum
        {
            EVEN, NORMAL, EARLY, LATE

        }
    }
}
