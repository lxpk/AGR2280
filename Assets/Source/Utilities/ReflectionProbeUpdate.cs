using UnityEngine;
using System.Collections;

public class ReflectionProbeUpdate : MonoBehaviour {

    private ReflectionProbe reflection;
    
    void Start()
    {
        reflection = GetComponent<ReflectionProbe>();
    }

    void LateUpdate()
    {
        reflection.RenderProbe();    
    }
}
