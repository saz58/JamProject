using System;
using UnityEngine;

namespace GT.Data.Game
{
    public class MockModuleData
    {
        public int Id { get; set; }
        public float Health { get; set; }
        public float CurrentHealth { get; set; }
        public string Name { get; set; }


        public Action<float> OnHealthUpdate;

        public void ReceiveDamage(float damage)
        {
            CurrentHealth -= damage;
            OnHealthUpdate?.Invoke(CurrentHealth);
            Debug.Log("CurrentHealth : "+CurrentHealth);
        }
        public MockModuleData(int id)
        {
            Id = id;
            CurrentHealth = Health = 20;
        }

        public MockModuleData()
        {
        }
    }
}