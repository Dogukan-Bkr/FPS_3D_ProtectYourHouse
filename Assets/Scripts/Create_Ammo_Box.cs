using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Ammo_Box : MonoBehaviour
{
    public List<GameObject> ammoBoxPoint = new List<GameObject>();
    public GameObject ammoBox;
    public static bool ammoBoxStatus;
    public float ammoBox_time;

    List<int>points = new List<int>();

    int randomNumber;
    void Start()
    {
        ammoBoxStatus = false;
        StartCoroutine(Create_AmmoBox());

    }

    IEnumerator Create_AmmoBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(ammoBox_time);
            randomNumber = Random.Range(0,5);

            if (!points.Contains(randomNumber))
            {
                points.Add(randomNumber);
            }
            else
            {
                randomNumber = Random.Range(0,5);
                continue;
            }
            GameObject myObject = Instantiate(ammoBox, ammoBoxPoint[randomNumber].transform.position, ammoBoxPoint[randomNumber].transform.rotation);
            myObject.transform.gameObject.GetComponentInChildren<Pickup_Ammo>().create_Point = randomNumber;
        }
    }
    public void deletePoint(int value)
    {
        points.Remove(value);
    }
}
