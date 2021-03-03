using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            print("asdasd");
            Animate();
        }
    }

    private void Animate()
    {
        transform.LeanMoveLocalY(2f, 0.3f);
        transform.LeanRotateY(90f, 0.3f).setOnComplete(() =>
        {
            transform.gameObject.SetActive(false);
        });
    }

}
