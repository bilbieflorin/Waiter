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
            lista_item_comanda_ = new List<Comanda>();
        }

        public void addComanda(Comanda comanda)
        {
            lista_item_comanda_.Add(comanda);

        }

        // Getteri.
        public int IdUser
        {
            get { return id_user_; }
        }

        public List<Comanda> ListaItemComanda
        {
            get { return lista_item_comanda_; }
        }

        private List<Comanda> lista_item_comanda_;
        int id_user_;
    }

}