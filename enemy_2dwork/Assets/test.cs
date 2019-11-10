using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Quaternion Ta;
    public Transform self;
    int a = 20;

    void Start()
    {
        StartCoroutine(angle());
        
    }
    private IEnumerator angle()
    {
        while(a  > 0){
            Ta = Quaternion.Euler(0, 0, Random.Range(1, 5) * 90);
            yield return new WaitForSeconds(3f);
            a--;

        }
    }


    void Update()
    {
        Vector3 vec = new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.rotation.z), Mathf.Cos(Mathf.Deg2Rad * transform.rotation.z), 0);
        transform.rotation = Quaternion.Slerp(self.rotation, Ta, 0.1f);
        transform.Translate(vec * Time.deltaTime , 0);
        //Debug.Log("方形");
        //Debug.Log(self.rotation);
        //Debug.Log(Ta);
    }
}
