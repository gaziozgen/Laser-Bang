using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Collectable : MonoBehaviour
{

    private LaserBangLevel levelManager;
    void Awake()
    {
        levelManager = (LaserBangLevel)LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Laser laser = other.transform.GetComponent<Laser>();
        if (laser)
        {
            Collected();
        }
    }

    private void Collected()
    {
        levelManager.AddScore(1);
        transform.LeanMoveLocalY(2f, 0.3f);
        transform.LeanRotateY(90f, 0.3f).setOnComplete(() =>
        {
            transform.gameObject.SetActive(false);
        });
    }

}
