using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    private List<TankController> all_tanks;
    private List<EnemyController> enemy_tanks;

    public TankController starting_tank_1;
    public TankController starting_tank_2;

    public EnemyController enemy_tank;

    // Use this for initialization
    void Start ()
    {
        this.all_tanks = new List<TankController>();
        this.enemy_tanks = new List<EnemyController>();

        this.all_tanks.Add(starting_tank_1);
        this.all_tanks.Add(starting_tank_2);

        this.enemy_tanks.Add(enemy_tank);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void RegisterNewTank(TankController tank)
    {
        all_tanks.Add(tank);
    }

    void RegisterNewEnemy(EnemyController enemy)
    {
        enemy_tanks.Add(enemy);
    }

    public List<TankController> GetAllTanks()
    {
        return all_tanks;
    }

    public List<EnemyController> GetAllEnemies()
    {
        return enemy_tanks;
    }
}
