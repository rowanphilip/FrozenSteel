using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    private List<TankController> all_tanks;
    private List<EnemyController> enemy_tanks;
    private List<FactoryScript> all_factories;

    public EnemyController enemy_tank;
    public FactoryScript start_factory;

    // Use this for initialization
    void Start ()
    {
        this.all_tanks = new List<TankController>();
        this.enemy_tanks = new List<EnemyController>();
        this.all_factories = new List<FactoryScript>();

        this.enemy_tanks.Add(enemy_tank);
        this.all_factories.Add(start_factory);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RegisterNewTank(TankController tank)
    {
        all_tanks.Add(tank);
    }

    void RegisterNewEnemy(EnemyController enemy)
    {
        enemy_tanks.Add(enemy);
    }

    void RegisterNewFactory(FactoryScript factory)
    {
        all_factories.Add(factory);
    }

    public List<TankController> GetAllTanks()
    {
        return all_tanks;
    }

    public List<EnemyController> GetAllEnemies()
    {
        return enemy_tanks;
    }

    public List<FactoryScript> GetAllFactories()
    {
        return all_factories;
    }
}
