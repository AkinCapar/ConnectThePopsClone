using System;
using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Data;
using ConnectThePops.Models;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Views
{
    public class PopView : MonoBehaviour, IPoolable<PopsData, IMemoryPool>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private int _value;
        private IMemoryPool _pool;
        private SlotModel _currentSlot;
        

        public void OnSpawned(PopsData data, IMemoryPool pool)
        {
            _value = data.popValue;
            _pool = pool;
            _spriteRenderer.sprite = data.popSprite;
        }

        public void SetCurrentSlot(SlotModel slot)
        {
            _currentSlot = slot;
        }

        public void SetPositionImmediate(Vector2 position)
        {
            transform.position = position;
        }
        
        public void OnDespawned()
        {
        }

        public class Factory : PlaceholderFactory<PopsData, PopView>
        {
        }

        public class Pool : MonoPoolableMemoryPool<PopsData, IMemoryPool, PopView>
        {
        }
    }
}
