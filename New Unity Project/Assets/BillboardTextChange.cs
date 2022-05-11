using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillboardTextChange : MonoBehaviour
{
    public List<string> texts;
    private int currentText = 0;
    public float timeToChange = 2;
    private float timer = 0;
    public float flickTime = 1;
    private float flickTimer = 0;
    TMP_Text textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.text = texts[0];
        timer = timeToChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(flickTimer > 0)
        {
            flickTimer -= Time.deltaTime * 2;
        }
        else
        {
            if(textMeshPro.color == Color.white)
            {
                textMeshPro.color = Color.black;
            }
            else
            {
                textMeshPro.color = Color.white;
            }
            flickTimer = flickTime;
        }

        if(timer > 0)
        {
            timer -= Time.deltaTime * 2;
        }
        else
        {
            if(texts.Count == (currentText + 1))
            {
                currentText = 0;
            }
            else
            {
                currentText++;
            }
            textMeshPro.text = texts[currentText];
            timer = timeToChange;
        }

    }
}
