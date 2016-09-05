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

    enum EAP_STATUS { OK, FAILED, CREATE_TABLE_FAILED, CREATE_DATABASE_FAILED, ADD_ROW_FAILED };

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

                
                //this.addCompetenceTypesTable();
                //this.addWPTable();
                //this.addEndTimesTable();
                //this.addEstimatesTable();

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


            cEAPWSWPTable wpTable = new cEAPWSWPTable(m_DatabaseConnection);
            cEAPWSJobsTable jobsTable = new cEAPWSJobsTable(m_DatabaseConnection);
            cEAPWSEstTimesTable estTimesTable = new cEAPWSEstTimesTable(m_DatabaseConnection);

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
            return status;
        }

    }
}

