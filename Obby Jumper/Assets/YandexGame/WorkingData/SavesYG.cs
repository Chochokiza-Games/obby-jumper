
using System;
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        /*        public int money = 1;                       // Можно задать полям значения по умолчанию
                public string newPlayerName = "Hello!";
                public bool[] openLevels = new bool[3];*/

        // Ваши сохранения
        public int money = 0;
        public int power = 10;
        public int bagUpgradesBought = 0;
        public int petDropOrderCurrentId = 0;
        public bool[] openedSkins = new bool[0];
        public bool[] openedAccessories = new bool[0];
        public bool[] openedTrails = new bool[0];
        public int[] petInventoryItems = new int[0];
        public int[] petEggsInventoryItems = new int[0];
        public int[] shopPrices = new int[(int)Store.StoreProducts.Max];
        public int educationPassedCount = 0;
        public bool skinStoreToastEducationShowed = false;
        public bool storeToastEducationShowed = false;
        public int lastSkinId = 0;
        public int lastTrailId = 0;
        public float progressBarAmount = 0;
        public int playerRecord = 0;
        public int level = 1;
        // education
        public bool startEducationCompleted;
        public bool secondLevelEducationCompleted;
        public bool thirdLevelEducationCompleted;

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
 
        }
    }
}
