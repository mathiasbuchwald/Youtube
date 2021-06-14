﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeConsole
{
    class VideoDAO
    {
        public List<Video> getAllVideos()
        {

            //mongodb
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = dbClient.GetDatabase("youtube");
            var vids = db.GetCollection<BsonDocument>("videos");
            var documents = vids.Find(new BsonDocument()).ToList();
            List<Video> videos = new List<Video>();
            foreach (BsonDocument doc in documents)
            {
                //Console.WriteLine(doc.ToString());
                Video v = new Video(doc["title"].ToString(), doc["description"].ToString());
                videos.Add(v);
            }
            return videos;
        }

        public List<Video> searchVideos(string title)
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = dbClient.GetDatabase("youtube");
            var vids = db.GetCollection<BsonDocument>("videos");

            FilterDefinition<BsonDocument> filter = "{ title : /" + title + "/}";

            var documents = vids.Find(filter).ToList();

            List<Video> videos = new List<Video>();

            if (documents.Count > 0)
            {
                foreach (BsonDocument doc in documents)
                {
                    //Console.WriteLine(doc.ToString());
                    Video v = new Video(doc["title"].ToString(), doc["description"].ToString());
                    videos.Add(v);
                }
            }
            else
            {
                Console.WriteLine("Dieser Titel konnte nicht gefunden werden!");
            }

            return videos;
        }

        public void insertVideo(string title, string description)
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = dbClient.GetDatabase("youtube");
            var vids = db.GetCollection<BsonDocument>("videos");
            var document = new BsonDocument { 
                { "title", title}, 
                { "description", description }
            };
            try
            {
                vids.InsertOne(document);
                Console.WriteLine("Das Video wurde erfolgreich hinzugefügt!");
            }
            catch (Exception)
            {

                Console.WriteLine("Das Video konnte nicht hinzugefügt werden!");
            }
            
        }
    }
}
