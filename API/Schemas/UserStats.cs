﻿namespace MTCG.HttpServer.Schemas
{
    internal class UserStats
    {

        public string Name { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

    }
}
