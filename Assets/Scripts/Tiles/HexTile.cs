using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Tiles
{
    public class HexTile : MonoBehaviour
    {

        public GameObject tileSpritePrefab;

        //Rendering coords for unity
        [HideInInspector]
        public float renderPosX = 0.0f;
        [HideInInspector]
        public float renderPosY = 0.0f;

        private float _width = 1.28f;
        private float _height = 1.28f;

        //Indicies in cube coords
        [HideInInspector]
        public int qIndex = 0;
        [HideInInspector]
        public int rIndex = 0;
        [HideInInspector]
        public int sIndex = 0;

        //public Tile(GameObject tileSpritePrefab, int qIndex, int rIndex, int sIndex)
        //{
        //    this.tileSpritePrefab = tileSpritePrefab;
        //    this.qIndex = qIndex;
        //    this.rIndex = rIndex;
        //    this.sIndex = sIndex;
        //}




        // Start is called before the first frame update
        void Start()
        {
            Instantiate(tileSpritePrefab, new Vector3(renderPosX, renderPosY, 0), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
