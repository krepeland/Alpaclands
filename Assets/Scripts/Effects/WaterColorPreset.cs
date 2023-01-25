using UnityEngine;

[CreateAssetMenu(fileName = "WaterColor", menuName = "Water/WaterColor", order = 0)]
public class WaterColorPreset : ScriptableObject 
{
    public Color mainColor = new Color(0.5355287f, 0.9245283f, 0.8958215f, 0.4823529f);
    public Color deepColor = new Color(0.1758989f, 0.5443938f, 0.5754717f, 1f);
    public Color foamColor = new Color(0.3888395f, 0.8018868f, 0.5963619f, 0.7058824f);
}
