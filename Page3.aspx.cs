using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversityDatabase
{
    public partial class Page3 : System.Web.UI.Page
    {


        /*-----------------------3. DEO Parametri-----------------------
         
         Dokumentacija: https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/configuring-parameters-and-parameter-data-types
        Upite pisemo tako sto ih ubacimo u tip string u C#-u. Imena kolona i vrednosti su
        takodje string-ovi. Vrednosti za koje zelimo nesto da dohvatimo, izmenimo
        unapred odredjene tj. fiksirane jer su tip string. Problem je u tome sto ako bismo
        zeleli da izmenimo podatke koje dohvatamo i kako ih dohvatamo, morali bismo da menjamo string(sql upit)
        ili da pisemo novu funkciju. Taj problem smo resili ubacivanjem C# promenljivih u sam string
        i time dali mogucnost da se ista funkcija izvrsava na razlicite nacine u zavisnosti od toga
        koja je vrednost C# promenljivih ubacenih u string, a prosledjenih kao argumenti C# funkcije.
        Problem koji onda moze da nastane jeste sto unos stringova u upit se odvaja navodnicima za
        sam string tj. " i " ali se i takodje tip string mora staviti u navodnike razlicite od " tj. izmedju ' i '
        jer je to tip string u sql-u. Ako bismo imali velike upite ili puno takvih mozemo lako napraviti
        gresku pri nadovezivanju stringova i otvaranju/zatvaranju navodnika jer ih ima puno i 2 tipa.
        Ovaj problem se prevazilazi uvodjenjem parametara.
        Za realizaciju toga koristimo klasu ASP.NET klasu SqlParameter.
        Dokumentacija: https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlparameter?view=netframework-4.8

        */

        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //objekat connection, on nas povezuje na bazu uz pomoc konekcionog stringa
            SqlConnection connection = new SqlConnection(connectionString);

            using (connection)
            {
                connection.Open();


                //PrintStudentsParameters(connection, 3);

                //PrintExamsParameters(connection, 1, "Calculus");

                //InsertStudentParameter(connection, "Pera2", "Peric2", 4);

                //UpdateStudentParameter(connection, 3, 16);

                //DeleteStudentParameter(connection, 16);

                //PrintFromDataAdapterDataTableParameter(connection, 1);

                //InsertStudentDataAdapterParameter(connection, "Pera", "Peric", 1);

                //UpdateStudentDataAdapterParameter(connection, 2, 8);

                //DeleteStudentDataAdapterParameter(connection, 10);

            }
        }

        void PrintStudentsParameters(SqlConnection connection, int year)
        {           
                //konstruisemo parametar
                SqlParameter parameter = new SqlParameter();

                /*atributom Value podesavamo vrednost parametra.
                 Ovde je to vrednost c# promenljive year*/
                parameter.Value = year;

                /*Ime parametra je naziv koji se pojavljuje u stringu koji je upit.
                 To pisemo umesto C# promenljive. Naziv parametra na pocetku mora
                imati @ nakon cega ide ime koje mu sami zadajemo(ne mora biti jednako imenu kolone iz tabele)
                Ovo vazi samo za Sql nacin pristupa bazi. U OleDb i ODBC se koristi upitnik ?, slicno kao u Javi*/
                parameter.ParameterName = "@Year";

                /*umesto ubacivanja C# promenljive year, napisemo samo ime parametra. 
                 Time nema mucenja sa navodnicima i nadovezivanjem stringova*/

                string query = "SELECT LastName, Year " +
                                "FROM Students " +
                                "WHERE Year = @Year";

                SqlCommand command = new SqlCommand(query, connection);

                /*u Sql komandu ubacimo parametar tako sto pozovemo property Parameters
                 i nakon toga metod Add ciji argument je parameter.
                Ovime smo u sql naredbu(komandu) dodali nas parametar.*/
                command.Parameters.Add(parameter);
                //zatim kao i do sada vrsimo ispis.
                

                SqlDataReader reader = command.ExecuteReader();

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
        /* 3b
         * ispisati nazive i ocene predmeta koji su polagani pri cemu
         * ocena je veca od grade i naziv predmeta nije courseTitle
         */
        void PrintExamsParameters(SqlConnection connection, int grade, string courseTitle)
        {
            
                //potrebna su nam 2 parametra
                SqlParameter p1 = new SqlParameter();
                SqlParameter p2 = new SqlParameter();

                p1.Value = grade;

                p2.Value = courseTitle;

                p1.ParameterName = "@Grade";

                p2.ParameterName = "@Title";

                string query = "SELECT c.Title, e.Grade " +
                                "FROM Courses c join Exams e " +
                                "on c.CourseID = e.CourseID " +
                                "WHERE e.Grade > @Grade and c.Title != @Title";

                SqlCommand command = new SqlCommand(query, connection);

                /*Dodajemo oba parametra. Redosled dodavanja nije vazan jer smo
                 jasno rekli koji su im nazivi i nece doci do greske.*/
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                //probajte da zamenite mesta p1 i p2 i videcete da radi isto
                

                SqlDataReader reader = command.ExecuteReader();

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
        /*3c - ubaciti custom studenta u bazu koriscenjem parametara*/
        void InsertStudentParameter(SqlConnection connection, string Name, string LastName, int year)
        {           
                

                SqlParameter p1 = new SqlParameter();
                SqlParameter p2 = new SqlParameter();
                SqlParameter p3 = new SqlParameter();

                p1.Value = Name;
                p2.Value = LastName;
                p3.Value = year;
                /*nazivi parametara ne moraju biti isti kao i nazivi kolona*/
                p1.ParameterName = "@Name";
                p2.ParameterName = "@LastName";
                p3.ParameterName = "@year";

                string query = "INSERT INTO Students (FirstName, LastName, Year) " +
                                "VALUES (@Name, @LastName, @year)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);

                command.ExecuteNonQuery();


            
        }
        //3d - Proizvoljno azurirati godinu studenta ciji id je prosledjen(id i year su parametri)
        void UpdateStudentParameter(SqlConnection connection, int year, int id)
        {
 
                SqlParameter p1 = new SqlParameter();
                SqlParameter p2 = new SqlParameter();

                p1.Value = year;
                p2.Value = id;

                p1.ParameterName = "@year";
                p2.ParameterName = "@id";

                string query = "UPDATE Students " +
                                "SET Year = @year " +
                                "WHERE StudentID = @id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(p1);
                command.Parameters.Add(p2);

                command.ExecuteNonQuery();

            
        }
        //3e - Obrisati bilo kog studenta ciji id se prosledjuje kao parametar
        void DeleteStudentParameter(SqlConnection connection, int id)
        {
                          

                SqlParameter p1 = new SqlParameter();

                p1.Value = id;

                p1.ParameterName = "@id";

                string query = "DELETE FROM Students " +
                                "WHERE StudentID = @id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(p1);

                command.ExecuteNonQuery();
            
        }

        /* DOMACI(parametarske varijante prethodnih domacih):
         * 3f - Dohvatiti sve info studenata cija godina je jednaka year. year je parametar.
         * 3g - Dohvatiti prezimena i ocene svih studenata koji su polozili ispit
         * ocenom vecom od grade i ciji Studentid je veci od id(grade i id su parametri).
         * 3h - Ispisati prezimena i prosecne ocene studenata na njihovim polaganjima pri cemu
         * ocene moraju biti razlicite od grade. Ispis grupisati po prezimenu studenata pri cemu
         * treba ispisati samo proseke vece od average. Proseke sortirati opadajuce.(grade i average su parametri).
         * 3i - CRUD parametarske operacije po izboru nad Exam tabelom. 
         * Vodite racuna o zavisnostima od Courses i Students Tabele!
         */

        /*DataAdapter sa parametrima
         Dokumentacija: https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/dataadapter-parameters 
        https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/updating-data-sources-with-dataadapters
        Sa DataAdapter-om smo samo radili Select upite.
        Insert, Update i Delete cemo raditi uz pomoc parametara.
        */

        /*3j - Napisati funkciju koja ispisuje preko DataTable koristeci DataAdapter.
         * Informacije studenata koji su year godina. year je parametar
         * 
         * 
         * */
        void PrintFromDataAdapterDataTableParameter(SqlConnection connection, int year)
        {

            System.Diagnostics.Debug.WriteLine("--PrintFromDataAdapterv2--");
            
                //napravimo parametar
                SqlParameter p = new SqlParameter();

                //dodelimo vrednosti na uobicajen nacin
                p.Value = year;

                p.ParameterName = "@year";

                string queryString =
                    "SELECT StudentID, LastName FROM Students " +
                        "WHERE Year = @year";

                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                SqlCommand command = new SqlCommand(queryString, connection);

                // u komandu ubacimo parametar kao i ranije
                command.Parameters.Add(p);

                //zatim takvu komandu sa parametrom dodelimo SelectCommand svojstvu adapter objekta
                adapter.SelectCommand = command;


                DataTable studentTable = new DataTable();
                adapter.Fill(studentTable);

                foreach (DataRow row in studentTable.Rows)
                {
                    System.Diagnostics.Debug.WriteLine("ID: {0} LastName: {1}", row[0], row[1]);
                    System.Diagnostics.Debug.WriteLine("---------------------------------------");
                }

            

        }
        //3k - napisati funkciju koja unosi bilo kakvog studenta ciji argumenti ce biti parametri
        void InsertStudentDataAdapterParameter(SqlConnection connection, string name, string surname, int year)
        {
           

                /*Rekli smo da se konstruktoru DataAdapteta prosledjuje upit koji se odmah
                 * upisuje u SelectCommand property. Ako zelimo da unesemo podatke u bazi nama
                 treba upit sa INSERT naredbom. Da bi nju upisali u dataAdapter moramo prvo
                dohvatiti podatke iz baze select upitom i onda dodati insert upit koji ce da
               ubaci podatke u RAM(program tj. c# objektima),
                tj. jos uvek nismo izmene uneli u bazu(disk).*/


                SqlDataAdapter dataAdapter = new SqlDataAdapter(
                  "SELECT * FROM Students",
                  connection);

                SqlParameter p1 = new SqlParameter();
                SqlParameter p2 = new SqlParameter();
                SqlParameter p3 = new SqlParameter();

                p1.Value = name;
                p2.Value = surname;
                p3.Value = year;

                p1.ParameterName = "@name";
                p2.ParameterName = "@surname";
                p3.ParameterName = "@year";

                string query = "INSERT INTO Students (FirstName, LastName, Year) " +
                                "VALUES (@name, @surname, @year);";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);

                //nakon ubacivanja parametara u komandu, dodelimo je insertCommand svojstvu dataAdaptera
                dataAdapter.InsertCommand = command;


                //nadalje uobicajen ispis
                DataTable studentTable = new DataTable();
                dataAdapter.Fill(studentTable);

                /*Da bi ubacili novog studenta moramo uraditi nesto cudno. Napravimo cemo novi
                 red newRow sa shemom podataka student tabele. U taj novi red cemo
                ubaciti neke random podatke i ubaciti ga u studentTable. Unos novog reda
                ce se samo videti u objektu studentTable, ali ne i u bazi.
                Ovo moramo uciniti da bi Update metod dataAdaptera upisao novog studenta u bazu.
                Novi student koji se unosi u bazu nije ovaj vec onaj definisan insertCommand svojstvom.
                Slicno cemo morati da uradimo sa update i delete komandama.
                Link: http://www.java2s.com/Tutorial/CSharp/0560__ADO.Net/Insertcommandwithparameters.htm
                 */

                DataRow newRow = studentTable.NewRow();

                newRow["StudentID"] = 0;
                newRow["LastName"] = "a";
                newRow["FirstName"] = "b";
                newRow["Year"] = 0;

                studentTable.Rows.Add(newRow);


                /*azuriramo bazu metodom Update(koristi se i za Delete i Update upite)*/
                dataAdapter.Update(studentTable);

            

        }
        //3l - Napisati funkciju koja azurira godinu studenta koristeci DataAdapter, godina i id su parametri
        // Dokumentacija: https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/updating-data-sources-with-dataadapters
        void UpdateStudentDataAdapterParameter(SqlConnection connection, int year, int id)
        {
            //funkcija vrsi izmenu podataka
            
                SqlDataAdapter dataAdapter = new SqlDataAdapter(
                  "SELECT * FROM Students",
                  connection);

                /*Rekli smo da se konstruktoru DataAdapteta prosledjuje upit koji se odmah
                 * upisuje u SelectCommand property. Ako zelimo da azuriramo podatke u bazi nama
                 treba upit sa UPDATE naredbom. Da bi nju upisali u dataAdapter moramo prvo
                dohvatiti podatke iz baze select upitom i onda dodati update upit koji ce da
                azurira podatke ali u RAM-u(program tj. c# objektima),
                tj. jos uvek nismo izmene uneli u bazu(disk).*/

                /*UpdateCommand property dataAdaptera dobija vrednost sql kommande*/
                /*OPREZ! DataAdapteru mora biti odredjen UpdateCommand property pre Fill metode!*/

                SqlParameter p1 = new SqlParameter();
                SqlParameter p2 = new SqlParameter();

                p1.Value = year;
                p2.Value = id;

                p1.ParameterName = "@year";
                p2.ParameterName = "@id";

                SqlCommand command = new SqlCommand("UPDATE Students SET Year = @year " +
                   "WHERE StudentID = @id", connection);

                command.Parameters.Add(p1);
                command.Parameters.Add(p2);

                dataAdapter.UpdateCommand = command;

                //mozete uraditi preko DataSet-a ili DataTable-a. Ovde smo preko DataTable

                DataTable studentTable = new DataTable();
                //DataSet studentSet = new DataSet();

                dataAdapter.Fill(studentTable);


                /*OPREZ!
                 Da bi metoda Update mogla da izmeni nesto u bazi, prethodno morate izvrsiti neku izmenu
                u DataTable. Bilo kakva izmena se moze uciniti nad bilo kojim podatkom jer se to cuva
                samo u RAM-u, a disku tj. bazi se salju izmene definisane SAMO upitom u UpdateCommand svojstvu
                DataAdaptera. Tako da je ovde svejedno sta cete napisati ali morate nesto.
                Npr. Ovde smo dohvatili prvi red tabele student i prvom podatku dodelili istu vrednost.
                Zvuci neobicno i bespotrebno ali ako biste zakomentarisali ove dve naredbe ne bi se desile
                izmene u bazi. Mozete probati da izmenite nesto drugo sto niste u update uputi i videcete
                da se ta promena nece odraziti na bazu nego samo na objekat studentTable.
                 */


                DataRow studentRow = studentTable.Rows[0]; // dohvati prvi red tabele
                studentRow[0] = studentRow[0]; // prvoj koloni prvog rade dodeli istu tu vrednost

                dataAdapter.Update(studentTable); //azuriramo bazu

            

        }
        //3m Napisati funkciju koja brise studenta sa datim id(parametar) preko DataAdaptera
        void DeleteStudentDataAdapterParameter(SqlConnection connection, int id)
        {
            
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Students", connection);

                DataTable studentTable = new DataTable();

                SqlCommand command = new SqlCommand("DELETE FROM Students " +
                                                        "WHERE StudentID = @id", connection);

                SqlParameter p = new SqlParameter();

                p.Value = id;
                p.ParameterName = "@id";
                //p.SourceVersion = DataRowVersion.Original;

                command.Parameters.Add(p);



                dataAdapter.DeleteCommand = command;

                dataAdapter.Fill(studentTable);
                /*
                 Opet moramo uciniti bilo sta sa StudentTable.
                Ovde mozemo dohvatiti prvi red i obrisati ga.
                Ovako ce DeleteCommmand property imati efekta.
                Link: https://social.msdn.microsoft.com/Forums/en-US/4bb34114-c049-4944-822c-19357c580224/deleting-rows-with-sqldataadapter?forum=adodotnetdataset
                 */

                DataRow studentRow = studentTable.Rows[0];
                studentRow.Delete(); //brisemo prvi red iz DataTable

                dataAdapter.Update(studentTable); //brisanje reda koji hocemo iz baze

            
        }


    }
}