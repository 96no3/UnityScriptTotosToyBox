using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotoFace : MonoBehaviour
{
    [SerializeField] private Renderer face;
    [SerializeField] private Material[] faceMat;
    
    public void ChangeFace(int n)
    {
        face.material = faceMat[n];
    }
}
