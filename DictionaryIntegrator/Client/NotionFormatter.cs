using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictionaryIntegrator.Notion
{
    internal class NotionFormatter
    {
        public static dynamic AddHeading2(string content, string? color = null)
        {
            dynamic heading2 = new ExpandoObject();
            heading2.@object = "block";
            heading2.type = "heading_2";
            heading2.heading_2 = new
            {
                rich_text = new[]
                {
                    new
                    {
                        type = "text",
                        text = new
                        {
                            content
                        }
                    }
                },
                color = color ?? "default"
            };
            return heading2;
        }

        public static dynamic AddParagraph(string content)
        {
            dynamic paragraph = new ExpandoObject();
            paragraph.@object = "block";
            paragraph.type = "paragraph";
            paragraph.paragraph = new
            {
                rich_text = new[]
                {
                    new
                    {
                        type = "text",
                        text = new
                        {
                            content
                        }
                    }
                }
            };
            return paragraph;
        }
        public static dynamic AddBulletPoint(string content)
        {
            dynamic bulletPoint = new ExpandoObject();
            bulletPoint.@object = "block";
            bulletPoint.type = "bulleted_list_item";
            bulletPoint.bulleted_list_item = new
            {
                rich_text = new[]
                {
                    new
                    {
                        type = "text",
                        text = new
                        {
                            content
                        }
                    }
                }
            };
            return bulletPoint;
        }
        public static dynamic AddBulletPoint(string content, string word)
        {
            dynamic bulletPoint = new ExpandoObject();
            bulletPoint.@object = "block";
            bulletPoint.type = "bulleted_list_item";
            bulletPoint.bulleted_list_item = new ExpandoObject();
            bulletPoint.bulleted_list_item.rich_text = new List<dynamic>();

            // Split the content by the word, including the word in the result
            var regexPattern = $"({Regex.Escape(word)})";
            string[] snippets = Regex.Split(content, regexPattern, RegexOptions.IgnoreCase);

            foreach (var snippet in snippets)
            {
                if (string.IsNullOrEmpty(snippet)) continue;

                if (snippet.Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    // Highlight the word
                    bulletPoint.bulleted_list_item.rich_text.Add(new
                    {
                        type = "text",
                        text = new
                        {
                            content = snippet
                        },
                        annotations = new
                        {
                            bold = true,
                            color = "yellow"
                        }
                    });
                }
                else
                {
                    // Add the snippet without highlighting
                    bulletPoint.bulleted_list_item.rich_text.Add(new
                    {
                        type = "text",
                        text = new
                        {
                            content = snippet
                        }
                    });
                }
            }
            return bulletPoint;
        }
        public static dynamic AddEmbed(string embedUrl)
        {
            dynamic child = new ExpandoObject();
            child.@object = "block";
            child.type = "embed";
            child.embed = new { url = embedUrl };
            return child;
        }
        public static dynamic AddImage(string imageUrl)
        {
            dynamic child = new ExpandoObject();
            child.@object = "block";
            child.type = "image";
            child.image = new
            {
                type = "external",
                external = new
                {
                    url = imageUrl
                }
            };
            return child;
        }
    }
}
