using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    UI_HeartsHealth ui_HeartsHealth;
    public HeartsHealthSystem heartsHealthSystem;
    
    bool invencible;

    void Start()
    {
        heartsHealthSystem = new HeartsHealthSystem (3);
        ui_HeartsHealth.SetHeartsHealthSystem (heartsHealthSystem);

        heartsHealthSystem.Damage(6);
    }

    public void Damage (int damageAmount)
    {
        if (!invencible)
        {
            invencible = true;
            StartCoroutine (SetInvencible (false, 1f));
            heartsHealthSystem.Damage(damageAmount);
            CinemachineShake.instance.ShakeCamera(1f, .3f);
            Popup.Create(new Vector3(transform.position.x+.02f, transform.position.y+.12f), "-" + damageAmount.ToString(), false, Color.red);
        }   
    }

    public void Heal (int healAmount)
    {
        heartsHealthSystem.Heal(healAmount);
        Popup.Create(new Vector3(transform.position.x+.02f, transform.position.y+.12f), "+" + healAmount.ToString(), false, Color.green);  
    }

    IEnumerator SetInvencible (bool value, float time)
    {
        yield return new WaitForSeconds(time);
        invencible = value;
    }
    
}
