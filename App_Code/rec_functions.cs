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
        public static List<Preparat> Recomandari(int id_user, Comanda comanda_actuala, int k)
        {
            IstoricComenzi istoric = DatabaseFunctions.getIstoric(id_user);
            List<Comanda> istoric_comenzi = istoric.ListaComenzi;
            List<Preparat> recomandari=null;

            if (istoric_comenzi.Count > 0)
            {
                recomandari = Gaseste_recomandari_ContentBased(id_user, k/2, istoric_comenzi);
                List<Preparat> recomandari_collective = Gaseste_recomandari_Collective(id_user, comanda_actuala, k);
                foreach (var preparat in recomandari_collective)
                {
                    if (recomandari.Count == k)
                    {
                        break;
                    }
                    if (!recomandari.Contains(preparat))
                    {
                        recomandari.Add(preparat);
                    }
                }
            }
            else
            {
                List<String> lista_specifice = DatabaseFunctions.getSpecificsForUser(id_user);
                if (lista_specifice == null)
                    recomandari = DatabaseFunctions.topKPreparate(k);
                else
                    recomandari = DatabaseFunctions.topKPreparateSpecific(lista_specifice, k);
            }
            return recomandari;
        }

        // Recomandari calculate prin Content Based Filtering.
        public static List<Preparat> Gaseste_recomandari_ContentBased(int id_user, int k, List<Comanda> istoric_comenzi)
        {
            Dictionary<String, int> dictionar_tipuri = new Dictionary<String,int>();
            Dictionary<String, int> dictionar_specificuri = new Dictionary<String,int>();

            // Calculam pretul mediu al unei comenzi, tipul si specificul preferate.
            double pret_mediu = 0;
            int nr_preparate = 0;
            String specific = "", tip = "";
            Hashtable lista_item_comanda;
            Preparat preparat;

            foreach(Comanda comanda in istoric_comenzi)
            {
                pret_mediu += comanda.Pret;
                nr_preparate += comanda.NumarPreparate;

                lista_item_comanda = comanda.ListaItem;
                foreach(DictionaryEntry item in lista_item_comanda)
                {
                    ItemComanda item_comanda = item.Value as ItemComanda;
                    preparat = item_comanda.Preparat;
                    specific = preparat.Specific;
                    if(dictionar_specificuri.ContainsKey(specific))
                    {
                        dictionar_specificuri[specific]++;
                        //.TryGetValue(specific, out value);
                        //dictionar_specificuri.Add(specific, value+1);
                    }
                    else
                    {
                        dictionar_specificuri.Add(specific, 1);
                    }

                    tip = preparat.Tip;
                    if (dictionar_tipuri.ContainsKey(tip))
                    {
                        dictionar_tipuri[tip]++;
                        //    .TryGetValue(tip, out value);
                        //dictionar_tipuri.Add(tip, value + 1);
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

            recomandari = preparateDupaParametri(id_user,specific,tip, pret_mediu, k);
            return recomandari;
        }

        // Recomandari calculate prin Collective Filtering.
        public static List<Preparat> Gaseste_recomandari_Collective(int id_user, Comanda comanda, int k)
        {
            // Gasim cei mai similari k vecini pentru userul cu id-ul user_id.
            int[] lista_vecini = Calculeaza_vecini(3, id_user);
            List<IstoricComenzi> lista_istorice = DatabaseFunctions.istoricUtilizatori(lista_vecini);
            IstoricComenzi istoric_user = DatabaseFunctions.getIstoric(id_user);
            // Eliminare preparate comandate.
            // Eliminare cele a caror ora nu apartine intervalului curent.
            comanda = (comanda == null)? new Comanda(): comanda;
            List<Preparat> preparate = eliminaComandate(lista_istorice, istoric_user, comanda);
            List<Preparat> recomandari = preparate.GetRange(0,k) as List<Preparat>;
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

        private static List<Preparat> eliminaComandate(List<IstoricComenzi> istorice, IstoricComenzi istoric_user, Comanda comanda)
        {
            HashSet<Preparat> preparate = new HashSet<Preparat>();
            int avg = DatabaseFunctions.numarMediuComandariPreparat(istoric_user.IdUser);
            foreach (var istoric in istorice)
            {
                foreach (var com in istoric.ListaComenzi)
                {
                    foreach (DictionaryEntry item in com.ListaItem)
                    {
                        ItemComanda item_comanda = item.Value as ItemComanda;
                        int nr = DatabaseFunctions.numarComandariPreparat(istoric_user.IdUser, item_comanda.Preparat.Id);
                        if (!comanda.ListaItem.ContainsKey(item_comanda.Preparat.Id) && avg >= nr)
                        {
                            preparate.Add(item_comanda.Preparat);
                        }
                    }
                }
            }
            return preparate.ToList();
        }

        private static List<Preparat> preparateDupaParametri(int id_user,String specific,String tip, double pret_mediu, int k)
        {
            List<Preparat> lista_preparate = new List<Preparat>();
            lista_preparate = DatabaseFunctions.preparateDupaPret(pret_mediu);
            List<Preparat> preparate = intersectie(id_user,specific,tip,lista_preparate);
            if (preparate.Count < k)
            {
                foreach (var preparat in preparate)
                {
                    lista_preparate.Remove(preparat);
                }
                int avg = DatabaseFunctions.numarMediuComandariPreparat(id_user);
                foreach (var preparat in lista_preparate)
                {
                    int nr = DatabaseFunctions.numarComandariPreparat(id_user, preparat.Id);
                    if (preparat.Tip.Equals(tip) && nr < avg)
                    {
                         preparate.Add(preparat);
                    }
                    if (preparate.Count == k)
                        break;
                }
                foreach (var preparat in lista_preparate)
                {
                    if (preparate.Count == k)
                        break;
                    int nr = DatabaseFunctions.numarComandariPreparat(id_user, preparat.Id);
                    if (preparat.Specific.Equals(specific) && nr < avg)
                    {
                        preparate.Add(preparat);
                    }
                   
                }
            }
            return preparate;
        }

        private static List<Preparat> intersectie(int id_user,String specific, String tip, List<Preparat> lista_preparate)
        {
            List<Preparat> preparate = new List<Preparat>();
            int avg = DatabaseFunctions.numarMediuComandariPreparat(id_user);
            foreach(var preparat in lista_preparate)
            {
                int nr = DatabaseFunctions.numarComandariPreparat(id_user, preparat.Id);
                if (preparat.Tip.Equals(tip) && preparat.Specific.Equals(specific)&& nr < avg)
                {
                    preparate.Add(preparat);
                }
            }
            return preparate;
        }
    }
} // namespace