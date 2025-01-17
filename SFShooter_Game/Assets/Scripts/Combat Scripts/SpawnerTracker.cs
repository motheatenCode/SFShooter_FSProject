using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTracker : MonoBehaviour
{
    [Header("--- Spawner Tracker Components ---")]
    [SerializeField] Transform trackerTransform;
    [SerializeField] GameObject container; // container that holds the scrap
    [SerializeField] GameObject scrap;

    int enemyCount;
    bool countAssigned;
    bool scrapCollected;
    Container containerScript;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        countAssigned = false;
        containerScript = container.GetComponent<Container>();
        scrap.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (countAssigned)
        {
            enemyCount = trackerTransform.childCount;
            if(enemyCount <= 0)
            {
                containerScript.openContainer();
                if(scrap != null)
                {
                    scrap.SetActive(true);
                }
                
            }

            if(scrap == null)
            {
                scrapCollected = true;
            }
        }
    }

    public void updateEnemyCount(int amount)
    {
        if (!countAssigned)
        {
            countAssigned = true;
            enemyCount = amount;
        }
    }

    protected Transform getTrackerTransform()
    {
        return trackerTransform;
    }

    public bool getScrapCollected()
    {
        return scrapCollected;
    }
}
