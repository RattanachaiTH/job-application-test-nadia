using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using TMPro;

public class FileManager : MonoBehaviour
{
    public static int captureHeight = 2048;
    public static int captureWidth = 2048;
    public GameObject imageObject;
    public GameObject toggleVersion;
    public GameObject pathObject;
    public GameObject lastSavedObject;
    public Camera cam;

    private string defaultPath;
    private string path;
    private string prefixName;
    private string fileType;
    private int imageCount;
    private Image image;
    private Toggle versionToggle;
    private bool save;
    private bool selectPath;
    private bool version;
    private string[] allowType;
    HashSet<string> currentImageFiles;

    void Awake()
    {
        // initial variable
        defaultPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        path = defaultPath;
        prefixName = "IOsystem_Image";
        fileType = "png";
        image = imageObject.transform.GetComponent<Image>();
        versionToggle = toggleVersion.GetComponent<Toggle>();
        version = false;
        save = false;
        selectPath = false;
        imageCount = 0;
        allowType = new string[] { ".png", ".jpg", ".PNG", ".JPG" };

        // Check files in folder
        currentImageFiles = ImageFilesSetFromPath(path);
    }

    void Update()
    {
        CheckDropedFile(path);
        CheckToggle();
        CheckInput();
        Procressing();
    }

    // Check Input
    void CheckInput()
    {
        if (Input.GetButtonDown("Enter"))
        {
            save = true;
        }
        if (Input.GetButtonDown("Space"))
        {
            selectPath = true;
        }
        pathObject.transform.GetComponent<TextMeshProUGUI>().SetText("Selected path      : " + path);
    }

    // Option in application
    void Procressing()
    {
        // When press "Enter"
        if (save == true)
        {
            CaptureScreen();
            save = false;
        }
        // When press "Enter"
        else if (selectPath == true)
        {
            string newPath = EditorUtility.OpenFolderPanel("Select new Path", path, "");
            if (newPath != "")
            {
                path = newPath;
                CaptureScreen();
                currentImageFiles = ImageFilesSetFromPath(path);
            }
            selectPath = false;
        }
    }

    void CaptureScreen()
    {
        imageCount++;
        // Capturing
        RenderTexture render = new RenderTexture(captureWidth, captureHeight, 24);
        cam.targetTexture = render;
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = render;
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(render);

        // Saving image
        byte[] bytes = screenShot.EncodeToPNG();
        string filePath = GetCapturePath();
        System.IO.File.WriteAllBytes(filePath, bytes);
        versionToggle.isOn = version;

        // show image from path in UI
        ShowImageOnScreen(filePath);
        lastSavedObject.transform.GetComponent<TextMeshProUGUI>().SetText("Last saved image: " + filePath);

        // Show saved image path
        Debug.Log(string.Format("Saved: {0}", filePath));
    }

    // Get full path for saving file
    string GetCapturePath()
    {
        if (version == true)
        {
            return string.Format("{0}/{1}_{2}.{3}", defaultPath, prefixName, imageCount, fileType);
        }
        else
        {
            return string.Format("{0}/{1}.{2}", path, prefixName, fileType);
        }
    }

    // Get Texture2d from image path
    Texture2D LoadImageFromPath(string filePath)
    {
        Texture2D image;
        byte[] data;
        if (File.Exists(filePath))
        {
            data = File.ReadAllBytes(filePath);
            image = new Texture2D(captureWidth, captureHeight);
            image.LoadImage(data);
            return image;
        }
        else
        {
            return null;
        }
    }

    HashSet<string> ImageFilesSetFromPath(string path)
    {
        // Get files from directory
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] files = directory.GetFiles("*.*");
        HashSet<string> filePathSet = new HashSet<string>();

        // Check Image file
        foreach (FileInfo file in files)
        {
            foreach (string type in allowType)
            {
                if (file.FullName.EndsWith(type))
                {
                    filePathSet.Add(file.FullName);
                    break;
                }
            }
        }
        return filePathSet;
    }

    HashSet<string> CheckNewItem(HashSet<string> oldItem, HashSet<string> newItem)
    {
        // clone newItem
        HashSet<string> newItemClone = CloneHashSet(newItem);

        // check new item
        newItemClone.ExceptWith(oldItem);
        return newItemClone;
    }

    HashSet<string> CloneHashSet(HashSet<string> set)
    {
        // clone newItem
        HashSet<string> clone = new HashSet<string>();
        foreach (string item in set)
        {
            clone.Add(item);
        }
        return clone;
    }
    void CheckDropedFile(string path)
    {
        HashSet<string> nextImageFiles = ImageFilesSetFromPath(path);
        HashSet<string> newItemSet = CheckNewItem(currentImageFiles, nextImageFiles);
        if (newItemSet.Count != 0)
        {
            List<string> itemList = new List<string>(newItemSet);
            foreach (string fullPath in itemList)
            {
                ShowImageOnScreen(fullPath);
            }
        }
        currentImageFiles = nextImageFiles;
    }
    void ShowImageOnScreen(string filePath)
    {
        Texture2D text = LoadImageFromPath(filePath);
        text = TextureResize(text, captureWidth, captureHeight);
        Sprite savedImageSprite = Sprite.Create(text, new Rect(0.0f, 0.0f, captureWidth, captureHeight), new Vector2(0.5f, 0.5f), 100.0f);
        image.sprite = savedImageSprite;
    }

    Texture2D TextureResize(Texture2D source, int width, int height)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rend = RenderTexture.GetTemporary(width, height);
        rend.filterMode = FilterMode.Point;
        RenderTexture.active = rend;
        Graphics.Blit(source, rend);
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        newTexture.Apply();
        RenderTexture.active = null;
        return newTexture;

    }


    // Check press toggle
    void CheckToggle()
    {
        if (versionToggle.isOn == true)
        {
            version = true;
        }
        else
        {
            version = false;
        }
    }
}
