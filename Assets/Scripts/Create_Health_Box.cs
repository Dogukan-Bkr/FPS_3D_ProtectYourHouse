using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Health_Box : MonoBehaviour
{
    public List<GameObject> healthBoxPoint = new List<GameObject>();
    public GameObject healthBox;
    public static bool healthBoxStatus;
    public float healthBox_time;

    

    int randomNumber;
    void Start()
    {
        healthBoxStatus = false;
        StartCoroutine(Create_healthBox());

    }

    IEnumerator Create_healthBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthBox_time);
            if (!healthBoxStatus) {

                randomNumber = Random.Range(0, 3);
                Instantiate(healthBox, healthBoxPoint[randomNumber].transform.position, healthBoxPoint[randomNumber].transform.rotation);
                healthBoxStatus = true;
            }

        }
    }
}
