using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveStickWithTouch : MonoBehaviour
{
    SphereCollider coll;
    List<Hair> hairs;
    Vector3 dissolveDirection;
    WaxMarkOnArm armMark;
    public GameObject invertedMaskObj;
    public GameObject collisionCheck;
    public Material material;
    public CameraShake cameraShake;


    public bool hasWaxDried = false;
    public bool isDissolved;
    bool isDissolving;
    public float spawnRate = 10f;
    float nextSpawnTime = 0f;
    float currentX;
    float currentY;
    float cutoffHeight = 3f;
    float cutoffMin;

    void Start()
    {
        coll = collisionCheck.GetComponent<SphereCollider>();
        ColliderBridge cb = coll.gameObject.AddComponent<ColliderBridge>();
        cb.Initialize(this);
        coll.enabled = false;
        hairs = FindObjectsOfType<Hair>().ToList();
        Debug.Log(hairs.Count);
        material.SetFloat("_CutoffHeight", cutoffHeight);
        armMark = FindObjectOfType<WaxMarkOnArm>();
        armMark.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if(!hasWaxDried){
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
                if(Physics.Raycast(touchRay, out RaycastHit touchRaycastHit))
                {
                transform.position = touchRaycastHit.point;
                }

                
            }
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray,100.0f);
            for(int i = 0; i< hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if(hit.collider.tag != "Stick")
                {
                    //Debug.Log(hit.collider.name);
                    transform.position = hit.point;
                
                    if(Time.time >= nextSpawnTime)
                    {
                        if(Input.GetMouseButton(0))
                        {
                            coll.enabled = true;
                            SpawnInvertedMask();
                            nextSpawnTime = Time.time + 1f / spawnRate;
                        }
                    }
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                coll.enabled = false;
            }
        }
        if(hasWaxDried)
        {
            armMark.gameObject.GetComponent<MeshRenderer>().enabled = true;
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray,100.0f);
            for(int i = 0; i< hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if(hit.collider.tag != "Stick")
                {
                    transform.position = hit.point;
                }
            }
            currentX = Mathf.Round(transform.position.x*10)/10;
            currentY = Mathf.Round(transform.position.y*10)/10;
            if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !isDissolved)
            {
                Debug.Log("X: "+ currentX +"Y: "+  currentY);
                if(currentX >= 0.2f && currentY == 1.1f)
                {
                    cutoffHeight = 0.23f;
                    cutoffMin = -0.30f;
                    dissolveDirection.Set(1,0,0);
                    Debug.Log("aga sağdır bu");
                }
                if(currentX > 0.0f && currentX <= 0.1f && currentY >= 1.1f)
                {
                    cutoffHeight = 1.33f;
                    cutoffMin = 0.77f;
                    dissolveDirection.Set(1,1,0);
                    Debug.Log("aga sağ üsttür bu");
                }
                if(currentX >= -0.1f && currentX <= 0.0f && currentY <= 1.1f)
                {
                    cutoffHeight = -0.92f;
                    cutoffMin = -1.12f;
                    dissolveDirection.Set(0,-1,0);
                    Debug.Log("aga alttır bu");
                }
                if(currentX >= -0.1f && currentX <= 0.0f && currentY >= 1.1f)
                {
                    cutoffHeight = 1.22f;
                    cutoffMin = 1.02f;
                    dissolveDirection.Set(0,1,0);
                    Debug.Log("aga üsttür bu");
                }
                if(currentX == -0.2f && currentY >= 1.1f)
                {
                    cutoffHeight = 1.45f;
                    cutoffMin = 0.91f;
                    dissolveDirection.Set(-1,1,0);
                    Debug.Log("aga sol üsttür bu");
                }
                if(currentX <= -0.3f && currentY >= 1.0f && currentY <= 1.1f)
                {
                    cutoffHeight = 0.37f;
                    cutoffMin = -0.13f;
                    dissolveDirection.Set(-1,0,0);
                    Debug.Log("aga soldur bu");
                }
                if(currentX == -0.2f && currentY <= 1.0f)
                {
                    cutoffHeight = -0.67f;
                    cutoffMin = -1.25f;
                    dissolveDirection.Set(-1,-1,0);
                    Debug.Log("aga sol alttır bu");
                }
                if((currentX == 0.1f || currentX == 0.2f) && currentY <= 1.0f)
                {
                    cutoffHeight = -0.8f;
                    cutoffMin = -1.37f;
                    dissolveDirection.Set(1,-1,0);
                    Debug.Log("aga sağ alttır bu");
                }
                isDissolving = true;
            }
        }
        material.SetVector("_DissolveDirection", dissolveDirection);
        if(isDissolving){
           DissolveAction();
        }
    }

    public void OnCollisionEnter(Collision collisionInfo)
    {
        foreach(var hair in hairs)
        {
            if(hair.gameObject == collisionInfo.collider.gameObject)
            {
                hairs.Remove(hair);
                collisionInfo.collider.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnInvertedMask(){
        Instantiate(invertedMaskObj,transform.position,transform.rotation);
    }

    void DissolveAction()
    {
        StartCoroutine(cameraShake.Shake(.15f, .005f));
        cutoffHeight -= (Time.deltaTime/2.5f);
        if(cutoffHeight <= cutoffMin){
            cutoffHeight = cutoffMin;
            isDissolving = false;
            isDissolved = true;
            FindObjectOfType<GameManager>().EndGame();
        }
        material.SetFloat("_CutoffHeight", cutoffHeight);
    }
}
