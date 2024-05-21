using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    // Start is called before the first frame update
    public Button BackBtn;
    void Start()
    {
        BackBtn.onClick.AddListener(ClickToQuit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClickToQuit(){
        Application.Quit();
    }
}
