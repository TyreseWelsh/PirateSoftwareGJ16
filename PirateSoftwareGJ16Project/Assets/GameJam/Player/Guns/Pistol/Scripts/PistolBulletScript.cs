using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PistolBulletScript : MonoBehaviour
{
    [SerializeField] private ProjectileScriptableObject projectileData;
    
    private Transform bulletTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        bulletTransform.Translate(projectileData.moveSpeed * Time.deltaTime * bulletTransform.forward);
    }
}
