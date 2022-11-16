namespace Light_Novel_EPUB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.url = new System.Windows.Forms.TextBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            this.saveEpub = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.TextBox();
            this.description = new System.Windows.Forms.TextBox();
            this.chaptersLabel = new System.Windows.Forms.Label();
            this.chaptersCount = new System.Windows.Forms.Label();
            this.coverImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFolder = new System.Windows.Forms.TextBox();
            this.saveTo = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.currentCount = new System.Windows.Forms.Label();
            this.totalCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.coverImage)).BeginInit();
            this.SuspendLayout();
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(52, 9);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(655, 22);
            this.url.TabIndex = 0;
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(12, 12);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(34, 16);
            this.urlLabel.TabIndex = 1;
            this.urlLabel.Text = "URL";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(713, 9);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(75, 23);
            this.search.TabIndex = 2;
            this.search.Text = "Search";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(12, 367);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(646, 105);
            this.log.TabIndex = 3;
            // 
            // saveEpub
            // 
            this.saveEpub.Enabled = false;
            this.saveEpub.Location = new System.Drawing.Point(670, 367);
            this.saveEpub.Name = "saveEpub";
            this.saveEpub.Size = new System.Drawing.Size(118, 61);
            this.saveEpub.TabIndex = 4;
            this.saveEpub.Text = "Save as EPUB";
            this.saveEpub.UseVisualStyleBackColor = true;
            this.saveEpub.Click += new System.EventHandler(this.saveEpub_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(246, 49);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(33, 16);
            this.titleLabel.TabIndex = 5;
            this.titleLabel.Text = "Title";
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(285, 46);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(503, 22);
            this.title.TabIndex = 6;
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(249, 93);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(539, 239);
            this.description.TabIndex = 7;
            // 
            // chaptersLabel
            // 
            this.chaptersLabel.AutoSize = true;
            this.chaptersLabel.Location = new System.Drawing.Point(246, 71);
            this.chaptersLabel.Name = "chaptersLabel";
            this.chaptersLabel.Size = new System.Drawing.Size(64, 16);
            this.chaptersLabel.TabIndex = 8;
            this.chaptersLabel.Text = "Chapters:";
            // 
            // chaptersCount
            // 
            this.chaptersCount.AutoSize = true;
            this.chaptersCount.Location = new System.Drawing.Point(316, 71);
            this.chaptersCount.Name = "chaptersCount";
            this.chaptersCount.Size = new System.Drawing.Size(30, 16);
            this.chaptersCount.TabIndex = 9;
            this.chaptersCount.Text = "N/A";
            // 
            // coverImage
            // 
            this.coverImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coverImage.ErrorImage = null;
            this.coverImage.InitialImage = null;
            this.coverImage.Location = new System.Drawing.Point(12, 37);
            this.coverImage.Name = "coverImage";
            this.coverImage.Size = new System.Drawing.Size(221, 324);
            this.coverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.coverImage.TabIndex = 10;
            this.coverImage.TabStop = false;
            this.coverImage.Paint += new System.Windows.Forms.PaintEventHandler(this.coverImage_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Save to:";
            // 
            // saveFolder
            // 
            this.saveFolder.Location = new System.Drawing.Point(308, 338);
            this.saveFolder.Name = "saveFolder";
            this.saveFolder.Size = new System.Drawing.Size(447, 22);
            this.saveFolder.TabIndex = 12;
            // 
            // saveTo
            // 
            this.saveTo.Location = new System.Drawing.Point(761, 338);
            this.saveTo.Name = "saveTo";
            this.saveTo.Size = new System.Drawing.Size(27, 23);
            this.saveTo.TabIndex = 13;
            this.saveTo.Text = "...";
            this.saveTo.UseVisualStyleBackColor = true;
            this.saveTo.Click += new System.EventHandler(this.saveTo_Click);
            // 
            // currentCount
            // 
            this.currentCount.Location = new System.Drawing.Point(684, 443);
            this.currentCount.Name = "currentCount";
            this.currentCount.Size = new System.Drawing.Size(40, 23);
            this.currentCount.TabIndex = 14;
            this.currentCount.Text = "0";
            this.currentCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalCount
            // 
            this.totalCount.Location = new System.Drawing.Point(731, 443);
            this.totalCount.Name = "totalCount";
            this.totalCount.Size = new System.Drawing.Size(40, 23);
            this.totalCount.TabIndex = 15;
            this.totalCount.Text = "0";
            this.totalCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(722, 446);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "/";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 484);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.totalCount);
            this.Controls.Add(this.currentCount);
            this.Controls.Add(this.saveTo);
            this.Controls.Add(this.saveFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.coverImage);
            this.Controls.Add(this.chaptersCount);
            this.Controls.Add(this.chaptersLabel);
            this.Controls.Add(this.description);
            this.Controls.Add(this.title);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.saveEpub);
            this.Controls.Add(this.log);
            this.Controls.Add(this.search);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.url);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Light Novel EPUB";
            ((System.ComponentModel.ISupportInitialize)(this.coverImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Button saveEpub;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label chaptersLabel;
        private System.Windows.Forms.Label chaptersCount;
        private System.Windows.Forms.PictureBox coverImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox saveFolder;
        private System.Windows.Forms.Button saveTo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label currentCount;
        private System.Windows.Forms.Label totalCount;
        private System.Windows.Forms.Label label4;
    }
}

