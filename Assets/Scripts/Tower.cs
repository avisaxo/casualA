using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public GameObject turretGiro;
    public Transform pointFire;
    public Transform pointFire1;
    public ManagerEnemies managerEnemies;
    public GameObject weapon;
    private const string nombreFuncion = "MiFuncionRepetitiva";

    public GameObject bulletPrefab; 
    public Transform shootPoint; 
    public float bulletSpeed = 30f;
    public bool isTowerActive;

    private void Start()
    {
        isTowerActive = true;
        InvokeRepeating(nombreFuncion, 0f, 1f);
    }

    // ----------------------------------------------------------------------

    public void ShootInDirection(Vector3 targetPosition, Vector3 targetPosition1)
    {
        if (isTowerActive)
        {
            GameObject newBulletGO = Instantiate(bulletPrefab, pointFire.position, Quaternion.identity);
            GameObject newBulletGO1 = Instantiate(bulletPrefab, pointFire1.position, Quaternion.identity);

            Vector3 direction = (targetPosition - pointFire.position).normalized;
            Vector3 direction1 = (targetPosition1 - pointFire1.position).normalized;
            weapon.transform.forward = direction;

            Bullet bulletScript = newBulletGO.GetComponent<Bullet>();
            Bullet bulletScript1 = newBulletGO1.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDirectionAndSpeed(direction, bulletSpeed);
                bulletScript1.SetDirectionAndSpeed(direction1, bulletSpeed);
            }
        }
    }
    
    void MiFuncionRepetitiva()
    {
        ShootInDirection(managerEnemies.enemies[0].transform.position, managerEnemies.enemies[1].transform.position);
    }
}
