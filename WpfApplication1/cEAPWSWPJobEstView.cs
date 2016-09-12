using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSWPJobEstView : cEAPWSView
    {
        public cEAPWSWPJobEstView(MySqlConnection aConnection) : base(aConnection)
        {
            viewName = "wp_job_est_view";
            cmdString = "(SELECT workpackage.wpId, workpackage.wpTypeId, workpackage.wpName, workpackage.wpDescription, job_est_view.compId, job_est_view.jobComment, job_est_view.jobCreated, job_est_view.jobModified, job_est_view.estPeriod, job_est_view.estType, job_est_view.hours, job_est_view.estCreated FROM workpackage INNER JOIN job_est_view ON workpackage.wpId=job_est_view.wpId)";

        }
    }
}
