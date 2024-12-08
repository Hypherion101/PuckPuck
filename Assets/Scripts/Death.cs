using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(enemy);
            GameManager.enemiesDefeated += 1;
            Debug.Log("Score: " + GameManager.enemiesDefeated);
        }
    }
}
