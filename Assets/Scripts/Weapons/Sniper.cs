using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sniper : MonoBehaviour
{
    Animator animator;
    [Header("Settings")]
    public bool fireStatus;
    float fireFrequency;
    public float generalFireFrequency;
    public float range;
    public GameObject Cross;
    public GameObject Scope;


    [Header("Audios")]
    public AudioSource fireSound;
    public AudioSource changeMagazineSound;
    public AudioSource endOfMagazineSound;
    public AudioSource getAmmoSound;
    [Header("Effects")]
    public ParticleSystem fireEffect;
    public ParticleSystem bulletTrace;
    public ParticleSystem bloodEffect;
    [Header("Others")]
    public Camera myCam;
    float camFieldPov;
    float zoomPov = 10;

    [Header("Weapon Settings")]
    int totalBulletNumber;
    public int magazineCapacity;
    int remainBullet;
    public string weapon_Type;
    public TextMeshProUGUI totalBullet_Text;
    public TextMeshProUGUI remainBullet_Text;
    public float damagePower;

    public bool bulletHive_Status;
    public GameObject bulletHive_Point;
    public GameObject bulletHive;

    public Create_Ammo_Box Create_Ammo_Box_Controller;

    void Start()
    {
        totalBulletNumber = PlayerPrefs.GetInt(weapon_Type + "_Bullet");
        bulletHive_Status = true;
        start_Bullet_Up();
        reloadMainFunction("WriteCanvasString");
        animator = GetComponent<Animator>();
        camFieldPov = myCam.fieldOfView;


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (fireStatus && Time.time > fireFrequency && remainBullet != 0)
            {
                if (!GameController.gameStatus)
                {
                    fire();
                    fireFrequency = Time.time + generalFireFrequency;
                }
            }

            if (remainBullet == 0) { endOfMagazineSound.Play(); }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (remainBullet < magazineCapacity && totalBulletNumber != 0) { animator.Play("changeMagazine"); }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            pickupBullet();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)){
            zoom(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            zoom(false);
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            getAmmo(other.transform.gameObject.GetComponent<Pickup_Ammo>().random_GunType, other.transform.gameObject.GetComponent<Pickup_Ammo>().random_BulletNumber);
            Create_Ammo_Box_Controller.deletePoint(other.transform.gameObject.GetComponent<Pickup_Ammo>().create_Point);
            Destroy(other.transform.parent.gameObject);
        }
        if (other.gameObject.CompareTag("HealthBox"))
        {
            Create_Ammo_Box_Controller.GetComponent<GameController>().healthUp();
            Create_Health_Box.healthBoxStatus = false;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.CompareTag("Bomb_Box"))
        {
            Create_Ammo_Box_Controller.GetComponent<GameController>().getBomb();
            Create_Bomb_Box.bombBoxStatus = false;
            Destroy(other.transform.gameObject);
        }
    }

   


    void fire()
    {
        fireMainFunction();

        RaycastHit hit;

        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, range))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Enemy>().damage(damagePower);
            }
            else if (hit.transform.gameObject.CompareTag("MovingObject"))
            {
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                rb.AddForce(-hit.normal * 50f);
                Instantiate(bulletTrace, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(bulletTrace, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void pickupBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, 2))
        {
            if (hit.transform.gameObject.CompareTag("Bullet"))
            {
                getAmmo(hit.transform.gameObject.GetComponent<Pickup_Ammo>().random_GunType, hit.transform.gameObject.GetComponent<Pickup_Ammo>().random_BulletNumber);
                Create_Ammo_Box.ammoBoxStatus = false;
                Destroy(hit.transform.parent.gameObject);
            }

        }

    }

    void start_Bullet_Up()
    {

        if (totalBulletNumber <= magazineCapacity)
        {
            remainBullet = totalBulletNumber;
            totalBulletNumber = 0;
            PlayerPrefs.SetInt(weapon_Type + "_Bullet", 0);


        }
        else
        {
            remainBullet = magazineCapacity;
            totalBulletNumber = totalBulletNumber - magazineCapacity;
            PlayerPrefs.SetInt(weapon_Type + "_Bullet", totalBulletNumber);

        }
    }

    void reloadMainFunction(string type)
    {
        switch (type)
        {
            case "Ybullet":
                if (totalBulletNumber <= magazineCapacity)
                {
                    int totalValue = remainBullet + totalBulletNumber;
                    if (totalValue > magazineCapacity)
                    {
                        remainBullet = magazineCapacity;
                        totalBulletNumber = totalValue - magazineCapacity;
                        PlayerPrefs.SetInt(weapon_Type + "_Bullet", totalBulletNumber);
                    }
                    else
                    {
                        remainBullet += totalBulletNumber;
                        totalBulletNumber = 0;
                        PlayerPrefs.SetInt(weapon_Type + "_Bullet", 0);
                    }
                }
                else
                {
                    totalBulletNumber -= magazineCapacity - remainBullet;
                    remainBullet = magazineCapacity;
                    PlayerPrefs.SetInt(weapon_Type + "_Bullet", totalBulletNumber);
                }
                totalBullet_Text.text = totalBulletNumber.ToString();
                remainBullet_Text.text = remainBullet.ToString();
                break;
            case "Nbullet":
                if (totalBulletNumber <= magazineCapacity)
                {
                    remainBullet = totalBulletNumber;
                    totalBulletNumber = 0;
                    PlayerPrefs.SetInt(weapon_Type + "_Bullet", 0);
                }
                else
                {
                    totalBulletNumber -= magazineCapacity;
                    PlayerPrefs.SetInt(weapon_Type + "_Bullet", totalBulletNumber);
                    remainBullet = magazineCapacity;
                }
                totalBullet_Text.text = totalBulletNumber.ToString();
                remainBullet_Text.text = remainBullet.ToString();
                break;
            case "WriteCanvasString":
                totalBullet_Text.text = totalBulletNumber.ToString();
                remainBullet_Text.text = remainBullet.ToString();
                break;
        }

    }

    void changeMagazine()
    {
        changeMagazineSound.Play();

        if (remainBullet < magazineCapacity && totalBulletNumber != 0)
        {
            if (remainBullet != 0)
            {
                reloadMainFunction("Ybullet");
            }
            else
            {
                reloadMainFunction("Nbullet");
            }
        }
    }
    void fireMainFunction()
    {
        if (bulletHive_Status)
        {
            GameObject myobject = Instantiate(bulletHive, bulletHive_Point.transform.position, bulletHive_Point.transform.rotation);
            Rigidbody rb = myobject.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f, 1, 0) * 60);
        }
        fireSound.Play();
        fireEffect.Play();
        animator.Play("fire");

        remainBullet--;
        remainBullet_Text.text = remainBullet.ToString();

    }

    void getAmmo(string weaponType, int bulletNumber)
    {

        getAmmoSound.Play();
        switch (weaponType)
        {
            case "AK47":
                PlayerPrefs.SetInt("AK47_Bullet", PlayerPrefs.GetInt("AK47_Bullet") + bulletNumber);
                break;
            case "Sniper":
                totalBulletNumber += bulletNumber;
                PlayerPrefs.SetInt(weaponType + "_Bullet", totalBulletNumber);
                reloadMainFunction("WriteCanvasString");
                break;
            case "Shotgun":
                PlayerPrefs.SetInt("Shotgun_Bullet", PlayerPrefs.GetInt("Shotgun_Bullet") + bulletNumber);
                break;
            case "Deagle":
                PlayerPrefs.SetInt("Deagle_Bullet", PlayerPrefs.GetInt("Deagle_Bullet") + bulletNumber);
                break;
        }
    }

    void zoom(bool status)
    {
        if (status)
        {
            Cross.SetActive(false);
            myCam.cullingMask = ~(1 << 6);
            animator.SetBool("zoom", status);
            myCam.fieldOfView = zoomPov;
            Scope.SetActive(true);
        }
        else
        {
            Scope.SetActive(false);
            myCam.cullingMask = -1;
            animator.SetBool("zoom", status);
            myCam.fieldOfView = camFieldPov;
            Cross.SetActive(true);
        }
    }

}