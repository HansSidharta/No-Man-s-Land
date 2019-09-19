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
            var _spotDeform = new GameObject().AddComponent<SpotDeform_TerrainEdit_ver2>();            
                        
            Assert.Throws<NullReferenceException>(() => _spotDeform.UpdateFrame());         
        }


    }
}
