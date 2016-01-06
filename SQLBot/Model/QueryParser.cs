using AIMLbot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class QueryParser
    {
        protected Bot ChatBot = new Bot();
        protected const string UserId = "SQLBot";
        protected User ChatUser;

        public QueryParser()
        {
            ChatBot.loadSettings();
            ChatUser = new User(UserId, ChatBot);
            ChatBot.loadAIMLFromFiles();
            ChatBot.isAcceptingUserInput = true;
        }

        public unsafe string ParseQuery(string query)
        {
            string res;
            try
            {
                Cindalnet.SQLBot.Model.MorfeuszDllWrapper.InterpMorf []items = MorfeuszDllWrapper.ParseQuery(query);

                Request chatRequest = new Request(query, ChatUser, ChatBot);
                Result chatRes = ChatBot.Chat(chatRequest);
                res = chatRes.Output;

                //res = "OK";
            }catch(Exception ex)
            {
                res = "Wystąpił błąd podczas przetwarzania zapytania: " + ex.Message;
            }
            return res;
        }
    }
}
