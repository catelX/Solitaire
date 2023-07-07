using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardJump : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 newPos;
    private float nextXdistance;
    public float baseYpoint;

    private Vector3 tempPos;
    private float timer;
    private float duration;
    private float lifeTime;
    public AnimationCurve upCurve;
    private float normalizedTime;

    private bool isJumping;
    private int bounceCount = 6;

    private void Start()
    {
        isJumping = false;
        duration = 2f;
    }

    private void Update()
    {
        if (!isJumping) return;
        Debug.Log(Vector3.Distance(transform.position, newPos));
        if (Vector3.Distance(transform.position, newPos) <= 0.5f)
        {
            Debug.Log("Change");
            SetNextLocation();
        }
        lifeTime += Time.deltaTime;
        if(lifeTime >= 10f)
        {
            gameObject.SetActive(false);
        }
        Move(startPos, newPos);
    }

    private void Move(Vector3 currentPos, Vector3 nextPos)
    {
            timer += Time.deltaTime;
            normalizedTime = timer / duration;
            float strength = upCurve.Evaluate(normalizedTime);
            float x = Mathf.Lerp(currentPos.x, nextPos.x, normalizedTime);
            float y = Mathf.Lerp(currentPos.y, nextPos.y, normalizedTime) + (strength * bounceCount);
            tempPos.x = x;
            tempPos.y = y;
            transform.position = tempPos;
    }

    private void SetNextLocation()
    {
        timer = 0;
        bounceCount--;
        nextXdistance *= 0.7f;
        duration *= 0.7f;
        startPos = transform.position;
        newPos.y = baseYpoint;
        newPos.x += nextXdistance;
    }

    public void ConfigureForJump()
    {
        isJumping = true;
        startPos = transform.position;
        timer = 0;
        float ranX = Random.Range(0.5f, 4f);
        ranX *= GetPositiveOrNegative();
        newPos.y = baseYpoint;
        newPos.x = transform.position.x + ranX;
        nextXdistance = ranX;
    }

    public void SetJump(bool _jump)
    {
        isJumping = _jump;
    }

    private int GetPositiveOrNegative()
    {
        int ran = Random.Range(0, 2);
        if (ran == 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
