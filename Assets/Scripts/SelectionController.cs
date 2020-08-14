using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public Camera main_camera;

    public float selection_radius;

    public Texture2D StandardCursor;
    public Texture2D SelectCursor;
    public Texture2D AttackCursor;

    public ObjectManager ObjectManager;

    private TankController SelectedTank;
    private bool TankSelected;

    private FactoryScript SelectedFactory;
    private bool FactorySelected;

    public RectTransform selectionBox;
    private Vector2 startPos;

    private List<TankController> selected_tanks;

    public GameObject Indicator;

    // Use this for initialization
    void Start ()
    {
        this.SelectedFactory = null;
        this.SelectedTank = null;
        this.TankSelected = false;
        this.FactorySelected = false;

        Cursor.SetCursor(StandardCursor, Vector2.zero, CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleMouseInput();


        //if statements set the variables required for selection box
        //code: https://www.youtube.com/watch?v=cd7pgnw5OLA
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            releaseSelection();
        }

        if (Input.GetMouseButton(0))
        {
            updateSelection(Input.mousePosition);
        }

        //A map interaction indicator
        /*
        if (tank_selected && Input.GetMouseButton(0))
        {
            Indicator.SetActive(true);
            Indicator.transform.position = getPointOfClick();
        }
        */
    }

    //Allows the creation of selection box
    //code: https://www.youtube.com/watch?v=cd7pgnw5OLA
    private void updateSelection(Vector2 currentMouse)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = currentMouse.x - startPos.x;
        float height = currentMouse.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width),Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    //is called when the selection box is completed, will find all units within the box and select them
    //code: https://www.youtube.com/watch?v=cd7pgnw5OLA
    private void releaseSelection()
    {
        selectionBox.gameObject.SetActive(false);

        List<TankController> all_tanks = ObjectManager.GetAllTanks();

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (TankController tank in all_tanks)
        {
            Vector3 screenPos = main_camera.WorldToScreenPoint(tank.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectSingleTank(tank);
            }
        }
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
            FactoryScript near_factory;

            // If hovering, clicking will select a tank
            if (FindNearTank(point_of_click, out near_tank))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //SelectMultipleTanks(near_tank);
                    SelectSingleTank(near_tank);
                }

                Cursor.SetCursor(SelectCursor, Vector2.zero, CursorMode.Auto);
            }
            else if (FindNearEnemy(point_of_click, out near_enemy))
            {
                if (TankSelected)
                {
                    Cursor.SetCursor(AttackCursor, Vector2.zero, CursorMode.Auto);

                    // Command attack
                    if (Input.GetMouseButtonDown(0))
                    {
                        SelectedTank.SetAttackTarget(near_enemy);
                    }
                }
            }
            else if (FindNearFactory(point_of_click, out near_factory))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SelectSingleBuilding(near_factory);
                }

                Cursor.SetCursor(SelectCursor, Vector2.zero, CursorMode.Auto);
            }
            // If not hovering, clicking will send a command to the selected tank
            else
            {
                if (TankSelected && Input.GetMouseButtonDown(0))
                {
                    SelectedTank.SetTargetLocation(point_of_click);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    UnselectAll();
                }

                Cursor.SetCursor(StandardCursor, Vector2.zero, CursorMode.Auto);
            }
        }

    }

    // Returns the location of the mouse click
    public Vector3 getPointOfClick()
    {
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        Vector3 point_of_click = ray.origin + ray.direction * hit.distance;

        return point_of_click;
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

    private bool FindNearFactory(Vector3 point_of_click, out FactoryScript found_factory)
    {
        found_factory = null;
        bool factory_selected = false;

        List<FactoryScript> all_factories = ObjectManager.GetAllFactories();

        float smallest_distance = selection_radius + 1;

        foreach (FactoryScript factory in all_factories)
        {
            float distance = Vector3.Distance(point_of_click, factory.GetPosition());

            if (distance < selection_radius && distance < smallest_distance)
            {
                smallest_distance = distance;
                found_factory = factory;
                factory_selected = true;
            }
        }

        return factory_selected;
    }

    private void UnselectAll()
    {
        if (TankSelected)
        {
            SelectedTank.UnSelect();
            TankSelected = false;
        }

        if (FactorySelected)
        {
            SelectedFactory.UnSelect();
            FactorySelected = false;
        }
    }

    private void SelectSingleTank(TankController selected_tank)
    {
        // Unselect anything currently selected
        UnselectAll();

        // Select new tank

        // Track selected
        SelectedTank = selected_tank;
        TankSelected = true;

        // Trigger tank selected behaviour
        SelectedTank.Select();
    }

    private void SelectSingleBuilding(FactoryScript selected_factory)
    {
        // Unselect anything currently selected
        UnselectAll();

        // Select new tank

        // Track selected
        SelectedFactory = selected_factory;
        FactorySelected = true;

        // Trigger tank selected behaviour
        SelectedFactory.Select();
    }

    private void SelectMultipleTanks(TankController selected_tank)
    {
        selected_tanks.Add(selected_tank);

        foreach (TankController Tank in selected_tanks)
        {
            Tank.Select();
        }
    }

}
