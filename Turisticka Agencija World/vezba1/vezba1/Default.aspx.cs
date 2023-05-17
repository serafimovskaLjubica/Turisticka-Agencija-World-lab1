using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading.Tasks;

namespace vezba1
{
    class TuristickaAgencija
    {
        private static void Start()
        {
            Agencija agencija = new Agencija("Internet Tehnologii");
            while (true)
            {
                Console.WriteLine(" " + agencija.Ime + " ");
                Console.WriteLine("Izberi broj: ");
                Console.WriteLine("1. Usluzhi go klientot\n" +
                                  "2. Prodadeni karti na shalterot\n" +
                                  "3. Vkupen promet na shalterot\n" +
                                  "4. Site prodadeni karti na agencijata, organizirani po shalter\n" +
                                  "5. Vkupniot promet na agencijata\n" +
                                  "6. Uspeshnost na agencijata\n" +
                                  "7. Exit\n");

                var c = Convert.ToInt32(Console.ReadLine());
                int brojshalter;
                switch (c)
                {
                    case 1:
                        ++agencija.BrojNaKlienti;
                        Console.WriteLine("Vnesete go vaseto ime, prezime, godini, destinacijata i shalterot");
                        string[] p = Console.ReadLine().Split(" ".ToCharArray());

                        if (p.Length == 5)
                        {
                            string ime = p[0];
                            string prezime = p[1];
                            int godini = Convert.ToInt32(p[2]);
                            string destinacija = p[3];
                            brojshalter = Convert.ToInt32(p[4]);

                            if (!agencija.Validacija(brojshalter))
                            {
                                Console.WriteLine("\n Brojot na shalterot e nevaliden!!! \n");
                                break;
                            }

                            Destinacija d;
                            if ((d = Agencija.GetDestination(destinacija)) == null)
                            {
                                Console.WriteLine("\n Nema karti za baranata destinacija!!! \n");
                                break;
                            }

                            agencija.Vraboteni[brojshalter]
                                .DodadiKarta(
                                    new Karta(ime, prezime, godini, d)
                                );
                            ++agencija.BrojNaUsluzeniKlienti;

                            Console.WriteLine("\n Uspeshna rezervacija \n");
                        }
                        else
                            Console.WriteLine("\n Neuspeshna rezervacija!!! \n");

                        break;

                    case 2:
                        Console.WriteLine("Vnesete broj na shalter");

                        brojshalter = Convert.ToInt32(Console.ReadLine());
                        if (!agencija.Validacija(brojshalter))
                        {
                            Console.WriteLine("\n Nevaliden broj na shalter \n");
                            break;
                        }

                        Console.WriteLine("\n{0}\n",
                            agencija.Vraboteni[brojshalter]
                                .ProdadeniKartiVoVremenskiOpseg(
                                    DateTime.Now.AddHours(-2),
                                    DateTime.Now.AddHours(2))
                        );
                        break;

                    case 3:
                        Console.WriteLine("Vnesete broj na shalter");

                        brojshalter = Convert.ToInt32(Console.ReadLine());
                        if (!agencija.Validacija(brojshalter))
                        {
                            Console.WriteLine("\n Nevaliden broj na shalter \n");
                            break;
                        }

                        Console.WriteLine("\n{0}den.\n",
                            agencija.Vraboteni[brojshalter]
                                .VkupenProfitOdOdredeniKarti(
                                    DateTime.Now.AddHours(-2),
                                    DateTime.Now.AddHours(2)
                                ));
                        break;

                    case 4:
                        Console.WriteLine("\n{0}\n",
                            agencija
                                .KartiProdadeniOdVrabotenite(
                                    DateTime.Now.AddHours(-2),
                                    DateTime.Now.AddHours(2)
                                ));
                        break;

                    case 5:
                        Console.WriteLine("\n{0}den.\n", agencija.VkupenProfit());
                        break;

                    case 6:
                        try
                        {
                            Console.WriteLine("\n{0:0.00}%\n", agencija.UspehNaAgencijata() * 100);
                        }
                        catch (DivideByZeroException)
                        {
                            Console.WriteLine("\n Nema klienti vo agencijata\n");
                        }

                        break;

                    case 7:
                        return;

                    default:
                        Console.WriteLine("\n NEVALIDEN VLEZ \n");
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            Start();
        }
    }

    class Destinacija
    {
        public string Ime { get; private set; }
        public int Cena { get; private set; }
        private static readonly int k = 2000;

        public Destinacija(string ime)
        {
            this.Ime = ime;
            this.Cena = Presmetka(this.Ime);
        }

        private static int Presmetka(string ime)
        {
            return ime.Length * k;
        }
    }

    class Karta
    {
        public string Ime { get; private set; }
        public string Prezime { get; private set; }
        public int Godini { get; private set; }
        public Destinacija Destinacija { get; private set; }
        public int CenaKarta { get; private set; }
        public DateTime VremeNaProdazba { get; private set; }

        public Karta(string ime, string prezime, int godini, Destinacija destinacija)
        {
            Ime = ime;
            Prezime = prezime;
            Godini = godini;
            Destinacija = destinacija;
            VremeNaProdazba = DateTime.Now;
            CenaKarta = destinacija.Cena;
        }
    }

    class Vraboten
    {
        public int BrojNaShalter { get; private set; }
        public List<Karta> Karti { get; private set; }

        public Vraboten(int brojnashalter)
        {
            BrojNaShalter = brojnashalter;
            Karti = new List<Karta>();
        }

        public void DodadiKarta(Karta karta)
        {
            Karti.Add(karta);
        }

        public int VkupnoProdadeniKarti()
        {
            return Karti.Count;
        }

        public int ProdadeniKartiVoVremenskiOpseg(DateTime start, DateTime end)
        {
            int br = 0;
            Karti.ForEach(karta =>
            {
                if (karta.VremeNaProdazba >= start && karta.VremeNaProdazba <= end)
                    ++br;
            });
            return br;
        }

        public int VkupenProfit()
        {
            int profit = 0;
            Karti.ForEach(karta => profit += karta.CenaKarta);
            return profit;
        }

        public int VkupenProfitOdOdredeniKarti(DateTime start, DateTime end)
        {
            int profit = 0;
            Karti.ForEach(karta =>
            {
                if (karta.VremeNaProdazba >= start && karta.VremeNaProdazba <= end)
                    profit += karta.CenaKarta;
            });
            return profit;
        }
    }

    class Agencija
    {
        public string Ime { get; private set; }
        public List<Vraboten> Vraboteni { get; private set; }
        public int BrojNaKlienti { get; set; }
        public int BrojNaUsluzeniKlienti { get; set; }

        private static Destinacija[] DostapniDestinacii = new Destinacija[]
        {
            new Destinacija("Rim"),
            new Destinacija("London"),
            new Destinacija("Tokio"),
            new Destinacija("Sofija"),
            new Destinacija("Istanbul"),
            new Destinacija("Toronto"),
            new Destinacija("Majami"),
            new Destinacija("Zagreb"),
            new Destinacija("Belgrad"),
            new Destinacija("Moskva")
        };

        public Agencija(string ime)
        {
            Ime  = ime;
            BrojNaUsluzeniKlienti = 0;
            BrojNaKlienti = 0;
            Vraboteni = new List<Vraboten>();
            for (int i = 0; i < 5; ++i)
                Vraboteni.Add(new Vraboten(i));
        }

        public decimal UspehNaAgencijata()
        {
            if (BrojNaKlienti == 0)
                throw new DivideByZeroException("Nema klienti");
            return BrojNaUsluzeniKlienti / (decimal)BrojNaKlienti;
        }

        public int VkupenProfit()
        {
            int profit = 0;
            Vraboteni.ForEach(vraboten => profit += vraboten.VkupenProfit());
            return profit;
        }

        public string KartiProdadeniOdVrabotenite(DateTime start, DateTime end)
        {
            StringBuilder s = new StringBuilder();
            foreach (var vraboten in Vraboteni)
                s.Append("Vraboten " + vraboten.BrojNaShalter + ": " + vraboten.VkupnoProdadeniKarti() + "\n");
            return s.ToString();
        }

        public static Destinacija GetDestination(string destinacija)
        {
            foreach (var dest in DostapniDestinacii)
                if (dest.Ime.ToLower() == destinacija.ToLower())
                    return dest;
            return null;
        }

        public bool Validacija(int number)
        {
            return number >= 0 && number < Vraboteni.Count;
        }
    }
}