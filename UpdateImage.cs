using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateImage : MonoBehaviour
{
    // Start is called before the first frame update
    public static UpdateImage updateImage_instance;
    // public Sprite untuneImg; // I attched these from editor
    public Sprite tuningImg;
    public Sprite tunedImg;
    public int status;
    private Image img;
    void Start()
    {
        updateImage_instance = this;
        status = 0;
    }
    void Update()
    {
        switch (status)
        {
            case 1:
                img.sprite = tuningImg;
                break;
            case 2:
                img.sprite = tunedImg;
                break;
        }
    }
    public void setStatusAndImage(Image _img, int foo)
    {
        img = _img;
        status = foo;
    }
}
