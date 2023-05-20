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

namespace Scripts
{
    public class BoardController : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> tiles = new List<GameObject>();

        public List<DevelopmentCard> developmentCards = new List<DevelopmentCard>();
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

        // Start is called before the first frame update
        void Start()
        {
            SetupGameState();

            Array values = Enum.GetValues(typeof(TerrainType));
            System.Random random = new System.Random();

            


            //Create a circular map of given radius
            for (int q = (-1* (mapRadius +2)); q <= (mapRadius + 2); q++)
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
                            if(randomType == TerrainType.Water)
                            {
                                randomType = TerrainType.Grass;
                            }    

                            if(Math.Abs(q) == mapRadius + 2 || Math.Abs(r) == mapRadius + 2 || Math.Abs(s) == mapRadius + 2)
                            {
                                randomType = TerrainType.Water;
                            }
                            if (Math.Abs(q) == mapRadius + 1 || Math.Abs(r) == mapRadius + 1|| Math.Abs(s) == mapRadius + 1)
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

                            if(q > 2)
                            {
                                t.owner = OwnerType.Good;
                            }
                            else if(q < -2)
                            {

                                t.owner = OwnerType.Evil;
                            }
                            else
                            {
                                t.owner = OwnerType.Neutral;
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
            
        }

        void ChangeTopCardDisplay()
        {
            string category = "";
            string spriteName = "";

            if (developmentCards.Count > 1) {
                developmentCards.RemoveAt(0);
                var card = developmentCards[0];
                switch (card.developmentType)
                {
                    case UtilityType.Mine:
                        category = "Mine";
                        spriteName = "Mine" + card.variant.ToString();
                        break;
                    case UtilityType.Town:
                        category = "Town";
                        spriteName = "Town" + card.variant.ToString();
                        break;
                    case UtilityType.QuestSite:
                        category = "QuestSite";
                        spriteName = "QuestSite" + card.variant.ToString();
                        break;
                    case UtilityType.Farm:
                        category = "Farm";
                        spriteName = "Farm" + card.variant.ToString();
                        break;
                    case UtilityType.SorcerersTower:
                        category = "SorcerersTower";
                        spriteName = "SorcerersTower" + card.variant.ToString();
                        break;
                    case UtilityType.Fort:
                        category = "Fort";
                        spriteName = "Fort" + card.variant.ToString();
                        break;
                    case UtilityType.Capital:
                        category = "Capital";
                        spriteName = "Capital" + card.variant.ToString();
                        break;
                    default:
                        break;

                }
            }
            else
            {
                category = "None";
                spriteName = "None";
            }

            

           
            _topCardTile = GameObject.Find("TileDeckTopCardImage");
            var sprite = _topCardTile.GetComponent<SpriteResolver>().spriteLibrary.GetSprite(category, spriteName);
            var image = _topCardTile.GetComponent<Image>().sprite = sprite;

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
            for(int i = 0; i < GameScaling.numberOfCards ; i++)
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
            if(CanClick() && developmentCards.Count > 0)
            {
                DeselectCardDisplay();
                if (GameState.interactionState == GameStateEnums.InteractionState.PlacingDevelopment)
                {
                    GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
                    DeselectAllTiles();
                }
                else
                {
                    GameState.SetInteractionState(GameStateEnums.InteractionState.PlacingDevelopment);
                    DeselectAllTiles();
                    if (GameState.playerActive == GameStateEnums.PlayerActive.Good)
                    {
                        HighlightBuildableTiles(OwnerType.Good, HighlightColor.Green) ;
                    }
                    else
                    {
                        HighlightBuildableTiles(OwnerType.Evil, HighlightColor.Green);
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
                DeselectCardDisplay();
            }
            else
            {
                Debug.Log("Clicked when inactive!");

            }

        }


        public void EndTurnButtonClicked()
        {
            if (CanClick())
            {
                DeselectAllTiles();
                GameState.SetPlayerActive(GetNextPlayer());
                GameState.SetInteractionState(GameStateEnums.InteractionState.SelectTile);
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
                        PlaceDevelopmentInTile(hexTile, UtilityType.Mine);
                        HighlightBuildableTiles(GameState.PlayerActiveToOwnerType(), HighlightColor.Green);
                      
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
