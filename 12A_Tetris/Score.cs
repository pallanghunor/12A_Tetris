using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris
{
    public class Score
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Point { get; set; }

        public Score(string data)
        {
            string[] database = data.Split(';');
            Name = database[0];
            Level = int.Parse(database[1]);
            Point = int.Parse(database[2]);       
        }
    }
}
