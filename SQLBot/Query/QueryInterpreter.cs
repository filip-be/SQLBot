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
                    if(Words.Count == 0 || Words.Last().k < item.k)
                    {   // Dodaj słowo jeśli nie istnieje na liście słów
                        Words.Add(word);
                    }
                    else if(word.PartOfSpeech == Word.SpeechPart.Noun)
                    {   // Rzeczownik
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
