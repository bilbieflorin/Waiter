using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace db_mapping
{

    /// <summary>
    /// Clasa Preparat mapeaza tabelul "Preparate" din baza de date.
    /// </summary>
    public class Preparat
    {
        // Constante pentru campurile neinitializate din clasa.
        // e.g. Pentru a verifica daca id_ a fost initializat,
        // il comparam cu UninitializedInt.
        public static int UninitializedInt = -100000;
        public static double UninitializedDouble = -100000.0;

        public Preparat()
        {
            id_ = UninitializedInt;
            denumire_preparat_ = path_imagine_preparat_ = tip_preparat_ =
                denumire_specific_ = null;
            gramaj_ = pret_ = UninitializedDouble;
            data_adaugare_ = default(DateTime);
            lista_ingrediente_ = new List<String>();
        }

        // Initializeaza atributele unui obiect de tip Preparat.
        // Sunt necesare doar id, denumire_preparat, tip_preparat si pret
        // pentru a face o initializare. Restul parametrilor iau valori de
        // tip "neinitializat".
        // Parametrul cantitate ia valoarea UninitializedDouble.
        // Parametrul id_specific ia valoarea UninitializedInt.
        //
        // Exemplu utilizare:
        //
        // Preparat preparat_incomplet = new Preparat();
        // preparat_incomplet.Initialize(143, "Briose", "Desert", "4.5");
        //
        // Preparat preparat_complet = new Preparat();
        // DateTime data_adaugare = new DateTime(2014, 1, 18);
        // preparat_complet.Initialize(143, "Briose", "Desert", "4.5", "images/briose.jpg",
        // 6.0, 2, data_adaugare);
        public void Initialize(int id, String denumire_preparat, String tip_preparat,
            double pret, String path_imagine = null, double cantitate = -100000.0,
            String denumire_specific = null, List<String> lista_ingrediente = null,
            DateTime data_adaugare = default(DateTime))
        {
            id_ = id;
            denumire_preparat_ = denumire_preparat;
            tip_preparat_ = tip_preparat;
            pret_ = pret;
            path_imagine_preparat_ = path_imagine;
            gramaj_ = cantitate;
            denumire_specific_ = denumire_specific;
            data_adaugare_ = data_adaugare;
            lista_ingrediente_ = lista_ingrediente;
        }

        // Returneaza intreg continutul unui obiect de tip Preparat
        // sub forma de String. Metoda folosita pentru testare.
        // Pentru a accesa campuri din obiect, se folosesc getterii!
        public String toString()
        {
            String preparat = "";
            if (id_ != UninitializedInt)
            {
                //preparat += "Id = " + id_ + " ";
                preparat += "Denumire preparat = " + denumire_preparat_ + " ";
                //preparat += "Tip Preparat = " + tip_preparat_ + " ";
                //preparat += "Pret = " + pret_ + " ";
                //preparat += "Path Imagine Preparat = " + path_imagine_preparat_ + " ";
                //preparat += "Cantitate = " + gramaj_ + " ";
                //preparat += "Id Specific = " + denumire_specific_ + " ";
                //preparat += "Data Adaugare = " + data_adaugare_ + "\n";

                // ? TODO: Adauga lista de ingrediente.
            }
            return preparat;
        }

        public override bool Equals(Object p)
        {
            if (p == null)
                return false;
            Preparat prep = p as Preparat;
            if (this.id_ == prep.id_)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        // Proprietati read-only.
        public int Id
        {
            get { return id_; }
        }

        public String Denumire
        {
            get { return denumire_preparat_; }
        }

        public String PathImagine
        {
            get { return path_imagine_preparat_; }
        }

        public String Tip
        {
            get { return tip_preparat_; }
        }

        public double Gramaj
        {
            get { return gramaj_; }
        }

        public double Pret
        {
            get { return pret_; }
        }

        public String Specific
        {
            get { return denumire_specific_; }
        }

        public DateTime DataAdaugare
        {
            get { return data_adaugare_; }
        }

        public List<String> ListaIngrediente
        {
            get { return lista_ingrediente_; }
        }
        private int id_;
        private String denumire_preparat_;
        private String path_imagine_preparat_;
        private String tip_preparat_;
        private double gramaj_;
        private double pret_;
        private String denumire_specific_;
        private DateTime data_adaugare_;
        private List<String> lista_ingrediente_;

    }
} // namespace