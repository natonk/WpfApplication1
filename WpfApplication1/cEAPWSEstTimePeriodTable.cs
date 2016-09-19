using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EstimationsAndPlanning
{
    class cEAPWSEstTimePeriodTable : cEAPWSTable
    {

        public cEAPWSEstTimePeriodTable(MySqlConnection aConnection) : base(aConnection)
        {
            tableName = "esttimeperiod";
            columns = "timePeriodId INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, timePeriodDescription CHAR(15) UNIQUE";
            keys = "";
        }

        public override EAP_STATUS createTable()
        {

            EAP_STATUS status = EAP_STATUS.OK;

            base.createTable();

            string fields = "timePeriodDescription";
            string values;
            string description;

            if (EAP_STATUS.OK == status)
            {
                description = "'M1'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M2'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M3'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M4'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M5'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M6'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M7'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M8'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M9'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M10'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M11'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M12'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M13'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M14'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M15'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M16'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M17'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M18'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M19'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M20'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M21'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M22'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M23'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M24'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M25'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M26'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M27'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M28'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M29'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M30'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M31'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M32'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M33'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M34'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M35'";
                values = description;
                status = this.addRow(fields, values);
            }
            if (EAP_STATUS.OK == status)
            {
                description = "'M36'";
                values = description;
                status = this.addRow(fields, values);
            }
            return status;
        }
    }
}

