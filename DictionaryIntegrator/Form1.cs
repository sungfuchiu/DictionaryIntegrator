using System.Text.RegularExpressions;
using DictionaryIntegrator.Utility;
using DictionaryIntegrator.Notion;
using DictionaryIntegrator.Client;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security.Policy;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
using System.Reflection;
using DictionaryIntegrator.Entity;
using System.Collections.Generic;

namespace DictionaryIntegrator
{
    public partial class MainForm : Form
    {
        string subtitlesFileName = @"temp_subtitles.txt";
        string tagFileName = @"tag.txt";
        string savedSubtitles;
        string suffix = string.Empty;
        string tag = string.Empty;
        string tempExample = string.Empty;
        int subtitlesPreviousHighlightStart = 0;
        int subtitlesPreviousHighlightLength = 0;
        int searchResultPreviousHighlightStart = 0;
        int searchResultPreviousHighlightLength = 0;
        private int startLineIndex = -1;
        private int endLineIndex = -1;
        string newWordMeaning = string.Empty;
        string newWordExamples = string.Empty;
        private Dictionary<string, string> pageIdTitlePair = new Dictionary<string, string>();
        NotionClient notionClient;
        CambridgeClient fetcher;
        Dictionary<string, List<MeaningEntity>> contentCategorizedByPartOfSpeech = new();
        Dictionary<PictureBox, int> pictureBoxOffCounters = new();
        Dictionary<int, MeaningEntity> meaningDictionary = new();
        string cambridgeUrl = "https://dictionary.cambridge.org/dictionary/english/";
        string cambridgeChineseUrl = "https://dictionary.cambridge.org/dictionary/english-chinese-traditional/";
        enum pictureBoxToggle { On, Off };
        public MainForm()
        {
            fetcher = new CambridgeClient();
            InitializeComponent();
            readData();
            notionWordsLoadingPic.Hide();
            notionBlockLoadingPic.Hide();
            if(Properties.Settings.Default.ApiKey == null || Properties.Settings.Default.DatabaseId == null)
            {
                SettingForm settingForm = new SettingForm();
                settingForm.ShowDialog();
            }
            notionClient = new NotionClient();
        }

        private void readData()
        {
            try
            {
                if (File.Exists(subtitlesFileName))
                {
                    savedSubtitles = File.ReadAllText(subtitlesFileName);
                    rtbxSubtitles.Text = savedSubtitles;
                }
                if (File.Exists(tagFileName))
                {
                    tbxTag.Text = File.ReadAllText(tagFileName);
                    tag = tbxTag.Text;
                    setSuffix();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button clickedButton = sender as System.Windows.Forms.Button;
            if (clickedButton != null)
            {
                try
                {
                    clickedButton.Enabled = false;
                    LoadingPictureToggleSetting(notionWordsLoadingPic, lbxNotionWords, pictureBoxToggle.On);
                    string englishWord = tbxWord.Text;
                    this.contentCategorizedByPartOfSpeech = await fetcher.FetchData(englishWord, new List<string>() { cambridgeChineseUrl, cambridgeUrl });
                    //foreach (var keyValuePair in await fetcher.FetchData(englishWord, cambridgeUrl))
                    //{
                    //    this.contentCategorizedByPartOfSpeech.TryAdd(keyValuePair.Key, "");
                    //    this.contentCategorizedByPartOfSpeech[keyValuePair.Key] += keyValuePair.Value;
                    //}
                    cbxPartOfSpeech.DataSource = new BindingSource(contentCategorizedByPartOfSpeech?.Keys.ToList(), null);
                    clickedButton.Enabled = true;
                    lbxNotionWords.DataSource = await notionClient.FetchPages(tbxWord.Text, pageIdTitlePair);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    clickedButton.Enabled = true;
                    LoadingPictureToggleSetting(notionWordsLoadingPic, lbxNotionWords, pictureBoxToggle.Off);
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(fetcher.SubtitleExample);
        }

        private void rtbxSubtitles_Leave(object sender, EventArgs e)
        {
            if (savedSubtitles != rtbxSubtitles.Text)
            {
                try
                {
                    rtbxSubtitles.Text = StripExtraNewLine(rtbxSubtitles.Text);
                    File.WriteAllText(subtitlesFileName, rtbxSubtitles.Text);
                    savedSubtitles = rtbxSubtitles.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private string StripExtraNewLine(string input)
        {
            string removeNewLine = Regex.Replace(input, @"(\r\n|\r|\n){2,}", Environment.NewLine);
            return removeNewLine;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            int scrollPos = NativeMethods.GetScrollPos(rtbxSubtitles.Handle, NativeMethods.SB_VERT);
            Properties.Settings.Default.ScoreBarLocationY = scrollPos;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int savedScrollPos = Properties.Settings.Default.ScoreBarLocationY;
            NativeMethods.SetScrollPos(rtbxSubtitles.Handle, NativeMethods.SB_VERT, savedScrollPos, true);
            rtbxSubtitles.SelectionStart = savedScrollPos;
            rtbxSubtitles.ScrollToCaret();
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                btnInsertPage.Enabled = false;
                SafeExecuteAsync(async () => await notionClient.InsertPageAsync(tbxWord.Text, meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())], tag));
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request exception: {ex.Message}");
            }
            finally
            {
                btnInsertPage.Enabled = true;
            }
        }

        private void tbxTag_Leave(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(tagFileName, tbxTag.Text);
                tag = tbxTag.Text;
                setSuffix();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void setSuffix()
        {
            if (string.IsNullOrEmpty(tbxTag.Text))
            {
                return;
            }
            suffix = $"[{tbxTag.Text}]";
        }
        private string ConvertToNotionFormat(string input)
        {
            List<string> sentences = input.Split("\r\n".ToCharArray()).ToList();
            List<string> resultSentences = new List<string>();
            foreach (string sentence in sentences)
            {
                if (!String.IsNullOrWhiteSpace(sentence) && !char.IsDigit(sentence[0]) && sentence[0] != '=')
                {
                    string newSentence;
                    newSentence = sentence.Replace("<i>", "");
                    newSentence = newSentence.Replace("</i>", "");
                    resultSentences.Add(newSentence);
                }
            }
            string result = resultSentences.Aggregate((cur, next) => $"{cur} {next}");
            result += suffix;

            Clipboard.SetText(result);
            return result;
        }

        private void rtbxSearchResult_MouseUp(object sender, MouseEventArgs e)
        {
            if (rtbxSearchResult.SelectionLength > 0)
            {
                int highlightStart = rtbxSearchResult.SelectionStart;
                int highlightLength = rtbxSearchResult.SelectionLength;
                if (searchResultPreviousHighlightStart != 0 && searchResultPreviousHighlightLength != 0)
                {
                    rtbxSearchResult.SelectionStart = searchResultPreviousHighlightStart;
                    rtbxSearchResult.SelectionLength = searchResultPreviousHighlightLength;
                    rtbxSearchResult.SelectionBackColor = Color.White;
                }
                searchResultPreviousHighlightStart = highlightStart;
                searchResultPreviousHighlightLength = highlightLength;
                rtbxSearchResult.SelectionStart = searchResultPreviousHighlightStart;
                rtbxSearchResult.SelectionLength = searchResultPreviousHighlightLength;
                rtbxSearchResult.SelectionBackColor = Color.Yellow;
                //ConvertSearchResultToNotionFormat(rtbxSearchResult.SelectedText);
            }
        }

        //private void ConvertSearchResultToNotionFormat(string input)
        //{
        //    int start = PartOfSpeech.FindPosition(input);
        //    start = start > 0 ? start : 0;
        //    int end = input.IndexOf("## Collocation");
        //    if (end > 0)
        //    {
        //        if (end > start)
        //        {
        //            newWordMeaning = input.Substring(start, end - start);
        //        }
        //        else
        //        {
        //            newWordMeaning = input.Substring(end, start - end);
        //        }
        //    }
        //    string exampleTitle = "## example sentences";
        //    start = input.IndexOf(exampleTitle) + exampleTitle.Length;
        //    end = input.IndexOf("---");
        //    if (end > start)
        //    {
        //        newWordExamples = input.Substring(start, end - start);
        //    }
        //    else
        //    {
        //        newWordExamples = input;
        //    }
        //}

        private async void lbxNotionWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                notionBlockLoadingPic.Show();
                lbxNotionBlocks.Enabled = false;
                if (pageIdTitlePair.ContainsKey(lbxNotionWords.SelectedItem.ToString()))
                {
                    string pageId = pageIdTitlePair[lbxNotionWords.SelectedItem.ToString()];
                    var keyValuePairs = await notionClient.FetchPageById(pageId);
                    lbxNotionBlocks.DataSource = new BindingSource(await notionClient.FetchPageById(pageId), null);
                    lbxNotionBlocks.DisplayMember = "Value";
                    lbxNotionBlocks.ValueMember = "Key";
                }
                else
                {
                    lbxNotionBlocks.DataSource = null;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request exception: {ex.Message}");
            }
            finally
            {
                lbxNotionBlocks.Enabled = true;
                notionBlockLoadingPic.Hide();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (rtbxSubtitles.SelectionLength > 0)
            {
                int selectionStart = rtbxSubtitles.SelectionStart;
                int selectionEnd = selectionStart + rtbxSubtitles.SelectionLength;
                int lineStartIndex = rtbxSubtitles.GetFirstCharIndexOfCurrentLine();
                int lastLineStartIndex = rtbxSubtitles.GetFirstCharIndexFromLine(rtbxSubtitles.GetLineFromCharIndex(selectionEnd) + 1);
                if (lastLineStartIndex == selectionEnd && lastLineStartIndex != 0)
                {
                    lastLineStartIndex = rtbxSubtitles.GetFirstCharIndexFromLine(rtbxSubtitles.GetLineFromCharIndex(selectionEnd) - 1);
                }
                int lastLineLength = (lastLineStartIndex == rtbxSubtitles.TextLength)
                    ? rtbxSubtitles.Lines[rtbxSubtitles.GetLineFromCharIndex(lastLineStartIndex)].Length : 0;
                rtbxSubtitles.Select(lineStartIndex, (lastLineStartIndex - lineStartIndex) + lastLineLength);


                int highlightLength = rtbxSubtitles.SelectionLength;
                if (subtitlesPreviousHighlightStart != 0 && subtitlesPreviousHighlightLength != 0)
                {
                    rtbxSubtitles.SelectionStart = subtitlesPreviousHighlightStart;
                    rtbxSubtitles.SelectionLength = subtitlesPreviousHighlightLength;
                    rtbxSubtitles.SelectionBackColor = Color.White;
                }
                subtitlesPreviousHighlightStart = lineStartIndex;
                subtitlesPreviousHighlightLength = highlightLength;
                rtbxSubtitles.SelectionStart = subtitlesPreviousHighlightStart;
                rtbxSubtitles.SelectionLength = subtitlesPreviousHighlightLength;
                rtbxSubtitles.SelectionBackColor = Color.Yellow;
                fetcher.SubtitleExample = ConvertToNotionFormat(rtbxSubtitles.SelectedText);
                rtbxSubtitles.SelectionLength = 0;
                newWordExamples = string.Empty;
            }
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            if (rtbxSubtitles.SelectionLength > 0)
            {
                tbxWord.Text = rtbxSubtitles.SelectedText;
                tbxWord.Text = tbxWord.Text.Replace("\n", " ");
                tbxWord.Text = tbxWord.Text.Trim();
                btnSearch_Click(btnSearch, EventArgs.Empty);
                rtbxSubtitles.SelectionLength = 0;
            }
        }

        private void btnInsertMeaning_Click(object sender, EventArgs e)
        {
            string pageId = pageIdTitlePair[lbxNotionWords.SelectedItem.ToString()];
            string blockId = lbxNotionBlocks.SelectedValue.ToString();
            try
            {
                btnInsertMeaning.Enabled = false;
                SafeExecuteAsync(async () => await notionClient.InsertMeaningBlock(meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())], pageId, blockId));
                SafeExecuteAsync(async () => await notionClient.EditProperties(pageId, tag));
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"HTTP request exception: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Request exception: {ex.Message}");
            }
            finally
            {
                btnInsertMeaning.Enabled = true;
            }
        }

        private async void btnInsertSentence_Click(object sender, EventArgs e)
        {
            string pageId = pageIdTitlePair[lbxNotionWords.SelectedItem.ToString()];
            string blockId = lbxNotionBlocks.SelectedValue.ToString();

            try
            {
                btnInsertSentence.Enabled = false;
                SafeExecuteAsync(async () =>
                            await notionClient.InsertSentenceBlock(
                                                tbxWord.Text,
                                                string.IsNullOrEmpty(rtbxSearchResult.SelectedText) ? fetcher.SubtitleExample : rtbxSearchResult.SelectedText,
                                                pageId,
                                                blockId));
                SafeExecuteAsync(async () => await notionClient.EditProperties(pageId, tag));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Request exception: {ex.Message}");
            }
            finally
            {
                btnInsertSentence.Enabled = true;
            }
        }

        private void cbxPartOfSpeech_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lbxMeaning.DataSource = new BindingSource(contentCategorizedByPartOfSpeech[cbxPartOfSpeech.SelectedItem.ToString()], null);
            meaningDictionary = contentCategorizedByPartOfSpeech[cbxPartOfSpeech.SelectedItem.ToString()].ToDictionary(obj => obj.GetHashCode(), obj => obj);
            lbxMeaning.DisplayMember = "Value";
            lbxMeaning.ValueMember = "Key";
            lbxMeaning.DataSource = new BindingSource(meaningDictionary, null);
        }

        private void LoadingPictureToggleSetting(PictureBox pictureBox, ListBox listBox, pictureBoxToggle toggle)
        {
            pictureBoxOffCounters.TryAdd(pictureBox, 0);
            if (toggle == pictureBoxToggle.On)
            {
                pictureBoxOffCounters[pictureBox] += 1;
                pictureBox.Show();
                listBox.Enabled = false;
            }
            else
            {
                pictureBoxOffCounters[pictureBox] -= 1;
                if (pictureBoxOffCounters[pictureBox] == 0)
                {
                    pictureBox.Hide();
                    listBox.Enabled = true;
                }
            }
        }

        private void btnFindInSubtitles_Click(object sender, EventArgs e)
        {
            TimeSpan time = new TimeSpan();
            if (TimeSpan.TryParse(tbxFindInSubtitles.Text, out time))
            {
                for (int i = 0; i < 10; i++)
                {
                    time = time - new TimeSpan(0, 0, i);
                    string resultTimeString = time.ToString(@"hh\:mm\:ss");

                    if (rtbxSubtitles.Find(resultTimeString) != -1)
                    {
                        tbxFindInSubtitles.Text = resultTimeString;
                        break;
                    }
                }
            }
            // find the word after the current selection
            int index = rtbxSubtitles.Find(tbxFindInSubtitles.Text, rtbxSubtitles.SelectionStart + rtbxSubtitles.SelectionLength, RichTextBoxFinds.None); // Search for the word
            index = index == -1 ? rtbxSubtitles.Find(tbxFindInSubtitles.Text, 0, RichTextBoxFinds.None) : index;
            if (index != -1) // Check if the word was found
            {
                rtbxSubtitles.SelectionStart = index; // Set the start of the selection to the found index
                rtbxSubtitles.SelectionLength = tbxFindInSubtitles.Text.Length; // Highlight the word
                rtbxSubtitles.SelectionBackColor = Color.Yellow; // Highlight the word with yellow
                rtbxSubtitles.ScrollToCaret(); // Scroll to the selected text
            }
            else
            {
                MessageBox.Show($"The word '{tbxFindInSubtitles.Text}' was not found.", "Search Result");
            }
        }

        private void tbxWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                btnSearch_Click(btnSearch, EventArgs.Empty);
            }
        }

        private void lbxNotionBlocks_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var item = lbxNotionBlocks.Items[e.Index];
                PropertyInfo propInfo = item.GetType().GetProperty(lbxNotionBlocks.DisplayMember);
                string text = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                e.DrawBackground();
                e.DrawFocusRectangle();
                e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }

        private void lbxNotionBlocks_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var item = lbxNotionBlocks.Items[e.Index];
                PropertyInfo propInfo = item.GetType().GetProperty(lbxNotionBlocks.DisplayMember);
                string text = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                SizeF size = e.Graphics.MeasureString(text, lbxNotionBlocks.Font, lbxNotionBlocks.Width);
                e.ItemHeight = (int)size.Height;
            }
        }
        private async void SafeExecuteAsync(Func<Task> asyncOperation)
        {
            try
            {
                await asyncOperation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            // Assuming panelToToggle is the panel you want to show/hide
            pnlSubtitles.Visible = !pnlSubtitles.Visible;

            // Adjust the form's height based on the panel's visibility
            if (pnlSubtitles.Visible)
            {
                // Expand the form to its original size when the panel is shown
                this.Width += pnlSubtitles.Width;
                btnSwitch.Text = ">";
                suffix = string.Empty;
                tag = tbxTag.Text;
                savedSubtitles = rtbxSubtitles.Text;
                fetcher.SubtitleExample = tempExample;
            }
            else
            {
                // Shrink the form when the panel is hidden
                this.Width -= pnlSubtitles.Width;
                btnSwitch.Text = "<";
                setSuffix();
                tag = string.Empty;
                savedSubtitles = string.Empty;
                tempExample = fetcher.SubtitleExample;
                fetcher.SubtitleExample = string.Empty;
            }
        }

        private void lbxMeaning_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtbxSearchResult.Text = meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())].ToString();
        }

        private void lbxMeaning_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var item = lbxMeaning.Items[e.Index];
                PropertyInfo propInfo = item.GetType().GetProperty(lbxMeaning.DisplayMember);
                string text = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                SizeF size = e.Graphics.MeasureString(text, lbxMeaning.Font, lbxMeaning.Width);
                e.ItemHeight = (int)size.Height;
            }
        }

        private void lbxMeaning_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var item = lbxMeaning.Items[e.Index];
                PropertyInfo propInfo = item.GetType().GetProperty(lbxMeaning.DisplayMember);
                string text = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                e.DrawBackground();
                e.DrawFocusRectangle();
                e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }

        private void rtbxSearchResult_Leave(object sender, EventArgs e)
        {
            int meaningStart = PartOfSpeech.FindPosition(rtbxSearchResult.Text);
            meaningStart = meaningStart > 0 ? meaningStart : 0;
            int meaningEnd = rtbxSearchResult.Text.IndexOf("## Collocation");
            if (meaningEnd > 0)
            {
                if (meaningEnd > meaningStart)
                {
                    newWordMeaning = rtbxSearchResult.Text.Substring(meaningStart, meaningEnd - meaningStart);
                }
                else
                {
                    newWordMeaning = rtbxSearchResult.Text.Substring(meaningEnd, meaningStart - meaningEnd);
                }
                if (!string.IsNullOrEmpty(newWordMeaning) && newWordMeaning != meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())].Meaning)
                {
                    meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())].Meaning = newWordMeaning;
                }
                string exampleTitle = "## example sentences";
                int exampleStart = rtbxSearchResult.Text.IndexOf(exampleTitle) + exampleTitle.Length;
                int exampleEnd = rtbxSearchResult.Text.IndexOf("---");
                if (exampleStart > 0 && exampleEnd > 0)
                {
                    newWordExamples = rtbxSearchResult.Text.Substring(exampleStart, exampleEnd - exampleStart);
                    var examples = newWordExamples.Split('\n').ToList();
                    if (examples.Any())
                    {
                        foreach (var example in examples)
                        {
                            var trimmedExample = example.Trim('-').Trim();
                            if (!string.IsNullOrEmpty(example) && !meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())].Examples.Contains(trimmedExample))
                            {
                                meaningDictionary[Int32.Parse(lbxMeaning.SelectedValue.ToString())].Examples.Insert(0, trimmedExample);
                            }
                        }
                    }
                }
            }
        }

        private void tbxFindInSubtitles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnFindInSubtitles_Click(btnFindInSubtitles, EventArgs.Empty);
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            //Open another window to set the settings
            SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();
            notionClient = new NotionClient();
        }
    }
}