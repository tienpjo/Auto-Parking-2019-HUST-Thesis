using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using excel = Microsoft.Office.Interop.Excel;
namespace Auto_parking
{
    public partial class Database : Form
    {
        public Database()
        {
            InitializeComponent();


        }

        private void Database_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;

            //xlApp = new Microsoft.Office.Interop.Excel.Application();
            //xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            ////add data 
            //int StartCol = 1;`enter code here`
            //    int StartRow = 1;
            //int j = 0, i = 0;

            ////Write Headers
            //for (j = 0; j < gridviewID.Columns.Count; j++)
            //{
            //    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[StartRow, StartCol + j];
            //    myRange.Value2 = gridviewID.Columns[j].HeaderText;
            //}

            //StartRow++;

            ////Write datagridview content
            //for (i = 0; i < gridviewID.Rows.Count; i++)
            //{
            //    for (j = 0; j < gridviewID.Columns.Count; j++)
            //    {
            //        try
            //        {
            //            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[StartRow + i, StartCol + j];
            //            myRange.Value2 = gridviewID[j, i].Value == null ? "" : gridviewID[j, i].Value;
            //        }
            //        catch
            //        {
            //            ;
            //        }
            //    }
            //}

            //Microsoft.Office.Interop.Excel.Range chartRange;

            //Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            //Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
            //Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;

            //chartRange = xlWorkSheet.get_Range("A1", "B" + gridviewID.Rows.Count);
            //chartPage.SetSourceData(chartRange, misValue);
            //chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            //xlApp.Visible = true;

        }
        //public void Export(System.Data.DataTable dt, string sheetName, string title)
        //{

        //    //Tạo các đối tượng Excel

        //    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

        //    Microsoft.Office.Interop.Excel.Workbooks oBooks;

        //    Microsoft.Office.Interop.Excel.Sheets oSheets;

        //    Microsoft.Office.Interop.Excel.Workbook oBook;

        //    Microsoft.Office.Interop.Excel.Worksheet oSheet;

        //    //Tạo mới một Excel WorkBook 

        //    oExcel.Visible = true;

        //    oExcel.DisplayAlerts = false;

        //    oExcel.Application.SheetsInNewWorkbook = 1;

        //    oBooks = oExcel.Workbooks;

        //    oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));

        //    oSheets = oBook.Worksheets;

        //    oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

        //    oSheet.Name = sheetName;

        //    // Tạo phần đầu nếu muốn

        //    Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "C1");

        //    head.MergeCells = true;

        //    head.Value2 = title;

        //    head.Font.Bold = true;

        //    head.Font.Name = "Tahoma";

        //    head.Font.Size = "18";

        //    head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //    // Tạo tiêu đề cột 

        //    Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3", "A3");

        //    cl1.Value2 = "ID";

        //    cl1.ColumnWidth = 13.5;





        //    Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3", "B3");

        //    cl2.Value2 = "Bien so";

        //    cl2.ColumnWidth = 25.0;

        //    Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C3", "C3");

        //    cl3.Value2 = "RFID";

        //    cl3.ColumnWidth = 40.0;



        //    Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D3", "D3");
        //    cl4.Value2 = "Hinhbiensovao";

        //    cl4.ColumnWidth = 20.0;

        //    Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E3", "E3");
        //    cl5.Value2 = "Giovao";

        //    cl5.ColumnWidth = 20.0;
        //    Microsoft.Office.Interop.Excel.Range cl6 = oSheet.get_Range("F3", "F3");
        //    cl6.Value2 = "Hinhbiensora";

        //    cl6.ColumnWidth = 20.0;
        //    Microsoft.Office.Interop.Excel.Range cl7 = oSheet.get_Range("G3", "H3");
        //    cl7.Value2 = "Giora";

        //    cl7.ColumnWidth = 20.0;


        //    Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "C3");

        //    rowHead.Font.Bold = true;

        //    // Kẻ viền

        //    rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

        //    // Thiết lập màu nền

        //    rowHead.Interior.ColorIndex = 15;

        //    rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //    // Tạo mẳng đối tượng để lưu dữ toàn bồ dữ liệu trong DataTable,

        //    // vì dữ liệu được được gán vào các Cell trong Excel phải thông qua object thuần.

        //    object[,] arr = new object[dt.Rows.Count, dt.Columns.Count];

        //    //Chuyển dữ liệu từ DataTable vào mảng đối tượng

        //    for (int r = 0; r < dt.Rows.Count; r++)

        //    {

        //        DataRow dr = dt.Rows[r];

        //        for (int c = 0; c < dt.Columns.Count; c++)

        //        {
        //            arr[r, c] = dr[c];
        //        }
        //    }

        //    //Thiết lập vùng điền dữ liệu

        //    int rowStart = 4;

        //    int columnStart = 1;

        //    int rowEnd = rowStart + dt.Rows.Count - 1;

        //    int columnEnd = dt.Columns.Count;

        //    // Ô bắt đầu điền dữ liệu

        //    Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];

        //    // Ô kết thúc điền dữ liệu

        //    Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];

        //    // Lấy về vùng điền dữ liệu

        //    Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

        //    //Điền dữ liệu vào vùng đã thiết lập

        //    range.Value2 = arr;

        //    // Kẻ viền

        //    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

        //    // Căn giữa cột STT

        //    Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnStart];

        //    Microsoft.Office.Interop.Excel.Range c4 = oSheet.get_Range(c1, c3);

        //    oSheet.get_Range(c3, c4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //}

    }
    }


