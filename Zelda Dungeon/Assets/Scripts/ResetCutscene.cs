using System;
using UnityEngine;
using UnityEngine.Playables;

public class ResetCutscene : MonoBehaviour
{
   public GameObject door1, door2, ball1, ball2;
   private Vector3 door1Pos, door2Pos, ball1Pos, ball2Pos;
   [SerializeField] private PlayableDirector playableDirector;
   
   void Start()
   {
      
      door1Pos = door1.transform.position;
      door2Pos = door2.transform.position;
      ball1Pos = ball1.transform.position;
      ball2Pos = ball2.transform.position;
      GameEventsManager.instance.hasSeenCutsceneAlready = false;
      
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         if(GameEventsManager.instance.hasSeenCutsceneAlready == false)
            return;
         
         
         ball2.GetComponent<DangerBallRush>().canMove = false;
         ball2.GetComponent<Rigidbody>().isKinematic = true;
         GameEventsManager.instance.hasSeenCutsceneAlready = false;
         playableDirector.Stop();
         playableDirector.time = 0;
         ball2.gameObject.SetActive(false);
         
         door1.transform.position = door1Pos;
         door2.transform.position = door2Pos;
         ball1.transform.position = ball1Pos;
         ball2.transform.position = ball2Pos;
        

      }
      
   }
}
