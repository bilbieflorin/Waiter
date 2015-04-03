using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace db_mapping
{

    /// <summary>
    /// Clasa Comanda mapeaza tabela "Comenzi" din baza de date.
    /// Contine, in plus, un HashSet de ItemComanda, iar pretul se updateaza
    /// la fiecare inserare de ItemComanda.
    /// </summary>
    public class Comanda
    {
        public Comanda()
        {
            lista_item_comanda_ = new HashSet<ItemComanda>();
            pret_ = 0;
        }

        // Returneaza intreg continutul unui obiect de tip Comanda
        // sub forma de String. Metoda folosita pentru testare.
        public String toString()
        {
            String comanda = "";
            String new_line = "\n";

            foreach (ItemComanda item_comanda in lista_item_comanda_)
            {
                comanda += item_comanda.toString();
                comanda += new_line;
                comanda += "Id utilizator = " + id_user_ + new_line;
                comanda += "Pret = " + pret_ + new_line;
                comanda += "Data = " + date_time_ + new_line;
            }

            return comanda;
        }

        // Adauga un ItemComanda la Comanda.
        // Pretul comenzii este updatat.
        public void addItemComanda(ItemComanda item_comanda)
        {
            lista_item_comanda_.Add(item_comanda);
            pret_ += item_comanda.getPret();
        }

        // Setteri.
        public void setIdUser(int id_user)
        {
            id_user_ = id_user;
        }

        public void setDateTime(DateTime date_time)
        {
            date_time_ = date_time;
        }

        // Getteri.
        public double getPret()
        {
            return pret_;
        }

        public DateTime getDateTime()
        {
            return date_time_;
        }

        public int getIdUser()
        {
            return id_user_;
        }

        private HashSet<ItemComanda> lista_item_comanda_;
        private double pret_;
        private DateTime date_time_;
        private int id_user_;
    }
} // namespace