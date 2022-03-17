using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp52.Data
{
    internal class Fonts
    {
        private static string[] real_font_data = { // lista standard
        "Andale Mono", "Arial", "Arial Black", "Arial Hebrew", "Arial MT", "Arial Narrow", "Arial Rounded MT Bold", "Arial Unicode MS",
        "Bitstream Vera Sans Mono", "Book Antiqua", "Bookman Old Style",
        "Calibri", "Cambria", "Cambria Math", "Century", "Century Gothic", "Century Schoolbook", "Comic Sans", "Comic Sans MS", "Consolas", "Courier", "Courier New",
        "Garamond", "Geneva", "Georgia",
        "Helvetica", "Helvetica Neue",
        "Impact",
        "Lucida Bright", "Lucida Calligraphy", "Lucida Console", "Lucida Fax", "Lucida Handwriting", "Lucida Sans", "Lucida Sans Typewriter", "Lucida Sans Unicode",
        "Microsoft Sans Serif", "Monaco", "Monotype Corsiva", "MS Gothic", "MS Outlook", "MS PGothic", "MS Reference Sans Serif", "MS Sans Serif", "MS Serif", "MYRIAD", "MYRIAD PRO",
        "Palatino", "Palatino Linotype",
        "Segoe Print", "Segoe Script",
        "Tahoma", "Times", "Times New Roman", "Times New Roman PS", "Trebuchet MS",
        "Verdana", "Wingdings", "Wingdings 2", "Wingdings 3"
        };

        private static string[] test_spoof = { // lista de pe net
            "Sitka Heading","Candara Light","Sylfaen","Monospaced","Microsoft YaHei UI Light","Yu Gothic","Ebrima","Lucida Grande","Candara","Microsoft JhengHei Light",
            "Microsoft YaHei Light","NSimSun","MV Boli","Yu Gothic UI Semibold","Nirmala UI Semilight","Segoe UI Semilight","Ink Free","Georgia","Leelawadee UI Semilight",
            "Microsoft New Tai Lue","Segoe UI Historic","Microsoft YaHei UI","Franklin Gothic Medium","PMingLiU-ExtB","Segoe UI Symbol","Roboto","Constantia","Nirmala UI","Sitka Small",
            "MS UI Gothic","Yu Gothic UI Semilight","Malgun Gothic Semilight","MingLiU-ExtB","HoloLens MDL2 Assets","Corbel Light","Segoe UI Semibold","Gadugi","Segoe UI Emoji","Gabriola",
            "SimSun","Sitka Display","Dialog","Segoe UI Black","Myanmar Text","Serif","Yu Gothic UI Light","Javanese Text","Malgun Gothic","Yu Gothic UI","Arial","Mongolian Baiti",
            "Cambria Math","Leelawadee UI","Microsoft YaHei","Bahnschrift","Lucida Bright","Microsoft Himalaya","Open Sans","Corbel","Symbol","Microsoft JhengHei UI","Webdings","Marlett","MingLiU_HKSCS-ExtB",
            "Sitka Subheading","Sitka Banner","Microsoft PhagsPa","DialogInput","Segoe MDL2 Assets","Microsoft JhengHei","Microsoft JhengHei UI Light","SansSerif","Yu Gothic Medium","SimSun-ExtB",
            "TeamViewer15","Segoe UI Regular",
            "Yu Gothic Light","Microsoft Tai Le","Sitka Text","Open Sans Semibold","Segoe UI","Microsoft Yi Baiti","Segoe UI Light","Calibri Light"
        };

        public static string StaticFont()
        {
            string list = null;
            foreach(string s in real_font_data)
            {
                list = list + s + ";" ;
            }
            return list;
        }
        public static string FontsGenerator(int value)
        {
            Random rnd = new Random();
            string temp_array_fp = null;
            string __final_fonts = null;

            for (int i = 1; i <= value; i++)
            {

                string[] random_font_list_fake = test_spoof.OrderBy(x => rnd.Next()).ToArray();
                string[] random_font_list_real = real_font_data.OrderBy(x => rnd.Next()).ToArray();

                var _buffer_fonts = random_font_list_real.Union(random_font_list_fake).OrderBy(o => o);

                string[] MyRandomArray = _buffer_fonts.OrderBy(x => rnd.Next()).ToArray();

                Array.Resize(ref MyRandomArray, _GenerateFingerprint_font(144));

                new HashSet<string>(MyRandomArray).ToArray();

                foreach (var item in MyRandomArray)
                {
                    temp_array_fp = temp_array_fp + item + "\n";
                }

                string chartest = "\n";

                if (!temp_array_fp.Contains("Arial"))
                    temp_array_fp = temp_array_fp + "Arial" + chartest;
                if (!temp_array_fp.Contains("Lucida Sans Unicode"))
                    temp_array_fp = temp_array_fp + "Lucida Sans Unicode" + chartest;
                if (!temp_array_fp.Contains("Microsoft Sans Serif"))
                    temp_array_fp = temp_array_fp + "Microsoft Sans Serif" + chartest;
                if (!temp_array_fp.Contains("Segoe UI Regular"))
                    temp_array_fp = temp_array_fp + "Segoe UI Regular" + chartest;
                if (!temp_array_fp.Contains("Microsoft JhengHei"))
                    temp_array_fp = temp_array_fp + "Microsoft JhengHei" + chartest;
                if (!temp_array_fp.Contains("Times New Roman"))
                    temp_array_fp = temp_array_fp + "Times New Roman" + chartest;
                if (!temp_array_fp.Contains("Tahoma"))
                    temp_array_fp = temp_array_fp + "Tahoma" + chartest;
                if (!temp_array_fp.Contains("Lucida Grande"))
                    temp_array_fp = temp_array_fp + "Lucida Grande" + chartest;

                var lines2 = temp_array_fp.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


                var randomOrder2 = lines2.OrderBy(x => Guid.NewGuid());
                new HashSet<string>(randomOrder2).ToArray();

                foreach (var item3 in randomOrder2)
                {
                    __final_fonts = __final_fonts + item3 + ";";
                }
            }
            return __final_fonts;
        }
        private static int _GenerateFingerprint_font(int value)
        {
            Random rand = new Random();

            int r_spoof = rand.Next(35, value);
            return r_spoof;
        }
    }
}
