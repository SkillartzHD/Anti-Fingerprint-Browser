using System;
using System.Threading;
using static SeleniumTest.api_data;
using static SeleniumTest.SeleniumAutomation;

namespace SeleniumTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string UserID = "test";

            socket_udp("engine login_data " + UserID + "|", true);

            for (int i = 0; i < ProfilName.Length;i++)
            {
                Thread thread_replay = new Thread(() => RunTest(i));

                thread_replay.Start();

                Thread.Sleep(10000);
            }

        }
    }
}
