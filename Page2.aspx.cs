using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversityDatabase
{
    public partial class Page2 : System.Web.UI.Page
    {

        /* ----------------------2.DEO DataSet i DataAdapter------------------------------
         Dokumentacija: https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/dataadapters-and-datareaders
        https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/populating-a-dataset-from-a-dataadapter 
        Sada cemo pokazati drugaciji nacin rada sa bazom. Tj. CRUD operacije pomocu posebnih C# klasa.
        Ovaj nacin u nekim slucajevima znatno poboljsava performanse rada aplikacija.
        Te klase su: DataAdapter, DataSet, DataTable.
         DataAdapter - klasa koji sluzi za interakciju iz programa sa podacima iz baze
         DataSet, DataTable - u ovim klasama se cuvaju odredjeni podaci iz baze koje im mi dodeljujemo.
        Ideja je da se preko DataAdaptera dohvate podaci iz baze, zatim uradi neka operacija sa njima
        i takvi vrate u bazu podataka. Dakle, podaci se dovlace sa HDD-a ili SSD-a tj. baze i
        upisuju u RAM memoriju(in-memory) tj. u program tj. u DataAdapter, DataSet, DataTable(nadalje Data klase).
        Zatim mi vrsimo CRUD operacije nad podacima u Data klasama i kada zavrsimo sa CRUD operacijama
        tada podatke(izmenjene ili ne) vracamo u bazu preko DataAdaptera i njegovih metoda.
        
         HDD - Hard Disk Drive(Hard disk)
         SSD - Solid State Drive
         
         */

        protected void Page_Load(object sender, EventArgs e)
        {

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //objekat connection, on nas povezuje na bazu uz pomoc konekcionog stringa
            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {

                connection.Open();

                //Populate(connection);

                //PopulateWithPK(connection);

                //GetStudentsSchemaInfo(connection);

                //PrintFromDataAdapter(connection);

                //PrintUsingDataTableAndDataSet(connection);          

                //PrintMultipleTables(connection);

            }
        }
        
        //2a - Napisati funkciju koja preko DataAdapter-a dohvata id i prezimena svih studenata
        // i ubacuje ih(puni) u DataSet i DataTable
        //Takvo ubacivanje se zove punjenje(populating, data populating)
        void Populate(SqlConnection connection)
        {
            string queryString = // neki upit koji dohvata podatke
                "SELECT StudentID, LastName FROM Students";
            //Za radoznale: Sta ako dohvatamo podatke iz 2 tabele tj. preko join?
            //DataAdapter konstruktor prihvata upit i konekciju
            //DataAdapter: https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dataadapter?view=netframework-4.8
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);


            //DataTable: https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=netframework-4.8
            //U DataTable se cuvaju podaci jedne tabele
            DataTable studentsTable = new DataTable(); //pravimo DataTable

            //DataSet: https://learn.microsoft.com/en-us/dotnet/api/system.data.dataset?view=netframework-4.8
            //U DataSet se cuvaju podaci iz vise tabela tj. to je skup vise tabela(DataTable elemenata)
            DataSet studentsSet = new DataSet(); //pravimo DataSet

            /*DataAdapterom mi dohvatamo podatke iz baze preko prvog argumenta njegovog konstruktora
             * tj. queryString. Te podatke ubacujemo u studentsTable odnosno studentsSet metodom
             * Fill. Metod fill dodaje redove iz baze ili refresh-uje redove.
             * Refresh-ovanje je korisno ako se npr. desila neka promena u bazi i onda zelimo da izmenjene
             * podatke ubacimo u studentsTable
             */
            adapter.Fill(studentsTable);

            /*metod Fill puni podatke iz adaptera(one dohvacene upitom koju mu je prosledjen u konstruktoru)
             * u DataSet studentsSet tj. vrsi data populating.
             Drugi argument je source tabela, tj. tabela iz koje se dohvataju podaci odnosno
            pomocu koje se DataSet puni*/
            adapter.Fill(studentsSet, "Students");


        }

        //2b - Napisati funkciju koja preko DataAdaptera dohvata id i prezimena studenata sa informacijama o Primarnim Kljucevima(PK)
        // DataTable i DataSet napuniti dohvacenim podacima
        void PopulateWithPK(SqlConnection connection)
        {
            string queryString =
                "SELECT StudentID, LastName FROM Students";

            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

            DataSet studentsSet = new DataSet();

            DataTable studentsTable = new DataTable();

            /*metod FillSchema omogucava da se DataAdapter popuni
             * sa informacijama o primarnim kljucevima(Primary Keys) i o shemi tabele
             */
            adapter.FillSchema(studentsSet, SchemaType.Source, "Students");
            adapter.Fill(studentsSet, "Students");

            adapter.FillSchema(studentsTable, SchemaType.Source);
            adapter.Fill(studentsTable);

        }

        //2c - Dohvatiti Shemu tabele koja ispisuje Id i Prezime svih studenata
        void GetStudentsSchemaInfo(SqlConnection connection)
        {
            //funkcija ispisuje podatke o tabeli iz baze
           
                SqlCommand command = new SqlCommand(
                  "SELECT StudentID, LastName FROM Students;",
                  connection);
                

                SqlDataReader reader = command.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable(); //vrati shemu tabele
                //DataRow je red podataka u DataTable
                foreach (DataRow row in schemaTable.Rows) //prodji kroz svaki red tabele
                {
                    //DataColumn je shema kolone u DataTable
                    foreach (DataColumn column in schemaTable.Columns) //prodji kroz svaku kolonu tabele tekuceg reda
                    {
                        //Format metod klase String vrsi odredjen nacin stampanja
                        System.Diagnostics.Debug.WriteLine(String.Format("{0} = {1}",
                           column.ColumnName, row[column])); //stampa kolonu i vrednost kolone
                    }
                }

                reader.Close();
            
        }




        //2d - Napisati funkciju koja dohvata id i prezimena studenata i stampa ih preko DataAdaptera
        void PrintFromDataAdapter(SqlConnection connection)
        {
            string queryString =
                "SELECT StudentID, LastName FROM Students";

            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

            DataSet students = new DataSet();
            adapter.Fill(students, "Students");

            /*OPREZ! DataAdapteru mora biti dat sql upit PRE poziva Fill metoda!*/

            /* Kada smo napravaili dataAdapter prosledili smo mu upit queryString.
             * Ovaj upit se moze dohvatiti preko svojstva SelectCommand
             * u koje se automatski upisuje.
             */
            SqlCommand command = adapter.SelectCommand;

            //metod Fill automatski otvara i zatvara konekciju, tako da je moramo opet otvoriti

            

            //stampamo podatke iz DataSet dohvacene upitom queryString
            SqlDataReader reader = command.ExecuteReader();
            //imamo reader i stampamo podatke kao i ranije
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    System.Diagnostics.Debug.WriteLine("\t{0}\t{1}",
                            reader[0], reader[1]);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No rows found");
            }
            reader.Close();

            
        }
        /*BITNA NAPOMENA: Ovim nacinom u 2d zadatku nista novo nismo uradili, tj. samo smo dohvatili
         * sql komandu i odstampali je kao i ranije odnosno nismo koristili DataSet.
         * U DataSet se nalaze podaci tj. oni su u RAM memoriji.
         * A mi smo u zadatku 2d ponovo dohvatali podatke sa diska tj. baze i ispisivali
         * ih metodom ExecuteReader(). Time se trosi vreme i usporava rad programa.
         * Nema potrebe to raditi kada smo vec dohvatili podatke metodom Fill u DataSet.
         * Zato zelim da direktno stampam podatke iz DataSet-a.
         * To radimo u zadatku 2e i tako cu traziti da uradite na odgovaranju!
         * DataSet i DataTable su korisni ako na primer zelimo da u programu duze
         * cuvamo dohvacene podatke iz nekog razloga, tj. bolje je cuvati ih tu
         * nego u DataReader-u.
         */
        //2e - Napisati funkciju koja dohvata id i prezimena studenata i stampa ih preko DataAdaptera koristeci DataTable i DataSet
        void PrintUsingDataTableAndDataSet(SqlConnection connection)
        {
            /*istovremeno cemo pokazati dva nacina stampanja podataka koristeci
             DataTable i DataSet*/


                string queryString =
                    "SELECT StudentID, LastName FROM Students";

                //2 NACINA dodele upita DataAdapter-u
                //I nacin je preko konstruktora gde se bilo koji upit upisuje u SelectCommand property
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);


                /* //II nacin je direktno preko SelectCommmand property-ja
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(queryString, connection);
                */

                /*OPREZ! DataAdapteru mora biti odredjen SelectCommand property pre Fill metode!*/

                //U DataTable se cuvaju podaci jedne tabele iz baze
                DataTable studentTable = new DataTable();
                adapter.Fill(studentTable); //dodaj redove iz baze u DataTable studentTable

                //U DataSet se cuvaju podaci vise tabela(vise DataTable objekata) iz baze
                DataSet studentsSet = new DataSet();
                //dodaj redove iz tabele Students u DataSet studentsSet
                adapter.Fill(studentsSet, "Students");

                System.Diagnostics.Debug.WriteLine("\n"); //stampam novi red radi lepseg ispisa
                System.Diagnostics.Debug.WriteLine("-------------Ispis preko DataTable-------------");
                //ispis podataka iz DataTable pri cemu se dohvata red po red(DataRow)
                foreach (DataRow row in studentTable.Rows)
                { // stampaj podatke za svaki red. u row nizu su redom podaci

                    System.Diagnostics.Debug.WriteLine("ID: {0} LastName: {1}", row[0], row[1]);
                    System.Diagnostics.Debug.WriteLine("---------------------------------------");

                }

                System.Diagnostics.Debug.WriteLine("\n");
                System.Diagnostics.Debug.WriteLine("-------------Ispis preko DataSet-------------");

                /*iz skupa tabela studentsSet dohvatamo kolekciju tabela pri cemu uzimamo prvu tabelu(index 0)
                zatim, dohvatamo sve redove te tabele i vrsimo ispis kao u prethodnoj petlji.
                Uzimamo prvu tabelu jer je to jedina tabela koju imamo jer upit je dohvatao podatke
                samo iz Students tabele. Inace u DataSet mogu stajati podaci iz vise tabela.

                */
                foreach (DataRow row in studentsSet.Tables[0].Rows)
                { // stampaj podatke za svaki red

                    System.Diagnostics.Debug.WriteLine("ID: {0} LastName: {1}", row[0], row[1]);
                    System.Diagnostics.Debug.WriteLine("---------------------------------------");

                }

            

        }

        //2f - napisati funkciju koja stampa rezultat dva upita sa 2 razlicite tabele preko DataSet
        void PrintMultipleTables(SqlConnection connection)
        {
            
                //2 select upita
                string doubleQuery = "SELECT StudentID, LastName, FirstName FROM Students; " +
                  "SELECT CourseID, Title, Points FROM Courses";

                //I NACIN
                // SqlDataAdapter adapter = new SqlDataAdapter(doubleQuery, connection);

                //II NACIN
                SqlCommand command = new SqlCommand(doubleQuery, connection);

                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = command;


                DataSet dataSet = new DataSet();

                //ubacujemo u DataSet rezultate dva upita
                adapter.Fill(dataSet);

                /*Moramo proci kroz sve podatke svake tabele, dakle, potrebne su nam 2 petlje, npr. foreach*/

                //dohvatimo kolekciju tabela iz skupa tabela
                DataTableCollection tables = dataSet.Tables;
                tables[0].TableName = "Students"; // odredimo imena tabela
                tables[1].TableName = "Courses";

                //prolazimo kroz svaku tabelu DataSet-a. Ima ih vise jer sql naredba dohvata vise tabela(ovde 2)
                foreach (DataTable table in tables)
                {
                    System.Diagnostics.Debug.WriteLine("\n\n");

                    System.Diagnostics.Debug.WriteLine("-----------------" + table.TableName + "-----------------"); //ime tabele

                    //ispis svake tabele tj. prolaz kroz svaki red tabele.
                    foreach (DataRow row in table.Rows)
                    {
                        // vodite racuna o broju kolona, ovde smo rekli da oba upita dohvataju isti broj kolona!                           
                        System.Diagnostics.Debug.WriteLine("1: {0} 2: {1} 3: {2}", row[0], row[1], row[2]);
                        System.Diagnostics.Debug.WriteLine("---------------------------------------");

                    }


                }

            
        }

        /* Razlika izmedju DataReader i DataAdapter-a.
         * 
         * U dokumentaciji pise: DataReader provides a way of reading a forward only stream of data.
         * Forward only znaci da se podaci u DataReader-u mogu citati samo u jednom smeru tj unapred.
         * Odnosno ne mozemo se vratiti na neki podatak na kojem smo bili tj. nakon poziva metode
         * Read() i prelaska na sledeci red ne mozemo se vratiti na prethodni. Zato sto je Read
         * jedan stream tj. tok podataka. To znaci da u programu(RAM) podaci dolaze red po red tabele
         * tj. ne ucitavaju se svi podaci dohvaceni upitom odmah, nego jedan po jedan.
         * Dakle, DataReader u nekom trenutku cuva samo jedan red podataka.
         * DataReader cuva podatke dokle god postoji konekcija prema bazi podataka.
         * 
         * DataAdapter odjednom dohvata sve podatke iz baze oznacene select upitom.
         * I svi ti podaci se mogu upisati u DataTable ili DataSet. Takodje, u svakom trenutku
         * mozemo dohvatiti bilo koji podatak iz DataTable ili DataSet tj. svi podaci
         * su nam uvek dostupni.  DataTable i DataSet cuvaju podatke u sebi i nakon zatvaranja
         * konekcije prema bazi.
         * 
         * Ostaje pitanje koji pristup je bolji. To zavisi od situacije. Npr. ako nam nije potrebna
         * vecina podataka u bazi, a tabela ima 10 000 podataka mozda je bolje preko DataReader-a dohvatiti
         * prvih nekoliko nego odmah sve preko DataAdaptera i time opteretiti RAM memoriju. S druge strane,
         * mana DataReader-a je sto ne cuva sve podatke nego prelaskom na sledeci red podataka mi vise
         * nemamo pristup prethodnim podacima i ako bismo hteli da ih opet dohvatimo moramo ponovo da ih citamo
         * sa diska sto opet dodatno usporava rad programa. Ne zaboravimo da je citanje podataka
         * sa diska(HDD, SSD) UVEK SPORIJE nego citanje iz RAM memorije.
         * Razmislite jos o prednostima i manama oba pristupa...
         *
         */

        /*DOMACI:
         * 2g - napisati funkciju koja preko DataTable i DataSet stampa
         * ocene i prezimena studenata koji su nesto polagali
         * 2h - Napisati funkciju koja ispisuje info samo nekih studenata definisanih custom uslovima(c# argumenti)
         *
         */


    }
}