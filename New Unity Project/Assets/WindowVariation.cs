using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowVariation : MonoBehaviour
{
    public Color color;
    public Cubemap ds;
    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer> ();
    }

    void Update()
    {
        Color lerpedColor = Color.Lerp(Color.black, Color.blue, Mathf.PingPong(Time.time, 0.9f));
        rend.material.SetColor("_Color", lerpedColor);
        rend.material.SetTexture("_RoomCube", ds);
    }
}
