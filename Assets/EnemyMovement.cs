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

    public AudioClip enemyAttackSound;  // �U����
    public AudioSource audioSource;


    //�y�ǉ��zTryMove���J�E���g����B
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


    //�y�ǉ��zUpdate��TryMove�J�E���g�������s
    void Update()
    {
        //�ړ����͈ړ����������Ȃ��悤�ɂ���
       // if (isMoving) return; ���s�v??

        //TryMove���Ăяo���ꂽ�����O�ȏ�
        if (counter > 0)
        {
            //�^�̈ړ�����
            TruthMove();

            //�J�E���g�����炷
            counter--;
        }
    }


    //�y�����z�J�E���g��������悤�ɕύX
    public void TryMove()
    {
        //�J�E���g����
        counter++;
    }


    //�y�����z�J�E���g�������ČĂяo�����
    private void TruthMove()
    {
     
 

        Vector3Int enemyCell = tilemap.WorldToCell(transform.position);
        Vector3Int diff = player.targetCell - enemyCell;

        // �΂߈ړ������݂�
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

        // �΂߂Ɉړ��ł��Ȃ���΁A���܂��͏c�Ɉړ������݂�
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
    {//�폜���邩��
        TryMove();
    }

    public void AttackPlayer()
    {
        // �v���C���[���U�����鏈��
        int playerdef = player.def;
        float randomFactor = UnityEngine.Random.Range(-0.1f, 0.1f);
        float baseDamage = atk / Mathf.Pow(2, playerdef / 10);
        int damage = (int)Math.Floor(baseDamage + (baseDamage * randomFactor));

        //Debug.Log("�U�����ꂽ��");
        messageController.ShowMessage($"{monsterName}����{damage}�_���[�W���󂯂�");
        player.hp -= damage;
        audioSource.PlayOneShot(enemyAttackSound);
        player.isPlayerTurn = true;
    }

    private IEnumerator MoveToCell(Vector3Int targetCell)
    {
        isMoving = true;
        targetPosition = tilemap.GetCellCenterWorld(targetCell);

        // �ړ��A�j���[�V�����Ȃǂ̏�����ǉ�����ꍇ�͂����ɋL�q

        // �ړ�
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}