using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareScript : MonoBehaviour
{


    public BoardSquare bsModel;
    public GameManagerScript gms;

    private SpriteRenderer sr;


    // UNITY HOOKS

    void Start()
    {
        this.sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (this.bsModel.active)
        {
            sr.color = Color.red;
        } else
        {
            sr.color = Color.blue;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.bsModel.active = !this.bsModel.active;
            this.gms.SyncGameStateToServer();
        }
    }

}
