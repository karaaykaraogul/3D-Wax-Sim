using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class ColliderBridge : MonoBehaviour
 {
     MoveStickWithTouch _listener;
     public void Initialize(MoveStickWithTouch l)
     {
         _listener = l;
     }
     void OnCollisionEnter(Collision collision)
     {
         _listener.OnCollisionEnter(collision);
     }
    //  void OnTriggerEnter2D(Collider2D other)
    //  {
    //      _listener.OnTriggerEnter(other);
    //  }
 }
