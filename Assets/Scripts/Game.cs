﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Компонент основной бизнес-логики игры
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("TDCore/Game")]
public class Game : MonoBehaviour, IGame
{
    [SerializeField]
    private int _waveCounts;
    /// <summary>
    /// Количество волн
    /// </summary>
    public int WaveCounts { get { return _waveCounts; } }
    
    private int _currentWave;
    /// <summary>
    /// Текущая волна
    /// </summary>
    public int CurrentWave { get { return _currentWave; } }

    /// <summary>
    /// Увеличение количества криптов с каждой волной
    /// </summary>
    [SerializeField]
    private int _criptsPerWave;
    /// <summary>
    /// Задержка между спауном врага
    /// </summary>
    [SerializeField]
    private float _spawnInterval = 0.3f;

    private bool gameIsEnd;

    private IGame.GameResult _status;
    /// <summary>
    /// Текущий статус игры - в процессе, проигрыш, выигрыш
    /// </summary>
    public IGame.GameResult Status { get { return _status; } }

    /// <summary>
    /// Прототип для инстанционирования первого типа - простой крип
    /// </summary>
    [SerializeField]
    private GameObject EnemyPrototypeOne;
    /// <summary>
    /// Прототип для инстанционирования второго типа - сильный крип
    /// </summary>
    [SerializeField]
    private GameObject EnemyPrototypeTwo;
    /// <summary>
    /// Точка спауна
    /// </summary>
    [SerializeField]
    private Transform SpawnPoint;
    /// <summary>
    /// Трансформ для всех созданных крипов
    /// </summary>
    [SerializeField]
    private Transform CripGameObjectsHolder;

    private List<IHittable> _allEnemies;
    private IHittable _keep;

    private ITowerBuilder _builder;

    public void Awake()
    {
        //Ограничем fps для 2D игры в 30.
        Application.targetFrameRate = 30;
    }

    public void StartGame()
    {
        _allEnemies = new List<IHittable>();
        _builder = FindObjectOfType<TowerBuilder>();
        _keep = GameObject.FindGameObjectWithTag("Keep").GetComponent<HaveHitPoint>();
        gameIsEnd = false;
        _status = IGame.GameResult.InBuildProcess;
        _builder.ShowUI = true;
    }

    /// <summary>
    /// Обработаем событие, когда все башни построены - начнём игру
    /// </summary>
    public void OnTowersIsBuild()
    {
        StartCoroutine(spawnEnemys());
        _status = IGame.GameResult.InProcess;
    }

    private void GameOver(IGame.GameResult result)
    {
        gameIsEnd = true;
        _status = result;
    }

    private IEnumerator spawnEnemys()
    {
        _currentWave += 1;
        _allEnemies.Clear();
        int count = _criptsPerWave * _currentWave;
        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(i%2 == 0 ? EnemyPrototypeTwo : EnemyPrototypeOne) as GameObject;
            enemy.transform.parent = CripGameObjectsHolder;
            enemy.transform.position = SpawnPoint.position;
            _allEnemies.Add(enemy.GetComponent<HaveHitPoint>());
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    /// <summary>
    /// Основная логика игры - если дом уничтожен то проигрыш, если уничтожены все крипы, но не последняя волна, то спауним ещё,
    /// если волна последняя и дом не разрушен то победили.
    /// </summary>
    public void Update()
    {
        if (Status == IGame.GameResult.NotStarted 
            || Status == IGame.GameResult.InBuildProcess
            || gameIsEnd) return;
        
        if(_keep.IsDead) 
            GameOver(IGame.GameResult.Lose);

        var allDead = _allEnemies.FindAll(enemy => enemy.IsDead);
        if (allDead.Count == _allEnemies.Count)
        {
            if(CurrentWave < WaveCounts)
                StartCoroutine(spawnEnemys());
            else
                GameOver(IGame.GameResult.Victory);
        }
    }
}
