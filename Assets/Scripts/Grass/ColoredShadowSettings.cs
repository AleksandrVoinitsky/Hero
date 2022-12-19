using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColoredShadowSettings : MonoBehaviour
{
    [SerializeField]
    private Color shadowColor;
    [SerializeField]
    private float shadowStrenght;

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying) // only in editor mode, take out of the if statement if you want to play with it during runtime
        {
            Shader.SetGlobalColor("_ShadowColor", shadowColor);
            Shader.SetGlobalFloat("_ShadowStrength", shadowStrenght);
        }


    }
}
