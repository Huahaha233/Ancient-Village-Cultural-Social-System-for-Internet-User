using UnityEngine;
[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	public Transform target;
	public Camera gameCamera;
	public Camera uiCamera;
	public bool disableIfInvisible = true;
	Transform mTrans;
	bool mIsVisible = false;

	/// <summary>
	/// Cache the transform;
	/// </summary>

	void Awake () { mTrans = transform; }

	/// <summary>
	/// Find both the UI camera and the game camera so they can be used for the position calculations
	/// </summary>

	void Start()
	{
		if (target != null)
		{
			if (gameCamera == null) gameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
			if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
			SetVisible(false);
		}
		else
		{
			Debug.LogError("Expected to have 'target' set to a valid transform", this);
			enabled = false;
		}
	}

	/// <summary>
	/// Enable or disable child objects.
	/// </summary>

	void SetVisible (bool val)
	{
		mIsVisible = val;

		for (int i = 0, imax = mTrans.childCount; i < imax; ++i)
		{
			NGUITools.SetActive(mTrans.GetChild(i).gameObject, val);
		}
	}

	/// <summary>
	/// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
	/// </summary>

	void Update ()
	{
		Vector3 pos = gameCamera.WorldToViewportPoint(target.position);

		// Determine the visibility and the target alpha
		bool isVisible = (gameCamera.orthographic || pos.z > 0f) && (!disableIfInvisible || (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f));

		// Update the visibility flag
		if (mIsVisible != isVisible) SetVisible(isVisible);

		// If visible, update the position
		if (isVisible)
		{
			transform.position = uiCamera.ViewportToWorldPoint(pos);
			pos = mTrans.localPosition;
			pos.x = Mathf.FloorToInt(pos.x);
			pos.y = Mathf.FloorToInt(pos.y);
			pos.z = 0f;
			mTrans.localPosition = pos;
		}
		OnUpdate(isVisible);
	}

	/// <summary>
	/// Custom update function.
	/// </summary>

	protected virtual void OnUpdate (bool isVisible) { }
}
