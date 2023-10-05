using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom_Handler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GenerationManager.instance.WinGame();
    }
}
