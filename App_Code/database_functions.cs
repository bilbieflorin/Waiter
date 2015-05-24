using System;
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
        // Intoarce lista de preparate din BD, fara sa ia in considerare
        // coloana "data_adaugare".
        public static List<Preparat> getPreparate(String connection_string)
        {
            List<Preparat> lista_preparate = new List<Preparat>();
            SqlConnection db_connection_preparate = new SqlConnection(connection_string);

            db_connection_preparate.Open();
            SqlCommand fetch_preparate = new SqlCommand(
                    @"select id_preparat, denumire_preparat, tip_preparat, path, gramaj, pret, denumire_specific
                      from preparate join specific on (
                      preparate.id_specific = specific.id_specific)",
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

                SqlConnection db_connection_ingrediente = new SqlConnection(connection_string);

                db_connection_ingrediente.Open();
                SqlCommand fetch_denumire_ingrediente = new SqlCommand(
                    @"select denumire
                            from ingrediente join contine on(ingrediente.id_ingredient = contine.id_ingredient)
                            where id_preparat = " + id, db_connection_ingrediente);

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
                
                Preparat preparat = new Preparat();
                preparat.Initialize(id, denumire, tip, pret, path, gramaj, specific,
                    lista_ingrediente);
                lista_preparate.Add(preparat);
            }
            data_reader_preparate.Close();
            db_connection_preparate.Close();
            return lista_preparate;
        }

        public static List<Ingredient> getIngrediente(String connection_string)
        {
            List<Ingredient> lista_ingrediente = new List<Ingredient>();
            SqlConnection db_connection_ingrediente = new SqlConnection(connection_string);
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

        public static List<User> getUsers(String connection_string)
        {
            List<User> lista_users = new List<User>();
            SqlConnection db_connection_user = new SqlConnection(connection_string);
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

        public static void insertUser(User user, String connection_string)
        {
            SqlConnection insert_user_connection = new SqlConnection(connection_string);
            insert_user_connection.Open();
            SqlCommand insert_user_command = new SqlCommand(
                                            @"insert into users (email, password, first_name, last_name, join_date)
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
            insert_user_command.ExecuteNonQuery();
            insert_user_connection.Close();
            int id = getUserIdByEmail(user.Email, connection_string);
            if (user.SpecificsList != null)
                insertSpecificsForUser(id, user.SpecificsList, connection_string);
        }

        public static int insertIngredient(Ingredient ingredient, string connection_string)
        {
            SqlConnection insert_ingredient_connection = new SqlConnection(connection_string);
            insert_ingredient_connection.Open();

            SqlCommand insert_ingredient_command = new SqlCommand(
                                           @"insert into ingrediente values(@denumire); SELECT SCOPE_IDENTITY();", 
                                           insert_ingredient_connection);
            insert_ingredient_command.Parameters.Add(new SqlParameter(@"denumire", ingredient.Denumire));

            int id_ingredient = Convert.ToInt32(insert_ingredient_command.ExecuteScalar());
            insert_ingredient_connection.Close();

            return id_ingredient;
        }

        public static void insertPreparatContineIngredient(int id_preparat, int id_ingredient, string connection_string)
        {
            SqlConnection insert_contine_connection = new SqlConnection(connection_string);
            insert_contine_connection.Open();

            SqlCommand insert_contine_command = new SqlCommand(
                @"insert into contine (id_preparat, id_ingredient) values(@id_preparat, @id_ingredient)",
                insert_contine_connection);
            insert_contine_command.Parameters.Add(new SqlParameter(@"id_preparat", id_preparat));
            insert_contine_command.Parameters.Add(new SqlParameter(@"id_ingredient", id_ingredient));
            insert_contine_command.ExecuteNonQuery();
            insert_contine_connection.Close();
        }

        public static int insertPreparat(Preparat preparat, String connection_string)
        {
            SqlConnection insert_preparat_connection = new SqlConnection(connection_string);
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
            insert_preparat_command.Parameters.Add(new SqlParameter(@"id_specific", getSpecificId(preparat.Specific, connection_string)));
            insert_preparat_command.Parameters.Add(new SqlParameter(@"data_adaugare", preparat.DataAdaugare));

            int id_preparat = Convert.ToInt32(insert_preparat_command.ExecuteScalar());
            insert_preparat_connection.Close();

            return id_preparat;
        }

        public static int getUserIdByEmail(String email, String connection_string)
        {
            SqlConnection get_user_id_connection = new SqlConnection(connection_string);
            get_user_id_connection.Open();
            SqlCommand get_user_id_command = new SqlCommand(
                        @"select id_user
                          from users
                          where email = @email",
                        get_user_id_connection);
            get_user_id_command.Parameters.Add(new SqlParameter("@email", email));
            SqlDataReader get_user_id_reader = get_user_id_command.ExecuteReader();
            get_user_id_reader.Read();
            int id = get_user_id_reader.GetInt32(0);
            get_user_id_reader.Close();
            get_user_id_connection.Close();
            return id;
        }

        public static bool checkEmailIfExist(String email, String connection_string)
        {
            SqlConnection get_user_id_connection = new SqlConnection(connection_string);
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

        public static int getSpecificId(String denumire_specific, String connection_string)
        {
            SqlConnection get_user_id_connection = new SqlConnection(connection_string);
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

        public static User checkEmailAndPasswordIfExists(String email, String password, String conection_string)
        {
            SqlConnection get_user_connection = new SqlConnection(conection_string);
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

        public static void trimiteComanda(Comanda comanda, String connection_string)
        {
            SqlConnection inserare_comanda_connection = new SqlConnection(connection_string);
            inserare_comanda_connection.Open();
            SqlCommand inserare_comanda_command = new SqlCommand(
                    @"insert into comenzi output inserted.id_comanda values(@data,@id_user,@valoare)",
                            inserare_comanda_connection);
            inserare_comanda_command.Parameters.Add(new SqlParameter("@data", DateTime.Now));
            if(comanda.IdUser!=0)
                inserare_comanda_command.Parameters.Add(new SqlParameter("@id_user", comanda.IdUser));
            else
                inserare_comanda_command.Parameters.Add(new SqlParameter("@id_user", (object)DBNull.Value));
            inserare_comanda_command.Parameters.Add(new SqlParameter("@valoare", comanda.Pret));
            int id_comanda = (int)inserare_comanda_command.ExecuteScalar();
            inserare_comanda_connection.Close();
            inserareItemComanda(comanda.ListaItem, id_comanda, connection_string);
        }
        
        private static void insertSpecificsForUser(int id, List<String> specifics_list, String connection_string)
        {
            foreach (string specific in specifics_list)
            {
                SqlConnection insert_specifics_for_user_connection = new SqlConnection(connection_string);
                insert_specifics_for_user_connection.Open();
                SqlCommand insert_specifics_for_user_command = new SqlCommand(
                    @"insert into prefera values(@user, @specific)",
                    insert_specifics_for_user_connection);
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@user", id));
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@specific", getSpecificId(specific, connection_string)));
                insert_specifics_for_user_command.ExecuteNonQuery();
                insert_specifics_for_user_connection.Close();
            }
        }

        private static void inserareItemComanda(Hashtable lista_item,int id_comanda, String connection_string)
        { 
            foreach(DictionaryEntry item in lista_item)
            {
                ItemComanda item_comanda = item.Value as ItemComanda;
                SqlConnection inserare_item_connection = new SqlConnection(connection_string);
                inserare_item_connection.Open();
                SqlCommand inserare_item_command = new SqlCommand(
                        @"insert into preparate_comanda
                          values( @id_comanda, @id_preparat, @cantitate)",
                        inserare_item_connection);
                inserare_item_command.Parameters.Add(new SqlParameter("@id_comanda",id_comanda));
                inserare_item_command.Parameters.Add(new SqlParameter("@id_preparat", item_comanda.Preparat.Id));
                inserare_item_command.Parameters.Add(new SqlParameter("@cantitate", item_comanda.Cantitate));
                inserare_item_command.ExecuteNonQuery();
                inserare_item_connection.Close();
            }
        }
    
    }
} // namespace