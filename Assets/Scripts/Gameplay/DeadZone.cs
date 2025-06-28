using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class InisibleKillZone : MonoBehaviour
{
    public InGameManager InGameMgr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            InGameMgr.StartGameOverCountdown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            InGameMgr.CancelGameOverCountdown();
        }
    }
}
