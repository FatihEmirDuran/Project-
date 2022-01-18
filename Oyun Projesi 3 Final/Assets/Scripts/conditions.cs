using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditions : MonoBehaviour
{
    public int Points = 0;
    public OnChangePosition HoleScript;

    private void OnTriggerEnter(Collider other) 
    {
        
        Destroy(other.gameObject);
        ilerleme();
    }

    private void ilerleme()
    {
        Points++;
        if(Points % 10 == 0)
        {
            StartCoroutine(HoleScript.Scalehole());
            
        }
    }
}
