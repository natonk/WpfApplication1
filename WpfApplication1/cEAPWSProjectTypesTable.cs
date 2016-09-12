using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSProjectTypesTable : cEAPWSTable
    {
        
        public cEAPWSProjectTypesTable(MySqlConnection aConnection): base(aConnection)
        {
            tableName = "WPTypes";
            columns = "wpTypeId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, name_short CHAR(1) UNIQUE, name_long VARCHAR(40) UNIQUE, description TEXT";
            keys = "";

        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "name_short, name_long, description";
            string values;
            string name_short;
            string name_long;
            string description;

            name_short = "'R'";
            name_long = "'R - Research Project/WP'";
            description = "'Work package or Project that is needed to be finshied to find a suitable solution and identify and remove extensive risks. When a reaserch project is finished it shall be possible to make a decission if to start a Technology Project and supply, benefits, specifications, estimations and risks'";
            values = name_short + ", " + name_long + ", " + description;
            status = this.addRow(fields, values);

            name_short = "'T'";
            name_long = "'T - Technology Project/WP'";
            description = "'Work package or Project that develops a new part/feature/functionality that is reusable for different products. If a corresponding R-project/WP is needed, that needs to be finished before the T-Project/WP can be started. The T-Project/WP needs to be finshed on the latest at the time when the first P-Project that features the Technology is production start.'";
            values = name_short + ", " + name_long + ", " + description;
            status = this.addRow(fields, values);
  
            name_short = "'P'";
            name_long = "'P - Product Project/WP'";
            description = "'Work package or Project that reusults in a product beeing launched.'";
            values = name_short + ", " + name_long + ", " + description;
            status = this.addRow(fields, values);
            
            name_short = "'M'";
            name_long = "'M - Maintainance Project/WP'";
            description = "'Work package or Project for maintenance work. Typically added as one project/WP per maintenance type and year.'";
            values = name_short + ", " + name_long + ", " + description;
            status = this.addRow(fields, values);
            
            return status;
        }

    }
}
