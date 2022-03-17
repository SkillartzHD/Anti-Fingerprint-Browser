using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WinFormsApp4.DataFingerPrints;

namespace WinFormsApp4
{
    internal class ClientData
    {
        public static string[] ProfilName = new string[5000];
        public static string[] FontList = new string[5000];
        public static string[] FingerprintList = new string[5000];
        public static string[] UserAgentList = new string[5000];

        public static string getBetween(string strSource, string strStart, string strEnd, bool replace_useless)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);

                return replace_useless == true ?
                    strSource.Substring(Start, End - Start).Replace("\"", null)
                    : strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public static void GetDataFromServer(int index)
        {
            ServerSend();

            string x = DecodeBase64(FingerprintList[index].Replace("--base64=", null));

            lang = getBetween(x, "cl13\":", ",", true); // lang

            plat = getBetween(x, "wx10\":", ",", true); // platform

            reso = getBetween(x, "hz50\":", ",", true); // resolution

            username_p = getBetween(x, "pu333\":", ",", true); // proxy username 
            password_p = getBetween(x, "pp91\":", ",", true); // proxy pass
            server_p = getBetween(x, "pru13\":", ",", true); // proxy sv
            protocol_p = getBetween(x, "prt91\":", ",", true); // proxy switch protocol (0 - nimic , 1 , https , 2 socks5)

            webrtc = getBetween(x, "pra11\":", ",", true); // webrtc
            timezone = getBetween(x, "xx15\":", ",", true); // timezone
            ClientRects = getBetween(x, "cr22\":", ",", true); // ClientRects emoticon/utf8

            video_out = getBetween(x, "xq36\":", ",", true); // video output
            audio_in = getBetween(x, "vd64\":", ",", true); // audio inputs
            audio_out = getBetween(x, "me64\":", ",", true); // audio output

            canvas = getBetween(x, "cp11\":", ",", true); //  canvas
            canvas_wid = getBetween(x, "wpw12\":", ",", true); // canvas width 
            canvas_hei = getBetween(x, "wph32\":", ",", true); // canvas height

            webgl1 = getBetween(x, "cx88\":", ",", true); // webgl 1 WebKit
            webgl2 = getBetween(x, "vr10\":", ",", true); // webgl 2 WebKit WebGL
            webgl3 = getBetween(x, "vw54\":", ",", true); // webgl 1 google Inc.
            webgl_fake = getBetween(x, "aw19\":", ",", true); // webgl fake videocard
            audioContext = getBetween(x, "ap32\":", ",", true); //  audiocontext

            longi = getBetween(x, "ek85\":", ",", true); //  longitude
            lati = getBetween(x, "os58\":", ",", true); //  latitude
            accuracy = getBetween(x, "ok33\":", "}", true); //  accuracy // capat de string

            device = getBetween(x, "md10\":", ",", true); //  ram memory
            hardware = getBetween(x, "hr94\":", ",", true); //  hardware cpu

        }
        public static void ServerSend()
        {
            string data = socket_udp("engine login_data " + UserID + "|", true);

            if(data.Contains("Nu exista acest id"))
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
            }
        }

        public static string _compress_data_(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeBase64(string buffer)
        {
            byte[] data = Convert.FromBase64String(buffer);
            return Encoding.UTF8.GetString(data);
        }
        public static void InsertAllp()
        {
            Console.Clear();
            for (int i = 0; i < ProfilName.Length; i++)
            {
                //Console.WriteLine(ProfilName[i]);
            }
        }
        public static void async_create_data(string data)
        {
            socket_udp("engine login_data " + UserID + " | new_fingerprint " + data + "|", true);
        }
        public static string async_data(string data)
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
            Array.Resize(ref ProfilName, count_profil);
            Array.Resize(ref FontList, count_fonset);
            Array.Resize(ref FingerprintList, count_finger);
            Array.Resize(ref UserAgentList, count_agent);

            //Console.WriteLine(ProfilName.Length);
            //GetAlldata();

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

                if (async == true)
                {
                    async_data(Encoding.Default.GetString(buffer));
                }

#pragma warning disable CS0162 // Unreachable code detected
                ServerResponseOK = true;
#pragma warning restore CS0162 // Unreachable code detected

                return Encoding.Default.GetString(buffer).Replace("\0", null);
            }
            catch
            {
                ServerResponseOK = false;
                return "fail";
            }
        }
    }
}
