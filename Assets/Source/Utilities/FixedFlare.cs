using UnityEngine;
using System.Collections;

public class FixedFlare : MonoBehaviour
{

    LensFlare FlareComponent;
    public float Size;

    void Start()
    {
        FlareComponent = GetComponent<LensFlare>();
    }

    void Update()
    {
        float ratio = Mathf.Sqrt(Vector3.Distance(transform.position, Camera.main.transform.position));
        GetComponent<LensFlare>().brightness = Size / ratio;
    }
}
