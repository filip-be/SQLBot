using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Query
{
    public class Word
    {
        public enum SpeechPart
        {
            /// <summary>
            /// Rzeczownik
            /// </summary>
            Noun,
            /// <summary>
            /// Przymiotnik
            /// </summary>
            Adjective,
            /// <summary>
            /// Przysłówek
            /// </summary>
            Adverb,
            /// <summary>
            /// Liczebnik
            /// </summary>
            Numeral,
            /// <summary>
            /// Zaimek
            /// </summary>
            Pronoun,
            /// <summary>
            /// Czasownik
            /// </summary>
            Verb,
            /// <summary>
            /// Predykatyw
            /// </summary>
            Predicative,
            /// <summary>
            /// Przyimek
            /// </summary>
            Preposition,
            /// <summary>
            /// Spójnik
            /// </summary>
            Conjuctiun,
            /// <summary>
            /// Kublik (partykuło-przysłówek), ciało obce nominalne, ciało obce luźne
            /// </summary>
            Other
        }

        /// <summary>
        /// Cześć mowy
        /// </summary>
        public SpeechPart PartOfSpeech { get; set; }

        /// <summary>
        /// Determine speech part
        /// </summary>
        /// <param name="speechPartName">short name of speech part</param>
        public void SetPartOfSpeech(string speechPartName)
        {
            switch (speechPartName)
            {
                case "subst":
                case "depr":
                    this.PartOfSpeech = SpeechPart.Noun;
                    break;
                case "adj":
                case "adja":
                case "adjp":
                    this.PartOfSpeech = SpeechPart.Adjective;
                    break;
                case "adv":
                    this.PartOfSpeech = SpeechPart.Adverb;
                    break;
                case "num":
                    this.PartOfSpeech = SpeechPart.Numeral;
                    break;
                case "ppron12":
                case "ppron3":
                case "siebie":
                    this.PartOfSpeech = SpeechPart.Pronoun;
                    break;
                case "fin":
                case "bedzie":
                case "aglt":
                case "praet":
                case "impt":
                case "imps":
                case "inf":
                case "pcon":
                case "pant":
                case "ger":
                case "pact":
                case "ppas":
                case "winien":
                    this.PartOfSpeech = SpeechPart.Verb;
                    break;
                case "pred":
                    this.PartOfSpeech = SpeechPart.Predicative;
                    break;
                case "prep":
                    this.PartOfSpeech = SpeechPart.Preposition;
                    break;
                case "conj":
                    this.PartOfSpeech = SpeechPart.Conjuctiun;
                    break;
                default:
                    this.PartOfSpeech = SpeechPart.Other;
                    break;
            };
        }

        /// <summary>
        /// Znacznik
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// Liczba
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Przypadek
        /// </summary>
        public string Case { get; set; }

        /// <summary>
        /// Rodzaj
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Hasło
        /// </summary>
        public string Form { get; set; }

        /// <summary>
        /// Forma podstawowa
        /// </summary>
        public string FormBase { get; set; }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>object form</returns>
        public override string ToString()
        {
            return Form;
        }

        public int k;
        public int p;

        /// <summary>
        /// Interpret word
        /// </summary>
        /// <param name="_interpMorf">interpreted parameters</param>
        public void Interpret(MorfeuszDllWrapper.InterpMorf _interpMorf)
        {
            this.k = _interpMorf.k;
            this.p = _interpMorf.p;
            this.Mark = _interpMorf.interp;

            string[] interpParams = _interpMorf.interp.Split(':');

            this.Form = _interpMorf.forma;
            this.FormBase = _interpMorf.haslo;

            if (interpParams.Length > 0)
            {
                this.SetPartOfSpeech(interpParams[0]);

                for(int parNum = 1; parNum < interpParams.Length; parNum++)
                {
                    string par = interpParams[parNum];
                    
                    switch(interpParams[parNum])
                    {
                        case "sg":
                            this.Number = interpParams[parNum];
                            break;
                        case "pl":
                            if(parNum == 1)
                                this.Number = interpParams[parNum];
                            else
                                this.Type =  interpParams[parNum];
                            break;
                        case "nom":
                            this.Case = interpParams[parNum];
                            break;
                        case "gen":
                            this.Case = interpParams[parNum];
                            break;
                        case "dat":
                            this.Case = interpParams[parNum];
                            break;
                        case "acc":
                            this.Case = interpParams[parNum];
                            break;
                        case "inst":
                            this.Case = interpParams[parNum];
                            break;
                        case "loc":
                            this.Case = interpParams[parNum];
                            break;
                        case "voc":
                            this.Case = interpParams[parNum];
                            break;
                        case "m1":
                            this.Type = interpParams[parNum];
                            break;
                        case "m2":
                            this.Type = interpParams[parNum];
                            break;
                        case "m3":
                            this.Type = interpParams[parNum];
                            break;
                        case "f":
                            this.Type = interpParams[parNum];
                            break;
                        case "n1":
                            this.Type = interpParams[parNum];
                            break;
                        case "n2":
                            this.Type = interpParams[parNum];
                            break;
                        //case "pl":
                        //    this.Type = interpParams[parNum];
                        //    break;
                        case "p1":
                            this.Type = interpParams[parNum];
                            break;
                        case "p2":
                            this.Type = interpParams[parNum];
                            break;
                        case "p3":
                            this.Type = interpParams[parNum];
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    
        public Word()
        {
        }

        public Word(MorfeuszDllWrapper.InterpMorf _interpMorf)
        {
            Interpret(_interpMorf);
        }
    }
}
