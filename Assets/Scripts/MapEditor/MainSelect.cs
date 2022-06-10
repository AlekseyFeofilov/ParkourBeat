using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace MapEditor
{
public class MainSelect : MonoBehaviour {

	public LayerMask selectableMask;
	public LayerMask toolMask;

	private Camera _camera;

	[SerializeField] 
	private MainTools mainTools;
	
	private void Start()
	{
		_camera = Camera.main;
		
	}

	public static OutlinedObject SelectedObj { get; private set; }

	// ReSharper disable twice Unity.PerformanceCriticalCodeNullComparison
	private void Select()
	{
		var obj = GetObjectByMousePosition();
		if (SelectedObj == obj) return;
		
		if (SelectedObj != null) SelectedObj.OutlineWidth = 0;

		if ((SelectedObj = obj) == null)
		{
			mainTools.ToolMode = MainTools.Mode.None;
			return;
		}

		SelectedObj.OutlineWidth = 10;
		mainTools.ToolMode = MainTools.Mode.MoveTool;
	}

	private OutlinedObject GetObjectByMousePosition()
	{
		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (!Physics.Raycast(ray, out hit, 70, toolMask) &&
		    !Physics.Raycast(ray, out hit, 70, selectableMask)) return null;

		var obj = hit.transform.GetComponent<OutlinedObject>();
		return obj ? obj : SelectedObj;
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Select();
		}
	}
}}
