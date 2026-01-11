using System;
using UnityEngine;
using UnityEngine.Events;


public enum ScaleBalance
{
    Balance,
    Left, 
    Right
}
public class WeighingScale : MonoBehaviour
{
    public WeighingPlate scale1;
    public WeighingPlate scale2;

    public float weightSpeed = 2f;

    private Vector3 ImbalanceScale1UpPos;
    private Vector3 ImbalanceScale2UpPos;
    private Vector3 ImbalanceScale1DownPos;
    private Vector3 ImbalanceScale2DownPos;
    private Vector3 BalanceScale1Pos;
    private Vector3 BalanceScale2Pos;

    public float threshHold = 0.1f;

   public ScaleBalance scaleBalance;
    
   public bool shouldActivateInitial;
    public UnityEvent OnScaleRight = new UnityEvent(), OnScaleLeft = new UnityEvent(), OnScaleBalance = new UnityEvent();
    private void Start()
    {
       SetPosition();

    }

    [ContextMenu("Set Position")]
    public void SetPosition()
    {
        ImbalanceScale1DownPos = scale1.transform.position + new Vector3(0, scale1.desiredImbalanceYValueDown, 0);
        ImbalanceScale2DownPos = (scale2.transform.position + new Vector3(0, scale2.desiredImbalanceYValueDown, 0));
        ImbalanceScale1UpPos = (scale1.transform.position + new Vector3(0, scale1.desiredImbalanceYValueUp, 0));
        ImbalanceScale2UpPos = (scale2.transform.position + new Vector3(0, scale2.desiredImbalanceYValueUp, 0));
        BalanceScale1Pos = scale1.transform.position + new Vector3(0, scale1.desiredBalanceYValue, 0);
        BalanceScale2Pos = scale2.transform.position + new Vector3(0, scale2.desiredBalanceYValue, 0);
        
       
        
         if(scale1.GetMass() > scale2.GetMass())
        {
      
            scale1.transform.position = ImbalanceScale1DownPos;
            scale2.transform.position = ImbalanceScale2UpPos;
            if (shouldActivateInitial)
            {
                OnScaleLeft?.Invoke();
            }
         
            scaleBalance = ScaleBalance.Left;
          
        }

        else if(scale2.GetMass() > scale1.GetMass())
        {
  
            scale1.transform.position = ImbalanceScale1UpPos;
            scale2.transform.position = ImbalanceScale2DownPos;
            if (shouldActivateInitial)
            {
                OnScaleRight?.Invoke();
            }
            scaleBalance = ScaleBalance.Right;
         
        }

        else
        {
            
            scale1.transform.position = BalanceScale1Pos;
            scale2.transform.position = BalanceScale2Pos;
            if (shouldActivateInitial)
            {
                OnScaleBalance?.Invoke();
            }
            
            scaleBalance = ScaleBalance.Balance;
            
        }
        
        
    }
    private void FixedUpdate()
    {
       

        if(scale1.accumulatedMass > scale2.accumulatedMass)
        {
            Debug.Log("Imbalanced towards Scale 1");
            scale1.rigidbody.MovePosition( Vector3.MoveTowards(scale1.rigidbody.position, ImbalanceScale1DownPos,  (Time.deltaTime * weightSpeed)));
            scale2.rigidbody.MovePosition( Vector3.MoveTowards(scale2.rigidbody.position, ImbalanceScale2UpPos,  (Time.deltaTime * weightSpeed)));

            if ((Vector3.Distance(scale1.rigidbody.position, ImbalanceScale1UpPos) <= threshHold) && Vector3.Distance(scale2.rigidbody.position, ImbalanceScale2UpPos) <= threshHold)
            {
                if(scaleBalance == ScaleBalance.Left)
                    return;
                OnScaleLeft?.Invoke();
                scaleBalance = ScaleBalance.Left;
                Debug.Log("Left Event Called");
            }
          
        }

        else if(scale2.accumulatedMass > scale1.accumulatedMass)
        {
            Debug.Log("Imbalanced towards Scale 2");
            scale1.rigidbody.MovePosition( Vector3.MoveTowards(scale1.rigidbody.position, ImbalanceScale1UpPos,  (Time.deltaTime * weightSpeed)));
            scale2.rigidbody.MovePosition( Vector3.MoveTowards(scale2.rigidbody.position, ImbalanceScale2DownPos,  (Time.deltaTime * weightSpeed)));

            if ((Vector3.Distance(scale1.rigidbody.position, ImbalanceScale1DownPos) <= threshHold) &&
                Vector3.Distance(scale2.rigidbody.position, ImbalanceScale2DownPos) <= threshHold)
            {
                if(scaleBalance == ScaleBalance.Right)
                    return;
                OnScaleRight?.Invoke();
                scaleBalance = ScaleBalance.Right;
                Debug.Log("Right Event Called");
            }
         
        }

        else
        {
            Debug.Log("Balanced");
            
            scale1.rigidbody.MovePosition( Vector3.MoveTowards(scale1.rigidbody.position, BalanceScale1Pos,  (Time.deltaTime * weightSpeed)));
            scale2.rigidbody.MovePosition( Vector3.MoveTowards(scale2.rigidbody.position, BalanceScale2Pos,  (Time.deltaTime * weightSpeed)));

            
            if ((Vector3.Distance(scale1.rigidbody.position, BalanceScale1Pos) <= threshHold &&
                 Vector3.Distance(scale2.rigidbody.position, BalanceScale2Pos) <= threshHold))
            {
                if(scaleBalance == ScaleBalance.Balance)
                    return;
                OnScaleBalance?.Invoke();
                scaleBalance = ScaleBalance.Balance;
                Debug.Log("Balanced Event Called");
            }
            
        }
    }


    
}
