using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;


    void OnDestroy()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List <Drops> possibleDrop = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrop.Add(rate);
            }
            
        }
        if (possibleDrop.Count > 0)
        {
            Drops drops = possibleDrop[UnityEngine.Random.Range(0, possibleDrop.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
