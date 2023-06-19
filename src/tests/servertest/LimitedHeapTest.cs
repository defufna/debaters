using Debaters.Server.Utils;
namespace servertest;

public class LimitedHeapTest
{
	[Fact]
	public void TestAdd()
	{
		int[] toAdd = new[] { 4, 2, -9 };
		LimitedHeap<int> heap = new LimitedHeap<int>(toAdd.Length, (a, b) => a - b);
		foreach (var num in toAdd)
			heap.Add(num);

		Assert.Equal(3, heap.Count);
		Assert.All(heap, num => toAdd.Contains(num));

		heap.Add(12);
		heap.Add(-5);
		Assert.Equal(3, heap.Count);
		HashSet<int> test = new HashSet<int>(new int[] { 4, 2, 12 });
		Assert.All(heap, num => test.Contains(num));
	}

	[Fact]
	public void TestRemove()
	{
		int[] toAdd = new[] { 5, 4, 3, 2, 1, 0, -1 };
		LimitedHeap<int> heap = new LimitedHeap<int>(toAdd.Length, (a, b) => a - b);
		foreach (var num in toAdd)
			heap.Add(num);

		System.Array.Sort(toAdd);

        for(int i = 0; i < toAdd.Length; i++)
        {
			int removed = heap.Remove();
			Assert.Equal(toAdd[i], removed);
			Assert.Equal(toAdd.Length - i - 1, heap.Count);
		}

		Assert.Throws<InvalidOperationException>(() => heap.Remove());
	}
}