using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp52.Program;
using static ConsoleApp52.Data.LoadData;
using static ConsoleApp52.Data.Navigator;
using static ConsoleApp52.Data.Fonts;
using Console = Colorful.Console;
using static ConsoleApp52.Data.Utils;
using Leaf.xNet;

namespace ConsoleApp52
{
    internal class Emulation
    {

        private static string FingerResult = null;

        private static string _ProfileHashGenerator(int length)
        {
            Random random = new Random();
            const string ProfileHashGenerator = "0123456789abcdefgh";
            return new string(Enumerable.Repeat(ProfileHashGenerator, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static string FullCodeEmulation = null;

        private static void FingerPrintSpoof(
            string protocol,
            string proxy_username,
            string proxy_password,
            string proxy_data,
            string screenspoof,
            string _fonts,
            string _webrtc_fake,
            string _webgl_fake,
            string _canavs_fake,
            string _canavs_width,
            string _canavs_height,
            string _hardware,
            string _memory,
            string _audiocontext,
            string _video_input,
            string _audio_input,
            string _audio_output,
            string _rects,
            string _timezone,
            string _lat,
            string _long,
            string _accuracy,
            string language,
            string platform,
            string webgl_1,
            string webgl_2,
            string webgl_3,
            string profile_name
            )
        {

            string temp_profile = _ProfileHashGenerator(8) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(12);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append(@"""titleBar"":""");
            sb.Append(profile_name);
            sb.Append(@""",");

            sb.Append(@"""ws21"":""");
            sb.Append(profile_name);
            sb.Append(@""",");

            sb.Append(@"""ej45"":""");

            sb.Append(temp_profile + ".bin"); /// ???? hash profile
            sb.Append(@""",");
            sb.Append(@"""jo11"":""");
            sb.Append(temp_profile); /// ???? hash profile
            sb.Append(@""",");
            sb.Append(@"""hr94"":""");
            sb.Append(_hardware.ToString()); // hardwareConcurrency
            sb.Append(@""",");
            sb.Append(@"""md10"":""");
            sb.Append(_memory.ToString()); // devicememory
            sb.Append(@""",");

            sb.Append(@"""cl13"":""");
            sb.Append(language); // language
            sb.Append(@""",");

            sb.Append(@"""ca41"":""");
            sb.Append(language); // language
            sb.Append(@";q=0.9"",");

            sb.Append(@"""wx10"":""");
            sb.Append(platform); // platforma
            sb.Append(@""",");

            sb.Append(@"""hz50"":""");
            sb.Append(screenspoof);
            sb.Append(@""",");

            sb.Append(@"""pu333"":""");
            sb.Append(proxy_username);
            sb.Append(@""",");

            sb.Append(@"""pp91"":""");
            sb.Append(proxy_password);
            sb.Append(@""",");

            sb.Append(@"""pru13"":""");
            sb.Append(proxy_data);
            sb.Append(@""",");
            //protocol

            sb.Append(@"""prt91"":""");
            sb.Append(protocol);
            sb.Append(@""",");
            // 1 -- HTTP/HTTPS
            // 2 - SOCKS 5


            sb.Append(@"""xw75"":""ab14"",");
            sb.Append(@"""pra11"":""");
            sb.Append(_webrtc_fake);
            sb.Append(@""",");

            sb.Append(@"""xx15"":"""); //timezone nu-mi trebuie
            sb.Append(_timezone);
            sb.Append(@""",");
            sb.Append(@"""no10"":""+03:00"",");

            sb.Append(@"""cr22"":""");
            sb.Append(_rects); //ClientRects spoof
            sb.Append(@""",");

            sb.Append(@"""dh65"":""");
            sb.Append(_fonts);
            sb.Append(@""",");

            sb.Append(@"""xq36"":""");
            sb.Append(_video_input);//video inputs
            sb.Append(@""",");

            sb.Append(@"""vd64"":""");
            sb.Append(_audio_input); //audio inputs
            sb.Append(@""",");

            sb.Append(@"""me64"":""");
            sb.Append(_audio_output); //audio outputs
            sb.Append(@""",");


            sb.Append(@"""cp11"":""");
            sb.Append(_canavs_fake.ToString());
            sb.Append(@""",");
            sb.Append(@"""wpw12"":""");
            sb.Append(_canavs_width); // Width
            sb.Append(@""",");
            sb.Append(@"""wph32"":""");
            sb.Append(_canavs_height);// Height
            sb.Append(@""",");


            sb.Append(@"""cx88"":""");
            sb.Append(webgl_1);// WebKit
            sb.Append(@""",");


            sb.Append(@"""vr10"":""");
            sb.Append(webgl_2);// WebKit WebGL
            sb.Append(@""",");

            sb.Append(@"""vw54"":""");
            sb.Append(webgl_3);// Google Inc.
            sb.Append(@""",");


            sb.Append(@"""aw19"":""");
            sb.Append(_webgl_fake);
            sb.Append(@""",");
            sb.Append(@"""ap32"":""");
            sb.Append(_audiocontext); //_audiocontext spoof
            sb.Append(@""",");
            sb.Append(@"""xu16"":""nx64"","); // nu stiu

            sb.Append(@"""ek85"":""");
            sb.Append(_long); //long
            sb.Append(@""",");
            sb.Append(@"""os58"":""");
            sb.Append(_lat); //lat
            sb.Append(@""",");
            sb.Append(@"""ok33"":""");
            sb.Append(_accuracy); //Accuracy
            sb.Append(@"""");

            sb.Append("}");

            FingerResult = sb.ToString();

        }
        public static string AsyncFromIP(string ip, string port, string user, string pass, int protocol, int value)
        {
            string result = null;

            try
            {
                var httpRequest = new HttpRequest();

                if (String.IsNullOrEmpty(user) && String.IsNullOrEmpty(pass))
                {
                    if (protocol == 1)
                    {
                        httpRequest.Proxy = HttpProxyClient.Parse(ip + ":" + port);
                    }
                    else if (protocol == 2)
                    {
                        httpRequest.Proxy = Socks5ProxyClient.Parse(ip + ":" + port);
                    }
                }
                else
                {
                    if (protocol == 1)
                    {
                        httpRequest.Proxy = HttpProxyClient.Parse(ip + ":" + port + ":" + user + ":" + pass);
                    }
                    else if (protocol == 2)
                    {
                        httpRequest.Proxy = Socks5ProxyClient.Parse(ip + ":" + port + ":" + user + ":" + pass);
                    }
                }
                httpRequest.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

                httpRequest.AddHeader("User-Agent", UserAgentData());

                if (value == 0)
                {
                    var resp = httpRequest.Get("https://ipinfo.io/json").ToString();

                    string GetTimeZone = getBetween(resp, "timezone", ",").Replace(" ", null)
                        .Replace(":", null)
                        .Replace("\"", null);
                    Console.WriteLine("timezeone:" + GetTimeZone, Color.Red);

                    result = GetTimeZone;
                }
                else if (value == 1)
                {
                    var resp = httpRequest.Get("https://ipinfo.io/json").ToString();
                    string GetLocation = getBetween(resp, "loc", "\",").Replace(" ", null)
                        .Replace(":", null)
                        .Replace("\"", null);
                    Console.WriteLine("GetLocation:" + GetLocation, Color.Red);

                    result = GetLocation;

                }

                else if (value == 3)
                {
                    var resp = httpRequest.Get("https://ipinfo.io/json").ToString();
                    string country = getBetween(resp, "country", "\",").Replace(" ", null)
                        .Replace(":", null)
                        .Replace("\"", null);
                    Console.WriteLine("Country:" + country, Color.Red);

                    result = country;

                }
                else if (value == 2)
                {

                    var resp = httpRequest.Get("https://api.ipify.org/").ToString();
                    Console.WriteLine("webrtc:" + resp, Color.Red);
                    result = resp.ToString();
                }
                else
                {
                    result = "error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "error";
            }
            return result;

        }
        public static string _compress_data_(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string _fingerprint_result_to_file(string id_profilename)
        {
            string[] geolocation = AsyncFromIP(null, null, null, null, 0, 1).Split(',');


            FingerPrintSpoof(
                "0", // protocol 0 - fara proxy 1- https 2- socks5
                "username_proxy", // username
                "password_proxy", // parola
                "server_proxy", // sv proxy
                 ScreenData(), // screen spoof ||||pentru font sa reactioneze diferit la unicode trebui sa fie diferita valoarea
                 null, // font local ??? or from list
                 AsyncFromIP(null, null, null, null, 0, 2), //webrtc spoof ip leak
                 GpuIndex(), // webgl 
                 ArgumentEmulation(20000).ToString(), // canvans fake
                 WebGL_H() + 5.ToString(), // webgl width
                 WebGL_H().ToString(), // webgl height
                 ArgumentEmulation(-1).ToString(), // hardwareconcurrency valoarea -1 inseamna ca genereaza un numar intre 2-12
                 ArgumentEmulation(-2).ToString(), // devicememory  valoarea -2 inseamna ca genereaza un numar intre 2-8
                 ArgumentEmulation(20000).ToString(), // audiocontext fake
                 ArgumentEmulation(5).ToString(), // video input fake
                 ArgumentEmulation(5).ToString(),// audio input fake
                 ArgumentEmulation(5).ToString(), // video ouput fake
                 ArgumentEmulation(20000).ToString(),// _rects client fake

                 AsyncFromIP(null, null, null, null, 0, 0),  // timezone
                 geolocation[0], // latitudine
                 geolocation[1], // longitudine
                 ArgumentEmulation(7000).ToString(), // acuratete
                 "en", // limba
                 "Win32", // platforma
                 "WebKit", //webgl mask 1 WebKit
                 "WebKit WebGL", // mask 2 WebKit WebGL
                 "Google Inc.", // mask 3 Google Inc
                 id_profilename

                );

            string font_buff = "--fontset=" + _compress_data_(FontsGenerator(1));
            string fing_buff = "--base64=" + _compress_data_(FingerResult);
            string user_buff = "--user-agent=" + "\"" + UserAgentData() + "\"";
            string profile_data = "--user-data-dir=" + id_profilename;

            FullCodeEmulation = profile_data + ":" + font_buff + ":" + fing_buff + ":" + user_buff.Replace("\0",null);

            Console.WriteLine(FullCodeEmulation);
            return FullCodeEmulation;
        }
    }
}
