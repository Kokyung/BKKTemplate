using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKK.Rendering
{
    public class RGB : MonoBehaviour
    {
        [SerializeField] private RgbType type = RgbType.Straight;

        public RgbType _type
        {
            get => type;
            set
            {
                end = true;
                StopAllCoroutines();
                type = value;
            }
        }

        [SerializeField] private Renderer rgbRenderer;
        [SerializeField] private int materialTargetIndex = 0;

        private static readonly int colorID = Shader.PropertyToID("_Color");
        private static readonly int emissionColorID = Shader.PropertyToID("_EmissionColor");

        public float speed = 5f;

        public float intensity = 2.3f;

        public Color startColor = Color.blue;
        public Color middleColor = Color.red;
        public Color lastColor = Color.green;

        private Color currentColor;

        private float timeCheck = 0;

        private bool emissionEnabled = false;
        
        private bool end = true;

        private void Awake()
        {
            GetRenderer();
            currentColor = startColor;
        }

        private void OnEnable()
        {
            end = true;
        }

        private void OnValidate()
        {
            GetRenderer();
            _type = type;
        }

        public void GetRenderer()
        {
            if (!rgbRenderer) rgbRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            switch (_type)
            {
                case RgbType.Straight:
                    Straight();
                    break;
                case RgbType.PingPong:
                    PingPong();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void Straight()
        {
            StartCoroutine(Co_Straight());
        }

        private IEnumerator ColorLerp(Color a, Color b)
        {
            timeCheck = 0;

            while (true)
            {
                var mat = rgbRenderer.materials[materialTargetIndex];
                
                timeCheck += Time.deltaTime;
                currentColor = Color.Lerp(a, b, timeCheck * speed / 10);
                
                emissionEnabled = mat.IsKeywordEnabled("_EMISSION");

                if (emissionEnabled && mat.globalIlluminationFlags != MaterialGlobalIlluminationFlags.RealtimeEmissive)
                    mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

                rgbRenderer.materials[materialTargetIndex].SetColor(emissionEnabled ? emissionColorID : colorID, 
                    currentColor * intensity);
                if (currentColor == b) break;
                yield return null;
            }
        }

        private IEnumerator Co_Straight()
        {
            if (!end) yield break;
            end = false;

            yield return ColorLerp(startColor, middleColor);
            yield return ColorLerp(middleColor, lastColor);
            yield return ColorLerp(lastColor, startColor);

            end = true;
        }

        private void PingPong()
        {
            StartCoroutine(Co_PingPong());
        }

        private IEnumerator Co_PingPong()
        {
            if (!end) yield break;
            end = false;

            yield return ColorLerp(startColor, middleColor);
            yield return ColorLerp(middleColor, lastColor);
            yield return ColorLerp(lastColor, middleColor);
            yield return ColorLerp(middleColor, startColor);

            end = true;
        }
    }

    public enum RgbType
    {
        Straight,
        PingPong,


    }
}