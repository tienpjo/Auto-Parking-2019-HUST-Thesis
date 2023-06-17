using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using System.IO;
using System.IO.Ports;
using tesseract;
using System.Collections;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;
using Auto_parking.SQL;
namespace Auto_parking
{
    public partial class MainForm : Form
    {

        SQLBUS bus;
        
        public MainForm()
        {
            InitializeComponent();
            bus = new SQLBUS();
            serialPort3.DataReceived += new SerialDataReceivedEventHandler(serialPort3_DataReceived);

            string[] BaudRate = { "9600" };

            comboBox1.Items.AddRange(BaudRate);

        }

        //public SerialPort COM;

        #region định nghĩa
        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        Image Plate_Draw;
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        public TesseractProcessor full_tesseract = null;
        public TesseractProcessor ch_tesseract = null;
        public TesseractProcessor num_tesseract = null;
        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";

        //int current = 0;
        Capture capture = null;
        #endregion


        #region di chuyển
        bool mouseDown = false;
        Point lastLocation;
        private void button_Leave(object sender, EventArgs e)
        {
            Button bsen = (Button)sender;
            bsen.ForeColor = Color.Black;
        }

        private void button_Enter(object sender, EventArgs e)
        {
            Button bsen = (Button)sender;
            bsen.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mouseDown == false && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDown = true;
                lastLocation = e.Location;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //contextMenuStrip1.Show(this.DesktopLocation.X + e.X, this.DesktopLocation.Y + e.Y);	
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.SetDesktopLocation(this.DesktopLocation.X - lastLocation.X + e.X, this.DesktopLocation.Y - lastLocation.Y + e.Y);
                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void panel1_MouseHover(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(15, 15, 15);
        }
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(64, 64, 64);
        }
        #endregion
        // Notify thongbao;
        ImageForm IF;
        //int x = 3;
        //s//tring InputData = String.Empty; // Khai báo string buff dùng cho hiển thị dữ liệu sau này.

        private void MainForm_Load(object sender, EventArgs e)
        {

            capture = new Emgu.CV.Capture();
            capture = new Capture();

            timer1.Enabled = true;
            //  radioButton1.Checked = true;

            timer2.Enabled = true;
            IF = new ImageForm();

            full_tesseract = new TesseractProcessor();
            bool succeed = full_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            full_tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTVXY1234567890").ToString();

            ch_tesseract = new TesseractProcessor();
            succeed = ch_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            ch_tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTUVXY").ToString();

            num_tesseract = new TesseractProcessor();
            succeed = num_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            num_tesseract.SetVariable("tessedit_char_whitelist", "1234567890").ToString();


            m_path = System.Environment.CurrentDirectory + "\\";
            //comboBox2.DataSource = SerialPort.GetPortNames();
            //string[] ports = SerialPort.GetPortNames();
            //comboBox1.Items.Clear();
            //load cong COM
            //if (ports.Length != 0)
            //{
            //    for (int j = 0; j < ports.Length; j++)
            //    {
            //        comboBox1.Items.Add(ports[j]);
            //    }
            //    comboBox1.Text = ports[0];
            //}
            //for (int i = 0; i < box.Length; i++)
            //{
            //    box[i] = new PictureBox();
            //}
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //if (IF.Visible == false)
            //{
            //     IF.Show();
            //  }
            ///   else
            //  {
            //     IF.Hide();
            // }
            quanly park = new quanly();
            park.Show();

        }
        bool success = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (success == true)
            {
                success = false;
                new Thread(() =>
                {
                    try
                    {
                        capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640);
                        capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480);
                        Image<Bgr, byte> cap = capture.QueryFrame();
                        if (cap != null)
                        {
                            MethodInvoker mi = delegate
                            {
                                try
                                {
                                    Bitmap bmp = cap.ToBitmap();
                                    pictureBox_WC.Image = bmp;
                                    IF.pictureBox4.Image = bmp;
                                    pictureBox_WC.Update();
                                    IF.pictureBox4.Update();
                                }
                                catch (Exception ex)
                                { }
                            };
                            if (InvokeRequired)
                                Invoke(mi);
                        }
                    }
                    catch (Exception) { }
                    success = true;
                }).Start();

            }


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime day = DateTime.Now;
            textBox7.Text = day.ToString();
        }
        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();
            FileStream fs = new FileStream(urlImage, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            Bitmap image = new Bitmap(img);
            //pictureBox2.Image = image;
            IF.pictureBox2.Image = image;
            fs.Close();

            FindLicensePlate4(image, out Plate_Draw);

        }
        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private string Ocr(Bitmap image_s, bool isFull, bool isNum = false)
        {
            string temp = "";
            Image<Gray, byte> src = new Image<Gray, byte>(image_s);
            double ratio = 1;
            while (true)
            {
                ratio = (double)CvInvoke.cvCountNonZero(src) / (src.Width * src.Height);
                if (ratio > 0.5) break;
                src = src.Dilate(2);
            }
            Bitmap image = src.ToBitmap();

            TesseractProcessor ocr;
            if (isFull)
                ocr = full_tesseract;
            else if (isNum)
                ocr = num_tesseract;
            else
                ocr = ch_tesseract;

            int cou = 0;
            ocr.Clear();
            ocr.ClearAdaptiveClassifier();
            temp = ocr.Apply(image);
            while (temp.Length > 3)
            {
                Image<Gray, byte> temp2 = new Image<Gray, byte>(image);
                temp2 = temp2.Erode(2);
                image = temp2.ToBitmap();
                ocr.Clear();
                ocr.ClearAdaptiveClassifier();
                temp = ocr.Apply(image);
                cou++;
                if (cou > 10)
                {
                    temp = "";
                    break;
                }
            }
            return temp;

        }

        public void FindLicensePlate2(Bitmap image)
        {
            if (image == null)
                return;
            Bitmap src;
            Image dst = image;
            Image<Bgr, byte> frame_b = null;
            Image<Bgr, byte> plate_b = null;
            double sum_b = 0;
            for (float i = -45; i <= 45; i = i + 5)
            {
                src = RotateImage(dst, i);
                PlateImagesList.Clear();
                Image<Bgr, byte> frame = new Image<Bgr, byte>(src);
                using (Image<Gray, byte> grayframe = new Image<Gray, byte>(src))
                {


                    var faces =
                           grayframe.DetectHaarCascade(
                                   new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml"), 1.1, 8,
                                   HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                   new Size(0, 0)
                                   )[0];
                    foreach (var face in faces)
                    {
                        Image<Bgr, byte> tmp = frame.Copy();
                        tmp.ROI = face.rect;

                        frame.Draw(face.rect, new Bgr(Color.Blue), 2);

                        PlateImagesList.Add(tmp.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true));


                    }

                }
                if (PlateImagesList.Count != 0)
                {
                    Image<Gray, byte> gr = new Image<Gray, byte>(PlateImagesList[0].Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR).ToBitmap());
                    Gray cannyThreshold = new Gray(gr.GetAverage().Intensity);
                    Gray cannyThresholdLinking = new Gray(gr.GetAverage().Intensity);
                    Image<Gray, byte> cannyEdges = gr.Canny(cannyThreshold, cannyThresholdLinking);

                    double sum = 0;
                    for (int j = 0; j < cannyEdges.Height - 1; j++)
                    {
                        for (int k = 0; k < cannyEdges.Width - 1; k++)
                        {
                            if (j < 20 || j > 180 || k < 20 || k > 180)
                            {
                                sum += cannyEdges.Data[j, k, 0]; // tính tổng các điểm trắng ở viền ngoài
                            }
                            //else
                            //{
                            //    cannyEdges.Data[j, k, 0] = 0;
                            //}
                        }
                    }
                    //pictureBox4.Image = cannyEdges.ToBitmap();
                    //pictureBox4.Update();
                    if (sum_b == 0 || sum > sum_b)
                    {
                        frame_b = frame.Clone();
                        plate_b = PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR).Clone();
                        sum_b = sum;
                    }
                }

            }
            if (plate_b != null)
            {
                PlateImagesList.Add(plate_b);
                pictureBox_WC.Image = frame_b.ToBitmap();
                pictureBox_WC.Update();
            }

        }
        public void FindLicensePlate(Bitmap image, out Image plateDraw)
        {
            plateDraw = null;
            Image<Bgr, byte> frame = new Image<Bgr, byte>(image);
            bool isface = false;
            using (Image<Gray, byte> grayframe = new Image<Gray, byte>(image))
            {


                var faces =
                       grayframe.DetectHaarCascade(
                               new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml"), 1.1, 8,
                               HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                               new Size(0, 0)
                               )[0];
                foreach (var face in faces)
                {
                    Image<Bgr, byte> tmp = frame.Copy();
                    tmp.ROI = face.rect;

                    frame.Draw(face.rect, new Bgr(Color.Blue), 2);

                    PlateImagesList.Add(tmp);

                    isface = true;
                }
                if (isface)
                {
                    Image<Bgr, byte> showimg = frame.Clone();
                    plateDraw = (Image)showimg.ToBitmap();
                    //showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                    //pictureBox1.Image = showimg.ToBitmap();
                    IF.pictureBox2.Image = showimg.ToBitmap();
                    if (PlateImagesList.Count > 1)
                    {
                        for (int i = 1; i < PlateImagesList.Count; i++)
                        {
                            if (PlateImagesList[0].Width < PlateImagesList[i].Width)
                            {
                                PlateImagesList[0] = PlateImagesList[i];
                            }
                        }
                    }
                    PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                }


            }
        }
        public void FindLicensePlate4(Bitmap image, out Image plateDraw)
        {
            plateDraw = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            //pictureBox2.Image = new Image<Gray, byte>(image).ToBitmap();
            Image dst = image;
            HaarCascade haar = new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml");
            for (float i = 0; i <= 20; i = i + 3)
            {
                for (float s = -1; s <= 1 && s + i != 1; s += 2)
                {
                    src = RotateImage(dst, i * s);
                    PlateImagesList.Clear();
                    frame = new Image<Bgr, byte>(src);
                    using (Image<Gray, byte> grayframe = new Image<Gray, byte>(src))
                    {
                        var faces =
                       grayframe.DetectHaarCascade(haar, 1.1, 8, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(0, 0))[0];
                        foreach (var face in faces)
                        {
                            Image<Bgr, byte> tmp = frame.Copy();
                            tmp.ROI = face.rect;

                            frame.Draw(face.rect, new Bgr(Color.Blue), 2);

                            PlateImagesList.Add(tmp);

                            isface = true;
                        }
                        if (isface)
                        {
                            Image<Bgr, byte> showimg = frame.Clone();
                            plateDraw = (Image)showimg.ToBitmap();
                            //showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                            //pictureBox1.Image = showimg.ToBitmap();
                            IF.pictureBox2.Image = showimg.ToBitmap();
                            if (PlateImagesList.Count > 1)
                            {
                                for (int k = 1; k < PlateImagesList.Count; k++)
                                {
                                    if (PlateImagesList[0].Width < PlateImagesList[k].Width)
                                    {
                                        PlateImagesList[0] = PlateImagesList[k];
                                    }
                                }
                            }
                            PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                            return;
                        }


                    }
                }
            }


        }
        public void FindLicensePlate3(Bitmap image)
        {
            if (image == null)
                return;
            Bitmap src;
            Image dst = image;
            Image<Bgr, byte> frame_b = null;
            Image<Bgr, byte> plate_b = null;
            double sum_b = 1000;
            HaarCascade haar = new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml");
            for (float i = 0; i <= 35; i = i + 3)
            {
                for (float s = -1; s <= 1 && s + i != 1; s += 2)
                {
                    src = RotateImage(dst, i * s);
                    PlateImagesList.Clear();
                    Image<Bgr, byte> frame = new Image<Bgr, byte>(src);
                    using (Image<Gray, byte> grayframe = new Image<Gray, byte>(src))
                    {


                        var faces = grayframe.DetectHaarCascade(haar, 1.1, 8, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(0, 0))[0];
                        foreach (var face in faces)
                        {
                            Image<Bgr, byte> tmp = frame.Copy();
                            tmp.ROI = face.rect;

                            frame.Draw(face.rect, new Bgr(Color.Blue), 2);

                            PlateImagesList.Add(tmp.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));

                            //imageBox1.Image = tmp;
                            //imageBox1.Update();

                        }
                        //Image<Bgr, Byte> showimg = new Image<Bgr, Byte>(image.Size);
                        //showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                        //pictureBox1.Image = grayframe.ToBitmap();
                    }
                    if (PlateImagesList.Count != 0)
                    {
                        Image<Gray, byte> src2 = new Image<Gray, byte>(PlateImagesList[0].ToBitmap());
                        double thr = src2.GetAverage().Intensity;

                        double min = 0, max = 255;
                        if (thr - 50 > 0)
                        {
                            min = thr - 50;
                        }
                        if (thr + 50 < 255)
                        {
                            max = thr + 50;
                        }
                        for (double value = min; value <= max; value += 5)
                        {
                            src2 = new Image<Gray, byte>(PlateImagesList[0].ToBitmap());
                            int c = 0;
                            List<Rectangle> listR = new List<Rectangle>();
                            using (MemStorage storage = new MemStorage())
                            {
                                src2 = src2.ThresholdBinary(new Gray(value), new Gray(255));
                                Contour<Point> contours = src2.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                                while (contours != null)
                                {

                                    Rectangle rect = contours.BoundingRectangle;
                                    double ratio = (double)rect.Width / rect.Height;
                                    if (rect.Width > 20 && rect.Width < 150
                                        && rect.Height > 80 && rect.Height < 180
                                        && ratio > 0.2 && ratio < 1.1)
                                    {
                                        c++;
                                        listR.Add(contours.BoundingRectangle);
                                    }
                                    contours = contours.HNext;
                                }
                            }
                            double sum = 1000;
                            if (c >= 2)
                            {
                                for (int u = 0; u < c; u++)
                                {
                                    for (int v = u + 1; v < c; v++)
                                    {
                                        if (Math.Abs(listR[u].Y - listR[v].Y) < sum)
                                        {

                                            sum = Math.Abs(listR[u].Y - listR[v].Y);
                                            if (sum < 4)
                                            {
                                                PlateImagesList.Add(PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR).Clone());
                                                pictureBox_XeRA.Image = frame.ToBitmap();
                                                pictureBox_XeRA.Update();
                                                return;
                                            }
                                        }
                                    }
                                }

                            }

                            if (sum < sum_b)
                            {
                                frame_b = frame.Clone();
                                plate_b = PlateImagesList[0].Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR).Clone();
                                sum_b = sum;
                            }
                        }
                    }
                }


            }
            if (plate_b != null)
            {
                PlateImagesList.Add(plate_b);
                pictureBox_XeRA.Image = frame_b.ToBitmap();
                pictureBox_XeRA.Update();
            }

        }

        private void Reconize(string link, out Image hinhbienso, out string bienso, out string bienso_text)
        {
            for (int i = 0; i < box.Length; i++)
            {
                this.Controls.Remove(box[i]);
            }

            hinhbienso = null;
            bienso = "";
            bienso_text = "";
            ProcessImage(link);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                FindContours con = new FindContours();
                Bitmap color;
                int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);  // find contour
                                                                                                                  //int z = con.count;
                                                                                                                  //    pictureBox_BiensoVAO.Image = color;
                IF.pictureBox1.Image = color;
                hinhbienso = Plate_Draw;
                // pictureBox_BiensoRA.Image = grayframe;
                IF.pictureBox3.Image = grayframe;
                //textBox2.Text = c.ToString();
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                //dst = dst.Dilate(2);
                //dst = dst.Erode(3);
                grayframe = dst.ToBitmap();
                //pictureBox2.Image = grayframe.Clone(listRect[2], grayframe.PixelFormat);
                string zz = "";

                // lọc và sắp xếp số
                List<Bitmap> bmp = new List<Bitmap>();
                List<int> erode = new List<int>();
                List<Rectangle> up = new List<Rectangle>();
                List<Rectangle> dow = new List<Rectangle>();
                int up_y = 0, dow_y = 0;
                bool flag_up = false;

                int di = 0;

                if (listRect == null) return;

                for (int i = 0; i < listRect.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(listRect[i], grayframe.PixelFormat);
                    int cou = 0;
                    full_tesseract.Clear();
                    full_tesseract.ClearAdaptiveClassifier();
                    string temp = full_tesseract.Apply(ch);
                    while (temp.Length > 3)
                    {
                        Image<Gray, byte> temp2 = new Image<Gray, byte>(ch);
                        temp2 = temp2.Erode(2);
                        ch = temp2.ToBitmap();
                        full_tesseract.Clear();
                        full_tesseract.ClearAdaptiveClassifier();
                        temp = full_tesseract.Apply(ch);
                        cou++;
                        if (cou > 10)
                        {
                            listRect.RemoveAt(i);
                            i--;
                            di = 0;
                            break;
                        }
                        di = cou;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    for (int j = i; j < listRect.Count; j++)
                    {
                        if (listRect[i].Y > listRect[j].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[j].Y;
                            dow_y = listRect[i].Y;
                            break;
                        }
                        else if (listRect[j].Y > listRect[i].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[i].Y;
                            dow_y = listRect[j].Y;
                            break;
                        }
                        if (flag_up == true) break;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    if (listRect[i].Y < up_y + 50 && listRect[i].Y > up_y - 50)
                    {
                        up.Add(listRect[i]);
                    }
                    else if (listRect[i].Y < dow_y + 50 && listRect[i].Y > dow_y - 50)
                    {
                        dow.Add(listRect[i]);
                    }
                }

                if (flag_up == false) dow = listRect;

                for (int i = 0; i < up.Count; i++)
                {
                    for (int j = i; j < up.Count; j++)
                    {
                        if (up[i].X > up[j].X)
                        {
                            Rectangle w = up[i];
                            up[i] = up[j];
                            up[j] = w;
                        }
                    }
                }
                for (int i = 0; i < dow.Count; i++)
                {
                    for (int j = i; j < dow.Count; j++)
                    {
                        if (dow[i].X > dow[j].X)
                        {
                            Rectangle w = dow[i];
                            dow[i] = dow[j];
                            dow[j] = w;
                        }
                    }
                }

                int x = 12;
                int c_x = 0;

                for (int i = 0; i < up.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(up[i], grayframe.PixelFormat);
                    Bitmap o = ch;
                    //ch = con.Erodetion(ch);
                    string temp;
                    if (i < 2)
                    {
                        temp = Ocr(ch, false, true); // nhan dien so
                    }
                    else
                    {
                        temp = Ocr(ch, false, false);// nhan dien chu
                    }

                    zz += temp;
                    /**edit by tien ngay 18/07 **/
                    //box[i].Location = new Point(x + i * 50, 290);
                    //box[i].Size = new Size(50, 100);
                    //box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    //box[i].Image = ch;
                    //box[i].Update();
                    //this.Controls.Add(box[i]);
                    //IF.Controls.Add(box[i]);
                    //c_x++;
                }
                zz += "\r\n";
                for (int i = 0; i < dow.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(dow[i], grayframe.PixelFormat);
                    //ch = con.Erodetion(ch);
                    string temp = Ocr(ch, false, true); // nhan dien so
                    zz += temp;
                    /////////////////// edit by tien 18-7////////////////
                    /*box[i + c_x].Location = new Point(x + i * 50, 390);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;
                    box[i + c_x].Update();
                    this.Controls.Add(box[i + c_x]);
                    IF.Controls.Add(box[i + c_x]);*/
                    /////////////////// edit by tien 18-7////////////////
                }
                bienso = zz.Replace("\n", "");
                bienso = bienso.Replace("\r", "");
                IF.textBox6.Text = zz;
                bienso_text = zz;

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                //while (true) ;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
                dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                string startupPath = dlg.FileName;

                Image temp1;
                string temp2, temp3;
                Reconize(startupPath, out temp1, out temp2, out temp3);
                textBox2.Text = DateTime.Now.ToString();
                Image imge_bienso = (Image)PlateImagesList[0].ToBitmap();
                pictureBox_XeVAO.Image = temp1;
                pictureBox_BiensoVAO.Image = imge_bienso;
                if (temp3 == "")
                    text_BiensoVAO.Text = "Cannot recognize license plate !";
                else
                    text_BiensoVAO.Text = temp3;

                //    SQLDTO data = new SQLDTO();

                //    data.Bienso = text_BiensoVAO.Text;

                //    data.Hinhbiensovao = pictureBox_XeVAO.Image;
                // data.Giovao = textBox2.Text;
                //    // bus.SQL_Insert(data);
                //    if (bus.SQL_Insert(data))
                //        MessageBox.Show("Insert thành công", "Success", MessageBoxButtons.OK);

                //    else
                //    {
                //        MessageBox.Show("Đã có lỗi, Không thêm được vào CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Bạn chưa chọn chế độ nhân diện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                timer1.Enabled = false;
                pictureBox_XeRA.Image = null;
                IF.pictureBox2.Image = null;
                capture.QueryFrame().Save("aa.bmp");
                FileStream fs = new FileStream(m_path + "aa.bmp", FileMode.Open, FileAccess.Read);
                Image temp = Image.FromStream(fs);
                fs.Close();
                pictureBox_XeRA.Image = temp;
                IF.pictureBox2.Image = temp;
                pictureBox_XeRA.Update();
                IF.pictureBox2.Update();
                Image temp1;
                string temp2, temp3;
                Reconize(m_path + "aa.bmp", out temp1, out temp2, out temp3);
                pictureBox_XeVAO.Image = temp1;
                if (temp3 == "")
                    text_BiensoVAO.Text = "Cannot recognize license plate !";
                else
                    text_BiensoVAO.Text = temp3;
                timer1.Enabled = true;
            }

        }
        void CF_RequestChange()
        {
            if (radioButton1.Checked == true)
            {
                try
                {


                    capture = new Capture(0); //create a camera captue
                    capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640);
                    capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480);
                    //CvInvoke.cvSetCaptureProperty(capture.Ptr, CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
                    //CvInvoke.cvSetCaptureProperty(capture.Ptr, CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 960);
                    timer1.Enabled = true;
                }
                catch (Exception ex)
                {
                    //AutoClosingMessageBox.Show("Không tìm thấy camera! \n\r error: " + ex.ToString(), "Thông Báo", 5000);
                    MessageBox.Show("Không tìm thấy camera! \n\r error: " + ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Enabled = false;
                    //  CF.radioButton2.Checked = true;
                }
            }
            else
            {
                timer1.Enabled = false;
                success = true;
                //while (!success);
                if (capture != null)
                {
                    capture.Dispose();
                }
                capture = null;
                pictureBox_WC.Image = null;
                pictureBox_WC.Invalidate();
                pictureBox_WC.Update();
                //IF.pictureBox4.Image = null;
            }
        }

        string IDRFID;
        string x;
        int[] a = new int[10];
        int i;
        int place;
        private void serialPort3_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Console.Beep(3000, 300);
                Thread.Sleep(100);
                byte[] data = new byte[11];
                serialPort3.Read(data, 0, 11);
                //Thread.Sleep(2000);
                serialPort3.Close();
                byte[] data2 = new byte[11];
                data2 = data;
                string s = System.Text.Encoding.ASCII.GetString(data2, 0, 11);
                //BitConverter.ToString(data2);
                //System.Text.Encoding.ASCII.GetString(data2, 0, 7);
                string[] a = s.Split('/');
                string z = a[0];
                    x = a[1];
                byte[] zzbytes = new byte[7];
                zzbytes = System.Text.Encoding.ASCII.GetBytes(z);
                string zz =BitConverter.ToString(zzbytes);
                //try
                //{
                //    x = int.Parse(a[1].ToString());
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Xảy ra lỗi ở day! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                MethodInvoker mi = delegate
                {
                    if (radioButton1.Checked == true)
                    {
                        IDRFID = zz;
                        textBox8.Text = zz;
                        textBox9.Text = x;
                        EVENT();
                    }
                    else
                    {
                        button3.PerformClick();
                    }
                };
                if (InvokeRequired)
                    this.Invoke(mi);
                //Display(z);

                serialPort3.Open();
            }
            catch (Exception ex)
            {

            }

        }
        //private delegate void LineReceivedEvent(string data);
        //private void LineReceived(string data)
        //{

        //    // textBox2.Text = data.ToString();
        //    string[] data2 = data.Split('/');
        //    // for (int i = 0; i < data.Length + 1; i++)
        //    //  {
        //    string z = data2[0];
        //    x = int.Parse(data2[1]);
        //    //}
        //    MethodInvoker mi = delegate
        //{
        //    if (radioButton1.Checked == true)
        //    {
        //        IDRFID = z;
        //        EVENT();
        //    }
        //    else
        //    {
        //        button3.PerformClick();
        //    }
        //};
        //    if (InvokeRequired)
        //        this.Invoke(mi);
        //    // string zz = BitConverter.ToString(z);
        //    serialPort3.Open();
        //    // progressBar1.Value = int.Parse(line);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Xảy ra lỗi ở day! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //throw e;
        //}
        ////    //  serialPort3.Close();
        //    Thread.Sleep(100);


        //    data2 = data.Split('/');
        //    string z = data2[0];
        //try
        //{
        //    x = int.Parse(data2[1]);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Xảy ra lỗi ở day! \n\r error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //   // throw ex;
        //}
        //MethodInvoker mi = delegate
        //{
        //    if (radioButton1.Checked == true)
        //    {
        //        IDRFID = z;
        //        EVENT();
        //    }
        //    else
        //    {
        //        button3.PerformClick();
        //    }
        //};
        //if (InvokeRequired)
        //    this.Invoke(mi);
        ////Display(z);
        ////serialPort3.Open();
        //public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{

        //}
        private void Process_VAO(ref SQLDTO data)
        {
            SQLBUS bus = new SQLBUS();
            // di chuyen de vi tri Reset
            Image Hinhxe_bienso = null;
            string biensoxe_ngan = "", biensoxe_dai = "";
            int times_totry = 0;
            string path = "";
            // bool formclosed = false;
            if (radioButton1.Checked == true)
            {

                Image img = null;
                while (img == null)
                    try
                    {

                        if (pictureBox_WC.Image != null)
                        {
                            MethodInvoker save_img = delegate
                                    {
                                        img = pictureBox_WC.Image;
                                        DateTime time_now = DateTime.Now;
                                        string ti = time_now.ToString() + " " + time_now.Millisecond.ToString();
                                        ti = ti.Replace('/', '-');
                                        ti = ti.Replace(':', '-');
                                        path = Application.StartupPath + @"\data\" + ti + ".bmp";
                                        img.Save(path);
                                    };
                            if (InvokeRequired)
                                Invoke(save_img);
                        }
                        else
                        {
                            MessageBox.Show(" Kiểm tra lại camera", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // capture.QueryFrame().Save("aa.bmp");
                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                        Image temp = Image.FromStream(fs);
                        pictureBox_XeVAO.Image = temp;
                        fs.Close();


                        Reconize(path, out Hinhxe_bienso, out biensoxe_ngan, out biensoxe_dai);
                        if (biensoxe_ngan.Length != 8)
                        {
                            if (times_totry++ > 6)
                            {
                                MessageBox.Show("Chưa nhận dạng đúng được Biển số \n\r\n " + "số chữ: " + biensoxe_ngan.Length.ToString()
                                   , "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                img = null;
                            }
                            img = null;
                        }
                    }
                    catch (Exception) { img = null; }
                data.Hinhbiensovao = Hinhxe_bienso;
                // if (!formclosed)
                data.Hinhbiensovao = (Image)PlateImagesList[0].ToBitmap();

                data.Biensovao = Hinhxe_bienso;
                data.Bienso = biensoxe_ngan;

                Image imge_bienso = data.Hinhbiensovao;
                MethodInvoker mi = delegate
                {
                    pictureBox_XeVAO.Image = Plate_Draw;
                    pictureBox_BiensoVAO.Image = imge_bienso;
                    text_BiensoVAO.Text = biensoxe_dai;
                    textBox1.Text = IDRFID;
                };
                if (InvokeRequired)
                    this.Invoke(mi);
             //  string zz = x;
                int zzz = int.Parse(x.ToString());
               
                for (i = 1; zzz > 0; i++)
                {
                    a[i] = zzz % 2;
                    zzz = zzz / 2;
                }
                for (int i = 1; i < 10; i++)
                {
                    if (a[i] == 1)
                    {
                        data.Vitri = i;
                        place = data.Vitri;
                        break;
                    }
                }
                
                string vitri = data.Vitri.ToString();
                string rfid = data.RFID;
                MethodInvoker mi2 = delegate
                {
                    textBox2.Text = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss");
                    textBox5.Text = vitri;

                };
                if (InvokeRequired)
                    this.Invoke(mi2);
                //DateTime dtgiovao = DateTime.Parse(textBox2.ToString());
                data.Giovao = Convert.ToDateTime(textBox2.Text.ToString());
                SQLDTO data2 = data;
                bool s = false;

                MethodInvoker ins = delegate
                {
                    s = bus.SQL_Insert(data2);
                };
                if (InvokeRequired)
                    Invoke(ins);
                else
                    s = bus.SQL_Insert(data2);
                if (s)
                {
                    switch (place)
                    {
                        case 1:
                            serialPort3.Write("1");
                            serialPort3.Close();
                           
                            break;
                        case 2:
                            serialPort3.Write("2");
                            serialPort3.Close();
                            break;
                        case 3:
                            serialPort3.Write("3");
                            serialPort3.Close();
                            break;
                        case 4:
                            serialPort3.Write("4");
                            serialPort3.Close();
                            break;
                        case 5:
                            serialPort3.Write("5");
                            serialPort3.Close();
                            break;
                        //case 6:
                        //    serialPort3.Write("6");
                        //    serialPort3.Close();
                        //    break;
                            //sau do di chuyen toi vi tri xe theo vi tri tren
                    }
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm xe vào CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
               
                Thread.Sleep(45000);
                serialPort3.Open();
                //Thread.Sleep(50000);
                serialPort3.Write("9");
                serialPort3.Close();
                Thread.Sleep(3000);
                
                 serialPort3.Open();
            }
        }
        private void Xe_Ra(ref SQLDTO data)
        {
            try
            {
                bool anyway = false;
                SQLBUS bus = new SQLBUS();
                bus.SQL_Select_row(ref data);
                string bienso = data.Bienso, giovao = data.Giovao.ToString(), mathe = data.RFID, vitri = data.Vitri.ToString();
                MethodInvoker mi = delegate
                {
                //text_BiensoRa.Text = bienso;
                textBox3.Text = mathe;
                    textBox10.Text = vitri;
                };
                if (InvokeRequired)
                    this.Invoke(mi);
                //Thread.Sleep(500);
                //serialPort3.Open();                    
                switch (textBox10.Text)
                {
                    case "1":
                        serialPort3.Write("21");
                        serialPort3.Close();                   
                        break;
                    case "2":
                        serialPort3.Write("22");
                        serialPort3.Close();
                        break;
                    case "3":
                        serialPort3.Write("23");
                        serialPort3.Close();
                        break;
                    case "4":
                        serialPort3.Write("24");
                        serialPort3.Close();

                        break;
                    case "5":
                        serialPort3.Write("25");
                        serialPort3.Close();
                        break;
                    //case "6":
                    //    serialPort3.Write("26");
                    //    serialPort3.Close();
                    //    break;
                        //sau do di chuyen toi vi tri xe theo vi tri tren
                }
               
                //chup anh
                Thread.Sleep(20000);
                Image Hinhxe_bienso = null;
                string biensoxe_ngan = "";
                string biensoxe_dai = "";

                int time_totry = 0;
                string path = "";
                if (radioButton1.Checked == true)
                {
                    Image img = null;
                    while (img == null)
                        try
                        {
                            if (pictureBox_WC.Image != null)
                            {
                                MethodInvoker mi2 = delegate
                                {
                                    img = pictureBox_WC.Image;
                                    DateTime time_now = DateTime.Now;
                                    string ti = time_now.ToString() + " " + time_now.Millisecond.ToString();
                                    ti = ti.Replace('/', '-');
                                    ti = ti.Replace(':', '-');
                                    path = Application.StartupPath + @"\data\" + ti + ".bmp";
                                    img.Save(path);
                                };
                                if (InvokeRequired)
                                    this.Invoke(mi2);
                            }
                            else
                            {
                                MessageBox.Show("Kiểm tra lại camera", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                            Image temp = Image.FromStream(fs);
                            pictureBox_XeRA.Image = temp;
                            fs.Close();
                            Reconize(path, out Hinhxe_bienso, out biensoxe_ngan, out biensoxe_dai);
                            if (biensoxe_ngan.Length != 8 || data.Bienso != biensoxe_ngan)
                            {
                                if (time_totry++ > 2)
                                {
                                    DialogResult result = MessageBox.Show("Chưa nhận dạng được Biển số \n\r\n" + "số chữ:"
                                      + biensoxe_ngan.Length.ToString()
                                      + "\n\r\n Biến số vào:" + data.Bienso
                                      + "\n\r\n Biển số ra:" + biensoxe_ngan
                                      , "Thông báo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                                    if (result == DialogResult.Yes)
                                    {
                                        anyway = true;
                                        Hinhxe_bienso = temp;
                                        break;
                                    }
                                    else if (result == DialogResult.No)
                                    {
                                        img = null;
                                    }
                                }
                                img = null;
                            }
                        }
                        catch (Exception) { img = null; }
                }
                //  Image Bienso_Ra = (Image)PlateImagesList[0].ToBitmap();
                MethodInvoker mi5 = delegate
                {
                    if (!anyway)
                    {
                        pictureBox_XeRA.Image = Plate_Draw;
                        pictureBox_BiensoRA.Image = PlateImagesList[0].ToBitmap();
                    }
                };
                if (InvokeRequired)
                    Invoke(mi5);
                serialPort3.Open();
                // di chuyen den vi tri co xe
                switch (data.Vitri)
                {
                    case 1:
                        serialPort3.Write("11");
                        serialPort3.Close();
                        break;
                    case 2:
                        serialPort3.Write("12");
                        serialPort3.Close();
                        break;
                    case 3:
                        serialPort3.Write("13");
                        serialPort3.Close();
                        break;
                    case 4:
                        serialPort3.Write("14");
                        serialPort3.Close();
                        break;
                    case 5:
                        serialPort3.Write("15");
                        serialPort3.Close();
                        break;
                    //case 6:
                    //    serialPort3.Write("16");
                    //    serialPort3.Close();
                    //    break;
                        //sau do di chuyen toi vi tri xe theo vi tri tren
                }
                Notify thongbao = new Notify();

                thongbao.Show();
               // Thread.Sleep(10000);
                thongbao.Close();
                Thread.Sleep(1000);
                serialPort3.Open();

                DateTime now = DateTime.Now;
                TimeSpan duration = now - data.Giovao;
                int tien = duration.Seconds + duration.Minutes * 60 + duration.Hours * 60 * 60 + duration.Days * 60 * 60 * 24;
                tien *= 10;
                data.Tiengui = tien;
                MethodInvoker mi3 = delegate
                {
                //pictureBox2.Image = Plate_Draw;
                //pictureBox5.Image = PlateImagesList[0].ToBitmap();
                    textBox4.Text = DateTime.Now.ToString();
                    textBox3.Text = IDRFID;
                    text_BiensoRa.Text = biensoxe_ngan;
                    textBox6.Text = tien.ToString("0,0");
                };
                if (InvokeRequired)
                    this.Invoke(mi3);
                data.Hinhbiensora = Hinhxe_bienso;
                SQLDTO temp_data = data;
                MethodInvoker mi4 = delegate
                {
                  bus.SQL_Update(temp_data);
                panel_RA.BackColor = Color.Chartreuse;
                };
                if (InvokeRequired)
                    this.Invoke(mi4);
                MessageBox.Show("Đã lấy xe ra", "Thành Công", MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {

            }

        }
        private void Reconize_process(ref SQLDTO data, SQLBUS bus, bool them_xe)
        {
            string path = "";
            if (radioButton1.Checked == true)
            {
                pictureBox_XeRA.Image = null;
                IF.pictureBox2.Image = null;
                // capture.QueryFrame().Save("aa.bmp");
                if (!them_xe && pictureBox_BiensoRA.Image != null)
                    pictureBox_BiensoRA.Image.Save(m_path + "aa.bmp");
                else
                    if (them_xe && pictureBox_BiensoVAO.Image != null)
                    pictureBox_BiensoVAO.Image.Save(m_path + "aa.bmp");
                else
                {
                    MessageBox.Show(" Kiểm tra lại camera", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FileStream fs = new FileStream(m_path + "aa.bmp", FileMode.Open, FileAccess.Read);
                Image temp = Image.FromStream(fs);
                fs.Close();
                pictureBox_XeRA.Image = temp;
                IF.pictureBox2.Image = temp;
                pictureBox_XeRA.Update();
                IF.pictureBox2.Update();
                path = m_path + "aa.bmp";
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
                dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                path = dlg.FileName;
            }
            Image Hinhxe_bienso;
            string biensoxe_ngan, biensoxe_dai;
            Reconize(path, out Hinhxe_bienso, out biensoxe_ngan, out biensoxe_dai);
            if (biensoxe_ngan.Length < 5)
            {
                MessageBox.Show("Chưa nhận dạng đúng được Biển số \n\r\n " + "số chữ: " + biensoxe_ngan.Length.ToString()
                    , "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (them_xe && biensoxe_ngan != "")
            {
                pictureBox_XeVAO.Image = Plate_Draw;
                pictureBox_XeRA.Image = null;
                pictureBox_BiensoVAO.Image = PlateImagesList[0].ToBitmap();
                pictureBox_BiensoRA.Image = null;

                text_BiensoVAO.Text = biensoxe_dai;
                text_BiensoRa.Text = "";
                textBox1.Text = IDRFID;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                data.Hinhbiensovao = Hinhxe_bienso;
                data.Biensovao = (Image)PlateImagesList[0].ToBitmap();
                data.Bienso = biensoxe_ngan;
                textBox2.Text = DateTime.Now.ToString();

                if (bus.SQL_Insert(data))
                {
                    // dieu khien arduino cat xe
                    //serialPort2.Write("1");

                    Notify thongbao = new Notify();
                    thongbao.textBox1.Text = "Đang thêm xe vào \n\r\n Vui lòng đợi trong giây lát...";
                    thongbao.Show();

                    // thongbao.Close();
                    MessageBox.Show("Đã thêm xe vào dữ liệu", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xảy ra lỗi khi thêm xe vào", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            else if (biensoxe_ngan != "")
            {
                bus.SQL_Select_row(ref data);
                pictureBox_XeVAO.Image = data.Hinhbiensovao;
                pictureBox_BiensoVAO.Image = data.Biensovao;
                text_BiensoVAO.Text = data.Bienso;
                textBox2.Text = data.Giovao.ToString();
                if (data.Bienso == biensoxe_ngan)
                {
                    // di chuyen lay xe ra
                    Notify thongbao = new Notify();
                    thongbao.textBox1.Text = "Đang lấy xe ra \n\r\n Vui lòng đợi trong giây lát ... ";
                    thongbao.Show();
                    thongbao.Close();
                    DateTime now = DateTime.Now;
                    TimeSpan duration = now - data.Giovao;
                    int tien = duration.Seconds + duration.Minutes * 60 + duration.Hours * 60 * 60 + duration.Days * 60 * 60 * 24;
                    data.Tiengui = tien;
                    pictureBox_XeRA.Image = Plate_Draw;
                    pictureBox_BiensoRA.Image = PlateImagesList[0].ToBitmap();
                    textBox4.Text = DateTime.Now.ToString();
                    textBox1.Text = "";
                    textBox3.Text = IDRFID;
                    textBox6.Text = tien.ToString("0,0");
                    data.Hinhbiensora = Hinhxe_bienso;
                    bus.SQL_Update(data);

                    MessageBox.Show("Đã lấy xe ra!!", "Thông Báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Xe không đúng", "Thông báo", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Xảy ra lỗi khi nhận diện biển số", "Warning", MessageBoxButtons.OK);
            }
        }
        Thread thread_Vao = null;
        Thread thread_Ra = null;
        private void EVENT()
        {
            SQLDTO data = new SQLDTO();
            data.RFID = IDRFID;
            SQLBUS bus = new SQLBUS();
            bool check = bus.SQL_Check(ref data);
            // if (check == 1)
            //// {
            //  MessageBox.Show("Lỗi: tìm thấy " + check.ToString() + " xe trong bãi", "Error", MessageBoxButtons.OK, //MessageBoxIcon.Error);
            //   return;
            //  }
            if (check == true)
            {
                thread_Vao = new Thread(() => { Process_VAO(ref data); });
                thread_Vao.Start();
            }
            else 
            {
                thread_Ra = new Thread(() => { Xe_Ra(ref data); });
                thread_Ra.Start();
            }
        }

        #region RESIZE
        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Thread.Sleep(100);
            //Size a = Size;
            //    panel1.Size = new Size(Size.Width, panel1.Size.Height);
            //    button1.Location = new Point(panel1.Size.Width - button1.Size.Width, button1.Location.Y);
            //    button6.Location = new Point(button1.Location.X - button6.Size.Width - 1, button6.Location.Y);
            //    button8.Location = new Point(button6.Location.X - button8.Size.Width - 1, button8.Location.Y);

            //    panel_VAO.Location = new Point(0, 0);
            //    panel_VAO.Size = new Size((Size.Width - 184) / 2, Size.Height);
            //    panel_RA.Location = new Point(panel_VAO.Size.Width, 0);
            //    panel_RA.Size = new Size((Size.Width - 184) / 2, Size.Height);
            //    panel_WC.Location = new Point(panel_VAO.Size.Width + panel_RA.Size.Width, 0);
            //    panel_WC.Size = new Size(184, Size.Height);

            //    panel5.Location = new Point(Size.Width - panel5.Width, Size.Height - panel5.Height);

            //    pictureBox_XeVAO.Size = new Size(panel_VAO.Size.Width * 294 / 306, panel_VAO.Size.Width * 260 / 306);
            //    pictureBox_XeVAO.Location = new Point(panel_VAO.Size.Width / 2 - pictureBox_XeVAO.Size.Width / 2, pictureBox_XeVAO.Location.Y);
            //    groupBox1.Location = new Point(groupBox1.Location.X, pictureBox_XeVAO.Location.Y + pictureBox_XeVAO.Size.Height + 6);

            //    int h = panel_VAO.Height - groupBox1.Location.Y - 10;
            //    int w = h * 294 / 194;
            //    if( h > 194 && w < panel_VAO.Width - 12)
            //        groupBox1.Size = new Size(w, h);
            //    else
            //    {
            //        w = panel_VAO.Width - 12;
            //        h = w * 194 / 294;
            //        if(w > 294 && h < panel_VAO.Height - groupBox1.Location.Y - 10)
            //            groupBox1.Size = new Size(w, h);
            //    }
            //    pictureBox_BiensoVAO.Size = new Size(groupBox1.Size.Width - 14 - pictureBox_BiensoVAO.Location.X
            //        , groupBox1.Size.Height - 67 - pictureBox_BiensoVAO.Location.Y);

            //    ///////////////////////

            //    pictureBox_XeRA.Size = new Size(panel_RA.Size.Width * 294 / 306, panel_RA.Size.Width * 260 / 306);
            //    pictureBox_XeRA.Location = new Point(panel_RA.Size.Width / 2 - pictureBox_XeRA.Size.Width / 2, pictureBox_XeRA.Location.Y);
            //    groupBox2.Location = new Point(groupBox2.Location.X, pictureBox_XeRA.Location.Y + pictureBox_XeRA.Size.Height + 6);

            //    h = panel_RA.Height - groupBox2.Location.Y - 10;
            //    w = h * 294 / 194;
            //    if (h > 194 && w < panel_RA.Width - 12)
            //        groupBox2.Size = new Size(w, h);
            //    else
            //    {
            //        w = panel_RA.Width - 12;
            //        h = w * 194 / 294;
            //        if (w > 294 && h < panel_RA.Height - groupBox2.Location.Y - 10)
            //            groupBox2.Size = new Size(w, h);
            //    }
            //    pictureBox_BiensoRA.Size = new Size(groupBox2.Size.Width - 14 - pictureBox_BiensoRA.Location.X
            //        , groupBox2.Size.Height - 67 - pictureBox_BiensoRA.Location.Y);
            //}
            //private void resizeInGr(GroupBox gr, ref TextBox tx, ref Label lb, int dis_d, int dis_r_t, int dis_r_l, bool t)
            //{
            //    if(dis_r_t < 0)
            //    {
            //        tx.Location = new Point(tx.Location.X, gr.Size.Height - dis_d);
            //        lb.Location = new Point(lb.Location.X, gr.Size.Height - dis_d);
            //    }
            //    else
            //    {
            //        tx.Location = new Point(gr.Size.Width - dis_r_t, gr.Size.Height - dis_d);
            //        lb.Location = new Point(gr.Size.Width - dis_r_l, gr.Size.Height - dis_d);
            //    }
            //    if (t)
            //        tx.Size = new Size(gr.Size.Width - 3 - tx.Location.X, tx.Size.Height);
            //}
            //private void button6_Click(object sender, EventArgs e)
            //{
            //    if (this.WindowState == FormWindowState.Normal)
            //        this.WindowState = FormWindowState.Maximized;
            //    else if (this.WindowState == FormWindowState.Maximized)
            //        this.WindowState = FormWindowState.Normal;
            //}

            //private void button8_Click(object sender, EventArgs e)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //}

            //private void splitter1_MouseMove(object sender, MouseEventArgs e)
            //{

            //    if (mouseDown)
            //    {
            //        int w = Size.Width + (e.X - lastLocation.X);
            //        if (w < 796)
            //        {
            //            w = 796;
            //        }
            //        this.Size = new Size(w, Size.Height);
            //        this.Update();
            //    }
            //}

            //private void splitter2_MouseMove(object sender, MouseEventArgs e)
            //{
            //    if (mouseDown)
            //    {
            //        int h = Size.Height + (e.Y - lastLocation.Y);
            //        if (h < 504)
            //        {
            //            h = 504;
            //        }
            //        this.Size = new Size(Size.Width, h);
            //        this.Update();
            //    }
            //}

            //private void panel5_MouseMove(object sender, MouseEventArgs e)
            //{
            //    if (mouseDown)
            //    {
            //        int w = Size.Width + (e.X - lastLocation.X);
            //        if (w < 796)
            //        {
            //            w = 796;
            //        }
            //        int h = Size.Height + (e.Y - lastLocation.Y);
            //        if (h < 504)
            //        {
            //            h = 504;
            //        }
            //        this.Size = new Size(w, h);
            //        this.Update();
            //    }
        }

        #endregion

        #region WEBCAM
        WEBCAM[] cam = new WEBCAM[1];
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox p = (PictureBox)sender;
                for (int i = 0; i < cam.Length; i++)
                {
                    if (cam[i] != null && cam[i].status == "run" && cam[i].pb == p.Name)
                    {
                        cam[i].Stop();
                        cam[i] = null;
                    }
                }
                ContextMenu m = new ContextMenu();
                List<string> ls = WEBCAM.get_all_cam();
                for (int i = 0; i <= 2 & i < ls.Count; i++)
                {
                    m.MenuItems.Add(ls[i], (s, e2) =>
                    {
                        MenuItem menuItem = s as MenuItem;
                        ContextMenu owner = menuItem.Parent as ContextMenu;
                        PictureBox pb = (PictureBox)owner.SourceControl;
                        if (cam[menuItem.Index] != null && cam[menuItem.Index].status == "run")
                        {
                            cam[menuItem.Index].Stop();
                            //cam[menuItem.Index] = null;
                        }
                        cam[menuItem.Index] = new WEBCAM();
                        cam[menuItem.Index].Start(menuItem.Index);
                        cam[menuItem.Index].put_picturebox(pb.Name);
                    });
                }
                m.Show(p, new Point(e.X, e.Y));
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < cam.Length; i++)
                {
                    if (cam[i] != null && cam[i].status == "run" && cam[i].image != null)
                    {
                        MethodInvoker mi = delegate
                        {
                            PictureBox pb = this.Controls.Find(cam[i].pb, true).FirstOrDefault() as PictureBox;
                            pb.Image = cam[i].image;
                            pb.Update();
                            pb.Invalidate();
                        };
                        if (InvokeRequired)
                        {
                            Invoke(mi);
                            return;
                        }

                        PictureBox pb2 = this.Controls.Find(cam[i].pb, true).FirstOrDefault() as PictureBox;
                        pb2.Image = cam[i].image;
                        pb2.Update();
                        pb2.Invalidate();
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion

        private void panel_WC_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
        //   bool success1 = true; 
        private void button4_Click(object sender, EventArgs e)
        {

            //string[] ports = SerialPort.GetPortNames();
            //comboBox1.Items.Clear();
            //if (ports.Length != 0)
            //{
            //    for (int j = 0; j < ports.Length; j++)
            //    {
            //        comboBox1.Items.Add(ports[j]);
            //    }
            //    comboBox1.Text = ports[0];
            //}


        }
        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
               Database f = new Database();
            DataTable dt = bus.get_All_Data();
            f.dataGridView2.DataSource = dt;
            f.Show();
        }
        private void button10_Click(object sender, EventArgs e)
        {

           // if (!serialPort2.IsOpen) // Nếu đối tượng serialPort2 chưa được mở , sau khi nhấn button 1 thỳ…
          //  {

             //   serialPort2.PortName = comboBox1.Text;//cổng serialPort1 = ComboBox mà bạn đang chọn
              //  serialPort2.Open();// Mở cổng serialPort2
           // }
        }

   
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (!serialPort3.IsOpen)
            {
                // button1.Text = ("Kết nối");
                label14.Text = ("Chưa kết nối");
                label14.ForeColor = Color.Red;
                if (label14.Text == "Chưa kết nối")
                {
                   // serialPort3.Open();
                }
            }
            else if (serialPort3.IsOpen)
            {
                // button1.Text = ("Ngắt kết nối");
                label14.Text = ("Ðã kết nối");
                label14.ForeColor = Color.Green;
                //Nếu Timer được làm mới, Cổng serialPort1 đã mở thì thay đổi Text trong button1, label3…đổi màu text label3 thành màu xanh

            }
        }
        private void timer5_Tick(object sender, EventArgs e)
        {
          
            ///while (serialPort2.IsOpen)
         //   {
           //     string sensor = serialPort1.ReadLine();//ReadExisting
               // this.BeginInvoke(new LineReceivedEvent(LineReceived), sensor);
          //  }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (!serialPort3.IsOpen) // Nếu đối tượng serialPort1 chưa được mở , sau khi nhấn button 1 thỳ…
            {

                serialPort3.PortName = comboBox1.Text;//cổng serialPort1 = ComboBox mà bạn đang chọn
                serialPort3.Open();// Mở cổng serialPort1
            }
        }
       


    }
}