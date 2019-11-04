namespace LoLStatsAPIv4_GUI {
    partial class MainForm {
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_apiKey = new System.Windows.Forms.TextBox();
            this.comboBox_Competition = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_UpdateRanks = new System.Windows.Forms.Button();
            this.button_LoadMatch = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button_EditMatchRoles = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_MatchId = new System.Windows.Forms.ComboBox();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_EditTeamNames = new System.Windows.Forms.Button();
            this.button_LoadChampJSON = new System.Windows.Forms.Button();
            this.textBox_ConnectionString = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "API Key:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_apiKey
            // 
            this.textBox_apiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_apiKey.Location = new System.Drawing.Point(118, 38);
            this.textBox_apiKey.Name = "textBox_apiKey";
            this.textBox_apiKey.Size = new System.Drawing.Size(297, 20);
            this.textBox_apiKey.TabIndex = 4;
            this.textBox_apiKey.TextChanged += new System.EventHandler(this.textBox_apiKey_TextChanged);
            // 
            // comboBox_Competition
            // 
            this.comboBox_Competition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Competition.FormattingEnabled = true;
            this.comboBox_Competition.Location = new System.Drawing.Point(118, 149);
            this.comboBox_Competition.Name = "comboBox_Competition";
            this.comboBox_Competition.Size = new System.Drawing.Size(297, 21);
            this.comboBox_Competition.TabIndex = 5;
            this.comboBox_Competition.SelectedIndexChanged += new System.EventHandler(this.comboBox_Competition_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select Competition:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_UpdateRanks
            // 
            this.button_UpdateRanks.Location = new System.Drawing.Point(274, 176);
            this.button_UpdateRanks.Name = "button_UpdateRanks";
            this.button_UpdateRanks.Size = new System.Drawing.Size(141, 21);
            this.button_UpdateRanks.TabIndex = 7;
            this.button_UpdateRanks.Text = "Update Ranks";
            this.button_UpdateRanks.UseVisualStyleBackColor = true;
            this.button_UpdateRanks.Click += new System.EventHandler(this.button_UpdateRanks_Click);
            // 
            // button_LoadMatch
            // 
            this.button_LoadMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadMatch.Location = new System.Drawing.Point(421, 149);
            this.button_LoadMatch.Name = "button_LoadMatch";
            this.button_LoadMatch.Size = new System.Drawing.Size(129, 21);
            this.button_LoadMatch.TabIndex = 15;
            this.button_LoadMatch.Text = "Load Match ID";
            this.button_LoadMatch.UseVisualStyleBackColor = true;
            this.button_LoadMatch.Click += new System.EventHandler(this.button_LoadMatch_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(12, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(538, 27);
            this.label6.TabIndex = 16;
            this.label6.Text = "Lol just do an Insert Query into the Competitions table";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(15, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(535, 25);
            this.label7.TabIndex = 19;
            this.label7.Text = "_________________________________________________________________________________" +
    "_____________________________________________";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_EditMatchRoles
            // 
            this.button_EditMatchRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_EditMatchRoles.Location = new System.Drawing.Point(421, 203);
            this.button_EditMatchRoles.Name = "button_EditMatchRoles";
            this.button_EditMatchRoles.Size = new System.Drawing.Size(129, 21);
            this.button_EditMatchRoles.TabIndex = 20;
            this.button_EditMatchRoles.Text = "Edit Match Roles";
            this.button_EditMatchRoles.UseVisualStyleBackColor = true;
            this.button_EditMatchRoles.Click += new System.EventHandler(this.button_EditMatchRoles_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 22;
            this.label3.Text = "Select Match ID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_MatchId
            // 
            this.comboBox_MatchId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_MatchId.FormattingEnabled = true;
            this.comboBox_MatchId.Location = new System.Drawing.Point(118, 203);
            this.comboBox_MatchId.Name = "comboBox_MatchId";
            this.comboBox_MatchId.Size = new System.Drawing.Size(297, 21);
            this.comboBox_MatchId.TabIndex = 21;
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Log.BackColor = System.Drawing.SystemColors.ControlLight;
            this.richTextBox_Log.Location = new System.Drawing.Point(12, 264);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.ReadOnly = true;
            this.richTextBox_Log.Size = new System.Drawing.Size(538, 160);
            this.richTextBox_Log.TabIndex = 23;
            this.richTextBox_Log.Text = "";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 23);
            this.label4.TabIndex = 24;
            this.label4.Text = "Log";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_EditTeamNames
            // 
            this.button_EditTeamNames.Location = new System.Drawing.Point(118, 176);
            this.button_EditTeamNames.Name = "button_EditTeamNames";
            this.button_EditTeamNames.Size = new System.Drawing.Size(150, 21);
            this.button_EditTeamNames.TabIndex = 26;
            this.button_EditTeamNames.Text = "Edit Teams";
            this.button_EditTeamNames.UseVisualStyleBackColor = true;
            this.button_EditTeamNames.Click += new System.EventHandler(this.button_EditTeamNames_Click);
            // 
            // button_LoadChampJSON
            // 
            this.button_LoadChampJSON.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadChampJSON.Location = new System.Drawing.Point(421, 37);
            this.button_LoadChampJSON.Name = "button_LoadChampJSON";
            this.button_LoadChampJSON.Size = new System.Drawing.Size(129, 21);
            this.button_LoadChampJSON.TabIndex = 27;
            this.button_LoadChampJSON.Text = "Load Champ JSON";
            this.button_LoadChampJSON.UseVisualStyleBackColor = true;
            this.button_LoadChampJSON.Click += new System.EventHandler(this.button_LoadChampJSON_Click);
            // 
            // textBox_ConnectionString
            // 
            this.textBox_ConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ConnectionString.Location = new System.Drawing.Point(118, 12);
            this.textBox_ConnectionString.Name = "textBox_ConnectionString";
            this.textBox_ConnectionString.Size = new System.Drawing.Size(297, 20);
            this.textBox_ConnectionString.TabIndex = 30;
            this.textBox_ConnectionString.TextChanged += new System.EventHandler(this.textBox_ConnectionString_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 29;
            this.label5.Text = "Connection String";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(538, 25);
            this.label8.TabIndex = 31;
            this.label8.Text = "_________________________________________________________________________________" +
    "_____________________________________________";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 436);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_ConnectionString);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_LoadChampJSON);
            this.Controls.Add(this.button_EditTeamNames);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBox_Log);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_MatchId);
            this.Controls.Add(this.button_EditMatchRoles);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_LoadMatch);
            this.Controls.Add(this.button_UpdateRanks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_Competition);
            this.Controls.Add(this.textBox_apiKey);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_apiKey;
        private System.Windows.Forms.ComboBox comboBox_Competition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_UpdateRanks;
        private System.Windows.Forms.Button button_LoadMatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_EditMatchRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_MatchId;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_EditTeamNames;
        private System.Windows.Forms.Button button_LoadChampJSON;
        private System.Windows.Forms.TextBox textBox_ConnectionString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
    }
}

