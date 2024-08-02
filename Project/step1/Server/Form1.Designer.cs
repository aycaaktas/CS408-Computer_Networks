namespace Server
{
    public partial class ServerForm : System.Windows.Forms.Form
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            name = new DataGridViewTextBoxColumn();
            score = new DataGridViewTextBoxColumn();
            startButton = new Button();
            stopButton = new Button();
            notificationsTextBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { name, score });
            dataGridView1.Location = new Point(0, 1);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.RowTemplate.Height = 33;
            dataGridView1.Size = new Size(502, 194);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // name
            // 
            name.HeaderText = "Player Name";
            name.MinimumWidth = 10;
            name.Name = "name";
            name.ReadOnly = true;
            // 
            // score
            // 
            score.HeaderText = "Score";
            score.MinimumWidth = 10;
            score.Name = "score";
            score.ReadOnly = true;
            // 
            // startButton
            // 
            startButton.BackColor = Color.FromArgb(192, 255, 192);
            startButton.Location = new Point(11, 333);
            startButton.Margin = new Padding(2);
            startButton.Name = "startButton";
            startButton.Size = new Size(108, 56);
            startButton.TabIndex = 2;
            startButton.Text = "Start Server";
            startButton.UseVisualStyleBackColor = false;
            startButton.Click += startButton_Click;
            // 
            // stopButton
            // 
            stopButton.BackColor = Color.FromArgb(255, 128, 128);
            stopButton.Location = new Point(377, 331);
            stopButton.Margin = new Padding(2);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(105, 58);
            stopButton.TabIndex = 3;
            stopButton.Text = "Stop Server";
            stopButton.UseVisualStyleBackColor = false;
            stopButton.Click += stopButton_Click;
            // 
            // notificationsTextBox
            // 
            notificationsTextBox.Location = new Point(29, 208);
            notificationsTextBox.Name = "notificationsTextBox";
            notificationsTextBox.Size = new Size(407, 120);
            notificationsTextBox.TabIndex = 4;
            notificationsTextBox.Text = "";
            notificationsTextBox.TextChanged += notificationsTextBox_TextChanged;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 428);
            Controls.Add(notificationsTextBox);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Controls.Add(dataGridView1);
            Margin = new Padding(2);
            Name = "ServerForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn score;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private RichTextBox notificationsTextBox;
    }
}
