using DG.Tweening;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    UnityEngine.Rendering.ProbeReferenceVolume probeRefVolume;
    public string defualtScenario = "Default";
    public string chasing = "Enemy";
    private float _blendingFactor = 0f;
    [Min(1)] public int numberOfCellsBlendedPerFrame = 10;

    [SerializeField] private Light[] _lights;
    private Color[] _originalColors;

    void Awake()
    {
        probeRefVolume = UnityEngine.Rendering.ProbeReferenceVolume.instance;
        probeRefVolume.lightingScenario = defualtScenario;
        probeRefVolume.numberOfCellsBlendedPerFrame = numberOfCellsBlendedPerFrame;

        _originalColors = new Color[_lights.Length];
        for (int i = 0; i < _lights.Length; i++)
        {
            _originalColors[i] = _lights[i].color;
        }
    }

    private void TransitionToChasingLights()
    {
        DOVirtual.Float(0f, 1f, 0.2f, t =>
            {
                UpdateLighting(t, chasing);
            });
    }

    private void TransitionToNormalLights()
    {
        DOVirtual.Float(1f, 0f, 0.2f, t =>
        {
            UpdateLighting(t, chasing);
        });
    }

    private void UpdateLighting(float t, string lightScenario) 
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].color = Color.Lerp(_originalColors[i], Color.red, t);
        }
        probeRefVolume.BlendLightingScenario(lightScenario, t);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) 
        {
            TransitionToChasingLights();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            TransitionToNormalLights();
        }
    }
}
