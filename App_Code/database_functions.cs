﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace db_mapping
{

    /// <summary>
    /// Functii de lucru cu baza de date.
    /// </summary>
    public class DatabaseFunctions
    {
        /// <summary>
        ///  Intoarce lista de preparate din BD, fara sa ia in considerare
        /// coloana "data_adaugare".
        /// </summary>
        public static List<Preparat> getPreparate()
        {
            List<Preparat> lista_preparate = new List<Preparat>();
            SqlConnection db_connection_preparate = new SqlConnection(connection_string_);

            db_connection_preparate.Open();
            SqlCommand fetch_preparate = new SqlCommand(
                    @"select id_preparat, denumire_preparat, tip_preparat, path, gramaj, pret, denumire_specific
                      from preparate join specific on (
                      preparate.id_specific = specific.id_specific)
                      order by case tip_preparat
                            when 'Mic dejun' then 1
                            when 'Pizza' then 2
                            when 'Paste' then 3
                            when 'Aperitiv' then 4
                            when 'Ciorba si supe' then 5
                            when 'Fructe de mare' then 6
                            when 'Peste' then 7
                            when 'Preparate din carne de pui' then 8
                            when 'Preparate din carne de porc' then 9
                            when 'Preparate din carne de vita' then 10
                            when 'Preparate din vanat' then 11
                            when 'Salate' then 12
                            when 'Desert' then 13
                            when 'Bauturi' then 14
                        end",
                      db_connection_preparate);

            SqlDataReader data_reader_preparate = fetch_preparate.ExecuteReader();

            // Cat timp se poate citi, citim pe rand atributele
            // fiecarui preparat din baza de date.
            while (data_reader_preparate.Read())
            {
                int id = data_reader_preparate.GetInt32(0);
                string denumire = data_reader_preparate.GetString(1);
                string tip = data_reader_preparate.GetString(2);
                string path = null;
                if (!data_reader_preparate.IsDBNull(3))
                {
                    path = data_reader_preparate.GetString(3);
                }
                double gramaj = data_reader_preparate.GetDouble(4);
                double pret = data_reader_preparate.GetDouble(5);
                string specific = null;
                if (!data_reader_preparate.IsDBNull(6))
                {
                    specific = data_reader_preparate.GetString(6);
                }

                Preparat preparat = new Preparat();
                preparat.Initialize(id, denumire, tip, pret, path, gramaj, specific,
                    ingredientePreparat(id));
                lista_preparate.Add(preparat);
            }
            data_reader_preparate.Close();
            db_connection_preparate.Close();
            return lista_preparate;
        }

        public static Preparat getPreparat(int id_preparat)
        {
            Preparat preparat = null;
            SqlConnection db_connection_preparate = new SqlConnection(connection_string_);

            db_connection_preparate.Open();
            SqlCommand fetch_preparat = new SqlCommand(
                    @"select denumire_preparat, tip_preparat, path, gramaj, pret, denumire_specific
                      from preparate join specific on (
                      preparate.id_specific = specific.id_specific)
                      where id_preparat = @id_preparat",
                      db_connection_preparate);

            fetch_preparat.Parameters.Add(new SqlParameter("@id_preparat", id_preparat));

            SqlDataReader data_reader_preparat = fetch_preparat.ExecuteReader();

            if (data_reader_preparat.Read())
            {
                string denumire = data_reader_preparat.GetString(0);
                string tip = data_reader_preparat.GetString(1);
                string path = null;
                if (!data_reader_preparat.IsDBNull(2))
                {
                    path = data_reader_preparat.GetString(2);
                }
                double gramaj = data_reader_preparat.GetDouble(3);
                double pret = data_reader_preparat.GetDouble(4);
                string specific = null;
                if (!data_reader_preparat.IsDBNull(5))
                {
                    specific = data_reader_preparat.GetString(5);
                }

                preparat = new Preparat();
                preparat.Initialize(id_preparat, denumire, tip, pret, path, gramaj, specific,
                    ingredientePreparat(id_preparat));
            }
            data_reader_preparat.Close();
            db_connection_preparate.Close();
            return preparat;
        }

        public static List<Preparat> getPotriviriPreparat(Preparat preparat)
        {
            List<Preparat> lista_preparate = new List<Preparat>();
            SqlConnection db_connection_potrivire = new SqlConnection(connection_string_);

            db_connection_potrivire.Open();
            SqlCommand fetch_potrivire = new SqlCommand(
                    @"select id_preparat1 
                      from potrivire
                      where id_preparat2 = @id_preparat
                      union
                      select id_preparat2 
                      from potrivire
                      where id_preparat1 = @id_preparat",
                      /* @"select case
                                when id_preparat1 = "+preparat.Id+" then id_preparat2 "+
                                "when id_preparat2 = " + preparat.Id + " then id_preparat1 " + 
                            " end "+
                      " from potrivire",*/
                      //where id_preparat1 = @id_preparat or id_preparat2 = @id_preparat" ,
                      db_connection_potrivire);

            fetch_potrivire.Parameters.Add(new SqlParameter("@id_preparat", preparat.Id));

            SqlDataReader data_reader_potrivire = fetch_potrivire.ExecuteReader();

            while (data_reader_potrivire.Read())
            {
                int id_preparat_potrivit = data_reader_potrivire.GetInt32(0);

                Preparat preparat_potrivit = getPreparat(id_preparat_potrivit);
                if (preparat_potrivit != null)
                    lista_preparate.Add(preparat_potrivit);
            }
            data_reader_potrivire.Close();
            db_connection_potrivire.Close();
            return lista_preparate;
        }

        /// <summary>
        /// Intoarce lista de ingrediente din baza de date
        /// </summary>
        public static List<Ingredient> getIngrediente()
        {
            List<Ingredient> lista_ingrediente = new List<Ingredient>();
            SqlConnection db_connection_ingrediente = new SqlConnection(connection_string_);
            db_connection_ingrediente.Open();

            SqlCommand fetch_ingrediente = new SqlCommand(@"select id_ingredient, denumire
                                                            from ingrediente", db_connection_ingrediente);

            SqlDataReader data_reader_ingredient = fetch_ingrediente.ExecuteReader();

            while (data_reader_ingredient.Read())
            {
                int id = data_reader_ingredient.GetInt32(0);
                String denumire = data_reader_ingredient.GetString(1);

                Ingredient ingredient = new Ingredient();
                ingredient.Initialize(id, denumire);
                lista_ingrediente.Add(ingredient);
            }
            data_reader_ingredient.Close();
            db_connection_ingrediente.Close();

            return lista_ingrediente;
        }

        /// <summary>
        /// Intoarce lista de utilizatori din baza de date
        /// </summary>
        public static List<User> getUsers()
        {
            List<User> lista_users = new List<User>();
            SqlConnection db_connection_user = new SqlConnection(connection_string_);
            db_connection_user.Open();

            SqlCommand fatch_users = new SqlCommand(@"select id_user, email, password, first_name, last_name, join_date, type 
                                                    from users", db_connection_user);

            SqlDataReader data_reader_user = fatch_users.ExecuteReader();

            //Citim pe rand atributele utilizatorilor.
            while (data_reader_user.Read())
            {
                int id = data_reader_user.GetInt32(0);
                String email = data_reader_user.GetString(1);
                String password = data_reader_user.GetString(2);
                String first_name = null;
                String type = data_reader_user.GetString(6);
                if (!data_reader_user.IsDBNull(3))
                {
                    first_name = data_reader_user.GetString(3);
                }
                String last_name = null;
                if (!data_reader_user.IsDBNull(4))
                {
                    last_name = data_reader_user.GetString(4);
                }
                DateTime join_date = data_reader_user.GetDateTime(5);

                User user = new User();
                user.Initialize(id, email, password, first_name, last_name, join_date, type);
                lista_users.Add(user);
            }
            data_reader_user.Close();
            db_connection_user.Close();
            return lista_users;
        }

        public static void updateUser(User user)
        {
            SqlConnection update_user_connection = new SqlConnection(connection_string_);
            update_user_connection.Open();
            SqlCommand update_user_command = new SqlCommand(
                                            @"update users
                                              set first_name = @first_name, last_name = @last_name 
                                              where id_user = @id_user",
                                              update_user_connection);
            if (user.FirstName == null)
                update_user_command.Parameters.Add(new SqlParameter("@first_name", (object)DBNull.Value));
            else
                update_user_command.Parameters.Add(new SqlParameter("@first_name", user.FirstName));
            if (user.LastName == null)
                update_user_command.Parameters.Add(new SqlParameter("@last_name", (object)DBNull.Value));
            else
                update_user_command.Parameters.Add(new SqlParameter("@last_name", user.LastName));
            update_user_command.Parameters.Add(new SqlParameter("@id_user", user.Id));
            update_user_command.ExecuteNonQuery();
            update_user_connection.Close();

            deleteSpecificsForUser(user.Id);
            if (user.SpecificsList != null)
                insertSpecificsForUser(user.Id, user.SpecificsList);
        }

        /// <summary>
        /// Adauga un nou utilizator la baza de date si intoarce id-ul liniei inserate
        /// </summary>
        public static int insertUser(User user)
        {
            SqlConnection insert_user_connection = new SqlConnection(connection_string_);
            insert_user_connection.Open();
            SqlCommand insert_user_command = new SqlCommand(
                                            @"insert into users (email, password, first_name, last_name, join_date)
                                              output inserted.id_user
                                              values(@email, @parola, @first_name, @last_name, @Date)",
                                              insert_user_connection);
            insert_user_command.Parameters.Add(new SqlParameter("@email", user.Email));
            insert_user_command.Parameters.Add(new SqlParameter("@parola", user.Password));
            if (user.FirstName == null)
                insert_user_command.Parameters.Add(new SqlParameter("@first_name", (object)DBNull.Value));
            else
                insert_user_command.Parameters.Add(new SqlParameter("@first_name", user.FirstName));

            if (user.LastName == null)
                insert_user_command.Parameters.Add(new SqlParameter("@last_name", (object)DBNull.Value));
            else
                insert_user_command.Parameters.Add(new SqlParameter("@last_name", user.LastName));
            insert_user_command.Parameters.Add(new SqlParameter("@date", user.JoinDate));
            int id = (int)insert_user_command.ExecuteScalar();
            insert_user_connection.Close();
            if (user.SpecificsList != null)
                insertSpecificsForUser(id, user.SpecificsList);
            return id;
        }

        /// <summary>
        /// Adauga un ingredient nou la baza de date
        /// </summary>
        public static int insertIngredient(Ingredient ingredient)
        {
            SqlConnection insert_ingredient_connection = new SqlConnection(connection_string_);
            insert_ingredient_connection.Open();

            SqlCommand insert_ingredient_command = new SqlCommand(
                                           @"insert into ingrediente values(@denumire); SELECT SCOPE_IDENTITY();",
                                           insert_ingredient_connection);
            insert_ingredient_command.Parameters.Add(new SqlParameter(@"denumire", ingredient.Denumire));

            int id_ingredient = Convert.ToInt32(insert_ingredient_command.ExecuteScalar());
            insert_ingredient_connection.Close();

            return id_ingredient;
        }

        /// <summary>
        ///Introduce o noua combinatie preparat-ingredient in baza de date
        /// </summary>
        public static void insertPreparatContineIngredient(int id_preparat, int id_ingredient)
        {
            SqlConnection insert_contine_connection = new SqlConnection(connection_string_);
            insert_contine_connection.Open();

            SqlCommand insert_contine_command = new SqlCommand(
                @"insert into contine (id_preparat, id_ingredient) values(@id_preparat, @id_ingredient)",
                insert_contine_connection);
            insert_contine_command.Parameters.Add(new SqlParameter(@"id_preparat", id_preparat));
            insert_contine_command.Parameters.Add(new SqlParameter(@"id_ingredient", id_ingredient));
            insert_contine_command.ExecuteNonQuery();
            insert_contine_connection.Close();
        }

        /// <summary>
        ///Insereaza un nou preparat in baza de date
        /// </summary>
        public static int insertPreparat(Preparat preparat)
        {
            SqlConnection insert_preparat_connection = new SqlConnection(connection_string_);
            insert_preparat_connection.Open();
            SqlCommand insert_preparat_command = new SqlCommand("insert into preparate values(@denumire_preparat, @path, @tip_preparat, @gramaj, @pret, @id_specific, @data_adaugare); SELECT SCOPE_IDENTITY();",
                                                                insert_preparat_connection);
            insert_preparat_command.Parameters.Add(new SqlParameter(@"denumire_preparat", preparat.Denumire));
            if (preparat.PathImagine == null)
                insert_preparat_command.Parameters.Add(new SqlParameter(@"path", (object)DBNull.Value));
            else
                insert_preparat_command.Parameters.Add(new SqlParameter(@"path", preparat.PathImagine));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"tip_preparat", preparat.Tip));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"gramaj", preparat.Gramaj));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"pret", preparat.Pret));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"id_specific", getSpecificId(preparat.Specific)));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"data_adaugare", preparat.DataAdaugare));

            int id_preparat = Convert.ToInt32(insert_preparat_command.ExecuteScalar());
            insert_preparat_connection.Close();

            return id_preparat;
        }

        /// <summary>
        /// Verifica daca exista email-ulul dat ca parametru in baza de date
        /// </summary>
        public static bool checkEmailIfExist(String email)
        {
            SqlConnection get_user_id_connection = new SqlConnection(connection_string_);
            get_user_id_connection.Open();
            SqlCommand get_user_id_command = new SqlCommand(
                        @"select count(*)
                          from users
                          where email = @email",
                        get_user_id_connection);
            get_user_id_command.Parameters.Add(new SqlParameter("@email", email));
            SqlDataReader get_user_id_reader = get_user_id_command.ExecuteReader();
            get_user_id_reader.Read();
            int appearance = get_user_id_reader.GetInt32(0);
            get_user_id_reader.Close();
            get_user_id_connection.Close();
            if (appearance > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Intoarce id-ul specificului cu denumirea denumire_specific
        /// </summary>
        public static int getSpecificId(String denumire_specific)
        {
            SqlConnection get_user_id_connection = new SqlConnection(connection_string_);
            get_user_id_connection.Open();
            SqlCommand get_user_id_command = new SqlCommand(
                        @"select id_specific
                          from specific
                          where denumire_specific = @denumire_specific",
                        get_user_id_connection);
            get_user_id_command.Parameters.Add(new SqlParameter("@denumire_specific", denumire_specific));
            SqlDataReader get_user_id_reader = get_user_id_command.ExecuteReader();
            get_user_id_reader.Read();
            int id = get_user_id_reader.GetInt32(0);
            get_user_id_reader.Close();
            get_user_id_connection.Close();
            return id;
        }

        /// <summary>
        /// Intoarce numarul mediu de comandari al unui preparat al utilizatorilui cu id-ul id_user
        /// </summary>
        public static int numarComandariPreparat(int id_user, int id_preparat)
        {
            SqlConnection connection = new SqlConnection(connection_string_);
            connection.Open();
            SqlCommand command = new SqlCommand(
                    @"select numar_comandari
                      from frecvente
                      where id_user = @id and id_preparat = @id_prep", connection);
            command.Parameters.Add(new SqlParameter("@id",id_user));
            command.Parameters.Add(new SqlParameter("@id_prep", id_preparat));
            SqlDataReader reader = command.ExecuteReader();
            int nr = 0;
            if (reader.Read())
                nr = Convert.ToInt32(reader.GetInt32(0)) + 1;
            reader.Close();
            connection.Close();
            return nr;
        }

        /// <summary>
        /// Intoarce numarul de comandari al preparaturlui id_preparat al utilizatorilui cu id-ul id_user
        /// </summary>
        public static int numarMediuComandariPreparat(int id_user)
        {
            SqlConnection connection = new SqlConnection(connection_string_);
            connection.Open();
            SqlCommand command = new SqlCommand(
                    @"select isnull(avg(numar_comandari),0)
                      from frecvente
                      where id_user = @id", connection);
            command.Parameters.Add(new SqlParameter("@id", id_user));
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int avg = reader.GetInt32(0);
            reader.Close();
            connection.Close();
            return avg;
        }

        /// <summary>
        /// Intoarce lista de denumiri ale specificelor
        /// </summary>
        public static List<String> getSpecifice()
        {
            List<String> lista_specifice = new List<String>();
            SqlConnection db_connection_specifice = new SqlConnection(connection_string_);

            db_connection_specifice.Open();
            SqlCommand fetch_specifice = new SqlCommand(@"select denumire_specific
                                                          from specific
                                                          order by denumire_specific",
                                                          db_connection_specifice);

            SqlDataReader data_reader_specifice = fetch_specifice.ExecuteReader();

            while (data_reader_specifice.Read())
            {
                string denumire = data_reader_specifice.GetString(0);
                lista_specifice.Add(denumire);
            }
            data_reader_specifice.Close();
            db_connection_specifice.Close();
            return lista_specifice;
        }

        /// <summary>
        /// Intoarce lista de specifice pentru user-ul cu care id-ul id_user
        /// </summary>
        public static List<String> getSpecificsForUser(int id_user)
        {
            List<String> lista_specifice = new List<String>();
            SqlConnection db_connection_specifice = new SqlConnection(connection_string_);

            db_connection_specifice.Open();
            SqlCommand fetch_specifice = new SqlCommand(@"select denumire_specific
                                                          from prefera join specific on (prefera.id_specific = specific.id_specific)
                                                          where id_user = @id
                                                          order by denumire_specific",
                                                          db_connection_specifice);
            fetch_specifice.Parameters.Add(new SqlParameter("@id", id_user));

            SqlDataReader data_reader_specifice = fetch_specifice.ExecuteReader();

            while (data_reader_specifice.Read())
            {
                string denumire = data_reader_specifice.GetString(0);
                lista_specifice.Add(denumire);
            }
            data_reader_specifice.Close();
            db_connection_specifice.Close();
            return lista_specifice.Count > 0 ? lista_specifice : null;
        }

        /// <summary>
        /// Verifica daca exista combinatia email-parola in baza de date
        /// </summary>
        public static User checkEmailAndPasswordIfExists(String email, String password)
        {
            SqlConnection get_user_connection = new SqlConnection(connection_string_);
            get_user_connection.Open();

            SqlCommand get_user_connection_command = new SqlCommand(@"select *
                                                                 from users
                                                                 where email = @email and password = @password",
                                                                get_user_connection);
            get_user_connection_command.Parameters.Add(new SqlParameter("@email", email));
            get_user_connection_command.Parameters.Add(new SqlParameter("@password", password));

            SqlDataReader get_user_reader = get_user_connection_command.ExecuteReader();
            User user = new User();
            if (get_user_reader.Read())
            {
                user.Initialize(
                    get_user_reader.GetInt32(0),
                    get_user_reader.GetString(1),
                    get_user_reader.GetString(2),
                    (get_user_reader.IsDBNull(3)) ? null : get_user_reader.GetString(3),
                    (get_user_reader.IsDBNull(4)) ? null : get_user_reader.GetString(4),
                    get_user_reader.GetDateTime(5),
                    get_user_reader.GetString(6),
                    null);
            }
            get_user_reader.Close();
            get_user_connection.Close();
            return user;
        }

        /// <summary>
        /// Insereaza o comanda in baza de date
        /// </summary>
        public static void trimiteComanda(Comanda comanda)
        {
            SqlConnection inserare_comanda_connection = new SqlConnection(connection_string_);
            inserare_comanda_connection.Open();
            SqlCommand inserare_comanda_command = new SqlCommand(
                    @"insert into comenzi output inserted.id_comanda values(@data,@id_user,@valoare)",
                            inserare_comanda_connection);
            inserare_comanda_command.Parameters.Add(new SqlParameter("@data", DateTime.Now));
            if (comanda.IdUser != 0)
                inserare_comanda_command.Parameters.Add(new SqlParameter("@id_user", comanda.IdUser));
            else
                inserare_comanda_command.Parameters.Add(new SqlParameter("@id_user", (object)DBNull.Value));
            inserare_comanda_command.Parameters.Add(new SqlParameter("@valoare", comanda.Pret));
            int id_comanda = (int)inserare_comanda_command.ExecuteScalar();
            inserare_comanda_connection.Close();
            inserareItemComanda(comanda.ListaItem, id_comanda);
        }

        /// <summary>
        /// Intoarce un dictionar de perechu de forma
        /// id_user-vector de id-uri de preparate comandate
        /// </summary>
        public static Dictionary<int, List<int>> preparateComandateDupaUtilizator()
        {
            List<User> users = getUsers();
            Dictionary<int, List<int>> dictionar = new Dictionary<int, List<int>>();
            foreach (User user in users)
            {
                dictionar.Add(user.Id, new List<int>());
            }
            SqlConnection preparate_comandate_connection = new SqlConnection(connection_string_);
            preparate_comandate_connection.Open();
            SqlCommand preparate_comandate_command = new SqlCommand(
                @"select id_user, id_preparat, numar_comandari
                  from frecvente
                  where id_preparat not in (66,65)", preparate_comandate_connection);
            SqlDataReader preparate_comandate_reader = preparate_comandate_command.ExecuteReader();
            while (preparate_comandate_reader.Read())
            {
                int id_user = preparate_comandate_reader.GetInt32(0);
                int id_preparat = preparate_comandate_reader.GetInt32(1);
                int cantitate = preparate_comandate_reader.GetInt32(2);
                //for (int i = 1; i <= cantitate; i++)
                //{
                    dictionar[id_user].Add(id_preparat);
                //}
            }
            preparate_comandate_reader.Close();
            preparate_comandate_connection.Close();
            return dictionar;
        }

        /// <summary>
        /// Intoarce o lista de IstoricComenzi pentru id-urile 
        /// utilizatorilor din vectorul users
        /// </summary>
        public static List<IstoricComenzi> istoricUtilizatori(int[] users)
        {
            List<IstoricComenzi> lista_istoric = new List<IstoricComenzi>();
            foreach (var user in users)
            {
                lista_istoric.Add(getIstoric(user));
            }
            return lista_istoric;
        }

        /// <summary>
        /// Intoarce istoricul comenzilor pentru utilizatorul cu id-ul id_user
        /// </summary>
        public static IstoricComenzi getIstoric(int user)
        {
            IstoricComenzi istoric = new IstoricComenzi(user);
            SqlConnection istoric_user_connection = new SqlConnection(connection_string_);
            istoric_user_connection.Open();
            SqlCommand istoric_user_command = new SqlCommand(
                    @"select id_comanda, data, valoare
                          from comenzi
                          where id_user = @user", istoric_user_connection);
            istoric_user_command.Parameters.Add(new SqlParameter("@user", user));
            SqlDataReader istoric_user_reader = istoric_user_command.ExecuteReader();
            while (istoric_user_reader.Read())
            {
                int id_comanda = istoric_user_reader.GetInt32(0);
                Comanda comanda = getComanda(id_comanda);
                comanda.IdUser = user;
                comanda.Pret = istoric_user_reader.GetDouble(2);
                comanda.Data = istoric_user_reader.GetDateTime(1);
                istoric.addComanda(comanda);
            }
            istoric_user_reader.Close();
            istoric_user_connection.Close();
            return istoric;
        }

        /// <summary>
        /// Intoarce cele mai comandate k preparate din lista de specifice data ca parametru 
        /// </summary>
        public static List<Preparat> topKPreparateSpecific(List<String> specifics, int k)
        {
            List<Preparat> preparate = new List<Preparat>();
            string lista_specifice="";
            int i = 1;
            foreach (var specific in specifics)
            {
                lista_specifice += "@specific"+i+",";
                i++;
            }
            lista_specifice = lista_specifice.Substring(0,lista_specifice.Length-1);
            SqlConnection connection = new SqlConnection(connection_string_);
            connection.Open();
            SqlCommand command = new SqlCommand(
                @"select top "+k+" p.id_preparat, denumire_preparat, pret, gramaj, denumire_specific, path, tip_preparat, sum(cantitate) "+
                  "from preparate as p join preparate_comanda as pc on pc.id_preparat=p.id_preparat "+
                                      "join specific as s on p.id_specific = s.id_specific "+
                  "where denumire_specific in ("+lista_specifice+")  and p.id_preparat not in (65,66)"+
                  "group by p.id_preparat, denumire_preparat, pret, gramaj, denumire_specific, path, tip_preparat " +
                  "order by sum(cantitate) desc", connection);
            i = 1;
            foreach (var specific in specifics)
            {
                command.Parameters.Add(new SqlParameter("@specific" + i, specific));
                i++;
            }
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string denumire = reader.GetString(1);
                double pret = reader.GetDouble(2);
                double gramaj = reader.GetDouble(3);
                string specific = reader.GetString(4);
                string path = reader.GetString(5);
                string tip = reader.GetString(6);
                Preparat preparat = new Preparat();
                preparat.Initialize(id,denumire,tip,pret,path,gramaj,specific,ingredientePreparat(id));
                preparate.Add(preparat);
            }
            reader.Close();
            connection.Close();
            return preparate;
        }

        /// <summary>
        /// Intoarce cele mai comandate k preparate  
        /// </summary>
        public static List<Preparat> topKPreparate(int k)
        {
            List<Preparat> preparate = new List<Preparat>();
            SqlConnection connection = new SqlConnection(connection_string_);
            connection.Open();
            SqlCommand command = new SqlCommand(
                @"select top " + k + " p.id_preparat, denumire_preparat, pret, gramaj, denumire_specific, path, tip_preparat, sum(cantitate) " +
                  "from preparate as p join preparate_comanda as pc on pc.id_preparat=p.id_preparat " +
                                      "join specific as s on p.id_specific = s.id_specific " +
                  "where p.id_preparat not in (65,66) "+
                  "group by p.id_preparat, denumire_preparat, pret, gramaj, denumire_specific, path, tip_preparat " +
                  "order by sum(cantitate) desc", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string denumire = reader.GetString(1);
                double pret = reader.GetDouble(2);
                double gramaj = reader.GetDouble(3);
                string specific = reader.GetString(4);
                string path = reader.GetString(5);
                string tip = reader.GetString(6);
                Preparat preparat = new Preparat();
                preparat.Initialize(id, denumire, tip, pret, path, gramaj, specific, ingredientePreparat(id));
                preparate.Add(preparat);
            }
            reader.Close();
            connection.Close();
            return preparate;
        }

        /// <summary>
        /// Intoarce preparatele care au pretul in intervalul[0.7 * pret_mediu, 1.3 * pret_mediu]
        /// </summary>
        public static List<Preparat> preparateDupaPret(double pret_mediu)
        {
            List<Preparat> preparate = new List<Preparat>();
            SqlConnection connection = new SqlConnection(connection_string_);
            connection.Open();
            SqlCommand command = new SqlCommand(
                @"select id_preparat, denumire_preparat, pret, gramaj, denumire_specific, path, tip_preparat
                  from preparate join specific on preparate.id_specific = specific.id_specific
                  where pret <= 1.2*@pret_mediu and id_preparat not in (65,66)", connection);
            command.Parameters.Add(new SqlParameter("@pret_mediu",pret_mediu));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string denumire = reader.GetString(1);
                double pret = reader.GetDouble(2);
                double gramaj = reader.GetDouble(3);
                string specific = reader.GetString(4);
                string path = reader.GetString(5);
                string tip = reader.GetString(6);
                Preparat preparat = new Preparat();
                preparat.Initialize(id, denumire, tip, pret, path, gramaj, specific, ingredientePreparat(id));
                preparate.Add(preparat);
            }
            reader.Close();
            connection.Close();
            return preparate;
        }

        /// <summary>
        /// Connection string-ul pentru legatura la baza de date 
        /// </summary>
        private static String connection_string_ = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// Intoarce comanda cu id-ul  id_comanda
        /// </summary>
        private static Comanda getComanda(int id_comanda)
        {
            Comanda comanda = new Comanda();
            SqlConnection comanda_connection = new SqlConnection(connection_string_);
            comanda_connection.Open();
            SqlCommand comanda_command = new SqlCommand(
                @"select preparate.id_preparat, denumire_preparat, gramaj, path, pret, denumire_specific, tip_preparat, cantitate, data_adaugare
                  from preparate_comanda join preparate on preparate_comanda.id_preparat=preparate.id_preparat
                                         join specific on preparate.id_specific=specific.id_specific
                  where id_comanda = @id_comanda and preparate.id_preparat not in(65,66)", comanda_connection);
            comanda_command.Parameters.Add(new SqlParameter("@id_comanda", id_comanda));
            SqlDataReader comanda_reader = comanda_command.ExecuteReader();
            while (comanda_reader.Read())
            {
                int id_preparat = comanda_reader.GetInt32(0);
                string denumire = comanda_reader.GetString(1);
                double gramaj = comanda_reader.GetDouble(2);
                string path = comanda_reader.GetString(3);
                double pret = comanda_reader.GetDouble(4);
                string specific = comanda_reader.GetString(5);
                string tip = comanda_reader.GetString(6);
                int cantiate = comanda_reader.GetInt32(7);
                DateTime data_adaugare = comanda_reader.GetDateTime(8);
                Preparat p = new Preparat();
                p.Initialize(id_preparat, denumire, tip, pret, path, gramaj, specific, ingredientePreparat(id_preparat), data_adaugare);
                comanda.addItemComanda(new ItemComanda(p, cantiate));
            }
            comanda_reader.Close();
            comanda_connection.Close();
            return comanda;
        }

        /// <summary>
        /// Intoarce lista de ingrediente pentru un preparat
        /// </summary>
        private static List<String> ingredientePreparat(int id_preparat)
        {
            SqlConnection db_connection_ingrediente = new SqlConnection(connection_string_);

            db_connection_ingrediente.Open();
            SqlCommand fetch_denumire_ingrediente = new SqlCommand(
                @"select denumire
                            from ingrediente join contine on(ingrediente.id_ingredient = contine.id_ingredient)
                            where id_preparat = @id_preparat", db_connection_ingrediente);
            fetch_denumire_ingrediente.Parameters.Add(new SqlParameter("@id_preparat", id_preparat));
            List<String> lista_ingrediente = new List<String>();
            SqlDataReader data_reader_ingrediente =
                fetch_denumire_ingrediente.ExecuteReader();

            while (data_reader_ingrediente.Read())
            {
                String denumire_ingredient = data_reader_ingrediente.GetString(0);
                lista_ingrediente.Add(denumire_ingredient);
            }

            data_reader_ingrediente.Close();
            db_connection_ingrediente.Close();
            return lista_ingrediente;
        }

        /// <summary>
        /// Insereaza o lista de specifice pentru un utilizator
        /// </summary>
        private static void insertSpecificsForUser(int id, List<String> specifics_list)
        {
            foreach (string specific in specifics_list)
            {
                SqlConnection insert_specifics_for_user_connection = new SqlConnection(connection_string_);
                insert_specifics_for_user_connection.Open();
                SqlCommand insert_specifics_for_user_command = new SqlCommand(
                    @"insert into prefera values(@user, @specific)",
                    insert_specifics_for_user_connection);
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@user", id));
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@specific", getSpecificId(specific)));
                insert_specifics_for_user_command.ExecuteNonQuery();
                insert_specifics_for_user_connection.Close();
            }
        }

        /// <summary>
        /// Insereaza o lista de ItemComanda in baza de date
        /// </summary>
        private static void inserareItemComanda(Hashtable lista_item, int id_comanda)
        {
            foreach (DictionaryEntry item in lista_item)
            {
                ItemComanda item_comanda = item.Value as ItemComanda;
                SqlConnection inserare_item_connection = new SqlConnection(connection_string_);
                inserare_item_connection.Open();
                SqlCommand inserare_item_command = new SqlCommand(
                        @"insert into preparate_comanda
                          values( @id_comanda, @id_preparat, @cantitate)",
                        inserare_item_connection);
                inserare_item_command.Parameters.Add(new SqlParameter("@id_comanda", id_comanda));
                inserare_item_command.Parameters.Add(new SqlParameter("@id_preparat", item_comanda.Preparat.Id));
                inserare_item_command.Parameters.Add(new SqlParameter("@cantitate", item_comanda.Cantitate));
                inserare_item_command.ExecuteNonQuery();
                inserare_item_connection.Close();
            }
        }

        private static void deleteSpecificsForUser(int id)
        {
            SqlConnection delete_specifics_for_user_connection = new SqlConnection(connection_string_);
            delete_specifics_for_user_connection.Open();
            SqlCommand delete_specifics_for_user_command = new SqlCommand(
                @"delete from prefera
                  where id_user = @id_user",
                  delete_specifics_for_user_connection);
            delete_specifics_for_user_command.Parameters.Add(new SqlParameter("@id_user", id));
            delete_specifics_for_user_command.ExecuteNonQuery();
            delete_specifics_for_user_connection.Close();
        }

        public static int numarUtilizatori() 
        {
            int count;
            SqlConnection users_count_connection = new SqlConnection(connection_string_);
            users_count_connection.Open();
            SqlCommand users_count_command = new SqlCommand(@"select count(*)
                                                              from users", users_count_connection);
            SqlDataReader users_count_reader = users_count_command.ExecuteReader();
            users_count_reader.Read();
            count = users_count_reader.GetInt32(0);
            users_count_reader.Close();
            users_count_connection.Close();

            return count;

        }
    }
} // namespace