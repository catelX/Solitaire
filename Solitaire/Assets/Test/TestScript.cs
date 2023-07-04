using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public List<Sprite> sprites;
    [SerializeField] private SpriteRenderer spRen;
    private int spriteCount;
    private int currentSpriteIndex=0;
    private Vector3 tempPos;
    private Vector3 startingPos;
    private Vector3 tempScale;
    private Vector3 startingScale;

    public float randomNessFactor = 2.5f;

    void Start()
    {
        spriteCount = sprites.Count;
        startingPos = transform.position;
        tempScale = transform.localScale;
        startingScale = tempScale;
        tempPos = startingPos;
    }
    private float timer;
  
    void Update()
    {
        timer += Time.deltaTime;
        tempPos.y += 0.1f * Random.Range(1, randomNessFactor);
        transform.position = tempPos;
        tempScale.x += 0.1f*Random.Range(1,randomNessFactor);
        tempScale.y += 0.1f * Random.Range(1, randomNessFactor);
        transform.localScale = tempScale;
        if (timer>0.35f)
        {
            tempPos = startingPos;
            tempScale = startingScale;
            transform.position = tempPos;
            transform.localScale = tempScale;
            timer = 0f;
            spRen.sprite = sprites[currentSpriteIndex];
            currentSpriteIndex++;
            if(currentSpriteIndex>=spriteCount)
            {
                currentSpriteIndex = 0;
            }
        }
    }
}
