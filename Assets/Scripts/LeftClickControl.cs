using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftClickControl : MonoBehaviour {

    public Camera main_camera;

    public float selection_radius;

    public Texture2D StandardCursor;
    public Texture2D SelectCursor;
    public Texture2D AttackCursor;

    public ObjectManager ObjectManager;

    private TankController ControlledTank;
    private bool UnitSelected;

	// Use this for initialization
	void Start ()
    {
        this.ControlledTank = null;
        this.UnitSelected = false;

        Cursor.SetCursor(StandardCursor, Vector2.zero, CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleMouseInput();
	}

    private void HandleMouseInput()
    {
        // Use ray casting to find click location
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Vector3 point_of_click = ray.origin + ray.direction * hit.distance;

            TankController near_tank;
            EnemyController near_enemy;

            // If hovering, clicking will select a tank
            if (FindNearTank(point_of_click, out near_tank))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // If tank already selected, first unselect it
                    if (UnitSelected)
                    {
                        ControlledTank.UnSelect();
                    }

                    // Select the new tank
                    ControlledTank = near_tank;
                    UnitSelected = true;
                    ControlledTank.Select();
                }

                Cursor.SetCursor(SelectCursor, Vector2.zero, CursorMode.Auto);
            }
            else if (FindNearEnemy(point_of_click, out near_enemy))
            {
                if (UnitSelected)
                {
                    Cursor.SetCursor(AttackCursor, Vector2.zero, CursorMode.Auto);

                    // Command attack
                    if (Input.GetMouseButtonDown(0))
                    {
                        ControlledTank.SetAttackTarget(near_enemy);
                    }
                }
            }
            // If not hovering, clicking will send a command to the selected tank
            else
            {
                if (UnitSelected && Input.GetMouseButtonDown(0))
                {
                    ControlledTank.SetTargetLocation(point_of_click);
                }

                Cursor.SetCursor(StandardCursor, Vector2.zero, CursorMode.Auto);
            }
        }

    }

    // Selects
    private bool FindNearTank(Vector3 point_of_click, out TankController found_tank)
    {
        // Set default search results
        found_tank = null;
        bool tank_selected = false;

        List<TankController> all_tanks = ObjectManager.GetAllTanks();

        float smallest_distance = selection_radius + 1;

        foreach (TankController tank in all_tanks)
        {
            float distance = Vector3.Distance(point_of_click, tank.GetPosition());

            // Tank must be the closest to the cursor so far and fall within the selection radius
            if (distance < selection_radius && distance < smallest_distance)
            {
                smallest_distance = distance;
                found_tank = tank;
                tank_selected = true;
            }
        }

        return tank_selected;
    }

    // Selects
    private bool FindNearEnemy(Vector3 point_of_click, out EnemyController found_enemy)
    {
        // Set default search results
        found_enemy = null;
        bool enemy_selected = false;

        List<EnemyController> all_enemies = ObjectManager.GetAllEnemies();

        float smallest_distance = selection_radius + 1;

        foreach (EnemyController enemy in all_enemies)
        {
            float distance = Vector3.Distance(point_of_click, enemy.GetPosition());

            // Tank must be the closest to the cursor so far and fall within the selection radius
            if (distance < selection_radius && distance < smallest_distance)
            {
                smallest_distance = distance;
                found_enemy = enemy;
                enemy_selected = true;
            }
        }

        return enemy_selected;
    }
}
