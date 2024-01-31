using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTrigger : MonoBehaviour
{
    IDamage dmg;
    bool inRange; // player is in range

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.CompareTag("Player"))
        {
            dmg = other.GetComponent<IDamage>();
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // player has walked out of explode range, so they should not get damaged
        dmg = null;
        inRange = false;
    }

    public IDamage getDmg() { return dmg; }

    public bool getInRange() { return inRange; }
}