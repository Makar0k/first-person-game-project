using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : Interactable
{
    public Vector3 openRotation;
    protected Quaternion openRotationQ;
    protected Quaternion defaultRotation;
    protected bool isOpened = false;
    public bool moveRoot;
    void Start()
    {
        defaultRotation = moveRoot ? transform.root.rotation : transform.rotation;
        openRotationQ = Quaternion.Euler(openRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpened == false)
        {
            if(moveRoot)
            {
                transform.root.rotation = Quaternion.Lerp(transform.root.rotation, defaultRotation, Time.deltaTime * 2);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, Time.deltaTime * 2);
            }
        }
        else
        {
            if(moveRoot)
            {
                transform.root.rotation = Quaternion.Lerp(transform.root.rotation, openRotationQ, Time.deltaTime * 2);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, openRotationQ, Time.deltaTime * 2);
            }
        }
    }

    public override void Interact()
    {
        if(isOpened == false)
        {
            isOpened = true;
            return;
        }
        else
        {
            isOpened = false;
            return;
        }
    }
}
