using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Web;
using db_mapping;

/// <summary>
/// Clasa de functii pentru sistemul de recomandari.
/// </summary>
///

namespace rec_system
{
    public class RecFunctions
    {
        // Functia principala de calcul a Recomandarilor.
        public List<Preparat> Recomandari(int id_user, Comanda comanda_actuala, int k)
        {
            IstoricComenzi istoric = DatabaseFunctions.getIstoric(id_user);
            List<Comanda> istoric_comenzi = istoric.ListaComenzi;
            List<Preparat> recomandari;

            if (istoric_comenzi.Count > 0)
            {
                recomandari = Gaseste_recomandari_ContentBased(id_user, k, istoric_comenzi);
                recomandari.AddRange(Gaseste_recomandari_Collective(id_user, comanda_actuala,
                    k));
            }
            else
            {   // Top din DB sau Top dupa specificul ales din formularul de inregistrare.
                recomandari = Gaseste_recomandari_Top(id_user, k);
            }

            return recomandari;
        }

        // Recomandari calculate prin Content Based Filtering.
        public List<Preparat> Gaseste_recomandari_ContentBased(int id_user, int k,
            List<Comanda> istoric_comenzi)
        {
            Dictionary<String, int> dictionar_tipuri = new Dictionary<String,int>();
            Dictionary<String, int> dictionar_specificuri = new Dictionary<String,int>();

            // Calculam pretul mediu al unei comenzi, tipul si specificul preferate.
            double pret_mediu = 0;
            int nr_preparate = 0, value = 0;
            String specific = "", tip = "";
            Hashtable lista_item_comanda;
            Preparat preparat;

            foreach(Comanda comanda in istoric_comenzi)
            {
                pret_mediu += comanda.Pret;
                nr_preparate += comanda.NumarPreparate;

                lista_item_comanda = comanda.ListaItem;
                foreach(ItemComanda item in lista_item_comanda)
                {
                    preparat = item.Preparat;
                    specific = preparat.Specific;
                    if(dictionar_specificuri.ContainsKey(specific))
                    {
                        dictionar_specificuri.TryGetValue(specific, out value);
                        dictionar_specificuri.Add(specific, value+1);
                    }
                    else
                    {
                        dictionar_specificuri.Add(specific, 1);
                    }

                    tip = preparat.Tip;
                    if (dictionar_tipuri.ContainsKey(tip))
                    {
                        dictionar_tipuri.TryGetValue(tip, out value);
                        dictionar_tipuri.Add(tip, value + 1);
                    }
                    else
                    {
                        dictionar_tipuri.Add(tip, 1);
                    }
                }
                
            }

            specific = dictionar_specificuri.MaxBy(x => x.Value).Key;
            tip = dictionar_tipuri.MaxBy(x => x.Value).Key;
            List<Preparat> recomandari = new List<Preparat>();
            if (nr_preparate > 0)
            {
                pret_mediu = pret_mediu / nr_preparate;
            }

            //recomandari = PreparateDupaParametri(specific, tip, pret_mediu,2*k);
            return recomandari;
        }

        // Recomandari calculate prin Collective Filtering.
        public static List<Preparat> Gaseste_recomandari_Collective(int id_user, 
            Comanda comanda, int k)
        {
            // Gasim cei mai similari k vecini pentru userul cu id-ul user_id.
            int[] lista_vecini = Calculeaza_vecini(3, id_user);
            List<IstoricComenzi> lista_istorice = DatabaseFunctions.istoricUtilizatori(lista_vecini);
            IstoricComenzi istoric_user = DatabaseFunctions.getIstoric(id_user);
            // Eliminare preparate comandate.
            // Eliminare cele a caror ora nu apartine intervalului curent.
            List<Preparat> preparate = eliminaComandate(lista_istorice, istoric_user);
            List<Preparat> recomandari = preparate.GetRange(0, 2*k) as List<Preparat>;
            return recomandari;
        }


        public static int[] Calculeaza_vecini(int k, int id_user)
        {
            // Extragem din DB un Dictionary de toate id utilizator, lista de preparate
            // comandate.
            Dictionary<int, List<int>> toatePrep = DatabaseFunctions.
                preparateComandateDupaUtilizator();
            Dictionary<int, int[]> signatures = new Dictionary<int, int[]>();

            foreach (KeyValuePair<int, List<int>> entry in toatePrep)
            {
                signatures.Add(entry.Key, entry.Value.ToArray());
            }

            // Extragem vectorul specific clientului cu id-ul id_user.
            int[] query;
            signatures.TryGetValue(id_user, out query);
            signatures.Remove(id_user);

            // Cream un obiect MinHasher cu 400 functii hash.
            // Si calculam signaturile min Hash celorlalti utilizatori.
            MinHasher minHasher = new MinHasher(400);
            Dictionary<int, int[]> signaturesMinHashes = minHasher.createMinhashCollection(
                signatures);
            // Calculam signatura minHash a query-ului.
            int[] queryMinhashSignature = minHasher.getMinHashSignature(query);

            Dictionary<int, double> results = new Dictionary<int, double>();
            // Calculam similaritatea Jaccard folosind signatura minHash a tuturor 
            // utilizatorilor.
            foreach (KeyValuePair<int, int[]> signatures_entry in signaturesMinHashes)
            {
                double jaccard = MinHasher.calculateJaccard(queryMinhashSignature,
                    signatures_entry.Value);
                results.Add(signatures_entry.Key, jaccard);
            }

            int[] vecini = new int[k];
            for (int index = 0; index < k; index++)
            {
                vecini[index] = results.MaxBy(x => x.Value).Key;
                results.Remove(vecini[index]);
            }

            return vecini;

        }

        private static List<Preparat> eliminaComandate(List<IstoricComenzi> istorice, IstoricComenzi istoric_user)
        {
            HashSet<Preparat> preparate = new HashSet<Preparat>();

            foreach (var istoric in istorice)
            {
                foreach (var comanda in istoric.ListaComenzi)
                {
                    foreach (DictionaryEntry item in comanda.ListaItem)
                    {
                        ItemComanda item_comanda = item.Value as ItemComanda;
                        if (!istoric_user.continePreparat(item_comanda.Preparat))
                        {
                            preparate.Add(item_comanda.Preparat);
                        }
                    }
                }
            }
            return preparate.ToList();
        }
    }
} // namespace