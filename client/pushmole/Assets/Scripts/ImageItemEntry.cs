using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageItemEntry : UILoopItem
{

    public Button Btn_;
    protected RawImage _RawImage;
    public Text text_;

    void Awake()
    {
        _RawImage = Btn_.GetComponent<RawImage>();
        //text = Btn_.GetComponent<Text>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTexture(Texture2D texture)
    {
        _RawImage.texture = texture;
    }

    protected override void ResetItem(int index)
    {

        text_.text = index + 1 + "";
    }
}
