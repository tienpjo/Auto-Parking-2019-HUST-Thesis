namespace Auto_parking
{
    partial class Database
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Database));
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bienso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hinhbiensovao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Giovao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hinhbiensora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vitri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Giora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Bienso,
            this.RFID,
            this.Hinhbiensovao,
            this.Giovao,
            this.Hinhbiensora,
            this.Vitri,
            this.Giora});
            this.dataGridView2.Location = new System.Drawing.Point(7, 65);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1327, 537);
            this.dataGridView2.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 50;
            // 
            // Bienso
            // 
            this.Bienso.DataPropertyName = "Bienso";
            this.Bienso.HeaderText = "Bienso";
            this.Bienso.Name = "Bienso";
            // 
            // RFID
            // 
            this.RFID.DataPropertyName = "RFID";
            this.RFID.HeaderText = "RFID";
            this.RFID.Name = "RFID";
            // 
            // Hinhbiensovao
            // 
            this.Hinhbiensovao.DataPropertyName = "Hinhbiensovao";
            this.Hinhbiensovao.HeaderText = "Hinhbiensovao";
            this.Hinhbiensovao.Name = "Hinhbiensovao";
            // 
            // Giovao
            // 
            this.Giovao.DataPropertyName = "Giovao";
            this.Giovao.HeaderText = "Giovao";
            this.Giovao.Name = "Giovao";
            // 
            // Hinhbiensora
            // 
            this.Hinhbiensora.DataPropertyName = "Hinhbiensora";
            this.Hinhbiensora.HeaderText = "Hinhbiensora";
            this.Hinhbiensora.Name = "Hinhbiensora";
            // 
            // Vitri
            // 
            this.Vitri.DataPropertyName = "Vitri";
            this.Vitri.HeaderText = "Vitri";
            this.Vitri.Name = "Vitri";
            // 
            // Giora
            // 
            this.Giora.DataPropertyName = "Giora";
            this.Giora.HeaderText = "Giora";
            this.Giora.Name = "Giora";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Myriad Pro Light", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(491, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "DATABASE AUTOPARKING ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1129, 608);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 36);
            this.button1.TabIndex = 2;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Database
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 690);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Database";
            this.Text = "Database";
            this.Load += new System.EventHandler(this.Database_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bienso;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hinhbiensovao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Giovao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hinhbiensora;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vitri;
        private System.Windows.Forms.DataGridViewTextBoxColumn Giora;
    }
}