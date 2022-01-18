using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;
    public Collider GroundCollider;
    public float initialScale = 0.5f;
    Mesh GeneratedMesh;

    private void Start()
    {
        GameObject[] AllObst = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(var obst in AllObst)
        {
            if(obst.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(obst.GetComponent<Collider>(),GeneratedMeshCollider,true);
            }
        }
    }

    
    public void Move(BaseEventData myEvent)
    {
        if(((PointerEventData)myEvent).pointerCurrentRaycast.isValid)
        {
            transform.position = ((PointerEventData)myEvent).pointerCurrentRaycast.worldPosition;
        }
    }
    
    public IEnumerator Scalehole()
    {
        Vector3 StartScale = transform.localScale;
        Vector3 EndScale = StartScale * 2;

        float t = 0;
        while (t <= 0.4f)
        {
            t += Time.deltaTime;
            transform.localScale= Vector3.Lerp(StartScale,EndScale,t);
            yield return null;
        }

    }
    
    private void OnTriggerEnter(Collider other) 
    {
        Physics.IgnoreCollision(other,GroundCollider,true);
        Physics.IgnoreCollision(other,GeneratedMeshCollider,false);
    }

    private void OnTriggerExit(Collider other) 
    {
        Physics.IgnoreCollision(other,GroundCollider,false);
        Physics.IgnoreCollision(other,GeneratedMeshCollider,true);
    }
    private void FixedUpdate() 
    {
        
        if(transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;  
            Make2DHole();
            Make3DColliderMesh();
        }

        //UpdatePos();
        
    }

    private void Make2DHole()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);

        for(int i=0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DColliderMesh()
    {   
        if(GeneratedMesh != null)
        {
            Destroy(GeneratedMesh);
        }
        GeneratedMesh = ground2DCollider.CreateMesh(true,true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;  
    }

    void UpdatePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameObject.transform.position = new Vector3(mousePos.x, 0, mousePos.z);

        print(gameObject.transform.position);
    }
}
