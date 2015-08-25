using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Server
{
    class Oracle
    {
        public static void AscmUnloadingPointUpdate(AscmUnloadingPoint point)
        {
            try
            {
                String ConnectionString = " Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.16.9.191)(PORT=1522))(CONNECT_DATA=(SERVICE_NAME=md_ascm)));User Id=ascm;Password=AScm1240#;";
                OracleConnection conn = new OracleConnection(ConnectionString);
                conn.Open();
                string modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "update ascm_unloading_point set status = '"+ point.status + "', modifytime = '" + modifyTime + "' where id = " + point.id;
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception e)
            {
            }
        }
    }
}
