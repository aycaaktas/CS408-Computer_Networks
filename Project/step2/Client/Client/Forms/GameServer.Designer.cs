namespace RPSGame.Client.Forms
{
    partial class GameServer
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
            this.ip_label = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.name_label = new System.Windows.Forms.Label();
            this.ip_text = new System.Windows.Forms.TextBox();
            this.port_text = new System.Windows.Forms.TextBox();
            this.name_text = new System.Windows.Forms.TextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ip_label
            // 
            this.ip_label.AutoSize = true;
            this.ip_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ip_label.Location = new System.Drawing.Point(101, 104);
            this.ip_label.Name = "ip_label";
            this.ip_label.Size = new System.Drawing.Size(156, 37);
            this.ip_label.TabIndex = 0;
            this.ip_label.Text = "Server IP:";
            this.ip_label.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.port_label.Location = new System.Drawing.Point(107, 172);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(85, 37);
            this.port_label.TabIndex = 1;
            this.port_label.Text = "Port:";
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.name_label.Location = new System.Drawing.Point(106, 236);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(173, 37);
            this.name_label.TabIndex = 2;
            this.name_label.Text = "Username:";
            // 
            // ip_text
            // 
            this.ip_text.Location = new System.Drawing.Point(264, 109);
            this.ip_text.Name = "ip_text";
            this.ip_text.Size = new System.Drawing.Size(408, 31);
            this.ip_text.TabIndex = 3;
            // 
            // port_text
            // 
            this.port_text.Location = new System.Drawing.Point(198, 178);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(408, 31);
            this.port_text.TabIndex = 4;
            // 
            // name_text
            // 
            this.name_text.Location = new System.Drawing.Point(285, 242);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(408, 31);
            this.name_text.TabIndex = 5;
            // 
            // connect_button
            // 
            this.connect_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.connect_button.Location = new System.Drawing.Point(328, 335);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(183, 73);
            this.connect_button.TabIndex = 6;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // GameServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 490);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.ip_text);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.ip_label);
            this.Name = "GameServer";
            this.Text = "GameServer";
            //this.Load += new System.EventHandler(this.GameServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ip_label;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.TextBox ip_text;
        private System.Windows.Forms.TextBox port_text;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Button connect_button;
    }
}