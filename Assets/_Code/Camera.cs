using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private static Camera _ins;
    public static Camera Ins
    {
        get
        {
            if (!_ins)
                _ins = FindObjectOfType<Camera>();

            return _ins;
        }
    }
}
