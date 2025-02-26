using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthbarGames
{
    public class TargetObject
    {
        private GameObject _gameObject;
        private Transform _transform;
        private Rigidbody _rigidbody;

        private Transform parent;
        private bool isKinematic;
        private bool useGravity;
        private RigidbodyInterpolation interpolation;

        public bool IsCaptured { get; private set; }

        public bool IsNull => _gameObject == null;

        public GameObject GameObject => _gameObject;
        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;

        public TargetObject()
        {
            Clear();
            IsCaptured = false;
        }

        public void StartAttractingObject(Rigidbody rigidbody)
        {
            if (rigidbody == null) return;

            Store(rigidbody);

            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.drag = 0.0f;
        }

        public void CaptureObject(Vector3 position, Transform parent)
        {
            if (_gameObject == null) return;

            _rigidbody.position = position;
            _transform.parent = parent;
            _rigidbody.velocity = Vector3.zero;

            IsCaptured = true;
        }

        public void HoldObject(Vector3 position)
        {
            if (_gameObject == null) return;

            _rigidbody.position = position;
            _rigidbody.velocity = Vector3.zero;
        }

        public Rigidbody ReleaseObject()
        {
            Rigidbody rb = Restore();
            Clear();
            IsCaptured = false;

            return rb;
        }

        private void Clear()
        {
            parent = null;
            _gameObject = null;
            _transform = null;
            _rigidbody = null;
            isKinematic = false;
            useGravity = false;
            interpolation = RigidbodyInterpolation.None;
        }

        private void Store(Rigidbody rb)
        {
            if (rb == null)
            {
                Clear();
                return;
            }

            _gameObject = rb.gameObject;
            _transform = rb.transform;
            parent = _transform.parent;
            _rigidbody = rb;
            isKinematic = rb.isKinematic;
            useGravity = rb.useGravity;
            interpolation = rb.interpolation;
        }

        private Rigidbody Restore()
        {
            if (_transform != null)
                _transform.parent = parent;

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = isKinematic;
                _rigidbody.useGravity = useGravity;
                _rigidbody.interpolation = interpolation;
            }

            return _rigidbody;
        }
    }
}