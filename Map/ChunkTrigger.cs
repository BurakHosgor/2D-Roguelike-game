using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mc;
    public GameObject targetMap;
    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            mc.currentChuck = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (mc.currentChuck == targetMap)
            {
                mc.currentChuck = null;
            }
        }
    }

}
