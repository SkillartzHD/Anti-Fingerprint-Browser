using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp52.Data
{
    internal class Navigator
    {
        private static string[] USAgent_randRealChrome = {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.128 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.72 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.72 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.72 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/82.0.4050.12 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36"
        };

        private static string[] screen_spoof = {
            "1920x1080","1768x992","1680x1050","1600x1024","1600x900","1440x900",
            "1366x768","1360x768","1280x960","1280x768","1280x720","1280x720",
            "1280x664","1176x864","1152x864","1024x768","800x600"
        };

        public static string UserAgentData()
        {
            Random rand = new Random();
            int agent_spoof_index = rand.Next(0, USAgent_randRealChrome.Length);
            return USAgent_randRealChrome[agent_spoof_index];

        }
        public static string ScreenData()
        {
            Random rand = new Random();
            int screen_spoof_index = rand.Next(0, screen_spoof.Length);
            return screen_spoof[screen_spoof_index];
        }

        public static string GetAllScreenData()
        {
            string data = null;

            foreach (string value in screen_spoof)
            {
                data = data + "\n" + value;
            }
            return data;
        }

        public static int WebGL_H()
        {
            int store_weblgl_h = ArgumentEmulation(130);

            if (store_weblgl_h <= 5)
            {
                store_weblgl_h = store_weblgl_h + 5;
            }
            else
            {
                store_weblgl_h = store_weblgl_h - 5;
            }
            return store_weblgl_h;
        }
        public static int ArgumentEmulation(int value)
        {
            int[] array_devices = new int[] { 2, 4, 6, 8, 16, };

            Random rand = new Random();

            int result = 0;

            if (value > 1) // altele
            {
                int r_spoof = rand.Next(1, value);
                result = r_spoof;
            }

            if (value == 107) // font fingerprint
            {
                int r_spoof_font = rand.Next(30, value);
                result = r_spoof_font;
            }
            if (value == -1) // hardwareConcurrency
            {
                int index_devices = rand.Next(0, array_devices.Length);
                int hardwareConcurrency = array_devices[index_devices];

                result = hardwareConcurrency;
            }

            if (value == -2) // devicememory
            {
                int index_devices = rand.Next(0, array_devices.Length);
                int device_memory = array_devices[index_devices];

                result = device_memory;
            }
            return result;
        }
    }
}
