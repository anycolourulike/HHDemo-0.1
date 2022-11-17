using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{  
   [SerializeField] Transform startPos;
   [SerializeField] Transform player;
   
   void OnTriggerEnter(Collider other)
   {
      if(other.CompareTag("Player"))
      {
        player.position = startPos.position;
      }
   }  
}
