using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Auto_parking.SQL
{
    public class SQLDTO
    {
        public int ID { set; get; }
        public string Bienso { get; set; }
        public string RFID { get; set; }
        public Image Hinhbiensovao { get; set; }
        public DateTime Giovao { get; set; }
        public Image Hinhbiensora { get; set; }
        public int Vitri { get; set; }
        public DateTime Giora { get; set; }
        public int Tiengui { get; set; }
        public Image Biensovao { get; set; }
    }
}
