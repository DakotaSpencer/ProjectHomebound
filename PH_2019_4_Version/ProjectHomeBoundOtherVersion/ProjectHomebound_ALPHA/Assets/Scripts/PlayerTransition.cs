using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{ 
    
    
    
    void OnCollisionEnter(Collision collision)
    {
 
        if(collision.gameObject.name == "Collider")
        {
            Debug.Log("Load dogs scene");
        }
        
    }
}
