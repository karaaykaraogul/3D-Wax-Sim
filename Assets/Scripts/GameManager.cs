using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    List<Hair> hairs;
    List<InvertedMask> invertedMasks;
    TransparentArmWax transparentWax;
    OpaqueArmWax opaqueWax;
    MoveStickWithTouch moveStick;
    bool gameHasEnded = false;
    bool checkHairs = true;
    public float restartDelay = 2f;
    private float waxDryDelay = 1.5f;
    void Start()
    {
        gameOverScreen.gameObject.SetActive(false);
        transparentWax = FindObjectOfType<TransparentArmWax>();
        opaqueWax = FindObjectOfType<OpaqueArmWax>();
        moveStick = FindObjectOfType<MoveStickWithTouch>();
    }

    void Update()
    {
        if(!moveStick.hasWaxDried)
        {
            if(checkHairs)
            {
                hairs = FindObjectsOfType<Hair>().ToList();
            }
            
            if(hairs.Count < 10)
            {
                checkHairs = false;
                invertedMasks = FindObjectsOfType<InvertedMask>().ToList();
                foreach(var mask in invertedMasks)
                {
                    Destroy(mask.gameObject);
                }
                foreach(var hair in hairs)
                {
                    Destroy(hair.gameObject);
                }
                transparentWax.gameObject.SetActive(false);
                opaqueWax.GetComponent<MeshRenderer>().enabled = true;
                Invoke("SetWaxStatus", waxDryDelay);
            }
        }
    }

    void SetWaxStatus()
    {
        moveStick.hasWaxDried = true;
    }

    public void EndGame()
    {
        if(!gameHasEnded)
        {
            gameHasEnded = true;
            gameOverScreen.Setup();
            Debug.Log("Bitti");
        }
    }
}
