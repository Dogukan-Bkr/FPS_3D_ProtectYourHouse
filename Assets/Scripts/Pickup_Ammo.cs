using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup_Ammo : MonoBehaviour
{
    string[] weapons = { "Deagle", "Shotgun", "Sniper", "AK47" };
    int[] bulletNumber = { 10, 12, 6, 30 };

    public List<Sprite> weapon_images = new List<Sprite>();
    public Image weaponImages;

    public string random_GunType;
    public int random_BulletNumber;
    public int create_Point;


    void Start()
    {
        int getKey = Random.Range(0, weapons.Length);
        random_GunType = weapons[getKey];
        random_BulletNumber = bulletNumber[Random.Range(0, bulletNumber.Length)];

        weaponImages.sprite = weapon_images[getKey];

        
    }
}
