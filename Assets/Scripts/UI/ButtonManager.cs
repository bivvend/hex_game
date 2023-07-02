using JetBrains.Annotations;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public BoardController boardController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBuildButtonClicked()
    {
        boardController.BuildButtonClicked();

    }

    public void OnPlaceUnitButtonClicked()
    {

        boardController.PlaceUnitButtonClicked();
    }

    public void OnShuffleButtonClicked()
    {
        boardController.ShuffleButtonClicked();

    }

    public void OnEndTurnButtonClicked()
    {

         boardController.EndTurnButtonClicked();
    }

    public void OnStartGameButtonClicked()
    {
        boardController.SetupInitialPlayerLands();
        GameObject.Find("StartGameButton").SetActive(false);
    }


    
}
