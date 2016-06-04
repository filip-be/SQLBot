using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Query
{
    public class MorfeuszDllWrapper
    {
        public struct InterpMorf
        {
            public int p, k; /* number of start node and end node */
            public string forma; /* segment (token) */
            public string haslo; /* lemma */
            public string interp; /* morphosyntactic tag */
        };

        public enum MorfeuszOptions
        {
            Encoding = 1
        }

        public enum MorfeuszEncodings
        {
            Utf8 = 8,
            Iso88592 = 88592,
            Cp1250 = 1250,
            Cp852 = 852
        }

        [DllImport("morfeusz2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "morfeusz_about")]
        public static extern IntPtr MorfeuszAbout();

        [DllImport("morfeusz2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "morfeusz_set_option")]
        public static extern int MorfeuszSetOption(int option, int value);

        public static int SetEncoding(MorfeuszEncodings encoding)
        {
            return MorfeuszSetOption((int)MorfeuszOptions.Encoding, (int)encoding);
        }

        [DllImport("morfeusz2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "morfeusz_analyse")]
        static extern IntPtr MorfAnalyse(string tekst);

        public static InterpMorf[] ParseQuery(string text)
        {
            try
            {
                SetEncoding(MorfeuszEncodings.Cp1250);

                IntPtr morfStruct = MorfAnalyse(text);

                List<InterpMorf> morfList = new List<InterpMorf>(3);
                int itemSize = Marshal.SizeOf(typeof(InterpMorf));
                int offset = 0;

                InterpMorf interpMorf;
                try
                {
                    interpMorf = (InterpMorf)Marshal.PtrToStructure(
                                                (IntPtr)((int)morfStruct + (offset * itemSize)),
                                                typeof(InterpMorf));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                while (interpMorf.p != -1)
                {
                    offset++;
                    morfList.Add(interpMorf);
                    interpMorf = (InterpMorf)Marshal.PtrToStructure(
                                                (IntPtr)((int)morfStruct + (offset * itemSize)),
                                                typeof(InterpMorf));
                }

                return morfList.ToArray();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
