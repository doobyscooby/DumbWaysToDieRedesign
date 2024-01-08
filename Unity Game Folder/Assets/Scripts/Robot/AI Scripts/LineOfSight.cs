using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    #region variables
    #region Edittable
    // Colour of Line Of Sight Visualization
    [SerializeField]
    Color sensorColour = Color.green;

    // Line Of Sight Visualization Variables
    [SerializeField]
    float angle = 30;
    [SerializeField]
    float distance = 10;
    [SerializeField]
    float height = 1.0f;
    [SerializeField]
    int segments = 10;

    // Scan for Objects frequency
    [SerializeField]
    int scanFrequency = 30;
    // Layer(s) of Target
    [SerializeField]
    LayerMask targetLayers;
    // Layer(s) that the target cannot be seen through.
    [SerializeField]
    LayerMask blockLayers;
    #endregion

    // List of Objects currently in Line of Sight.
    public List<GameObject> objs = new List<GameObject>();
    // List of Colliders in the surrounding sphere.
    [HideInInspector]
    public Collider[] colliders = new Collider[40];

    // Count of colliders in surrounding sphere - Has its own variable because colliders will have null values
    int count;
    // Interval inbetween each scan
    float scanInterval;
    // Keeps track of next scan time.
    float scanTimer;
    // Our Line of Sight sensors mesh.
    Mesh Sensor;
    // Getter for objects currently in Line of Sight.
    public List<GameObject> Objs { get => objs; }

    [SerializeField]
    Light sensorLight;
    bool active = false;
    #endregion

    #region methods
    private void Awake()
    {
        
        // Set starting scan interval
        scanInterval = 1.0f / scanFrequency;
    }

    public void Activate()
    {
        sensorLight.enabled = true;
        active = true;
    }

    private void Update()
    {
        if (!active)
            return;
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }
    public bool InSight(GameObject obj)
    {
        // Stores the origin, destination and direction.
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        // If the object is above or below our line of sight.
        if (direction.y < 0 || direction.y > height)
            return false;

        // Reset y direction so that our delta angle is not affected
        direction.y = 0;
        // Calculate delta angle.
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        // If object is outside of our line of sight.   
        if (deltaAngle > angle)
            return false;

        // Place origin in the center of our object.
        origin.y += height / 2;
        // Reflect this in our destination.
        destination.y = origin.y;
        // Shoot a linecast from our origin to our destination and check for any block layers.
        if (Physics.Linecast(origin, destination, blockLayers))
            return false;

        // If we have reached this far, the object is in the Line of Sight.
        return true;
    }

    private void Scan()
    {
        // Check in sphere for colliders.
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayers, QueryTriggerInteraction.Collide);

        objs.Clear();
        // For each collider, check if in light of sight and if so, add to list.
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (InSight(obj))
                objs.Add(obj);
        }
    }

    /* Creates our cone mesh
     * This is created in inspector so that we can edit its parameters
     * segments, angle, distance and height.
     */
    Mesh CreateSensorMesh()
    {
        // Create new mesh.
        Mesh mesh = new Mesh();

        // Declare number of triangles and vertices.
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        // Allocate arrays.
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        // Declare preset values.
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int index = 0;

        // Create Left Side of Mesh.
        vertices[index++] = bottomCenter;
        vertices[index++] = bottomLeft;
        vertices[index++] = topLeft;

        vertices[index++] = topLeft;
        vertices[index++] = topCenter;
        vertices[index++] = bottomCenter;

        // Create Right Side of Mesh.
        vertices[index++] = bottomCenter;
        vertices[index++] = topCenter;
        vertices[index++] = topRight;

        vertices[index++] = topRight;
        vertices[index++] = bottomRight;
        vertices[index++] = bottomCenter;

        // Flip our angle and generate our deltaAngle
        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            // Reset our preset values.
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // Create Far Side of Mesh.
            vertices[index++] = bottomLeft;
            vertices[index++] = bottomRight;
            vertices[index++] = topRight;

            vertices[index++] = topRight;
            vertices[index++] = topLeft;
            vertices[index++] = bottomLeft;

            // Create Top Side of Mesh.
            vertices[index++] = topCenter;
            vertices[index++] = topLeft;
            vertices[index++] = topRight;

            // Create Bottom Side of Mesh.
            vertices[index++] = bottomCenter;
            vertices[index++] = bottomRight;
            vertices[index++] = bottomLeft;

            // Increment angle.
            currentAngle += deltaAngle;
        }
        
        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }
        // Pass through our values to our generated mesh.
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    /* Called when value is changed in inspector.
     * Used such that when a value is editted in inspector, this is reflected in scene.
     */
    private void OnValidate()
    {
        Sensor = CreateSensorMesh();
    }
    /* Called when Gizmos are drawn.
     * Used to render our Line of Sight and Spheres representing Targets found.
     */
    private void OnDrawGizmos()
    {
        // If we have generated the mesh
        if (Sensor)
        {
            // Draw mesh
            Gizmos.color = sensorColour;
            Gizmos.DrawMesh(Sensor, transform.position, transform.rotation);
        }

        // Render all spheres nearby
        /*   Gizmos.color = Color.red;
           Gizmos.DrawWireSphere(transform.position, distance);
           for (int i = 0; i < count; ++i)
           {
               Gizmos.DrawSphere(colliders[i].transform.position, 1.0f);
           }

           Gizmos.color = sensorColour;*/

/*        // Render Spheres at all objects within Line of Sight.
        objs.RemoveAll(s => s == null);
        foreach (var Object in objs)
        {
            if (Object == null)
            {
                objs.Remove(Object);
                continue;
            }
          
            Gizmos.DrawSphere(Object.transform.position, 1.0f);
        }*/

    }
    #endregion
}
