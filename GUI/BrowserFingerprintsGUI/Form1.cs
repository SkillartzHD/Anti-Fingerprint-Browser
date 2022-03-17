using Leaf.xNet;
using ReaLTaiizor.Colors;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Util;
using System.Diagnostics;
using System.Text;
using static WinFormsApp4.ClientData;
using static WinFormsApp4.DataFingerPrints;
using Console = Colorful.Console;

namespace WinFormsApp4
{
    public partial class Form1 : MaterialForm
    {
        private readonly MaterialManager materialManager;

        private int SwitchInfo = 0;

        public Form1()
        {
            InitializeComponent();
            materialManager = MaterialManager.Instance;


            materialManager.EnforceBackcolorOnAllComponents = true;

            materialManager.AddFormToManage(this);
            materialManager.Theme = MaterialManager.Themes.LIGHT;
            metroTabControl1.Style = ReaLTaiizor.Enum.Metro.Style.Light;

            materialManager.ColorScheme = new MaterialColorScheme(MaterialPrimary.Green400, MaterialPrimary.BlueGrey900, MaterialPrimary.BlueGrey900, MaterialAccent.Red200, MaterialTextShade.WHITE);
        }

        private void Call()
        {
            materialLabel19.Text = "Browser profile fonts list:" + GetFontListFromServer();

            UpdateGUI_FromServer();
            materialLabel16.Text = "Browser glyphs & DOMRect: " + _compress_data_(ClientRects);
            LabelViews();
            TextBox_useragent.Text = GetUserAgentFromServer();
            aloneNotice3.Text = "Unele website-uri pot verifica tara de origine a ip-ului\n ca limba setata pe browser.";
        }

        private void LabelViews()
        {
            int get_index = materialComboBox7.SelectedIndex;

            string font_real = FontList[get_index].Replace("--fontset=", null);
            string font_decoded = DecodeBase64(font_real);

            int count = font_decoded.Count(f => f == ';');
            string user_agent_real = UserAgentList[get_index].Replace("--user-agent=", null).Replace("\"", null);

            if (font_real.Contains(real_font))
            {
                // real
                FontListSwitch.Checked = false;
            }
            else
            {
                FontListSwitch.Checked = true;
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (ClientRects.Length > 0)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                // real
                FontListSwitch.Checked = true;
            }
            else
            {
                FontListSwitch.Checked = false;
            }

            label_devices.Text = video_out + "," + audio_in + "," + audio_out;
            label_fonts.Text = FontListSwitch.Checked == true ? Convert.ToString(count) : "Real";
            label_audiocon.Text = _compress_data_(audioContext);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            label_webl_meta.Text = webgl_fake.Length > 10 ? "On" : "Off";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            label_webgl_img.Text = _compress_data_(canvas_hei + canvas_wid);
            label_canvas.Text = _compress_data_(canvas);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (webrtc.Contains("-1"))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                label_webrtc.Text = "Off";

            }
            else
            {
                label_webrtc.Text = webrtc;
            }
            label_geolocation.Text = lati + ", " + longi + ", " + accuracy;
            label_timezone.Text = timezone;
            label_lang.Text = lang;
            label_resolution.Text = reso;
            label_useragent.Text = user_agent_real.Length > 10 ? "On" : "Off";
            label_platform.Text = plat;
            if (Convert.ToInt32(protocol_p) > 0)
            {
                label_proxy_info.Text = server_p;
            }
            else
            {
                label_proxy_info.Text = "Without proxy";
            }
        }
        private void UpdateProfileList()
        {

            ServerSend();

            object[] ItemObject = new object[ProfilName.Length];

            materialComboBox7.Items.Clear();
            for (int i = 0; i < ProfilName.Length; i++)
            {
                ItemObject[i] = ProfilName[i].Replace("--user-data-dir=", null);
            }
            materialComboBox7.Items.AddRange(ItemObject);

            //materialComboBox7.SelectedIndex = 0;

            //materialLabel19.Text = "Browser profile fonts list:" + GetFontList();

        }
        public void ClientConnect()
        {
            //

            if (IsConnected == false)
            {
                materialTabControl1.SelectedIndex = 2;
            }
            else
            {
                materialRichTextBox1.Clear();
            }

            if (IsConnected == false)
            {
                pictureBox1.Image = BrowserFingerprints.Properties.Resources._5509476641551938928_32; // eroare
                materialLabel56.Text = "Disconnected";
            }
            else
            {
                pictureBox1.Image = BrowserFingerprints.Properties.Resources._11875166141558096434_32; // ok
                materialLabel56.Text = "Connected";

                if (materialTextBox2.Text.Length < 50 || String.IsNullOrWhiteSpace(materialTextBox2.Text))
                {
                    StreamWriter data_save = new StreamWriter("settings/local_data.ini");
                    data_save.Write(materialTextBox2.Text);
                    data_save.Close();
                    data_save.Dispose();
                }

                if (auto_connect == true)
                {
                    materialTabControl1.SelectedIndex = 0;
                    auto_connect = false;
                }
            }

        }

        public void DefaultSettings_load()
        {

            UserID = materialTextBox2.Text;

            ServerSend();

            ClientConnect();

            Console.WriteLine(ServerResponseOK);
            Console.WriteLine(IsConnected);


            if (ServerResponseOK == false || IsConnected == false)
            {
                materialTabControl1.SelectedIndex = 2; // settings page
                materialRichTextBox1.Text = ServerResponseOK == false ? "Serverul nu este online." : "Id-ul \"" + UserID + "\" nu exista.";
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = false;
                DefaultSettings();
                RealButton();
                UpdateProfileList();
            }
        }
        private void UpdateGUI_FromServer()
        {
            int get_index = materialComboBox7.SelectedIndex;
            GetDataFromServer(get_index);

            ProxyComboBox();



            /// panel proxy
            aloneTextBox2.Text = server_p;
            aloneTextBox1.Text = username_p;
            aloneTextBox3.Text = password_p;

            try
            {
                int protocol_int = Convert.ToInt32(protocol_p);
                proxytype_comboBox.SelectedIndex = protocol_int;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // end


            //geolocation

            TextBox_long.Text = longi;
            TextBox_lat.Text = lati;
            TextBox_accuracy.Text = accuracy;


            // mai trb niste if-uri  la butoanele de switch bazate pe ip.

            //end


            //webrtc
            // mai trb niste if-uri  la butoanele de switch bazate pe ip.

            TextBox_PublicIP.Text = webrtc;


            //end


            //timezone
            // mai trb niste if-uri  la butoanele de switch bazate pe ip.
            materialComboBox1.Text = timezone;


            //end


            //navigator
            TextBox_useragent.Text = UserAgentList[get_index].Replace("--user-agent=", null)
                .Replace("\"", null)
                .Replace("\n", null);
            TextBox_platform.Text = plat;
            TextBox_lang.Text = lang;
            materialComboBox4.Text = hardware; // hardware concurrency
            materialComboBox3.Text = reso; // screenresolution
            materialComboBox5.Text = device; // device memory
                                             // materialComboBox2.Text = track; //not track

            //end


            //fonts 
            if (String.IsNullOrEmpty(ClientRects))
            {
                DomRect_Switch.Checked = false;
            }
            else
            {
                DomRect_Switch.Checked = true;
            }

            //end

            //media devices

            if (String.IsNullOrEmpty("video_out") == true)
            {
                MediaSwitch.Checked = false;
            }
            else
            {
                MediaSwitch.Checked = true;
            }

            //end

            //hardware tab

            if (canvas_block.Enabled == true)
            {
                materialLabel20.Text = "ID : " + _compress_data_(canvas);
            }

            if (audiocon_default.Enabled == true)
            {
                audiocon_id.Text = "ID : " + _compress_data_(audioContext);
            }

            if (webgl_button_default.Enabled == true)
            {
                webgl_id.Text = "ID : " + _compress_data_(canvas_hei + canvas_wid);
            }

            webglMetaTextBox_vendor.Text = webgl1;
            webglMetaTextBox_renderer.Text = webgl2;
            webglMetaTextBox_unmaskedvendor.Text = webgl3;
            webglMetaTextBox_unmasked.Text = webgl_fake;

            textbox_videoin.Text = video_out;
            textbox_audioin.Text = audio_in;
            textbox_audio_out.Text = audio_out;

            //end
        }

        private int GetFontListFromServer()
        {
            int get_index = materialComboBox7.SelectedIndex;

            string font_real = FontList[get_index].Replace("--fontset=", null);
            string font_decoded = DecodeBase64(font_real);

            //Console.WriteLine(font_decoded);

            int count = font_decoded.Count(f => f == ';');

            return count;
        }

        private string GetUserAgentFromServer()
        {
            int get_index = materialComboBox7.SelectedIndex;

            string user_agent_real = UserAgentList[get_index]
                .Replace("--user-agent=", null)
                .Replace("\"", null)
                .Replace("\n", null);

            //Console.WriteLine(user_agent_real, Color.Red);
            return user_agent_real;
        }

        private string temporary_fonts = string.Empty;

        private string GenerateRectsFromServer()
        {
            string new_rects = socket_udp("engine rects", false);
            string hash_rects = _compress_data_(new_rects);
            ClientRects = new_rects;

            return hash_rects;
        }
        private int GenerateFontListFromServer()
        {
            string new_fonts = socket_udp("engine font", false);

            temporary_fonts = new_fonts.Replace(" ", null).Replace("\"", null);
            int get_index = materialComboBox7.SelectedIndex;

            FontList[get_index] = "--fontset=" + temporary_fonts;

            string font_decoded = DecodeBase64(temporary_fonts);

            int count = font_decoded.Count(f => f == ';');

            return count;
        }
        private void GetResolutionFromServer()
        {
            string new_fonts = socket_udp("engine screen", false);

            string[] array_screen = new string[] { "" };
            array_screen = new_fonts.Split('\n');

            /*
            //Console.WriteLine(array_screen[1]);
            //Console.WriteLine(array_screen[0]);
            //Console.WriteLine(array_screen[2]);
            */

            array_screen = array_screen.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            System.Object[] ItemObject = new System.Object[array_screen.Length];

            materialComboBox3.Items.Clear();
            for (int i = 0; i < array_screen.Length; i++)
            {
                ItemObject[i] = array_screen[i];
            }
            materialComboBox3.Items.AddRange(ItemObject);

        }
        private void DefaultSettings()
        {
            GetResolutionFromServer();

            System.Object[] ItemObject = new System.Object[6];

            materialComboBox4.Items.Clear();
            materialComboBox5.Items.Clear();

            for (int i = 0; i < 6; i++)
            {
                //Console.WriteLine(i);

                if (i == 0)
                {
                    ItemObject[i] = 1;
                }
                else if (i == 5)
                {
                    ItemObject[i] = 16;
                }
                else
                {
                    ItemObject[i] = i * 2;
                }


            }
            materialComboBox4.Items.AddRange(ItemObject);
            materialComboBox5.Items.AddRange(ItemObject);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            auto_connect = false;
            string? id = null;

            if (!Directory.Exists("settings"))
            {
                Directory.CreateDirectory("settings");
            }
            if (File.Exists("settings/local_data.ini"))
            {
                id = System.IO.File.ReadAllText("settings/local_data.ini");
                auto_connect = true;

                if (id.Length < 50 || String.IsNullOrWhiteSpace(id))
                {
                    materialTextBox2.Text = id;
                }
            }
            DefaultSettings_load();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void materialSwitch1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (materialSwitch1.Checked == true)
            {
                materialManager.Theme = MaterialManager.Themes.DARK;
                materialSwitch1.Text = "Dark";
                metroTabControl1.Style = ReaLTaiizor.Enum.Metro.Style.Dark;
            }
            else
            {

                materialManager.Theme = MaterialManager.Themes.LIGHT;
                materialSwitch1.Text = "Light";
                metroTabControl1.Style = ReaLTaiizor.Enum.Metro.Style.Light;


            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            materialButton5.HighEmphasis = false;
            materialButton3.HighEmphasis = false;
            materialButton1.HighEmphasis = true;
            materialSwitch3.Checked = true;
            materialSwitch3.Enabled = true;
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            //chatBubbleRight1.Text = "hello";
            TextBox_lat.Text = RealLati;
            TextBox_long.Text = RealLongi;

            materialSwitch3.Checked = true;
            materialSwitch3.Enabled = false;
            materialButton5.HighEmphasis = false;
            materialButton3.HighEmphasis = true;
            materialButton1.HighEmphasis = false;

        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            TextBox_lat.Text = null;
            TextBox_long.Text = null;
            materialSwitch3.Checked = true;
            materialSwitch3.Enabled = false;
            materialButton5.HighEmphasis = true;
            materialButton3.HighEmphasis = false;
            materialButton1.HighEmphasis = false;


        }

        private void materialSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch3.Checked == false)
            {
                panel1.Visible = true;
                TextBox_lat.Text = lati;
                TextBox_long.Text = longi;
            }
            else
            {
                TextBox_lat.Text = AsyncLati;
                TextBox_long.Text = AsyncLongi;

                panel1.Visible = false;
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel4_Click(object sender, EventArgs e)
        {
            notificationBox133.Visible = true;
            notify_close.Start();
            notify_close.Enabled = true;
            notificationBox133.Text =
                "\nDefault - reprezinta valorile default prestabilita\n\n"
                +
                "Allow - permite interogarea a browserului sa afisezie valorile de geolocatie originale.\n\n" +
                "Block - interogarea browserului nu o sa trimita nici o informatie despre geolocatie\n\n" +
                "Switch-ul respectiv rezprezinta interogarea cu un server API la IP-ul existent si informatile de latitudine si longitudine sa fie exacte si reale. ";

        }

        private void notify_close_Tick(object sender, EventArgs e)
        {
            notificationBox133.Visible = false;
            notify_close.Stop();
            notify_close.Enabled = false;
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            materialButton6.HighEmphasis = false;
            materialButton4.HighEmphasis = false;
            materialButton2.HighEmphasis = true;
            materialSwitch2.Checked = true;
            materialSwitch2.Enabled = true;
            notificationBox6.Visible = false;
            notificationBox6.Text = null;
            notificationBox6.Refresh();
            TextBox_PublicIP.Text = webrtc;

        }

        private void RealButton()
        {
            var httpRequest = new HttpRequest();
            string resp = httpRequest.Get("https://api.ipify.org/").ToString();

            RealWebRTC = resp;
            resp = httpRequest.Get("https://ipinfo.io/json").ToString();

            RealTimeZone = getBetween(resp, "timezone", ",", false).Replace(" ", null)
                .Replace(":", null)
                .Replace("\"", null);

            //Console.WriteLine("timezeone:" + RealTimeZone, Color.Red);

            string GetLocation = getBetween(resp, "loc", "\",", false).Replace(" ", null)
                .Replace(":", null)
                .Replace("\"", null);

            string[] geolocation_array = GetLocation.Split(',');


            RealLati = geolocation_array[0];
            RealLongi = geolocation_array[1];

            //Console.WriteLine("lati:" + RealLati, Color.Red);
            //Console.WriteLine("longi:" + RealLongi, Color.Red);

        }
        private void materialButton6_Click(object sender, EventArgs e)
        {
            TextBox_PublicIP.Text = RealWebRTC;

            materialButton6.HighEmphasis = true;
            materialButton4.HighEmphasis = false;
            materialButton2.HighEmphasis = false;
            materialSwitch2.Checked = true;
            materialSwitch2.Enabled = false;
            notificationBox6.Visible = true;
            notificationBox6.Text = "Afiseaza informatile WebRTC originale.";
            notificationBox6.Refresh();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            TextBox_PublicIP.Text = "-1";
            materialButton6.HighEmphasis = false;
            materialButton4.HighEmphasis = true;
            materialButton2.HighEmphasis = false;
            materialSwitch2.Checked = true;
            materialSwitch2.Enabled = false;
            notificationBox6.Visible = true;
            notificationBox6.Text = "Dezactiveaza WebRTC";
            notificationBox6.Refresh();
        }
        private void update_webrtc()
        {
            if (materialSwitch2.Checked == false)
            {
                panel2.Visible = true;
                TextBox_PublicIP.Text = webrtc;
            }
            else
            {
                TextBox_PublicIP.Text = AsyncWebRTC;
                panel2.Visible = false;
            }
        }
        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            update_webrtc();
        }

        private void materialLabel10_Click(object sender, EventArgs e)
        {
            if (SwitchInfo == 0)
            {
                webView21.Visible = true;
                SwitchInfo = 1;
                webView21.Source = new System.Uri("https://www.youtube.com/embed/_1tidSiTs8Y", System.UriKind.Absolute);
            }
            else
            {
                webView21.Visible = false;
                SwitchInfo = 0;
                webView21.Source = new System.Uri("about:blank", System.UriKind.Absolute);

            }

            //notificationBox6.Visible = true;
            //notify_close.Start();
            // notify_close.Enabled = true;
        }

        private void materialCard16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 0; // = pagina home
        }

        private void materialSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            if (Switch_Timezone.Checked == false)
            {
                panel3.Visible = true;
                materialComboBox1.Text = RealTimeZone;
                aloneNotice2.Visible = true;
            }
            else
            {
                materialComboBox1.Text = AsyncTimeZone;
                panel3.Visible = false;
                aloneNotice2.Visible = false;
                aloneNotice2.Text = "Unele website-uri pot verifica timeonze-ul ip-ului.";
                aloneNotice2.Refresh();
            }
        }

        private void materialSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch5.Checked == true)
            {
                TextBox_lang.Visible = false;
                aloneNotice3.Visible = false;
                TextBox_lang.Text = AsyncCountry;
            }
            else
            {
                TextBox_lang.Visible = true;
                aloneNotice3.Visible = true;
                aloneNotice3.Text = "Unele website-uri pot verifica tara de origine a ip-ului\n ca limba setata pe browser.";
            }
        }

        private void materialSwitch9_CheckedChanged(object sender, EventArgs e)
        {

            if (MediaSwitch.Checked == true)
            {
                panel5.Visible = false;

                textbox_videoin.Text = video_out;
                textbox_audioin.Text = audio_in;
                textbox_audio_out.Text = audio_out;

            }
            else
            {
                panel5.Visible = true;


            }
        }
        private void RandomizeButtonDataFonts()
        {
            if (DomRect_Switch.Checked == true)
            {
                materialLabel16.Text = "Browser glyphs & DOMRect: " + GenerateRectsFromServer();
            }
            if (FontListSwitch.Checked == true)
            {
                materialLabel19.Text = "Browser profile fonts list:" + GenerateFontListFromServer();
            }
        }
        private void materialButton9_Click(object sender, EventArgs e)
        {
            RandomizeButtonDataFonts();
        }

        private void materialButton19_Click(object sender, EventArgs e)
        {
            webgl_button_default.HighEmphasis = false;
            webgl_panel.Visible = false;
            webgl_button_off.HighEmphasis = true;
            webgl_button_generate.Enabled = false;
            webgl_msg.Visible = true;

        }

        private void materialButton18_Click(object sender, EventArgs e)
        {
            webgl_button_default.HighEmphasis = true;
            webgl_panel.Visible = true;
            webgl_button_off.HighEmphasis = false;
            webgl_button_generate.Enabled = true;
            webgl_msg.Visible = false;



        }
        private void materialButton16_Click(object sender, EventArgs e) // off
        {
            audiocon_off.HighEmphasis = true;
            audiocon_panel.Visible = false;
            audiocon_default.HighEmphasis = false;
            audiocon_generate.Enabled = false;
            audiocon_msg.Visible = true;

        }

        private void materialButton15_Click(object sender, EventArgs e) // on
        {
            audiocon_off.HighEmphasis = false;
            audiocon_panel.Visible = true;
            audiocon_default.HighEmphasis = true;
            audiocon_generate.Enabled = true;
            audiocon_msg.Visible = false;

        }

        private void materialButton13_Click(object sender, EventArgs e)
        {
            audioContext = socket_udp("engine audiocontext", false);

            if (audiocon_default.Enabled == true)
            {
                audiocon_id.Text = "ID : " + _compress_data_(audioContext);
            }
        }

        private void materialButton17_Click(object sender, EventArgs e)
        {
            string[] array_webgl = new string[] { "" };

            string test = socket_udp("engine wid_webgl", false);

            array_webgl = test.Split('\n');

            array_webgl = array_webgl.Where(x => !string.IsNullOrEmpty(x)).ToArray();


            canvas_wid = array_webgl[1];
            //Console.WriteLine(canvas_wid);

            if (webgl_button_default.Enabled == true)
            {
                webgl_id.Text = "ID : " + _compress_data_(canvas_hei + canvas_wid);
            }
        }

        private void materialButton11_Click(object sender, EventArgs e) // off
        {
            canvas_off.HighEmphasis = true;
            canvas_default.HighEmphasis = false;
            canvas_block.HighEmphasis = false;
            canvas_panel.Visible = false;
            canvas_msg.Visible = true;

            canvas_generate.Enabled = false;
        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            canvas_off.HighEmphasis = false;
            canvas_default.HighEmphasis = false;
            canvas_block.HighEmphasis = true;
            canvas_panel.Visible = false;
            canvas_msg.Visible = true;

            canvas_generate.Enabled = false;
        }


        private void materialButton10_Click(object sender, EventArgs e)
        {
            canvas_off.HighEmphasis = false;
            canvas_default.HighEmphasis = true;
            canvas_block.HighEmphasis = false;
            canvas_panel.Visible = true;
            canvas_msg.Visible = false;

            canvas_generate.Enabled = true;
        }
        private void materialButton21_Click(object sender, EventArgs e)//on
        {
            webglMeta_default.HighEmphasis = true;
            webglMeta_off.HighEmphasis = false;
            webglMeta_msg.Visible = false;
            webglMeta_panel.Visible = true;

            webglMeta_generate.Enabled = true;
        }

        private void materialButton22_Click(object sender, EventArgs e) //off
        {
            webglMeta_off.HighEmphasis = true;
            webglMeta_default.HighEmphasis = false;
            webglMeta_msg.Visible = true;
            webglMeta_panel.Visible = false;

            webglMeta_generate.Enabled = false;
        }

        private void aloneNotice1_TextChanged(object sender, EventArgs e)
        {
        }


        private void materialSwitch7_CheckedChanged(object sender, EventArgs e)
        {
            if (DomRect_Switch.Checked == true)
            {
                aloneNotice1.Visible = false;
                materialLabel16.Visible = true;
                RandomizeButtonDataFonts();

            }
            else
            {
                aloneNotice1.Visible = true;
                ClientRects = "";
                aloneNotice1.Text = "Browserul afiseaza un DOMRect original";
                aloneNotice1.Refresh();
                materialLabel16.Visible = false;
            }
        }

        private void materialSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            if (FontListSwitch.Checked == true)
            {
                materialLabel19.Visible = true;
                aloneNotice4.Visible = false;
                RandomizeButtonDataFonts();
            }
            else
            {
                int get_index = materialComboBox7.SelectedIndex;
                string static_font = _compress_data_(socket_udp("engine static_fon", false));
                materialLabel19.Visible = false;
                FontList[get_index] = "--fontset=" + static_font;
                aloneNotice4.Text = "Browserul foloseste o lista statica de fonturi.";
                aloneNotice1.Refresh();
                aloneNotice4.Visible = true;
            }
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            async_from_proxy(aloneTextBox2.Text, aloneTextBox1.Text, aloneTextBox3.Text, Convert.ToString(proxytype_comboBox.SelectedIndex));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (AsyncLati.Contains("error") || AsyncWebRTC.Contains("error"))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                proxy_textbox_info.Text = "Date invalide sau server nefunctional";
            }
            else
            {
                TextBox_lang.Text = AsyncCountry;
                TextBox_lat.Text = AsyncLati;
                TextBox_long.Text = AsyncLongi;
                TextBox_PublicIP.Text = AsyncWebRTC;
                materialComboBox1.Text = AsyncTimeZone;
                proxy_textbox_info.Text = AsyncTimeZone + "\n" + AsyncWebRTC + "\n" + AsyncLati + "\n" + AsyncLongi + "\nCountry : " + AsyncCountry;
            }

        }

        private void proxy_textbox_info_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void proxy_textbox_info_KeyPress(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ProxyComboBox()
        {
            if (proxytype_comboBox.SelectedIndex == 0)//fara proxy
            {
                panel_proxy.Enabled = false;
            }
            if (proxytype_comboBox.SelectedIndex == 1)//https proxy
            {
                panel_proxy.Enabled = true;
            }
            if (proxytype_comboBox.SelectedIndex == 2)//socks proxy
            {
                panel_proxy.Enabled = true;
            }
        }
        private void proxytype_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProxyComboBox();
        }

        private void poisonListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void materialCard19_Paint(object sender, PaintEventArgs e)
        {

        }

        private void materialButton14_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;

            materialButton16.Enabled = false;
            materialButton8.Enabled = false;
            panel6.Visible = true;
        }

        private void materialTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(materialTextBox1.Text) == true)
            {
                materialButton11.Enabled = false;
            }
            else
            {
                materialButton11.Enabled = true;
            }
        }

        private void materialButton11_Click_1(object sender, EventArgs e)
        {
            panel7.Visible = true;
            materialButton16.Enabled = true;
            materialButton8.Enabled = true;
            panel6.Visible = false;
            async_create_data(materialTextBox1.Text);
            UpdateProfileList();
            materialTextBox1.Text = null;
            string data_combo = materialComboBox7.GetItemText(materialComboBox7.SelectedItem);

            if (!String.IsNullOrWhiteSpace(data_combo))
            {
                materialComboBox7.SelectedIndex = 0;

            }
        }

        private void materialButton15_Click_1(object sender, EventArgs e)
        {
            panel7.Visible = true;
            panel6.Visible = false;
            materialButton16.Enabled = true;
            materialTextBox1.Text = null;
            materialButton8.Enabled = true;

        }

        private void materialComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data_combo = materialComboBox7.GetItemText(materialComboBox7.SelectedItem);

            proxy_textbox_info.Text = null;

            if (String.IsNullOrWhiteSpace(data_combo) == true)
            {
                materialButton10.Enabled = false;
                materialButton12.Enabled = false;
                materialButton16.Enabled = false;
                materialButton8.Enabled = false;
            }
            else
            {
                materialButton10.Enabled = true;
                materialButton12.Enabled = true;
                materialButton16.Enabled = true;
                materialButton8.Enabled = true;
                Call();

                int get_index = materialComboBox7.SelectedIndex;

                string id_name = ProfilName[get_index].Replace("--user-data-dir=", null).Replace("\n", null);

                TabInfoLabelid.Text = "ID: " + id_name;

                TextBox_lat.Text = AsyncLati;
                TextBox_long.Text = AsyncLongi;
                TextBox_PublicIP.Text = AsyncWebRTC;
                materialComboBox1.Text = AsyncTimeZone;

            }
        }
        private void materialButton10_Click_1(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 1; // = pagina fingerprint
            UpdateGUI_FromServer();

        }

        private void materialButton17_Click_1(object sender, EventArgs e)
        {
            string text = socket_udp("engine register_data " + materialTextBox2.Text + " | ", false);
            materialRichTextBox1.Text = text;
        }

        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e) // default
        {
            if (materialRadioButton1.Checked == true) // default
            {
                materialManager.ColorScheme = new MaterialColorScheme(MaterialPrimary.Green400, MaterialPrimary.BlueGrey900, MaterialPrimary.BlueGrey900, MaterialAccent.Red200, MaterialTextShade.WHITE);
                if (materialManager.Theme == MaterialManager.Themes.DARK == true)
                {
                    materialManager.Theme = MaterialManager.Themes.LIGHT;
                    materialManager.Theme = MaterialManager.Themes.DARK;

                }
                else
                {
                    materialManager.Theme = MaterialManager.Themes.DARK;
                    materialManager.Theme = MaterialManager.Themes.LIGHT;

                }
            }

        }

        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e) //red theme
        {

            materialManager.ColorScheme = new MaterialColorScheme(
                MaterialPrimary.Red500,
                MaterialPrimary.BlueGrey900,
                MaterialPrimary.Red500,
                MaterialAccent.Orange200,
                MaterialTextShade.WHITE);
            if (materialManager.Theme == MaterialManager.Themes.DARK == true)
            {
                materialManager.Theme = MaterialManager.Themes.LIGHT;
                materialManager.Theme = MaterialManager.Themes.DARK;

            }
            else
            {
                materialManager.Theme = MaterialManager.Themes.DARK;
                materialManager.Theme = MaterialManager.Themes.LIGHT;

            }


        }

        private void materialRadioButton3_CheckedChanged(object sender, EventArgs e) // silver theme
        {
            materialManager.ColorScheme = new MaterialColorScheme(
            MaterialPrimary.Purple400,
            MaterialPrimary.BlueGrey900,
            MaterialPrimary.Purple400,
            MaterialAccent.LightBlue200,
            MaterialTextShade.WHITE);
            if (materialManager.Theme == MaterialManager.Themes.DARK == true)
            {
                materialManager.Theme = MaterialManager.Themes.LIGHT;
                materialManager.Theme = MaterialManager.Themes.DARK;

            }
            else
            {
                materialManager.Theme = MaterialManager.Themes.DARK;
                materialManager.Theme = MaterialManager.Themes.LIGHT;

            }
        }

        private void materialButton19_Click_1(object sender, EventArgs e)
        {
            DefaultSettings_load();
        }

        private void materialButton12_Click_1(object sender, EventArgs e)
        {
            int get_index = materialComboBox7.SelectedIndex + 1;
            socket_udp("engine login_data " + UserID + " | delete " + get_index + " |", false);
            UpdateProfileList();

            string data_combo = materialComboBox7.GetItemText(materialComboBox7.SelectedItem);

            if (!String.IsNullOrWhiteSpace(data_combo))
            {
                materialComboBox7.SelectedIndex = 0;
            }
        }

        private void label_proxy_info_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel8_Click(object sender, EventArgs e)
        {
            materialComboBox1.Refresh();
            materialComboBox1.Update();
        }

        private void webglMeta_generate_Click(object sender, EventArgs e)
        {
            webgl_fake = socket_udp("engine webgl_metadata", false);
            webglMetaTextBox_unmasked.Text = webgl_fake;
        }

        private void canvas_generate_Click(object sender, EventArgs e)
        {
            canvas = socket_udp("engine canvas", false);

            if (canvas_block.Enabled == true)
            {
                materialLabel20.Text = "ID : " + _compress_data_(canvas);
            }
        }

        private void TextBox_long_TextChanged(object sender, EventArgs e)
        {

        }
        private string _ProfileHashGenerator(int length)
        {
            Random random = new Random();
            const string ProfileHashGenerator = "0123456789abcdefgh";
            return new string(Enumerable.Repeat(ProfileHashGenerator, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void materialButton16_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(@"driver\chrome.exe"))
            {
                using var process = new Process();

                int get_index = materialComboBox7.SelectedIndex;

                string useragent_fix = UserAgentList[get_index].Replace("--user-agent=", null);
                useragent_fix = "\"" + useragent_fix + "\"";
                string xuseragent_fix = "--user-agent=" + useragent_fix;
                string list_string = FontList[get_index] + " " + FingerprintList[get_index] + " " + xuseragent_fix + " --user-data-dir=Profile\\" +
                    ProfilName[get_index]
                    .Replace("\n", null)
                    .Replace("--user-data-dir=", null);

                process.StartInfo.FileName = @"driver\chrome.exe";
                process.StartInfo.Arguments = list_string;
                process.Start();
            }
            else
            {
                MessageBox.Show("Nu exista browserul. (driver\\chrome.exe)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Console.WriteLine(list_string);
        }

        private string FingerPrintSpoof()
        {

            int get_index = materialComboBox7.SelectedIndex;
            string temp_profile = _ProfileHashGenerator(8) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(12);
            string temp_profile2 = _ProfileHashGenerator(8) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(4) + "-" + _ProfileHashGenerator(12);
            StringBuilder sb = new StringBuilder();

            string profil_name = ProfilName[get_index].Replace("--user-data-dir=", null);
            sb.Append("{");

            sb.Append(@"""titleBar"":""");
            sb.Append(profil_name);
            sb.Append(@""",");

            sb.Append(@"""ws21"":""");
            sb.Append(temp_profile2);
            sb.Append(@""",");

            sb.Append(@"""ej45"":""");
            sb.Append(temp_profile + ".bin"); /// ???? hash profile
            sb.Append(@""",");

            sb.Append(@"""jo11"":""");
            sb.Append(temp_profile); /// ???? hash profile
            sb.Append(@""",");

            sb.Append(@"""hr94"":""");
            sb.Append(materialComboBox4.Text); // hardwareConcurrency
            sb.Append(@""",");

            sb.Append(@"""md10"":""");
            sb.Append(materialComboBox5.Text); // devicememory
            sb.Append(@""",");

            sb.Append(@"""cl13"":""");
            sb.Append(TextBox_lang.Text); // language
            sb.Append(@""",");

            sb.Append(@"""ca41"":""");
            sb.Append(TextBox_lang.Text); // language
            sb.Append(@";q=0.9"",");

            sb.Append(@"""wx10"":""");
            sb.Append(TextBox_platform.Text); // platforma
            sb.Append(@""",");

            sb.Append(@"""hz50"":""");
            sb.Append(materialComboBox3.Text);
            sb.Append(@""",");

            sb.Append(@"""pu333"":""");
            sb.Append(aloneTextBox1.Text);
            sb.Append(@""",");

            sb.Append(@"""pp91"":""");
            sb.Append(aloneTextBox3.Text);
            sb.Append(@""",");

            sb.Append(@"""pru13"":""");
            sb.Append(aloneTextBox2.Text);
            sb.Append(@""",");
            //protocol

            sb.Append(@"""prt91"":""");
            sb.Append(Convert.ToInt32(proxytype_comboBox.SelectedIndex)); ;
            sb.Append(@""",");

            sb.Append(@"""xw75"":""ab14"",");
            sb.Append(@"""pra11"":""");
            sb.Append(TextBox_PublicIP.Text);
            sb.Append(@""",");

            sb.Append(@"""xx15"":"""); //timezone
            sb.Append(materialComboBox1.Text);
            sb.Append(@""",");
            sb.Append(@"""no10"":""+03:00"",");

            sb.Append(@"""cr22"":""");
            sb.Append(ClientRects); //ClientRects spoof
            sb.Append(@""",");

            sb.Append(@"""dh65"":""");
            sb.Append("");
            sb.Append(@""",");

            sb.Append(@"""xq36"":""");
            sb.Append(textbox_videoin.Text);//video inputs
            sb.Append(@""",");

            sb.Append(@"""vd64"":""");
            sb.Append(textbox_audioin.Text); //audio inputs
            sb.Append(@""",");

            sb.Append(@"""me64"":""");
            sb.Append(textbox_audio_out.Text); //audio outputs
            sb.Append(@""",");


            sb.Append(@"""cp11"":""");
            sb.Append(canvas);
            sb.Append(@""",");
            sb.Append(@"""wpw12"":""");
            sb.Append(canvas_wid); // Width
            sb.Append(@""",");
            sb.Append(@"""wph32"":""");
            sb.Append(canvas_hei);// Height
            sb.Append(@""",");


            sb.Append(@"""cx88"":""");
            sb.Append(webglMetaTextBox_vendor.Text);// WebKit
            sb.Append(@""",");

            sb.Append(@"""vr10"":""");
            sb.Append(webglMetaTextBox_renderer.Text);// WebKit WebGL
            sb.Append(@""",");

            sb.Append(@"""vw54"":""");
            sb.Append(webglMetaTextBox_unmaskedvendor.Text);// Google Inc.
            sb.Append(@""",");


            sb.Append(@"""aw19"":""");
            sb.Append(webglMetaTextBox_unmasked.Text);
            sb.Append(@""",");
            sb.Append(@"""ap32"":""");
            sb.Append(audioContext); //_audiocontext spoof
            sb.Append(@""",");
            sb.Append(@"""xu16"":""nx64"","); // nu stiu

            sb.Append(@"""ek85"":""");
            sb.Append(TextBox_long.Text); //long
            sb.Append(@""",");
            sb.Append(@"""os58"":""");
            sb.Append(TextBox_lat.Text); //lat
            sb.Append(@""",");
            sb.Append(@"""ok33"":""");
            sb.Append(TextBox_accuracy.Text); //Accuracy
            sb.Append(@"""");

            sb.Append("}");

            return sb.ToString();

        }
        private void async_from_proxy(string server, string username, string pass, string protocol)
        {
            string async_proxy_info = socket_udp("engine async_proxy:" + server + ":" + username + ":" + pass + ":" + protocol, false);
            string[] data_async = new string[] { "" };
            data_async = async_proxy_info.Split(':');

            data_async = data_async.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string[] geolocation = new string[] { "" };
            geolocation = data_async[1].Split(',');

            AsyncTimeZone = data_async[0];
            AsyncWebRTC = data_async[2];
            AsyncCountry = data_async[3];

            AsyncLati = geolocation[0];
            AsyncLongi = geolocation[1];

            ////Console.WriteLine(data_async[0]);
            ////Console.WriteLine(data_async[1]);
            ////Console.WriteLine(data_async[2]);

        }
        private void updateProfileBtn_Click(object sender, EventArgs e)
        {
            //async_from_proxy(aloneTextBox2.Text, aloneTextBox1.Text, aloneTextBox3.Text, Convert.ToString(proxytype_comboBox.SelectedIndex));

            string x = FingerPrintSpoof().Replace("\n", null);
            //Console.WriteLine(x, Color.Red);
            int get_index = materialComboBox7.SelectedIndex;
            //Console.WriteLine(get_index + " index");

            string user_agent = "--user-agent=\"" + TextBox_useragent.Text + "\"";

            socket_udp("engine login_data " + UserID + "  | update" +

                ProfilName[get_index].Replace("\n", null) +

                ":\n" + FontList[get_index].Replace("\n", null) + ":\n--base64=" + _compress_data_(x) +

                ":\n" + user_agent.Replace("\n", null) +

                "index:" + get_index + "|", false);

            materialTabControl1.SelectedIndex = 0; // = pagina home

            string data_combo = materialComboBox7.GetItemText(materialComboBox7.SelectedItem);

            if (!String.IsNullOrWhiteSpace(data_combo))
            {
                materialComboBox7.SelectedIndex = 0;

            }

            //async_create_data(".."); // update

        }

        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServerResponseOK == false || IsConnected == false)
            {
                foreach (TabPage tab in materialTabControl1.TabPages)
                {
                    tab.Enabled = false;
                }
                (materialTabControl1.TabPages[2] as TabPage).Enabled = true;
            }
            else if (string.IsNullOrWhiteSpace(materialComboBox7.Text) == true)
            {
                foreach (TabPage tab in materialTabControl1.TabPages)
                {
                    tab.Enabled = false;
                }
                (materialTabControl1.TabPages[2] as TabPage).Enabled = true;
                (materialTabControl1.TabPages[0] as TabPage).Enabled = true;
            }
            else
            {
                (materialTabControl1.TabPages[0] as TabPage).Enabled = true;
                (materialTabControl1.TabPages[1] as TabPage).Enabled = true;
                (materialTabControl1.TabPages[2] as TabPage).Enabled = true;
            }
        }
        private void canvas_help_Click(object sender, EventArgs e)
        {
            panel9.Visible = true;
            panel9.Dock = DockStyle.Fill;
            webView22.Source = new System.Uri("https://www.youtube.com/embed/FqcZQ6lZqTE", System.UriKind.Absolute);
        }

        private void materialLabel57_Click(object sender, EventArgs e)
        {
            panel9.Dock = DockStyle.None;
            panel9.Visible = false;
            webView22.Source = new System.Uri("about:blank", System.UriKind.Absolute);
        }

        private void materialLabel17_Click(object sender, EventArgs e)
        {
            panel9.Visible = true;
            panel9.Dock = DockStyle.Fill;
            webView22.Source = new System.Uri("https://www.youtube.com/embed/FqcZQ6lZqTE", System.UriKind.Absolute);
            metroTabControl1.SelectedIndex = 7;
        }

        private void materialButton8_Click_1(object sender, EventArgs e)
        {
            if (File.Exists("SeleniumTest.exe"))
            {
                Process.Start("SeleniumTest.exe");
            }
            else
            {
                MessageBox.Show("Nu exista nici un test pentru selenium", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}