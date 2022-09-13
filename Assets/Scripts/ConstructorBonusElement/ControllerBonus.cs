using Assets.Scripts.Ui;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ConstructorBonusElement
{
    public class ControllerBonus : MonoBehaviour
    {
        [SerializeField] private List<Bonus> AllBonus;
        [field: SerializeField] public List<Bonus> CurrentBonus { get; private set; } //TEST Visibility
        [SerializeField] private List<Bonus> RemoveList;//TEST Visibility
        [SerializeField] private int[] CurrentLevelBonus = new int[8];
        const int MAX_BONUS_LEVEL = 4;
        const int CURRENT_INDEX_SPAWN_NEW_CARD = 2;
        public int LengthBranchBonus;

        private void Start()
        {
            InitBonus();
        }

        private void InitBonus()
        {
            AllBonus = UIData.instanse.AllBonus;
            for (int i = 0; i < AllBonus.Count; i++)
            {
                if (AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus == 1)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                }
            }
            RemoveBonus();
        }


        /// <summary>
        /// После выбора текущего навыка идёт обновления текущих рангов.
        /// </summary>
        /// <param name="improvements"> текущий выбранный бонус</param>
        public void BonusSelection(Improvements improvements) //TODO Передать бонус и изменить статы.
        {
            Bonus bonus = null;
            //нужно сравнить текущий  бонус 
            var LevelBonus = CurrentLevelBonus[(int)improvements]++; // повышаем текущий левел выпадения бонуса.

            for (int i = 0; i < AllBonus.Count; i++)
            {
                if (AllBonus[i].SaveCurrentBonus.improvements == improvements && AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus == LevelBonus)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                }
                if (AllBonus[i].SaveCurrentBonus.improvements == improvements && AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus > LevelBonus && LevelBonus != CURRENT_INDEX_SPAWN_NEW_CARD)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                    break;
                }
            }
            for (int i = 0; i < CurrentBonus.Count; i++)
            {
                if (CurrentBonus[i].SaveCurrentBonus.improvements == improvements && CurrentBonus[i].SaveCurrentBonus.RangeToSelectionBonus == LevelBonus - 1)
                {
                    CurrentBonus.Remove(CurrentBonus[i]);
                    break;
                }
            }

            RemoveBonus();
        }
        private void RemoveBonus()
        {
            for (int i = 0; i < RemoveList.Count; i++)
            {
                if (RemoveList[i].SaveCurrentBonus.RangeToSelectionBonus != MAX_BONUS_LEVEL)
                {
                    AllBonus.Remove(RemoveList[i]);
                }
            }
            RemoveList.Clear();
        }
    }
}
