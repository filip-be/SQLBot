using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class MorfeuszDllWrapper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct CInterpMorf
        {
            public int p, k; /* number of start node and end node */
            public IntPtr forma; /* segment (token) */
            public IntPtr haslo; /* lemma */
            public IntPtr interp; /* morphosyntactic tag */
        };

        public struct InterpMorf
        {
            public int p, k; /* number of start node and end node */
            public string forma; /* segment (token) */
            public string haslo; /* lemma */
            public string interp; /* morphosyntactic tag */
        };

        [DllImport("MorfeuszWrapper", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern bool ParseQuery(out ItemsSafeHandle itemsHandle,
            out CInterpMorf* items, out int itemCount, [MarshalAs(UnmanagedType.LPStr)] string query);

        [DllImport("MorfeuszWrapper", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern bool ReleaseItems(IntPtr itemsHandle);

        private static unsafe ItemsSafeHandle ParseQuery(out CInterpMorf* items, out int itemsCount, string query)
        {
            ItemsSafeHandle itemsHandle;
            if (!ParseQuery(out itemsHandle, out items, out itemsCount, query))
            {
                throw new InvalidOperationException();
            }
            return itemsHandle;
        }

        public static unsafe InterpMorf[] ParseQuery(string query)
        {
            InterpMorf[] res = null;
            try
            {
                Cindalnet.SQLBot.Model.MorfeuszDllWrapper.CInterpMorf* items;
                int itemsCount;
                using (MorfeuszDllWrapper.ParseQuery(out items, out itemsCount, query))
                {
                    res = new InterpMorf[itemsCount];
                    for (int i = 0; i < itemsCount; i++)
                    {
                        try
                        {
                            res[i].p = items[i].p;
                            res[i].k = items[i].k;
                            res[i].forma = Marshal.PtrToStringAnsi(items[i].forma);
                            res[i].haslo = Marshal.PtrToStringAnsi(items[i].haslo);
                            res[i].interp = Marshal.PtrToStringAnsi(items[i].interp);
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
            catch (Exception)
            {
            }
            return res;
        }

        public class ItemsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public ItemsSafeHandle()
                : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return ReleaseItems(handle);
            }
        }
    }
}
