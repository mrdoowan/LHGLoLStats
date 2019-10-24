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
            this.button_LoadNames = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_apiKey = new System.Windows.Forms.TextBox();
            this.comboBox_Competition = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_UpdateRanks = new System.Windows.Forms.Button();
            this.button_LoadMatch = new System.Windows.Forms.Button();
            this.textBox_NewCompName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_ExportExcel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button_AssignRoles = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_MatchId = new System.Windows.Forms.ComboBox();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_NoDBLog = new System.Windows.Forms.CheckBox();
            this.button_LoadTeamNames = new System.Windows.Forms.Button();
            this.button_LoadChampJSON = new System.Windows.Forms.Button();
            this.comboBox_CompetitionType = new System.Windows.Forms.ComboBox();
            this.textBox_ConnectionString = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_LoadNames
            // 
            this.button_LoadNames.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_LoadNames.Location = new System.Drawing.Point(370, 94);
            this.button_LoadNames.Name = "button_LoadNames";
            this.button_LoadNames.Size = new System.Drawing.Size(129, 47);
            this.button_LoadNames.TabIndex = 0;
            this.button_LoadNames.Text = "Make New Competition";
            this.button_LoadNames.UseVisualStyleBackColor = true;
            this.button_LoadNames.Click += new System.EventHandler(this.button_LoadNames_Click);
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
            this.textBox_apiKey.Location = new System.Drawing.Point(118, 38);
            this.textBox_apiKey.Name = "textBox_apiKey";
            this.textBox_apiKey.Size = new System.Drawing.Size(246, 20);
            this.textBox_apiKey.TabIndex = 4;
            this.textBox_apiKey.TextChanged += new System.EventHandler(this.textBox_apiKey_TextChanged);
            // 
            // comboBox_Competition
            // 
            this.comboBox_Competition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.comboBox_Competition.FormattingEnabled = true;
            this.comboBox_Competition.Location = new System.Drawing.Point(118, 174);
            this.comboBox_Competition.Name = "comboBox_Competition";
            this.comboBox_Competition.Size = new System.Drawing.Size(246, 21);
            this.comboBox_Competition.TabIndex = 5;
            this.comboBox_Competition.SelectedIndexChanged += new System.EventHandler(this.comboBox_Competition_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.Location = new System.Drawing.Point(12, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select Competition:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_UpdateRanks
            // 
            this.button_UpdateRanks.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_UpdateRanks.Location = new System.Drawing.Point(118, 255);
            this.button_UpdateRanks.Name = "button_UpdateRanks";
            this.button_UpdateRanks.Size = new System.Drawing.Size(120, 36);
            this.button_UpdateRanks.TabIndex = 7;
            this.button_UpdateRanks.Text = "Update Summoner Ranks in Competition";
            this.button_UpdateRanks.UseVisualStyleBackColor = true;
            this.button_UpdateRanks.Click += new System.EventHandler(this.button_UpdateRanks_Click);
            // 
            // button_LoadMatch
            // 
            this.button_LoadMatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_LoadMatch.Location = new System.Drawing.Point(370, 174);
            this.button_LoadMatch.Name = "button_LoadMatch";
            this.button_LoadMatch.Size = new System.Drawing.Size(129, 21);
            this.button_LoadMatch.TabIndex = 15;
            this.button_LoadMatch.Text = "Load Match ID";
            this.button_LoadMatch.UseVisualStyleBackColor = true;
            this.button_LoadMatch.Click += new System.EventHandler(this.button_LoadMatch_Click);
            // 
            // textBox_NewCompName
            // 
            this.textBox_NewCompName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox_NewCompName.Location = new System.Drawing.Point(118, 94);
            this.textBox_NewCompName.Name = "textBox_NewCompName";
            this.textBox_NewCompName.Size = new System.Drawing.Size(246, 20);
            this.textBox_NewCompName.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.Location = new System.Drawing.Point(12, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 16;
            this.label6.Text = "Competition Name:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_ExportExcel
            // 
            this.button_ExportExcel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_ExportExcel.Location = new System.Drawing.Point(244, 255);
            this.button_ExportExcel.Name = "button_ExportExcel";
            this.button_ExportExcel.Size = new System.Drawing.Size(120, 36);
            this.button_ExportExcel.TabIndex = 18;
            this.button_ExportExcel.Text = "Export Competition Stats in Excel";
            this.button_ExportExcel.UseVisualStyleBackColor = true;
            this.button_ExportExcel.Click += new System.EventHandler(this.button_ExportExcel_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.Location = new System.Drawing.Point(15, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(484, 25);
            this.label7.TabIndex = 19;
            this.label7.Text = "_______________________________________________________________________________";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_AssignRoles
            // 
            this.button_AssignRoles.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_AssignRoles.Location = new System.Drawing.Point(370, 228);
            this.button_AssignRoles.Name = "button_AssignRoles";
            this.button_AssignRoles.Size = new System.Drawing.Size(129, 21);
            this.button_AssignRoles.TabIndex = 20;
            this.button_AssignRoles.Text = "Assign Names";
            this.button_AssignRoles.UseVisualStyleBackColor = true;
            this.button_AssignRoles.Click += new System.EventHandler(this.button_AssignRoles_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.Location = new System.Drawing.Point(12, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 22;
            this.label3.Text = "Select Match ID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_MatchId
            // 
            this.comboBox_MatchId.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.comboBox_MatchId.FormattingEnabled = true;
            this.comboBox_MatchId.Location = new System.Drawing.Point(118, 228);
            this.comboBox_MatchId.Name = "comboBox_MatchId";
            this.comboBox_MatchId.Size = new System.Drawing.Size(246, 21);
            this.comboBox_MatchId.TabIndex = 21;
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.richTextBox_Log.BackColor = System.Drawing.SystemColors.ControlLight;
            this.richTextBox_Log.Location = new System.Drawing.Point(12, 326);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.ReadOnly = true;
            this.richTextBox_Log.Size = new System.Drawing.Size(487, 86);
            this.richTextBox_Log.TabIndex = 23;
            this.richTextBox_Log.Text = "";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 23);
            this.label4.TabIndex = 24;
            this.label4.Text = "Log";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_NoDBLog
            // 
            this.checkBox_NoDBLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBox_NoDBLog.Location = new System.Drawing.Point(244, 297);
            this.checkBox_NoDBLog.Name = "checkBox_NoDBLog";
            this.checkBox_NoDBLog.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_NoDBLog.Size = new System.Drawing.Size(255, 24);
            this.checkBox_NoDBLog.TabIndex = 25;
            this.checkBox_NoDBLog.Text = "Do not log Database calls";
            this.checkBox_NoDBLog.UseVisualStyleBackColor = true;
            this.checkBox_NoDBLog.CheckedChanged += new System.EventHandler(this.checkBox_NoDBLog_CheckedChanged);
            // 
            // button_LoadTeamNames
            // 
            this.button_LoadTeamNames.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_LoadTeamNames.Location = new System.Drawing.Point(118, 201);
            this.button_LoadTeamNames.Name = "button_LoadTeamNames";
            this.button_LoadTeamNames.Size = new System.Drawing.Size(120, 21);
            this.button_LoadTeamNames.TabIndex = 26;
            this.button_LoadTeamNames.Text = "Load Team Names";
            this.button_LoadTeamNames.UseVisualStyleBackColor = true;
            this.button_LoadTeamNames.Click += new System.EventHandler(this.button_LoadTeamNames_Click);
            // 
            // button_LoadChampJSON
            // 
            this.button_LoadChampJSON.Location = new System.Drawing.Point(370, 37);
            this.button_LoadChampJSON.Name = "button_LoadChampJSON";
            this.button_LoadChampJSON.Size = new System.Drawing.Size(129, 21);
            this.button_LoadChampJSON.TabIndex = 27;
            this.button_LoadChampJSON.Text = "Load Champ JSON";
            this.button_LoadChampJSON.UseVisualStyleBackColor = true;
            this.button_LoadChampJSON.Click += new System.EventHandler(this.button_LoadChampJSON_Click);
            // 
            // comboBox_CompetitionType
            // 
            this.comboBox_CompetitionType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.comboBox_CompetitionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CompetitionType.FormattingEnabled = true;
            this.comboBox_CompetitionType.Items.AddRange(new object[] {
            "Tournament",
            "League Regular",
            "League Playoffs"});
            this.comboBox_CompetitionType.Location = new System.Drawing.Point(118, 120);
            this.comboBox_CompetitionType.Name = "comboBox_CompetitionType";
            this.comboBox_CompetitionType.Size = new System.Drawing.Size(246, 21);
            this.comboBox_CompetitionType.TabIndex = 28;
            // 
            // textBox_ConnectionString
            // 
            this.textBox_ConnectionString.Location = new System.Drawing.Point(118, 12);
            this.textBox_ConnectionString.Name = "textBox_ConnectionString";
            this.textBox_ConnectionString.Size = new System.Drawing.Size(246, 20);
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
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(487, 25);
            this.label8.TabIndex = 31;
            this.label8.Text = "_______________________________________________________________________________";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 424);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_ConnectionString);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_CompetitionType);
            this.Controls.Add(this.button_LoadChampJSON);
            this.Controls.Add(this.button_LoadTeamNames);
            this.Controls.Add(this.checkBox_NoDBLog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBox_Log);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_MatchId);
            this.Controls.Add(this.button_AssignRoles);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button_ExportExcel);
            this.Controls.Add(this.textBox_NewCompName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_LoadMatch);
            this.Controls.Add(this.button_UpdateRanks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_Competition);
            this.Controls.Add(this.textBox_apiKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_LoadNames);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LoadNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_apiKey;
        private System.Windows.Forms.ComboBox comboBox_Competition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_UpdateRanks;
        private System.Windows.Forms.Button button_LoadMatch;
        private System.Windows.Forms.TextBox textBox_NewCompName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_ExportExcel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_AssignRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_MatchId;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_NoDBLog;
        private System.Windows.Forms.Button button_LoadTeamNames;
        private System.Windows.Forms.Button button_LoadChampJSON;
        private System.Windows.Forms.ComboBox comboBox_CompetitionType;
        private System.Windows.Forms.TextBox textBox_ConnectionString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
    }
}

