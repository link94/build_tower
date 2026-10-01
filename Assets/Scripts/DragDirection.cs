using System;
using System.Collections.Generic;

public class DragDirection
{
	private static int MAX_ITEM_COUNT = 2;

	private List<float> items = new List<float>();

	private float currentDirection;

	public void addItem(float item)
	{
		if (this.items.Count == DragDirection.MAX_ITEM_COUNT)
		{
			if (item != this.items[1])
			{
				this.items.RemoveAt(0);
				this.items.Insert(DragDirection.MAX_ITEM_COUNT - 1, item);
			}
		}
		else
		{
			this.items.Add(item);
		}
		float direction = this.getDirection();
		if (this.currentDirection == 0f || direction != this.currentDirection)
		{
		}
		this.currentDirection = direction;
	}

	public float getDirection()
	{
		if (this.items.Count < 2)
		{
			return 0f;
		}
		float num = this.items[0];
		float num2 = this.items[this.items.Count - 1];
		return num2 - num;
	}

	public void clear()
	{
		this.items.Clear();
	}
}
