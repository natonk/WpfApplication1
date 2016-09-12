using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSCalenderedNewestWPJobEstView : cEAPWSView
    {
        public cEAPWSCalenderedNewestWPJobEstView(MySqlConnection aConnection) : base(aConnection)
        {
            viewName = "calendered_newest_wp_job_est_view";
            cmdString = "(SELECT wpId, wpTypeId, wpName, compId, jobComment, jobCreated, jobModified, estPeriod, estType, hours, max(estCreated) AS estCreated FROM eaptestdatabaseexample.wp_job_est_view GROUP BY wpId, compId, estPeriod)";
            cmdString = "(SELECT calItems.calItemId, calItems.calId, calItems.wpId, calItems.calItemEndTime, newest_wp_job_est_view.wpTypeId, newest_wp_job_est_view.wpName, newest_wp_job_est_view.compId, newest_wp_job_est_view.jobComment, newest_wp_job_est_view.jobCreated, newest_wp_job_est_view.jobModified, newest_wp_job_est_view.estPeriod, newest_wp_job_est_view.estType, newest_wp_job_est_view.hours, newest_wp_job_est_view.estCreated FROM calItems INNER JOIN newest_wp_job_est_view ON calItems.wpId=newest_wp_job_est_view.wpId)";
        }
    }
}
