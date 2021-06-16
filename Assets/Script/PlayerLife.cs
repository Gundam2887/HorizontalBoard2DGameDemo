using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    //血量
    public int maxLife = 100;
    public int  life = 100;

    private Animator anim;
    //被击退的速度
    public float repelSpeed;

    public static bool isDie = false;
    //private bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();          
    }

    // Update is called once per frame
    void Update()
    {
        LifeBar.lifeMax = maxLife;
        LifeBar.lifeCurrent = life;
        Die();
    }

    //改变生命值
    public void ChangeLife(int amount)
    {
         life -= amount;
        if(life > maxLife)
        {
            life = maxLife;
        }
        this.GetComponent<Player>().getHit = false;
    }
    
    void Die()
    {
        if(life <= 0)
        {
            anim.SetBool("Die", true);
            Destroy(this.gameObject, 0.6f);
            isDie = true;
        }
    }
}
