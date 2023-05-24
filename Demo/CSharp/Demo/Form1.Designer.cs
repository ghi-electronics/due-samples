namespace Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btSound = new Button();
            this.ledOn = new Button();
            this.button1 = new Button();
            this.label1 = new Label();
            this.sendtxt = new TextBox();
            this.btSend = new Button();
            this.SuspendLayout();
            // 
            // btSound
            // 
            this.btSound.Location = new Point(221, 235);
            this.btSound.Name = "btSound";
            this.btSound.Size = new Size(407, 40);
            this.btSound.TabIndex = 0;
            this.btSound.Text = "Play sound (on Pulse only)";
            this.btSound.UseVisualStyleBackColor = true;
            this.btSound.Click += this.btSound_Click;
            // 
            // ledOn
            // 
            this.ledOn.Location = new Point(221, 303);
            this.ledOn.Name = "ledOn";
            this.ledOn.Size = new Size(184, 40);
            this.ledOn.TabIndex = 1;
            this.ledOn.Text = "Led On";
            this.ledOn.UseVisualStyleBackColor = true;
            this.ledOn.Click += this.ledOn_Click;
            // 
            // button1
            // 
            this.button1.Location = new Point(444, 303);
            this.button1.Name = "button1";
            this.button1.Size = new Size(184, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Led Off";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(83, 113);
            this.label1.Name = "label1";
            this.label1.Size = new Size(103, 30);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input text";
            // 
            // sendtxt
            // 
            this.sendtxt.Location = new Point(221, 110);
            this.sendtxt.Name = "sendtxt";
            this.sendtxt.Size = new Size(407, 35);
            this.sendtxt.TabIndex = 4;
            this.sendtxt.Text = "Hellow world!";
            // 
            // btSend
            // 
            this.btSend.Location = new Point(221, 169);
            this.btSend.Name = "btSend";
            this.btSend.Size = new Size(407, 40);
            this.btSend.TabIndex = 5;
            this.btSend.Text = "Send text (to Pulse only)";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += this.btSend_Click;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(12F, 30F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.sendtxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ledOn);
            this.Controls.Add(this.btSound);
            this.Name = "Form1";
            this.Text = "DUELink's Demo";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btSound;
        private Button ledOn;
        private Button button1;
        private Label label1;
        private TextBox sendtxt;
        private Button btSend;
    }
}