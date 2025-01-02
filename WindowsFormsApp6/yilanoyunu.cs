using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace WindowsFormsApp6
{
    public partial class yilanoyunu : Form
    {
        private Label _yilanKafasi;
        private int _yilanParcasiArasiMesafe = 2;

        private int _yilanParcasiSayisi;
        private int _yilanBoyutu = 15;
        private int _yemBoyutu = 15;
        private Label _yem;
        private Random _random;
        private HareketYonu _yon;

        public yilanoyunu()
        {
            InitializeComponent();
            _random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblpuan.Text = "0";
            lblsure.Text = "0";


            _yilanParcasiSayisi = 0;
            YemOlustur();

            YeminYeriniDegistir();
            YilaniYerlestir();
            timerYilanHareket.Enabled=true;
            timersaat.Enabled = true;

        }
        private void YenidenBaslat()
        {
            this.pnl.Controls.Clear();
            YemOlustur();
            _yilanParcasiSayisi = 0;           
            YeminYeriniDegistir();
            YilaniYerlestir();
            lblpuan.Text = "0";
            lblsure.Text = "0";

            timerYilanHareket.Enabled = true;
            timersaat.Enabled = true;




        }
        private Label YilanParcasiOlustur(int locationX, int locationY)
        {

            _yilanParcasiSayisi++;


            Label lbl = new Label()
            {
                Name = "yilanparca" + _yilanParcasiSayisi,
                BackColor = Color.Red,
                Width = _yilanBoyutu,
                Height = _yilanBoyutu,
                Location = new Point(locationX, locationY)
            };
            this.pnl.Controls.Add(lbl);
            return lbl;
        }
        private void YilaniYerlestir()
        {
            _yilanKafasi = YilanParcasiOlustur(0, 0);
            _yilanKafasi.Text = ":";
            _yilanKafasi.TextAlign = ContentAlignment.MiddleCenter;
            _yilanKafasi.ForeColor = Color.White;
            var locationX = (pnl.Width / 2) - (_yilanKafasi.Width / 2);
            var locationY = (pnl.Height / 2) - (_yilanKafasi.Height / 2);
            _yilanKafasi.Location = new Point(locationX, locationY);

        }
        private void YemOlustur()
        {


            Label lbl = new Label()
            {
                Name = "yem",
                BackColor = Color.Yellow,
                Width = _yemBoyutu,
                Height = _yemBoyutu,

            };
            _yem = lbl;

            this.pnl.Controls.Add(lbl);

        }
        private void YeminYeriniDegistir()
        {
            var locationX = 0;
            var locationY = 0;
            bool durum;
            do
            {
                durum = false;
                locationX = _random.Next(0, pnl.Width - _yemBoyutu);
                locationY = _random.Next(0, pnl.Height - _yemBoyutu);
                var rect1 = new Rectangle(new Point(locationX, locationY), _yem.Size);
                foreach (Control control in pnl.Controls)
                {
                    if (control is Label && control.Name.Contains("yilanparca"))
                    {
                        var rec2 = new Rectangle(control.Location, control.Size);
                        if (rect1.IntersectsWith(rec2))
                        {
                            durum = true;
                            break;
                        }

                    }
                }

            } while (durum);

            _yem.Location = new Point(locationX, locationY);

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
            YilanKafasiniTakipEt();
            YilaniYurut();

            YilanYemiYedimi();

            //OyunBittimi();

        }



        private void YilaniYurut()
        {
            var locationX = _yilanKafasi.Location.X;
            var locationY = _yilanKafasi.Location.Y;

            switch (_yon)
            {
                case HareketYonu.Yukari:
                    _yilanKafasi.Location = new Point(locationX, locationY -
                        (_yilanKafasi.Width - _yilanParcasiArasiMesafe));
                    break;



                case HareketYonu.Asagi:
                    _yilanKafasi.Location = new Point(locationX, locationY +
                        (_yilanKafasi.Width - _yilanParcasiArasiMesafe)); break;




                case HareketYonu.Sola:
                    _yilanKafasi.Location = new Point(locationX -
                        (_yilanKafasi.Width - _yilanParcasiArasiMesafe), locationY);
                    break;




                case HareketYonu.Saga:
                    _yilanKafasi.Location = new Point(locationX +
                       (_yilanKafasi.Width - _yilanParcasiArasiMesafe), locationY); break;




                default:
                    break;

            }
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


        private void YilanYemiYedimi()
        {
            var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);
            var rect2 = new Rectangle(_yem.Location, _yem.Size);
            if(rect1.IntersectsWith(rect2))

            {
                lblpuan.Text = (Convert.ToInt32(lblpuan.Text) + 10).ToString();
                YeminYeriniDegistir();
                YilanParcasiOlustur(-_yilanBoyutu, -_yilanBoyutu);


            }




        }
        private void YilanKafasiniTakipEt()
        {
            if (_yilanParcasiSayisi <= 1) return;

            for(int i=_yilanParcasiSayisi;1<i;i--)
            {
                var sonrakiParca = (Label)pnl.Controls[i];
                var oncekiParca = (Label)pnl.Controls[i-1];
                sonrakiParca.Location=oncekiParca.Location;


            }
        }

        private void timersaat_Tick(object sender, EventArgs e)
        {
            lblsure.Text = (Convert.ToInt32(lblsure.Text)+1).ToString();
        }
    }
}
