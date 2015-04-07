using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace db_mapping
{

    /// <summary>
    /// Clasa Comanda mapeaza tabela "Comenzi" din baza de date.
    /// Contine, in plus, un Hashtable de ItemComanda, iar pretul se updateaza
    /// la fiecare inserare de ItemComanda.
    /// </summary>
    public class Comanda
    {
        public Comanda()
        {
            lista_item_comanda_ = new Hashtable();
            pret_ = 0;
            numar_preparate_ = 0;
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
            pret_ += item_comanda.Pret;
            numar_preparate_ += item_comanda.Cantitate;
            if (lista_item_comanda_.ContainsKey(item_comanda.Preparat.Id))
            {
                ItemComanda item = lista_item_comanda_[item_comanda.Preparat.Id] as ItemComanda;
                int cantitate = item_comanda.Cantitate + item.Cantitate;
                item_comanda.Cantitate = cantitate;
                lista_item_comanda_[item.Preparat.Id] = item_comanda;
            }
            else
            {
                lista_item_comanda_.Add(item_comanda.Preparat.Id, item_comanda);
            }
        }

        public void removeItemComanda(ItemComanda item)
        {
            lista_item_comanda_.Remove(item.Preparat.Id);
            pret_ -= item.Pret;
            numar_preparate_ -= item.Cantitate;
        }

        // Setteri si getteri.
        public int IdUser
        {
            get { return id_user_; }
            set { id_user_ = value; }
        }

        public DateTime Data
        {
            get { return date_time_; }
            set { date_time_ = value; }
        }

        public double Pret
        {
            get { return pret_; }
        }

        public Hashtable ListaItem
        {
            get { return lista_item_comanda_; }
        }

        public int NumarPreparate
        {
            get { return numar_preparate_; }
            set { numar_preparate_ = value; }
        }

        private Hashtable lista_item_comanda_;
        private double pret_;
        private DateTime date_time_;
        private int numar_preparate_;
        private int id_user_;

    }
} // namespace