using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] GameObject m_damageText;

    TextMeshPro dmgTxtMesh;
    Color textColor;

    float disappearTime = 1f;
    private void Start()
    {

        if (!CompareTag("CombatManager")) {
            dmgTxtMesh = this.GetComponent<TextMeshPro>();
            textColor = dmgTxtMesh.color;
        }
    }
    public DamagePopUp Create(Vector3 position, int damageAmount) {
        GameObject damageText = Instantiate(m_damageText, position, Quaternion.identity);
        DamagePopUp damagePopUp = damageText.GetComponent<DamagePopUp>();
        damageText.GetComponent<TextMeshPro>().SetText("-" + damageAmount.ToString());
        //dmgTxtMesh = damageText.GetComponent<TextMeshPro>();

        return damagePopUp;
    }


    private void Update()
    {
        float delta = Time.deltaTime;

        float speedY = 0.2f;
        transform.position += new Vector3(0, speedY * delta, 0);

        disappearTime -= delta;
        if (disappearTime < 0) {
            float disapearSpeed = 3f;
            textColor.a -= disapearSpeed * Time.deltaTime;
            dmgTxtMesh.color = textColor;

            if (textColor.a < 0) { Destroy(gameObject); }
        }


    }

}
