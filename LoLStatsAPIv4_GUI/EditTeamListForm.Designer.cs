namespace LoLStatsAPIv4_GUI {
    partial class EditTeamListForm {
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
            this.listBox_Teams = new System.Windows.Forms.ListBox();
            this.label_CompetitionName = new System.Windows.Forms.Label();
            this.button_AddTeam = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.listBox_Summoners = new System.Windows.Forms.ListBox();
            this.button_AddPlayer = new System.Windows.Forms.Button();
            this.button_RemovePlayer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Teams
            // 
            this.listBox_Teams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Teams.FormattingEnabled = true;
            this.listBox_Teams.Location = new System.Drawing.Point(12, 41);
            this.listBox_Teams.Name = "listBox_Teams";
            this.listBox_Teams.Size = new System.Drawing.Size(139, 186);
            this.listBox_Teams.TabIndex = 0;
            this.listBox_Teams.SelectedIndexChanged += new System.EventHandler(this.listBox_Teams_SelectedIndexChanged);
            // 
            // label_CompetitionName
            // 
            this.label_CompetitionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CompetitionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CompetitionName.Location = new System.Drawing.Point(12, 9);
            this.label_CompetitionName.Name = "label_CompetitionName";
            this.label_CompetitionName.Size = new System.Drawing.Size(280, 23);
            this.label_CompetitionName.TabIndex = 1;
            this.label_CompetitionName.Text = "label1";
            this.label_CompetitionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_AddTeam
            // 
            this.button_AddTeam.Location = new System.Drawing.Point(12, 233);
            this.button_AddTeam.Name = "button_AddTeam";
            this.button_AddTeam.Size = new System.Drawing.Size(139, 23);
            this.button_AddTeam.TabIndex = 3;
            this.button_AddTeam.Text = "Add Team";
            this.button_AddTeam.UseVisualStyleBackColor = true;
            this.button_AddTeam.Click += new System.EventHandler(this.button_AddTeam_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Location = new System.Drawing.Point(12, 262);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(139, 23);
            this.button_Edit.TabIndex = 6;
            this.button_Edit.Text = "Edit Team Name";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // listBox_Summoners
            // 
            this.listBox_Summoners.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Summoners.FormattingEnabled = true;
            this.listBox_Summoners.Location = new System.Drawing.Point(157, 41);
            this.listBox_Summoners.Name = "listBox_Summoners";
            this.listBox_Summoners.Size = new System.Drawing.Size(139, 186);
            this.listBox_Summoners.TabIndex = 7;
            // 
            // button_AddPlayer
            // 
            this.button_AddPlayer.Location = new System.Drawing.Point(157, 233);
            this.button_AddPlayer.Name = "button_AddPlayer";
            this.button_AddPlayer.Size = new System.Drawing.Size(139, 23);
            this.button_AddPlayer.TabIndex = 8;
            this.button_AddPlayer.Text = "Add Player";
            this.button_AddPlayer.UseVisualStyleBackColor = true;
            this.button_AddPlayer.Click += new System.EventHandler(this.button_AddPlayer_Click);
            // 
            // button_RemovePlayer
            // 
            this.button_RemovePlayer.Location = new System.Drawing.Point(157, 262);
            this.button_RemovePlayer.Name = "button_RemovePlayer";
            this.button_RemovePlayer.Size = new System.Drawing.Size(139, 23);
            this.button_RemovePlayer.TabIndex = 9;
            this.button_RemovePlayer.Text = "Remove Player";
            this.button_RemovePlayer.UseVisualStyleBackColor = true;
            this.button_RemovePlayer.Click += new System.EventHandler(this.button_RemovePlayer_Click);
            // 
            // EditTeamListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 291);
            this.Controls.Add(this.button_RemovePlayer);
            this.Controls.Add(this.button_AddPlayer);
            this.Controls.Add(this.listBox_Summoners);
            this.Controls.Add(this.button_Edit);
            this.Controls.Add(this.button_AddTeam);
            this.Controls.Add(this.label_CompetitionName);
            this.Controls.Add(this.listBox_Teams);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 330);
            this.MinimumSize = new System.Drawing.Size(320, 300);
            this.Name = "EditTeamListForm";
            this.Text = "EditTeamListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Teams;
        private System.Windows.Forms.Label label_CompetitionName;
        private System.Windows.Forms.Button button_AddTeam;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.ListBox listBox_Summoners;
        private System.Windows.Forms.Button button_AddPlayer;
        private System.Windows.Forms.Button button_RemovePlayer;
    }
}