using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace db_mapping
{
    /// <summary>
    /// IstoricComenzi mapeaza istoricul de comenzi din BD ale unui utilizator.
    /// </summary>
    public class IstoricComenzi
    {
        public static int UninitializedInt = -100000;

        public IstoricComenzi(int user_id = -100000)
        {
            id_user_ = user_id;
            lista_comenzi_ = new List<Comanda>();
        }

        public void addComanda(Comanda comanda)
        {
            lista_comenzi_.Add(comanda);

        }

        public bool continePreparat(Preparat preparat)
        {
            foreach (var comanda in ListaComenzi)
            {
                if (comanda.ListaItem[preparat.Id] != null)
                {
                    return true;
                }
            }
            return false;
        }

        // Getteri.
        public int IdUser
        {
            get { return id_user_; }
        }

        public List<Comanda> ListaComenzi
        {
            get { return lista_comenzi_; }
        }

        private List<Comanda> lista_comenzi_;
        int id_user_;
    }

}