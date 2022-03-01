using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject m_gameManager;
    private PlayerVisionRange _visionRange;
    public bool engaged;
    Vector2 previousPosition;
    Vector2 currentPosition;
    private Animator[] anims;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
           _visionRange = GetComponentInChildren<PlayerVisionRange>();
        engaged = false;

        anims = GetComponentsInChildren<Animator>();

        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("isWalking", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;

        setWalkingAnimation();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("entra");
            engaged = true;
            m_gameManager.GetComponent<Game_Manager>().enemyEngaged = true;

            m_gameManager.GetComponent<Game_Manager>().enemyOnCombat = other.gameObject;


        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            engaged = false;

        }
    }

    public bool partyIsMoving()
    {

        if (currentPosition != previousPosition)
        {
            return true;
        }

        return false;
    }

    public void setWalkingAnimation()
    {
        if (partyIsMoving())
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].SetBool("isWalking", true);
            }
        }
        else {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].SetBool("isWalking", false);
            }
        }
    }




}
