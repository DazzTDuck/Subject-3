using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckpointHolder : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> checkpoints = new List<Transform>();

}
