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

                cEAPWSEstimatesTable estTable = new cEAPWSEstimatesTable(m_DatabaseConnection);
                estTable.createTable();

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
                cEAPWSWPTable wpTable = new cEAPWSWPTable(m_DatabaseConnection);
                status = wpTable.stdPopulateTable();
            }
            if (EAP_STATUS.OK == status)
            {
                cEAPWSEstimatesTable estTable = new cEAPWSEstimatesTable(m_DatabaseConnection);
                status = estTable.stdPopulateTable();
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


   
        protected EAP_STATUS addEstimatesTable()
        {
            MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Estimates (id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpId INTEGER, competenceId INTEGER, M36 SMALLINT UNSIGNED, M35 SMALLINT UNSIGNED, M34 SMALLINT UNSIGNED, comment VARCHAR(255), created DATETIME, modified DATETIME, FOREIGN KEY (wpId) REFERENCES WorkPackage(wpId), FOREIGN KEY (competenceId) REFERENCES Competence(competenceId))";
            cmd.ExecuteNonQuery();
            return EAP_STATUS.OK;
        }

        protected EAP_STATUS addWPTable()
        {
            MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS WorkPackage (wpId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, wpType INTEGER, description TEXT, created DATETIME, modified DATETIME, FOREIGN KEY (wpType) REFERENCES WPTypes(wpType))";
            cmd.ExecuteNonQuery();
            return EAP_STATUS.OK;
        }
        protected EAP_STATUS addEndTimesTable() 
        {
            MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS EndTimes (id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, date DATE, calender INTEGER, wp INTEGER)";
            cmd.ExecuteNonQuery();
            return EAP_STATUS.OK;
        }

        protected EAP_STATUS populateStdEndTimeTables()
        {
            MySqlCommand cmd = this.m_DatabaseConnection.CreateCommand();
            cmd.CommandText = "INSERT INTO EndTimes (date, calender, wp) VALUES ('2017-05-30', 1, 1)";

            cmd.ExecuteNonQuery();
            
            return EAP_STATUS.OK;
        }

    }
}

