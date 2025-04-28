using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
	[System.Serializable]
	public class ThemedUIElement : ThemedUI
	{
		[Header("Parameters")]
		Color outline;
		Image image;
		GameObject message;
		public enum OutlineStyle { solidThin, solidThick, dottedThin, dottedThick };
		public bool hasImage = false;
		public bool isText = false;
		// Used AI to help debug the textmesh's not being happy together
		protected override void OnSkinUI()
		{
			base.OnSkinUI();

			if (hasImage)
			{
				image = GetComponent<Image>();
				image.color = themeController.currentColor;
			}

			message = gameObject;

			if (isText)
			{
				var tmp = message.GetComponent<TextMeshPro>();

				var tmpUGUI = message.GetComponent<TextMeshProUGUI>();

				if (tmp != null)
				{
					tmp.color = themeController.textColor;
				}
				else if (tmpUGUI != null)
				{
					tmpUGUI.color = themeController.textColor;
				}
				else Debug.Log("Stuff broken");

			}
		}
	}
}