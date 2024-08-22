using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryIntegrator.Entity
{
    internal class MeaningEntity
    {
        public MeaningEntity(string word) {
            this.Word = word;
            this.PartOfSpeech = string.Empty;
            this.Examples = new List<string>();
        }
        public MeaningEntity(MeaningEntity meaningEntity)
        {
            this.Word = meaningEntity.Word;
            this.Meaning = meaningEntity.Meaning;
            this.Translation = meaningEntity.Translation;
            this.PartOfSpeech = meaningEntity.PartOfSpeech;
            this.UsAudioUrl = meaningEntity.UsAudioUrl;
            this.Pronunciation = meaningEntity.Pronunciation;
            this.PictureURL = meaningEntity.PictureURL;
            this.Examples = new List<string>();
            foreach (var example in meaningEntity.Examples)
            {
                this.Examples.Add(example);
            }
        }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string Translation { get; set; }
        public string PartOfSpeech { get; set; }
        public string UsAudioUrl { get; set; }
        public string Pronunciation { get; set; }
        public string PictureURL { get; set; }
        public List<string> Examples { get; set; }
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(this.Word);
            stringBuilder.AppendLine(this.PartOfSpeech);
            if (!string.IsNullOrEmpty(this.Pronunciation))
            {
                stringBuilder.AppendLine(this.Pronunciation);
            }
            if (!string.IsNullOrEmpty(this.UsAudioUrl))
            {
                stringBuilder.AppendLine(this.UsAudioUrl);
            }
            stringBuilder.AppendLine(this.Meaning);
            if (!string.IsNullOrEmpty(this.Translation))
            {
                stringBuilder.AppendLine(this.Translation);
            }
            if(!string.IsNullOrEmpty(this.PictureURL))
            {
                stringBuilder.AppendLine(this.PictureURL);
            }
            stringBuilder.AppendLine("## Collocation");
            stringBuilder.AppendLine("-");
            stringBuilder.AppendLine("### Synonym");
            stringBuilder.AppendLine("-");
            stringBuilder.AppendLine("### Related");
            stringBuilder.AppendLine("-");
            stringBuilder.AppendLine("## example sentences");
            foreach (var example in this.Examples)
            {
                stringBuilder.AppendLine($"- {example}");
            }
            stringBuilder.AppendLine("---");
            return stringBuilder.ToString();
        }
    }
}
