using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Auto_parking.SQL
{
    public class SQLDAO
    {
        SQL_Connect dc;
        SqlDataAdapter da;
        SqlCommand cmd;
        public SQLDAO()
        {
            dc = new SQL_Connect();
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public DataTable get_All_Data()
        {
            string sql = "SELECT * FROM data_BKA";
            SqlConnection con = dc.getConnect();
            da = new SqlDataAdapter(sql, con);
            con.Open();
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;

        }
        public bool SQL_Insert(SQLDTO data)
        {
            string sql = "INSERT INTO data_BKA (Bienso,RFID,Hinhbiensovao,Vitri,Biensovao,Giovao) VALUES(@Bienso,@RFID,@Hinhbiensovao,@Vitri,@Biensovao,@Giovao)";
            SqlConnection con = dc.getConnect();
            //try
            //{
            cmd = new SqlCommand(sql, con);
            con.Open();
           

            cmd.Parameters.Add("@RFID", SqlDbType.NVarChar).Value = data.RFID;
            cmd.Parameters.Add("@Bienso", SqlDbType.NVarChar).Value = data.Bienso;
            if (data.Hinhbiensovao == null)
            {
                cmd.Parameters.AddWithValue("@Hinhbiensovao", (object)DBNull.Value).SqlDbType = SqlDbType.Image;
            }
            else
            {
                Image Hinhbiensovao = (Image)data.Hinhbiensovao.Clone();
                cmd.Parameters.Add("@Hinhbiensovao", SqlDbType.Image).Value = imageToByteArray(Hinhbiensovao);
            }
            if (data.Biensovao == null)
                cmd.Parameters.AddWithValue("@Biensovao", (object)DBNull.Value).SqlDbType = SqlDbType.Image;
            else
            {
                Image Biensovao = (Image)data.Biensovao.Clone();
                cmd.Parameters.Add("@Biensovao", SqlDbType.Image).Value = imageToByteArray(Biensovao);
                cmd.Parameters.Add("@Vitri", SqlDbType.Int).Value = data.Vitri;

                cmd.Parameters.Add("@Giovao", SqlDbType.Date).Value = data.Giovao;
                cmd.ExecuteNonQuery();
                con.Close();
                //}
                //catch (Exception e)
                //{
                //    return false;
            }
            return true;

        }

        public bool SQL_Update(SQLDTO data)
        {
            string sql = "UPDATE data_BKA SET Hinhbiensora = @Hinhbiensora, Tiengui = @Tiengui WHERE ID = @ID";
            SqlConnection con = dc.getConnect();
            //try
            //{
                cmd = new SqlCommand(sql, con);
                con.Open();
                 cmd.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                if (data.Hinhbiensora == null)
                    cmd.Parameters.AddWithValue("@Hinhbiensora", (object)DBNull.Value).SqlDbType = SqlDbType.Image;
                //cmd.Parameters.Add("@Hinhbiensora", SqlDbType.Image).Value = new byte[] { };
                else
               // {
                  //  Image Hinhbiensora = (Image)data.Hinhbiensora.Clone();
                    cmd.Parameters.Add("@Hinhbiensora", SqlDbType.Image).Value = imageToByteArray(data.Hinhbiensora);
                    cmd.Parameters.Add("@Tiengui", SqlDbType.Int).Value = data.Tiengui;
                    cmd.Parameters.Add("@Giora", SqlDbType.DateTime).Value = data.Giora;
                    cmd.ExecuteNonQuery();
                    con.Close();
               // }
              //  }
           // catch (Exception e)
           // {
             //   return false;
          //  }
        
            return true;
        }
        public void SQL_Select_row(ref SQLDTO data)
        {
            string sql = "SELECT * FROM data_BKA WHERE RFID =  '" + data.RFID + "'";
            SqlConnection con = dc.getConnect();
            cmd = new SqlCommand(sql, con);
            con.Open();
          
           // cmd.Parameters.Add("@RFID", SqlDbType.NVarChar).Value = data.RFID;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data.RFID = dt.Rows[0][2].ToString();
            data.Bienso = dt.Rows[0][1].ToString();
            data.Vitri = int.Parse(dt.Rows[0][6].ToString());
           // data.Giovao = DateTime.Parse(dt.Rows[0][5].ToString());

            // MemoryStream ms = new MemoryStream((byte[])dt.Rows[0][4]);
            //data.Hinhbiensovao = Image.FromStream(ms);

            //ms = new MemoryStream((byte[])dt.Rows[0][8]);
            //data.Biensovao = Image.FromStream(ms);

            //data.Vitri = int.Parse(dt.Rows[0][5].ToString());
            //data.Maxe = int.Parse(dt.Rows[0][10].ToString());
        }
        public bool SQL_Check(ref SQLDTO data)
        {
            string sql = @"IF EXISTS(SELECT * FROM data_BKA
                       WHERE RFID = '" + data.RFID + "') SELECT 1 ELSE SELECT 0";
            //string sql = "SELECT COUNT(*) FROM data_BKParking WHERE RFID = " + data.RFID + "";
            SqlConnection con = dc.getConnect();
            //try
            //{
            cmd = new SqlCommand(sql, con);
            con.Open();
            //DataTable dt = new DataTable();
            //da.Fill(dt);
           
            cmd.Parameters.AddWithValue("@RFID", data.RFID);
          

            int x = Convert.ToInt32(cmd.ExecuteScalar());
            if (x == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
            // SqlDataReader reader = cmd.ExecuteReader();
            // int RFIDExist = (int)cmd.ExecuteScalar();
            //while (reader.Read()) {
            // if (reader.HasRows)
            // {

            //    if ()
            //   {
            //      
            //       return true;
            //   }
            //   else
            //   {
            //       return false;
            //   }

            //  reader.Dispose();
            //cmd.Parameters.Add("@RFID", SqlDbType.NVarChar, 50).Value = data.RFID;
            // cmd.Parameters.Add("@exists", SqlDbType.Int).Direction = ParameterDirection.Output;
            // cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
            // cmd.Parameters.Add("@Biensoxe", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Add("@Bienso", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
            data.Bienso = cmd.Parameters["@Bienso"].Value.ToString().Replace(" ", "");
            // data.Bienso = cmd.Parameters["@Biensoxe"].Value.ToString().Replace(" ", "");
            //data.Giovao = DateTime.Parse(cmd.Parameters["@Giovao"].Value.ToString());
            //data.ID = Convert.ToInt32(cmd.Parameters["@ID"].Value);
            //return Convert.ToInt32(cmd.Parameters["@exists"].Value);
            //reader.Close();
            con.Close();
            }
        public string[] SQL_Select_park()
        {
           string[] bienso = { "0", "0", "0", "0", "0", "0"};
            string sql = "SELECT * FROM data_BKA";
            //string sql = "SELECT COUNT(*) FROM data_BKParking WHERE RFID = " + data.RFID + "";
            SqlConnection con = dc.getConnect();
            //try
            //{
            cmd = new SqlCommand(sql, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int lenght = dt.Rows.Count;
            for (int i = 1; i < lenght; i++)
            {
                bienso[int.Parse(dt.Rows[i][6].ToString()) - 1] = dt.Rows[i][1].ToString();
            }
                return bienso;
        }


        public bool SQL_Select_row2(ref SQLDTO data)
        {
            string sql = "select ID from data_BKA where Vitri = " + data.Vitri + " AND Tiengui = 0";
            SqlConnection con = dc.getConnect();
            cmd = new SqlCommand(sql, con);
            con.Open();
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    data.ID = reader.GetInt32(0);
                }
                else
                {
                    return false;
                }
                return true;
            }
        }
   } 
}

   

    
