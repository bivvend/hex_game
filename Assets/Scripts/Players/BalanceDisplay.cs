using Players;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceDisplay : MonoBehaviour
{
    public Player player;
    public ResourceType resourceType;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var val = player.GetBalance(resourceType).balance.ToString();
        gameObject.GetComponent<TextMeshProUGUI>().text = val;
    }
}
