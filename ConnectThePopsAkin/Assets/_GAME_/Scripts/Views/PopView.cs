using System;
using System.Collections;
using System.Collections.Generic;
using ConnectThePops.Data;
using ConnectThePops.Models;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace ConnectThePops.Views
{
    public class PopView : MonoBehaviour, IPoolable<PopsData, IMemoryPool>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private int  _value;
        public int Value => _value;
        private IMemoryPool _pool;
        public SlotModel CurrentSlot{ get; private set; }
        public bool IsTapped{ get; private set; }
        

        public void OnSpawned(PopsData data, IMemoryPool pool)
        {
            _value = data.popValue;
            _pool = pool;
            _spriteRenderer.sprite = data.popSprite;
        }

        public void SetCurrentSlot(SlotModel slot)
        {
            CurrentSlot = slot;
        }

        public void SetPositionImmediate(Vector2 position)
        {
            transform.position = position;
        }

        public void Tapped()
        {
            IsTapped = true;
            transform.DOScale(Vector2.one * .55f, .05f);
        }

        public void Release()
        {
            IsTapped = false;
            transform.DOScale(Vector2.one / 2, .05f);
        }

        public void Merge(SlotModel slot)
        {
            transform.DOMove(slot.WorldPos, CurrentSlot.GetDistanceToOtherSlot(slot) * .25f);
            transform.DOScale(Vector2.zero, 1); //TODO delete it, it was a test.
            CurrentSlot.SetEmpty();
        }

        public void MoveToNewSlot(SlotModel slot)
        {
            transform.DOMove(slot.WorldPos, CurrentSlot.GetDistanceToOtherSlot(slot) * .25f);
            SetCurrentSlot(slot);
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
