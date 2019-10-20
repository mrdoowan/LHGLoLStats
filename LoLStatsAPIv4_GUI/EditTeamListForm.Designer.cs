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
            this.button_RemoveTeam = new System.Windows.Forms.Button();
            this.button_Confirm = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Teams
            // 
            this.listBox_Teams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Teams.FormattingEnabled = true;
            this.listBox_Teams.Location = new System.Drawing.Point(12, 41);
            this.listBox_Teams.Name = "listBox_Teams";
            this.listBox_Teams.Size = new System.Drawing.Size(277, 186);
            this.listBox_Teams.TabIndex = 0;
            // 
            // label_CompetitionName
            // 
            this.label_CompetitionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CompetitionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CompetitionName.Location = new System.Drawing.Point(12, 9);
            this.label_CompetitionName.Name = "label_CompetitionName";
            this.label_CompetitionName.Size = new System.Drawing.Size(277, 23);
            this.label_CompetitionName.TabIndex = 1;
            this.label_CompetitionName.Text = "label1";
            this.label_CompetitionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_AddTeam
            // 
            this.button_AddTeam.Location = new System.Drawing.Point(12, 233);
            this.button_AddTeam.Name = "button_AddTeam";
            this.button_AddTeam.Size = new System.Drawing.Size(93, 23);
            this.button_AddTeam.TabIndex = 3;
            this.button_AddTeam.Text = "Add Team";
            this.button_AddTeam.UseVisualStyleBackColor = true;
            this.button_AddTeam.Click += new System.EventHandler(this.button_AddTeam_Click);
            // 
            // button_RemoveTeam
            // 
            this.button_RemoveTeam.Location = new System.Drawing.Point(196, 233);
            this.button_RemoveTeam.Name = "button_RemoveTeam";
            this.button_RemoveTeam.Size = new System.Drawing.Size(93, 23);
            this.button_RemoveTeam.TabIndex = 4;
            this.button_RemoveTeam.Text = "Remove Team";
            this.button_RemoveTeam.UseVisualStyleBackColor = true;
            this.button_RemoveTeam.Click += new System.EventHandler(this.button_RemoveTeam_Click);
            // 
            // button_Confirm
            // 
            this.button_Confirm.Location = new System.Drawing.Point(12, 261);
            this.button_Confirm.Name = "button_Confirm";
            this.button_Confirm.Size = new System.Drawing.Size(277, 23);
            this.button_Confirm.TabIndex = 5;
            this.button_Confirm.Text = "Confirm Team Names";
            this.button_Confirm.UseVisualStyleBackColor = true;
            this.button_Confirm.Click += new System.EventHandler(this.button_Confirm_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Location = new System.Drawing.Point(111, 233);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(79, 23);
            this.button_Edit.TabIndex = 6;
            this.button_Edit.Text = "Edit Name";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // EditTeamListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 298);
            this.Controls.Add(this.button_Edit);
            this.Controls.Add(this.button_Confirm);
            this.Controls.Add(this.button_RemoveTeam);
            this.Controls.Add(this.button_AddTeam);
            this.Controls.Add(this.label_CompetitionName);
            this.Controls.Add(this.listBox_Teams);
            this.MaximizeBox = false;
            this.Name = "EditTeamListForm";
            this.Text = "EditTeamListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Teams;
        private System.Windows.Forms.Label label_CompetitionName;
        private System.Windows.Forms.Button button_AddTeam;
        private System.Windows.Forms.Button button_RemoveTeam;
        private System.Windows.Forms.Button button_Confirm;
        private System.Windows.Forms.Button button_Edit;
    }
}