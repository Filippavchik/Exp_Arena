using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesHolder : MonoBehaviour
{
    public int maxHealthPoints;
    public int ņurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        ņurrentHealth -= damage;
        if (ņurrentHealth <= 0) gameObject.GetComponent<PMoveController>().die.Invoke();
    }
}
