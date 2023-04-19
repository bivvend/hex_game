using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        private float _size = 1.28f; //Width of flat top 

        //Indicies in cube coords
        [HideInInspector]
        public int qIndex = 0;
        [HideInInspector]
        public int rIndex = 0;
        [HideInInspector]
        public int sIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            PointF pos = TIleUtilities.convertHexIndiciesToCartesianFlatTop(qIndex, rIndex);
            renderPosX = (_size / 2.0f) * pos.X;
            renderPosY = (_size / 2.0f) * pos.Y;
            Instantiate(tileSpritePrefab, new Vector3(renderPosX, renderPosY, 0), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
