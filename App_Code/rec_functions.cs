using System;
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
        public static List<Preparat> Gaseste_recomandari(int id_user, Comanda comanda)
        {
            // Gasim cei mai similari k vecini pentru userul cu id-ul user_id.
            int[] lista_vecini = Calculeaza_vecini(3, id_user);
            List<Preparat> recomandari = new List<Preparat>();

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
                if (entry.Value.Count > 0)
                {
                    signatures.Add(entry.Key, entry.Value.ToArray());
                }
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

            int[] vecini = new int[k + 1];
            for (int index = 0; index < k; index++)
            {
                vecini[index] = results.MaxBy(x => x.Value).Key;
                results.Remove(vecini[index]);
            }

            return vecini;

        }
    }
} // namespace