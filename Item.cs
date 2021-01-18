using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public HeartsHealthSystem heartsHealthSystem;
    public int amount;
    public ItemType itemType;
    public enum ItemType
    {
        Heal,
        Energy
    }
    
    public void PickUp ()
    {
        StartCoroutine(DestroyItem());
    }

    public ItemType GetType ()
    {
        return itemType;
    }

    IEnumerator DestroyItem ()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
