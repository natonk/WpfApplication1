using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace EstimationsAndPlanning
{

    enum EAP_STATUS { OK, FAILED, CREATE_TABLE_FAILED, CREATE_DATABASE_FAILED, ADD_ROW_FAILED, CREATE_VIEW_FAILED };

    class cEAPWorkspace
    {
        MySqlConnection m_DatabaseConnection;
        string m_server = "localhost";
        string m_user = "root";
        string m_port = "3306";
        string m_password = "F98claes";
        string m_database = "EAPDataBase";

        public EAP_STATUS createNewEmptyDatabase(String aDataBaseName)
        {
            try
            {
                string connString = "Server=" + m_server + ";user=" + m_user + ";port=" + m_port + ";password=" + m_password;
                this.m_DatabaseConnection = new MySqlConnection(connString);
                try
                {
                    //*** Connect to SQL Server
                    Console.WriteLine("Connecting to MySQL...");
                    this.m_DatabaseConnection.Open();
                    //*** Drop database if it already exist
                    MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
                    cmd.CommandText = "DROP DATABASE IF EXISTS " + aDataBaseName;
                    Console.WriteLine("Droping database " + aDataBaseName + "if it exist");
                    cmd.ExecuteNonQuery();
                    //*** Create New database if it not exist
                    cmd.CommandText = "CREATE DATABASE IF NOT EXISTS " + aDataBaseName;
                    Console.WriteLine("Creating database " + aDataBaseName + "if it not exist");
                    cmd.ExecuteNonQuery();
                    m_database = aDataBaseName;
                    //*** Close connection to server
                    Console.WriteLine("Closinng connection to SQL server");
                    m_DatabaseConnection.Close();
                    //*** Connect to the newly created database
                    connString = "Server=" + m_server + ";database=" + m_database + ";user=" + m_user + ";port=" + m_port + ";password=" + m_password;
                    this.m_DatabaseConnection.ConnectionString = connString;
                    Console.WriteLine("Connecting to MySQL Database " + m_database);
                    this.m_DatabaseConnection.Open();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                cEAPWSProjectTypesTable projectTypesTable = new cEAPWSProjectTypesTable( m_DatabaseConnection );
                projectTypesTable.createTable();

                cEAPWSCompetenceTypesTable competenceTypesTable = new cEAPWSCompetenceTypesTable(m_DatabaseConnection);
                competenceTypesTable.createTable();

                cEAPWSWPTable wpTable = new cEAPWSWPTable(m_DatabaseConnection);
                wpTable.createTable();

                cEAPWSEstTimePeriodTable estTimePeriodTable = new cEAPWSEstTimePeriodTable(m_DatabaseConnection);
                estTimePeriodTable.createTable();

                cEAPWSJobsTable jobsTable = new cEAPWSJobsTable(m_DatabaseConnection);
                jobsTable.createTable();

                cEAPWSEstTimesTable estTimesTable = new cEAPWSEstTimesTable(m_DatabaseConnection);
                estTimesTable.createTable();

                cEAPWSCalenderTable calenderTable = new cEAPWSCalenderTable(m_DatabaseConnection);
                calenderTable.createTable();

                cEAPWSCalenderItemTable calenderItemTable = new cEAPWSCalenderItemTable(m_DatabaseConnection);
                calenderItemTable.createTable();

                cEAPWSNewestEstView newestEstView = new cEAPWSNewestEstView(m_DatabaseConnection);
                newestEstView.createView();

                cEAPWSJobEstView jobEstView = new cEAPWSJobEstView(m_DatabaseConnection);
                jobEstView.createView();

                cEAPWSWPJobEstView wpJobEstView = new cEAPWSWPJobEstView(m_DatabaseConnection);
                wpJobEstView.createView();

                cEAPWSNewestWPJobEstView newestWPJobEstView = new cEAPWSNewestWPJobEstView(m_DatabaseConnection);
                newestWPJobEstView.createView();

                cEAPWSCalenderedNewestWPJobEstView caledNewestWPJobEstView = new cEAPWSCalenderedNewestWPJobEstView(m_DatabaseConnection);
                caledNewestWPJobEstView.createView();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return EAP_STATUS.OK;     
        }

        public EAP_STATUS createNewStdDatabase(String aDataBaseName)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            status = this.createNewEmptyDatabase( aDataBaseName );
            if (EAP_STATUS.OK == status)
            {
                //status = this.populateStdWPTypesTable();
                cEAPWSProjectTypesTable projectTypesTable = new cEAPWSProjectTypesTable(m_DatabaseConnection);
                status = projectTypesTable.stdPopulateTable();

            }
            if (EAP_STATUS.OK == status)
            {
                //status = this.populateStdCompetenceTypesTable();
                cEAPWSCompetenceTypesTable competenceTypesTable = new cEAPWSCompetenceTypesTable(m_DatabaseConnection);
                status = competenceTypesTable.stdPopulateTable();
            }
            
            if (EAP_STATUS.OK == status)
            {
                //status = this.populateStdEndTimeTables();
            }
            return status;
        }

        public EAP_STATUS openExistingDatabase(String aDataBaseName)
        {
            EAP_STATUS status;
            status = this.createNewEmptyDatabase(aDataBaseName);
            return status;
        }

        public EAP_STATUS closeDatabase()
        {
            m_DatabaseConnection.Close();
            return EAP_STATUS.OK;
        }


   
        protected EAP_STATUS addEndTimesTable() 
        {
            MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS EndTimes (id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, date DATE, calender INTEGER, wp INTEGER)";
            cmd.ExecuteNonQuery();
            return EAP_STATUS.OK;
        }

        public EAP_STATUS populateDatabaseWithExamples(String aDataBaseName)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            int lastAddedWPId = 0;
            int lastAddedJobId = 0;

            const int compSW = 1;
            const int compSysVer = 2;
            const int compMech = 3;
            const int compElec = 4;
            const int compTechInfo = 5;
            const int compProj = 6;

            const int wpTypeR = 1;
            const int wpTypeT = 2;
            const int wpTypeP = 3;
            const int wpTypeM = 4;

            int calenderAgressive = 0;
            int calenderSafe = 0;


            cEAPWSWPTable wpTable = new cEAPWSWPTable(m_DatabaseConnection);
            cEAPWSJobsTable jobsTable = new cEAPWSJobsTable(m_DatabaseConnection);
            cEAPWSEstTimesTable estTimesTable = new cEAPWSEstTimesTable(m_DatabaseConnection);
            cEAPWSCalenderTable calenderTable = new cEAPWSCalenderTable(m_DatabaseConnection);
            cEAPWSCalenderItemTable calenderItemTable = new cEAPWSCalenderItemTable(m_DatabaseConnection);

            if (EAP_STATUS.OK == status)
            {
                status = calenderTable.addCalender( "'PM -Agressive'", "'Product Managers aggresiv suggestion for product/project roadmap'", ref calenderAgressive);
            }
            if (EAP_STATUS.OK == status)
            {
                status = calenderTable.addCalender( "'PM -Safe'", "'Product Managers safe suggestion for product/project roadmap'", ref calenderSafe);
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeR, "'R - Brushless Main Motor'", "'Research if it is feasible from a cost and performance perspective to use a brushless main motor. If so the project shall have a recommendation of supplier and a specific motor'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "'Work for implemetation of test sw for lab purposes'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 80, 2, 0, "20160824120000");
                    }
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 80, 2);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compElec, "'Work for evaluating different options regarding brushless main motor'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Small follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 25, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD CALENDER ITEM ******************
                    DateTime date = new DateTime(2016,01,23, 12, 0, 0);
                    date = date.ToUniversalTime();
                    status = calenderItemTable.addCalenderItem(calenderAgressive, lastAddedWPId, date.ToString());
                }
            }

            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeR, "'R - RFID identification of accessories'", "'Research if it is feasible from a cost and performance perspective to use RFID to detect differnt attached accessories. Identify associated/possible features. Evaluate customer value.'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "'Work for implemetation of test sw for lab purposes, Workshops for feature identification'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 3);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compElec, "'Work for evaluating different options regarding RFID identification'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 80, 6);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeR, "'R - Blender drive line  Gen 4'", "'Research a solution for a new drive line (motor, transmission) lighter, longer life, higher speed and more torque. Evaluate customer value.'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "'Work for for investigating, specifying and elimanate risks'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 2000, 12);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 160, 12);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeR, "'R - Blender cover set  Gen 4'", "'Research a solution for a new cover set. Module, reusable for many models and a modern look'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "'Work for for investigating, preliminar design, specifying and elimanate risks'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 2000, 12);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 160, 12);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeT, "'T - Brushless main motor'", "'Develop technology for first use in a product accordint to specification decided based on coresponding R-project'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "'Work for implemetation of driver of brushless main motor'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6, 3);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "'Work for verification of quality and usability of brushless main motor'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 6,6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compElec, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 80, 1, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 240, 12);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeT, "'T - RFID identification of accessories'", "'Develop technology for first use in a product according to specification decided based on coresponding R-project. Includes development/modofocation of 3 accesories to use RFID'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6, 3);
                    }

                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 80, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compElec, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 6, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 180, 3, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 240, 12);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeT, "'T - Facelift Main Cover Design'", "'Complete new cover set'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 30, 8, 4);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 800, 8, 6);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 10, 4);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeT, "'T - Battery Low warning to App'", "'Add battery low warning to existing apps for ios and android'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 100, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 20, 1, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Tiny follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 5, 2, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 2 Low-Range classic'", "'Facelift of existing model'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 100, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 2 Mid-Range classic'", "'Facelift of existing model'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 100, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 2 High-Range classic'", "'Facelift of existing model'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 100, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 3 Low-Range'", "'New model on platform Gen 3'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 3 Mid-Range'", "'New model on platform Gen 3'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeP, "'P - Blender Gen 3 High-Range'", "'New model on platform Gen 3'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 120, 2, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 6, 1);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 400, 6, 3);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 200, 2, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compProj, "'Medium follow up work'", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 300, 9, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeM, "'M - Product Maintenance Blenders Gen 2'", "'Planned Maintenance for 3 years after launch'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 180, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 180, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 360, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 72, 36, 0);
                    }
                }
            }
            if (EAP_STATUS.OK == status)
            {
                // ****** ADD WORKPACKAGE ******************
                status = wpTable.addWP(wpTypeM, "'M - Product Maintenance Blenders Gen 3'", "'Planned Maintenance for 3 years after launch'", ref lastAddedWPId);
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSW, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 360, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compSysVer, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 240, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compMech, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 360, 36, 0);
                    }
                }
                if (EAP_STATUS.OK == status)
                {
                    // ****** ADD JOB ******************
                    status = jobsTable.addJob(lastAddedWPId, compTechInfo, "''", ref lastAddedJobId);
                    if (EAP_STATUS.OK == status)
                    {
                        // ****** ADD ESTIMATES ******************
                        status = estTimesTable.addEstimation(lastAddedJobId, 72, 36, 0);
                    }
                }
            }
            return status;
        }

    }
}

