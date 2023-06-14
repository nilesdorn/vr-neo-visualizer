using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using SimpleFileBrowser;
using TMPro;

public class NEOManager : MonoBehaviour
{
    // NEO prefab reference
    public GameObject neoPrefab;
    // Current targets menu item prefab reference
    public GameObject targetMenuItem;
    // current targets menu reference
    public GameObject targetMenu;
    public GameObject neoParent;
    // UI input field reference
    public InputField targetInput;
    // API url
    private string url;
    // resulting JSON from API request
    public JSONNode jsonResult;
    // instance
    public static NEOManager instance;
    
    private TouchScreenKeyboard keyboard;
    List<GameObject> currentTargets = new List<GameObject>();
    List<GameObject> currentTargetsUI = new List<GameObject>();
    List<String> fileTargets = new List<String>();

    public GameObject browserCanvas;
    public GameObject rotationDevice;


    AudioSource audioData;

    void Awake()
    {
        // set the instance to be this script
        instance = this;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateController();
    }

    
    
     public void RotateController()
    {
        transform.rotation = rotationDevice.transform.rotation;
    }
    

    // sends an API request - returns a JSON file and instantiates NEO object
    IEnumerator CreateNEO(string designation)
    {
        // create the web request and download handler
        UnityWebRequest webReq = new UnityWebRequest();
        webReq.downloadHandler = new DownloadHandlerBuffer();

       
        
        
        // build the url and query
        webReq.url = string.Format("https://ssd-api.jpl.nasa.gov/sbdb.api?sstr={0}", designation);

        // send the web request and wait for a returning result
        yield return webReq.SendWebRequest();

        // convert the byte array and wait for a returning result
        string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);

        // parse the raw string into a json result we can easily read
        jsonResult = JSON.Parse(rawJson);
        Debug.Log(jsonResult);
        
        // instantiate the object
        GameObject neoController = Instantiate(neoPrefab, neoParent.transform);
        neoController.name = designation + "Controller";
        GameObject neo = neoController.GetComponent<Transform>().GetChild(0).gameObject;
        Debug.Log(designation);
        neo.name = designation;
        neo.GetComponent<SelectObjectInScene>().designation = designation;
        GameObject nameplate = neo.transform.GetChild(0).gameObject;
        //Debug.Log(nameplate);
        nameplate.GetComponent<TextMeshPro>().text = designation;



        // access the OrbitingBody.cs script and set element values
        var orbitingBody = neo.GetComponent<OrbitingBody>();
        Debug.Log(orbitingBody);
        orbitingBody.e = jsonResult["orbit"]["elements"][0]["value"];
        orbitingBody.a = jsonResult["orbit"]["elements"][1]["value"];
        orbitingBody.i = jsonResult["orbit"]["elements"][3]["value"];
        orbitingBody.node = jsonResult["orbit"]["elements"][4]["value"];
        orbitingBody.peri = jsonResult["orbit"]["elements"][5]["value"];
        orbitingBody.tp = jsonResult["orbit"]["elements"][7]["value"];
        orbitingBody.n = jsonResult["orbit"]["elements"][9]["value"];

        //neoController.transform.SetParent(neoParent.transform);
        

        GameObject menuItem = Instantiate(targetMenuItem, targetMenu.transform);
        
        menuItem.GetComponent<UnityEngine.UI.Text>().text = designation; 
        menuItem.GetComponent<TargetInfoManager>().designation = designation;

        menuItem.name = designation + "UI";
        currentTargets.Add(neoController);
        currentTargetsUI.Add(menuItem);
        

        //neoController.GetComponent<TrailRenderer_Local>().enabled = true;
        
    }

    IEnumerator CreateTargets(string[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            yield return StartCoroutine(CreateNEO(targets[i]));
            Debug.Log(targets[i]);
        }
    }

    // wrapper for UI interaction
    public void AddTargets()
    {
        
        // get the user input for target name
        string userInput = targetInput.text;
        Debug.Log(userInput);
        // split user input by line and add to a list
        string[] targets = userInput.Split (new string[] {Environment.NewLine}, StringSplitOptions.None);
        
        StartCoroutine(CreateTargets(targets));
        
    }

    public void RemoveTarget()
    {
        // get the user input for target name
        string designation = targetInput.text;
        GameObject neo = GameObject.Find(designation + "Controller");
        currentTargets.Remove(neo);
        Destroy(neo);
        
        GameObject menuItem = GameObject.Find(designation + "UI");
        currentTargetsUI.Remove(menuItem);
        Destroy(menuItem);
    }

    public void RemoveAllTargets()
    {
        foreach (GameObject target in currentTargets)
        {
            
            Destroy(target);
           
        }
        currentTargets.RemoveAll(s => s == null);

        foreach (GameObject menuItem in currentTargetsUI)
        {
            
            Destroy(menuItem);
        }
        currentTargetsUI.RemoveAll(s => s == null);
    }


    public void UploadFile()
    {
        browserCanvas.GetComponent<Canvas>().enabled = true;
        
        // Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Text Files", ".txt", ".csv" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter( ".csv" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		StartCoroutine( ShowLoadDialogCoroutine() );
    }


    IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                Debug.Log(FileBrowser.Result[i]);
                string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			    Debug.Log(destinationPath);
                FileBrowserHelpers.CopyFile( FileBrowser.Result[i], destinationPath );
                StreamReader strReader = new StreamReader(destinationPath);

                string line = String.Empty;
                
                while ((line = strReader.ReadLine()) != null)
                {
                    Debug.Log(line);
                    fileTargets.Add(line);
                    yield return StartCoroutine(CreateNEO(line));
                    Debug.Log("coroutien done");
                }
            }



			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			//byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			// Or, copy the first file to persistentDataPath
			
            

            

		}

        browserCanvas.GetComponent<Canvas>().enabled = false;
	}

    

    
}


