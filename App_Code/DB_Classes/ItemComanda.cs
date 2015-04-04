using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace db_mapping
{
    /// <summary>
    /// ItemComanda este wrapper peste clasa Preparat.
    /// Un obiect de tip ItemComanda contine un singur obiect Preparat,
    /// cantitatea in care acest preparat a fost comandat si pretul (total).
    /// Toate modificarile asupra cantitatii vor updata si pretul.
    /// </summary>
    public class ItemComanda
    {
        // Constructorul necesita doar un preparat. Cantitatea poate fi
        // omisa aici si setata apoi prin intermediul setCantitate.
        public ItemComanda(Preparat preparat, int cantitate = 0)
        {
            preparat_ = preparat;
            cantitate_ = cantitate;
            pret_ = preparat_.Pret * cantitate_;
        }

        // Seteaza o cantitate pentru obiectul ItemComanda
        // si updateaza pretul.
        public void setCantitate(int cantitate)
        {
            cantitate_ = cantitate;
            pret_ = preparat_.Pret * cantitate_;
        }

        // Returneaza intreg continutul unui obiect de tip ItemComanda
        // sub forma de String. Metoda folosita pentru testare.
        public String toString()
        {
            String item_comanda = "";
            String new_line = "\n";
            item_comanda += preparat_.toString();
            item_comanda += new_line;
            item_comanda += "Cantitate = " + cantitate_ + new_line;
            item_comanda += "Pret item comanda = " + pret_ + new_line;

            return item_comanda;
        }        

        // Getteri.
        public Preparat getPreparat()
        {
            return preparat_;
        }

        public int getCantitate()
        {
            return cantitate_;
        }

        public double getPret()
        {
            return pret_;
        }

        private Preparat preparat_;
        private int cantitate_;
        private double pret_;
    }
} // namespace