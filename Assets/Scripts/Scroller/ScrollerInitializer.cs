using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollerInitializer : MonoBehaviour
{
    [SerializeField] ScrollView scrollView = null;
    
    void Start()
    {
        var items = Enumerable.Range(0, 20)
            .Select(i => new ItemData($"Cell {i}"))
            .ToArray();
        
        scrollView.UpdateData(items);
    }  
}