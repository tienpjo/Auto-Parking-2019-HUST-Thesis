using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Auto_parking.SQL
{
    public class SQL_Connect
    {
        string conStr;
        public SQL_Connect()
        {
            conStr = "Data Source=TIENPJO\\SQLEXPRESS;Initial Catalog=data_ParkingBKA;Integrated Security=True";
        }
        public SqlConnection getConnect()
        {
            return new SqlConnection(conStr);

        }
    }

}

