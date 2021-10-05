using Sprite = Graphics.Sprite;

public class Ball
{
	public int x, y;
	private int xDir, yDir, xDim, yDim;
	public Sprite sprite;

	int bottomY, rightX;

	public Ball(int x, int y, int xDim, int yDim, Sprite sprite)
	{
		this.x = x;
		this.y = y;
		
		this.sprite = sprite;

		this.xDim = xDim;
		this.yDim = yDim;

		xDir = 1;
		yDir = 1;
	}

	public void SetBorder(int x, int y)
	{
		rightX = x;
		bottomY = y;
	}

	public void Move()
	{
		x += xDir;
		y += yDir;

		if (x == 1) xDir = 1;
		else if (x == rightX - xDim - 1) xDir = -1;

		if (y == 1) yDir = 1;
		else if (y == bottomY - yDim - 1) yDir = -1;
	}
}