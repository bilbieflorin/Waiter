using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Web;
using db_mapping;

/// <summary>
/// Summary description for rec_functions
/// </summary>
///

namespace rec_system
{
    public class RecFunctions
    {
        public List<Preparat> Gaseste_recomandari(int id_user, Comanda comanda)
        {
            // Gasim cei mai similari k vecini pentru userul cu id-ul user_id.
            int[] lista_vecini = Calculeaza_vecini(3, id_user);
            List<Preparat> recomandari = new List<Preparat>();

            return recomandari;
        }

        public int[] Calculeaza_vecini(int k, int id_user)
        {
            int[] vecini = new int[k + 1];
            Dictionary<int, int[]> signatures = FlorinMagicDinBD();

            // Extragem vectorul specific clientului cu id-ul id_user.
            int[] query;
            signatures.TryGetValue(id_user, out query);
            signatures.Remove(id_user);

            //Now create a MinHasher object to minhash each of the documents created above
            //using 300 unique hashing functions.
            MinHasher minHasher = new MinHasher(400);
            Dictionary<int, int[]> docMinhashes = minHasher.createMinhashCollection(
                numDocCreator.documentCollection);
            //Create the test doc minhash signature
            int[] queryMinhashSignature = minHasher.getMinHashSignature(query);

            Dictionary<int, double> results = new Dictionary<int,double>();
            // double minhashCount = queryMinhashSignature.Length;

            //Compare the test document minhash signature to all minhash signatures  
            //in our document collection using Jaccard similarity 
            Console.WriteLine("Jaccard Similarity for each Minhashed collection:");
            foreach (KeyValuePair<int, int[]> signatures_entry in signatures)
            {
               // sw.Restart();
                double jaccard = MinHasher.calculateJaccard(queryMinhashSignature,
                    signatures_entry.Value);
                results.Add(signatures_entry.Key, jaccard);
              //  sw.Stop();
                Console.WriteLine("Document " + signatures_entry.Key.ToString() + ": " +
                    jaccard.ToString() + "     Time (Ticks = 10k ms per tick):" + 
                    sw.ElapsedTicks);
              //  minhashCount += document.Value.Length;
            }

            for (int index = 0; index < k; index++)
            {
                vecini[index] = results.MinBy(x => x.Value).Key;
                results.Remove(vecini[index]);
            }


        }
    }
} // namespace