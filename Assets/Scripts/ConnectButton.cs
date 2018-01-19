
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConnectButton : MonoBehaviour {
	private Vector3 startingPosition;

	public Material inactiveMaterial;
	public Material gazedAtMaterial;
	float timeToPress = 2f;
	static float INACTIVE = -1f;
	float startTime;
	public GameObject gameManager;
	public GameObject device;

	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		startTime = INACTIVE;
	}


	public void BeginTimer() {
		startTime = Time.time;
		SetGazedAt (true);
	}

	public void ResetTimer() {
		startTime = INACTIVE;
		SetGazedAt (false);
	}

	public void Update() {
		if (startTime != INACTIVE) {
			if (Time.time - startTime >= timeToPress) {
				device.GetComponent<MyDevice> ().prepareBluetooth ();
				print (device.GetComponent<MyDevice>().MAC);
				startTime = INACTIVE;
				SetGazedAt (false);
			}
		}
	}

	public void SetGazedAt(bool gazedAt) {
		if (inactiveMaterial != null && gazedAtMaterial != null) {
			GetComponent<Renderer>().material = gazedAt ? gazedAtMaterial : inactiveMaterial;
			return;
		}
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}

	public void Reset() {
		transform.localPosition = startingPosition;
	}

	public void Recenter() {
		#if !UNITY_EDITOR
		GvrCardboardHelpers.Recenter();
		#else
		if (GvrEditorEmulator.Instance != null) {
			GvrEditorEmulator.Instance.Recenter();
		}
		#endif  // !UNITY_EDITOR
	}
}