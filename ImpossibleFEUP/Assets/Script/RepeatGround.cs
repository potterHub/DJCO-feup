using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatGround : MonoBehaviour {
    
    private float groundHorizontalLenght;

    // Use this for initialization
    void Start()
    {
        groundHorizontalLenght = 0;
        foreach (BoxCollider2D g in GetComponentsInChildren<BoxCollider2D>())
            groundHorizontalLenght += g.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= -(groundHorizontalLenght + (groundHorizontalLenght / 2f)))
            repositionBackGround();
        
    }

    public void repositionBackGround()
    {
        transform.position = new Vector2(transform.position.x + (groundHorizontalLenght * 3f - 0.25f), transform.position.y);
    }
}
