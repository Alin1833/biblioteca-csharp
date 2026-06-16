using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    class Autor
    {
        public int Id { get; set; }
        public string NumePrenume { get; set; }
    }

    class Carte
    {
        public string Isbn { get; set; }
        public string Titlu { get; set; }
        public float Pret { get; set; }
        public int AnPublicare { get; set; }
        public bool EsteDisponibila { get; set; } = true;
        public List<Autor> Autori { get; set; } = new List<Autor>();

        public bool ValideazaIsbn(string Isbn)
        {
            foreach (var caracter in Isbn)
            {
                if (!char.IsDigit(caracter))
                {
                    if (caracter != '-')
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class Cititor
    {
        public int NumarLegitimatie { get; set; }
        public string NumePrenume { get; set; }
        public string Email { get; set; }
        public string DataEmitereLegitimatie { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public int ValabilitateLegitimatie { get; set; } = 3;

    }

    class Imprumut
    {
        public int IdImprumut { get; set; }
        public int NumarLegitimatie { get; set; }
        public string Isbn { get; set; }
        public string DataImprumut { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string DataReturnare { get; set; } = null;
    }

    class Program
    {
        static void Main(string[] args)
        {
            int optiune;
            List<Carte> listaCarti = new List<Carte>();
            List<Autor> listaAutori = new List<Autor>();
            List<Cititor> listaCititori = new List<Cititor>();
            List<Imprumut> listaImprumuturi = new List<Imprumut>();
            PreluareDateDinFisier(listaCarti, listaCititori, listaImprumuturi);
            while (true)
            {
                Console.WriteLine("0.Salvare/Iesire" +
                    "\n1.Afiseaza detalii carti gestionate" +
                    "\n2.Adaugare carte" +
                    "\n3.Afisare carti scrise de un anumit autor" +
                    "\n4.Stergere carte dupa ISBN" +
                    "\n5.Stergere carte dupa titlu" +
                    "\n6.Sa se afiseze toate cartile publicate intr-un an introdus de utilizator" +
                    "\n7.Adauga cititor nou" +
                    "\n8.Afisare cititori cu legitimatia expirata" +
                    "\n9.Reinnoirea unei legitimatii" +
                    "\n10.Imprumut carte" +
                    "\n11.Returnare carte imprumutata de un cititor" +
                    "\n12.Afisare cititori ai unei carti" +
                    "\n13.Afisare carti imprumutate" +
                    "\n14.Calculare numar de carti imprumutate de un cititor");
                do
                {
                    Console.Write("Introduceti ce optiune alegeti: ");
                } while (!Int32.TryParse(Console.ReadLine(), out optiune));
                switch (optiune)
                {
                    case 0:
                        SalvareDateInFisier(listaCarti, listaCititori, listaImprumuturi);
                        return;
                    case 1:
                        AfisareDetalii(listaCarti);
                        break;
                    case 2:
                        AdaugareCarte(listaCarti, listaAutori);
                        break;
                    case 3:
                        CautareDupaAutor(listaCarti);
                        break;
                    case 4:
                        StergereCarteDupaIsbn(listaCarti);
                        break;
                    case 5:
                        StergereCarteDupaTitlu(listaCarti);
                        break;
                    case 6:
                        CautareDupaAnPublicare(listaCarti);
                        break;
                    case 7:
                        AdaugareCititorNou(listaCititori);
                        break;
                    case 8:
                        AfisareLegitimatiiExpirate(listaCititori);
                        break;
                    case 9:
                        ReinnoireLegitimatii(listaCititori);
                        break;
                    case 10:
                        ImprumutCarte(listaCarti, listaCititori, listaImprumuturi);
                        break;
                    case 11:
                        ReturnareCarte(listaCarti, listaCititori, listaImprumuturi);
                        break;
                    case 12:
                        AfisareCititoriCarte(listaCarti, listaCititori, listaImprumuturi);
                        break;
                    case 13:
                        AfisareCartiImprumutate(listaCarti, listaCititori, listaImprumuturi);
                        break;
                    case 14:
                        AfisareNumarCartiCitite(listaCarti, listaCititori, listaImprumuturi);
                        break;
                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
                Console.WriteLine("Apasati orice tasta pentru a continua...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #region preluare date din fisier
        static void PreluareDateDinFisier(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            string filePath = "Carti.txt";
            string filePath1 = "Cititori.txt";
            string filePath2 = "Imprumuturi.txt";
            if (!File.Exists(filePath))
            {
                ;
            }
            else
            {
                var liniiFisier = File.ReadAllLines(filePath);

                foreach (var linie in liniiFisier)
                {
                    Carte carteCurenta = new Carte();

                    var elemente = linie.Split('_');
                    carteCurenta.Isbn = elemente[0];
                    carteCurenta.Titlu = elemente[1];
                    carteCurenta.Pret = float.Parse(elemente[2]);
                    carteCurenta.AnPublicare = Convert.ToInt32(elemente[3]);
                    carteCurenta.EsteDisponibila = Convert.ToBoolean(elemente[4]);
                    List<Autor> autoriCurenti = new List<Autor>();
                    var autori = elemente[5].Split(';');
                    foreach (var autor in autori)
                    {
                        if (autor == string.Empty)
                            break;
                        var detalii = autor.Split(',');
                        Autor autorCurent = new Autor
                        {
                            Id = Convert.ToInt32(detalii[0]),
                            NumePrenume = detalii[1]
                        };
                        autoriCurenti.Add(autorCurent);
                    }
                    carteCurenta.Autori = autoriCurenti;
                    listaCarti.Add(carteCurenta);
                }
            }

            if(!File.Exists(filePath1))
            {
                ;
            }
            else
            {
                var liniiFisier = File.ReadAllLines(filePath1);

                foreach(var linie in liniiFisier)
                {
                    Cititor cititorCurent = new Cititor();
                    var elemente = linie.Split('_');
                    cititorCurent.NumarLegitimatie = Convert.ToInt32(elemente[0]);
                    cititorCurent.NumePrenume = elemente[1];
                    cititorCurent.Email = elemente[2];
                    cititorCurent.DataEmitereLegitimatie = elemente[3];
                    cititorCurent.ValabilitateLegitimatie = Convert.ToInt32(elemente[4]);
                    listaCititori.Add(cititorCurent);
                }
            }

            if (!File.Exists(filePath2))
            {
                ;
            }
            else
            {
                var liniiFisier = File.ReadAllLines(filePath2);

                foreach (var linie in liniiFisier)
                {
                    Imprumut imprumutCurent = new Imprumut();
                    var elemente = linie.Split('_');
                    imprumutCurent.IdImprumut = Convert.ToInt32(elemente[0]);
                    imprumutCurent.NumarLegitimatie = Convert.ToInt32(elemente[1]);
                    imprumutCurent.Isbn = elemente[2];
                    imprumutCurent.DataImprumut = elemente[3];
                    imprumutCurent.DataReturnare = elemente[4];
                    listaImprumuturi.Add(imprumutCurent);
                }
            }
        }
        #endregion

        #region 0 - salvare in fisier
        static void SalvareDateInFisier(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            using (StreamWriter sw = new StreamWriter("Carti.txt"))
            {
                foreach (var carte in listaCarti)
                {
                    sw.Write($"{carte.Isbn}_{carte.Titlu}_{carte.Pret}_{carte.AnPublicare}" +
                        $"_{carte.EsteDisponibila}_");

                    foreach (var autor in carte.Autori)
                        sw.Write($"{autor.Id},{autor.NumePrenume};");
                    sw.WriteLine();
                }
            }
            using (StreamWriter sw=new StreamWriter("Cititori.txt"))
            {
                foreach (var cititor in listaCititori)
                {
                    sw.Write($"{cititor.NumarLegitimatie}_{cititor.NumePrenume}_{cititor.Email}" +
                        $"_{cititor.DataEmitereLegitimatie}_" +
                        $"{(DateTime.Parse(cititor.DataEmitereLegitimatie).AddYears(cititor.ValabilitateLegitimatie).Year - DateTime.Now.Year)}");
                    sw.WriteLine();
                }
            }
            using (StreamWriter sw=new StreamWriter("Imprumuturi.txt"))
            {
                foreach(var imprumut in listaImprumuturi)
                {
                    sw.Write($"{imprumut.IdImprumut}_{imprumut.NumarLegitimatie}_{imprumut.Isbn}_{imprumut.DataImprumut}" +
                        $"_{imprumut.DataReturnare}");
                    sw.WriteLine();
                }
            }
        }
        #endregion

        #region 1 - afisare detalii despre toate cartile gestionate
        static void AfisareDetalii(List<Carte> listaCarti)
        {
            foreach (var carte in listaCarti)
            {
                Console.Write($"\nISBN:{carte.Isbn} " +
                    $" Titlu:{carte.Titlu} " +
                    $" Pret:{carte.Pret} " +
                    $" An publicare:{carte.AnPublicare}" +
                    $" Disponibila:{carte.EsteDisponibila}");
                foreach (var autor in carte.Autori)
                {
                    Console.Write($" Autori/Autor: {autor.Id},{autor.NumePrenume};");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region 2 - adauga carte noua
        static void AdaugareCarte(List<Carte> listaCarti, List<Autor> listaAutori)
        {
            char optiune;
            string isbn, titlu, numePrenume;
            float pret;
            int anPublicare;
            Carte carteNoua = new Carte();
            List<Autor> autori = new List<Autor>();
            do
            {
                Console.Write("Introduceti un ISBN valid: ");
                isbn = Console.ReadLine();
            } while (carteNoua.ValideazaIsbn(isbn));

            var carteVerificata = listaCarti.SingleOrDefault(c => c.Isbn == isbn);
            if (carteVerificata != null)
            {
                Console.WriteLine($"ISBN-ul introdus este alocat altei carti:" +
                    $"\nISBN:{carteVerificata.Isbn} " +
                    $"\nTitlu:{carteVerificata.Titlu} " +
                    $"\nPret:{carteVerificata.Pret} " +
                    $"\nAn publicare:{carteVerificata.AnPublicare}");
            }
            else
            {
                do
                {
                    Console.Write("Introduceti titlul: ");
                    titlu = Console.ReadLine();
                } while (string.IsNullOrEmpty(titlu));
                do
                {
                    Console.Write("Introduceti pretul: ");
                } while (!float.TryParse(Console.ReadLine(),
                NumberStyles.Float, new CultureInfo("ro-RO"), out pret) || pret <= 0);
                do
                {
                    Console.Write("Introduceti anul publicarii: ");
                } while (!Int32.TryParse(Console.ReadLine(), out anPublicare) ||
                anPublicare > DateTime.Now.Year);
                do
                {
                    do
                    {
                        Console.Write("Introduceti numele si prenumele autorului: ");
                        numePrenume = Console.ReadLine();
                    } while (string.IsNullOrEmpty(numePrenume));
                    if (listaAutori.Count != 0)
                    {
                        var autorVerificat = listaAutori.SingleOrDefault(a => a.NumePrenume == numePrenume);
                        if (autorVerificat == null)
                        {
                            autorVerificat = new Autor
                            {
                                Id = listaAutori.Max(i => i.Id) + 1,
                                NumePrenume = numePrenume
                            };
                            autori.Add(autorVerificat);
                            listaAutori.Add(autorVerificat);
                        }
                        else
                        {
                            if (!carteNoua.Autori.Contains(autorVerificat))
                                autori.Add(autorVerificat);
                        }
                    }
                    else
                    {
                        autori.Add(new Autor { Id = listaAutori.Count + 1, NumePrenume = numePrenume });
                    }
                    do
                    {
                        Console.Write("Continuati sa introduceti autori? [d/n]: ");
                        optiune = Convert.ToChar(Console.ReadLine());
                    } while (optiune != 'd' && optiune != 'n');
                } while (optiune != 'n');
                carteNoua = new Carte
                {
                    Isbn = isbn,
                    Titlu = titlu,
                    Pret = pret,
                    AnPublicare = anPublicare,
                    Autori = autori
                };
                listaCarti.Add(carteNoua);
            }
        }
        #endregion

        #region 3 - cautare carti dupa un anumit autor
        static void CautareDupaAutor(List<Carte> listaCarti)
        {
            string numePrenume;
            do
            {
                Console.Write("Introduceti numele si prenumele autorului: ");
                numePrenume = Console.ReadLine();
            } while (string.IsNullOrEmpty(numePrenume));
            var cartiGasiteCuAutor = listaCarti.Where(c => c.Autori.Select(a=>a.NumePrenume).Contains(numePrenume));
            
            if(cartiGasiteCuAutor.Count()==0)
                Console.WriteLine("Nu exista autorul introdus!");
            else if(cartiGasiteCuAutor.Count()==1)
            {
                foreach (var carte in cartiGasiteCuAutor)
                {
                    Console.WriteLine($"Exista o singura carte care este scrisa de {numePrenume} " +
                   $"\nISBN:{carte.Isbn} " +
                   $"\nTitlu:{carte.Titlu} " +
                   $"\nPret:{carte.Pret} " +
                   $"\nAn publicare:{carte.AnPublicare}");
                }
            }
            else
            {
                Console.WriteLine($"Au fost gasite mai multe carti scrise de {numePrenume}:");
                foreach (var carte in cartiGasiteCuAutor)
                {
                    Console.WriteLine($"ISBN:{carte.Isbn} " +
                   $"\nTitlu:{carte.Titlu} " +
                   $"\nPret:{carte.Pret} " +
                   $"\nAn publicare:{carte.AnPublicare}");
                }
            }
        }
        #endregion

        #region 4 - stergere carte dupa ISBN
        static void StergereCarteDupaIsbn(List<Carte> listaCarti)
        {
            string isbn;
            Carte carteCautata = new Carte();
            do
            {
                Console.Write("Introduceti un ISBN valid: ");
                isbn = Console.ReadLine();
            } while (carteCautata.ValideazaIsbn(isbn));

            var carteVerificata = listaCarti.SingleOrDefault(c => c.Isbn == isbn);
            if (carteVerificata != null)
            {
                if (carteVerificata.EsteDisponibila)
                {
                    Console.WriteLine($"Cartea cu isbn-ul {carteVerificata.Isbn} si cu " +
                        $"titlul {carteVerificata.Titlu}  este disponibila si va fi stearsa!");
                    listaCarti.Remove(carteVerificata);
                }
                else
                {
                    Console.WriteLine($"Cartea cu isbn-ul {carteVerificata.Isbn} si cu " +
                        $"titlul {carteVerificata.Titlu} nu este disponibila si nu poate fi stearsa!");
                }
            }
            else
            {
                Console.WriteLine("Nu exista carte cu ISBN-ul introdus!");
            }
        }
        #endregion

        #region 5 - stergere carte dupa titlu
        static void StergereCarteDupaTitlu(List<Carte> listaCarti)
        {
            string titlu;
            Carte carteCautata = new Carte();
            do
            {
                Console.Write("Introduceti un titlu valid: ");
                titlu = Console.ReadLine();
            } while (string.IsNullOrEmpty(titlu));

            var carteVerificata = listaCarti.Where(c => c.Titlu == titlu);
            if (carteVerificata.Count() == 0)
            {
                Console.WriteLine($"Nu exista carte cu titlul \"{titlu}\"");
            }
            else if (carteVerificata.Count() == 1)
            {
                var carte = listaCarti.SingleOrDefault(c => c.Titlu == titlu);
                if (carte.EsteDisponibila)
                {
                    Console.WriteLine($"Cartea cu isbn-ul {carte.Isbn} si cu " +
                        $"titlul {carte.Titlu}  este disponibila si va fi stearsa!");
                    listaCarti.Remove(carte);
                }
                else
                {
                    Console.WriteLine($"Cartea cu isbn-ul {carte.Isbn} si cu " +
                        $"titlul {carte.Titlu} nu este disponibila si nu poate fi stearsa!");
                }
            }
            else
            {
                Console.WriteLine("Sunt mai multe carti cu titlul cautat, asadar se va face stergerea dupa ISBN!");
                foreach (var carteDeSters in carteVerificata)
                {
                    Console.Write($"\nISBN:{carteDeSters.Isbn} " +
                   $" Titlu:{carteDeSters.Titlu} " +
                   $" Pret:{carteDeSters.Pret} " +
                   $" An publicare:{carteDeSters.AnPublicare}" +
                   $" Disponibila:{carteDeSters.EsteDisponibila}");
                    foreach (var autor in carteDeSters.Autori)
                    {
                        Console.Write($" Autori/Autor: {autor.Id},{autor.NumePrenume};");
                    }
                    Console.WriteLine();
                }
                StergereCarteDupaIsbn(listaCarti);
            }
        }
        #endregion

        #region 6 - cautare carti dupa un anumit an de publicare
        static void CautareDupaAnPublicare(List<Carte> listaCarti)
        {
            int anPublicare;
            do
            {
                Console.Write("Introduceti anul publicarii: ");
            } while (!Int32.TryParse(Console.ReadLine(), out anPublicare) ||
                anPublicare > DateTime.Now.Year);
            var cartiCautate = listaCarti.Where(c => c.AnPublicare == anPublicare);
            if (cartiCautate.Count() == 0)
            {
                Console.WriteLine($"Nu exista carti publicate in anul {anPublicare}!");
            }
            else
            {
                foreach (var carte in cartiCautate)
                {
                    Console.Write($"\nISBN:{carte.Isbn} " +
                   $" Titlu:{carte.Titlu} " +
                   $" Pret:{carte.Pret} " +
                   $" An publicare:{carte.AnPublicare}" +
                   $" Disponibila:{carte.EsteDisponibila}");
                    foreach (var autor in carte.Autori)
                    {
                        Console.Write($" Autori/Autor: {autor.Id},{autor.NumePrenume};");
                    }
                    Console.WriteLine();
                }
            }
        }
        #endregion

        #region 7 - adaugare cititor nou
        static void AdaugareCititorNou(List<Cititor> listaCititori)
        {
            char varianta;
            int nrLeg;
            string numePrenume, email, data;
            if (listaCititori.Count != 0)
            {
                nrLeg = listaCititori.Select(c => c.NumarLegitimatie).Max() + 1;
            }
            else
                nrLeg = 1;
            Console.Write("Introduceti numele si prenumele: ");
            numePrenume = Console.ReadLine();
            Console.Write("Introduceti emailul: ");
            email = Console.ReadLine();
            do
            {
                Console.Write("Doriti sa introduceti alta data de emitere legitimatie? [d/n]: ");
                varianta = Convert.ToChar(Console.ReadLine());
            } while (varianta != 'd' && varianta != 'n');
            if(varianta=='d')
            {
                Cititor cititor = new Cititor()
                {
                    NumarLegitimatie = nrLeg,
                    NumePrenume = numePrenume,
                    Email = email
                };
                listaCititori.Add(cititor);
            }
            else
            {
                Console.Write("Introduceti data de emitere a legitimatiei: [dd-MM-yyyy]");
                data = Console.ReadLine();
                Cititor cititor = new Cititor()
                {
                    NumarLegitimatie = nrLeg,
                    NumePrenume = numePrenume,
                    Email = email,
                    DataEmitereLegitimatie=data
                };
                listaCititori.Add(cititor);
            }
            
        }
        #endregion

        #region 8 - afisare cititori cu legitimatii expirate
        static void AfisareLegitimatiiExpirate(List<Cititor> listaCititori)
        {
            if(listaCititori.Count==0)
                Console.WriteLine("Nu exista cititori inregistrati!!!");
            else
            {
                var legitimatiiExpirate = listaCititori.Where(c => c.ValabilitateLegitimatie <= 0);
                if(legitimatiiExpirate.Count()==0)
                    Console.WriteLine("Nu exista legitimatii expirate!!!");
                else
                {
                    foreach (var cititor in legitimatiiExpirate)
                    {
                        Console.Write($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}" +
                            $" | Email:{cititor.Email}  Data emitere legitimatie:{cititor.DataEmitereLegitimatie}");
                        Console.WriteLine();
                    }
                }
            }
        }
        #endregion

        #region 9 - reinnoire legitimatii expirate
        static void ReinnoireLegitimatii(List<Cititor> listaCititori)
        {
            int leg;
            var cititoriExpirati = listaCititori.Where(c => c.ValabilitateLegitimatie <= 0);
            foreach (var cititor in cititoriExpirati)
            {
                Console.WriteLine($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}" +
                            $" | Data emitere legitimatie:{cititor.DataEmitereLegitimatie}");
            }
            do
            {
                Console.Write("Introduceti numarul de legitimatie al celui pe care doriti sa-l reinnoiti: ");
            } while (!Int32.TryParse(Console.ReadLine(),out leg));
            var cititorCautat = cititoriExpirati.SingleOrDefault(c => c.NumarLegitimatie == leg);
            if(cititorCautat!=null)
            {
                cititorCautat.DataEmitereLegitimatie = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");
                cititorCautat.ValabilitateLegitimatie = 2;
                Console.WriteLine($"Cititorului {cititorCautat.NumePrenume} i-a fost reinnoit legitimatia pe 2 ani!" +
                    $"\nLista de cititori s-a actualizat conform schimbarilor: ");
                foreach (var cititor in listaCititori)
                {
                    Console.Write($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}" +
                            $" | Email:{cititor.Email} | Data emitere legitimatie:{cititor.DataEmitereLegitimatie}");
                }
            }
            else
            {
                Console.WriteLine($"Nu exista cititor cu numarul de legitimatie {leg}!");
            }
        }
        #endregion

        #region 10 - imprumut de carte
        static void ImprumutCarte(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            int id, leg;
            string isbn;
            Carte carteVerificata = new Carte();
            if (listaImprumuturi.Count==0)
            {
                id = 100;
            }
            else
            {
                id = listaImprumuturi.Select(i => i.IdImprumut).Max()+1;
            }
            do
            {
                Console.Write("Introduceti ISBN-ul cartii de imprumutat: ");
                isbn = Console.ReadLine();
            } while (carteVerificata.ValideazaIsbn(isbn));
            var carteCautata = listaCarti.SingleOrDefault(c => c.Isbn == isbn);
            if(carteCautata==null)
                Console.WriteLine("Nu exista cartea cu ISBN-ul introdus!!!");
            else
            {
                if(carteCautata.EsteDisponibila)
                {
                    foreach (var cititor in listaCititori)
                    {
                        if (DateTime.Now.AddDays(30) < DateTime.Parse(cititor.DataEmitereLegitimatie).AddYears(cititor.ValabilitateLegitimatie))
                        {
                            Console.WriteLine($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}");
                        }
                    }
                    do
                    {
                        Console.Write("Introduceti numarul de legitimatie al celui care doreste sa imprumute cartea: ");
                    } while (!Int32.TryParse(Console.ReadLine(), out leg));
                    var cititorCautat = listaCititori.SingleOrDefault(c => c.NumarLegitimatie == leg);
                    if(cititorCautat==null)
                        Console.WriteLine($"Nu exista cititor cu numarul de legitimatie {leg}");
                    else
                    {
                        Console.WriteLine($"Imprumutul a fost aprobat!\n{cititorCautat.NumePrenume} cu numarul de legitimatie" +
                            $" {cititorCautat.NumarLegitimatie} a imprumutat cartea cu ISBN-ul {carteCautata.Isbn} si cu titlul" +
                            $" \"{carteCautata.Titlu}\" pe o perioada de 30 de zile!");
                        carteCautata.EsteDisponibila = false;
                        Imprumut imprumut = new Imprumut()
                        {
                            IdImprumut = id,
                            NumarLegitimatie = leg,
                            Isbn = isbn
                        };
                        listaImprumuturi.Add(imprumut);
                    }
                }
                else
                {
                    Console.WriteLine($"Cartea \"{carteVerificata.Titlu}\" nu este disponibila!");
                }
            }
        }
        #endregion

        #region 11 - returnare carte
        static void ReturnareCarte(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            if(listaImprumuturi.Count!=0)
            {
                string isbn;
                Carte carteVerificata = new Carte();
                var cartiImprumutate = listaImprumuturi.Where(i => i.DataReturnare == null).Select(i => i.Isbn);
                if(cartiImprumutate.Count()==0)
                    Console.WriteLine("Nu exista carti imprumutate in momentul actual!");
                else
                {
                    foreach (var carte in listaCarti)
                    {
                        if (cartiImprumutate.Contains(carte.Isbn))
                        {
                            Console.WriteLine($"ISBN:{carte.Isbn} | Titlu:{carte.Titlu} ");
                        }
                    }
                    do
                    {
                        Console.Write("Introduceti ISBN-ul cartii de returnat: ");
                        isbn = Console.ReadLine();
                    } while (carteVerificata.ValideazaIsbn(isbn));
                    var carteCautata = listaCarti.SingleOrDefault(c => c.Isbn == isbn);
                    if (carteCautata == null)
                        Console.WriteLine("Nu exista carte de returnat cu isbn-ul introdus!");
                    else
                    {
                        var imprumutCautat = listaImprumuturi.SingleOrDefault(i => i.Isbn == isbn);
                        var cititor = listaCititori.SingleOrDefault(c => c.NumarLegitimatie == imprumutCautat.NumarLegitimatie);
                        if (imprumutCautat == null)
                            Console.WriteLine("Nu exista niciun imprumut la cartea cu isbn-ul introdus!");
                        else
                        {
                            Console.WriteLine($"A fost returnata cartea cu ISBN-ul {carteCautata.Isbn} de catre cititorul cu numele" +
                                $" si prenumele {cititor.NumePrenume}!");
                            carteCautata.EsteDisponibila = true;
                            imprumutCautat.DataReturnare = DateTime.Now.ToString("dd-MM-yyyy");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Nu exista niciun imprumut!");
            }
        }
        #endregion

        #region 12 - afisare cititori care au imprumutat o carte
        static void AfisareCititoriCarte(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            string isbn;
            Carte carteVerificata = new Carte();
            var cartiCitite = listaImprumuturi.Select(c => c.Isbn).Distinct();
            foreach (var carte in listaCarti)
            {
                if (cartiCitite.Contains(carte.Isbn))
                {
                    Console.WriteLine($"ISBN:{carte.Isbn} | Titlu:{carte.Titlu} ");
                }
            }
            do
            {
                Console.Write("Introduceti ISBN-ul cartii de returnat: ");
                isbn = Console.ReadLine();
            } while (carteVerificata.ValideazaIsbn(isbn));
            var carteCautata = listaCarti.SingleOrDefault(c => c.Isbn==isbn);
            if(carteCautata==null)
                Console.WriteLine("Nu exista carte cu isbn-ul introdus!!!");
            else
            {
                var cititori = listaImprumuturi.Select(i => i.NumarLegitimatie).Distinct();
                if (cititori.Count() == 1)
                {
                    Console.Write($"Exista un singur cititor care a imprumutat cartea \"{carteCautata.Titlu}\" si acela este: ");
                    foreach (var cititor in listaCititori)
                    {
                        if(cititori.Contains(cititor.NumarLegitimatie))
                            Console.WriteLine($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}");
                    }
                }
                else
                {
                    Console.Write($"Cititorii care au imprumutat cartea \"{carteCautata.Titlu}\" sunt: ");
                    foreach (var cititor in listaCititori)
                    {
                        if (cititori.Contains(cititor.NumarLegitimatie))
                            Console.Write($"\nNumar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}");
                    }
                }
            }
        }
        #endregion

        #region 13 - afisare carti imprumutate de un cititor
        static void AfisareCartiImprumutate(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            int leg;
            var cititori = listaImprumuturi.Select(c => c.NumarLegitimatie).Distinct();
            foreach (var cititor in listaCititori)
            {
                if (cititori.Contains(cititor.NumarLegitimatie))
                    Console.WriteLine($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}");
            }
            do
            {
                Console.Write("Introduceti numarul de legitimatie al celui pe care doriti sa-i vedeti istoricul de imprumuturi: ");
            } while (!Int32.TryParse(Console.ReadLine(), out leg));
            var cititorCautat = listaCititori.SingleOrDefault(c => c.NumarLegitimatie == leg);
            if(cititorCautat==null)
                Console.WriteLine("Nu exista cititor cu numarul de legitimatie introdus!");
            else
            {
                var cartiCitite=listaImprumuturi.Where(i=>i.NumarLegitimatie==cititorCautat.NumarLegitimatie)
                                                .Select(i=>i.Isbn)
                                                .Distinct();
                Console.WriteLine($"Cititorul {cititorCautat.NumePrenume} a citit (imprumutat) urmatoarele carti: ");
                foreach (var carte in listaCarti)
                {
                    if(cartiCitite.Contains(carte.Isbn))
                        Console.WriteLine($"ISBN:{carte.Isbn} | Titlu:{carte.Titlu}");
                }
            }
        }
        #endregion

        #region 14 - afisare numar carti citite/imprumutate de catre un cititor
        static void AfisareNumarCartiCitite(List<Carte> listaCarti, List<Cititor> listaCititori, List<Imprumut> listaImprumuturi)
        {
            int leg;
            var cititori = listaImprumuturi.Select(c => c.NumarLegitimatie).Distinct();
            foreach (var cititor in listaCititori)
            {
                if (cititori.Contains(cititor.NumarLegitimatie))
                    Console.WriteLine($"Numar legitimatie:{cititor.NumarLegitimatie} | Nume si prenume:{cititor.NumePrenume}");
            }
            do
            {
                Console.Write("Introduceti numarul de legitimatie al celui pe care doriti sa-i vedeti numarul de " +
                    "carti imprumutate: ");
            } while (!Int32.TryParse(Console.ReadLine(), out leg));
            var cititorCautat = listaCititori.SingleOrDefault(c => c.NumarLegitimatie == leg);
            if (cititorCautat == null)
                Console.WriteLine("Nu exista cititor cu numarul de legitimatie introdus!");
            else
            {
                var cartiCitite = listaImprumuturi.Where(i => i.NumarLegitimatie == cititorCautat.NumarLegitimatie)
                                                .Count();
                Console.WriteLine($"Cititorul {cititorCautat.NumePrenume} a citit (imprumutat) {cartiCitite} carti!");
            }
        }
        #endregion
    }
}

