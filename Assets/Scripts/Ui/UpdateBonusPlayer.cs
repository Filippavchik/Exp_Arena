using Assets.Scripts.ConstructorBonusElement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class UpdateBonusPlayer : MonoBehaviour
    {
        //TODO Add branch
        public static Action<int> UpdateStatePlayer { get; set; }
        [SerializeField] private List<Bonus> _CurrentBonusList; //TEST Visibility
        private ControllerBonus _controllerBonus;
        private const int NUMBER_CARD_PER_LEVEL_UPDATE = 3;
        private void Awake()
        {
            _controllerBonus = GetComponent<ControllerBonus>();
        }

        public void  UpdateBonus()
        {
            var UiData = UIData.instanse;
            _CurrentBonusList = _controllerBonus.CurrentBonus;

            for (int i = 0; i < NUMBER_CARD_PER_LEVEL_UPDATE; i++)
            {
                var IndexRandomCard =   UnityEngine.Random.Range(0, _CurrentBonusList.Count - 1);
                UiData.AllLevelUiData[i].NameBonus.text = _CurrentBonusList[IndexRandomCard].NameBonus;
                UiData.AllLevelUiData[i].Description.text = _CurrentBonusList[IndexRandomCard].Description;
                UiData.AllLevelUiData[i].BonusImage.sprite = _CurrentBonusList[IndexRandomCard].BonusImage;
                UiData.AllLevelUiData[i].Frame.sprite = _CurrentBonusList[IndexRandomCard].Frame;

                UiData.AllLevelUiData[i].ElementBonus = _CurrentBonusList[IndexRandomCard];
            }

        }

        public void ClickToCurrentBonus(int BonusID)
        {
            UpdateStatePlayer?.Invoke(BonusID);
        }
    }
}