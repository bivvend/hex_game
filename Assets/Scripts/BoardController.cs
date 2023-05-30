using System.Collections.Generic;
using UnityEngine;
using Scripts.Tiles;
using System;
using System.Linq;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Drawing;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Scripts.Cards;
using Players;
using TMPro;
using static Scripts.GameStateEnums;
using UnityEngine.U2D;

namespace Scripts
{
    public class BoardController : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> tiles = new List<GameObject>();

        public List<DevelopmentCard> developmentCards = new List<DevelopmentCard>();

        private DevelopmentCard _topCard =>  developmentCards[0];

        public List<UnitCard> unitCards = new List<UnitCard>();


        private GameObject _topCardTile;

        public GameObject baseTilePrefab;
        public GameObject edgePrefab;
        public GameObject unitsSpritePrefab;
        public GameObject heroSpritePrefab;
        public GameObject developmentPrefab;
        public int mapRadius;

        [HideInInspector]
        public GameState GameState;

        public GameObject GoodPlayer;
        public GameObject EvilPlayer;

        private GameObject _currentPlayer => GameState.playerActive == PlayerActive.Good ? GoodPlayer: EvilPlayer;

        private bool _afterFirstFrame = false;

        // Start is called before the first frame update
        void Start()
        {
            SetupGameState();

            Array values = Enum.GetValues(typeof(TerrainType));
            System.Random random = new System.Random();

            //Create a circular map of given radius
            for (int q = (-1 * (mapRadius + 2)); q <= (mapRadius + 2); q++)
            {
                for (int r = (-1 * (mapRadius + 2)); r <= (mapRadius + 2); r++)
                {
                    for (int s = (-1 * (mapRadius + 2)); s <= (mapRadius + 2); s++)
                    {
                        if (s == -q - r)
                        {
                            GameObject tile = new GameObject("HexTile");
                            tile.AddComponent<HexTile>();
                            HexTile t = tile.GetComponent<HexTile>();
                            t.tileSpritePrefab = baseTilePrefab;
                            t.edgeSpritePrefab = edgePrefab;
                            t.developmentPrefab = developmentPrefab;
                            t.boardController = this;

                            TerrainType randomType = (TerrainType)values.GetValue(random.Next(values.Length));
                            if (randomType == TerrainType.Water)
                            {
                                randomType = TerrainType.Grass;
                            }

                            if (Math.Abs(q) == mapRadius + 2 || Math.Abs(r) == mapRadius + 2 || Math.Abs(s) == mapRadius + 2)
                            {
                                randomType = TerrainType.Water;
                            }
                            if (Math.Abs(q) == mapRadius + 1 || Math.Abs(r) == mapRadius + 1 || Math.Abs(s) == mapRadius + 1)
                            {
                                //Make a rough coast
                                if (random.Next(0, 2) > 0)
                                {
                                    randomType = TerrainType.Water;
                                }
                            }

                            t.TerrainType = randomType;
                            t.qIndex = q;
                            t.rIndex = r;
                            t.sIndex = s;
                            t.owner = OwnerType.Neutral;

                            if (r == 0 && q == mapRadius && s == -1 * mapRadius)
                            {
                                //t.ChangeOwnerShip(OwnerType.Good);
                                t.TerrainType = TerrainType.Grass;
                                //t.AddDevelopment(new List<UtilityType>() { UtilityType.Capital }, OwnerType.Good);
                            }
                            if (r == 0 && q == -1 * mapRadius && s == mapRadius)
                            {

                                //t.ChangeOwnerShip(OwnerType.Evil);
                                t.TerrainType = TerrainType.Grass;
                                //t.AddDevelopment(new List<UtilityType>() { UtilityType.Capital }, OwnerType.Evil);
                            }

                            tiles.Add(tile);
                        }
                    }

                }
            }

            DeselectCardDisplay();
            ChangeTopCardDisplay();

        }

        // Update is called once per frame
        void Update()
        {
            if(!_afterFirstFrame)
            {
                _afterFirstFrame = true;
            }
        }

        public void SetupInitialPlayerLands()
        {
            int r = 0;
            int q = 0;
            int s = 0;
            tiles.ForEach((tile) =>
            {
                //Place capitals
                HexTile t = tile.GetComponent<HexTile>();
                r = t.rIndex;
                q = t.qIndex;
                s = t.sIndex;

                if (r == 0 && q == mapRadius && s == -1 * mapRadius)
                {
                    t.ChangeOwnerShip(OwnerType.Good);
                    t.TerrainType = TerrainType.Grass;
                    t.AddDevelopment(new List<UtilityType>() { UtilityType.Capital }, OwnerType.Good);
                }
                if (r == 0 && q == -1 * mapRadius && s == mapRadius)
                {

                    t.ChangeOwnerShip(OwnerType.Evil);
                    t.TerrainType = TerrainType.Grass;
                    t.AddDevelopment( new List<UtilityType>() { UtilityType.Capital }, OwnerType.Evil);
                }


            });
            
        }


        private void AddIncomes()
        {
            //Find all tiles of the current player
            List<HexTileLite> matchedTiles = new();
            List<HexTileLite> buildableTiles = new();
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();

            OwnerType ownerType = _currentPlayer.GetComponent<Player>().ownerType;

            List<(object, object)> positiveConditions = new();
            List<(object, object)> negativeConditions = new();
            positiveConditions.Add((ownerType, ownerType));
            negativeConditions.Add((TerrainType.Water, TerrainType.Water));
            //Get all tiles of that owner
            matchedTiles = TIleUtilities.FilterTilesByListOfGenericConditions(liteList, positiveConditions, negativeConditions);

            List<Cost> incomes = new();

            matchedTiles.ForEach((t) =>
            {
                t.Developments.ForEach((d) =>
                {
                    incomes.AddRange(GameScaling.developmentBaseIncomeList[d]);
                });

            });

            incomes.ForEach((i) =>
            {
                _currentPlayer.GetComponent<Player>().AddIncomeToBalance(i);

            });
            


        }

        

        void ChangeTopCardDisplay(bool withRemoval = true)
        {
            string category = "";
            string spriteName = "";

            List<Cost> newCosts = new List<Cost>();

            if (developmentCards.Count > 1) {
                if (withRemoval)
                {
                    developmentCards.RemoveAt(0);
                }
                var card = developmentCards[0];
                string currentOwner = GameState.playerActive == GameStateEnums.PlayerActive.Good ? "Good" : "Evil";
                switch (card.developmentType)
                {
                    case UtilityType.Mine:
                        category = "Mine";
                        spriteName = "Mine" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.Mine];
                        break;
                    case UtilityType.Town:
                        category = "Town";
                        spriteName = "Town" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.Town];
                        break;
                    case UtilityType.QuestSite:
                        category = "QuestSite";
                        spriteName = "QuestSite" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.QuestSite];
                        break;
                    case UtilityType.Farm:
                        category = "Farm";
                        spriteName = "Farm" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.Farm];
                        break;
                    case UtilityType.SorcerersTower:
                        category = "SorcerersTower";
                        spriteName = "SorcerersTower" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.SorcerersTower];
                        break;
                    case UtilityType.Fort:
                        category = "Fort";
                        spriteName = "Fort" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.Fort];
                        break;
                    case UtilityType.Capital:
                        category = "Capital";
                        spriteName = "Capital" + currentOwner;
                        newCosts = GameScaling.developmentPurchaseCostList[UtilityType.Capital];
                        break;
                    default:
                        break;

                }
            }
            else
            {
                category = "None";
                spriteName = "NoneGood";
            }

            _topCardTile = GameObject.Find("TileDeckTopCardImage");
            var sprite = _topCardTile.GetComponent<SpriteResolver>().spriteLibrary.GetSprite(category, spriteName);
            var image = _topCardTile.GetComponent<Image>().sprite = sprite;

            //Change the displayed costs

            int newMetalCost = 0;
            int newFoodCost = 0;
            int newManaCost = 0;
            int newGoldCost = 0;

            foreach(Cost c in newCosts)
            {
                switch(c.costType)
                {
                    case ResourceType.Food:
                        newFoodCost += c.cost;
                        break;
                    case ResourceType.Gold:
                        newGoldCost += c.cost;
                        break;
                    case ResourceType.Metal:
                        newMetalCost += c.cost;
                        break;
                    case ResourceType.Mana:
                        newManaCost += c.cost;
                        break;
                }
            }

            GameObject.Find("GoldCostDisplay").GetComponent<TextMeshProUGUI>().text = newGoldCost.ToString();
            GameObject.Find("FoodCostDisplay").GetComponent<TextMeshProUGUI>().text = newFoodCost.ToString();
            GameObject.Find("ManaCostDisplay").GetComponent<TextMeshProUGUI>().text = newManaCost.ToString();
            GameObject.Find("MetalCostDisplay").GetComponent<TextMeshProUGUI>().text = newMetalCost.ToString();



        }

        void ChangeSelectedCardDisplay(HexTile tile)
        {
            _topCardTile = GameObject.Find("CurrentTileImage");
            var sprite = _topCardTile.GetComponent<SpriteResolver>().spriteLibrary.GetSprite(tile.terrainCategory, tile.spriteName);
            var image = _topCardTile.GetComponent<Image>().sprite = sprite;

        }

        void DeselectCardDisplay()
        {
            _topCardTile = GameObject.Find("CurrentTileImage");
            var sprite = _topCardTile.GetComponent<SpriteResolver>().spriteLibrary.GetSprite("None", "None");
            var image = _topCardTile.GetComponent<Image>().sprite = sprite;

        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupGameState()
        {

            //Define a game state
            GameState = new GameState();
            GameState.SetSelectionState(GameStateEnums.SelectionState.None);
            GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
            GameState.SetAnimationState(GameStateEnums.CurrentAnimationState.None);
            GameState.SetPlayerActive(GameStateEnums.PlayerActive.Good);
            GameState.SetMapMode(GameStateEnums.MapMode.All);


            //Setup cards
            for (int i = 0; i < GameScaling.numberOfCards; i++)
            {
                //Get a development card
                developmentCards.Add(GameScaling.GetRandomDevelopmentCard());
            }


        }

        public void HighlightTile(HexTile hexTile)
        {
            tiles.ForEach((t) =>
            {
                HexTile tFound = t.GetComponent<HexTile>();
                if (tFound.qIndex == hexTile.qIndex && tFound.rIndex == hexTile.rIndex && tFound.sIndex == hexTile.sIndex)
                {
                    tFound.ChangeSelectionStatus(true);


                }
                else
                {
                    tFound.ChangeSelectionStatus(false);
                }

            });

        }


        public void BuildButtonClicked()
        {
            if (CanClick() && developmentCards.Count > 0)
            {
                DeselectCardDisplay();
                if (GameState.interactionState == GameStateEnums.InteractionState.PlacingDevelopment)
                {

                    GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
                    DeselectAllTiles();
                 
                }
                else
                {
                    if (CanAffordCost(_topCard.Costs, GameState.playerActive))
                    {

                        GameState.SetInteractionState(GameStateEnums.InteractionState.PlacingDevelopment);
                        DeselectAllTiles();
                        if (GameState.playerActive == GameStateEnums.PlayerActive.Good)
                        {
                            HighlightBuildableTiles(OwnerType.Good, HighlightColor.Green);
                        }
                        else
                        {
                            HighlightBuildableTiles(OwnerType.Evil, HighlightColor.Green);
                        }
                    }


                }
            }

        }

        private bool CanClick()
        {
            return GameState.interactionState != GameStateEnums.InteractionState.None && GameState.animationState == GameStateEnums.CurrentAnimationState.None;
        }


        public void PlaceUnitButtonClicked()
        {
            if (CanClick())
            {
                DeselectCardDisplay();
            }
            else
            {
                Debug.Log("Clicked when inactive!");

            }

        }


        public void ShuffleButtonClicked()
        {
            if (CanClick())
            {
                if (CanAffordCost(GameScaling.shuffleCost, GameState.playerActive))
                {
                    DeselectCardDisplay();
                    //Check the player can afford to shuffle

                    //Shuffle the cards
                    System.Random rng = new System.Random();
                    int n = developmentCards.Count;
                    while (n > 1)
                    {
                        n--;
                        int k = rng.Next(n + 1);
                        var value = developmentCards[k];
                        developmentCards[k] = developmentCards[n];
                        developmentCards[n] = value;
                    }

                    //Display the new top card
                    ChangeTopCardDisplay(withRemoval: false);
                    //Charge the player for the costs
                    RemoveCostsFromPlayer(GameScaling.shuffleCost, GameState.playerActive);

                }
            }
            else
            {
                Debug.Log("Clicked when inactive!");

            }

        }

        void RemoveCostsFromPlayer(List<Cost> costs, GameStateEnums.PlayerActive playerActive)
        {
            foreach (Cost c in costs)
            {
                if (playerActive == GameStateEnums.PlayerActive.Evil)
                {
                    EvilPlayer.GetComponent<Player>().RemoveCostFromBalance(c);
                    
                }
                else
                {
                    GoodPlayer.GetComponent<Player>().RemoveCostFromBalance(c);
                }
            }

        }


        public void EndTurnButtonClicked()
        {
            if (CanClick())
            {
                AddIncomes();
                DeselectAllTiles();
                GameState.SetPlayerActive(GetNextPlayer());
                GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
                ChangeTopCardDisplay(withRemoval:false);
            }
            else
            {
                Debug.Log("Clicked when inactive!");

            }
        }

        private GameStateEnums.PlayerActive GetNextPlayer()
        {
            if(GameState.playerActive == GameStateEnums.PlayerActive.Good)
            {
                return GameStateEnums.PlayerActive.Evil;
            }
            else
            {
                return GameStateEnums.PlayerActive.Good;
            }
        }

        /// <summary>
        /// Generic handler for the actiion when a tile is clicked - depends on current GameState
        /// </summary>
        /// <param name="hexTile"></param>
        public void ClickedTile(HexTile hexTile)
        {
            if(GameState.interactionState != GameStateEnums.InteractionState.None && GameState.animationState == GameStateEnums.CurrentAnimationState.None)
            {
                if(GameState.interactionState == GameStateEnums.InteractionState.SelectTile)
                {
                    HighlightTile(hexTile);
                    ChangeSelectedCardDisplay(hexTile);

                }
                else if (GameState.interactionState == GameStateEnums.InteractionState.PlacingDevelopment)
                {
                   if(hexTile.isHighlightedGreen)
                   {
                        if (CanAffordCost(_topCard.Costs, GameState.playerActive))
                        {
                            RemoveCostsFromPlayer(_topCard.Costs, GameState.playerActive);
                            PlaceDevelopmentInTile(hexTile, developmentCards[0].developmentType); //This will flip to next card

                            if(CanAffordCost(_topCard.Costs, GameState.playerActive))
                            {
                                HighlightBuildableTiles(GameState.PlayerActiveToOwnerType(), HighlightColor.Green);
                            }
                            else
                            {
                                GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
                                DeselectAllTiles();

                            }
                            
                            
                        }
                        
                        
                        
                      
                    }
                }


            }
            else
            {
                Debug.Log("Clicked when inactive!");

            }


            //HighlightBuildableTiles(OwnerType.Good, HighlightColor.Green);

        }

        public void PlaceDevelopmentInTile(HexTile tile, UtilityType utilityType)
        {
            tile.AddDevelopment(new List<UtilityType>() {utilityType}, GameState.PlayerActiveToOwnerType());
            ChangeTopCardDisplay();
            ChangeSelectedCardDisplay(tile);
           
        }

        public bool CanAffordCost(List<Cost> costs, GameStateEnums.PlayerActive playerActive)
        {
            bool canAfford = true;

            

            foreach(Cost c in costs)
            {
                if (playerActive == GameStateEnums.PlayerActive.Evil)
                {
                    if(EvilPlayer.GetComponent<Player>().GetBalance(c.costType).balance < c.cost)
                    {
                        canAfford = false;
                    }
                }
                else
                {
                    if (GoodPlayer.GetComponent<Player>().GetBalance(c.costType).balance < c.cost)
                    {
                        canAfford = false;
                    }
                }
            }


            return canAfford;

        }

        

        //Changes the tile to deselected
        public void DeselectAllTiles()
        {
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                tile.ChangeHighlightGreenStatus(false);
                tile.ChangeHighlightRedStatus(false);
                tile.ChangeSelectionStatus(false);

            });

        }

        /// <summary>
        /// Highlights all the neigbours 
        /// </summary>
        /// <param name="hexTile"></param>
        public void HighlightNeighbours(HexTile hexTile, HighlightColor colour)
        {
            //Get a list of all possible indicies for neighbours
            List<(int, int, int)> neighbours = TIleUtilities.GetNeighbours(hexTile.qIndex, hexTile.rIndex);

            //Highlight all these tiles
            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                neighbours.ForEach((tN) =>
                {
                    if(tN.Item1 == tile.qIndex && tN.Item2 == tile.rIndex && tN.Item3 == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if(found)
                {
                    switch(colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }
                    
                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });

        }

        /// <summary>
        /// Flood fills highlights for tiles connected in chains to the clicked tile
        /// Flexible match criteria
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="searchStrategies"></param>
        /// <param name="method"></param>
        /// <param name="colour"></param>
        public void HighlightMatchedAndConnectedTiles(HexTile tile, List<SearchStrategy> searchStrategies, SearchMatchMethod method, HighlightColor colour)
        {
            //Convert tiles to a new list of HexTileLite
            HexTileLite liteTile = new HexTileLite(tile.GetComponent<HexTile>());
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            var matchedTiles = TIleUtilities.FindConnectedTilesByCategory(liteList, liteTile, searchStrategies, method);

            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });
        }

        /// <summary>
        /// Highlight the neighbour tiles with a matched set of criteria
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="searchStrategies"></param>
        /// <param name="method"></param>
        /// <param name="colour"></param>
        public void HighlightMatchedNeighboursOfTile(HexTile tile, List<SearchStrategy> searchStrategies, SearchMatchMethod method, HighlightColor colour)
        {
            //Convert tiles to a new list of HexTileLite
            HexTileLite liteTile = new HexTileLite(tile.GetComponent<HexTile>());
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            var matchedTiles = TIleUtilities.GetNeighboursWithCriteria(liteList, liteTile, searchStrategies, method);

            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });
        }


        public void HighlightBuildableTiles(OwnerType ownerType, HighlightColor colour)
        {
            List<HexTileLite> matchedTiles = new();
            List<HexTileLite> buildableTiles = new();
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();

            List<(object, object)> positiveConditions = new();
            List<(object, object)> negativeConditions = new();
            positiveConditions.Add((ownerType, ownerType));
            negativeConditions.Add((TerrainType.Water, TerrainType.Water));
            //Get all tiles of that owner
            matchedTiles = TIleUtilities.FilterTilesByListOfGenericConditions(liteList, positiveConditions, negativeConditions);

            //Get all the neighbours of all the tiles that are matched and are neutral

            List<HexTileLite> neighbours = new();
            
            matchedTiles.ForEach((m) => {
                HexTileLite targetTile = new HexTileLite(m);
                targetTile.owner = OwnerType.Neutral;
                //Change to neutral for search
                neighbours.AddRange(TIleUtilities.GetNeighboursWithCriteria(liteList, targetTile, new List<SearchStrategy> { SearchStrategy.Owner }, SearchMatchMethod.All));
            });

            //Build a unique set

            neighbours = neighbours.Distinct().ToList();

            //Remove any neighbours that are water
            neighbours = neighbours.Where((n) => n.TerrainType != TerrainType.Water).ToList();



            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                neighbours.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });

        }

        public void HighlightTilesBasedOnGenericFilter<T>( List<(T, object)> positiveConditions, List<(T, object)> negativeConditions, HighlightColor colour, bool containsRepeatedTypes)
        {

            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            List<HexTileLite> matchedTiles = new();
            if (!containsRepeatedTypes)
            {
                 matchedTiles = TIleUtilities.FilterTilesByListOfGenericConditions(liteList, positiveConditions, negativeConditions);
            }
            else
            {
                matchedTiles = TIleUtilities.FilterTilesByListOfGenericConditionsWithSupportForRepeats(liteList, positiveConditions, negativeConditions);
            }

            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });

        }
    }
}
