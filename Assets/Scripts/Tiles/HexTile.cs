using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scripts.Tiles
{
    public class HexTile : MonoBehaviour
    {

        public SpriteResolver spriteResolver;
        public GameObject tileSpritePrefab;
        public GameObject edgeSpritePrefab;

        //Rendering coords for unity
        [HideInInspector]
        public float renderPosX { get; set; } = 0.0f;
        [HideInInspector]
        public float renderPosY = 0.0f;

        private float _size = 1.35f; //Width of flat top 

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
        public List<UtilityType> Developments = new();

        [HideInInspector]
        public OwnerType owner = OwnerType.Good;

        private string _terrainCategory;
        private string _spriteName;
        private int _terrainVariant;

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

            _spriteName = _terrainCategory + _terrainVariant.ToString();


            //Get the sprite resolver from the prefab
            spriteResolver = tileSpritePrefab.GetComponent<SpriteResolver>();
            spriteResolver.SetCategoryAndLabel(_terrainCategory, _spriteName);
            PointF pos = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex, rIndex);
            renderPosX = (_size / 2.0f) * pos.X;
            renderPosY = (_size / 2.0f) * pos.Y;

            Instantiate(tileSpritePrefab, new Vector3(renderPosX, renderPosY, 0), Quaternion.identity);
            Instantiate(edgeSpritePrefab, new Vector3(renderPosX, renderPosY, -0.01f), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
