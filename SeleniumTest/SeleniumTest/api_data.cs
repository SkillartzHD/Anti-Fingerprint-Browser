using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    internal class api_data
    {
        public static string[] ProfilName = new string[5000];
        public static string[] FontList = new string[5000];
        public static string[] FingerprintList = new string[5000];
        public static string[] UserAgentList = new string[5000];

        public static bool Serverdown = false;

        public static string[] Test = {
            "http://iguanmedia.com/fin.html",
            "https://abrahamjuliot.github.io/creepjs/",
            "https://bot.incolumitas.com/",
            "https://fingerprintjs.com/demo/",
            "https://browserleaks.com/ip",
            "https://browserleaks.com/webrtc",
            "https://browserleaks.com/canvas",
            "https://browserleaks.com/fonts",
            "https://browserleaks.com/webgl",
            "https://browserleaks.com/geo",
            "https://browserleaks.com/rects",
            "https://audiofingerprint.openwpm.com/"

        };
        private static string async_data(string data)
        {
            string[] words = data.Split(':');
            int count_profil = 0;
            int count_fonset = 0;
            int count_finger = 0;
            int count_agent = 0;

            Array.Resize(ref ProfilName, 3000);
            Array.Resize(ref FontList, 3000);
            Array.Resize(ref FingerprintList, 3000);
            Array.Resize(ref UserAgentList, 3000);

            foreach (var word in words)
            {

                if (word.Contains("--user-data-dir="))
                {
                    ProfilName[count_profil] = word;
                    count_profil++;
                }
                if (word.Contains("--fontset="))
                {
                    FontList[count_fonset] = word;
                    count_fonset++;
                }
                if (word.Contains("--base64="))
                {
                    FingerprintList[count_finger] = word;
                    count_finger++;
                }
                if (word.Contains("--user-agent="))
                {
                    UserAgentList[count_agent] = word.Replace("\"", null).Replace("\0", null);
                    count_agent++;
                }
            }
            if (String.IsNullOrEmpty(ProfilName[0]))
            {
                Serverdown = false;
            }
            else
            {
                Array.Resize(ref ProfilName, count_profil);
                Array.Resize(ref FontList, count_fonset);
                Array.Resize(ref FingerprintList, count_finger);
                Array.Resize(ref UserAgentList, count_agent);
            }

            return data;
        }
        public static string socket_udp(string data, bool async)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Connect(remoteEP);
                List<byte> list = new List<byte>();
                byte[] buffer = new byte[50000];
                list.AddRange(Encoding.Default.GetBytes(data));
                list.AddRange(new byte[] { 0x20 });
                socket.Send(list.ToArray());
                socket.Receive(buffer);

                Serverdown = true;

                if (async == true)
                {
                    async_data(Encoding.Default.GetString(buffer));
                }

                return Encoding.Default.GetString(buffer).Replace("\0", null);
            }
            catch
            {
                Serverdown = false;
                return "Serverul este offline.";
            }
        }
    }
}
