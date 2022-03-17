using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ConsoleApp52.Data.Utils;
using static ConsoleApp52.Emulation;

namespace ConsoleApp52.Data
{
    internal class Data_ID
    {
        private static string locate_dir = @"Data\";

        public static string CreateFingerprint(string profil_data,string id_login,int update,int index)
        {
            string pure_finger = null;

            try
            {
                string dir_real = locate_dir + id_login + ".txt";

                if ((update == 0 ? InvalidCmds(profil_data) : AllowUpdateCmds(profil_data)) || InvalidCmds(id_login))
                    return "Invalid characters";

                Console.WriteLine(dir_real);

                if (File.Exists(dir_real))
                {
                    if (update == 0)
                    {
                        pure_finger = _fingerprint_result_to_file(profil_data);
                    }
                    else
                    {
                        pure_finger = profil_data;
                    }

                    if (update == 0)
                    {

                        if (is_profile_exist(dir_real, profil_data) == true)
                        {
                            return "Acest profil deja exista";
                        }

                        object object_data = new Object();

                        lock (object_data)
                        {
                            File.AppendAllText(dir_real, pure_finger + Environment.NewLine);
                        }
                    }
                    else if(update == 1)
                    {
                        string[] arrLine = File.ReadAllLines(dir_real);
                        arrLine[index - 1] = profil_data;
                        File.WriteAllLines(dir_real, arrLine);

                    }
                    else
                    {
                        string[] arrLine = File.ReadAllLines(dir_real);
                        arrLine[index - 1] = null;
                        File.WriteAllLines(dir_real, arrLine);

                        string text = File.ReadAllText(dir_real);
                        string result = Regex.Replace(text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                        File.WriteAllText(dir_real, result);
                    }

                }

                pure_finger = pure_finger == null ? "Nu exista acest id : " + id_login : pure_finger;
            }
            catch
            {
                pure_finger = "Invalid Content";
            }

            return pure_finger;
        }
        public static string RegisterID(string buffer)
        {
            string result = null;
            string dir_real = locate_dir + buffer + ".txt";


            if (InvalidCmds(buffer))
                return "Invalid characters";

            if (Directory.Exists(locate_dir))
            {
                if (File.Exists(dir_real))
                {
                    result = "Id-ul \"" + buffer + "\" deja exista!|BAD";
                }
                else
                {
                    try
                    {
                        File.Create(@locate_dir + buffer + ".txt").Close();
                    }
                    catch (Exception ex)
                    {
                        result = ex.ToString();
                    }
                    result = "Id-ul " + buffer + " a fost inregistrat|OK";
                }
            }
            else
            {
                result = "\"Data\" nu exista.";
            }
            return result;
        }

        public static string LoginId(string buffer)
        {
            string dir_real = locate_dir + buffer + ".txt";

            if (InvalidCmds(buffer))
                return "Invalid characters";

            string full_data = null;

            if (Directory.Exists(locate_dir))
            {
                if (File.Exists(dir_real))
                {
                    try
                    {
                        string line2;
                        System.IO.StreamReader file2 = new System.IO.StreamReader(dir_real);
                        while ((line2 = file2.ReadLine()) != null)
                        {
                            full_data = full_data + ":" + "\n" + line2;
                        }
                        file2.Close();
                    }
                    catch (Exception ex)
                    {
                        full_data = ex.ToString();
                    }
                }
                else
                {
                    return "Nu exista acest id : " + buffer;
                }
            }
            else
            {
                full_data = "\"Data\" nu exista.";
            }
            //Console.WriteLine(full_data);
            return full_data == null ? "null_data" : full_data;
        }
    }
}
