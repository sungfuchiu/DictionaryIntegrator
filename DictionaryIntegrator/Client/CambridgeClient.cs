using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using DictionaryIntegrator.Entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Collections.Specialized.BitVector32;

namespace DictionaryIntegrator.Client
{
    internal class CambridgeClient
    {
        private static readonly HttpClient client = new HttpClient();
        public string SubtitleExample = string.Empty;
        string host = string.Empty;
        public async Task<Dictionary<string, List<MeaningEntity>>> FetchData(string word, List<string> uriStrings)
        {
            List<MeaningEntity> meaningEntities = new();
            var stringBuilder = new StringBuilder();
            Dictionary<string, List<MeaningEntity>> contentCategorizedByPartOfSpeech = new();
            foreach (string uriString in uriStrings)
            {
                Uri uri = new Uri(uriString + word.Trim().Replace(" ", "-").ToLower());
                host = $"{uri.Scheme}://{uri.Host}";

                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(content);

                    var diBodies = htmlDoc.DocumentNode.SelectNodes("//*[@class='di-body']");
                    if (diBodies != null)
                    {
                        foreach (var diBody in diBodies)
                        {
                            List<HtmlNodeCollection> htmlNodes = new()
                            {
                                diBody.SelectNodes($".//div[contains(@class, 'entry-body__el')]"),
                                diBody.SelectNodes(".//div[@class='pr idiom-block']"),
                                diBody.SelectNodes(".//div[@class='pr phrase-block dphrase-block ']"),
                                diBody.SelectNodes(".//span[contains(@class, 'phrase-di-block')]"),
                            };
                            foreach (var htmlNode in htmlNodes)
                            {
                                if (htmlNode == null)
                                {
                                    continue;
                                }
                                MeaningEntity meaningEntity = new(word);
                                foreach (var entryBody in htmlNode)
                                {
                                    meaningEntity.Word = ReadTitle(entryBody) ?? word;
                                    meaningEntity.PartOfSpeech = ReadPartOfSpeech(entryBody);
                                    var usPronunciationNode = entryBody.SelectSingleNode("//span[@class='us dpron-i ']");
                                    var usAudioNode = usPronunciationNode?.SelectSingleNode(".//audio[@id='audio2']/source[@type='audio/mpeg']");
                                    if (usAudioNode != null)
                                    {
                                        meaningEntity.UsAudioUrl = host + usAudioNode.GetAttributeValue("src", string.Empty);
                                    }
                                    var pronunciationNode = usPronunciationNode?.SelectSingleNode(".//span[@class='pron dpron']");
                                    if (pronunciationNode != null)
                                    {
                                        meaningEntity.Pronunciation = pronunciationNode.InnerText;
                                    }
                                    List<HtmlNodeCollection> htmlSections = new()
                                    {
                                        entryBody.SelectNodes(".//div[starts-with(@class, 'pr dsense')]"),
                                        entryBody.SelectNodes(".//div[contains(@class, 'phrase-di-body')]"),
                                        entryBody.SelectNodes(".//div[contains(@class, 'phrase-body')]"),
                                        entryBody.SelectNodes(".//span[contains(@class, 'phrase-di-body')]"),
                                    };
                                    var sections = htmlSections.FirstOrDefault(s => s != null);
                                    if (sections != null)
                                    {
                                        foreach (var section in sections)
                                        {
                                            var body = section.SelectSingleNode(".//div[@class='sense-body dsense_b']");
                                            //var detailedPartOfSpeech = section.SelectSingleNode(".//h3[@class='dsense_h']");
                                            if (body == null)
                                            {
                                                body = section;
                                            }
                                            var blocks = body.SelectNodes("./div[@class='def-block ddef_block ']");
                                            if (blocks != null)
                                            {
                                                foreach (var block in blocks)
                                                {
                                                    var meaningEntityInBlock = new MeaningEntity(meaningEntity);
                                                    //if (detailedPartOfSpeech != null)
                                                    //{
                                                    //    meaningEntityInBlock.PartOfSpeech = StripEmptySpace(detailedPartOfSpeech.InnerText);
                                                    //}
                                                    var meaningNode = block.SelectSingleNode(".//div[@class='ddef_h']");
                                                    if (meaningNode != null)
                                                    {
                                                        meaningEntityInBlock.Meaning = meaningNode.InnerText;
                                                    }
                                                    var translationNode = block.SelectSingleNode(".//span[@class='trans dtrans dtrans-se  break-cj']");
                                                    if (translationNode != null)
                                                    {
                                                        meaningEntityInBlock.Translation = translationNode.InnerText;
                                                    }
                                                    var picture = block.SelectSingleNode(".//amp-img[@class='dimg_i hp']");
                                                    if (picture != null)
                                                    {
                                                        meaningEntityInBlock.PictureURL = new Uri(host + picture.GetAttributeValue("src", string.Empty)).GetLeftPart(UriPartial.Path);
                                                    }

                                                    var examples = block.SelectNodes(".//span[@class='eg deg']");
                                                    if (examples != null)
                                                    {
                                                        foreach (var examplesNode in examples)
                                                        {
                                                            meaningEntityInBlock.Examples.Add(examplesNode.InnerText);
                                                        }
                                                    }
                                                    var moreExamples = block.SelectNodes(".//li[@class='eg dexamp hax']");
                                                    if (moreExamples != null)
                                                    {
                                                        foreach (var moreExamplesNode in moreExamples)
                                                        {
                                                            meaningEntityInBlock.Examples.Add(moreExamplesNode.InnerText);
                                                        }
                                                    }
                                                    var targetMeaning = meaningEntities.FirstOrDefault(m =>
                                                    {
                                                        // Remove non-alphabet characters and make both strings lowercase for case-insensitive comparison
                                                        var cleanedM = Regex.Replace(m.Meaning, "[^a-zA-Z]", "").ToLower();
                                                        var cleanedMeaningEntityInBlock = Regex.Replace(meaningEntityInBlock.Meaning, "[^a-zA-Z]", "").ToLower();

                                                        // Check if either string contains the other
                                                        return cleanedM.Contains(cleanedMeaningEntityInBlock) || cleanedMeaningEntityInBlock.Contains(cleanedM);
                                                    });
                                                    if (targetMeaning == null)
                                                    {
                                                        meaningEntities.Add(new MeaningEntity(meaningEntityInBlock));
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(targetMeaning.PictureURL))
                                                        {
                                                            targetMeaning.PictureURL = meaningEntityInBlock.PictureURL;
                                                        }
                                                        if (string.IsNullOrEmpty(targetMeaning.Translation))
                                                        {
                                                            targetMeaning.Translation = meaningEntityInBlock.Translation;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception($"Error: {response.Content}");
                }
            }
            if(meaningEntities.Count == 0)
            {
                meaningEntities.Add(new MeaningEntity(word));
            }
            foreach (var meaningEntity in meaningEntities)
            {
                meaningEntity.Examples.Add(SubtitleExample);
                contentCategorizedByPartOfSpeech.TryAdd(meaningEntity.PartOfSpeech, new());
                contentCategorizedByPartOfSpeech[meaningEntity.PartOfSpeech].Add(meaningEntity);
            }
            //return contentCategorizedByPartOfSpeech.ToDictionary(kvp => kvp.Key, kvp => string.Join("\n", kvp.Value));
            return contentCategorizedByPartOfSpeech;
        }

        private string? ReadTitle(HtmlNode parent)
        {
            var titleNode = parent.SelectSingleNode(".//div[@class='di-title']");
            if (titleNode == null)
            {
                titleNode = parent.SelectSingleNode(".//div[@class='phrase-head dphrase_h']");
            }
            return titleNode?.InnerText;
        }

        private string ReadPartOfSpeech(HtmlNode parent)
        {
            string partOfSpeech;
            var biggerPartOfSpeechNode = parent.SelectSingleNode($".//div[starts-with(@class, 'posgram')]");
            var partOfSpeechNode = parent.SelectSingleNode(".//span[@class='anc-info-head danc-info-head']");
            if (biggerPartOfSpeechNode != null)
            {
                partOfSpeech = biggerPartOfSpeechNode.InnerText;
            }
            else if (partOfSpeechNode != null)
            {
                partOfSpeech = partOfSpeechNode.InnerText;
            }
            else
            {
                partOfSpeech = "phrase";
            }
            var usage = parent.SelectSingleNode(".//span[@class='usage dusage']");
            if (usage != null)
            {
                partOfSpeech += " ";
                partOfSpeech += usage.InnerText;
            }
            var alternative = parent.SelectSingleNode(".//span[@class='spellvar dspellvar']");
            if (alternative != null)
            {
                partOfSpeech += " ";
                partOfSpeech += alternative.InnerText;
            }
            return partOfSpeech;
        }
        private string StripEmptySpace(string html)
        {
            string removeSpace = Regex.Replace(html, @"\s+", " ");
            return removeSpace;
        }
    }
}
