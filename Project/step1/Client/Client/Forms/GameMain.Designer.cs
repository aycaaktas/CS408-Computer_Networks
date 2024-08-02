namespace RPSGame.Client.Forms
{
    partial class GameMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.button_rock = new System.Windows.Forms.Button();
            this.button_paper = new System.Windows.Forms.Button();
            this.button_scissor = new System.Windows.Forms.Button();
            this.score_list = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Score = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_leave = new System.Windows.Forms.Button();
            this.button_terminate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.score_list)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(60, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(616, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "You have 10 seconds to pick! Otherwise you will lose!";
            // 
            // button_rock
            // 
            this.button_rock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button_rock.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.button_rock.Location = new System.Drawing.Point(105, 145);
            this.button_rock.Margin = new System.Windows.Forms.Padding(2);
            this.button_rock.Name = "button_rock";
            this.button_rock.Size = new System.Drawing.Size(111, 100);
            this.button_rock.TabIndex = 1;
            this.button_rock.Text = "R";
            this.button_rock.UseVisualStyleBackColor = false;
            this.button_rock.Click += new System.EventHandler(this.button_rock_Click);
            // 
            // button_paper
            // 
            this.button_paper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button_paper.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.button_paper.Location = new System.Drawing.Point(305, 145);
            this.button_paper.Margin = new System.Windows.Forms.Padding(2);
            this.button_paper.Name = "button_paper";
            this.button_paper.Size = new System.Drawing.Size(108, 100);
            this.button_paper.TabIndex = 2;
            this.button_paper.Text = "P";
            this.button_paper.UseVisualStyleBackColor = false;
            this.button_paper.Click += new System.EventHandler(this.button_paper_Click);
            // 
            // button_scissor
            // 
            this.button_scissor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button_scissor.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.button_scissor.Location = new System.Drawing.Point(521, 145);
            this.button_scissor.Margin = new System.Windows.Forms.Padding(2);
            this.button_scissor.Name = "button_scissor";
            this.button_scissor.Size = new System.Drawing.Size(115, 100);
            this.button_scissor.TabIndex = 3;
            this.button_scissor.Text = "S";
            this.button_scissor.UseVisualStyleBackColor = false;
            this.button_scissor.Click += new System.EventHandler(this.button_scissor_Click);
            // 
            // score_list
            // 
            this.score_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.score_list.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.Score});
            this.score_list.Location = new System.Drawing.Point(745, 65);
            this.score_list.Margin = new System.Windows.Forms.Padding(2);
            this.score_list.Name = "score_list";
            this.score_list.RowHeadersWidth = 82;
            this.score_list.RowTemplate.Height = 33;
            this.score_list.Size = new System.Drawing.Size(555, 344);
            this.score_list.TabIndex = 4;
            // 
            // name
            // 
            this.name.HeaderText = "Player Name";
            this.name.MinimumWidth = 10;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 200;
            // 
            // Score
            // 
            this.Score.HeaderText = "Score (Count of Wins)";
            this.Score.MinimumWidth = 10;
            this.Score.Name = "Score";
            this.Score.ReadOnly = true;
            this.Score.Width = 200;
            // 
            // button_leave
            // 
            this.button_leave.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button_leave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button_leave.Location = new System.Drawing.Point(165, 304);
            this.button_leave.Margin = new System.Windows.Forms.Padding(2);
            this.button_leave.Name = "button_leave";
            this.button_leave.Size = new System.Drawing.Size(126, 48);
            this.button_leave.TabIndex = 5;
            this.button_leave.Text = "Leave";
            this.button_leave.UseVisualStyleBackColor = false;
            this.button_leave.Click += new System.EventHandler(this.button_leave_Click);
            // 
            // button_terminate
            // 
            this.button_terminate.BackColor = System.Drawing.Color.SkyBlue;
            this.button_terminate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button_terminate.Location = new System.Drawing.Point(403, 304);
            this.button_terminate.Margin = new System.Windows.Forms.Padding(2);
            this.button_terminate.Name = "button_terminate";
            this.button_terminate.Size = new System.Drawing.Size(127, 48);
            this.button_terminate.TabIndex = 6;
            this.button_terminate.Text = "Terminate";
            this.button_terminate.UseVisualStyleBackColor = false;
            this.button_terminate.Click += new System.EventHandler(this.button_terminate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(141, 362);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Continue to watch the game";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(411, 362);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Leave the game";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(105, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(555, 86);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // GameMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 460);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_terminate);
            this.Controls.Add(this.button_leave);
            this.Controls.Add(this.score_list);
            this.Controls.Add(this.button_scissor);
            this.Controls.Add(this.button_paper);
            this.Controls.Add(this.button_rock);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GameMain";
            this.Text = "GameMain";
            this.Load += new System.EventHandler(this.GameMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.score_list)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_rock;
        private System.Windows.Forms.Button button_paper;
        private System.Windows.Forms.Button button_scissor;
        private System.Windows.Forms.DataGridView score_list;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Score;
        private System.Windows.Forms.Button button_leave;
        private System.Windows.Forms.Button button_terminate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}