using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSJobEstView : cEAPWSView
    {
        public cEAPWSJobEstView(MySqlConnection aConnection) : base(aConnection)
        {
            viewName = "job_est_view";
            cmdString = "(SELECT jobs.wpId, jobs.compId, jobs.jobComment, jobs.jobCreated, jobs.jobModified, esttimes.estPeriod, esttimes.estType, esttimes.hours, esttimes.estCreated FROM jobs INNER JOIN esttimes ON jobs.jobId=esttimes.jobId)";
            
        } 
    }
}
