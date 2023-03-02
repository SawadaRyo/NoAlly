using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchTypeMove : DoorSwitchBase
{
    [SerializeField] TypeOfPiston _typeOfPiston = default;
    [SerializeField] float _objDia = 1.0f;
    [SerializeField] float _objHz = 1.0f;
    float _cycle;
   
    public override void ObjectAction()
    {
        if (GameManager.Instance.IsGame == GameState.InGame)
        {
            _cycle += Time.deltaTime;

            switch (_typeOfPiston)
            {
                case TypeOfPiston.HType:
                    this.transform.position = new Vector3(this.transform.position.x, Mathf.Sin(_objDia * Mathf.PI * _objHz * _cycle) + this.transform.position.y, this.transform.position.z);
                    break;
                case TypeOfPiston.VType:
                    this.transform.position = new Vector3(Mathf.Sin(_objDia * Mathf.PI * _objHz * _cycle) + this.transform.position.x, this.transform.position.y, this.transform.position.z);
                    break;
                case TypeOfPiston.CircleType:
                    this.transform.position = new Vector3(Mathf.Sin(_objDia * Mathf.PI * _objHz * _cycle), Mathf.Sin(_objDia * Mathf.PI * _objHz * _cycle), 0f);
                    break;
                default:
                    break;
            }
        }
    }
}

enum TypeOfPiston 
{
    HType,
    VType,
    CircleType
}