using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();
    public GameObject[] itemTest;
    public InventoryObject m_inventory;

    private GameObject m_gameManager;
    private GameObject m_pointToGo;

    public bool engaged;
    private bool movementAnimSet;

    Vector2 previousPosition;
    Vector2 currentPosition;

    public GFX_MapParty gfxController;
    private SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        movementAnimSet = false;
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");
        engaged = false;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        previousPosition = currentPosition;
        currentPosition = transform.position;
        SetWalkingAnimation();
        //FlipSprites();

        if (Input.GetKeyDown(KeyCode.E)) {
            GameManager.Instance.SetAddingItemsBool(true);
            foreach (GameObject _item in itemTest){
                if (_item){
                    ItemToPick item = _item.GetComponent<ItemToPick>();
                    if (m_inventory.AddItem(new Item(item.item), 1, InventoryType.MAIN)){
                        Destroy(item.gameObject);
                    }
                }
            }
            GameManager.Instance.SetAddingItemsBool(false);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap")) {
            //Debug.Log("entra");
            engaged = true;
            m_gameManager.GetComponent<GameManager>().enemyEngaged = true;

            GameManager.Instance.SetCurrentEnemyID(other.gameObject.GetComponent<PatrolAI>().GetEnemyID());
            //Debug.Log(other.gameObject.GetComponent<PatrolAI>().GetEnemyID());
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

    public bool PartyIsMoving()
    {
        Vector2 playerPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 positionToGo = new Vector2(Mathf.Round(m_pointToGo.transform.position.x), Mathf.Round(m_pointToGo.transform.position.y));

        if (playerPos != positionToGo)
        {
            return true;
        }

        return false;

    }

    public void SetWalkingAnimation()
    {
        if (PartyIsMoving() && !movementAnimSet)
        {
            gfxController.MapMove();
            movementAnimSet = true;
        }
        else if(!PartyIsMoving()) {
            gfxController.MapIdle();
            movementAnimSet = false;
        }
    }

    public void FlipSprites() {
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
