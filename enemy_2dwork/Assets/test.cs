using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Quaternion Ta;
    public Transform self;
    int a = 20;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(self.rotation, Ta, 0.1f);
        transform.Translate(Vector3.forward*Time.deltaTime, 0);
    }
}
