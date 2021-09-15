using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxAutoResize : MonoBehaviour
{
    public RectTransform text1;
    public RectTransform text2;
    void Start()
    {

    }
    void Update()
    {
        print(LayoutUtility.GetPreferredWidth(text2) + " - " + transform.GetComponent<RectTransform>().sizeDelta.y);
        //Canvas.ForceUpdateCanvases();
        if(text1.sizeDelta.y + text2.sizeDelta.y - transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y < 0)
        {
            GetComponent<RectTransform>().sizeDelta += new Vector2(0, 0.1f);
        }
        if(LayoutUtility.GetPreferredWidth(text2) > transform.GetComponent<RectTransform>().sizeDelta.y)
        {
            GetComponent<RectTransform>().sizeDelta += new Vector2(0,LayoutUtility.GetPreferredWidth(text2));
            text2.sizeDelta = new Vector2(text2.sizeDelta.x, LayoutUtility.GetPreferredWidth(text2));
            text2.position -= new Vector3(0, LayoutUtility.GetPreferredWidth(text2)/2, 0);
        }
    }
}
