using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryIntegrator.Utility
{
    internal static class PartOfSpeech
    {
        static string[] partOfSpeeches = new string[] { 
            "noun", 
            "phrasal verb", 
            "verb", 
            "phrase", 
            "adjective", 
            "adverb", 
            "pronoun", 
            "preposition", 
            "conjunction", 
            "determiner", 
            "exclamation", 
            "idiom" };
        public static int FindPosition(string input)
        {
            int position = -1;
            foreach(var partOfSpeech in partOfSpeeches)
            {
                if (input.Contains(partOfSpeech))
                {
                    int partOfSpeechPosition = input.IndexOf(partOfSpeech);
                    if (position == -1 || partOfSpeechPosition < position)
                    {
                        position = partOfSpeechPosition;
                    }
                }
            }
            return position;
        }
    }
}
