using System.Collections.Generic;

namespace KaySquadron
{
    public static class SquadronData
    {
        public enum SquadronClass
        {
            Gladiator = 1,
            Pugilist = 2,
            Marauder = 3,
            Lancer = 4,
            Archer = 5,
            Conjurer = 6,
            Thaumaturge = 7,
            Arcanist = 26,
            Rogue = 29
        }

        public static Attributes GetBaseStats(SquadronClass job, int level)
        {
            // Approximate formula: 100 base + 5 per level
            int total = 100 + (level * 5);
            
            return job switch
            {
                SquadronClass.Gladiator or SquadronClass.Marauder => new Attributes((int)(total * 0.6), (int)(total * 0.15), (int)(total * 0.25)),
                SquadronClass.Conjurer => new Attributes((int)(total * 0.15), (int)(total * 0.65), (int)(total * 0.2)),
                SquadronClass.Archer or SquadronClass.Rogue or SquadronClass.Lancer => new Attributes((int)(total * 0.2), (int)(total * 0.2), (int)(total * 0.6)),
                SquadronClass.Thaumaturge or SquadronClass.Arcanist or SquadronClass.Pugilist => new Attributes((int)(total * 0.25), (int)(total * 0.4), (int)(total * 0.35)),
                _ => new Attributes(total / 3, total / 3, total / 3)
            };
        }

        public static List<SquadronMission> GetMissions()
        {
            var list = new List<SquadronMission>();
            
            try
            {
                var sheet = Plugin.Data.GetExcelSheet<Lumina.Excel.Sheets.GcArmyExpedition>();
                var typeSheet = Plugin.Data.GetExcelSheet<Lumina.Excel.Sheets.GcArmyExpeditionType>();
                
                if (sheet != null)
                {
                    foreach (var row in sheet)
                    {
                        if (row.RequiredLevel == 0) continue; // Skip empty rows

                        string name = "Mission " + row.RowId;
                        
                        // Try to get the real mission name
                        try 
                        {
                            string realName = row.Name.ToString();
                            if (!string.IsNullOrEmpty(realName))
                            {
                                name = realName;
                            }
                        } catch { }
                        
                        list.Add(new SquadronMission 
                        { 
                            Id = (uint)row.RowId, 
                            Name = $"[Niv. {row.RequiredLevel}] {name}",
                            LevelRequirement = (int)row.RequiredLevel, 
                            RequiredAttributes = new Attributes(
                                row.ExpeditionParams[0].RequiredPhysical, 
                                row.ExpeditionParams[0].RequiredMental, 
                                row.ExpeditionParams[0].RequiredTactical)
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.Error(Loc.T("ErrLuminaLoad", ex.Message));
            }

            // Fallback if sheet fails or is empty
            if (list.Count == 0)
            {
                list.Add(new SquadronMission { Id = 1, Name = Loc.T("MissionNameFallback"), LevelRequirement = 1, RequiredAttributes = new Attributes(145, 140, 140) });
            }

            return list;
        }
    }
}
