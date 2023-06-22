using System.Collections;
using System.Runtime.CompilerServices;

namespace Debaters.Server.Utils;

internal class LimitedHeap<T> : IEnumerable<T>
{
	int maxElements;
	T[] items;
	Comparison<T> comparison;

	public int Count {get; private set;}

	public LimitedHeap(int maxElements, Comparison<T> comparison)
	{
		this.maxElements = maxElements;
		items = new T[maxElements];
		Count = 0;
		this.comparison = comparison;
	}

	public void Add(T item)
	{
		if(Count == maxElements)
		{
			if(comparison(items[0], item) < 0)
			{
				items[0] = item;
				FixTop();
			}
		}
		else
		{
			items[Count++] = item;
			FixBottom();
		}
	}

	public bool TryGetTop(out T? top)
	{
		top = default(T);
		if(Count == 0)
			return false;

		top = items[0];
		return true;
	}

	public T Remove()
	{
		if(Count == 0)
			throw new InvalidOperationException("The heap is empty");

		T item = items[0];
		items[0] = items[Count - 1];
		Count--;
		FixTop();
		return item;
	}

	public void Clear()
	{
		Count = 0;
	}

	public IEnumerator<T> GetEnumerator()
	{
		if(Count == maxElements)
			return ((IEnumerable<T>)items).GetEnumerator();
		else
			return GetEnumeratorWithCount();
	}

	private IEnumerator<T> GetEnumeratorWithCount()
	{
		for(int i = 0; i < Count; i++)
			yield return items[i];
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void FixBottom()
	{
		int current = Count - 1;
		int next;

		while(current != 0)
		{
			next = (current - 1) / 2;
			if(comparison(items[current], items[next]) < 0)
			{
				Swap(current, next);
				current = next;
			}
			else
				break;

		}
	}

	private void FixTop()
	{
		int current = 0;
		int next;

		while (true)
		{
			next = current * 2 + 1;
			if (next >= Count)
				break;

			if (next + 1 < Count && comparison(items[next], items[next + 1]) > 0)
			{
				next++;
			}

			if (comparison(items[current], items[next]) <= 0)
			{
				break;
			}

			Swap(current, next);
			current = next;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Swap(int first, int second)
	{
		T temp = items[first];
		items[first] = items[second];
		items[second] = temp;
	}
}
