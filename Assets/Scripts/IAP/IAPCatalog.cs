/// <summary>
/// Danh mục gói điểm IAP. ID và chữ lấy từ cửa hàng cube jump.
/// Giá thật đặt trên Google Play Console — chữ giá chỉ hiện khi store chưa trả về giá nội tệ.
/// </summary>
public static class IAPCatalog
{
	public struct Pack
	{
		public string ProductId;
		public int Points;
		public string DisplayName;
		public string PricePlaceholder;

		public Pack(string productId, int points, string displayName, string pricePlaceholder)
		{
			ProductId = productId;
			Points = points;
			DisplayName = displayName;
			PricePlaceholder = pricePlaceholder;
		}
	}

	public static readonly Pack[] Packs =
	{
		new Pack("iap1", 10, "Starter Stack", "$0.50"),
		new Pack("iap2", 20, "Small Stack", "$1.00"),
		new Pack("iap3", 40, "Block Stack", "$2.00"),
		new Pack("iap4", 60, "Big Stack", "$3.00"),
		new Pack("iap5", 100, "Mega Stack", "$5.00"),
		new Pack("iap6", 200, "Turbo Stack", "$7.00"),
		new Pack("iap7", 500, "Ultra Stack", "$10.00"),
	};

	public static int GetPoints(string productId)
	{
		for (int i = 0; i < Packs.Length; i++)
		{
			if (Packs[i].ProductId == productId)
				return Packs[i].Points;
		}

		return 0;
	}
}
