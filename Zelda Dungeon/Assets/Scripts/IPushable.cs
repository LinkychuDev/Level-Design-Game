using UnityEngine;

public interface IPushable
{
   void Initialize(Transform rb);

   void Cancel();
}