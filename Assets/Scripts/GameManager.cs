using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    private bool _keyToCastleAquired;
    public bool KeyToCastleAquired { get => _keyToCastleAquired; set => _keyToCastleAquired = value; }
}
