using UnityEngine;
using UnityEngine.UI;

public class VoxelmetricExample : MonoBehaviour
{
    Vector2 rot;

    public string blockToPlace = "air";
    public Text selectedBlockText;
    public Text saveProgressText;

    BlockPos pfStart;
    BlockPos pfStop;
    public PathFinder pf;

    SaveProgress saveProgress;
	public float maxDistance = 3;

    public void SetType(string newType){
        blockToPlace = newType;
    }

	void Start(){
		Cursor.visible = false;
	}

    void Update()
    {

		if(Input.GetKeyDown (KeyCode.Alpha1))
			SetType ("air");
		if(Input.GetKeyDown(KeyCode.Alpha2))
			SetType("stone");
        RaycastHit hit;
        var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if (Input.GetMouseButtonDown(0))
        {

            bool adjacent = true;
            if (((Block)blockToPlace).type == Block.Air.type)
            {
                adjacent = false;
            }

			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
            {
                Voxelmetric.SetBlock(hit, blockToPlace, adjacent);
            }
        }

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            selectedBlockText.text = Voxelmetric.GetBlock(hit).ToString();
        }

        if (saveProgress != null)
        {
            saveProgressText.text = SaveStatus();
        }
        else
        {
            saveProgressText.text = "Save";
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Physics.Raycast(Camera.main.transform.position, mousePos - Camera.main.transform.position, out hit, 100))
            {
                pfStart = Voxelmetric.GetBlockPos(hit);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (Physics.Raycast(Camera.main.transform.position, mousePos - Camera.main.transform.position, out hit, 100))
            {
                pfStop = Voxelmetric.GetBlockPos(hit);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
             pf = new PathFinder(pfStart, pfStop, World.instance, 2);
            Debug.Log(pf.path.Count);
        }

        if (pf!=null && pf.path.Count != 0)
        {
            for (int i = 0; i < pf.path.Count - 1; i++)
                Debug.DrawLine(pf.path[i].Add(0,1,0), pf.path[i + 1].Add(0,1,0));
        }
    }

    public void SaveAll()
    {
        saveProgress = Voxelmetric.SaveAll();
    }

    public string SaveStatus()
    {
        if (saveProgress == null)
            return "";

        return saveProgress.GetProgress() + "%";
    }

}
