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
        private List<int> _list;
        private int _batchLength;
        
        [SetUp]
        public void SetUp()
        {
            _list = new List<int> { 1, 2, 3, 4, 5, 6 };
            _batchLength = 2;
        }

        [Test]
        public void Batch_throws_an_error_when_batch_size_is_zero()
        {
            // arrange
            var wasExceptionCaught = false;

            // act
            try
            {
                foreach (var batch in _list.Batch(0)) { }
            }
            catch
            {
                wasExceptionCaught = true;
            }
            
            // assert
            Assert.IsTrue(wasExceptionCaught);
        }
        
        [Test]
        public void Batch_creates_batches_of_the_appropriate_size_with_contents_in_original_order()
        {
            // arrange
            var expectedBatchOffset = 0;
            
            foreach (var batch in _list.Batch(_batchLength))
            {
                // act
                var expectedBatch = _list.GetRange(expectedBatchOffset, _batchLength);
                
                // assert
                Assert.AreEqual(_batchLength, batch.Length);
                CollectionAssert.AreEqual(expectedBatch, batch);
                expectedBatchOffset += _batchLength;
            }
        }

        [Test]
        public void BatchNoMultipleEnumeration_applies_function_to_each_batch_with_contents_changed_by_the_function_in_original_order()
        {
            // arrange
            var expectedList = new List<int> { 3, 4, 5, 6, 7, 8 };
            
            // act
            var enumeratedList = _list.BatchNoMultipleEnumeration(IncreaseIntegersInListByTwo, 2);
            
            // assert
            CollectionAssert.AreEqual(expectedList, enumeratedList);
        }

        private static IEnumerable<int> IncreaseIntegersInListByTwo(IList<int> numbers)
        {
            return numbers.Select(number => number + 2).ToList();
        }
    }
}