using System;
using System.Collections.Generic;
using Enums;

namespace Config
{    
    [Serializable]
    public class LevelConfig
    {
        public string LevelName = "Sector 1";
        public WinConditionSettings WinCondition; 
        public List<PrizeDropConfig> PrizeDrops; 
    }

    [Serializable]
    public class PrizeDropConfig
    {        
        public PrizesType PrizeType;
        public int RequiredHits;
        public bool IsFinalPrize = false; 
    }

    [Serializable]
    public class WinConditionSettings
    { 
        public int RequiredPrizeCount = 5;
        public bool IsVictoryCondition = true; 
    }
}