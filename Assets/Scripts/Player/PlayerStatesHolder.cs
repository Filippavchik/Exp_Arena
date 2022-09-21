using Assets.Scripts.ConstructorBonusElement;
using InfimaGames.LowPolyShooterPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
public class PlayerStatesHolder : MonoBehaviour
{
    public int maxHealthPoints = 3;
    public int currentHealth = 3;
    public static Action EventMinusPlayerHp { get; set; }
    public static Action<bool> EventToDeadPlayer { get; set; }
    public UnityEvent recieveDamage;
    public GameObject _camera;
    private Vignette vignette;
    private Bloom bloom;
    public static List<Bonus> currentBonus;
    public GameObject Sphere;
    public static float distancetoPlayer;
    public static float enemyDrag = 100;
    /// <summary>
    Movement movement;
    PShootingController shootingController;
    List<Bonus> bonusBuffer;
    Event sphereExpansion;
    /// </summary>
    // Start is called before the first frame update
    private void Awake()
    {
        currentBonus = new List<Bonus>();
        bonusBuffer = currentBonus;
    }
    void Start()
    {
        //        vignette = _camera.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();

        _camera.GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);
        _camera.GetComponent<PostProcessVolume>().profile.TryGetSettings(out bloom);
        movement = gameObject.GetComponent<Movement>();
        shootingController = gameObject.GetComponent<PShootingController>();
        currentHealth = maxHealthPoints;
    }

    public void UpgradeMovement(float Coefficient)
    {
        
        movement.speedWalking *= Coefficient;

    }
    public void UpgradeRunning(float Coefficient)
    {
        movement.speedRunning *= Coefficient;
    }
    public void UpgradeJummp(float Coefficient)
    {
        gameObject.GetComponent<PMoveController>().jumpHeigth *= Coefficient;
        gameObject.GetComponent<PMoveController>().jumpChargeTime *= Coefficient;
    }
    public void UpgradeDamage(float Coefficient)
    {
        shootingController.damage = (int)(shootingController.damage * Coefficient);
    }
    public void UpgradeShootSpeed(float Coefficient)
    {
        shootingController.weapon.GetComponent<Weapon>().roundsPerMinutes = (int)(shootingController.weapon.GetComponent<Weapon>().roundsPerMinutes* Coefficient);
    }
    public void UpgradeBulletSpeed(float Coefficient)
    {
        shootingController.BulletSpeed *= Coefficient;
    }
    public void UpgradeCriticalDamage(float Coefficient)
    {
        shootingController.DamagePlayer = (int)(shootingController.DamagePlayer*Coefficient);
    }
    public void UpgradeSphere(float _minTime, float _maxTime)
    {
        timeBuffer = 0;
        maxTime = _maxTime;
        minTime = _minTime;
    }
    public void UpgradeCollapse(float Coefficient)
    {
        collapsePow *= Coefficient;

    }
    public void UpgradeEnemyDrag(float Coefficient)
    {
        enemyDrag /= Coefficient;
    }
    float timeBuffer = -1;
    float minTime = 8;
    float maxTime = 11;
    float collapsePow = 2;
    public void SphereCollaps()
    {
        if (timeBuffer >= minTime && timeBuffer <= maxTime)
        {
            timeBuffer += Time.deltaTime * 10;
            distancetoPlayer = timeBuffer + 1;
            Sphere.transform.localScale = new Vector3(Mathf.Pow(1+ timeBuffer-minTime, collapsePow), Mathf.Pow(1 + timeBuffer - minTime, collapsePow), Mathf.Pow(1 + timeBuffer - minTime, collapsePow));
        }
        else if(timeBuffer > -1 && timeBuffer <= maxTime)
        {
            timeBuffer += Time.deltaTime;
        }
        else if(timeBuffer > -1)
        {
            timeBuffer = 0;
            distancetoPlayer = 1;
            Sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
    // Update is called once per frame

    void Update()
    {
        if (bonusBuffer != currentBonus)
        {
            BonusApply(currentBonus[currentBonus.Count - 1]);
            bonusBuffer = currentBonus;
        }

        if (currentHealth <= 0) { gameObject.GetComponent<PMoveController>().die.Invoke(); }
        if (vignette.opacity < 0)
        {
            vignette.intensity.value = 0f;
            bloom.intensity.value = 0;
        }
        else
        {
            vignette.intensity.value -= Time.deltaTime / 5;
            bloom.intensity.value = 10 * (vignette.intensity.value) / 0.655f;
        }
        SphereCollaps();
    }

    public void TakeDamage(int damage)
    {

        if (vignette.intensity.value <= 0f)
        {
            if (recieveDamage != null) { recieveDamage.Invoke(); }
            currentHealth -= damage;
            EventMinusPlayerHp?.Invoke();
            vignette.intensity.value = 0.655f;
            bloom.intensity.value = 5;
        }


        if (currentHealth <= 0)
        {
            Debug.Log(currentHealth);
            if (EventToDeadPlayer != null) EventToDeadPlayer?.Invoke(true);
        }


    }


    



    public void BonusApply(Bonus bonus)
    {
        switch (bonus.SaveCurrentBonus.RangeToSelectionBonus) { 
            #region Level1 
            case 0:
                
                switch (bonus.SaveCurrentBonus.improvements) {
                    case Improvements.AccurateKiller:
                        UpgradeCriticalDamage(1.25f);
                        break;
                    case Improvements.BigBoy:
                        break;
                    case Improvements.BulletHell:
                        UpgradeDamage(1.20f);
                        break;
                    case Improvements.DeathLover:
                        break;
                    case Improvements.ExpMaster:
                        break;
                    case Improvements.ImpulseShmimpulse:
                        UpgradeSphere(8, 11);
                        break;
                    case Improvements.Runner:
                        UpgradeMovement(1.25f);
                        break;
                    case Improvements.WalkTalk:
                        break;
        

        }
                break;
            #endregion
            #region Level2
            case 1:
                switch (bonus.SaveCurrentBonus.improvements)
                {
                    case Improvements.AccurateKiller:
                        UpgradeCriticalDamage(1.1f);
                        UpgradeBulletSpeed(1.1f);
                        break;
                    case Improvements.BigBoy:
                        break;
                    case Improvements.BulletHell:
                        UpgradeShootSpeed(1.25f);
                        break;
                    case Improvements.DeathLover:
                        break;
                    case Improvements.ExpMaster:
                        break;
                    case Improvements.ImpulseShmimpulse:
                        UpgradeCollapse(1.5f);
                        break;
                    case Improvements.Runner:
                        UpgradeRunning(1.3f);
                        break;
                    case Improvements.WalkTalk:
                        break;


                }
                break;
            #endregion
            #region Level3
            case 2:
                switch (bonus.SaveCurrentBonus.improvements)
                {
                    case Improvements.AccurateKiller:
                        UpgradeCriticalDamage(1.1f);
                        UpgradeBulletSpeed(1.1f);
                        break;
                    case Improvements.BigBoy:
                        break;
                    case Improvements.BulletHell:
                        UpgradeBulletSpeed(1.3f);
                        break;
                    case Improvements.DeathLover:
                        break;
                    case Improvements.ExpMaster:
                        break;
                    case Improvements.ImpulseShmimpulse:
                        break;
                    case Improvements.Runner:
                        UpgradeJummp(1.3f);
                        break;
                    case Improvements.WalkTalk:
                        break;


                }
                break;
#endregion
            #region Level4
            case 3:
                switch (bonus.SaveCurrentBonus.improvements)
                {
                    case Improvements.AccurateKiller:
                        UpgradeCriticalDamage(1.1f);
                        UpgradeBulletSpeed(1.1f);
                        UpgradeShootSpeed(1.5f);
                        break;
                    case Improvements.BigBoy:
                        break;
                    case Improvements.BulletHell:
                        UpgradeEnemyDrag(1.5f);
                        break;
                    case Improvements.DeathLover:
                        break;
                    case Improvements.ExpMaster:
                        break;
                    case Improvements.ImpulseShmimpulse:
                        UpgradeSphere(4, 7);
                        break;
                    case Improvements.Runner:
                        UpgradeMovement(1.2f);
                        UpgradeRunning(1.2f);
                        break;
                    case Improvements.WalkTalk:
                        break;


                }
                break;
                #endregion
        }


    }


}
