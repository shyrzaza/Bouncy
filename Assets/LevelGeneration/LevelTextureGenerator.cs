using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelTextureGenerator : MonoBehaviour
{

    const int HEIGHT = 256;
    const int WIDTH = 1028;

    Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(WIDTH, HEIGHT, TextureFormat.ARGB32, false);

        paintAll(new Color32(0xFE,0x7E, 0x01, 0xFF));


        Circle[] circles = GenerateCircles(12);


        paintCircles(circles, new Color32(0xFE,0x66, 0x01,0xFF));

        SaveTextureAsPNG(texture, "plat");


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void paintAll(Color color)
    {
        for(int x = 0; x < WIDTH; x++)
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

    void paintDiamond(int x, int y, int radius, Color color)
    {

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {

                if (MDist(i, j, x, y) < radius)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }

    }

    void paintCircle(int x, int y, int radius, Color color)
    {

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                Vector2 dist = new Vector2(x - i, y - j);
                if (dist.sqrMagnitude < radius * radius)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }
    }

    void paintCircle(Circle circle, Color color)
    {

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                Vector2 dist = new Vector2(circle.x - i, circle.y - j);
                if (dist.sqrMagnitude < circle.radius * circle.radius)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }
    }




    void paintCircles(Circle[] circles, Color color)
    {

        foreach(Circle circle in circles)
        {
            paintCircle(circle, color);
        }

    }

    Circle[] GenerateCircles(int num)
    {
        Circle[] circles = new Circle[num];
        

        for(int i =0; i < num; i ++)
        {
            circles[i].x = Random.Range(0, WIDTH);
            circles[i].y = Random.Range(0, HEIGHT);
            circles[i].radius = Random.Range(30f, 200f);
        }
        return circles;
    }

    public struct Circle
    {
        public int x;
        public int y;
        public float radius;
    }


    int MDist(int fromX, int fromY, int toX, int toY)
    {
        return (Mathf.Abs(fromX - toX) + Mathf.Abs(fromY - toY));
    }

    void SaveTextureAsPNG(Texture2D _texture, string name)
    {

        //then Save To Disk as PNG
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/LevelGeneration/GeneratedContent/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + name + ".png", bytes);
    }



}
