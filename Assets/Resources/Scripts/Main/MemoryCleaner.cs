using System;
using System.Collections;
using UnityEngine;

public class MemoryCleaner : MonoBehaviour
{
    public void Awake()
    {
        StartCoroutine(Clean());
    }

    IEnumerator Clean()
    {
        GC.Collect();

        yield return new WaitForSeconds(30f);

        StartCoroutine(Clean());
    }
}