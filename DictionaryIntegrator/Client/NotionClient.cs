using DictionaryIntegrator.Entity;
using DictionaryIntegrator.Properties;
using DictionaryIntegrator.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DictionaryIntegrator.Notion
{
    internal class NotionClient
    {
        private static readonly HttpClient client = new HttpClient();
        string newWordMeaning = string.Empty;
        enum medim
        {
            mp3,
            jpg
        }
        public NotionClient()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Default.ApiKey);
            client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
        }
        public async Task InsertPageAsync(string word, MeaningEntity meaningEntity, string tag, int retryCounter = 0)
        {
            if (retryCounter > 5)
            {
                throw new Exception("Failed to insert page");
            }
            var url = "https://api.notion.com/v1/pages";
            dynamic data = new ExpandoObject();
            data.parent = new
            {
                database_id = Settings.Default.DatabaseId
            };
            var properties = new ExpandoObject() as IDictionary<string, Object>;

            properties["Name"] = new
            {
                title = new[]
                {
                    new
                    {
                        text = new
                        {
                            content = word
                        }
                    }
                }
            };

            properties["Updated"] = new
            {
                date = new
                {
                    start = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss-08:00")
                }
            };

            if (!string.IsNullOrEmpty(tag))
            {
                // Dynamically add the Tags property
                properties["Tags"] = new
                {
                    multi_select = new[]
                    {
                        new { name = tag }
                    }
                };
            }

            data.properties = properties;
            data.children = new List<dynamic>();
            if (!string.IsNullOrEmpty(meaningEntity.UsAudioUrl))
            {
                data.children.Add(NotionFormatter.AddEmbed(meaningEntity.UsAudioUrl));
            }
            if (!string.IsNullOrEmpty(meaningEntity.PictureURL))
            {
                data.children.Add(NotionFormatter.AddEmbed(meaningEntity.PictureURL));
            }
            data.children.Add(NotionFormatter.AddParagraph(meaningEntity.PartOfSpeech));
            if(!string.IsNullOrEmpty(meaningEntity.Pronunciation))
                data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Pronunciation));
            data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Meaning));
            if (!string.IsNullOrEmpty(meaningEntity.Translation))
                data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Translation));
            data.children.Add(NotionFormatter.AddHeading2("Collocation"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(NotionFormatter.AddHeading2("Synonym"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(NotionFormatter.AddHeading2("Related"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(
                NotionFormatter.AddHeading2(content: "example sentences", color: "blue_background")
            );
            foreach (var example in meaningEntity.Examples)
            {
                data.children.Add(NotionFormatter.AddBulletPoint(example.Trim('-').Trim(), meaningEntity.Word));
            }
            var json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await InsertPageAsync(word, meaningEntity, tag, retryCounter++).ConfigureAwait(false);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
            response.EnsureSuccessStatusCode();
        }

        private string FetchAudioOrImageUrl(medim medim)
        {
            // Check if the input is null or empty
            if (string.IsNullOrEmpty(this.newWordMeaning))
            {
                throw new ArgumentException("Input cannot be null or empty.", nameof(this.newWordMeaning));
            }

            // Split the input string by new lines
            var lines = this.newWordMeaning.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Find the line that contains the URL, assuming it's the one that starts with "http"
            //string url = lines.FirstOrDefault(line => line.Trim().EndsWith(medim.ToString())) ?? "";
            string url = string.Empty;
            for(int i=0; i<lines.Count; ++i)
            {
                if (lines[i].Trim().EndsWith(medim.ToString()))
                {
                    url = lines[i];
                    lines.RemoveAt(i);
                    break;
                }
            }
            this.newWordMeaning = string.Join("\n", lines);
            // Return the URL if found, otherwise return null or throw an exception based on your error handling strategy
            return url.Trim(); // Using ?.Trim() to ensure any leading or trailing whitespace is removed
        }


        public async Task<List<string>> FetchPages(string searchWord, Dictionary<string, string> pageIdTitlePair)
        {
            var url = $"https://api.notion.com/v1/databases/{Settings.Default.DatabaseId}/query";
            dynamic data = new ExpandoObject();
            data.page_size = 100;
            List<string> names = new List<string>();
            bool hasMore = true;
            string? nextCursor = null;
            data.filter = new
            {
                or = new dynamic[]
                {
                    new
                    {
                        property = "Name",
                        title = new
                        {
                            contains = searchWord
                        }
                    }
                }
            };
            pageIdTitlePair.Clear();
            while (hasMore)
            {
                if (nextCursor != null)
                {
                    data.start_cursor = nextCursor;
                }
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;

                var responseObject = JObject.Parse(responseBody);
                var results = responseObject["results"].Value<JArray>();
                foreach (var result in results)
                {
                    var name = result["properties"]["Name"]["title"][0]["plain_text"].Value<string>();
                    names.Add(name);
                    pageIdTitlePair.Add(name, result["id"].Value<string>());
                }
                hasMore = responseObject["has_more"].Value<bool>();
                nextCursor = responseObject["next_cursor"]?.Value<string>();
            }
            var sortedNames = names.OrderBy(name => Algorithm.LevenshteinDistance(name, searchWord)).ToList();
            if (!sortedNames.Any())
            {
                sortedNames.Add("No matching word found");
            }
            return sortedNames;
        }

        public async Task<Dictionary<string, string>> FetchPageById(string pageId)
        {
            var url = $"https://api.notion.com/v1/blocks/{pageId}/children";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseBody);
            var results = responseObject["results"].Value<JArray>();
            Dictionary<string, string> notionBlockContents = new();
            foreach (var result in results)
            {
                string content = string.Empty;
                if (result["paragraph"]?["rich_text"].HasValues == true)
                {
                    foreach (var rich_text in result["paragraph"]["rich_text"])
                    {
                        content += rich_text?["text"]?["content"]?.Value<string>();
                    }
                }
                else if (result["bulleted_list_item"]?["rich_text"].HasValues == true)
                {
                    foreach (var rich_text in result["bulleted_list_item"]?["rich_text"])
                    {
                        content += rich_text?["text"]?["content"]?.Value<string>();
                    }
                }
                else if (result["heading_2"]?["rich_text"].HasValues == true)
                {
                    foreach (var rich_text in result["heading_2"]?["rich_text"])
                    {
                        content += rich_text?["text"]?["content"]?.Value<string>();
                    }
                }
                else if (result["heading_3"]?["rich_text"].HasValues == true)
                {
                    foreach (var rich_text in result["heading_3"]?["rich_text"])
                    {
                        content += rich_text?["text"]?["content"]?.Value<string>();
                    }
                }
                if (!string.IsNullOrWhiteSpace(content))
                {
                    if (PartOfSpeech.FindPosition(content.ToLower()) != -1)
                    {
                        string randomString = Guid.NewGuid().ToString();
                        notionBlockContents.Add(randomString, " ");
                    }
                    notionBlockContents.Add(result["id"].Value<string>(), content);
                }
            }
            return notionBlockContents;
        }

        public async Task InsertMeaningBlock(MeaningEntity meaningEntity, string pageId, string blockId, int retryCounter = 0)
        {
            if (retryCounter > 5)
            {
                throw new Exception("Failed to insert meaning block");
            }
            var url = $"https://api.notion.com/v1/blocks/{pageId}/children";
            dynamic data = new ExpandoObject();
            data.children = new List<dynamic>();
            //string audioUrl = FetchAudioOrImageUrl(medim.mp3);
            if (!string.IsNullOrEmpty(meaningEntity.UsAudioUrl))
            {
                data.children.Add(NotionFormatter.AddEmbed(meaningEntity.UsAudioUrl));
            }
            //string imageUrl = FetchAudioOrImageUrl(medim.jpg);
            if (!string.IsNullOrEmpty(meaningEntity.PictureURL))
            {
                data.children.Add(NotionFormatter.AddEmbed(meaningEntity.PictureURL));
            }
            data.children.Add(NotionFormatter.AddParagraph(meaningEntity.PartOfSpeech));
            if (!string.IsNullOrEmpty(meaningEntity.Pronunciation))
                data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Pronunciation));
            data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Meaning));
            if (!string.IsNullOrEmpty(meaningEntity.Translation))
                data.children.Add(NotionFormatter.AddParagraph(meaningEntity.Translation));
            data.children.Add(NotionFormatter.AddHeading2("Collocation"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(NotionFormatter.AddHeading2("Synonym"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(NotionFormatter.AddHeading2("Related"));
            data.children.Add(NotionFormatter.AddBulletPoint(""));
            data.children.Add(NotionFormatter.AddHeading2(content: "example sentences", color: "blue_background"));
            data.after = blockId;
            //var examples = newWordExamples.Trim().Split("\n");
            foreach (var example in meaningEntity.Examples)
            {
                data.children.Add(NotionFormatter.AddBulletPoint(example.Trim('-').Trim(), meaningEntity.Word));
            }
            var json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                InsertMeaningBlock(meaningEntity, pageId, blockId, retryCounter++);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
            response.EnsureSuccessStatusCode();
        }
        public async Task InsertSentenceBlock(string word, string newWordExamples, string pageId, string blockId, int retryCounter = 0)
        {
            if (retryCounter > 5)
            {
                throw new Exception("Failed to insert sentence block");
            }
            var url = $"https://api.notion.com/v1/blocks/{pageId}/children";
            dynamic data = new ExpandoObject();
            data.children = new List<dynamic>();
            data.after = blockId;
            var examples = newWordExamples.Trim().Split("\n");
            foreach (var example in examples)
            {
                data.children.Add(NotionFormatter.AddBulletPoint(example.Trim('-').Trim(), word));
            }
            var json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await InsertSentenceBlock(word, newWordExamples, pageId, blockId, retryCounter++);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task EditProperties(string pageId, string newTag, int retryCounter = 0)
        {
            if (string.IsNullOrEmpty(newTag))
            {
                return;
            }
            if (retryCounter > 5)
            {
                throw new Exception("Failed to insert sentence block");
            }
            var url = $"https://api.notion.com/v1/pages/{pageId}/properties/Tags";
            var getRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var getResponse = await client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseBody);
            var tags = responseObject["multi_select"].Value<JArray>();
            List<string> tagNames = new List<string>();
            foreach (var tag in tags)
            {
                tagNames.Add(tag["name"].Value<string>());
            }
            tagNames.Add(newTag);
            url = $"https://api.notion.com/v1/pages/{pageId}";
            dynamic data = new ExpandoObject();
            data.properties = new
            {
                Tags = new
                {
                    multi_select = tagNames.Select(tagName => new { name = tagName }).ToArray()
                },
                Updated = new
                {
                    date = new
                    {
                        start = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss-08:00")
                    }
                }
            };
            var json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await EditProperties(pageId, newTag, retryCounter++);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
