using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollerInitializer : MonoBehaviour
{
    [SerializeField] ScrollView scrollView = null;
    
    void Start()
    {
        var cubeSet = Resources.Load<CubeSet>("Configs/NewCubeSet");
        
        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new ItemData($"Cell {i}", cubeSet.CubeSets[i]))
            .ToArray();
        
        scrollView.UpdateData(items);
    }  
}