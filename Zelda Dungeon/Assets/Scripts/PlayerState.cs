using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
   public static PlayerState instance {get; private set;}
   public bool isPushing;
   public bool isGrounded;

   public Action<bool> OnPlayerAim;

   private void Awake()
   {
      instance = this;
   }

   public void PlayerAimEvent(bool isAiming)
   {
      if (isAiming)
      {
         OnPlayerAim?.Invoke(true);
      }
      else
      {
         OnPlayerAim?.Invoke(false);
      }
   }
}