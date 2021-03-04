using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool isLaserSend = false;
    private LaserGun laserGun = null;

    void Awake()
    {
        laserGun = transform.GetComponent<LaserGun>();
    }

    void Update()
    {
        if (!isLaserSend)
        {
            laserGun.CleanPreview();
            UpdateDirection();
            laserGun.Preview(transform.position, transform.forward);
            CheckShoot();
        }
    }

    private void UpdateDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitData, 1000))
        {
            Vector3 cameraTarget = new Vector3(hitData.point.x, transform.position.y, hitData.point.z);
            Vector3 direction = (cameraTarget - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            laserGun.CleanPreview();
            isLaserSend = true;
            laserGun.Shoot(transform.position, transform.forward);
        }
    }

}
