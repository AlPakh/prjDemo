
namespace prjDemo
{
    partial class Form1
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
            this.pnlView = new System.Windows.Forms.Panel();
            this.pnlMakeMap = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnSaveMap = new System.Windows.Forms.Button();
            this.pnlField = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlView
            // 
            this.pnlView.BackColor = System.Drawing.Color.White;
            this.pnlView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlView.Location = new System.Drawing.Point(12, 18);
            this.pnlView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(1121, 814);
            this.pnlView.TabIndex = 1;
            // 
            // pnlMakeMap
            // 
            this.pnlMakeMap.Location = new System.Drawing.Point(16, 68);
            this.pnlMakeMap.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMakeMap.Name = "pnlMakeMap";
            this.pnlMakeMap.Size = new System.Drawing.Size(840, 406);
            this.pnlMakeMap.TabIndex = 3;
            this.pnlMakeMap.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 523);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 4;
            this.textBox1.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 555);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(137, 21);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "fill on mouseover";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.Location = new System.Drawing.Point(1140, 271);
            this.btnSaveMap.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new System.Drawing.Size(208, 76);
            this.btnSaveMap.TabIndex = 6;
            this.btnSaveMap.Text = "Save map to file";
            this.btnSaveMap.UseVisualStyleBackColor = true;
            this.btnSaveMap.Visible = false;
            this.btnSaveMap.Click += new System.EventHandler(this.btnSaveMap_Click);
            // 
            // pnlField
            // 
            this.pnlField.BackColor = System.Drawing.Color.Black;
            this.pnlField.Location = new System.Drawing.Point(1177, 563);
            this.pnlField.Margin = new System.Windows.Forms.Padding(4);
            this.pnlField.Name = "pnlField";
            this.pnlField.Size = new System.Drawing.Size(318, 203);
            this.pnlField.TabIndex = 11;
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtMessage.Location = new System.Drawing.Point(1140, 18);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(390, 178);
            this.txtMessage.TabIndex = 16;
            this.txtMessage.Text = "Вы ощущаете лёгкий ветер на лице. С пробуждением";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(1140, 773);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(390, 60);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "Выйти";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::prjDemo.Properties.Resources._00;
            this.ClientSize = new System.Drawing.Size(1535, 848);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pnlField);
            this.Controls.Add(this.btnSaveMap);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pnlMakeMap);
            this.Controls.Add(this.pnlView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.Panel pnlMakeMap;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnSaveMap;
        private System.Windows.Forms.Panel pnlField;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnExit;
    }
}

