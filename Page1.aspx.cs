using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversityDatabase
{
    public partial class Page1 : System.Web.UI.Page
    {
      

        //---------------------- 1.DEO Uvod, CRUD operacije(unapred zadate i custom) -------------------------------------------

        /*
         * Ove godine pravimo veb aplikacije u ASP.NET-u(.NET Framework)
         * Do sada smo podatke na nasoj veb stranici cuvali u promenljivama i
         * strukturama podataka(List, LinkedList, Dictionary, ...)
         * u jeziku C#(ASP.NET). Medjutim, takav nacin cuvanja
         * podataka ima ozbiljne mane:
         * 1. neefikasno, tesko, nepregledno upisivanje, pretrazivanje, menjanje i brisanje podataka
         * 2. Promene podataka se ponistavaju nakon gasenja programa jer one vaze
         *      samo dok program radi, pogotovo nakon gasenja racunara jer se sve cuva u RAM memoriji
         * 3. Bezbednosni rizik, opasnost od napada, kradje, neautorizovanog menjanja podataka, ...
         * 
         * Zbog navedenih mana, su izmisljene BAZE PODATAKA koje resavaju probleme redom:
         * 1. Brze, efikasnije, preglednije i jednostavnije menjanje podataka
         * 2. Promene podataka ostaju zapisane nakon gasenja veb aplikacije i/ili racunara
         *      jer se podaci cuvaju na HDD-u, SSD-u, ...
         * 3. Povecana bezbednost, prava pristupa, mogucnost sifrovanja podataka(Zastita Info Sistema), ...
         * 
         * Takodje, ne mozemo samo koristiti baze bez nasih C#, ASP.NET Aplikacija, odnosno,
         * samo pisati upite jer nasi klijenti hoce softver koji ima i frontend a
         * a nasi klijenti uglavnom NE ZNAJU SQL! Znaci treba
         * nekako povezati bazu i njen interfejs sa C#, ASP.NET aplikacijama
         * 
         * Da bi povezivali nase ASP.NET veb aplikacije na bazu podataka
         * moramo koristiti nekog posrednika tj. neku tehnologiju koja ce da napravi
         * vezu izmedju podataka predstavljenim tabelama u bazi i podacima u C#-u.
         * Takodje ta tehnologija omogucava pristup podacima u bazi iz nasih C# aplikacija
         * Postoje dve glavne tehnologije:
         * 1.ADO.NET(naziv dobijen od ADO(ActiveX Data Objects), njegovog prethodnika)
         * 2.Entity Framework(EF)
         * 
         * EF je gradjen na osnovu ADO.NET-a i predstavlja bolju i napredniju tehnologiju koja
         * se danas najvise koristi u industriji. U .NET-u se zove Entity Framework Core(EF Core).
         * EF i EF Core su takozvane ORM(Object Relation Mapping) tehnologije koje vrse mapiranje
         * izmedju C# klase(objekta) i tabele(reda tabele) u bazi, to je ta veza o kojoj smo gore govorili
         * 
         * ADO.NET se toliko vise ne koristi u industriji ali nije da je mrtav jer ispod haube
         * EF-a upravo lezi ADO.NET, kao sto ispod haube OOP paradigme programiranja stoji imperativna paradigma.
         * Takodje se ADO.NET koristi u takozvanim legacy projektima(starijim aplikacijama koji se danas odrzavaju i koriste).
         * ADO.NET se koristi u .NET Framework-u.
         * 
         * Mi radimo ADO.NET(gle čuda...)
         * 
         * $$MINDSET HINT$$: Nemojte to da vas obeshrabri, svakako ce ovaj nacin razmisljanja
         * da vam pomogne kada kasnije budete nesto slicno radili u nekoj drugoj tehnologiji :)
         * 
         
         */

        protected void Page_Load(object sender, EventArgs e)
        {

            /* U page_load metodu pozivamo ostale metode i povezujemo se na bazu.
             * Metod Page_Load se poziva kada se stranica ucita(otvori) tj. page load-uje.
             * Da biste mogli da otvorite stranicu morate postaviti link ka njoj u Site Master-u gde su ostali linkovi ili
             * u solution explorer-u kliknuti desnim klikom na nju(.aspx) i izabrati set as start page.
                */

            /*Slobdno uklanjajte komentare da vidite kako rade ove metode, jedna po jedna.
             * Bitno je da ako pozivate vise od jedne funkcije da nakon izvrsenja jedne,
             * ponovo uspostavite konekciju na bazu zbog naredne metode jer se konekcija zatvara na kraju svake funkcije.
             * DOMACI: Mozemo li nesto uraditi u kodu da non stop ne otvaramo/zatvaramo konekciju?
             */

            /*connection string - opis baze, pomocu njega se povezujemo na bazu
             * Za rad sa bazama u Visual Studiu koristimo SQL Server Object Explorer.
             * On se otvara u View -> SQL Server Object Explorer.
             * U njemu se nalaze sve baze, tabele, kolone i ostale informacije o bazama.
             * Nase baze otvaramo u SQL Server -> (localdb)\MSSQLlocalDB - >Databases-> Vasa baza.
             * MSSQLlocalDB je ime servera na kojem se nalazi baza podataka.            
             * Tu bi trebalo da je ime baze University ako ste je napravili.
             * Otvaranjem baze i desnim klikom se otvara meni. Kliknite na properties.
             * Pojavice se prozor properties gde mozete naci konekcioni string baze.
             * Konekcioni string je niz karaktera koji je jedinstven za svaku bazu
             * i njime se povezujemo na zadatu bazu.
             * Ako ste bazu napravili na nekom drugom serveru promenite onda konekcioni string ili
             * je napravite na istom serveru kao ja.
             * U Server Object Explorer-u mozete naci ikonicu new query gde se otvara prozor
             * gde mozete kucati sql upite i izvrsavati ih u Visual Studiju.
             *  Ako se ne vide odmah izmene podataka nakon neke vase operacije kliknite na dugme
             *  refresh(plava kruzna strelica) koje se javlja kada kliknete na Object Explorer ili
             *  kad gledate tabelu. Desnim klikom na Tabele u Server Object Explorer-u mozete izabrati
             *  opciju view data da vidite podatke. Alternativno, mozete kucati upit select * from ime_tabele
             *  tako sto cete izabrati new query.
             */
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //objekat connection, on nas povezuje na bazu uz pomoc konekcionog stringa
            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {

                /*using omogucava da se svi resursi connection objekta automatski zatvore
               * nakon njegovog koriscenja, tj. nakon using bloka. Zato posle nema nigde zatvaranja konekcije ka bazi.
               * 
               * II nacin bez koriscenja using bloka jeste da na pocetku napisemo connection.Open i nakon
               * poziva funkcija napisati connection.Close() da bi zatvorili konekciju ka bazi
               * */

                //konekciju cemo drzati stalno otvorenom dokle god pozivamo funkcije koje rade sa bazom

                connection.Open(); // otvoranje konekcije konekciju

                //da biste testirali neku funkcije samo sklonite komentare sa poziva funkcije

                //PrintStudentRows(connection);

                //PrintStudentsAndCourses(connection);          

                //InsertStudent(connection); // u ovoj funkciji smo otvorili konekciju i zatvorili je

                //UpdateStudent(connection);

                //DeleteStudent(connection);

                /*int scalar = ReturnScalar(connection);

                System.Diagnostics.Debug.WriteLine("Scalar Value: " + scalar);

                */

                //PrintExamsCustom(connection, 1);

                //InsertStudentCustom(connection, "Peric3", "Pera3", 3);

                //UpdateStudentCustom(connection, "Peric", "Pera", 2, 9);

                //DeleteStudentCustom(connection, 1016);

            }

           
        }


        //metode pozivamo u Page_Load funkciji i definisemo ih izvan Page_Load-a
        /* U nastavku cemo imati metode pomocu kojih vrsimo CRUD operacije:
         * C - Create(Unos novih podataka)
         * R - Read(Citanje podataka)
         * U - Update(Menjanje podataka)
         * D - Delete(Brisanje podataka)
         */

        /*NAPOMENA: Zadaci ce biti numerisani brojevima i slovima nakon cega ide tekst zadatka*/

        //1a - Ispisati sve informacije(sve podatke svih kolona) svih studenata.
        void PrintStudentRows(SqlConnection connection)
        {

            //funkcija stampa rezultate select upita

                //objekat command prihvata sql upit u obliku stringa i connection objekat

                // DOMACI: Koja je mana ovog pristupa prihvatanja SQL upita?

                /*Preporuka je da svaku naredbu upita pisete u narednom redu radi
                 bolje preglednosti koda tj:
                SELECT ....
                FROM ....
                WHERE ....
                GROUP BY....
                HAVING ...
                ORDER BY...
                ....
                Pri cemu treba da vodite racuna o spajanju stringova operatorom + kao i o razmaku
                u samom stringu koji se mora pojaviti na kraju ili pocetku svakog reda sa naredbe
                ne bi spajale jedna sa drugom.
                Druga varijanta je da sve pisete u jednom redu. Oba nacina su tacna.*/

                SqlCommand command = new SqlCommand(
                  "SELECT StudentID, LastName, FirstName, Year" +
                  " FROM Students;",
                  connection);
               


                //DataReader objekat sluzi za izvrsavanje upita sa SELECT naredbom,
                //Ovo je SQL varijanta DataReader-a, takozvani SqlDataReader
                SqlDataReader reader = command.ExecuteReader(); //izvrsi komandu citanja(SELECT), povratna vrednost je DataReader(Sql tipa)

                if (reader.HasRows) // ako ima redova u tabeli
                {
                    /*Read() metod prelazi na sledeci red. Posto smo u while petlji,
                     program ce da prelazi na sledece redove dokle god ih ima*/
                    while (reader.Read())
                    {

                        /*Stampanje u Visual Studiju
                         NE MOZE Console.WriteLine jer je ovo veb aplikacija, a NE console app!
                        
                        Rezultati select upita se stavljaju u niz reader onim redom kojim su dohvaceni upitom.
                        Stampanje se vrsi u Output prozoru, Debug deo.
                        \t je tabulator(tab) i njime se u tekstu stavlja razmak velicine jednog tab-a.
                        Setimo se da backslash tj. \ dodaje ili oduzima specijalno znacenje karatera, ovde slova t
                        */
                        System.Diagnostics.Debug.WriteLine("\t{0}\t{1}\t{2}\t{3}",
                                reader[0], reader[1], reader[2], reader[3]);

                        //stampano liniju radi lepseg ispisa izmedju redova podataka
                        System.Diagnostics.Debug.WriteLine("------------------------------------------------");

                    /*//II Nacin ispisa je sa obicnim nadovezivanjem stringova
                    System.Diagnostics.Debug.WriteLine("ID:" + reader[0] + " LastName:" + reader[1] + " FirstName:" + reader[2] + " Year: " + reader[3]);
                    System.Diagnostics.Debug.WriteLine("------------------------------------------------");
                    */


                    //III Nacin ispisa je sa $ notacijom
                    System.Diagnostics.Debug.WriteLine($"ID: {reader[0]}  LastName:  {reader[1]}  FirstName: {reader[2]}  Year:  {reader[3]});");
                    System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");

                }
                }
                else
                {

                    System.Diagnostics.Debug.WriteLine("No rows found");
                }
                reader.Close(); // obavezno zatvoriti reader
            
        }

        /*NAPOMENA 1: Ako niste sigurni da li vam je upit tacan mozete ga testirati u Visual Studiju
         u SQL Server Explorer-u. Izaberite Server, zatim Databases i onda nadjete vasu bazu(University) i desnim
        klikom kliknete na nju. Zatim izaberete new query.
        Drugi nacin je u Sql Server Management Studiju.
        
         NAPOMENA 2(try-catch): Sve metode koje smo koristili tj. Read(), ExecuteReader(), Open() i mnoge
        druge mogu ispaliti razne izuzetke i tacnije bi bilo sve ove kodove ubaciti u try-catch blok.
        Sada dok ucimo nisam to pisao da vam se dodatno ne bi zakrcio kod koji citate.*/

        /*Do sada smo koristili objekte connection, command i DataReader.
         * Ali sta primecujete?
         * Ne zovu se oni tako vec imaju prefix Sql. Zasto? Jer u ADO.NET-u postoji
         * vise nacina(takozvanih .NET Database provider-i) pristupa bazi podataka u zavisnosti od prirode baze.
         * Oni su:
         * 1. SQL
         * 2. OLE DB
         * 3. ODBC
         * 4. Oracle
         * 
         * Za svaki provider postoje objekti connection, command, ... koji nose odgovarajuci
         * prefix. Dakle postoji SqlConnection, OleDbConnetion, OdbcCommand itd. Sve te
         * klase su zapravo izvedene klase objekata DbConnection, DbCommand, DbDataReader, ... respektivno(redom)
         * Ima slika u prilogu na ucionici.
         * 
         * Mi cemo najvise raditi prvi(SQL) nacin.
         * Dokumentacija: https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/retrieving-and-modifying-data
         * Mapiranje podataka iz C# u Sql i obrnuto(ovde pise koji tip podatka u sql odgovara kom tipu iz C#-a):
         * https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
         */

        //1b - Dohvatiti sve informacije o svim studentima i sve info o svim kursevima u dva posebna upita
        void PrintStudentsAndCourses(SqlConnection connection)
        {

            //funkcija stampa rezultate dva select upita
           
                SqlCommand command = new SqlCommand(
                  "SELECT StudentID, LastName, FirstName FROM Students;" +
                  "SELECT CourseID, Title, Points FROM Courses",
                  connection);
                

                SqlDataReader reader = command.ExecuteReader();

                while (reader.HasRows)
                {           // dokle god ima redova, ovde mora while jer imamo 2 upita


                    //GetName metod reader-a ispisuje ime kolone
                    System.Diagnostics.Debug.WriteLine("\t{0}\t{1}\t{2}", reader.GetName(0),
                        reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine("\t{0}\t{1}\t{2}", reader[0],
                            reader[1], reader[2]);
                    }
                    reader.NextResult(); //  predji na sledeci upit, tj. na sledecu tabelu

                    System.Diagnostics.Debug.WriteLine("-----------------------------------------");
                }

                reader.Close();
            
        }



        /*
         * Domaci(napisati funkcije koje obradjuju naredne upite):
         * 1c - Dohvatiti sve info studenata koji su 4. godina sortirane po prezimenu opadajuce.
         * 1d - Dohvatiti prezimena i ocene svih studenata koji su polagali neki ispit sortirane po ocenama rastuce.
         * 1e - Ispisati prezimena i prosecne ocene studenata na njihovim polaganjima pri cemu
         * ocene moraju biti unete(nisu NULL). Ispis grupisati po prezimenu studenata pri cemu
         * treba ispisati samo proseke vece od 2. Ispis sortirati po proseku opadajuce.
         */

        //1f - Napisati funkciju koja ubacuje studenta
        void InsertStudent(SqlConnection connection)
        {  /*funkcija ubacuje jednog studenta(unapred dati podaci)
            * Priroda nase baze je takva da se ID svake tabele(primarni kljuc)
            * unosi automatski tj. mi ga NE unosimo.
            * */
            
                SqlCommand command = new SqlCommand("INSERT INTO Students (LastName, FirstName, Year)" +
                                                    " VALUES ('Peric', 'Pera', 3);", connection);
                //vodite racuna da smo napravili razmak izmedju dva reda tj. VALUES mora biti odvojeno od ) iz prethodnog reda!
                
                /*metod ExecuteNonQuery() objekta command sluzi za izvrsavanje upita
                 * koji ne vracaju redove iz tabele(INSERT, UPDATE, DELETE, ...)
                 * */
                command.ExecuteNonQuery();


            
        }
        //1g - Napisati funkciju koja azurira nekog studenta
        void UpdateStudent(SqlConnection connection)
        { //funkcija menja podatke postojeceg studenta, where uslov je preko ID jer je ID Primary Key(Primarni Kljuc) tj. jedinstven je
            
                SqlCommand command = new SqlCommand("UPDATE Students " +
                                                        "SET LastName = 'Mikic' , FirstName = 'Mika', Year = 2 " +
                                                            "WHERE StudentID = 10", connection);
                
                command.ExecuteNonQuery();
            
        }

        //1h - Napisati funkciju koja brise nekog studenta
        void DeleteStudent(SqlConnection connection)
        { // funkcija brise studenta
            
                SqlCommand command = new SqlCommand("DELETE from Students " +
                                                        "WHERE StudentID = 10", connection);                

                command.ExecuteNonQuery();
            
        }

        //1i - Napisati funkciju koja vraca jednu vrednost na osnovu nekog select upita.
        int ReturnScalar(SqlConnection connection)
        { // funkcija vraca tacno jednu vrednost

            int scalar; // ovde cuvamo tu jednu vrednost(skalar)

            
             // vraca broj studenata ciji ID je veci od 5
                SqlCommand command = new SqlCommand("SELECT COUNT(StudentID) " +
                                                        "FROM Students " +
                                                        "WHERE StudentID > 5", connection);

                

                /* metod ExecuteScalar() objekta command izvrsava sql upite koji vracaju
                 * tacno jednu vrednost(skalar) za razliku od ostalih select upita koji
                 * mogu vracati vise vrednosti(vektor). Preciznije, metod ExecuteScalar() vraca
                 * prvu kolonu prvog reda rezultata select upita.
                 */
                scalar = (int)command.ExecuteScalar();
            

            return scalar;
        }



        /*
         * Funkcija InsertStudent je ubacivala studenta na nacin unapred definisan upitom u stringu.
         * Ako zelimo da ubaci nekog drugog studenta moramo da izmenimo funkciju tj. string(upit)
         * ili da napisemo novu funkciju. Za velike sisteme sa konstantnim potrebama
         * ubacivanja bi to bilo neprakticno. Zato cemo funkciji proslediti C# promenljive
         * koje cemo ubaciti u string tj. u upit na nacin pokazan ispod i samim tim
         * imati funkciju koja moze da ubaci bilo kakvog studenta pri cemu se samo promeniti argumenti
         * funkcije pri njenom pozivu. Slicno ce vaziti za Update, Delete i Select Funkcije.
         * 
         * InsertStudentCustom je varijanta funkcije koja ubacuje bilo kakvog studenta
         sa podacima koje korisnik prosledi kao argument.
        */
        //1j - Napisati funkciju koja moze da ubaci bilo kakvog studenta odredjenog C# argumentima funkcije
        void InsertStudentCustom(SqlConnection connection, string prezime, string ime, int year)
        {
            
                string query = "INSERT INTO Students (LastName, FirstName, Year)" +
                                                    " VALUES ( '" + prezime + "', '" + ime + "', " + year + " );";

                /*primetite i dvostruke i jednostruke navodnike. Dvostrukim navodnicima se obelezava
                 pocetak i kraj stringa tj. upita, a jednostruki navodnici su za navodjenje stringa u upitu, kao sto se radi u sql-u!
                Deluje komplikovano? Moze li jednostavnije?
                II NACIN je da se ispred stringa stavi $, zatim se promenljive navode u viticastim zagradama
                 */

                string query2 = "INSERT INTO Students (LastName, FirstName, Year)" +
                                                    $" VALUES ( '{prezime}', '{ime}', {year} );";

                SqlCommand command = new SqlCommand(query, connection);

                //SqlCommand command2 = new SqlCommand(query2, connection);

                command.ExecuteNonQuery();

                //command2.ExecuteNonQuery();

            
        }



        //1k  -Napisati funkciju koja moze da azurira bilo kog studenta na bilo koji nacin
        void UpdateStudentCustom(SqlConnection connection, string prezime, string ime, int year, int ID)
        {
            //funkcija update-uje proizvoljnog studenta na proizvoljan nacin
            
                SqlCommand command = new SqlCommand("UPDATE Students " +
                                                        "SET LastName = '" + prezime + "' , FirstName = '" + ime + "', Year = " + year + " " +
                                                            "WHERE StudentID = " + ID, connection);
                //vodite racuna o razmacima izmedju year i WHERE, zato smo ubacili onaj jedan space
                

                command.ExecuteNonQuery();
            
        }


        //1l - Napisati funkciju koja moze da obrise bilo kog studenta tj. id se prosledjuje kao argument
        void DeleteStudentCustom(SqlConnection connection, int ID)
        { // funkcija brise studenta odredjenog ID-a prosledjenog kao argument
            
                SqlCommand command = new SqlCommand("DELETE from Students " +
                                                        "WHERE StudentID = " + ID, connection);

                command.ExecuteNonQuery();
            
        }

        //1m- Napisati funkciju koja ispisuje sve info o ispitima cija je ocena veca od grade, grade je argument
        void PrintExamsCustom(SqlConnection connection, int grade)
        {
                            
                string query = "SELECT *" +
                                " FROM Exams " +
                                "WHERE Grade > " + grade + " ;";

                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        System.Diagnostics.Debug.WriteLine("\t{0}\t{1}\t{2}\tGrade: {3}",
                                reader[0], reader[1], reader[2], reader[3]);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No rows found");
                }
                reader.Close();
            

        }


        /*DOMACI(custom varijante prethodnih domacih):
         * 1n - Dohvatiti sve info studenata cija godina je jednaka year. year je c# argument fje.
         * 1o - Dohvatiti prezimena i ocene svih studenata koji su polozili ispit ocenom vecom od grade
         * i ciji Studentid je veci od id(grade i id su c# parametri).
         * 1p - Ispisati prezimena i prosecne ocene studenata na njihovim polaganjima pri cemu
         * ocene moraju biti razlicite od grade. Ispis grupisati po prezimenu studenata pri cemu
         * treba ispisati samo proseke vece od average. Proseke sortirati opadajuce.(grade i average su c# promenljive).
         */


    }
}