using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beams : MonoBehaviour
{
    #region fields
    [SerializeField]
    private electricBeam[] myBeams;
    #endregion

    #region properties
    public electricBeam[] electricBeams
    {
        get { return myBeams; }
    }
    #endregion
}
