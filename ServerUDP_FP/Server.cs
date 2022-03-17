using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp52.Emulation;
using static ConsoleApp52.Data.LoadData;
using static ConsoleApp52.Data.Data_ID;
using static ConsoleApp52.Data.Utils;
using static ConsoleApp52.Data.Fonts;
using static ConsoleApp52.Data.Navigator;

using Console = Colorful.Console;
using Leaf.xNet;
using System.IO;

namespace ConsoleApp52
{
    internal class Server
    {

        public static void BuildServer()
        {

            try
            {
                GpuList();

                string cmd_register = "register_data";
                string cmd_login = "login_data";
                string cmd_newfingerprint = "new_fingerprint";
                string cmd_updatefingerprint = "update";
                string cmd_delfingerprint = "delete";

                byte[] data = new byte[1024];
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5000);
                UdpClient newsock = new UdpClient(ipep);

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                string DataRecive = Encoding.ASCII.GetString(data, 0, data.Length);

                while (true)
                {
                    data = newsock.Receive(ref sender);

                    DataRecive = Encoding.ASCII.GetString(data, 0, data.Length);

                    Console.WriteLine(DataRecive.ToString());

                    byte[] arg_data;

                    if (DataRecive.Contains("engine"))
                    {

                        if (DataRecive.Contains("font")) // engine login_data asdasda |
                        {
                            string fonts = _compress_data_(FontsGenerator(1));
                            arg_data = Encoding.ASCII.GetBytes(fonts);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }
                        else if (DataRecive.Contains("static_fon")) // engine login_data asdasda |
                        {
                            string screen = StaticFont();
                            arg_data = Encoding.ASCII.GetBytes(screen);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }
                        else if (DataRecive.Contains("screen")) // engine login_data asdasda |
                        {
                            string screen = GetAllScreenData();
                            arg_data = Encoding.ASCII.GetBytes(screen);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }

                        else if (DataRecive.Contains("canvas")) // engine login_data asdasda |
                        {
                            string canvas = ArgumentEmulation(20000).ToString();
                            arg_data = Encoding.ASCII.GetBytes(canvas);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }

                        else if (DataRecive.Contains("audiocontext")) // engine login_data asdasda |
                        {
                            string audiocontext = ArgumentEmulation(20000).ToString();
                            arg_data = Encoding.ASCII.GetBytes(audiocontext);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }
                        else if (DataRecive.Contains("wid_webgl")) // engine login_data asdasda |
                        {
                            string webgl_image = WebGL_H() + 5 + "\n" + WebGL_H();
                            arg_data = Encoding.ASCII.GetBytes(webgl_image);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }
                        else if (DataRecive.Contains("webgl_metadata")) // engine login_data asdasda |
                        {
                            string webgl_metadata = GpuIndex();
                            arg_data = Encoding.ASCII.GetBytes(webgl_metadata);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }

                        else if (DataRecive.Contains("rects")) // engine login_data asdasda |
                        {
                            string rects = ArgumentEmulation(20000).ToString();
                            arg_data = Encoding.ASCII.GetBytes(rects);
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }
                        else if (DataRecive.Contains("async_proxy")) // engine login_data asdasda |
                        {
                            try
                            {
                                string replace_data = DataRecive.Replace("engine async_proxy", null);
                                string[] async_proxy_array = replace_data.Split(':');
                                async_proxy_array = async_proxy_array.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                                string full = null;

                                for (int i = 0; i < 4; i++)
                                {
                                    full = full + ":" + AsyncFromIP(
                                        async_proxy_array[0],//ip
                                        async_proxy_array[1],//port
                                        async_proxy_array[2],//user
                                        async_proxy_array[3],//pass
                                        Convert.ToInt32(async_proxy_array[4]), // protocol
                                        i); // functie
                                }

                                // Console.WriteLine(full);
                                arg_data = Encoding.ASCII.GetBytes(full);
                            }
                            catch
                            {
                                arg_data = Encoding.ASCII.GetBytes("BAD");
                            }
                            newsock.Send(arg_data, arg_data.Length, sender);

                        }

                        if (DataRecive.Contains(cmd_login)) // engine login_data asdasda |
                        {
                            string arg_1 = getBetween(DataRecive, cmd_login, "|")
                            .Replace(" ", null)
                            .Replace(cmd_login, null);

                            if (DataRecive.Contains(cmd_newfingerprint)) // engine login_data test | new_fingerprint testx |
                            {
                                string arg_newfp = getBetween(DataRecive, cmd_newfingerprint, "|").Replace(" ", null).Replace(cmd_newfingerprint, null);

                                arg_data = Encoding.ASCII.GetBytes(CreateFingerprint(arg_newfp, arg_1, 0, 0));

                                Console.WriteLine(arg_newfp);

                                newsock.Send(arg_data, arg_data.Length, sender);
                            }
                            else if (DataRecive.Contains(cmd_updatefingerprint)) // engine login_data test | new_fingerprint testx |
                            {
                                try
                                {
                                    string arg_newfp = getBetween(DataRecive, cmd_updatefingerprint, "|").Replace(cmd_updatefingerprint, null);

                                    arg_data = Encoding.ASCII.GetBytes("OK");

                                    int get_index = Convert.ToInt32(getBetween(DataRecive, "index:", "|"));

                                    arg_newfp = arg_newfp.Replace("index:" + get_index, null).Replace("\n", null).Replace("\0", null);

                                    string arg_2 = getBetween(DataRecive, cmd_login, "|")
                                    .Replace(" ", null)
                                    .Replace(cmd_login, null);

                                    CreateFingerprint(arg_newfp, arg_2, 1, get_index + 1); // update line

                                }
                                catch
                                {
                                    arg_data = Encoding.ASCII.GetBytes("BAD");
                                }

                                newsock.Send(arg_data, arg_data.Length, sender);
                            }
                            else if (DataRecive.Contains(cmd_delfingerprint)) // engine login_data test | new_fingerprint testx |
                            {
                                string arg_newfp = getBetween(DataRecive, cmd_delfingerprint, "|").Replace(cmd_delfingerprint, null);

                                arg_data = Encoding.ASCII.GetBytes("OK");

                                int get_index = Convert.ToInt32(getBetween(DataRecive, "delete", "|").Replace(" ", null));

                                arg_newfp = arg_newfp.Replace("index:" + get_index, null).Replace("\n", null).Replace("\0", null);
                                Console.WriteLine(arg_newfp);
                                Console.WriteLine(arg_1);

                                try
                                {
                                    CreateFingerprint(":", arg_1, 2, get_index); // remove line
                                }
                                catch
                                {
                                    arg_data = Encoding.ASCII.GetBytes("BAD");
                                }

                                newsock.Send(arg_data, arg_data.Length, sender);
                            }
                            else
                            {
                                arg_data = Encoding.ASCII.GetBytes(LoginId(arg_1));
                                newsock.Send(arg_data, arg_data.Length, sender);
                            }
                        }
                        if (DataRecive.Contains(cmd_register)) // engine register_data asdasda |
                        {
                            string arg_2 = getBetween(DataRecive, cmd_register, "|")
                                .Replace(" ", null)
                                .Replace(cmd_register, null);

                            arg_data = Encoding.ASCII.GetBytes(RegisterID(arg_2));

                            LoginId(arg_2);

                            newsock.Send(arg_data, arg_data.Length, sender);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString(), Color.Red);
            }
        }
    }
}
