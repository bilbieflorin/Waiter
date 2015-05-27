using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MinHash
/// </summary>
public class MinHash<T> {

	private int[] hash;
	private int numHash;
	
	public MinHash(int universe){
		numHash = universe;
		hash = new int[numHash];
		Random r = new Random(121);
	    for (int i = 0; i < numHash; i++)
        {
            int a = (int)r.Next(universe);
            int b = (int)r.Next(universe);
            int c = (int)r.Next(universe);
	        int x = Hash(a*b*c, a, b, c);
	        hash[i] = x;
	     } 		    
    }
	
	private static int Hash(int x, int a, int b, int c) {
        int hashValue = (int)((a * (x >> 4) + b * x + c) & 131071);
        return Math.Abs(hashValue);
    }
	
	
	public double similarity(HashSet<T> set1, HashSet<T> set2){ 
		
        HashSet<T> hashSet = new HashSet<T>();
        hashSet.UnionWith(set1);
        hashSet.UnionWith(set2);
        int[][] minHashValues = genBitMatrix(hashSet, set1, set2);        
        return computeSimilarityFromSignatures(minHashValues, hashSet.Count(), 0, 1);
    }
	
    
    private int[][] genBitMatrix(HashSet<T> hashSet, HashSet<T> set1, HashSet<T> set2){
    	int [][] bitMatrix = new int[2][];
        bitMatrix[0] = new int[hashSet.Count()];
        bitMatrix[1] = new int[hashSet.Count()];
    	
    	int index = 0;
    	foreach (T element in hashSet){
    		if(set1.Contains(element)){
    			bitMatrix[0][index] = 1;
    		}
    		if(set2.Contains(element)){
    			bitMatrix[1][index] = 1;
    		}
    		index++;
    	}    	
    	return bitMatrix;
    }
    
    
	
	public int[][] initializeHashBuckets(int numSets, int numHashFunctions) {
		int[][] minHashValues = new int[numSets][];
        for (int i = 0; i < numSets; i++ )
        {
            minHashValues[i] = new int[numHashFunctions];
        }
            for (int i = 0; i < numSets; i++)
            {
                for (int j = 0; j < numHashFunctions; j++)
                {
                    minHashValues[i][j] = Int32.MaxValue;
                }
            }
        return minHashValues;
    }
	
	
	private double computeSimilarityFromSignatures(int[][] minHashValues, int numHashFunctions, int setIndex, int canditateIndex) {
		int identicalMinHashes = 0;
        for (int i = 0; i < numHashFunctions; i++){
            if (minHashValues[setIndex][i] == minHashValues[canditateIndex][i]) {
                identicalMinHashes++;
            }
        }
        return (1.0 * identicalMinHashes) / numHashFunctions;
    }
	
	
	public void computeMinHashForSet(HashSet<T> set, int setIndex, int[][] minHashValues, HashSet<T> bitArray){    	
    	 int index = 0;
         foreach (T element in bitArray){        	 
        	 if(set.Contains(element)){   
        		 for (int i = 0; i < numHash; i++) {  
        			 int hindex = hash[index];
                     if (hindex < minHashValues[setIndex][index]){
                         minHashValues[setIndex][i] = hindex;
                     }
                 }
             }
             index++;
         }   	
    }
	
	public double ComputeSimilarity(int[][] m_minHashMatrix, int setIndex, int candidateIndex) {
		int length = m_minHashMatrix.Count();		
		return computeSimilarityFromSignatures(m_minHashMatrix, length, setIndex, candidateIndex);        
	}
}