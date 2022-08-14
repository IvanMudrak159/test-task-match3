using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public List<int> _firstList;
        public List<int> _secondList;
        private void Start()
        {
            _firstList = new List<int>();
            _secondList = new List<int>();
            _firstList.Add(1);
            _firstList.Add(2);
            _firstList.Remove(4);
            Debug.Log(_secondList.Count);
            _firstList.AddRange(_secondList);

        }
    }
}