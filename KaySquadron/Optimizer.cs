using System;
using System.Collections.Generic;
using System.Linq;

namespace KaySquadron
{
    public class OptimizationResult
    {
        public List<SquadronMember> Team { get; set; } = new();
        public Attributes BestTraining { get; set; }
        public int RequirementsMet { get; set; }
        public Attributes FinalStats { get; set; }
    }

    public class Optimizer
    {
        private static List<Attributes> _possibleTrainingDistributions = null!;

        static Optimizer()
        {
            InitializeTrainingDistributions();
        }

        private static void InitializeTrainingDistributions()
        {
            _possibleTrainingDistributions = new List<Attributes>();
            // 400 total points, steps of 20
            for (int p = 0; p <= 400; p += 20)
            {
                for (int m = 0; m <= 400 - p; m += 20)
                {
                    int t = 400 - p - m;
                    _possibleTrainingDistributions.Add(new Attributes(p, m, t));
                }
            }
        }

        public List<OptimizationResult> Optimize(List<SquadronMember> allMembers, SquadronMission mission)
        {
            var results = new List<OptimizationResult>();

            // Combinations of 4 members from 8
            var combinations = GetCombinations(allMembers, 4);

            foreach (var team in combinations)
            {
                var baseStats = new Attributes(0, 0, 0);
                foreach (var member in team)
                {
                    baseStats += member.BaseAttributes;
                }

                OptimizationResult? bestForThisTeam = null;

                foreach (var training in _possibleTrainingDistributions)
                {
                    var finalStats = baseStats + training;
                    int met = finalStats.RequirementsMetCount(mission.RequiredAttributes);

                    if (bestForThisTeam == null || met > bestForThisTeam.RequirementsMet)
                    {
                        bestForThisTeam = new OptimizationResult
                        {
                            Team = team.ToList(),
                            BestTraining = training,
                            RequirementsMet = met,
                            FinalStats = finalStats
                        };
                    }

                    if (met == 3) break; // Perfect match found for this team
                }

                if (bestForThisTeam != null)
                {
                    results.Add(bestForThisTeam);
                }
            }

            // Return top results ordered by requirements met, then maybe total surplus
            return results.OrderByDescending(r => r.RequirementsMet).Take(10).ToList();
        }

        private static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetCombinations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
