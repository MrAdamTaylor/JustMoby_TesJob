using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocalizable 
{
    LocalizationData Data { get; set; }
    void Localize();
}
