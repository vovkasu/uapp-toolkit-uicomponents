using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UAppToolkit.UIComponents
{
    [ExecuteAlways]
    public class UIParticleScaler : MonoBehaviour
    {
        [Serializable]
        private class ParticleDefaultValue
        {
            public ParticleSystem Particle;
            public Vector3 ShapeScale;
        }

        [SerializeField] private bool _scaleX = true;
        [SerializeField] private bool _scaleY = true;
        [Space]
        [SerializeField] private bool _scaleShape = true;
        [Space]
        [SerializeField] private bool _updateScaleOnSizeChanged = true;
        [SerializeField] private List<ParticleSystem> _particles;

        [HideInInspector] [SerializeField] private Vector3 _defaultRectSize;
        [HideInInspector] [SerializeField] private List<ParticleDefaultValue> _defaultParticleValues = new();

        private void Awake()
        {
            UpdateScale();
        }

        public void UpdateScale()
        {
            if (_particles == null || _particles.Count == 0)
                return;

            var currentRectSize = ((RectTransform)transform).rect.size;
            var scaleFactor = currentRectSize / _defaultRectSize;

            foreach (var particle in _particles)
                TryUpdateShapeScale(particle, scaleFactor);
        }

        private void TryUpdateShapeScale(ParticleSystem particle, Vector3 scaleFactor)
        {
            if (!_scaleShape)
                return;

            var shape = particle.shape;
            var particleDefaultValues = _defaultParticleValues.FirstOrDefault(x => x.Particle == particle);
            if (particleDefaultValues == null)
                return;

            var newShapeScale = particleDefaultValues.ShapeScale;
            if (_scaleX)
                newShapeScale.x *= scaleFactor.x;

            if (_scaleY)
                newShapeScale.y *= scaleFactor.y;

            shape.scale = newShapeScale;
        }

        private void OnRectTransformDimensionsChange()
        {
            if (_updateScaleOnSizeChanged)
                UpdateScale();
        }

        #if UNITY_EDITOR
        [Button("Bake")]
        private void SaveDefaultValues()
        {
            SaveDefaultRectSize();
            SaveParticleDefaultValues();
            Debug.Log($"{nameof(UIParticleScaler)} ({name}): Default values saved", this);
        }

        private void SaveDefaultRectSize()
        {
            _defaultRectSize = ((RectTransform)transform).rect.size;
            EditorUtility.SetDirty(this);
        }

        private void SaveParticleDefaultValues()
        {
            if (_particles == null || _particles.Count == 0)
                return;

            _defaultParticleValues.Clear();
            foreach (var particle in _particles)
            {
                var shape = particle.shape;
                _defaultParticleValues.Add(new ParticleDefaultValue
                {
                    Particle = particle,
                    ShapeScale = shape.scale
                });
            }

            EditorUtility.SetDirty(this);
        }
        #endif
    }
}
