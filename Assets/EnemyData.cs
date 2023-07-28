using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public List<MonsterData> dataList;

    [Serializable]
    public class MonsterData
    {
        public string monsterName;
        public int hp;
        public int atk;
        public int def;
        public int exp;

    }
}