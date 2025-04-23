using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Data
{
    public class PlayerStats
    {
        private Dictionary<Stat, StatData> _statDatas;
        private PlayerStats()
        {
            _statDatas = new Dictionary<Stat, StatData>();
            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
                _statDatas.Add(
                    stat, 
                    new StatData(stat, 1));
        }

        public StatData GetStatData(Stat stat)
        {
            return _statDatas[stat];
        }
    }

    public enum Stat
    {
        None = 0,
        Speed = 1,
        ThrowPower = 2,
        Income= 3,
    }
}