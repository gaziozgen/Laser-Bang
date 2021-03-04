using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab = null;
    [SerializeField] private GameObject laserPreviewPrefab = null;
    [SerializeField] private int previewReflection = 1;
    private int localPreviewReflection;

    private LaserBangLevel levelManager;
    private List<GameObject> laserPreviews;

    private void Awake()
    {
        levelManager = (LaserBangLevel)LevelManager.Instance;
        laserPreviews = new List<GameObject>();
        localPreviewReflection = previewReflection;
    }

    public void Shoot(Vector3 pos, Vector3 dir)
    {
        if (Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject laser = Instantiate(laserPrefab, pos, Quaternion.LookRotation(dir));
            laser.LeanScaleZ(hit.distance, 0.1f)
                .setEaseInCubic()
                
                .setOnComplete(() =>
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

    public void CleanPreview()
    {
        for (int i = 0; i < laserPreviews.Count; i++)
        {
            Destroy(laserPreviews[i]);
        }
    }

    public void Preview(Vector3 pos, Vector3 dir)
    {
        if (Physics.Raycast(pos, dir, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject laserPreview = Instantiate(laserPreviewPrefab, pos, Quaternion.LookRotation(dir));
            laserPreviews.Add(laserPreview);
            Vector3 newScale = laserPreview.transform.localScale;
            newScale.z = hit.distance;
            laserPreview.transform.localScale = newScale;
            Mirror mirror = hit.transform.GetComponent<Mirror>();
            if (mirror && (localPreviewReflection > 0))
            {
                localPreviewReflection -= 1;
                Preview(hit.point, Vector3.Reflect(dir, hit.normal));
            } 
            else 
            {
                localPreviewReflection = previewReflection;
            }
        }
    }
}
