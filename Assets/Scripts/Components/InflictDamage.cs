﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент наносящий перидоческий урон цели
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("TDCore/InflictDamage")]
public class InflictDamage : MonoBehaviour, IInflictDamage
{
    private IHittable _target;

    [SerializeField]
    private int _damageValue = 5;
    /// <summary>
    /// Количество наносимого урона
    /// </summary>
    public int DamageValue { get { return _damageValue; } }

    [SerializeField]
    private float _cooldown = 0.5f;
    /// <summary>
    /// Пауза между каждым нанесением урона
    /// </summary>
    public float Cooldown { get { return _cooldown; } }

    public void Update()
    {
        
    }

    public void BeginDPS(IHittable target)
    {
        if (_target != null) EndDPS();
        _target = target;
        StartCoroutine(inflictDamage());
    }

    /// <summary>
    /// Наносит цели периодический урон
    /// </summary>
    /// <returns></returns>
    public IEnumerator inflictDamage() 
    {
        while (_target != null && !_target.IsDead)
        {
            _target.ImpactDamage(DamageValue);
            yield return new WaitForSeconds(Cooldown);
        }
        //Уведомим что текущая цель уничтожена или недоступна для атаки
        if(_target!=null)
            SendMessage("CurrentTargetLose", _target);
    }
    /// <summary>
    /// Прератить наносить текущей цели урон
    /// </summary>
	public void EndDPS()
	{
	    _target = null;
	}
    
    /// <summary>
    /// Обработка события смерти - пекращаем атаку. Крипт может умереть атакуя дом.
    /// </summary>
    public void HaveHitPointIsDead()
    {
        EndDPS();
    }
}

