using Scripts.Cards;
using Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scripts.Tiles
{
    public class HexTile : MonoBehaviour
    {
        //Game objects
        //Sprite resolvers allow swapping sprite at runtime
        public SpriteResolver spriteResolver;
        public SpriteResolver developmentResolver;
        public SpriteResolver edgeResolver;
        public SpriteResolver unitResolver;
        public SpriteResolver heroResolver;
        //Prefabs from editor
        public GameObject tileSpritePrefab;
        public GameObject edgeSpritePrefab;
        public GameObject developmentPrefab;
        public GameObject unitsSpritePrefab;
        public GameObject heroSpritePrefab;
        public GameObject unitCountPrefab;

        private GameObject _tileSprite;
        private GameObject _edgeSprite;
        private GameObject _developmentSprite;
        private GameObject _unitSprite;
        private GameObject _heroSprite;
        private GameObject _unitCountSprite;
        

        public BoardController boardController;

        //Rendering coords for unity
        [HideInInspector]
        public float renderPosX { get; set; } = 0.0f;
        [HideInInspector]
        public float renderPosY = 0.0f;

        public float renderOffsetX = -5.0f;
        public float renderOffsetY = 0.0f;

        private float _size = 2.8f; //Width of flat top 

        //Indicies in cube coords
        [HideInInspector]
        public int qIndex = 0;
        [HideInInspector]
        public int rIndex = 0;
        [HideInInspector]
        public int sIndex = 0;

        //Tile data
        //Terrain
        [HideInInspector]
        public TerrainType TerrainType;
        //List of 6 utility types one for each side
        [HideInInspector]
        public List<UtilityType> Developments { get; set; } = new();
        [HideInInspector]
        public List<Unit> Units { get; private set; } = new();

        [HideInInspector]
        public bool hasHero => Units.Where((u) => u.unitType == UnitEnums.UnitType.Hero).ToList().Count > 0;

        [HideInInspector]
        public bool hasGeneral => Units.Where((u) => u.unitType == UnitEnums.UnitType.General).ToList().Count > 0;

        [HideInInspector]
        public bool hasWizard => Units.Where((u) => u.unitType == UnitEnums.UnitType.Wizard).ToList().Count > 0;

        [HideInInspector]
        public bool hasUnits => Units.Count > 0;

        [HideInInspector]
        /// <summary>
        /// A calcualted number added to the tile income based on surrounding development
        /// Recalcualted when a new development is added.
        /// </summary>
        public int AdjacencyBonus = 0;

        [HideInInspector]
        public OwnerType owner = OwnerType.Good;

        //Variables used to select sprite 
        public string terrainCategory;
        public string developmentCategory;
        public string spriteName;
        private int _terrainVariant;

        private int _developmentVariant;

        //Selection statuses - only set via methods.
        public bool isSelected { get; private set; } = false;
        public bool isHighlightedGreen { get; private set; } = false;
        public bool isHighlightedRed { get; private set; } = false;

        private bool _selectionStatusChanged = true;
        private bool _ownershipChanged = true;
        private bool _developmentsChanged = true;
        private bool _unitsChanged = true;

        private PointF _unitsRenderOffset = new PointF(0.0f, 0.0f);   //Used for animation


        /// <summary>
        /// Returns the stength of the square based on the Units present.
        /// </summary>
        /// <returns></returns>
        public int GetUnitStrength()
        {
            int numTroops = 0;

            Units.ForEach((unit) =>
            {
                numTroops += unit.GetNumberOfTroops();
            });

            return numTroops;

        }

        /// <summary>
        /// Sets the sprite offset to be a given fraction (0...1) of the distance between the tiles with given indicies.
        /// </summary>
        /// <param name="qIndex1"></param>
        /// <param name="rIndex1"></param>
        /// <param name="qIndex2"></param>
        /// <param name="rIndex2"></param>
        /// <param name="fraction"></param>
        public void SetSpriteOffset(int qIndex1, int rIndex1, int qIndex2, int rIndex2, float fraction)
        {
            PointF pos1 = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex1, rIndex1);
            float posX1 = (_size / 2.0f) * pos1.X + renderOffsetX;
            float posY1 = (_size / 2.0f) * pos1.Y + renderOffsetY;

            PointF pos2 = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex2, rIndex2);
            float posX2 = (_size / 2.0f) * pos2.X + renderOffsetX;
            float posY2 = (_size / 2.0f) * pos2.Y + renderOffsetY;

            float dx = fraction * (posX2 - posX1);
            float dy = fraction * (posY2 - posY1);

            _unitsRenderOffset.X = dx;
            _unitsRenderOffset.Y = dy;


            _unitsChanged = true;
        }


        public void ChangeSelectionStatus(bool statusIn)
        {
            isSelected = statusIn;
            _selectionStatusChanged = true;
        }

        public void ChangeHighlightGreenStatus(bool statusIn)
        {
            isHighlightedGreen = statusIn;
            _selectionStatusChanged = true;
        }


        public void ChangeHighlightRedStatus(bool statusIn)
        {
            isHighlightedRed = statusIn;
            _selectionStatusChanged = true;
        }

        // Start is called before the first frame update
        void Start()
        {

            _terrainVariant = Random.Range(1, 5);  //4 variants
            switch (TerrainType)
            {
                case TerrainType.Grass:
                    terrainCategory = "Grass";
                    break;
                case TerrainType.Mountains:
                    terrainCategory = "Mountains";
                    break;
                case TerrainType.Hills:
                    terrainCategory = "Hills";
                    break;
                case TerrainType.Swamp:
                    terrainCategory = "Swamp";
                    break;
                case TerrainType.Water:
                    terrainCategory = "Water";
                    break;
                case TerrainType.Forest:
                    terrainCategory = "Forest";
                    break;
            }

            string terrainSuffix = "";
            switch(owner)
            {
                case OwnerType.Good:
                    terrainSuffix = "Good";
                    break;
                case OwnerType.Evil:
                    terrainSuffix = "Evil";
                    break;
                case OwnerType.Neutral:
                    terrainSuffix = "";
                    break;
                default:
                    terrainSuffix = "";
                    break;
            }

            
            //This order matters!
            spriteName = terrainCategory + _terrainVariant.ToString();
            terrainCategory += terrainSuffix;

            //Get the sprite resolvers from the prefabs
            spriteResolver = tileSpritePrefab.GetComponent<SpriteResolver>();
            spriteResolver.SetCategoryAndLabel(terrainCategory, spriteName);

            developmentResolver = developmentPrefab.GetComponent<SpriteResolver>();
            developmentResolver.SetCategoryAndLabel("None", "NoneGood");

            unitResolver = unitsSpritePrefab.GetComponent<SpriteResolver>();
            unitResolver.SetCategoryAndLabel("None", "None");

            edgeResolver = edgeSpritePrefab.GetComponent<SpriteResolver>();
            edgeResolver.SetCategoryAndLabel("Selectors", "Black");

            PointF pos = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex, rIndex);
            renderPosX = (_size / 2.0f) * pos.X + renderOffsetX;
            renderPosY = (_size / 2.0f) * pos.Y + renderOffsetY;

            _tileSprite = Instantiate(tileSpritePrefab, new Vector3(renderPosX, renderPosY, 0), Quaternion.identity);
            //Pass the parent to the sprite prefab to allow click event bubbling up.
            _tileSprite.transform.parent = gameObject.transform;

            _edgeSprite = Instantiate(edgeSpritePrefab, new Vector3(renderPosX, renderPosY, -0.02f), Quaternion.identity);
            _edgeSprite.transform.parent = gameObject.transform;

            _developmentSprite = Instantiate(developmentPrefab, new Vector3(renderPosX, renderPosY, -0.01f), Quaternion.identity);

            _unitSprite = Instantiate(unitsSpritePrefab, new Vector3(renderPosX + 0.3f, renderPosY+ 0.3f, -0.5f), Quaternion.identity);

            _unitCountSprite = Instantiate(unitCountPrefab, new Vector3(renderPosX + 0.5f, renderPosY - 0.6f, -0.8f), Quaternion.identity);
            _unitCountSprite.GetComponent<TextMesh>().text = "";
            _unitCountSprite.transform.localScale = new Vector3(0.02f, 0.02f, 1.0f);


        }

        // Update is called once per frame
        void Update()
        {
            if(_selectionStatusChanged)
            {
                if(isSelected)
                {
                    _edgeSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel("Selectors", "Yellow");
                }
                else if (isHighlightedGreen)
                {
                    _edgeSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel("Selectors", "Green");
                }
                else if (isHighlightedRed)
                {
                    _edgeSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel("Selectors", "Red");
                }
                else
                {
                    _edgeSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel("Selectors", "Black");
                }
                

                _selectionStatusChanged = false;
            }
            if(_ownershipChanged)
            {
                switch (TerrainType)
                {
                    case TerrainType.Grass:
                        terrainCategory = "Grass";
                        break;
                    case TerrainType.Mountains:
                        terrainCategory = "Mountains";
                        break;
                    case TerrainType.Hills:
                        terrainCategory = "Hills";
                        break;
                    case TerrainType.Swamp:
                        terrainCategory = "Swamp";
                        break;
                    case TerrainType.Water:
                        terrainCategory = "Water";
                        break;
                    case TerrainType.Forest:
                        terrainCategory = "Forest";
                        break;
                }

                string terrainSuffix = "";
                switch (owner)
                {
                    case OwnerType.Good:
                        terrainSuffix = "Good";
                        break;
                    case OwnerType.Evil:
                        terrainSuffix = "Evil";
                        break;
                    case OwnerType.Neutral:
                        terrainSuffix = "";
                        break;
                    default:
                        terrainSuffix = "";
                        break;
                }


                //This order matters!
                spriteName = terrainCategory + _terrainVariant.ToString();
                terrainCategory += terrainSuffix;

                //Get the sprite resolvers from the prefabs
                _tileSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel(terrainCategory, spriteName);

                _ownershipChanged = false;
            }

            if(_developmentsChanged)
            {

                
                string category = "None";
                string devSpriteName = "NoneGood";
                if (Developments.Count > 0)
                {
                    string currentOwner = owner == OwnerType.Good ? "Good" : "Evil";
                    
                    switch (Developments[0])
                    {
                        case UtilityType.Mine:
                            category = "Mine";
                            devSpriteName = "Mine" + currentOwner;
                            break;
                        case UtilityType.Town:
                            category = "Town";
                            devSpriteName = "Town" + currentOwner;
                            break;
                        case UtilityType.QuestSite:
                            category = "QuestSite";
                            devSpriteName = "QuestSite" + currentOwner;
                            break;
                        case UtilityType.Farm:
                            category = "Farm";
                            devSpriteName = "Farm" + currentOwner;
                            break;
                        case UtilityType.SorcerersTower:
                            category = "SorcerersTower";
                            devSpriteName = "SorcerersTower" + currentOwner;
                            break;
                        case UtilityType.Fort:
                            category = "Fort";
                            devSpriteName = "Fort" + currentOwner;
                            break;
                        case UtilityType.Capital:
                            category = "Capital";
                            devSpriteName = "Capital" + currentOwner;
                            break;
                        default:
                            break;

                    }
                }
                else
                {
                    category = "None";
                    devSpriteName = "NoneGood";
                }

                _developmentSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel(category, devSpriteName);

                _developmentsChanged = false;
            }
            if(_unitsChanged)
            {
                _unitsChanged = false;

                string currentOwner = owner == OwnerType.Good ? "Good" : "Evil";

                if (hasUnits)
                {
                    _unitSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel(currentOwner, "Warrior");
                    _unitSprite.transform.position = new Vector3(renderPosX + 0.3f + _unitsRenderOffset.X, renderPosY + 0.3f + _unitsRenderOffset.Y, -0.5f);
                    

                    _unitCountSprite.GetComponent<TextMesh>().text = Units.Count.ToString(); 
                }
                else
                {
                    _unitSprite.GetComponent<SpriteResolver>().SetCategoryAndLabel("None", "None");
                    _unitCountSprite.GetComponent<TextMesh>().text = "";
                }

                

                _unitsChanged = false;

            }
        }

        public void ChangeOwnerShip(OwnerType ownerTypeIn)
        {
            owner = ownerTypeIn;
            _ownershipChanged = true;
            _developmentsChanged = true;
            _unitsChanged = true;
        

        }


        public void AddTroops(List<Unit> newUnitList)
        {
            Units.AddRange(newUnitList);
            _unitsChanged = true;

        }

        public void RemoveTroops(List<int> indices)
        {
            indices.ForEach((i) =>
            {
                if(Units.Count > i)
                {
                    Units.RemoveAt(i);  
                }
            });
            _unitsChanged = true;

        }

        public void RemoveAllTroops()
        {
            Units.Clear();
            _unitsChanged = true;

        }

        public void AddDevelopment(List<UtilityType> newDevelopments, OwnerType newOwner)
        {
            ChangeOwnerShip(newOwner);
            Developments.AddRange(newDevelopments);
            _developmentsChanged = true;
        }

        [HideInInspector]    
        public void Clicked()
        {
            Debug.Log("Click from child!");
            boardController.ClickedTile(this);

        }
    }
}
