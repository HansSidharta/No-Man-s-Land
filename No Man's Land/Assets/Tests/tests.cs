using System.Collections;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class tests
    {
        [Test]
        public void _testNullWorldException()
        {  
            // Ensure Test catches when the world is not given in the Editor (A correct 'World name'World must be given to pass)
            GameObject _spotDeform =  GameObject.CreatePrimitive(PrimitiveType.Cube);                                   
                        
            Assert.Catch<NullReferenceException>(() => _spotDeform.AddComponent<SpotDeform_TerrainEdit>());         
        }

        [Test]
        public void _testWorldBoundException()
        {
            // Tests that the script correctly recognises when vertex data is not within its bounds
            var _spotDeform = new GameObject().AddComponent<SpotDeform_TerrainEdit>();

            Assert.Catch<NullReferenceException>(() => _spotDeform.UpdateFrame());
        }


    }
}
