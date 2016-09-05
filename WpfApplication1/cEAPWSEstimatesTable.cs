using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSEstimatesTable : cEAPWSTable
    {
        UInt16[] monthEst = new UInt16[36];

        public cEAPWSEstimatesTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "Estimates";
            columns = "id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpId INTEGER, competenceId INTEGER, M35 SMALLINT UNSIGNED, M34 SMALLINT UNSIGNED, M33 SMALLINT UNSIGNED, M32 SMALLINT UNSIGNED, M31 SMALLINT UNSIGNED, M30 SMALLINT UNSIGNED, M29 SMALLINT UNSIGNED, M28 SMALLINT UNSIGNED, M27 SMALLINT UNSIGNED, M26 SMALLINT UNSIGNED, M25 SMALLINT UNSIGNED, M24 SMALLINT UNSIGNED, M23 SMALLINT UNSIGNED, M22 SMALLINT UNSIGNED, M21 SMALLINT UNSIGNED, M20 SMALLINT UNSIGNED, M19 SMALLINT UNSIGNED, M18 SMALLINT UNSIGNED, M17 SMALLINT UNSIGNED, M16 SMALLINT UNSIGNED, M15 SMALLINT UNSIGNED, M14 SMALLINT UNSIGNED, M13 SMALLINT UNSIGNED, M12 SMALLINT UNSIGNED, M11 SMALLINT UNSIGNED, M10 SMALLINT UNSIGNED, M9 SMALLINT UNSIGNED, M8 SMALLINT UNSIGNED, M7 SMALLINT UNSIGNED, M6 SMALLINT UNSIGNED, M5 SMALLINT UNSIGNED, M4 SMALLINT UNSIGNED, M3 SMALLINT UNSIGNED, M2 SMALLINT UNSIGNED, M1 SMALLINT UNSIGNED, M0 SMALLINT UNSIGNED, comment VARCHAR(255), created DATETIME, modified DATETIME";
            keys = ", FOREIGN KEY (competenceId) REFERENCES Competences(competenceId)";
        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "wpId, competenceId, M35, M34, M33, M32, M31, M30, M29, M28, M27, M26, M25, M24, M23, M22, M21, M20, M19, M18, M17, M16, M15, M14, M13, M12, M11, M10, M9, M8, M7, M6, M5, M4, M3, M2, M1, M0, comment, created, modified";
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
                values = wpId + ", " + competenceId + ", " + monthEst[35] + ", " + monthEst[34] + ", " + monthEst[33] + ", " + monthEst[32] + ", " + monthEst[31] + ", " + monthEst[30] + ", " + monthEst[29] + ", " + monthEst[28] + ", " + monthEst[27] + ", " + monthEst[26] + ", " + monthEst[25] + ", " + monthEst[24] + ", " + monthEst[23] + ", " + monthEst[22] + ", " + monthEst[21] + ", " + monthEst[20] + ", " + monthEst[19] + ", " + monthEst[18] + ", " + monthEst[17] + ", " + monthEst[16] + ", " + monthEst[15] + ", " + monthEst[14] + ", " + monthEst[13] + ", " + monthEst[12] + ", " + monthEst[11] + ", " + monthEst[10] + ", " + monthEst[9] + ", " + monthEst[8] + ", " + monthEst[7] + ", " + monthEst[6] + ", " + monthEst[5] + ", " + monthEst[4] + ", " + monthEst[3] + ", " + monthEst[2] + ", " + monthEst[1] + ", " + monthEst[0] + ", " + comment + ", " + created + ", " + modified;
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
                    UInt16 permonth = checked((UInt16) (hours / month));
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
                        if (i < permonthmod )
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

