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
    /// <summary>
    Movement movement;
    PShootingController shootingController;
/// </summary>
    // Start is called before the first frame update
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
        movement.speedRunning *= Coefficient;
        movement.speedWalking *= Coefficient;

    }
    public void UpgradeDamage(float Coefficient)
    {
        shootingController.damage = (int)(shootingController.damage * Coefficient);
    }
    // Update is called once per frame

    void Update()
    {

        if (currentHealth <= 0) { gameObject.GetComponent<PMoveController>().die.Invoke(); }
        if (vignette.opacity < 0)
        {
            vignette.intensity.value = 0f;
            bloom.intensity.value = 0;
        }
        else
        {
            vignette.intensity.value -= Time.deltaTime / 5;
            bloom.intensity.value = 10 * (vignette.intensity.value)/0.655f;
        }    }

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
}
