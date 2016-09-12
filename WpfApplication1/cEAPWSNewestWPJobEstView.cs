using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSNewestWPJobEstView : cEAPWSView
    {
        public cEAPWSNewestWPJobEstView(MySqlConnection aConnection) : base(aConnection)
        {
            viewName = "newest_wp_job_est_view";
            cmdString = "(SELECT wpId, wpTypeId, wpName, wpDescription, compId, jobComment, jobCreated, jobModified, estPeriod, estType, hours, max(estCreated) as estCreated FROM eaptestdatabaseexample.wp_job_est_view GROUP BY wpId, compId, estPeriod)";
            
        } 
    }
}
