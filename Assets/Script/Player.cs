using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //获取刚体
    private Rigidbody2D rb;
    //获取速度
    public float speed;
    //获取跳跃速度
    public float jumpforce;
    //获取Animtor
    private Animator anim;
    //获取Layer
    public LayerMask ground;
    //获取碰撞体
    private Collider2D coll;
    //地面检测点
    public Transform groundCheck;
    //判断是否在地面 是否在跳
    public bool isGround, isJump;
    //判断跳跃键是否按下
    bool jumpPressed;
    //记录跳跃剩余次数
    int jumpCount;
    //轻攻击combo计数
    public int LAComboStep;
    //重攻击combo计数
    public int HAComboStep;
    //判断是否在攻击
    private bool isAttack;
    //计时
    private float LAtimer;
    private float HAtimer;
    //允许连续combo时间
    public float interval = 1.2f;
    //区分攻击类型
    public string attackType;
    //攻击补偿速度
    public float lightSpeed;
    public float heavySpeed;
    //被敌人击退的速度
    public float isHitSpeed;
    //轻攻击停顿帧数
    public int lightPause;
    //重攻击停顿帧数
    public int heavyPause;
    //震动
    private Cinemachine.CinemachineCollisionImpulseSource myInpulse;
    //音效
    public AudioSource runAudio,jumpAudio,atkAudio,toGroundAudio,hitAudio;

    public bool getHit;

    private AnimatorStateInfo info;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        myInpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
    }

    void Update()
    {
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !isAttack)
        {
            jumpAudio.Play();
            anim.SetBool("ToGround", false);
            jumpPressed = true;
            //Rigidbody.velocity 刚体的速度矢量
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            anim.SetBool("Jumping", true);
        }
        GetHit();
        Attack();
    }

    void FixedUpdate()
    {
        //检测Player的碰撞体是否碰撞到地面
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        SwitchAnim();
        Jump();
        GroundMove();
    }

    //移动
    public void GroundMove()
    {

        //移动 Horizontal在X轴上移动 Vertical在Y轴上移动
        float horizontalMove = Input.GetAxis("Horizontal");
        //转向 Horizontal在X轴上移动 Vertical在Y轴上移动
        float faceDircetion = Input.GetAxisRaw("Horizontal");
        
        //移动
        if (horizontalMove != 0)
        {
            //Vector3(x轴,y轴，z轴)
            //Time.deltaTime 增量时间 作用：当设备不能流畅运行帧数不足时，使用增量时间可以让游戏运行得更流畅
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            //将浮点值发送到动画器以影响过渡。  返回绝对值。
            anim.SetFloat("Running", Mathf.Abs(faceDircetion));
        }
        if(horizontalMove != 0 && !runAudio.isPlaying && isGround)
        {
            runAudio.Play();
        }
        if (horizontalMove == 0 || !isGround)
        {
            runAudio.Stop();
        }

        //转向
        if (faceDircetion != 0)
        {
            transform.localScale = new Vector3(-faceDircetion, 1, 1);
        }

        //攻击补偿速度
        if (!isAttack)
        {
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        }
        else
        {
            if (attackType == "Light")
            {
                rb.velocity = new Vector2(transform.localScale.x * lightSpeed, rb.velocity.y);
            }
            else if (attackType == "Heavy")
            {
                rb.velocity = new Vector2(transform.localScale.x * heavySpeed, rb.velocity.y);
            }
        }
    }

    //跳跃
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && !isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpCount--;
            jumpPressed = false;
        }
        //二段跳
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    //攻击
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttack)
        {
            atkAudio.Play();
            isAttack = true;
            attackType = "Light";
            LAComboStep++;
            if (LAComboStep > 3) LAComboStep = 1;
            LAtimer = interval;
            anim.SetTrigger("LightAttack");
            anim.SetInteger("LAComboStep", LAComboStep);
        }
        //重攻击
        if (Input.GetKeyDown(KeyCode.K) && !isAttack)
        {
            atkAudio.Play();
            isAttack = true;
            attackType = "Heavy";
            HAComboStep++;
            if (HAComboStep > 3) HAComboStep = 1;
            HAtimer = interval;
            anim.SetTrigger("HeavyAttack");
            anim.SetInteger("HAComboStep", HAComboStep);
        }
        if (LAtimer != 0)
        {
            LAtimer -= Time.deltaTime;
            if (LAtimer <= 0)
            {
                LAtimer = 0;
                LAComboStep = 0;
            }
        }
        if (HAtimer != 0)
        {
            HAtimer -= Time.deltaTime;
            if (HAtimer <= 0)
            {
                HAtimer = 0;
                HAComboStep = 0;
            }
        }
    }
    //结束攻击
    public void AttackOver()
    {
        isAttack = false;
    }

    //攻击判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Parameter parameter = new Parameter();
        if (other.CompareTag("Enemy"))
        {
            if (attackType == "Light")
            {
                myInpulse.GenerateImpulse();
                AttackSense.Instance.HitPause(lightPause);
                hitAudio.Play();
                other.GetComponent<FSM>().parameter.getHit = true;
                other.GetComponent<FSM>().parameter.health -= 10;
            }
            else if (attackType == "Heavy")
            {
                myInpulse.GenerateImpulse();
                AttackSense.Instance.HitPause(heavyPause);
                hitAudio.Play();
                other.GetComponent<FSM>().parameter.getHit = true;
                other.GetComponent<FSM>().parameter.health -= 30;
            }
            //敌人受伤
            if (transform.localScale.x > 0)
            {
                other.GetComponent<FSM>().GetHit(Vector2.right);
            }
            else if (transform.localScale.x < 0)
            {
                other.GetComponent<FSM>().GetHit(Vector2.left);
            }
        }
    }

    //受击判定
    void GetHit()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (getHit)
        {
            anim.SetTrigger("IsHit");
            Debug.Log(getHit);
            if (info.normalizedTime >= 0.01f)
            {
                getHit = false;
                Debug.Log(getHit);
            }
        }
       
    }

    //动画切换
    void SwitchAnim()
    {
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("ToGround", false);
            anim.SetBool("Idle", true);
        }
        else if (!isGround && rb.velocity.y > 0.5f)
        {
            anim.SetBool("Jumping", true);
        }
        else if (!isGround && rb.velocity.y < 0.5f)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("ToGround", true);
            toGroundAudio.Play();
        }
    }
}