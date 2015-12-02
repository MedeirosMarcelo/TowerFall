using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelChanger01 : MonoBehaviour {
	
	public TextAsset modelText;
	public TextAsset aniText;
	public TextAsset weaponText;
	
	public GameObject gunTarget;
	private GameObject goWeapon;
	
	
	string[] modelList;
	string[] aniList;
	string[] weaponList;
	
	string pathModel = "FPS_CharacterPack_F/Models Legacy/";
	string pathAni = "FPS_CharacterPack_F/Animations Legacy/";
	
	public string aniModelName;
	
	public GUIText txtModelName;
	public GUIText txtAniName;
	
	public float buttonWidth = 20f;
	public float buttonHeight = 20f;
	public float buttonOffsetX = 80f;
	public float buttonOffsetY = 80f;
	public float buttonGab = 20f;
	
	List<GameObject> modelObjectList = new List<GameObject>();
	List<GameObject> modelWeaponList = new List<GameObject>();
	
	int currentModelNum = 0;
	int currentAniNum = 0;
	int currentWeaponNum = 0;
	
	// Use this for initialization
	void Start () {
		
		aniList = aniText.text.Split('\n');
		modelList = modelText.text.Split('\n');
		weaponList = weaponText.text.Split('\n');
		
		MakeCharacterModel();
		
		
		
		ShowModel();
	}
	
	void MakeCharacterModel( ) {
		
		for ( int i = 0; i < modelList.Length; i++ ) {
			
			string modelName = pathModel + modelList[i];
			
			GameObject modelPrefab = Resources.Load(modelName) as GameObject;
			GameObject model = Instantiate( modelPrefab, transform.position, transform.rotation ) as GameObject;
			
			foreach (SkinnedMeshRenderer smr
			         in model.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				//Debug.Log( " Bone Length: " + smr.bones.Length );
				//for( int i = 0; i < smr.bones.Length; ++i ){
				//         Debug.Log( smr.name );
				//}
				//				if (!smr.enabled) continue;
				//				if (!smr.name.Contains("HAND")) continue;
				
				foreach (Transform t in smr.bones)
				{
					//Debug.Log( t.name );                                     
					if (t.name == "Bip01 R Hand")
					{   goWeapon = Instantiate(gunTarget) as GameObject;
						goWeapon.name = "TestWeapon";
						goWeapon.transform.position = t.position;
						goWeapon.transform.rotation = t.rotation;
						
						goWeapon.transform.parent = t;
						//goWeapon.transform.localScale = new Vector3(20,20,20);
						
						
					}
				}
				//Debug.Log( " Bind poses Length: " + smr.sharedMesh.bindposes.Length );
				
				//				if (smr.bones.Length != smr.sharedMesh.bindposes.Length)
				//				{
				//					Debug.Log("Error: size of bones and bindposes are different");
				//					
				//				}
			}
			
			//			Transform bip01 = model.transform.FindChild("Bip01").transform;
			//			GameObject gun01 = gunTarget;
			//			GameObject gunch = Instantiate(gun01 , bip01.transform.position, bip01.transform.rotation ) as GameObject;
			//			gunch.transform.parent = bip01.transform;
			
			
			for ( int j = 0; j < aniList.Length; j++ ) {
				
				GameObject nClip = Resources.Load( pathAni + aniModelName + "@" + aniList[j] ) as GameObject;
				nClip.animation.clip.wrapMode = WrapMode.Loop;
				model.animation.AddClip( nClip.animation.clip, aniList[j] );
				
			}
			
			model.transform.parent = transform;
			model.SetActive( false );
			modelObjectList.Add( model );
			
		}
		
	}
	
	void HideAllModels() {
		
		for ( int i = 0; i < modelObjectList.Count; i++ ) {
			
			modelObjectList[i].SetActive( false );
		}
	}
	
	void ShowModel() {
		
		HideAllModels();
		modelObjectList[currentModelNum].SetActive( true );
		modelObjectList[currentModelNum].animation.Play( aniList[currentAniNum] );
		
		txtModelName.text = modelList[currentModelNum];
		txtAniName.text = aniList[currentAniNum];
	}
	
	void OnGUI() {
		
		if ( GUI.Button( new Rect( buttonOffsetX, buttonOffsetY, buttonWidth, buttonHeight ), "<" ) ) {
			
			if (currentModelNum > 0)
				currentModelNum--;
			else
				currentModelNum = modelList.Length - 1;
			
			ShowModel();
		}
		
		if ( GUI.Button( new Rect( buttonOffsetX + buttonWidth + buttonGab, buttonOffsetY , buttonWidth, buttonHeight ), ">" ) ) {
			
			if (currentModelNum < modelList.Length - 1)
				currentModelNum++;
			else
				currentModelNum = 0;
			
			ShowModel();
			
		}
		
		
		if ( GUI.Button( new Rect( buttonOffsetX, buttonOffsetY + buttonHeight + buttonGab, buttonWidth, buttonHeight ), "<" ) ) {
			
			if (currentAniNum > 0)
				currentAniNum--;
			else
				currentAniNum = aniList.Length - 1;
			
			ShowModel();
			
		}
		
		if ( GUI.Button( new Rect( buttonOffsetX + buttonWidth + buttonGab, buttonOffsetY + buttonHeight + buttonGab, buttonWidth, buttonHeight ), ">" ) ) {
			
			if (currentAniNum < aniList.Length - 1)
				currentAniNum++;
			else
				currentAniNum = 0;
			
			ShowModel();
		}
		
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
}


