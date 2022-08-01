using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogrenme
{

   

    class Noron
    {
      // sütun sayısı-1 kadar olacak ( y1 hariç olduğundan) 
        Random rastgele = new Random();
        double[] ilkAgirlikDizisi;
        int sat, sut;
        double[,] gelenSatirVerilerDizisi;
        double[] toplamFonksiyonuSonuclarDizisi;
        double[] cikisDizisi;

        double bias;

        public double toplamSonuc, cikis;

        public Noron(int sutun,double randombias)
        {
            this.ilkAgirlikDizisi = new double[sutun];
            this.bias = randombias;
            agirlikDizisiUret(sutun);
        }


        public Noron(int sut, double[] gelenVerilerDizisi, int sat, double randombias)
        {
            // ağırlık değerleri
            this.ilkAgirlikDizisi = new double[sut];
            // Console.WriteLine("Ağırlıklar***********************");
            agirlikDizisiUret(sut);



            //toplam fonksiyonu
            this.gelenSatirVerilerDizisi = new double[sat, sut];

            this.toplamFonksiyonuSonuclarDizisi = new double[sat]; // gelen değerlerin satırlarındaki değerlerin toplam fonksiyonundaki sonuclarını kaydediyor
            this.cikisDizisi = new double[sat];
            this.sat = sat;
            this.sut = sut;
            this.bias = randombias;
            //Console.WriteLine("Toplam fonskiyonu ***************");

            //toplamFonksiyonu(gelenVerilerDizisi); // ilk bu çağırılmalı

            // aktivasyonFonksiyonu();


        }







        private void agirlikDizisiUret(int sutunSayisi)//ağırlık değerleri üretilip, ilkAgirlikDizisine ekleniyor
        {
            for (int i = 0; i < sutunSayisi; i++)
            {

                this.ilkAgirlikDizisi[i] = rastgele.NextDouble();
                
            }
        }



        public void toplamFonksiyonu( double[] gelenSatirDegerlerDizisi)
        {


            toplamSonuc = 0;
           
               

                for (int i = 0; i < gelenSatirDegerlerDizisi.Length; i++)
                {
                   
                    toplamSonuc =toplamSonuc+( this.ilkAgirlikDizisi[i] * gelenSatirDegerlerDizisi[i]); 

              

                }
           
            


        }

        public void tumHesaplamalar(double[] gelenSatirDegerlerDizisi, double randombias,int satir,int sutun)
        {
          
            toplamFonksiyonu(gelenSatirDegerlerDizisi);
            aktivasyonFonksiyonu();
        }

        public void aktivasyonFonksiyonu()
        {
         
           cikis = (1 / (1 + Math.Exp(-1 * (toplamSonuc * this.bias))));
           // Console.WriteLine("Toplam sonuc: " + toplamSonuc);
             // ikincisinde sıfırlıyor
        }
        
        public void testFonksiyonu()// gelen değerleri istediğimde test etmek için kullanıyorum
        {
            Console.WriteLine("Bias degeri: " +this.bias);

        }


        public double[] eskiAgirlikDizisiniDisaAktar()
        {
            return this.ilkAgirlikDizisi;
        }


        public void agirlikDizisiniGuncelle(double[] gelenYeniAgirlikDizisi) //hesaplamalardan sonra ağırlık dizisini güncellememize yaran fonksiyon
        {
            for (int i=0;i<gelenYeniAgirlikDizisi.Length;i++)
            {
                this.ilkAgirlikDizisi[i] = gelenYeniAgirlikDizisi[i];
            }

        }


        



    }




}
