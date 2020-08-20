using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float constantMovementSpeed = 0.005f;
    public float controlMovementSpeedModifier = 10f;
    [SerializeField] public float waveSpeedModificator = 1f;
    [SerializeField] public float waveLifeModificator = 1f;
    [SerializeField] public float waveSpawnRate = 1.5f;

    private bool isAnim = false;
    
    [SerializeField] private float lastSpawnTime = 0f;
    private Vector3 lastMousePosition;
    private Vector3 playerMovement;

    private float deltaX, deltaY;
    private Rigidbody2D rb;
    
        
    private float? lastMousePointX = null;
    private float? lastMousePointY = null;

    private int isSpawnBigWave = 0;
    public GameObject sparkle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boost"))
            SpawnBigWave();

        if (other.gameObject.CompareTag("Chaser"))
        {
            Instantiate(sparkle, transform);
            GetComponent<SpriteRenderer>().enabled = false;
            SpawnBigWave();
        }
        
    }


    void Update()
    {
        /*if ((transform.position - lastMousePosition).magnitude < 70)
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); 
            playerMovement = mousePos- lastMousePosition;
            lastMousePosition = mousePos;
            transform.position += playerMovement;
        }*/
        
                
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePointX = Input.mousePosition.x;
            lastMousePointY = Input.mousePosition.y;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(GetComponentsInChildren<SpriteRenderer>()[1].DOFade(1, 0.3f));

        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastMousePointX = null;
            lastMousePointY = null;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(GetComponentsInChildren<SpriteRenderer>()[1].DOFade(0, 0.3f));
        }
        if (lastMousePointX != null || lastMousePointY != null)
        {
            float differenceX = Input.mousePosition.x - lastMousePointX.Value;
            float differenceY = Input.mousePosition.y - lastMousePointY.Value;
            
            transform.position = new Vector3(transform.position.x + (differenceX / 188) , transform.position.y+ (differenceY / 188), transform.position.z);
            lastMousePointX = Input.mousePosition.x;
            lastMousePointY = Input.mousePosition.y;
        }

        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {

                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    break;

                case TouchPhase.Moved:
                    transform.position += new Vector3(touchPos.x - deltaX, touchPos.y - deltaY, 0);//rb.MovePosition(new Vector2(touchPos.x -deltaX, touchPos.y - deltaY));
                    break;

                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }*/

        /*if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            deltaX = touchPos.x - transform.position.x;
            deltaY = touchPos.y - transform.position.y;
            //Debug.Log("MOUSE " +touchPos + " " +deltaX + " " + deltaY);
            
            if(Input.GetAxis("Mouse X")!= 0 || Input.GetAxis("Mouse Y") != 0){
                Vector3 touchPos2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                Debug.Log("MOUSE MOVE! " + (touchPos2.x - deltaX));
                transform.position = new Vector3(touchPos2.x - deltaX, touchPos2.y - deltaY, 0);//rb.MovePosition(new Vector2(touchPos.x -deltaX, touchPos.y - deltaY));

            }
            
        }*/
        
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {

                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    break;

                case TouchPhase.Moved:
                    
                    transform.position = new Vector3(touchPos.x - deltaX, touchPos.y - deltaY, 0);//rb.MovePosition(new Vector2(touchPos.x -deltaX, touchPos.y - deltaY));
                    break;

                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }

*/

        //transform.position += Vector3.up * constantMovementSpeed;
        
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up *  (constantMovementSpeed * controlMovementSpeedModifier);
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.down *  (constantMovementSpeed * controlMovementSpeedModifier);
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right *  (constantMovementSpeed * controlMovementSpeedModifier);
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left *  (constantMovementSpeed *  controlMovementSpeedModifier);

        if (Time.time > lastSpawnTime)
        {
            if (isSpawnBigWave == 0)
            {
                Anim();
                SpawnWave();
            }
            else
            {
                isSpawnBigWave--;
            }
            
            lastSpawnTime += waveSpawnRate;
        }
            
        
    }
    
    public void Anim()
    {
        
        Sequence mySequence = DOTween.Sequence();
        //Instantiate(sparkleTouchEffectPrefab, transform);
        //mySequence.Append(transform.DOScale(0.35f, 0.1f)).Append(transform.DOScale(0.30f, 0.1f));

        if (!isAnim)
        {
            Vector3 startScale = transform.localScale;
            isAnim = true;
            
            //Debug.Log(wb.transform.position + " " +wb.transform.position.normalized);
            //mySequence.Append(transform.DOMove(startPosition - wb.transform.position.normalized * 0.1f, 0.25f)).
            mySequence.Append(transform.DOScale(new Vector3(0.35f, 0.35f, 1f), 0.25f)).
                Append(transform.DOScale(startScale, 0.25f)).OnComplete(() => { isAnim = false; });;
            

        }

    }
    
    public void SpawnWave()
    {
        
        var waveBehaviour = LevelController.Instance.CreateWave(transform.position, transform);
        waveBehaviour.speed *= waveSpeedModificator;
        waveBehaviour.lifeTime *= waveLifeModificator;
    }
    
    public void SpawnBigWave()
    {
        var waveBehaviour = LevelController.Instance.CreateBoostWave(transform.position, transform, 5);
        waveBehaviour.speed *= waveSpeedModificator;
        waveBehaviour.lifeTime = 3;
        isSpawnBigWave = 2;
    }
}
