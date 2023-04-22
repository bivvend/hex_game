using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera mainCamera;
    public GameObject backgroundImage;

    void Start()
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float ratio = screenWidth / screenHeight;

        float camHeight = 100.0f * mainCamera.orthographicSize * 2.0f;
        float camWidth = camHeight * ratio;
        SpriteRenderer back = backgroundImage.GetComponent<SpriteRenderer>();
        float bgImgH = back.sprite.rect.height;
        float bgImgW = back.sprite.rect.width;
        float scaleH = camHeight/ bgImgH;
        float scaleW = camWidth/ bgImgW;


        backgroundImage.transform.localScale = new Vector3(scaleW, scaleH, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
