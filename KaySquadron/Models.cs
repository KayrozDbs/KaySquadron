using System;

namespace KaySquadron
{
    public struct Attributes
    {
        public int Physical;
        public int Mental;
        public int Tactical;

        public Attributes(int p, int m, int t)
        {
            Physical = p;
            Mental = m;
            Tactical = t;
        }

        public static Attributes operator +(Attributes a, Attributes b) 
            => new Attributes(a.Physical + b.Physical, a.Mental + b.Mental, a.Tactical + b.Tactical);

        public int Total => Physical + Mental + Tactical;

        public bool Meets(Attributes requirement) 
            => Physical >= requirement.Physical && Mental >= requirement.Mental && Tactical >= requirement.Tactical;

        public int RequirementsMetCount(Attributes requirement)
        {
            int count = 0;
            if (Physical >= requirement.Physical) count++;
            if (Mental >= requirement.Mental) count++;
            if (Tactical >= requirement.Tactical) count++;
            return count;
        }
    }

    public class SquadronMember
    {
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Attributes BaseAttributes { get; set; }
        
        // Bonus attributes from Chemistry can be added here later
    }

    public class SquadronMission
    {
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LevelRequirement { get; set; }
        public Attributes RequiredAttributes { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsPriority { get; set; }
    }
}
