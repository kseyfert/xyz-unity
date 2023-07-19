using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [Serializable]
    internal struct Clip
    {
        [SerializeField] public string name;
        [SerializeField] public bool loop;
        [SerializeField] public bool allowNext;
        [SerializeField] public Sprite[] sprites;
    }
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private Clip[] clips;
        [SerializeField] private int frameRate = 10;
        [SerializeField] private UnityEvent onComplete;

        private SpriteRenderer _spriteRenderer;
        private bool _active = true;

        private float _frameSeconds;
        private float _nextFrameTime;
        private int _currentFrameIndex = 0;
        private int _currentClipIndex = 0;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _frameSeconds = 1f / frameRate;
            _nextFrameTime = Time.time + _frameSeconds;

            var frame = GetCurrentFrame();
            if (frame == null) _active = false;
            else _spriteRenderer.sprite = frame;
        }

        private void Update()
        {
            if (!_active) return;
            if (Time.time < _nextFrameTime) return;

            while (Time.time >= _nextFrameTime)
            {
                GoNextFrame();
                _nextFrameTime += _frameSeconds;
            }

            _spriteRenderer.sprite = GetCurrentFrame();
            
            if (!_active) onComplete?.Invoke();
        }

        private Sprite GetCurrentFrame()
        {
            if (_currentClipIndex >= clips.Length) return null;
            var clip = clips[_currentClipIndex];

            if (_currentFrameIndex >= clip.sprites.Length) return null;
            return clip.sprites[_currentFrameIndex];
        }

        private void GoNextFrame()
        {
            var currentClip = clips[_currentClipIndex];
            var isLastClip = _currentClipIndex == clips.Length - 1;
            var isLastFrame = _currentFrameIndex == currentClip.sprites.Length - 1;

            if (!isLastFrame)
            {
                _currentFrameIndex++;
                return;
            }

            if (currentClip.loop)
            {
                _currentFrameIndex = 0;
                return;
            }

            if (!currentClip.allowNext || isLastClip)
            {
                _active = false;
                return;
            }

            _currentFrameIndex = 0;
            _currentClipIndex++;
        }

        public int FindClip(string clipName)
        {
            for (var i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == clipName) return i;
            }

            return -1;
        }

        public void SetClip(int clipIndex)
        {
            if (clipIndex < 0 || clipIndex >= clips.Length)
            {
                throw new Exception("Can't find clip");
            }

            _currentClipIndex = clipIndex;
            _currentFrameIndex = 0;
        }

        public void SetClip(string clipName)
        {
            SetClip(FindClip(clipName));
        }
    }
}