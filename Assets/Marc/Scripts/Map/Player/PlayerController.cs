using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject m_gameManager;
    private GameObject m_pointToGo;
    private PlayerVisionRange _visionRange;
    public bool engaged;

    Vector2 previousPosition;
    Vector2 currentPosition;

    private Animator[] anims;
    private SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _visionRange = GetComponentInChildren<PlayerVisionRange>();
        engaged = false;

        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");

        anims = GetComponentsInChildren<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        /*for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("isWalking", false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {

        previousPosition = currentPosition;
        currentPosition = transform.position;
        //setWalkingAnimation();
        flipSprites();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap")) {
            //Debug.Log("entra");
            engaged = true;
            m_gameManager.GetComponent<Game_Manager>().enemyEngaged = true;

            m_gameManager.GetComponent<Game_Manager>().enemyOnCombat = other.gameObject;

            other.gameObject.GetComponent<Collider2D>().enabled = false;
            m_pointToGo.transform.position = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap"))
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
            return;
        }
        else {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].SetBool("isWalking", false);
            }
        }
    }

    public void flipSprites() {
        if (currentPosition.x > previousPosition.x) {
            for (int i = 0; i < sprites.Length; i++) {
                sprites[i].flipX = false;
            }
        }
        else if(currentPosition.x < previousPosition.x) {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].flipX = true;
            }
        }
    }




}
