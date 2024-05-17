using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapLocation {
  public int x;
  public int y;

  public MapLocation(int _x, int _z) {
    x = _x;
    y = _z;
  }

  public Vector2 ToVector() { return new Vector2(x, y); }

  public static MapLocation operator +(MapLocation a, MapLocation b) =>
      new MapLocation(a.x + b.x, a.y + b.y);

  public override bool Equals(object obj) {
    if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      return false;
    else
      return x == ((MapLocation)obj).x && y == ((MapLocation)obj).y;
  }

  public override int GetHashCode() { return 0; }
}

public class PathMarker {

  public MapLocation location;
  public float G, H, F;
  // public GameObject marker;
  public PathMarker parent;

  public PathMarker(MapLocation l, float g, float h, float f, PathMarker p) {

    location = l;
    G = g;
    H = h;
    F = f;
    // marker = m;
    parent = p;
  }

  public override bool Equals(object obj) {

    if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      return false;
    else
      return location.Equals(((PathMarker)obj).location);
  }

  public override int GetHashCode() { return 0; }
}

public class FindPathAStar : MonoBehaviour {

  public List<MapLocation> directions = new List<MapLocation>() {
    new MapLocation(1, 0),  new MapLocation(1, 1),   new MapLocation(0, 1),
    new MapLocation(-1, 0), new MapLocation(-1, -1), new MapLocation(1, -1),
    new MapLocation(0, -1), new MapLocation(-1, 1)
  };

  private BoardManager maze;
  // public Material closedMaterial;
  // public Material openMaterial;
  // public GameObject start;
  // public GameObject end;
  // public GameObject pathP;

  PathMarker startNode;
  PathMarker goalNode;
  PathMarker lastPos;
  bool done = false;
  public int moveSpeed;
  bool hasStarted = false;
  private GameObject player;
  private bool isRunning = false;
  private float cooldownTimer = 1.0f;
  private List<MapLocation> locations = new List<MapLocation>();

  List<PathMarker> open = new List<PathMarker>();
  List<PathMarker> closed = new List<PathMarker>();
  private List<Vector3> enemyPath = new List<Vector3>();
  private int pathIndex;

  // void RemoveAllMarkers() {
  //
  //   // GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
  //
  //   foreach (GameObject m in markers)
  //     Destroy(m);
  // }

  void BeginSearch() {

    done = false;
    // RemoveAllMarkers();

    MapLocation playerLoc = new MapLocation((int)player.transform.position.x,
                                            (int)player.transform.position.y);
    // locations.Shuffle();

    startNode = new PathMarker(
        new MapLocation((int)transform.position.x, (int)transform.position.y),
        0.0f, 0.0f, 0.0f, null);

    goalNode = new PathMarker(playerLoc, 0.0f, 0.0f, 0.0f, null);

    open.Clear();
    closed.Clear();

    open.Add(startNode);
    lastPos = startNode;
    Debug.Log("Start: " + startNode.location.x + " " + startNode.location.y);
    Debug.Log("Goal: " + goalNode.location.x + " " + goalNode.location.y);
  }

  void Search(PathMarker thisNode) {

    if (thisNode.Equals(goalNode)) {

      done = true;
      Debug.Log("DONE!");
      return;
    }

    foreach (MapLocation dir in directions) {

      MapLocation neighbour = dir + thisNode.location;

      if (maze.map[neighbour.x, neighbour.y] == 1)
        continue;
      if (neighbour.x < 1 || neighbour.x >= maze.boardRows || neighbour.y < 1 ||
          neighbour.y >= maze.boardColumns)
        continue;
      if (IsClosed(neighbour))
        continue;

      float g =
          Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) +
          thisNode.G;
      float h =
          Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
      float f = g + h;

      if (!UpdateMarker(neighbour, g, h, f, thisNode)) {

        open.Add(new PathMarker(neighbour, g, h, f, thisNode));
      }
    }
    open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();
    PathMarker pm = (PathMarker)open.ElementAt(0);
    // Debug.Log("Current: " + pm.location.x + " " + pm.location.y);
    closed.Add(pm);

    open.RemoveAt(0);

    lastPos = pm;
  }

  bool UpdateMarker(MapLocation pos, float g, float h, float f,
                    PathMarker prt) {

    foreach (PathMarker p in open) {

      if (p.location.Equals(pos)) {

        p.G = g;
        p.H = h;
        p.F = f;
        p.parent = prt;
        return true;
      }
    }
    return false;
  }

  bool IsClosed(MapLocation marker) {

    foreach (PathMarker p in closed) {

      if (p.location.Equals(marker))
        return true;
    }
    return false;
  }

  void StartSearch() {
    isRunning = true;
    while (!done)
      Search(lastPos);
    GetPath();
  }

  void Start() {
    maze =
        GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>();
    player = GameObject.FindGameObjectWithTag("Player");
    locations.Clear();
    for (int y = 1; y < maze.boardColumns - 1; ++y) {
      for (int x = 1; x < maze.boardRows - 1; ++x) {

        if (maze.map[x, y] != 1) {

          locations.Add(new MapLocation(x, y));
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject == player) {
      hasStarted = true;
    }
  }
  private void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject == player) {
      hasStarted = false;
    }
  }

  void GetPath() {
    enemyPath.Clear();
    PathMarker begin = lastPos;
    while (!startNode.Equals(begin) && begin != null) {
      enemyPath.Add(new Vector3(begin.location.x, begin.location.y, 0));
      begin = begin.parent;
    }
    pathIndex = enemyPath.Count - 1;
  }

  void Update() {

    cooldownTimer -= Time.deltaTime;
    if (cooldownTimer <= 0.0f && hasStarted) {
      isRunning = false;
      BeginSearch();
      // Debug.Log("Searching...");
      cooldownTimer = 1.0f;
    }

    if (hasStarted && !isRunning) {
      StartSearch();
    } else if (hasStarted) {

      if (pathIndex >= 0) {
        transform.position =
            Vector3.MoveTowards(transform.position, enemyPath[pathIndex],
                                moveSpeed * Time.deltaTime);
        if (transform.position == enemyPath[pathIndex]) {
          pathIndex -= 1;
        }
      }
    }
  }
}
