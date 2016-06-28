using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Query
{
    public class QueryInterpreter
    {
        public QueryInterpreter()
        {
            Initialize();
        }

        public QueryInterpreter(string sentence)
        {
            Initialize();
            Interpret(sentence);
        }

        private void Initialize()
        {
            IsInterpreted = false;
            DesiredParameter = null;
            Words = new List<Word>();
        }

        public void Interpret(string sentence)
        {
            if (MorfParse(sentence))
            {
                IsInterpreted = true;
                PrepareWords(-1);
            }
        }

        public bool IsInterpreted { get; set; }
        public string DesiredParameter { get; set; }
        public int DesiredParameterIndex { get; set; }

        public string AsSingleString
        {
            get
            {
                if (IsInterpreted)
                    return string.Join(" ", Words);
                else
                    return "ERROR";
            }
        }

        public string AsStringOfBase
        {
            get
            {
                if (IsInterpreted)
                    return string.Join(" ", Words.Select(w => w.FormBase));
                else
                    return "ERROR";
            }
        }

        override public string ToString()
        {
            return AsSingleString;
        }

        public MorfeuszDllWrapper.InterpMorf[] RawInput { get; protected set; }
        
        public List<Word> Words { get; protected set; }

        private void PrepareWords(int _DesiredParameterIndex)
        {
            if (!IsInterpreted)
                return;
            else
            {
                Words.Clear();
                foreach(var item in RawInput)
                {
                    Word word = new Word(item);
                    if (word.PartOfSpeech == Word.SpeechPart.Numeral
                            && Words.Count > 0
                            && Words.Last().PartOfSpeech == Word.SpeechPart.Numeral)
                    {   // Wartość liczbowa występująca po sobie
                        Word lastWord = Words.Last();
                        Words.Remove(Words.Last());
                        word.FormBase = lastWord.FormBase + word.FormBase;
                        word.Form = lastWord.Form + word.Form;
                        Words.Add(word);
                    }
                    else if (Words.Count == 0 || Words.Last().k < item.k)
                    {   // Dodaj słowo jeśli nie istnieje na liście słów
                        Words.Add(word);
                    }
                    else if (word.PartOfSpeech == Word.SpeechPart.Numeral)
                    {   // Słowo już istnieje na liście, jednak to wystąpienie jest prawdopodobnie liczebnikiem
                        Words.Remove(Words.Last());
                        Words.Add(word);
                    }
                    else if (word.PartOfSpeech == Word.SpeechPart.Noun
                        && !(Words.Count > 1 && Words.Last().PartOfSpeech == Word.SpeechPart.Numeral))
                    {   // Słowo istnieje na liście, jednak to wystąpienie jest najprawdopodobniej rzeczowniwiem
                        if (word.Case == "nom"
                            && (_DesiredParameterIndex == item.k || _DesiredParameterIndex == -1))
                        {   // Mianownik oraz poszukiwane słowo
                            DesiredParameterIndex = _DesiredParameterIndex = item.k;
                            DesiredParameter = word.FormBase;
                            Words.Remove(Words.Last());
                            Words.Add(word);
                        }
                        else if (word.Case == "acc")
                        {   // Biernik
                            Words.Remove(Words.Last());
                            Words.Add(word);
                        }
                        else
                        {   // Inny przypadek

                        }
                    }
                    else if (word.PartOfSpeech == Word.SpeechPart.Conjuctiun
                            && Words.Count > 1
                            && Words[Words.Count - 2].PartOfSpeech == Word.SpeechPart.Noun)
                    {   // Słowo już istnieje na liście, jednak to wystąpienie jest prawdopodobnie spójnikiem, a nie inną częścią zdania
                        Words.Remove(Words.Last());
                        Words.Add(word);
                    }
                    else
                    {   // Słowo już istnieje na liście

                    }
                }
            }
        }

        private bool MorfParse(string Query)
        {
            try
            {
                DesiredParameter = null;
                RawInput = MorfeuszDllWrapper.ParseQuery(Query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
