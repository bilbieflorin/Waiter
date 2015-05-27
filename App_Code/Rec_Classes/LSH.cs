using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LSH
/// </summary>
public class LSH<T>
{

    int m_numBands = 6;
    int PROCENT = 5;
    Dictionary<Int64, HashSet<int>> m_lshBuckets = new Dictionary<Int64, HashSet<int>>();
    int[][] minHashValues;
    int ROWSINBAND = 3;
    List<HashSet<T>> l_sets;
    private Dictionary<int, HashSet<int>> signatures;


    public LSH(int[][] m_minHashValues, List<HashSet<T>> sets)
    {
        l_sets = sets;
        minHashValues = m_minHashValues;
        for (int s = 0; s < sets.Count(); s++)
        {
            for (int b = 0; b < m_numBands; b++)
            {
                Int64 sum = 0;
                for (int i = 0; i < ROWSINBAND; i++)
                {
                    sum += minHashValues[s][b * ROWSINBAND + i];
                }
                sum = sum % PROCENT;
                if (m_lshBuckets.ContainsKey(sum))
                {
                    m_lshBuckets[sum].Add(s);
                }
                else
                {
                    HashSet<int> set = new HashSet<int>();
                    set.Add(s);
                    m_lshBuckets.Add(sum, set);
                }
            }
        }
    }

    public LSH(int[][] minHashValues, Dictionary<int, HashSet<int>> signatures)
    {
        this.minHashValues = minHashValues;
        this.signatures = signatures;
    }

    private HashSet<int> getCloseMembers(int setIndex, MinHash<T> minHasher)
    {
        HashSet<int> closeMembers = new HashSet<int>();
        for (int b = 0; b < m_numBands; b++)
        {
            int sum = 0;
            for (int i = 0; i < ROWSINBAND; i++)
            {
                sum += minHashValues[setIndex][b * ROWSINBAND + i];
                
            }
            sum = sum % PROCENT;
            foreach (int intInx in m_lshBuckets[sum])
            {
                closeMembers.Add(intInx);
            }
        }
        return closeMembers;
    }


    public Dictionary<int, double> closestSimilarItems(int setIndex, MinHash<T> minHasher)
    {
        Dictionary<int, double> closestMap = new Dictionary<int, double>();

        HashSet<int> closeMembers = getCloseMembers(setIndex, minHasher);
        foreach (int intIndex in closeMembers)
        {
            if (intIndex != setIndex)
            {
                double similarity = minHasher.ComputeSimilarity(minHashValues, setIndex, intIndex);
                closestMap.Add(intIndex, similarity);
            }
        }
        return closestMap;
    }
}