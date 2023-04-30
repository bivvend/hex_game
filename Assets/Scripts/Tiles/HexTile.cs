using Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scripts.Tiles
{
    public class HexTile : MonoBehaviour
    {
        //Game objects
        //Sprite resolvers allolw swapping sprite at runtime
        public SpriteResolver spriteResolver;
        public SpriteResolver edgeResolver;
        public SpriteResolver unitResolver;
        public SpriteResolver heroResolver;
        //Prefabs from editor
        public GameObject tileSpritePrefab;
        public GameObject edgeSpritePrefab;
        public GameObject unitsSpritePrefab;
        public GameObject heroSpritePrefab;

        private GameObject _tileSprite;
        private GameObject _edgeSprite;
        private GameObject _unitSprite;
        private GameObject _heroSprite;

        public BoardController boardController;

        //Rendering coords for unity
        [HideInInspector]
        public float renderPosX { get; set; } = 0.0f;
        [HideInInspector]
        public float renderPosY = 0.0f;

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
        public List<UtilityType> Developments { get; private set; } = new();
        [HideInInspector]
        public List<Unit> Units { get; private set; } = new();

        [HideInInspector]
        public bool hasUnits => Units.Count > 0;

        /// <summary>
        /// A calcualted number added to the tile income based on surrounding development
        /// Recalcualted when a new development is added.
        /// </summary>
        public int AdjacencyBonus = 0;

        [HideInInspector]
        public OwnerType owner = OwnerType.Good;

        //Variables used to select sprite 
        private string _terrainCategory;
        private string _spriteName;
        private int _terrainVariant;

        //Selection statuses - only set via methods.
        public bool isSelected { get; private set; } = false;
        public bool isHighlightedGreen { get; private set; } = false;
        public bool isHighlightedRed { get; private set; } = false;
        private bool _selectionStatusChanged = false;

        /// <summary>
        /// Returns the stength of the square based on the Units present.
        /// </summary>
        /// <returns></returns>
        public int GetUnitStrength()
        {
            int numTroops = 0;


            return numTroops;

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
                    _terrainCategory = "Grass";
                    break;
                case TerrainType.Mountains:
                    _terrainCategory = "Mountains";
                    break;
                case TerrainType.Hills:
                    _terrainCategory = "Hills";
                    break;
                case TerrainType.Swamp:
                    _terrainCategory = "Swamp";
                    break;
                case TerrainType.Water:
                    _terrainCategory = "Water";
                    break;
                case TerrainType.Forest:
                    _terrainCategory = "Forest";
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
            _spriteName = _terrainCategory + _terrainVariant.ToString();
            _terrainCategory += terrainSuffix;

            //Get the sprite resolvers from the prefabs
            spriteResolver = tileSpritePrefab.GetComponent<SpriteResolver>();
            spriteResolver.SetCategoryAndLabel(_terrainCategory, _spriteName);

            edgeResolver = edgeSpritePrefab.GetComponent<SpriteResolver>();
            edgeResolver.SetCategoryAndLabel("Selectors", "Black");

            PointF pos = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex, rIndex);
            renderPosX = (_size / 2.0f) * pos.X;
            renderPosY = (_size / 2.0f) * pos.Y;

            _tileSprite = Instantiate(tileSpritePrefab, new Vector3(renderPosX, renderPosY, 0), Quaternion.identity);
            //Pass the parent to the sprite prefab to allow click event bubbling up.
            _tileSprite.transform.parent = gameObject.transform;
            _edgeSprite = Instantiate(edgeSpritePrefab, new Vector3(renderPosX, renderPosY, -0.01f), Quaternion.identity);
            _edgeSprite.transform.parent = gameObject.transform;

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
        }

        [HideInInspector]    
        public void Clicked()
        {
            Debug.Log("Click from child!");
            boardController.ClickedTile(this);

        }
    }
}
