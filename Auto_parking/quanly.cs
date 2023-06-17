using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Auto_parking.SQL;
using System.Threading;

namespace Auto_parking
{
    public partial class quanly : Form
    {
        public quanly()
        {

            InitializeComponent();
            tx_ls.Add(textBox1);
            tx_ls.Add(textBox2);
            tx_ls.Add(textBox3);
            tx_ls.Add(textBox4);
            tx_ls.Add(textBox5);
            tx_ls.Add(textBox6);
            //tx_ls.Add(textBox7);
            //tx_ls.Add(textBox8);
            //tx_ls.Add(textBox9);
            //tx_ls.Add(textBox10);
            //tx_ls.Add(textBox11);
            //tx_ls.Add(textBox12);

            label.Add(label1);
            label.Add(label2);
            label.Add(label3);
            label.Add(label4);
            label.Add(label5);
            label.Add(label6);
            //label.Add(label7);
            perform_tick();
        }
        List<TextBox> tx_ls = new List<TextBox>();
        List<Label> label = new List<Label>();
        SQLBUS bus = new SQLBUS();
        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void quanly_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        public bool success = true;
        private void perform_tick()
        {
            // bus.SQL_Select_row(ref data);
            string[] biensox = bus.SQL_Select_park();

            try
            {
                for (int i = 0; i < tx_ls.Count; i++)
                {
                    tx_ls[i].Text = biensox[i].ToString();
                    //  tx_ls[i].Text = biensox[i].ToString();
                }
                textBox7.Text = "CÒN TRỐNG";
                textBox8.Text = "CÒN TRỐNG";
                textBox9.Text = "CÒN TRỐNG";
                textBox10.Text = "CÒN TRỐNG";
                textBox11.Text = "CÒN TRỐNG";
                textBox12.Text = "CÒN TRỐNG";
            }
            catch(Exception ex)
            {

            }





        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            perform_tick();
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            for (int i = 1; i < tx_ls.Count; i++) {
                if (this.tx_ls[i].InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                //    for (int i = 1; i < tx_ls.Count; i++)
                        this.tx_ls[i].Text = text;
                }
            }
        }
    }
}
