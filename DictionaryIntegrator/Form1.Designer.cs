namespace DictionaryIntegrator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tbxWord = new TextBox();
            rtbxSearchResult = new RichTextBox();
            btnSearch = new Button();
            btnInsertPage = new Button();
            lbxNotionWords = new ListBox();
            lbxNotionBlocks = new ListBox();
            btnInsertMeaning = new Button();
            btnInsertSentence = new Button();
            cbxPartOfSpeech = new ComboBox();
            notionWordsLoadingPic = new PictureBox();
            notionBlockLoadingPic = new PictureBox();
            pnlSearch = new Panel();
            lbxMeaning = new ListBox();
            rtbxSubtitles = new RichTextBox();
            lblTag = new Label();
            tbxFindInSubtitles = new TextBox();
            btnFindInSubtitles = new Button();
            tbxTag = new TextBox();
            btnFetch = new Button();
            btnCopy = new Button();
            btnSelect = new Button();
            pnlSubtitles = new Panel();
            btnSwitch = new Button();
            btnSetting = new Button();
            ((System.ComponentModel.ISupportInitialize)notionWordsLoadingPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)notionBlockLoadingPic).BeginInit();
            pnlSearch.SuspendLayout();
            pnlSubtitles.SuspendLayout();
            SuspendLayout();
            // 
            // tbxWord
            // 
            tbxWord.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            tbxWord.Location = new Point(11, 9);
            tbxWord.Margin = new Padding(2);
            tbxWord.Name = "tbxWord";
            tbxWord.Size = new Size(305, 31);
            tbxWord.TabIndex = 0;
            tbxWord.KeyDown += tbxWord_KeyDown;
            // 
            // rtbxSearchResult
            // 
            rtbxSearchResult.Location = new Point(11, 304);
            rtbxSearchResult.Margin = new Padding(2);
            rtbxSearchResult.Name = "rtbxSearchResult";
            rtbxSearchResult.Size = new Size(387, 251);
            rtbxSearchResult.TabIndex = 1;
            rtbxSearchResult.Text = "";
            rtbxSearchResult.Leave += rtbxSearchResult_Leave;
            rtbxSearchResult.MouseUp += rtbxSearchResult_MouseUp;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(320, 7);
            btnSearch.Margin = new Padding(2);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(78, 37);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnInsertPage
            // 
            btnInsertPage.Location = new Point(403, 8);
            btnInsertPage.Margin = new Padding(2);
            btnInsertPage.Name = "btnInsertPage";
            btnInsertPage.Size = new Size(78, 37);
            btnInsertPage.TabIndex = 12;
            btnInsertPage.Text = "Insert page";
            btnInsertPage.UseVisualStyleBackColor = true;
            btnInsertPage.Click += btnInsert_Click;
            // 
            // lbxNotionWords
            // 
            lbxNotionWords.BackColor = SystemColors.Window;
            lbxNotionWords.FormattingEnabled = true;
            lbxNotionWords.ItemHeight = 15;
            lbxNotionWords.Location = new Point(403, 50);
            lbxNotionWords.Margin = new Padding(2);
            lbxNotionWords.Name = "lbxNotionWords";
            lbxNotionWords.Size = new Size(404, 139);
            lbxNotionWords.TabIndex = 15;
            lbxNotionWords.SelectedIndexChanged += lbxNotionWords_SelectedIndexChanged;
            // 
            // lbxNotionBlocks
            // 
            lbxNotionBlocks.DrawMode = DrawMode.OwnerDrawVariable;
            lbxNotionBlocks.FormattingEnabled = true;
            lbxNotionBlocks.HorizontalScrollbar = true;
            lbxNotionBlocks.ItemHeight = 15;
            lbxNotionBlocks.Location = new Point(402, 198);
            lbxNotionBlocks.Margin = new Padding(2);
            lbxNotionBlocks.Name = "lbxNotionBlocks";
            lbxNotionBlocks.Size = new Size(404, 349);
            lbxNotionBlocks.TabIndex = 16;
            lbxNotionBlocks.DrawItem += lbxNotionBlocks_DrawItem;
            lbxNotionBlocks.MeasureItem += lbxNotionBlocks_MeasureItem;
            // 
            // btnInsertMeaning
            // 
            btnInsertMeaning.Location = new Point(683, 222);
            btnInsertMeaning.Margin = new Padding(2);
            btnInsertMeaning.Name = "btnInsertMeaning";
            btnInsertMeaning.Size = new Size(106, 38);
            btnInsertMeaning.TabIndex = 19;
            btnInsertMeaning.Text = "Insert meaning";
            btnInsertMeaning.UseVisualStyleBackColor = true;
            btnInsertMeaning.Click += btnInsertMeaning_Click;
            // 
            // btnInsertSentence
            // 
            btnInsertSentence.Location = new Point(683, 296);
            btnInsertSentence.Margin = new Padding(2);
            btnInsertSentence.Name = "btnInsertSentence";
            btnInsertSentence.Size = new Size(106, 37);
            btnInsertSentence.TabIndex = 20;
            btnInsertSentence.Text = "Insert sentence";
            btnInsertSentence.UseVisualStyleBackColor = true;
            btnInsertSentence.Click += btnInsertSentence_Click;
            // 
            // cbxPartOfSpeech
            // 
            cbxPartOfSpeech.FormattingEnabled = true;
            cbxPartOfSpeech.Location = new Point(11, 50);
            cbxPartOfSpeech.Name = "cbxPartOfSpeech";
            cbxPartOfSpeech.Size = new Size(387, 23);
            cbxPartOfSpeech.TabIndex = 21;
            cbxPartOfSpeech.SelectedIndexChanged += cbxPartOfSpeech_SelectedIndexChanged;
            // 
            // notionWordsLoadingPic
            // 
            notionWordsLoadingPic.BackColor = Color.Transparent;
            notionWordsLoadingPic.Image = (Image)resources.GetObject("notionWordsLoadingPic.Image");
            notionWordsLoadingPic.Location = new Point(540, 65);
            notionWordsLoadingPic.Name = "notionWordsLoadingPic";
            notionWordsLoadingPic.Size = new Size(138, 111);
            notionWordsLoadingPic.SizeMode = PictureBoxSizeMode.StretchImage;
            notionWordsLoadingPic.TabIndex = 22;
            notionWordsLoadingPic.TabStop = false;
            // 
            // notionBlockLoadingPic
            // 
            notionBlockLoadingPic.BackColor = Color.Transparent;
            notionBlockLoadingPic.Image = (Image)resources.GetObject("notionBlockLoadingPic.Image");
            notionBlockLoadingPic.Location = new Point(540, 291);
            notionBlockLoadingPic.Name = "notionBlockLoadingPic";
            notionBlockLoadingPic.Size = new Size(138, 111);
            notionBlockLoadingPic.SizeMode = PictureBoxSizeMode.StretchImage;
            notionBlockLoadingPic.TabIndex = 23;
            notionBlockLoadingPic.TabStop = false;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(btnSetting);
            pnlSearch.Controls.Add(lbxMeaning);
            pnlSearch.Controls.Add(btnInsertPage);
            pnlSearch.Controls.Add(btnInsertMeaning);
            pnlSearch.Controls.Add(rtbxSearchResult);
            pnlSearch.Controls.Add(cbxPartOfSpeech);
            pnlSearch.Controls.Add(notionBlockLoadingPic);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(notionWordsLoadingPic);
            pnlSearch.Controls.Add(btnInsertSentence);
            pnlSearch.Controls.Add(tbxWord);
            pnlSearch.Controls.Add(lbxNotionWords);
            pnlSearch.Controls.Add(lbxNotionBlocks);
            pnlSearch.Dock = DockStyle.Right;
            pnlSearch.Location = new Point(374, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(818, 566);
            pnlSearch.TabIndex = 26;
            // 
            // lbxMeaning
            // 
            lbxMeaning.DrawMode = DrawMode.OwnerDrawVariable;
            lbxMeaning.FormattingEnabled = true;
            lbxMeaning.ItemHeight = 15;
            lbxMeaning.Location = new Point(11, 82);
            lbxMeaning.Name = "lbxMeaning";
            lbxMeaning.Size = new Size(387, 214);
            lbxMeaning.TabIndex = 24;
            lbxMeaning.DrawItem += lbxMeaning_DrawItem;
            lbxMeaning.MeasureItem += lbxMeaning_MeasureItem;
            lbxMeaning.SelectedIndexChanged += lbxMeaning_SelectedIndexChanged;
            // 
            // rtbxSubtitles
            // 
            rtbxSubtitles.Location = new Point(17, 94);
            rtbxSubtitles.Margin = new Padding(2);
            rtbxSubtitles.Name = "rtbxSubtitles";
            rtbxSubtitles.Size = new Size(338, 453);
            rtbxSubtitles.TabIndex = 5;
            rtbxSubtitles.Text = "";
            rtbxSubtitles.Leave += rtbxSubtitles_Leave;
            // 
            // lblTag
            // 
            lblTag.AutoSize = true;
            lblTag.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblTag.Location = new Point(10, 14);
            lblTag.Margin = new Padding(2, 0, 2, 0);
            lblTag.Name = "lblTag";
            lblTag.Size = new Size(43, 25);
            lblTag.TabIndex = 14;
            lblTag.Text = "Tag:";
            // 
            // tbxFindInSubtitles
            // 
            tbxFindInSubtitles.Location = new Point(17, 56);
            tbxFindInSubtitles.Name = "tbxFindInSubtitles";
            tbxFindInSubtitles.Size = new Size(228, 23);
            tbxFindInSubtitles.TabIndex = 24;
            tbxFindInSubtitles.Text = "00:00:00";
            tbxFindInSubtitles.KeyDown += tbxFindInSubtitles_KeyDown;
            // 
            // btnFindInSubtitles
            // 
            btnFindInSubtitles.Location = new Point(249, 56);
            btnFindInSubtitles.Name = "btnFindInSubtitles";
            btnFindInSubtitles.Size = new Size(106, 29);
            btnFindInSubtitles.TabIndex = 25;
            btnFindInSubtitles.Text = "Find";
            btnFindInSubtitles.UseVisualStyleBackColor = true;
            btnFindInSubtitles.Click += btnFindInSubtitles_Click;
            // 
            // tbxTag
            // 
            tbxTag.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            tbxTag.Location = new Point(51, 14);
            tbxTag.Margin = new Padding(2);
            tbxTag.Name = "tbxTag";
            tbxTag.Size = new Size(305, 31);
            tbxTag.TabIndex = 10;
            tbxTag.Text = "HIMYM S8E5 The autumn of break-ups";
            tbxTag.Leave += tbxTag_Leave;
            // 
            // btnFetch
            // 
            btnFetch.Location = new Point(256, 386);
            btnFetch.Margin = new Padding(2);
            btnFetch.Name = "btnFetch";
            btnFetch.Size = new Size(78, 76);
            btnFetch.TabIndex = 17;
            btnFetch.Text = "Fetch";
            btnFetch.UseVisualStyleBackColor = true;
            btnFetch.Click += btnFetch_Click;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(256, 466);
            btnCopy.Margin = new Padding(2);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(78, 74);
            btnCopy.TabIndex = 9;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(256, 304);
            btnSelect.Margin = new Padding(2);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(78, 70);
            btnSelect.TabIndex = 18;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // pnlSubtitles
            // 
            pnlSubtitles.Controls.Add(btnSelect);
            pnlSubtitles.Controls.Add(btnCopy);
            pnlSubtitles.Controls.Add(btnFetch);
            pnlSubtitles.Controls.Add(tbxTag);
            pnlSubtitles.Controls.Add(btnFindInSubtitles);
            pnlSubtitles.Controls.Add(tbxFindInSubtitles);
            pnlSubtitles.Controls.Add(lblTag);
            pnlSubtitles.Controls.Add(rtbxSubtitles);
            pnlSubtitles.Dock = DockStyle.Left;
            pnlSubtitles.Location = new Point(0, 0);
            pnlSubtitles.Name = "pnlSubtitles";
            pnlSubtitles.Size = new Size(368, 566);
            pnlSubtitles.TabIndex = 26;
            // 
            // btnSwitch
            // 
            btnSwitch.Location = new Point(0, 240);
            btnSwitch.Name = "btnSwitch";
            btnSwitch.Size = new Size(18, 112);
            btnSwitch.TabIndex = 26;
            btnSwitch.Text = ">";
            btnSwitch.UseVisualStyleBackColor = true;
            btnSwitch.Click += btnSwitch_Click;
            // 
            // btnSetting
            // 
            btnSetting.BackgroundImage = Properties.Resources.setting;
            btnSetting.BackgroundImageLayout = ImageLayout.Zoom;
            btnSetting.Image = Properties.Resources.setting;
            btnSetting.Location = new Point(765, 10);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(42, 35);
            btnSetting.TabIndex = 25;
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 566);
            Controls.Add(btnSwitch);
            Controls.Add(pnlSearch);
            Controls.Add(pnlSubtitles);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "Dictionary";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)notionWordsLoadingPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)notionBlockLoadingPic).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlSubtitles.ResumeLayout(false);
            pnlSubtitles.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox tbxWord;
        private RichTextBox rtbxSearchResult;
        private Button btnSearch;
        private Button btnInsertPage;
        private ListBox lbxNotionWords;
        private ListBox lbxNotionBlocks;
        private Button btnInsertMeaning;
        private Button btnInsertSentence;
        private ComboBox cbxPartOfSpeech;
        private PictureBox notionWordsLoadingPic;
        private PictureBox notionBlockLoadingPic;
        private Panel pnlSearch;
        private RichTextBox rtbxSubtitles;
        private Label lblTag;
        private TextBox tbxFindInSubtitles;
        private Button btnFindInSubtitles;
        private TextBox tbxTag;
        private Button btnFetch;
        private Button btnCopy;
        private Button btnSelect;
        private Panel pnlSubtitles;
        private Button btnSwitch;
        private ListBox lbxMeaning;
        private Button btnSetting;
    }
}