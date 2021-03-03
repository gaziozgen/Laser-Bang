using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab = null;
    [SerializeField] private GameObject laserPreviewPrefab = null;
    [SerializeField] private int previewReflection = 2;

    private bool isLaserSend = false;
    private LaserBangLevel levelManager;
    private List<GameObject> laserPreviews;

    private void Awake()
    {
        levelManager = (LaserBangLevel)LevelManager.Instance;
        laserPreviews = new List<GameObject>();
    }

    void Update()
    {
        if (!isLaserSend)
        {
            CleanPreview();
            UpdateDirection();
            Preview(transform.position, transform.forward, previewReflection);
            CheckShoot();
        }
    }

    private void Shoot(Vector3 pos, Vector3 dir)
    {
        if (Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject laser = Instantiate(laserPrefab, pos, Quaternion.LookRotation(dir));
            laser.LeanScaleZ(hit.distance, 0.1f).setOnComplete(() =>
            {
                Mirror mirror = hit.transform.GetComponent<Mirror>();
                if (mirror)
                {
                    LeanTween.delayedCall(0.1f, () =>
                    {
                        Shoot(hit.point, Vector3.Reflect(dir, hit.normal));
                    });
                }
                else
                {
                    Target target = hit.transform.GetComponent<Target>();
                    LeanTween.delayedCall(0.5f, () =>
                    {
                        levelManager.FinishLevel(target != null);
                    });
                }
            });
        }
    }

    private void CleanPreview()
    {
        for (int i = 0; i < laserPreviews.Count; i++)
        {
            Destroy(laserPreviews[i]);
        }
    }

    private void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CleanPreview();
            isLaserSend = true;
            Shoot(transform.position, transform.forward);
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

    private void Preview(Vector3 pos, Vector3 dir , int n)
    {
        if (Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject laserPreview = Instantiate(laserPreviewPrefab, pos, Quaternion.LookRotation(dir));
            laserPreviews.Add(laserPreview);
            Vector3 newScale = laserPreview.transform.localScale;
            newScale.z = hit.distance;
            laserPreview.transform.localScale = newScale;
            Mirror mirror = hit.transform.GetComponent<Mirror>();
            if (mirror && (n > 0))
            {
                Preview(hit.point, Vector3.Reflect(dir, hit.normal), n-1);
            }
        }
    }
}
