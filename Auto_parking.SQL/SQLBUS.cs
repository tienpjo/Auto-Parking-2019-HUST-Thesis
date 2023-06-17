using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Auto_parking.SQL
{
  public class SQLBUS
    {
        SQLDAO db;

        public SQLBUS()
        {
            db = new SQLDAO();
        }
        public DataTable get_All_Data()
        {
            return db.get_All_Data();
        }
        public bool SQL_Insert(SQLDTO data)
        {
            return db.SQL_Insert(data);

        }

        public void SQL_Select_row(ref SQLDTO data)
        {
            try
            {
                db.SQL_Select_row(ref data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi ở SQL_Select_row! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //throw e;
            }
        }
        public bool SQL_Update(SQLDTO data)
        {
            return db.SQL_Update(data);
        }
        public bool SQL_Check(ref SQLDTO data)
        {
            return db.SQL_Check(ref data);
        }
        public bool SQL_Select_row2(ref SQLDTO data)
        {

            return SQL_Select_row2(ref data);
        }

        public string[] SQL_Select_park()
        {
            try
            {
                return db.SQL_Select_park();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi ở SQL_Select_android! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw e;
            }
            return null;
        }
        //public string[] SQL_Select_park(out string[] bienso)
        //{
        //    bienso = new string[] { "0", "0", "0", "0", "0", "0" };

        //    try
        //    {
        //        string[] biensoso = db.SQL_Select_park(out bienso);
        //        return biensoso;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Xảy ra lỗi ở SQL_Select_park! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //throw e;
        //    }
        //    return null;
        //}

        //public DataTable SQL_Select_all()
        //{
        //    try
        //    {
        //        return db.SQL_Select_all();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Xảy ra lỗi ở SQL_Select_all! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //        //throw e;
        //    }
        //}
    }
}
