using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab = null;

    private bool isLaserSend = false;
    private LaserBangLevel levelManager;
    private Mirror mirror;
    private Target target;
    private GameObject laserPreview;
    private GameObject laser;
    private RaycastHit hit;
    private Vector3 direction;
    private Vector3 cameraTarget;
    private Vector3 position;

    private void Awake()
    {
        position = transform.position;
        levelManager = (LaserBangLevel)LevelManager.Instance;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (!isLaserSend)
        {
            UpdateCameraTarget();
            Preview();
            CheckShoot();
        }
    }

    private void Shoot(Vector3 pos, Vector3 dir)
    {
        // atýþ
        if (Physics.Raycast(pos, dir, out hit, Mathf.Infinity))
        {
            laser = Instantiate(laserPrefab, pos, Quaternion.LookRotation(dir));
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, laser.transform.localScale.z * hit.distance);

            // ayna kontrol
            mirror = hit.transform.GetComponent<Mirror>();
            if (mirror)
            {
                // açý ayarý, tekrar atýþ
                LeanTween.delayedCall(0.35f, () =>
                {
                    Shoot(hit.point, Vector3.Reflect(dir, hit.normal));
                });
            } 
            else
            {
                target = hit.transform.GetComponent<Target>();
                if(target)
                {
                    print("win");
                    levelManager.FinishLevel(true);
                }
                else
                {
                    print("lose");
                    levelManager.FinishLevel(false);
                }
            }
        }
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isLaserSend = true;
            Destroy(laserPreview);
            Shoot(position, direction);
        }
    }

    private void UpdateCameraTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, 1000))
        {
            cameraTarget = new Vector3(hitData.point.x, position.y, hitData.point.z);
        }
    }

    private void Preview()
    {
        //Debug.DrawRay(position, direction);

        direction = (cameraTarget - position).normalized;
        if (Physics.Raycast(position, direction, out hit, Mathf.Infinity))
        {
            Destroy(laserPreview);
            laserPreview = Instantiate(laserPrefab, position, Quaternion.LookRotation(direction));
            laserPreview.transform.localScale = new Vector3(laserPreview.transform.localScale.x, laserPreview.transform.localScale.y, laserPreview.transform.localScale.z * hit.distance);
        }
    }
}
