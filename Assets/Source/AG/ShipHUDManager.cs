using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipHUDManager : MonoBehaviour {

    public GameObject targetShip;
    public GameObject speedText;

    void Update()
    {
        // Get the ship speed
        float speed = targetShip.transform.InverseTransformDirection(targetShip.GetComponent<Rigidbody>().velocity).z * 2;
        // Set speed text
        speedText.GetComponent<Text>().text = Mathf.RoundToInt(speed).ToString();
    }
}
