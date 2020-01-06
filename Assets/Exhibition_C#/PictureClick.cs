using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using NGUI;

public class PictureClick : MonoBehaviour {

    //点击图片
    // Use this for initialization
   
    //按钮1
    public void Click1()
    {
        Judge(1);
    }
    //按钮2
    public void Click2()
    {
        Judge(2);
    }
    //按钮3
    public void Click3()
    {
        Judge(3);
    }
    //按钮4
    public void Click4()
    {
        Judge(4);
    }
    //用于判断用户按了哪个按钮。然后通过判断按钮实例化物体
    private void Judge(int count)
    {
        
        switch (UIFlash.sort)
        {
            case "fumiture":
                Fumiture(count);
                break;
            case "build":
                Build(count);
                break;
            case "video":
                Video(count);
                break;
            case "picture":
                Picture(count);
                break;
        }
    }
    private void Fumiture(int count)
    {
        switch (count)
        { 
            case 1:
                destroy();
                GameObject instance = (GameObject)Instantiate(Resources.Load("Myprefabs/"+UIFlash.sort + count), new Vector3(0,-2,6), Quaternion.Euler(-90,0,0));
                instance.name = "prefabs";
                break;
            case 2:
                destroy();
                GameObject instance1 = (GameObject)Instantiate(Resources.Load("Myprefabs/" + UIFlash.sort + count), new Vector3(0,4,6), Quaternion.Euler(-90, 0, 0));
                instance1.name = "prefabs";
                break;
            case 3:
                destroy();
                GameObject instance2 = (GameObject)Instantiate(Resources.Load("Myprefabs/" + UIFlash.sort + count), new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0));
                instance2.name = "prefabs";
                break;
            //case 4:
            //    break;
        }
            
    }
    private void Build(int count)
    {
        switch (count)
        {
            case 1:
                destroy();
                GameObject instance3 = (GameObject)Instantiate(Resources.Load("Myprefabs/" + UIFlash.sort + count), new Vector3(0, -10, 26), Quaternion.identity);
                instance3.name = "prefabs";
                break;
            //case 2:
            //    break;
            //case 3:
            //    break;
            //case 4:
            //    break;
        }
    }
    private void Video(int count)
    {
        destroy();
        transform.Find("Video").gameObject.SetActive(true);//激活视频展示栏
        transform.Find("Video").GetComponent<VideoPlayer>().url=Application.dataPath+"/video/video1.mp4";
        transform.Find("视频播放暂停").GetComponent<TweenScale>().PlayForward();//激活视频播放按钮UI
    }
        
    private void Picture(int count)
    {
       destroy();
       transform.Find("Picture").GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(UIFlash.sort + count);
       transform.Find("Picture").GetComponent<TweenAlpha>().PlayForward();//激活图片展示栏
        PictureIns(count);
    }

    //视频播放与暂停按钮
    public void IsPlayVideo()
    {
        if(transform.Find("Video").GetComponent<VideoPlayer>().isPlaying==false)
        {
            transform.Find("Video").GetComponent<VideoPlayer>().Play();
            transform.Find("视频播放暂停").GetComponent<UISprite>().spriteName = "暂停";
        }else
        {
            transform.Find("Video").GetComponent<VideoPlayer>().Pause();
            transform.Find("视频播放暂停").GetComponent<UISprite>().spriteName = "播放";
        }
    }


    //切换不同的图片时，图片的介绍也会变化
    private void PictureIns(int count)
    {
        switch (count)
        {
            case 1:
                transform.Find("Picture/Label").GetComponent<UILabel>().text = "婺源古村落的建筑，是当今中国古建筑保存最多、最完好的地方之一。古徽州之一。全县至今仍完好地保存着明清时代的古祠堂113座、古府第28栋、古民宅36幢和古桥187座。村庄一般都选择在前有流水、后靠青山的地方。";
                break;
            case 2:
                transform.Find("Picture/Label").GetComponent<UILabel>().text = "村前的小河、水口山、水口林和村后的后龙山上的林木，历来得到村民悉心的保护，谁要是砍了山上的一竹一木，就要受到公众的谴责和乡规民约的处罚。自1992年建立自然保护区后，河流、林木、古民宅、古树、古桥、古祠堂、古府第、古楼台、古碑和珍禽飞鸟保护得更好了，成了全国\"生态文化旅游示范县\"。";
                break;
            case 3:
                transform.Find("Picture/Label").GetComponent<UILabel>().text = "江西婺源地处赣东北，与皖南、浙西毗邻，已被国内外誉为\"中国最美丽的农村\"。婺源古村落除了山川的峰峦、幽谷、晒秋、花海梯田、溪涧、林木、奇峰、异石、古树、驿道、亭台、廊桥、溶洞和鸟类奇多之外，就是古村落古民居建筑堪称九州大地之一绝。";
                break;
            case 4:
                transform.Find("Picture/Label").GetComponent<UILabel>().text = "“古树高低屋，斜阳远近山，林梢烟似带，村外水如环。”晓起村被誉为天人合一的生态家园，明清建筑众多，徽派民居遍布其中。清一色的青砖灰瓦，朴实素雅。高峻的马头墙，仰天昂起。雕刻、彩绘，精致细腻，让人赏心悦目。数以百计的屋宇，堂上有匾，门旁有联。联匾皆有来历，意境深远。";
                break;
        }
    }

    //摧毁之前的实例化物体，防止挡住
    private void destroy()
    {
        Destroy(GameObject.Find("prefabs"));
    }
}


