using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //------------------------------------
    //---------NODE--------CLASS----------
    //------------------------------------

    private class Node
    {
        public bool isCliff;
        public Vector3 position;

        private int gCost;
        private int hCost;

        private int fCost;

        //Defines the size in unity's measurement so that a grid of 2x2 can be created (the same size as TerrainTiles)
        private int nodeSizeX;
        private int nodeSizeY;

        private Node sourceNode;

        private Node parentNode;

        public Node(bool isCliff, Vector3 position)
        {
            this.isCliff = isCliff;
            this.position = position;
        }

        void Start()
        {
            nodeSizeX = 2;
            nodeSizeY = 2;
        }

        void Update()
        {
            //gCost = *Distance to source*
            //hCost = *Distance to target*
        }

        int getFCost()
        {
            fCost = gCost + hCost;
            return fCost;
        }

        int getHCost()
        {
            return hCost;
        }

        int getPositionX()
        {
            int posX = position.x;

            return posX;
        }

        int getPositionY()
        {
            int posY = position.y;

            return posY;
        }
    }

    //------------------------------------
    //---------MAIN--------CLASS----------
    //------------------------------------

    //Temporary:
    private Vector3 cliff;
    //

    public Vector3 startPos;
    public Vector3 targetPos;

    private TerrainTile[,] TerrainGrid; //2D array of all tiles on map (each composed of 4 corners)

    public TerrainGenerator tgScript; //get terrain gen script
    public SelectionController scScript; //get selection control script
    public TankController tcScript;

    private int gridXLength;
    private int gridYLength;

    private List<Node> OpenList;
    private List<Node> ClosedList;

    private Node currentNode;

    private bool pathFound;

    private int movementCost;

    //For Node Setup
    private Node[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        startPos = scScript.getSelectedTank().transform.position;
        targetPos = scScript.getPointOfClick();

        gridXLength = 50;
        gridYLength = 50;

        currentNode = findStartNode();

        pathFound = false;
    }

    private void Update()
    {
        //FindPath(StartPosition.position, TargetPosition.position);
    }

    private void SetupGrid() //This method should create a grid of 50 * 50 nodes, this way each node matches up with an existing TerrainTile.
    {
        for (int x = 0; x < gridXLength; x++)
        {
            for (int y = 0; y < gridYLength; y++)
            {
                Vector3 nodePos = new Vector3(x, y, 0);

                if (nodePos != cliff) //cliff still needs to be defined within TerrainGenerator or TerrainTile, this bit of code needs to check whether the map position this new node belongs to is a cliff or not.
                {
                    grid.Add(new Node(false, nodePos));
                }
                else
                {
                    grid.Add(new Node(true, nodePos));
                }
            }
        }
    }

    private Node getNode(int x, int y)
    {
        return grid[x, y];
    }

    private List<Node> getNearNodes()
    {
        //This method should look around the current node and find all nodes in the immediete area
        //It's currently written incredibly inefficiently

        List<Node> nearNodes = new List<>();

        Node northNode = grid[currentNode.getPositionX(), currentNode.getPositionY() - 1];
        Node neNode = grid[currentNode.getPositionX() + 1, currentNode.getPositionY() - 1];
        Node eastNode = grid[currentNode.getPositionX() + 1, currentNode.getPositionY()];
        Node seNode = grid[currentNode.getPositionX() + 1, currentNode.getPositionY() + 1];
        Node southNode = grid[currentNode.getPositionX(), currentNode.getPositionY() + 1];
        Node swNode = grid[currentNode.getPositionX() - 1, currentNode.getPositionY() + 1];
        Node westNode = grid[currentNode.getPositionX() - 1, currentNode.getPositionY()];
        Node nwNode = grid[currentNode.getPositionX() - 1, currentNode.getPositionY() - 1];

        List<Node> nearNodes = new List<>();

        nearNodes.Add(northNode, neNode, eastNode, seNode, southNode, swNode, westNode, nwNode);

        return nearNodes;
    }

    private Node findStartNode() //This method should look around for the selected tank and makes the node that the tank is on the currentNode (as multi-unit selection comes into existence, this will have to be altered)
    {
        foreach (Node n in grid)
        {
            if (Node.position == tcScript.getSelectedTank().transform.position) //This wont work as it will never be the exact location. UPDATE TO: IF TANK WITHIN NODE AREA
            {
                return n;
            }
        }
    }

    private void pathFind() //This should go through each node and find a suitable path accordingly
    {
        OpenList.Add(currentNode);

        currentNode = OpenList[0];

        for (int i = 0; i < OpenList.Count; i++)
        {
            if (OpenList[i].getFCost() >= currentNode.getFCost() || OpenList[i].getHCost() < currentNode.getHCost())
            {
                currentNode = OpenList[i];
            }
        }

        OpenList.Remove(currentNode);
        ClosedList.Add(currentNode);

        if (currentNode.getPosition == targetPos)
        {
            finalPathFound();
        }

        foreach (nearNode n in getNearNodes())
        {
            if (n.isCliff || ClosedList.Contains(n))
            {
                continue;
            }

            //The rest of this method should look at all the nearby nodes and calculate the movement costs for each, look at bellow code for theory on this.

            if (movementCost > n.getGCost() || !OpenList.Contains(n))
            {
                currentNode = n;

                if (!OpenList.Contains(n))
                {
                    OpenList.Add(n);
                }
            }
        }
    }

    void finalPathFound()
    {
        //This method should calculate the final path based on all nodes identified.

        List<Node> finalPath = new List<>();

        Node endNode = currentNode;

        //This needs to be completed
    }


    //------------------------------------
    //---------NOT--------OURS------------
    //------------------------------------


    //Code:https://www.youtube.com/watch?v=AKKpPmxx07w
    //The following methods should be used as theory to base pathfinding
    /**
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = grid.NodeFromWorldPosition(a_StartPos);
        Node TargetNode = grid.NodeFromWorldPosition(a_TargetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
           
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeightborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeightborNode);
                    }
                }
            }

        }
    }

    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while (CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();

        grid.FinalPath = FinalPath;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY = a_nodeB.gridY);

        return iX + iY;
    }

    */





    //Fin


}
