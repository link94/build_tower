using System;
using System.Collections.Generic;

public class DragForce
{
	private static int MAX_ITEM_COUNT = 5;

	private static int DELAY = 1;

	private List<float> items = new List<float>();

	private int delayIndex;

	public void addItem(float item)
	{
		if (this.items.Count == DragForce.MAX_ITEM_COUNT)
		{
			this.items.RemoveAt(0);
			this.items.Insert(DragForce.MAX_ITEM_COUNT - 1, item);
		}
		else if (this.delayIndex > DragForce.DELAY)
		{
			this.items.Add(item);
		}
		else
		{
			this.delayIndex++;
		}
	}

	public float getForce()
	{
		if (this.items.Count < 2)
		{
			return 0f;
		}
		float num = this.items[0];
		float num2 = this.items[this.items.Count - 1];
		return num - num2;
	}

	public void clear()
	{
		this.delayIndex = 0;
		this.items.Clear();
	}
}
