using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
    public Vector2 targetpos = new Vector2(0, 0);
    public Vector2 currentspeed = new Vector2(5,5);
    public float maxspeed = 1000;
    public float smoothtime = 0.0000001f;
    
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, 0, 3);                            //隨機轉向測試
        Quaternion target = Quaternion.Euler(0, 0, Random.Range(1, 5) * 90);
        Target.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 0.1f);
        Target.position += transform.up * Time.deltaTime * 0.1f;
        
        
    }
        //transform.position = Vector2.SmoothDamp(transform.position, targetpos, ref currentspeed, smoothtime, maxspeed);
}
