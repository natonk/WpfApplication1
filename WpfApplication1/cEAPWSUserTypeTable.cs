using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSUserTypeTable : cEAPWSTable
    {

        public cEAPWSUserTypeTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "UserType";
            columns = "userTypeId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, userTypeName VARCHAR(50) UNIQUE, userTypeDescription TEXT";
            keys = "";
        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "userTypeName, userTypeDescription";
            string values;
            string name;
            string description;

            name = "'Estimator'";
            description = "'Estimates WP/Projects'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'Scheduler'";
            description = "'Creates and schedules calenders'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            return status;
        }

        public EAP_STATUS addUserType( string aUserTypeName, string aUserTypeDescription)
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "userTypeName, userTypeDescription";
            string values;
            
            values = aUserTypeName + ", " + aUserTypeDescription;
            status = this.addRow(fields, values);

         
            return status;
        }

    }
}

