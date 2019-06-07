using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class L_SaveTest : MonoBehaviour
{
    public string systemTime;

    public string[] splittime;
    public string timeNumber;

    public Texture2D ScreenShot;

    private byte[] imageByte;

    public List<Sprite> newSprites;

    public string readTime;
    // Start is called before the first frame update
    void Start()
    {
        systemTime = System.DateTime.Now.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        timeNumber = systemTime.Replace('/','Z');
        timeNumber = timeNumber.Replace(':', 'Z');
        timeNumber = timeNumber.Replace(' ', 'Y');
        splittime = systemTime.Split(' ');
        /*
        if (Input.GetMouseButtonDown(1))
        {
            ScreenCapture.CaptureScreenshot("SomeLevel");
        }
        */
        Rect newRect = new Rect(0,0,800,640);
        if (Input.GetMouseButtonDown(1))
        {
            ScreenShot = CaptureCamera(Camera.main, newRect);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            int i = 0;
            //string filename = "/Project/DogDaysOfFall/DogDaysOfFall/Assets/Screenshot.png";
            string filename = Application.dataPath + "/Screenshot.png";
            //Texture2D _tex = (Texture2D)Resources.Load("Lighthouse");
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            FileInfo fi = new FileInfo(Application.dataPath + "/Screenshot.png");
            readTime = fi.LastWriteTime.ToString();
            fs.Seek(0, SeekOrigin.Begin);
            imageByte = new byte[fs.Length];
            fs.Read(imageByte, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            fs = null;
            int width = 800;
            int height = 640;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(imageByte);
            ScreenShot = texture;
            //ScreenShot = (Texture2D) Resources.Load(filename);
            ScreenShot.Apply();
            newSprites.Add(ChangeToSprite(ScreenShot));
        }
        
    }
    private Sprite ChangeToSprite(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    
    Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
        //ps: camera2.targetTexture = rt;
        //ps: camera2.Render();
        //ps: -------------------------------------------------------------------

        // 激活这个rt, 并从中中读取像素。
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));

        return screenShot;
    }
    
   
    
}
