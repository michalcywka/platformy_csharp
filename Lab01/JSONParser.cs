﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
namespace Lab01
{
    public class JSONParser
    {
        public string text;
        public int number;

        public static List<string> apiList = new List<string> {
                "https://cat-fact.herokuapp.com/facts/random", "https://api.coinranking.com/v1/public/coins?base=PLN&timePeriod=7d", "https://randomfox.ca/floof/" };

        public static Person ParseJSON(string result, string apiUrl)
        {
            
            JToken token = JObject.Parse(result);
            if (apiUrl == apiList[0])
            {
                string text = (string)token.SelectToken("text");
                return new Person() { Name = text };
            }

            else if (apiUrl == apiList[1])
            {
                string text = (string)token.SelectToken("data.coins[0].symbol");
                string number = (string)token.SelectToken("data.coins[0].price");
                return new Person()
                {
                    Name = text,
                    Age = float.Parse(number, System.Globalization.CultureInfo.InvariantCulture)
                };
            }

            else if (apiUrl == apiList[2])
            {
                string image = (string)token.SelectToken("image");
                BitmapImage img = new BitmapImage();
                using (WebClient client = new WebClient())
                {
                    byte[] pic = client.DownloadData(image);
                

                MemoryStream strmImg = new MemoryStream(pic);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                img = myBitmapImage;
                }
                return new Person()
                {
                    Picture = img
                };
            }

            else return new Person();
        }
    }
}