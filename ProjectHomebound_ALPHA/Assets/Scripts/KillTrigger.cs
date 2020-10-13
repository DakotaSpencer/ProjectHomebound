using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public PlayerManager playerManager;

    void OnTriggerEnter ()
    {
        playerManager.GameOver();
    }
}
