using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSCompetenceTypesTable : cEAPWSTable
    {

        public cEAPWSCompetenceTypesTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "Competences";
            columns = "competenceId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, name VARCHAR(40), description TEXT";
            keys = "";

        }

        public override EAP_STATUS stdPopulateTable()
        {
            EAP_STATUS status = EAP_STATUS.OK;
            string fields = "name, description";
            string values;
            string name;
            string description;

            name = "'Software Design'";
            description = "'Software design and implementation'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'System Verification'";
            description = "'Software test and system verification'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'Mechanical Design'";
            description = "'Mechanical design and implementation'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'Electronics Design'";
            description = "'Electronics design and implementation'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'Technical Information '";
            description = "'Manuals, online help, application texts'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            name = "'Project Management'";
            description = "'Project management for driving the project'";
            values = name + ", " + description;
            status = this.addRow(fields, values);

            return status;
        }

    }
}

