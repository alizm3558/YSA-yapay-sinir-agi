using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ogrenme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTableCollection dtc;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFile=new OpenFileDialog() {Title="Excel" })
            {
                if (openFile.ShowDialog()==DialogResult.OK)
                {
                    textBox1.Text = openFile.FileName;
                    using ( var stream=File.Open(openFile.FileName,FileMode.Open,FileAccess.Read))
                    {
                        using (IExcelDataReader excelreader=ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet resault = excelreader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable=(x)=>new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow=true
                                }
                            }
                                
                                );
                            dtc = resault.Tables;
                            comboBox1.Items.Clear();
                            foreach (DataTable table in dtc) comboBox1.Items.Add(table.TableName);
                         }
                    }
                }

            }
            label3.Visible = true;
            comboBox1.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = dtc[comboBox1.SelectedIndex];
            dataGridView1.DataSource = dt;
            button3.Visible = true;

           // button2.Visible = true;
        }

        // veri setini oranlıyor
        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox2.Text != "" && Convert.ToInt32(textBox2.Text) <= 100)
            {

                egitimListesi.Items.Clear();
                dataGridRowsColorReset();
                DataGridViewCellStyle style = new DataGridViewCellStyle();

                style.BackColor = Color.Aqua;

                int sat = dataGridView1.Rows.Count;
                int oranlanmisSayi = (sat * Convert.ToInt32(textBox2.Text)) / 100;
                Boolean anahtar = false, anahtar2;
                Random rastgele = new Random();

                egitimListesi.Items.Add(1);

                do
                {
                    anahtar = false;
                    anahtar2 = true;
                    int uretilenSayi = rastgele.Next(0, sat - 1);

                    for (int i = 0; i < egitimListesi.Items.Count; i++)
                    {
                        if (uretilenSayi == Convert.ToInt32(egitimListesi.Items[i]))
                        {
                            anahtar2 = false;

                        }



                    }

                    if (anahtar2 == true)
                    {
                        egitimListesi.Items.Add(uretilenSayi);
                    }
                    if (oranlanmisSayi - 1 == egitimListesi.Items.Count)
                    {
                        anahtar = true;
                    }



                } while (anahtar == false);



                for (int a = 0; a < egitimListesi.Items.Count; a++)
                {
                    dataGridView1.Rows[Convert.ToInt32(egitimListesi.Items[a])].DefaultCellStyle = style;
                }


                MessageBox.Show("Eğitim için " + egitimListesi.Items.Count + " satır veri ayrıldı.");

            }
            else
            {
                MessageBox.Show("Alan boş ve girilen değer 100'den büyük olamaz!");
            }
            /////////////////////////////////////////////

           
            label6.Visible = true;
            listBox1.Visible = true;


        }


        //normalizasyon
        private void button3_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();


            int sat, sut;
            sat = dataGridView1.RowCount;
            sut = dataGridView1.ColumnCount;
            double[,] gondereilecekDizi = new double[sut, sat];

            for (int a=0;a<sut;a++)
            {
                for (int b=0;b<sat-1;b++)
                {
                    gondereilecekDizi[a, b] =Convert.ToDouble( dataGridView1.Rows[b].Cells[a].Value);
                }
            }


            normalizasonYap(gondereilecekDizi, (sat-1), sut);

            groupBox1.Visible = true;

        }



        public void normalizasonYap(double[,] dizi, int satir, int sutun)
        {
            double max, min;
            double[,] normalizeDizi = new double[sutun, satir];
            double[] yeniDizi = new double[satir];

            for (int a = 0; a < sutun; a++)
            {
                for (int i = 0; i < satir; i++)
                {
                    yeniDizi[i] = dizi[a, i];
                }
                // dizi = dizi.Where((source, index) => index != 2).ToArray();     
                
                max = yeniDizi.Max();
                min = yeniDizi.Min();

                for (int c = 0; c < satir; c++)
                {
                    normalizeDizi[a,c] = ((dizi[a, c] - min) / (max - min));

                    dataGridView1.Rows[c].Cells[a].Value= normalizeDizi[a, c].ToString(); // normalize edip datagride aktarıyor, başarılı
                    listBox4.Items.Add(normalizeDizi[a,c]);
                    //listBox3.Items.Add(dizi[a, c]);// değerler doğru geliyor
                    
                    //dataGridView2.Rows[c].Cells[a].Value = "a";
                }
               


            }

           

            //for (int a = 0; a < sutun; a++)
            //{
            //    for (int b = 0; b < satir; b++)
            //    {
            //        // gondereilecekDizi[a, b] = (double)dataGridView1.Rows[b].Cells[a].Value;

            //        dataGridView2.Rows[b].Cells[a].Value = normalizeDizi[a,b].ToString();
            //    }
            //}



        }







        // fazlalıklar
        private void dataGridRowsColorReset()
        {
            int sat = dataGridView1.Rows.Count;
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.BackColor = Color.White;
            for (int i=0;i<sat;i++)
            {
                dataGridView1.Rows[i].DefaultCellStyle = style;
            }
        }


        private Boolean RandomNumber(int randomNumber)// random sayıyı var mı yok mu diye kontrol ediyor
        {
            int sat = egitimListesi.Items.Count;
            int anahtar = 0;
            
            for (int i=0;i<sat;i++)
            {
                if (randomNumber == Convert.ToInt32(egitimListesi.Items[i]))
                    anahtar = 1;
                else
                    anahtar = 0;
            
            }

            if (anahtar == 1)
                return true;
            else
                return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {



            int sat, sut,cikisSutunu;
            sat = egitimListesi.Items.Count;
            sut = dataGridView1.Columns.Count - 1;
            //cikisSutunu = sut+1; // en son sütunu temsil ediyor

            double[] EmDizisi = new double[sat];
            double[] butunVeriler = new double[sut];


            Random random = new Random();


            int noronSayisi = Convert.ToInt32(listBox1.SelectedItem.ToString());

            double[] araKatmanCikisDegerleri = new double[noronSayisi]; // bir sonuc gelecek
            double[] ilkEgitimCikisDgerleri = new double[sat];
            //Noron cikislarIcinNoron = new Noron(sut);
            Noron[] girisKatmani = new Noron[noronSayisi];

             //noron classından gelen eski ağırlık değerleri tutuluyor 

            double randomBias = random.NextDouble();


            Noron[] araNoronlar = new Noron[noronSayisi];

            for (int i=0;i<noronSayisi;i++)
            {
                araNoronlar[i] = new Noron(sut,randomBias);
            }

            double Em,Bm,Cm,signumDegeri; // Em: hata değeri, Bm: veri Setindeki çıkış değeri, Cm: program ile bulunduğumuz çıkış değeri



            double[] oncekiAgirlikDegisimDizisi = new double[noronSayisi];
            //başlangıçta  hepsine 0 değerini atıyorum
            for (int i = 0; i < oncekiAgirlikDegisimDizisi.Length; i++)
            { // ikinci satıra geçmeden en son bu dizisi tekrardan sıfırlamamız gerekiyor
                oncekiAgirlikDegisimDizisi[i] = 0;
            }




            for (int satir = 0; satir < sat; satir++)
            {
                Em = 0; Bm = 0; Cm=0; signumDegeri = 0; // her satırda değerler sıfırlanıyor (Em=Bm-Cm)
                Bm = Convert.ToDouble(dataGridView1.Rows[satir].Cells[sut].Value); // sat zaten son çıkış sütunu değerine eşit oluyor
                Console.WriteLine("Veri setindeki satır çıkış değeri: "+Bm);

                for (int sutun  = 0; sutun < sut; sutun++)//-1 y1 sutunu olduğu için(dinamik olarak kullanıcıdan seçebiliriz) / her satır için ayrı sütun girişlerini alıyoruz
                {
                   
                    butunVeriler[sutun] = Convert.ToDouble(dataGridView1.Rows[Convert.ToInt32(egitimListesi.Items[satir])].Cells[sutun].Value);// aşağıdaki noronlara satır değerlerini gönderiyor

                }


                for (int i = 0; i < noronSayisi; i++)
                {

                    //fora al her nörondan sonra çıkış değerlerini kaydet ve egitim tamamlanınca ağırlık değerlerini kaydet
                    // ilkCikisDegerleri[i]= // nöron sayısı kadar aktivasyon sonucu çıkması gerekiyor

                    //aktivasyondan gelen sonucu al
                    //Console.WriteLine("aktivasyondan gelen değer:"+ noron.aktivasyonFonksiyonu());
                    araNoronlar[i].tumHesaplamalar(butunVeriler,randomBias,sat,sut);
                    araKatmanCikisDegerleri[i] = araNoronlar[i].cikis;//noron[i]
                   // araNoronlar[i].testFonksiyonu(); // çalışıyor
                    
                }

               
                Noron sonCikisNoronu = new Noron(araKatmanCikisDegerleri.Length,randomBias);
                sonCikisNoronu.tumHesaplamalar(araKatmanCikisDegerleri, randomBias, sat, sut);
                double[] cikisNoronuEskiAgirlikDizisi = new double[araKatmanCikisDegerleri.Length];


                ilkEgitimCikisDgerleri[satir] = sonCikisNoronu.cikis;







                // çıkış nöronu ağırlık değerleri güncelleştirme işlemi başladı

                Cm= sonCikisNoronu.cikis;
                Console.WriteLine("Her satırın Cm degeri: "+Cm);

                Em = Bm - Cm;
                Console.WriteLine("Her satırın Em değeri: "+Em); // Her satırın Em değerini bulunuyor (çalışıyor)

                signumDegeri = ((Cm * (1 - Cm)) * Em); // signumDegeri doğru hesaplanıyor(çalışıyor)
                Console.WriteLine("Her satırın singumDeger: "+signumDegeri);


               
                cikisNoronuEskiAgirlikDizisi = sonCikisNoronu.eskiAgirlikDizisiniDisaAktar(); // çıkış nöronundan ağırlık değerleri geliyor (çalışıyor). 

                // önceki ağırlık değişimi hesaplamalarında kaldık. daha başlamadım


                // delta


                double deltaAjm; // deltaAjm formülü için gerekli
                
                

               
                // lambda: 0,01 | alfa: 0,02
                double lambda = 0.01, alfa = 0.02;

                for (int i=0;i<araKatmanCikisDegerleri.Length;i++)
                {
                    deltaAjm = (lambda * signumDegeri*araKatmanCikisDegerleri[i]) + (alfa * oncekiAgirlikDegisimDizisi[i]); // ara nöronunun çıkış değeri mi sor!
                    oncekiAgirlikDegisimDizisi[i] = deltaAjm;

                    Console.WriteLine("deltaAjm değerleri: "+deltaAjm);
                }

                double[] CikisYeniAgirlikDizisi = new double[noronSayisi];
        
                

                //Akj bulma (çalışıyor)
                for (int i=0;i< araKatmanCikisDegerleri.Length; i++) // çıkış nöronundaki ağırlık değerleri yeniden hesaplanıyor. ara nöronlarda güncellenince en son burası da güncellenecek. o nedenle şu an başka dizide tutuyorum
                { // çıkış nöronuna arakatman çıkış değeri adetinde veri gönderdiğimiz için
                    CikisYeniAgirlikDizisi[i] = (cikisNoronuEskiAgirlikDizisi[i]) + oncekiAgirlikDegisimDizisi[i];
                    Console.WriteLine("Çıkış nöronu eski ağırlık: "+cikisNoronuEskiAgirlikDizisi[i]);
                    Console.WriteLine("Çıkış nöronu yeni ağırlık: "+CikisYeniAgirlikDizisi[i]);
                } // çıkış nöronuna yeni ağırlık değerleri üretiliyor ama henüz eskilerini güncellemedim


                // çıkış nöronu güncelleştirme işlemi son.










                //en sonunda güncelleme için kullanılacak kısım
                double[] cikisNorunuIcinYeniAgirlikDizisi = new double[sut];
                cikisNorunuIcinYeniAgirlikDizisi = yeniAgirlikDizisiUret(sut);//yeniAgirlikDizisi 
                for (int i=0;i<sut;i++)
                {
                    //Console.WriteLine("Güncelleme için üretilen ağırlık değerleri: "+cikisNorunuIcinYeniAgirlikDizisi[i]);
                }

                

            } // for(satir) sonu





            Console.WriteLine("------------------------------");
            Console.WriteLine("Nöron sayısı: "+noronSayisi);


            for (int i=0;i<ilkEgitimCikisDgerleri.Length;i++)// değerleri yazdırıyorum
            {
                Console.WriteLine((i+1)+". veri satırının çıkış değeri: "+ilkEgitimCikisDgerleri[i]);
            }
            // ilkeğitimcikis değerlerini de alıyoruz, exceldeki çıkış değerleriyle karışaltırıp %3 hata payı olana kadar devam etmesini sağlayamk kaldı




            //çıkış nöronuna ait
            for (int i = 0; i < oncekiAgirlikDegisimDizisi.Length; i++)
            { // ikinci satıra geçmeden en son bu dizisi tekrardan sıfırlamamız gerekiyor
                oncekiAgirlikDegisimDizisi[i] = 0;
            }

        } // eğit butonu

            
            

      

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i=1;i<21;i++)
            {
                listBox1.Items.Add(i);
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            button4.Visible = true; // eğitim butonunu gösteriyor
        }


        private double[] yeniAgirlikDizisiUret(int uretilecekDiziBoyutu)
        {
            Random random = new Random();
            double[] uretilenDizi = new double[uretilecekDiziBoyutu];
            for (int i=0;i<uretilecekDiziBoyutu;i++)
            {
                double deger = random.NextDouble();
                uretilenDizi[i] = deger;
            }

            return uretilenDizi;
        }

    }


  
}
