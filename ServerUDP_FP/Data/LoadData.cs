using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp52.Data
{
    internal class LoadData
    {
        private static string gpu_listname = "data/gpu_list.txt";
        private static string[] ArrayGPUList = new string[9999];


        public static string GpuIndex()
        {
            Random rnd = new Random();
            int gpu_index_array = rnd.Next(0, ArrayGPUList.Length);
            return ArrayGPUList[gpu_index_array];

        }
        public static void GpuList()
        {
            int array_index_gpu = 0;
            string GpuLine;
            System.IO.StreamReader GpuFile = new System.IO.StreamReader(gpu_listname);

            while ((GpuLine = GpuFile.ReadLine()) != null)
            {
                array_index_gpu++;
                ArrayGPUList[array_index_gpu] = GpuLine + "\r";
            }
            Array.Resize(ref ArrayGPUList, array_index_gpu);
        }
    }
}
