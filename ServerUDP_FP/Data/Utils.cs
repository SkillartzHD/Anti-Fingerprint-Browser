using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp52.Data
{
    internal class Utils
    {
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public static bool InvalidCmds(string cmds)
        {
            string[] data = { "..", "/", "\\", ":" };

            foreach (var bad_char in data)
            {
                if (cmds.Contains(bad_char))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AllowUpdateCmds(string cmds)
        {
            string[] data = { ":", " --user-data-dir=", "--user-agent=", "--fontset=", "--base64=" };

            foreach (var bad_char in data)
            {
                if (cmds.Contains(bad_char))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool is_profile_exist(string dir , string buffer)
        {
            try
            {
                bool safe_close = false;
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(dir);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(buffer))
                    {
                        safe_close = true;
                    }
                }
                file.Close();

                if (safe_close == true)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

    }
}
