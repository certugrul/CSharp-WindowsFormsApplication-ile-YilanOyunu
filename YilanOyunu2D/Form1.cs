using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace YilanOyunu2D
{
    public partial class Form1 : Form
    {
        private Label _yilanKafasi;
        private int _yilanParcasiArasiMesafe = 2;
        private int _yilanParcasiSayisi;
        private int _yilanBoyutu=20;
        private int _yemBoyutu = 10;
        private Random _random;
        private Label _yem;
        private HareketYonu _yon;
        public Form1()
        {
            InitializeComponent();
            _random= new Random();
        }

    
        private void Form1_Load(object sender, EventArgs e)
        {

            Sifirla();
           
            
        }

        private void yenidenBaslat()
        {
            this.panel.Controls.Clear();
           
            lblPuan.Text = "0";
            lblSure.Text = "0";

            Sifirla();
        }
        public void Sifirla()
        {
            panel.Controls.Clear();
            _yilanParcasiSayisi = 0;

            yemOlustur();
            yilaniYerlestir();
            yeminYeriniDegistir();
            _yon = HareketYonu.Saga;
            timerYilanHareket.Enabled = true;
            timerSaat.Enabled = true;

        }
        //public void kontrol_KeyDown(KeyEventArgs e)
        //{
        //    var keyCode = e.KeyCode;
        //    switch (keyCode)
        //    {
        //        case Keys.Enter:
        //            MessageBox.Show("a");
        //            break;
        //        default: break;
        //    }
        //}

        private Label yilanParcasiOlustur(int locationX, int locationY)
        {
            _yilanParcasiSayisi++;

            Label lbl=new Label()
            {
                Name = "yilanParca" + _yilanParcasiSayisi,
                BackColor = Color.Red,
                Width= _yilanBoyutu,
                Height= _yilanBoyutu,
                Location=new Point(locationX, locationY)

            };

            this.panel.Controls.Add(lbl);
            return lbl;
        }
        private void yilaniYerlestir()
        {
            
            _yilanKafasi = yilanParcasiOlustur(0, 0);
            _yilanKafasi.BackColor = Color.Purple;
            _yilanKafasi.Text = ":)";
            _yilanKafasi.TextAlign=ContentAlignment.MiddleCenter;
            _yilanKafasi.ForeColor= Color.Red;
            _yilanKafasi.BorderStyle = BorderStyle.FixedSingle;

           var LocationX=(panel.Width/2) - (_yilanKafasi.Width/2);
            var LocationY = (panel.Height / 2) - (_yilanKafasi.Height / 2);
           _yilanKafasi.Location=new Point(LocationX, LocationY);

        }

        private void yemOlustur()
        {

            Label lbl = new Label()
            {
                Name = "yilanParca" + _yilanParcasiSayisi,
                BackColor = Color.Red,
                BorderStyle= BorderStyle.FixedSingle,
                Width = _yemBoyutu,
                Height = _yemBoyutu,
                //Location = new Point(locationX, locationY)

            };
            _yem = lbl;

            this.panel.Controls.Add(lbl);
          
        }

        private void yeminYeriniDegistir()
        {
            var LocationX = 0;
            var LocationY = 0;
            
            bool durum;
            do
            {

                durum = false;
                 LocationX = _random.Next(0, panel.Width - _yemBoyutu);
                 LocationY = _random.Next(0, panel.Height - _yemBoyutu);

                var rect1 = new Rectangle(new Point(LocationX, LocationY), _yem.Size);
                foreach (Control control in panel.Controls)
                {
                    if (control is Label && control.Name.Contains("yilanParca"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);
                        if (rect1.IntersectsWith(rect2))
                        {
                            durum = true;
                        }
                    }
                }
            } while (durum);

            _yem.Location =new Point(LocationX, LocationY);


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
            if (_yon == HareketYonu.Sola && keyCode == Keys.Right ||
                _yon == HareketYonu.Saga && keyCode == Keys.Left ||
                _yon == HareketYonu.Yukari && keyCode == Keys.Down ||
              _yon == HareketYonu.Asagi && keyCode == Keys.Up) return;

            switch (keyCode)
            {
                case Keys.Up:
                    _yon = HareketYonu.Yukari;
                    break;
                case Keys.Down:
                    _yon = HareketYonu.Asagi;
                    break;
                case Keys.Left:
                    _yon = HareketYonu.Sola;
                    break;
                case Keys.Right:
                    _yon = HareketYonu.Saga;
                    break;
                case Keys.P:
                   
                    timerYilanHareket.Enabled = false;
                    timerSaat.Enabled = false;
                    DialogResult sonuc = MessageBox.Show("Oyunu durdunuz. Devam etmek ister misiniz?", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(sonuc == DialogResult.Yes)
                    {
                        timerYilanHareket.Enabled = true;
                        timerSaat.Enabled = true;

                    }
                    else
                    {
                        Application.Exit();

                    }

                    break;

                default:
                    break;
            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            yilanKafasiniTakipEt();
            yilaniYurut();
            yilanYemiYedimi();
            oyunBittimi();

        }

        private void oyunBittimi()
        {
            bool oyunBittimi = false;

            var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);

            foreach  (Control control in panel.Controls)
            {
                if (control is Label && control.Name.Contains("yilanParca") && control.Name !=_yilanKafasi.Name)
                {
                    var rect2=new Rectangle(control.Location, control.Size);

                    if(rect1.IntersectsWith(rect2))
                    {
                        oyunBittimi = true;
                        timerSaat.Enabled=false;

                        break;
                    }
                }
            }

            if(oyunBittimi)
            {
                timerYilanHareket.Enabled = false;
               DialogResult sonuc =   MessageBox.Show($"Süre Skorunuz:{lblSure.Text}    |  Puanınız:{lblPuan.Text} \n \n Oyuna devam etmek ister misiniz?", "Oyun Bitti :)",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
               if(sonuc==DialogResult.Yes)
                {
                    yenidenBaslat();
                }
                else
                {
                    DialogResult soru = MessageBox.Show("Eğer hayır butonuna basarsanız, oyundan çıkacaksınız! Devam etmek istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (soru==DialogResult.Yes)
                    Application.Exit();
                    else
                    {
                        yenidenBaslat();
                    }
                }
               
            }
        }

        private void yilaniYurut()
        {
            var locationX = _yilanKafasi.Location.X;
            var locationY = _yilanKafasi.Location.Y;
            switch (_yon)
            {
                case HareketYonu.Yukari:
                    _yilanKafasi.Location = new Point(locationX, locationY - (_yilanKafasi.Width + _yilanParcasiArasiMesafe));
                    if(locationY<0)
                    {
                        _yilanKafasi.Location = new Point(locationX ,500);
                    }
                    break;
                case HareketYonu.Asagi:
                    _yilanKafasi.Location = new Point(locationX, locationY + (_yilanKafasi.Width + _yilanParcasiArasiMesafe));
                    if(locationY>500)
                    {
                        _yilanKafasi.Location = new Point(locationX, 0);
                    }
                    break;
                case HareketYonu.Sola:
                    _yilanKafasi.Location = new Point(locationX - (_yilanKafasi.Width + _yilanParcasiArasiMesafe), locationY);
                    if(locationX<0)
                    {
                        _yilanKafasi.Location = new Point(700, locationY);
                    }
                    break;
                case HareketYonu.Saga:
                    _yilanKafasi.Location = new Point(locationX + (_yilanKafasi.Width + _yilanParcasiArasiMesafe), locationY);
                    if (locationX > 700)
                    {
                        _yilanKafasi.Location = new Point(0, locationY);
                    }
                    break;
                default:
                    break;
            }
        }

        private void yilanYemiYedimi()
        {
            var rect1 = new Rectangle(_yilanKafasi.Location, _yilanKafasi.Size);
            var rect2 =new Rectangle(_yem.Location,_yem.Size);

            if(rect1.IntersectsWith(rect2))
            {
                lblPuan.Text=(int.Parse(lblPuan.Text)+10).ToString();
                if (int.Parse(lblPuan.Text) >int.Parse(lblEnYuksekPuan.Text))
                { 
                lblEnYuksekPuan.Text = (int.Parse(lblEnYuksekPuan.Text) + 10).ToString();

                }
                yeminYeriniDegistir();
                yilanParcasiOlustur(-_yilanBoyutu, -_yilanBoyutu);
                
            }

        }
        private void yilanKafasiniTakipEt()
        {
            if (_yilanParcasiSayisi <= 1) return;
            
            for (int i = _yilanParcasiSayisi; i > 1; i--)
            {
                var sonrakiParca = (Label)panel.Controls[i];
                var oncekiParca= (Label)panel.Controls[i-1];
                sonrakiParca.Location = oncekiParca.Location;
            }
            
        }

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            lblSure.Text = (int.Parse(lblSure.Text) + 1).ToString();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            label6.Text = "Yön Tuşu-Yukarı \n Yön Tuşu-Aşağı \n  Yön Tuşu-Sola \n Yön Tuşu-Sağa \n P-Duraklat";
        }
    }
}
