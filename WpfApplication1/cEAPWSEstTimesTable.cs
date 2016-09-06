using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSEstTimesTable : cEAPWSTable
    {
        UInt16[] monthEst = new UInt16[36];

        public cEAPWSEstTimesTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "EstTimes";
            columns = "estTimeId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, jobId INTEGER, estPeriod INTEGER, estType ENUM('MIN', 'LIKELY', 'MAX'),hours SMALLINT UNSIGNED, created DATETIME";
            keys = ", FOREIGN KEY (jobId) REFERENCES Jobs(jobId), FOREIGN KEY (estPeriod) REFERENCES EstTimePeriod(timePeriodId)";
        }

        public EAP_STATUS addEstimation( int aJobId, UInt16 aNbrHours, UInt16 aNbrMonths, UInt16 endOffsetMonths = 0  )
        {
            EAP_STATUS status = EAP_STATUS.OK;

            string fields = "jobId, estPeriod, estType, hours, created";
            string values;
            int jobId = aJobId;
            int timePeriod;
            string estType = "'LIKELY'";
            string hours;
            string created = "UTC_TIMESTAMP()";


            // Validate in parameters
            if (MAX_NR_HOURS < aNbrHours) { aNbrHours = MAX_NR_HOURS; }
            if (MAX_NR_MONTHS < aNbrMonths) { aNbrMonths = MAX_NR_MONTHS; }
            if (MAX_NR_MONTHS < (aNbrMonths+ endOffsetMonths)) { endOffsetMonths = (UInt16)(MAX_NR_MONTHS-aNbrMonths); }
            
            jobId = aJobId;
            status = this.distributeEstimate(aNbrHours, aNbrMonths, distributionEnum.EVEN, endOffsetMonths);
            for (int i = 35; i >= 0; i--)
            {
                if ((EAP_STATUS.OK == status) && (monthEst[i] != 0))
                {
                    timePeriod = i + 1;
                    hours = (monthEst[i]).ToString();
                    values = jobId + ", " + timePeriod + ", " + estType + ", " + hours + ", " + created;
                    status = this.addRow(fields, values);
                }
            }
            return status;
        }


        public EAP_STATUS distributeEstimate(UInt16 hours, UInt16 month, distributionEnum dist, UInt16 endOffsetMonths)
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
                        if ( (i < (month+endOffsetMonths)) && (i >= endOffsetMonths) )
                        {
                            monthEst[i] = permonth;
                            sum += permonth;
                        }
                        else
                        {
                            monthEst[i] = 0;

                        }
                        if ( (i < (permonthmod + endOffsetMonths)) && (i >= endOffsetMonths) )
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

        const UInt16 MAX_NR_MONTHS = 16;
        const UInt16 MAX_NR_HOURS = 9999;

    }
}

