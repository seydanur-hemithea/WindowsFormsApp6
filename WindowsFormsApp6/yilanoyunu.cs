using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

using Newtonsoft.Json; // NuGet'ten ekle


namespace WindowsFormsApp6
{
    public partial class yilanoyunu : Form
    {
        private Label _yilanKafasi;
        private int _yilanParcasiArasiMesafe = 2;

        private int _yilanParcasiSayisi;
        private int _rakipYilanParcasiSayisi;
        private int _yilanBoyutu = 15;
        private int _yemBoyutu = 15;
        private Label _yem;
        private Random _random;
        private HareketYonu _yon;
        private Dictionary<string, Dictionary<HareketYonu, double>> QTable
    = new Dictionary<string, Dictionary<HareketYonu, double>>();
        private Random rnd = new Random();
        private double alpha = 0.1;   // öğrenme oranı
        private double gamma = 0.9;   // geleceği dikkate alma
        private double epsilon = 0.1; // keşif oranı

        // Rakip yılan kafası ve yön alanları (eğer eklemediysen)
        private Label _rakipYilanKafasi;
        private HareketYonu _rakipYon;

        private List<Label> _oyuncuYilan = new List<Label>();
        private List<Label> _rakipYilan = new List<Label>();
        private Label lblRakipPuan;

        public yilanoyunu()
        {
            InitializeComponent();


            
            _random = new Random();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Dekor ve Q-Tablosu yükleme
            DuvarlariOlustur();
            LoadQTable();

            // Başlangıç puan ve süre
            lblpuan.Text = "0";
            lblsure.Text = "0";
            RakipPuanGost.Text = "0";

            // Oyuncu ve rakip yılan listelerini başlat
            _oyuncuYilan = new List<Label>();
            _rakipYilan = new List<Label>();

            // Oyuncu yılan kafası
            _yilanKafasi = YeniParca(Color.Red, new Point(60, 120));
            _oyuncuYilan.Add(_yilanKafasi);
            pnl.Controls.Add(_yilanKafasi);

            // Rakip yılan kafası
            _rakipYilanKafasi = YeniParca(Color.Blue, new Point(300, 120));
            _rakipYilan.Add(_rakipYilanKafasi);
            pnl.Controls.Add(_rakipYilanKafasi);
            // Form1_Load içinde veya Designer'da
            lblRakipPuan = new Label()
            {
                Name = "lblRakipPuan",
                Text = "0",
                ForeColor = Color.Blue,
                Location = new Point(10, 60), // oyuncu puanının altına koyabilirsin
                AutoSize = true
            };
            this.Controls.Add(lblRakipPuan);

            // Yem oluştur ve yerleştir
            EnsureFoodExists();

            // RL başlangıç durumu
            string initialState = GetState(_rakipYilanKafasi.Location, _yem.Location);
            if (!QTable.ContainsKey(initialState))
            {
                QTable[initialState] = new Dictionary<HareketYonu, double>()
        {
            { HareketYonu.Yukari, 0 },
            { HareketYonu.Asagi, 0 },
            { HareketYonu.Sola, 0 },
            { HareketYonu.Saga, 0 }
        };
            }

            // Timer ayarları
            timerYilanHareket.Interval = 150;
            timerYilanHareket.Enabled = true;

            timersaat.Interval = 1000;
            timersaat.Enabled = true;

            // Klavye olayları
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;


        }
        private void YenidenBaslat()
        {
            this.pnl.Controls.Clear();
            EnsureFoodExists();
            _yilanParcasiSayisi = 0;
            YemiRasgeleYerlestir();
            YilaniYerlestir();
            lblpuan.Text = "0";
            lblsure.Text = "0";


            timerYilanHareket.Enabled = true;
            timersaat.Enabled = true;





        }
        private Label YeniParca(Color color, Point loc)
        {
            return new Label
            {
                BackColor = color,
                Width = _yilanBoyutu,
                Height = _yilanBoyutu,
                Location = loc
            };
        }
        private void EnsureFoodExists()
        {
            if (_yem == null || _yem.IsDisposed)
            {
                _yem = new Label
                {
                    BackColor = Color.Lime,
                    Width = _yemBoyutu,
                    Height = _yemBoyutu
                };
                pnl.Controls.Add(_yem);
            }
            YemiRasgeleYerlestir();
        }

        private void YemiRasgeleYerlestir()
        {
            int x, y;
            bool conflict;
            do
            {
                conflict = false;
                x = _random.Next(0, pnl.Width / _yemBoyutu) * _yemBoyutu;
                y = _random.Next(0, pnl.Height / _yemBoyutu) * _yemBoyutu;

                var rectFood = new Rectangle(new Point(x, y), new Size(_yemBoyutu, _yemBoyutu));

                // Oyuncu yılan parçalarıyla çakışma kontrolü
                foreach (var seg in _oyuncuYilan)
                    if (rectFood.IntersectsWith(new Rectangle(seg.Location, seg.Size)))
                        conflict = true;

                // Rakip yılan parçalarıyla çakışma kontrolü
                foreach (var seg in _rakipYilan)
                    if (rectFood.IntersectsWith(new Rectangle(seg.Location, seg.Size)))
                        conflict = true;

            } while (conflict);

            _yem.Location = new Point(x, y);
        }
        private void YilaniYerlestir()
        {
            // Oyuncu yılan kafasını oluştur
            _yilanKafasi = YeniParca(Color.Red, new Point(0, 0));
            _yilanKafasi.Text = ":";
            _yilanKafasi.TextAlign = ContentAlignment.MiddleCenter;
            _yilanKafasi.ForeColor = Color.White;

            // Ortaya yerleştir
            var locationX = (pnl.Width / 2) - (_yilanKafasi.Width / 2);
            var locationY = (pnl.Height / 2) - (_yilanKafasi.Height / 2);
            _yilanKafasi.Location = new Point(locationX, locationY);

            // Listeye ve panele ekle
            _oyuncuYilan.Add(_yilanKafasi);
            pnl.Controls.Add(_yilanKafasi);
        }
        private enum HareketYonu
        {
            Yukari,
            Asagi,
            Sola,
            Saga
        }
           


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;
            if(_yon==HareketYonu.Sola&&keyCode==Keys.D||
                _yon==HareketYonu.Saga&&keyCode==Keys.A||
                _yon == HareketYonu.Yukari && keyCode == Keys.S|| 
                _yon == HareketYonu.Asagi && keyCode == Keys.W)

            {
                return;
            }
            switch (keyCode)
            {
                case Keys.W:
                    _yon=HareketYonu.Yukari; break;


                case Keys.S:
                    _yon = HareketYonu.Asagi; break;



                case Keys.A:
                    _yon = HareketYonu.Sola; break;


                case Keys.D:
                    _yon = HareketYonu.Saga; break;

                case Keys.P:
                    timersaat.Enabled = false;
                    timerYilanHareket.Enabled = false;
                    break;

                case Keys.C:
                    timersaat.Enabled = true ;
                    timerYilanHareket.Enabled = true;
                    break;


                default:
                    break;





            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            // 1) Parçaları takip ettir
            TakipGuncelle(_oyuncuYilan);
            TakipGuncelle(_rakipYilan);

            // 2) Oyuncu yılanı hareket ettir
            YilaniYurut();

            // 3) Rakip yılanı RL ile hareket ettir
            string state = GetState(_rakipYilanKafasi.Location, _yem.Location);
            HareketYonu action = ChooseAction(state);
            RakipYilaniYurut(action);

            // 4) Yem kontrolü
            OyuncuYemKontrolu();
            RakipYilanYemiYedimi();

            // 5) RL güncellemesi
            string nextState = GetState(_rakipYilanKafasi.Location, _yem.Location);
            double reward = CalculateReward(_rakipYilanKafasi.Location, _yem.Location);
            UpdateQ(state, action, reward, nextState);
        }
        private void TakipGuncelle(List<Label> yilan)
        {
            if (yilan == null || yilan.Count <= 1) return;

            for (int i = yilan.Count - 1; i >= 1; i--)
            {
                yilan[i].Location = yilan[i - 1].Location;
            }
        }

        //OyunBittimi();



        private double CalculateReward(Point rivalPos, Point foodPos)
        {
            // Panel dışına çıktı mı?
            bool outOfBounds = rivalPos.X < 0 || rivalPos.Y < 0
                            || rivalPos.X + _rakipYilanKafasi.Width > pnl.Width
                            || rivalPos.Y + _rakipYilanKafasi.Height > pnl.Height;

            if (outOfBounds) return -10.0;

            // Yemi yedi mi?
            var rivalRect = new Rectangle(rivalPos, _rakipYilanKafasi.Size);
            var foodRect = new Rectangle(foodPos, _yem.Size);
            if (rivalRect.IntersectsWith(foodRect)) return +10.0;

            // Adım cezası
            double stepPenalty = -0.1;

            // Mesafe bonusu (yaklaşma/uzaklaşma)
            double dist = Math.Abs(rivalPos.X - foodPos.X) + Math.Abs(rivalPos.Y - foodPos.Y);
            // Burada sadece mesafeyi kullanıyoruz, istersen önceki pozisyonu da parametre olarak ekleyip karşılaştırabilirsin.
            double approachBonus = -0.01 * dist; // uzaksa daha fazla ceza, yakınsa daha az

            return stepPenalty + approachBonus;
        }

        private void UpdateQ(string state, HareketYonu action, double reward, string nextState)
        {
            // Eğer state yoksa ekle
            if (!QTable.ContainsKey(state))
            {
                QTable[state] = new Dictionary<HareketYonu, double>()
        {
            { HareketYonu.Yukari, 0 },
            { HareketYonu.Asagi,  0 },
            { HareketYonu.Sola,   0 },
            { HareketYonu.Saga,   0 }
        };
            }

            if (!QTable.ContainsKey(nextState))
            {
                QTable[nextState] = new Dictionary<HareketYonu, double>()
        {
            { HareketYonu.Yukari, 0 },
            { HareketYonu.Asagi,  0 },
            { HareketYonu.Sola,   0 },
            { HareketYonu.Saga,   0 }
        };
            }

            double oldValue = QTable[state][action];
            double nextMax = QTable[nextState].Values.Max();

            // Q-learning güncelleme formülü
            double newValue = oldValue + alpha * (reward + gamma * nextMax - oldValue);
            QTable[state][action] = newValue;
        }

        private HareketYonu ChooseAction(string state)
        {
            // Eğer state yoksa ekle
            if (!QTable.ContainsKey(state))
            {
                QTable[state] = new Dictionary<HareketYonu, double>()
        {
            { HareketYonu.Yukari, 0 },
            { HareketYonu.Asagi,  0 },
            { HareketYonu.Sola,   0 },
            { HareketYonu.Saga,   0 }
        };
            }

            // Keşif (random) veya sömürü (en iyi aksiyon)
            if (rnd.NextDouble() < epsilon)
            {
                // Rastgele yön seç
                Array values = Enum.GetValues(typeof(HareketYonu));
                return (HareketYonu)values.GetValue(rnd.Next(values.Length));
            }
            else
            {
                // En yüksek Q değerine sahip yönü seç
                return QTable[state].OrderByDescending(kv => kv.Value).First().Key;
            }
        }

        private void YilaniYurut()
        {
            int locationX = _yilanKafasi.Location.X;
            int locationY = _yilanKafasi.Location.Y;
            int step = _yilanKafasi.Width - _yilanParcasiArasiMesafe;

            // Önce yeni koordinatları hesapla
            switch (_yon)
            {
                case HareketYonu.Yukari:
                    locationY -= step;
                    break;

                case HareketYonu.Asagi:
                    locationY += step;
                    break;

                case HareketYonu.Sola:
                    locationX -= step;
                    break;

                case HareketYonu.Saga:
                    locationX += step;
                    break;
            }

            // Panel sınır kontrolü
            if (locationX < 0) locationX = 0;
            if (locationY < 0) locationY = 0;
            if (locationX + _yilanKafasi.Width > pnl.Width)
                locationX = pnl.Width - _yilanKafasi.Width;
            if (locationY + _yilanKafasi.Height > pnl.Height)
                locationY = pnl.Height - _yilanKafasi.Height;

            // Son olarak Location’ı güncelle
            _yilanKafasi.Location = new Point(locationX, locationY);
        }



        //private void OyunBittimi()
        //{
        //    bool oyunBittimi = false;
        //    var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);


        //    foreach (Control control in pnl.Controls)
        //    {

        //        if (control is Label && control.Name.Contains("yilanparca") && control.Name != _yilanKafasi.Name)
        //        {
        //            var rect2 = new Rectangle(control.Location, control.Size);

        //            if (rect1.IntersectsWith(rect2))
        //            {
        //                oyunBittimi = true;
        //                break;

        //            }

        //        }
        //    }

        //    if (oyunBittimi)
        //    {
        //        timerYilanHareket.Enabled = false;
        //        timersaat.Enabled = false;
        //        DialogResult sonuc = MessageBox.Show("Puanınız:" + lblpuan.Text,
        //            "Oyun bitti!"
        //            , MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        //        if (sonuc == DialogResult.OK)
        //        {
        //            YenidenBaslat();
        //        }
        //    }
        //}


        private void OyuncuYemKontrolu()
        {
            var rectHead = new Rectangle(_oyuncuYilan[0].Location, _oyuncuYilan[0].Size);
            var rectFood = new Rectangle(_yem.Location, _yem.Size);

            if (rectHead.IntersectsWith(rectFood))
            {
                // Puan artır
                lblpuan.Text = (Convert.ToInt32(lblpuan.Text) + 10).ToString();

                // Yem yerini değiştir
                YemiRasgeleYerlestir();

                // Yeni parça ekle (son parçanın konumunda başlat)
                var tail = _oyuncuYilan[_oyuncuYilan.Count - 1];
                var yeni = YeniParca(Color.Red, tail.Location);
                _oyuncuYilan.Add(yeni);
                pnl.Controls.Add(yeni);
            }
        }
       

        
      private void timersaat_Tick(object sender, EventArgs e)
        {
            lblsure.Text = (Convert.ToInt32(lblsure.Text) + 1).ToString();

            if (Convert.ToInt32(lblsure.Text) >= 180) // 180 saniye sonra bitir
            {
                OyunuBitir();
            }
        }


        private void RakipYilaniYerlestir()
        {
            // Rakip yılan kafasını oluştur
            _rakipYilanKafasi = YeniParca(Color.Blue, new Point(pnl.Width - 50, pnl.Height - 50));
            _rakipYilanKafasi.Name = "rakipyilan";

            // Listeye ve panele ekle
            _rakipYilan.Add(_rakipYilanKafasi);
            pnl.Controls.Add(_rakipYilanKafasi);

            // Başlangıç yönü sola olsun
            _rakipYon = HareketYonu.Sola;
        }
        
        private void RakipYilaniYurut(HareketYonu action)
        {
            int step = _yilanBoyutu - _yilanParcasiArasiMesafe;
            int x = _rakipYilan[0].Location.X;
            int y = _rakipYilan[0].Location.Y;

            switch (action)
            {
                case HareketYonu.Yukari: y -= step; break;
                case HareketYonu.Asagi: y += step; break;
                case HareketYonu.Sola: x -= step; break;
                case HareketYonu.Saga: x += step; break;
            }

            // Panel sınır kontrolü
            if (x < 0) { x = 0; _rakipYon = HareketYonu.Saga; }
            if (y < 0) { y = 0; _rakipYon = HareketYonu.Asagi; }
            if (x + _rakipYilan[0].Width > pnl.Width)
            {
                x = pnl.Width - _rakipYilan[0].Width;
                _rakipYon = HareketYonu.Sola;
            }
            if (y + _rakipYilan[0].Height > pnl.Height)
            {
                y = pnl.Height - _rakipYilan[0].Height;
                _rakipYon = HareketYonu.Yukari;
            }

            // Kafayı güncelle
            _rakipYilan[0].Location = new Point(x, y);

            // Parçaları takip ettir
            TakipGuncelle(_rakipYilan);
        }
        private string GetState(Point rivalPos, Point foodPos)
        {
            // Adım boyutuna göre grid koordinatlarını hesapla
            int step = _yilanBoyutu - _yilanParcasiArasiMesafe;

            int rx = rivalPos.X / step;
            int ry = rivalPos.Y / step;
            int fx = foodPos.X / step;
            int fy = foodPos.Y / step;

            // State stringi: rakip ve yem koordinatları
            return $"R:{rx},{ry}|F:{fx},{fy}";
        }
        private void DuvarlariOlustur()
        {
            int kalinlik = 10;

            Label solDuvar = new Label()
            {
                BackColor = Color.Black,
                Location = new Point(0, 0),
                Size = new Size(kalinlik, pnl.Height)
            };
            Label sagDuvar = new Label()
            {
                BackColor = Color.Black,
                Location = new Point(pnl.Width - kalinlik, 0),
                Size = new Size(kalinlik, pnl.Height)
            };
            Label ustDuvar = new Label()
            {
                BackColor = Color.Black,
                Location = new Point(0, 0),
                Size = new Size(pnl.Width, kalinlik)
            };
            Label altDuvar = new Label()
            {
                BackColor = Color.Black,
                Location = new Point(0, pnl.Height - kalinlik),
                Size = new Size(pnl.Width, kalinlik)
            };

            pnl.Controls.Add(solDuvar);
            pnl.Controls.Add(sagDuvar);
            pnl.Controls.Add(ustDuvar);
            pnl.Controls.Add(altDuvar);
        }

        private void RakipYilanYemiYedimi()
        {
            // Rakip kafası ile yem çarpışıyor mu?
            var rectRival = new Rectangle(_rakipYilan[0].Location, _rakipYilan[0].Size);
            var rectFood = new Rectangle(_yem.Location, _yem.Size);

            if (rectRival.IntersectsWith(rectFood))
            {
                // Puan artır (istersen ayrı label kullanabilirsin)
                RakipPuanGost.Text = (Convert.ToInt32(RakipPuanGost.Text) + 10).ToString();

                // Yem yerini değiştir
                YemiRasgeleYerlestir();

                // Yeni parça ekle (son parçanın konumunda başlat)
                var tail = _rakipYilan[_rakipYilan.Count - 1];
                var yeni = YeniParca(Color.Blue, tail.Location);

                // Listeye ve panele ekle
                _rakipYilan.Add(yeni);
                pnl.Controls.Add(yeni);

                // 🔹 Takip güncelleme fonksiyonunu çağırmayı unutma!
                TakipGuncelle(_rakipYilan);
            }
        }

        private void SaveQTable()
        {
            string json = JsonConvert.SerializeObject(QTable, Formatting.Indented);
            File.WriteAllText("qtable.json", json);
        }
        private void LoadQTable()
        {
            if (File.Exists("qtable.json"))
            {
                string json = File.ReadAllText("qtable.json");
                QTable = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<HareketYonu, double>>>(json);
            }
        }
        private void yilanoyunu_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveQTable();   // oyun kapanırken Q-Tablosunu kaydet
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveQTable();   // Q tablosunu kaydet
            base.OnFormClosing(e);
        }

        private void lblsure_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void OyunuBitir()
        {
            // Hareket ve süre timerlarını durdur
            timerYilanHareket.Stop();
            timersaat.Stop();

            // Puanları al
            int oyuncuPuan = Convert.ToInt32(lblpuan.Text);
            int rakipPuan = Convert.ToInt32(RakipPuanGost.Text);

            // Kazananı belirle
            string mesaj;
            if (oyuncuPuan > rakipPuan)
                mesaj = "🎉 Oyuncu kazandı!";
            else if (rakipPuan > oyuncuPuan)
                mesaj = "🤖 Rakip yılan kazandı!";
            else
                mesaj = "🤝 Berabere!";

            // Mesaj kutusu ile göster
            MessageBox.Show(mesaj, "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
