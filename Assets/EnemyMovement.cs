using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static EnemyData;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyMovement : MonoBehaviour
{
    public EnemyData MonsterData;
    public string monsterName;
    public int hp;
    public int atk;
    public int def;
    public int exp;

    
    

    public Tilemap tilemap;
    public float moveDistance = 1f;
    public PlayerMovement player;
    public MessageController messageController;


    private bool isMoving = false;
    private Vector3 targetPosition;

    public AudioClip enemyAttackSound;  // 攻撃音
    public AudioSource audioSource;


    //【追加】TryMoveをカウントする。
    private int counter = 0;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
           monsterName = MonsterData.dataList[0].monsterName;
            hp = MonsterData.dataList[0].hp;
            atk = MonsterData.dataList[0].atk;
            def = MonsterData.dataList[0].def;
            exp = MonsterData.dataList[0].exp;
    }


    //【追加】UpdateでTryMoveカウント数を実行
    void Update()
    {
        //移動中は移動処理をしないようにする
       // if (isMoving) return; ←不要??

        //TryMoveが呼び出された数が０以上
        if (counter > 0)
        {
            //真の移動処理
            TruthMove();

            //カウントを減らす
            counter--;
        }
    }


    //【改造】カウントだけするように変更
    public void TryMove()
    {
        //カウントする
        counter++;
    }


    //【改造】カウント数を見て呼び出される
    private void TruthMove()
    {
     
 

        Vector3Int enemyCell = tilemap.WorldToCell(transform.position);
        Vector3Int diff = player.targetCell - enemyCell;

        // 斜め移動を試みる
        if (Mathf.Abs(diff.x) >= 1 && Mathf.Abs(diff.y) >= 1)
        {
            Vector3Int direction = new Vector3Int((int)Mathf.Sign(diff.x), (int)Mathf.Sign(diff.y), 0);
            Vector3Int targetCell = enemyCell + direction;
            if (targetCell == player.targetCell)
            {
                AttackPlayer();
                return;
            }
            else if (player.IsWalkableTile(targetCell))
            {
                StartCoroutine(MoveToCell(targetCell));
                return;
            }
        }

        // 斜めに移動できなければ、横または縦に移動を試みる
        if (diff.x != 0)
        {
            Vector3Int targetCell = enemyCell + new Vector3Int((int)Mathf.Sign(diff.x), 0, 0);
            if (targetCell == player.targetCell)
            {
                AttackPlayer();
                return;
            }
            else if (player.IsWalkableTile(targetCell))
            {
                StartCoroutine(MoveToCell(targetCell));
                return;
            }
        }

        if (diff.y != 0)
        {
            Vector3Int targetCell = enemyCell + new Vector3Int(0, (int)Mathf.Sign(diff.y), 0);
            if (targetCell == player.targetCell)
            {
                AttackPlayer();
                return;
            }
            else if (player.IsWalkableTile(targetCell))
            {
                StartCoroutine(MoveToCell(targetCell));
                return;
            }
        }
    }

    private void EnemyTurn()
    {//削除するかも
        TryMove();
    }

    public void AttackPlayer()
    {
        // プレイヤーを攻撃する処理
        int playerdef = player.def;
        float randomFactor = UnityEngine.Random.Range(-0.1f, 0.1f);
        float baseDamage = atk / Mathf.Pow(2, playerdef / 10);
        int damage = (int)Math.Floor(baseDamage + (baseDamage * randomFactor));

        //Debug.Log("攻撃されたよ");
        messageController.ShowMessage($"{monsterName}から{damage}ダメージを受けた");
        player.hp -= damage;
        audioSource.PlayOneShot(enemyAttackSound);
        player.isPlayerTurn = true;
    }

    private IEnumerator MoveToCell(Vector3Int targetCell)
    {
        isMoving = true;
        targetPosition = tilemap.GetCellCenterWorld(targetCell);

        // 移動アニメーションなどの処理を追加する場合はここに記述

        // 移動
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}