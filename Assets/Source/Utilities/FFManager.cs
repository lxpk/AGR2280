using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class FFManager : MonoBehaviour {

    public float timerMotor;
    public float vibLeftMotor;
    public float vibRightMotor;

    void Update()
    {
        if (timerMotor == -1)
        {
            GamePad.SetVibration(0, vibLeftMotor, vibRightMotor);
        } else
        {
            timerMotor -= Time.deltaTime;
            if (timerMotor < 1)
            {
                timerMotor = 0;
                GamePad.SetVibration(0, 0, 0);
            } else
            {
                GamePad.SetVibration(0, vibLeftMotor, vibRightMotor);
            }
        }
    }

    public void StopVibration()
    {
        GamePad.SetVibration(0, 0, 0);
    }

    void OnApplicationQuit()
    {
        StopVibration();
    }
}
