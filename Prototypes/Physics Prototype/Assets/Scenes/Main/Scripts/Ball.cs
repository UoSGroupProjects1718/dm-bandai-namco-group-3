using UnityEngine;

namespace Scenes.Main.Scripts
{
	public class Ball : MonoBehaviour 
	{
		private void OnCollisionEnter2D(Collision2D collision2D)
		{
			if (collision2D.gameObject.CompareTag("Green"))
			{
				GetComponent<SpriteRenderer>().color = Color.green;
			}
			else if(collision2D.gameObject.CompareTag("Red"))
			{
				GetComponent<SpriteRenderer>().color = Color.red;
			}
			else if (collision2D.gameObject.CompareTag("Water"))
			{
				GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			}
		}
	}
}
