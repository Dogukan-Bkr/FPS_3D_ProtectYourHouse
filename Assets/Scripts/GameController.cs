using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    float health = 100;
    [Header("Health Settings")]
    public Image HealthBar;

    [Header("Weapon Settings")]
    public GameObject[] weapons;
    public AudioSource changeWeapons;
    public GameObject bomb;
    public GameObject bombPoint;
    public GameObject bombCam;

    [Header("Enemy Settings")]
    public GameObject[] enemies;
    public GameObject[] exitPoints;
    public GameObject[] targetPoints;
    public float enemyUpTime;
    public TextMeshProUGUI remainEnemy_text;
    public int starter_enemy_count;
    public static int remain_enemy_count;
    [Header("Other Settings")]
    public GameObject GameOverCanvas;
    public GameObject victoryCanvas;
    public GameObject pauseCanvas;
    public AudioSource gameMusic;
    public TextMeshProUGUI medicBoxNumber_text;
    public TextMeshProUGUI bombNumber_text;


    public static bool gameStatus;
    void Start()
    {
        
        HealthBar.fillAmount = 1f;

        firstStart();
        
        
    }

    IEnumerator enemyTime()
    {
        

        while (true)
        {
            yield return new WaitForSeconds(enemyUpTime);
            if (starter_enemy_count != 0)
            {
                int enemy = Random.Range(0, 4);
                int exitPoint = Random.Range(0, 4);
                int targetPoint = Random.Range(0, 2);

                GameObject obje = Instantiate(enemies[enemy], exitPoints[exitPoint].transform.position, Quaternion.identity);
                obje.GetComponent<Enemy>().findTarget(targetPoints[targetPoint]);
                starter_enemy_count--;
            }
            
                
        }

    }

    void firstStart()
    {
        gameStatus = false;
        if (!PlayerPrefs.HasKey("GameStatus"))
        {
            PlayerPrefs.SetInt("AK47_Bullet", 45);
            PlayerPrefs.SetInt("Sniper_Bullet", 5);
            PlayerPrefs.SetInt("Shotgun_Bullet", 8);
            PlayerPrefs.SetInt("Deagle_Bullet", 20);
            PlayerPrefs.SetInt("MedicBox_Number", 1);
            PlayerPrefs.SetInt("Bomb_Number", 5);
            PlayerPrefs.SetInt("GameStatus", 1);

        }
        remainEnemy_text.text = starter_enemy_count.ToString();
        remain_enemy_count = starter_enemy_count;
        medicBoxNumber_text.text = PlayerPrefs.GetInt("MedicBox_Number").ToString();
        bombNumber_text.text = PlayerPrefs.GetInt("Bomb_Number").ToString();
        gameMusic = GetComponent<AudioSource>();
        gameMusic.Play();
        StartCoroutine(enemyTime());
    }


    public void update_enemyCount()
    {
        remain_enemy_count--;
        if(remain_enemy_count <= 0)
        {
            victoryCanvas.SetActive(true);
            remainEnemy_text.text = "0";
            Time.timeScale = 0;
        }
        else
        {
            remainEnemy_text.text = remain_enemy_count.ToString();
        }
        
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !gameStatus)
        {
            changeWeapon(0);
            

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !gameStatus)
        {
            changeWeapon(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !gameStatus)
        {
            changeWeapon(2);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !gameStatus)
        {
            changeWeapon(3);

        }
        if (Input.GetKeyDown(KeyCode.G) && !gameStatus)
        {
            throwBomb();

        }
        if (Input.GetKeyDown(KeyCode.H) && !gameStatus)
        {
            healthUp();
            
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (!gameStatus))
        {
            Pause();    

        }
    }
    public void damage(float damagePower)
    {
        health -= damagePower;
        HealthBar.fillAmount = health / 100;
        if(health <= 0 )
        {
            GameOver();
        }
    }

   

    void changeWeapon(int number)
    {
        changeWeapons.Play();
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[number].SetActive(true);
    }

  

    public void healthUp()
    {
        if (PlayerPrefs.GetInt("MedicBox_Number") != 0 && health != 100)
        {
            health = 100;
            HealthBar.fillAmount = health / 100;
            PlayerPrefs.SetInt("MedicBox_Number", PlayerPrefs.GetInt("MedicBox_Number") - 1);
            medicBoxNumber_text.text = PlayerPrefs.GetInt("MedicBox_Number").ToString();
        }
        else
        {
            // Maybe can add voice 
        }


        
    }
    public void getHealth()
    {

        PlayerPrefs.SetInt("MedicBox_Number", PlayerPrefs.GetInt("MedicBox_Number") + 1);
        medicBoxNumber_text.text = PlayerPrefs.GetInt("MedicBox_Number").ToString();
    }

    void throwBomb()
    {
        if (PlayerPrefs.GetInt("Bomb_Number") != 0)
        {
            GameObject obj = Instantiate(bomb, bombPoint.transform.position, bombPoint.transform.rotation);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            Vector3 angel = Quaternion.AngleAxis(90, bombCam.transform.forward) * bombCam.transform.forward;
            rb.AddForce(angel * 200f);

            PlayerPrefs.SetInt("Bomb_Number", PlayerPrefs.GetInt("Bomb_Number") - 1);
            bombNumber_text.text = PlayerPrefs.GetInt("Bomb_Number").ToString();
        }
        else
        {
            // Maybe can add voice 
        }
        

    }

    public void getBomb()
    {

        PlayerPrefs.SetInt("Bomb_Number", PlayerPrefs.GetInt("Bomb_Number") + 1);
        bombNumber_text.text = PlayerPrefs.GetInt("Bomb_Number").ToString();

    }
    void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        gameStatus = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        gameStatus = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Pause()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
        gameStatus = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Continue()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
        gameStatus = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
