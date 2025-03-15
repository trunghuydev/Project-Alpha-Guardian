using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMinionStats : MonoBehaviour
{
    public float hp;
    public float atk;
    public float def;
    public float shield;
    public float movementSpeed;
    public float res;
    public int range;

    public float getHP()
    {
        return hp;
    }

    public float getDef()
    {
        return def;
    }

    public float getAtk()
    {
        return atk;
    }
    public float getRes()
    {
        return res;
    }
}
