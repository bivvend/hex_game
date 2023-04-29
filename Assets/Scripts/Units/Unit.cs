using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector]
    public int numberOfTroops;
    [HideInInspector]
    public bool isHero;
    [HideInInspector]
    public OwnerType ownerType;

    public GameObject spritePrefab;

    public abstract void SetNumberOfTroops(int number);


}
