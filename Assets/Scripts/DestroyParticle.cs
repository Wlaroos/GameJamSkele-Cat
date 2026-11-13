using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private void Awake()
    {
        transform.rotation = Quaternion.identity;
        DelayHelper.DelayAction(this, Destroy, 0.3f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
