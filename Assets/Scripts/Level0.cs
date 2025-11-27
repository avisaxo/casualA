using System.Collections.Generic;
using UnityEngine;

public class Level0 : MonoBehaviour
{
    public Transform initPosition;
    public Transform finalPosition;
    public Transform positionEnemy;
    public GameObject ProgresBar1;
    public GameObject ProgresBar2;
    public GameObject auxProgresBar = null;
    public GameObject auxProgresBar1 = null;
    public ManagerEnemies managerEnemies;
    public List<GameObject> missilesPoint;
    public List<GameObject> missilesPointsTarguet;
    public GameObject MissilePrefab;
    public GameObject explosion;
    public GameObject obstacleBrick;
    public GameManager gameManager;

    public void CreateProgresBar()
    {
        if (auxProgresBar == null)
        {
            auxProgresBar = Instantiate(ProgresBar1, transform);
            auxProgresBar.GetComponent<ProgressCircular>().gameManager = gameManager;
            auxProgresBar.GetComponent<ProgressCircular>().managerEnemies = managerEnemies;
        }
    }
    
    public void CreateProgresBar1()
    {
        if (auxProgresBar1 == null)
        {
            auxProgresBar1 = Instantiate(ProgresBar2, transform);
            auxProgresBar1.GetComponent<ProgressCircular>().gameManager = gameManager;
            auxProgresBar1.GetComponent<ProgressCircular>().managerEnemies = managerEnemies;
        }
    }

    public void FireMissileToTarguet()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject auxMissile = Instantiate(MissilePrefab);
            auxMissile.transform.position = missilesPoint[i].transform.position;
            auxMissile.GetComponent<ParabolicMover>().label = this;
            auxMissile.GetComponent<ParabolicMover>().SetRarguetPosition(missilesPointsTarguet[i].transform);
            auxMissile.GetComponent<ParabolicMover>().Launch();
        }
    }

    public void CreateExplocionMissile(Transform positionMissile)
    {
        Debug.Log("Create Explocion misile");
        GameObject auxExplocion = Instantiate(explosion);
        auxExplocion.transform.position = positionMissile.position;
    }
}
