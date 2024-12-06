using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{   
    NavMeshAgent agent;
    GameObject target;
    public float health;
    public float enemyDamagePower;
    GameObject mainController;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mainController = GameObject.FindWithTag("mainController");
    }

    public void findTarget(GameObject myObj)
    {
        target = myObj;
    }

    void Update()
    {
        agent.SetDestination(target.transform.position);
    }

    public void damage(float damagePower)
    {
        health -= damagePower;
        if (health < 0) {
            dead();
            gameObject.tag = "Untagged";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("protect_target")){

            mainController.GetComponent<GameController>().damage(enemyDamagePower);
            dead();
        }
    }
    public void dead()
    {

        mainController.GetComponent<GameController>().update_enemyCount();
        animator.SetTrigger("Dying_Backwards");
        Destroy(gameObject, 5f);
    }
}
