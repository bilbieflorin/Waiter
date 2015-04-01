using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

interface Preparate{};

/// <summary>
/// Functii de lucru cu baza de date
/// </summary>
public class DatabaseFunctions
{
    //intoarce lista de preparate din BD
    static public List<Preparate> getPreparate(string conString)
    {
        List<Preparate> l = null;
        using(SqlConnection con = new SqlConnection(conString))
        {
            SqlCommand cmd = new SqlCommand(@"select id_preparat, denumire_praparat, tip_preparat, path, cantitate, pret, denumire_specific
                                              from preparate join specific on (preparate.id_specific = specific.id_specific)",con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                //dupa cum poti vedea reader functioneaza ca un cursor
                //comanda asta intoarce true daca poate face fetch, deci face 2 lucruri: fetch + iti spune daca mai are date
                //si ca sa ai acces la un anumit camp trebuie sa apelezi metoda care are in nume tipul de date al coloanei din
                //tabel sau ceva compatibil(varchar~string) si numarul de ordine al coloanei (indexarea se face de la 0);
                
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string denumire = reader.GetString(1);
                    string tip = reader.GetString(2);
                    string path = reader.GetString(3);
                    float gramaj = reader.GetFloat(4);
                    float pret = reader.GetFloat(5);
                    string specific = reader.GetString(6);
                    cmd.CommandText = @"select denumire
                                        from ingrediente join contine on(ingrediente.id_ingredient = contine.id_ingredient)
                                        where id_preparat = "+id;
                    List<string> ing_list = new List<string>();
                    using (SqlDataReader read = cmd.ExecuteReader())
                    {
                        while(read.Read())
                        {
                            string den = reader.GetString(0);
                            ing_list.Add(den);
                        }
                    }
                    //urmeaza sa crezi tu aici obiectul de tip preparat si sa ii dai valori la campuri
                }
            }
        }
        return l;
    }

}