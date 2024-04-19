using System;
using UnityEngine;

namespace CodeBase
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _coroutineRunner;

        public static CoroutineRunner Instance => _coroutineRunner;

        private void Awake()
        {
            if (_coroutineRunner == null)
                _coroutineRunner = this;
        }
    }
}