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
        public static List<Preparat> getPreparate(string connection_string)
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

                List<string> lista_ingrediente = new List<string>();
                SqlDataReader data_reader_ingrediente =
                    fetch_denumire_ingrediente.ExecuteReader();

                while (data_reader_ingrediente.Read())
                {
                    string denumire_ingredient = data_reader_ingrediente.GetString(0);
                    lista_ingrediente.Add(denumire_ingredient);
                }

                Preparat preparat = new Preparat();
                preparat.Initialize(id, denumire, tip, pret, path, gramaj, specific,
                    lista_ingrediente);
                lista_preparate.Add(preparat);
            }



            return lista_preparate;
        }

        public static List<User> getUsers(string connection_string)
        {
            List<User> lista_users = new List<User>();
            SqlConnection db_connection_user = new SqlConnection(connection_string);
            db_connection_user.Open();

            SqlCommand fatch_users = new SqlCommand(@"select id_user, email, password, first_name, last_name, join_date 
                                                    from users", db_connection_user);

            SqlDataReader data_reader_user = fatch_users.ExecuteReader();

            //Citim pe rand atributele utilizatorilor.
            while (data_reader_user.Read())
            {
                int id = data_reader_user.GetInt32(0);
                string email = data_reader_user.GetString(1);
                string password = data_reader_user.GetString(2);
                string first_name = null;
                if (!data_reader_user.IsDBNull(3))
                {
                    first_name = data_reader_user.GetString(3);
                }
                string last_name = null;
                if (!data_reader_user.IsDBNull(4))
                {
                    last_name = data_reader_user.GetString(4);
                }
                DateTime join_date = data_reader_user.GetDateTime(5);

                User user = new User();
                user.Initialize(id, email, password, first_name, last_name, join_date);
                lista_users.Add(user);
            }

            return lista_users;
        }

        public static void insertUser(User user, string connection_string)
        {
            SqlConnection insert_user_connection = new SqlConnection(connection_string);
            insert_user_connection.Open();
            SqlCommand insert_user_command = new SqlCommand(
                                            @"insert into users 
                                              values(@email, @parola, @first_name, @last_name, @Date)",
                                              insert_user_connection);
            insert_user_command.Parameters.Add(new SqlParameter("@email", user.getEmail()));
            insert_user_command.Parameters.Add(new SqlParameter("@parola", user.getPassword()));
            if (user.getFirstName() == null)
                insert_user_command.Parameters.Add(new SqlParameter("@first_name", (object)DBNull.Value));
            else
                insert_user_command.Parameters.Add(new SqlParameter("@first_name", user.getFirstName()));

            if (user.getLastName() == null)
                insert_user_command.Parameters.Add(new SqlParameter("@last_name", (object)DBNull.Value));
            else
                insert_user_command.Parameters.Add(new SqlParameter("@last_name", user.getLastName()));
            insert_user_command.Parameters.Add(new SqlParameter("@date", user.getJoinDate()));
            insert_user_command.ExecuteNonQuery();
            insert_user_connection.Close();
            int id = getUserIdByEmail(user.getEmail(), connection_string);
            if (user.getSpecificsList() != null)
                insertSpecificsForUser(id, user.getSpecificsList(), connection_string);
        }

        public static int getUserIdByEmail(string email, string connection_string)
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

        public static bool checkEmailIfExist(string email, string connection_string)
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
            int appearance  = get_user_id_reader.GetInt32(0);
            get_user_id_reader.Close();
            get_user_id_connection.Close();
            if (appearance > 0)
                return true;
            return false;
        }

        public static int getSpecificId(string denumire_specific, string connection_string)
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

        private static void insertSpecificsForUser(int id, List<string> specifics_list, string connection_string)
        {
            foreach(string specific in specifics_list)
            {
                SqlConnection insert_specifics_for_user_connection = new SqlConnection(connection_string);
                insert_specifics_for_user_connection.Open();
                SqlCommand insert_specifics_for_user_command = new SqlCommand(
                    @"insert into prefera values(@user, @specific)",
                    insert_specifics_for_user_connection);
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@user",id));
                insert_specifics_for_user_command.Parameters.Add(new SqlParameter("@specific", getSpecificId(specific,connection_string)));
                insert_specifics_for_user_command.ExecuteNonQuery();
                insert_specifics_for_user_connection.Close();
            }
        }
    }
} // namespace