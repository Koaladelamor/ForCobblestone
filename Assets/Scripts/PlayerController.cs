using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();
    public GameObject[] itemTest;
    public InventoryObject m_inventory;

    private GameObject m_gameManager;
    private GameObject m_pointToGo;
    public bool engaged;

    Vector2 previousPosition;
    Vector2 currentPosition;

    public GFXController gfxController;
    private SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start()
    {
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

            GameManager.Instance.SetEnemyOnCombat(other.gameObject);

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

        if (currentPosition != previousPosition)
        {
            return true;
        }

        return false;
    }

    public void SetWalkingAnimation()
    {
        if (PartyIsMoving())
        {
            gfxController.MapMove();
        }
        else {
            gfxController.MapIdle();
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
