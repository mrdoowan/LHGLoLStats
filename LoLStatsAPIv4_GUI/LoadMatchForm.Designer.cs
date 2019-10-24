namespace LoLStatsAPIv4_GUI {
    partial class LoadMatchForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label6 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_MatchId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_BlueTeamName = new System.Windows.Forms.ComboBox();
            this.comboBox_RedTeamName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 18;
            this.label6.Text = "Blue Team Name:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(370, 12);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(81, 71);
            this.button_OK.TabIndex = 22;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 23;
            this.label1.Text = "Red Team Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_MatchId
            // 
            this.textBox_MatchId.Location = new System.Drawing.Point(118, 63);
            this.textBox_MatchId.Name = "textBox_MatchId";
            this.textBox_MatchId.Size = new System.Drawing.Size(246, 20);
            this.textBox_MatchId.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 25;
            this.label2.Text = "Match ID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_BlueTeamName
            // 
            this.comboBox_BlueTeamName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_BlueTeamName.FormattingEnabled = true;
            this.comboBox_BlueTeamName.Location = new System.Drawing.Point(118, 11);
            this.comboBox_BlueTeamName.Name = "comboBox_BlueTeamName";
            this.comboBox_BlueTeamName.Size = new System.Drawing.Size(246, 21);
            this.comboBox_BlueTeamName.TabIndex = 27;
            // 
            // comboBox_RedTeamName
            // 
            this.comboBox_RedTeamName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_RedTeamName.FormattingEnabled = true;
            this.comboBox_RedTeamName.Location = new System.Drawing.Point(118, 37);
            this.comboBox_RedTeamName.Name = "comboBox_RedTeamName";
            this.comboBox_RedTeamName.Size = new System.Drawing.Size(246, 21);
            this.comboBox_RedTeamName.TabIndex = 28;
            // 
            // LoadMatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 93);
            this.Controls.Add(this.comboBox_RedTeamName);
            this.Controls.Add(this.comboBox_BlueTeamName);
            this.Controls.Add(this.textBox_MatchId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label6);
            this.MaximizeBox = false;
            this.Name = "LoadMatchForm";
            this.Text = "LoadMatchStats";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_MatchId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_BlueTeamName;
        private System.Windows.Forms.ComboBox comboBox_RedTeamName;
    }
}