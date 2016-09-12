using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSNewestEstView : cEAPWSView
    {
        public cEAPWSNewestEstView(MySqlConnection aConnection) : base(aConnection)
        {
            viewName = "newest_est_view";
            cmdString = "(SELECT max(estCreated) mostRecent, estTimeId, jobId, estPeriod, hours, estCreated FROM eaptestdatabaseexample.esttimes GROUP BY jobId, estPeriod)";
            
        } 
    }
}
