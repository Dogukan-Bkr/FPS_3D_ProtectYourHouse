using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Bomb_Box : MonoBehaviour
{
    public List<GameObject> bombBoxPoint = new List<GameObject>();
    public GameObject bombBox;
    public static bool bombBoxStatus;
    public float bombBox_time;

    

    int randomNumber;
    void Start()
    {
        bombBoxStatus = false;
        StartCoroutine(Create_healthBox());

    }

    IEnumerator Create_healthBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(bombBox_time);
            if (!bombBoxStatus) {

                randomNumber = Random.Range(0, 3);
                Instantiate(bombBox, bombBoxPoint[randomNumber].transform.position, bombBoxPoint[randomNumber].transform.rotation);
                bombBoxStatus = true;
            }   

        }
    }
}
