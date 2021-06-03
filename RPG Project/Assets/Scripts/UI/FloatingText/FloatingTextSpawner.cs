﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.FloatingText
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] FloatingText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            FloatingText instance = Instantiate<FloatingText>(damageTextPrefab);
            instance.transform.position = transform.position;
            instance.SetValue(damageAmount);
        }
    }
}