namespace Supercell.Laser.Server.Database
{
    using MongoDB.Driver;
    using Supercell.Laser.Server.Database.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Mongo
    {
        internal static IMongoCollection<AvatarDb> Avatars;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Mongo"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the <see cref="Mongo"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Mongo.Initialized)
            {
                return;
            }

            var mongoClient = new MongoClient("mongodb://127.0.0.1:27017/");
            var mongoDb = mongoClient.GetDatabase("BrawlStars");

            Console.WriteLine($"Connected to Mongo Database at {mongoClient.Settings.Server.Host}.");
            Console.WriteLine();

            if (mongoDb.GetCollection<AvatarDb>("Avatars") == null)
            {
                mongoDb.CreateCollection("Avatars");
            }

            Mongo.Avatars = mongoDb.GetCollection<AvatarDb>("Avatars");

            Mongo.Avatars.Indexes.CreateOne(Builders<AvatarDb>.IndexKeys.Combine(
                Builders<AvatarDb>.IndexKeys.Ascending(db => db.HighID),
                Builders<AvatarDb>.IndexKeys.Descending(db => db.LowID)),

                new CreateIndexOptions
                {
                    Name = "entityIds",
                    Background = true
                }
            );

            Mongo.Initialized = true;
        }


        /// <summary>
        /// Gets the seed for the specified collection.
        /// </summary>
        internal static int AvatarSeed
        {
            get
            {
                return Mongo.Avatars.Find(db => db.HighID == 0)
                           .Sort(Builders<AvatarDb>.Sort.Descending(db => db.LowID))
                           .Limit(1)
                           .SingleOrDefault()?.LowID ?? 0;
            }
        }
    }
}
