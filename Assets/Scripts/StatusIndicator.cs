using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {
    [Header("Object Constants")]
	public RectTransform healthBarRect;
    public Text healthText;

    void Awake()
    {
        initializeObject();
    }

    void initializeObject()
    {
        healthBarRect = transform.GetChild(0).GetComponent<RectTransform>();
        healthText = transform.GetChild(1).GetComponent<Text>();
    }

	public void SetHealth(int _cur, int _max) {
		float _value = (float)_cur / _max;

		healthBarRect.localScale = new Vector3 (_value, healthBarRect.localScale.y, healthBarRect.localScale.y);
		healthText.text = _cur.ToString ();
	}
}
