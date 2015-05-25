//*********************************************************************************************************
// © 2014 jakemdrew.com. All rights reserved. 
// This source code is licensed under The GNU General Public License (GPLv3):  
// http://opensource.org/licenses/gpl-3.0.html
//*********************************************************************************************************

//*********************************************************************************************************
//MinHasher - Example minhashing engine.
//Created By - Jake Drew, www.jakemdrew.com 
//Version -    1.0, 05/08/2013
//*********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rec_system
{
    class MinHasher
    {
        public int signatureSize;
        public Tuple<int, int>[] minhashes;

        public MinHasher(int SignatureSize)
        {
            signatureSize = SignatureSize;
            minhashes = new Tuple<int, int>[SignatureSize];
            
            //Create our unique family of hashing function seeds.
            createMinhashSeeds();
        }

        private void createMinhashSeeds()
        {
            HashSet<int> skipDups = new HashSet<int>();
            Random r = new Random();
            for (int i = 0; i < minhashes.Length; i++)
            {
                Tuple<int, int> seed = new Tuple<int, int>(r.Next(), r.Next());

                if (skipDups.Add(seed.GetHashCode()))
                    minhashes[i] = seed;
                else
                    i--;  //duplicate seed, try again 
            }
        }

        public static int LSHHash<T>(T inputData, int seedOne, int seedTwo)
        {   //Faster, Does not throw exception for overflows during hashing.
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ seedOne.GetHashCode();
                hash = hash * 16777619 ^ seedTwo.GetHashCode();
                hash = hash * 16777619 ^ inputData.GetHashCode();
                return hash;
            }
        }

        public static double calculateJaccard(int[] setA, int[] setB)
        {
            int intersection = setA.Intersect(setB).Count();
            return intersection / (double)setA.Length;
        }

        public int[] getMinHashSignature(int[] tokens)
        {
            //Create a new signature initialized to all int max values
            int[] minHashValues = Enumerable.Repeat(int.MaxValue, signatureSize).ToArray();
            
            HashSet<int> skipDups = new HashSet<int>();
            //Go through every single token 
            foreach (var token in tokens)
            {   //We do not want to hash the same token value more than once...
                if (skipDups.Add(token))
                {   //Hash each unique token with each unique hashing function
                    for (int i = 0; i < signatureSize; i++)
                    {   //Use the same seeds everytime for each hashing function (this is very important!!!)
                        Tuple<int,int> seeds =  minhashes[i];
                        int currentHashValue = LSHHash(token, seeds.Item1, seeds.Item2);
                        //Only retain the minimum value produced by each unique hashing function.
                        if (currentHashValue < minHashValues[i])
                            minHashValues[i] = currentHashValue;
                    }
                }
            }
            return minHashValues;
        }

        public Dictionary<int, int[]> createMinhashCollection(Dictionary<int, int[]> documents)
        {
            Dictionary<int, int[]> minhashCollection = new Dictionary<int, int[]>(documents.Count);

            foreach (var document in documents)
            {
                int[] minhashSignature = getMinHashSignature(document.Value);
                minhashCollection.Add(document.Key, minhashSignature);
            }
            return minhashCollection;
        }
    }
}
