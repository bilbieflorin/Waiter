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
        static public List<Preparat> getPreparate(string connection_string)
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
                double cantitate = data_reader_preparate.GetDouble(4);
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
                preparat.Initialize(id, denumire, tip, pret, path, cantitate, specific,
                    lista_ingrediente);
                lista_preparate.Add(preparat);
            }



            return lista_preparate;
        }

    }
} // namespace