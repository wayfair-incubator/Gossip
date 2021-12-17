using System;
using System.Collections.Generic;
using System.Linq;
using Gossip.Utilities;
using NUnit.Framework;

namespace Gossip.UnitTests.Utilities
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        private IEnumerable<int> _list;
        
        [SetUp]
        public void SetUp()
        {
            _list = new List<int> { 1, 2, 3, 4, 5, 6 };
        }

        [Test]
        public void Batch_throws_an_error_when_batch_size_is_zero()
        {
            // arrange
            var wasExceptionCaught = false;
            var errorMessage = string.Empty;
            
            // act
            try
            {
                foreach (var batch in _list.Batch(0)) { }
            }
            catch(Exception e)
            {
                errorMessage = e.Message;
                wasExceptionCaught = true;
            }
            
            // assert
            Assert.IsTrue(wasExceptionCaught);
        }
        
        [Test]
        public void Batch_creates_a_batch()
        {
            // act
            foreach (var batch in _list.Batch(2))
            {
                // assert
                Assert.AreEqual(batch.Length, 2);
            }
        }

        [Test]
        public void BatchNoMultipleEnumeration_applies_function_to_batch()
        {
            // arrange
            var expectedList = new List<int> { 3, 4, 5, 6, 7, 8 };
            
            // act
            var newList = _list.BatchNoMultipleEnumeration(IncreaseListElementsByTwo, 2);
            
            // assert
            CollectionAssert.AreEqual(newList, expectedList);
        }

        private static IEnumerable<int> IncreaseListElementsByTwo(IList<int> numbers)
        {
            return numbers.Select(number => number + 2).ToList();
        }
    }
}