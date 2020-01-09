using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    // Start is called before the first frame update

    private CharacterController movePlayer;
    private Vector3 direction;
    public float speed = 6.0f;
    public float gravity = 10.0f;
    public float jumpSpeed = 10.0f;
    // Update is called once per frame
    private Vector3 currentPosition;
    private bool canJump;
    public int money;

    public int getAndSetMoney
    {
        get
        {
            return this.money;
        }
        private set
        {
            this.money = value;
        }
    }
    void Start()
    {
        money = 100;
        movePlayer = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (movePlayer.isGrounded)
        {
            currentPosition = this.transform.position;
            canJump = true;
        }


        direction = new Vector3(Input.GetAxis("Horizontal") * speed, -gravity, Input.GetAxis("Vertical") * speed);


        if (Input.GetButton("Jump") && canJump)
        {
            direction.y = jumpSpeed;
            if (this.transform.position.y >= currentPosition.y + 2)
            {
                canJump = false;
            }
        }
        if (!Input.GetButton("Jump"))
        {
            canJump = false;
        }

        movePlayer.Move(direction * Time.deltaTime);

    }

    public int giveMoney(int amountToGive)
    {
        int moneyToGive = 0;
        if (this.getAndSetMoney >= amountToGive)
        {
            this.getAndSetMoney -= amountToGive;
            moneyToGive = amountToGive;
        }
        return moneyToGive;
    }

    public void takeMoney(int amountTotake)
    {
        this.getAndSetMoney += amountTotake;
    }

}
