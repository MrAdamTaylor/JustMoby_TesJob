using System;
using System.Diagnostics;
using Common;
using Core.Tower;
using DragAndDrop;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Canvas _canvas;
    
    private GameObject _canvasInstance;
    private GameContentProvider _gameContentProvider;

    public override void InstallBindings()
    {
        
        _canvasInstance = GameObject.Instantiate(_canvasPrefab, _canvas.transform);
        _gameContentProvider = _canvasInstance.GetComponent<GameContentProvider>();

        Stopwatch bindTime = new Stopwatch();
        bindTime.Start();
        Container.Bind<FancyScrollView.Scroller>()
            .FromInstance(_gameContentProvider.Scroller)
            .AsSingle();

        Container.Bind<DragDropElementView>()
            .FromInstance(_gameContentProvider.DragDropElementView)
            .AsSingle();
        
        Container.Bind<InputHandler>()
            .AsSingle()
            .NonLazy();

        Container.Bind<OnDroppedHandler>()
            .AsSingle();
        
        Container.BindInterfacesAndSelfTo<DragDropManager>()
            .AsSingle()
            .NonLazy();
        
        bindTime.Stop();
        Debug.Log($"Bind Time in Zenject {bindTime.ElapsedMilliseconds} ms.");
    }
}
