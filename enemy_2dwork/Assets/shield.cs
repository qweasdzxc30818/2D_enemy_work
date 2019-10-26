using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    
    void Update()
    {
        shield_1.GetComponent<Transform>().position = Target.GetComponent<Transform>().position;
    }
    public GameObject Target;
    public GameObject shield_1;
}
