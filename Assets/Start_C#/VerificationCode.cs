using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using Random = System.Random;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class VerificationCode : MonoBehaviour {
    private string text = "";
    private Bitmap image;
    private int letterWidth = 40;  //单个字体的宽度范围
    private int letterHeight = 100; //单个字体的高度范围
    private static byte[] randb = new byte[4];
    private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

    /// <summary>
    /// 字体样式 信息
    /// </summary>
    private System.Drawing.Font[] fonts = {
        new System.Drawing.Font(new FontFamily("Times New Roman"),40 +Next(1),System.Drawing.FontStyle.Regular),
        new System.Drawing.Font(new FontFamily("Georgia"), 45 + Next(1),System.Drawing.FontStyle.Regular),
        new System.Drawing.Font(new FontFamily("Arial"), 45 + Next(1),System.Drawing.FontStyle.Regular),
        new System.Drawing.Font(new FontFamily("Comic Sans MS"), 38 + Next(1),System.Drawing.FontStyle.Regular)
    };

    /// <summary>
    /// 验证码
    /// </summary>
    public string Text
    {
        get { return this.text; }
    }

    /// <summary>
    /// 验证码图片
    /// </summary>
    public System.Drawing.Bitmap Image
    {
        get { return this.image; }
    }

    public VerificationCode(int imgWidth, int imgHeight, int length)
    {
        Number(length, false);
        CreateImage(imgWidth, imgHeight);
    }


    /// <summary>
    /// 生成随机数字
    /// </summary>
    /// <param name="Length">生成长度</param>
    /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
    private string Number(int Length, bool Sleep)
    {
        if (Sleep) System.Threading.Thread.Sleep(3);
        string result = "";
        System.Random random = new System.Random();
        for (int i = 0; i < Length; i++)
        {
            result += random.Next(10).ToString();
        }
        this.text = result;
        //Debug.Log("result: " + result);
        return result;
    }

    /// <summary>
    /// 获得下一个随机数
    /// </summary>
    /// <param name="max">最大值</param>
    private static int Next(int max)
    {
        rand.GetBytes(randb);
        int value = BitConverter.ToInt32(randb, 0);
        value = value % (max + 1);
        if (value < 0) value = -value;
        return value;
    }

    /// <summary>
    /// 获得下一个随机数
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    private static int Next(int min, int max)
    {
        int value = Next(max - min) + min;
        return value;
    }


    /// <summary>
    /// 绘制验证码
    /// </summary>
    private void CreateImage(int imgWidth, int imgHeight)
    {
        image = new Bitmap(imgWidth, imgHeight);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
        //画白色背景
        g.Clear(System.Drawing.Color.White);

        //画三条 扰乱视野的线
        for (int i = 0; i < 2; i++)
        {
            int x1 = Next(imgWidth / 2);
            int x2 = Next(imgWidth / 2, imgWidth - 1);
            int y1 = Next(imgHeight / 2);
            int y2 = Next(imgHeight / 2, imgHeight - 1);
            g.DrawLine(new Pen(GetRandomColor()), x1, y1, x2, y2);
        }

        int _x = 0, _y = -30;
        for (int int_index = 0; int_index < this.text.Length; int_index++)
        {
            //随机字符的左边位置
            _x += Next(10, 80); //x坐标 累加
            _y = Next(5, 40);//y坐标 在一个范围内 随机
            string str_char = this.text.Substring(int_index, 1);
            str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
            Brush newBrush = new SolidBrush(GetRandomColor());
            Point thePos = new Point(_x, _y);
            Debug.Log("index: " + int_index + "    x: " + _x + "    y: " + _y);
            g.DrawString(str_char, fonts[Next(fonts.Length - 1)], newBrush, thePos);
        }
        for (int i = 0; i < 10; i++)
        {
            int x = Next(image.Width - 1);
            int y = Next(image.Height - 1);
            image.SetPixel(x, y, System.Drawing.Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
        }
        //image = TwistImage(image, true, Next(1, 3), Next(4, 6));
        //g.DrawRectangle(new Pen(System.Drawing.Color.LightGray, 5), 0, 0, 150 - 1, (letterHeight - 1));
    }

    /// <summary>
    /// 字体随机颜色
    /// </summary>
    public System.Drawing.Color GetRandomColor()
    {
        System.Random RandomNum_First = new System.Random((int)DateTime.Now.Ticks);
        System.Threading.Thread.Sleep(RandomNum_First.Next(50));
        System.Random RandomNum_Sencond = new System.Random((int)DateTime.Now.Ticks);
        int int_Red = RandomNum_First.Next(180);
        int int_Green = RandomNum_Sencond.Next(180);
        int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
        int_Blue = (int_Blue > 255) ? 255 : int_Blue;
        return System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue);
    }

    /// <summary>
    /// 将Image 转为 Texture
    /// </summary>
    /// <param name="im"></param>
    /// <returns></returns>
    public static Texture2D Image2Texture(System.Drawing.Image im)
    {
        if (im == null)
        {
            return new Texture2D(4, 4);
        }

        //Memory stream to store the bitmap data.
        MemoryStream ms = new MemoryStream();

        //Save to that memory stream.
        im.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

        //Go to the beginning of the memory stream.
        ms.Seek(0, SeekOrigin.Begin);
        //make a new Texture2D
        Texture2D tex = new Texture2D(im.Width, im.Height);

        tex.LoadImage(ms.ToArray());

        //Close the stream.
        ms.Close();
        im.Dispose();
        ms = null;
        //
        return tex;
    }

    /*产生验证码*/
    public string CreateCode(int codeLength)
    {
        string so = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] strArr = so.Split(',');
        string code = "";
        Random rand = new Random();
        for (int i = 0; i < codeLength; i++)
        {
            code += strArr[rand.Next(0, strArr.Length)];
        }
        return code;
    }

    /// <summary>
    /// 显示图片验证码
    /// </summary>
    /// <param name="img">显示验证码的image组件</param>
    /// <param name="imgWidth">图片宽度，150</param>
    /// <param name="imgHeight">图片高度， 60</param>
    /// <param name="length">字符长度，一般为4</param>
    /// <returns>验证码字符串</returns>
    //private static string ShowVerificationCode(UnityEngine.UI.Image img, int imgWidth, int imgHeight, int length)
    //{
    //    VerificationCode vCode = new VerificationCode(imgWidth, imgHeight, length);
    //    Texture2D texture = VerificationCode.Image2Texture(vCode.Image);
    //    img.sprite = Sprite.Create(texture, new Rect(0, 0, imgWidth, imgHeight), new Vector2(0.5f, 0.5f), 125);
    //    return vCode.Text;
    //}

    //public static void Click()
    //{
    //    GameObject Code_img = GameObject.Find("Canvas/Code_Image");
    //    string code=ShowVerificationCode(Code_img.GetComponent<UnityEngine.UI.Image>(),300,100,4);
    //    //Mysqlconn.Code = code;
    //}
    
}
