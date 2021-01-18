using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance {get; private set;}

    public GameAssets ()
    {
        Instance = this;
    }

    public Transform pfPopup;
}
