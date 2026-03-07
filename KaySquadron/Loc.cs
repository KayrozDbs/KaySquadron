using Dalamud.Game;
using System.Collections.Generic;

namespace KaySquadron
{
    public static class Loc
    {
        private static ClientLanguage _currentLanguage = ClientLanguage.English;

        public static void Initialize(ClientLanguage language)
        {
            _currentLanguage = language;
        }

        private static readonly Dictionary<string, string> EnglishStrings = new()
        {
            // General
            { "PluginName", "KaySquadron - GC Squadron Optimizer" },
            { "SelectMission", "1. Choose a mission" },
            { "CurrentMissionPlaceholder", "Select a mission..." },
            { "OptimizeBtn", "Optimize Squadron" },
            { "ResetBtn", "Reset Training" },
            
            // Tabs
            { "OptimizationTab", "Optimization" },
            { "MembersTab", "Members" },
            
            // Optimization Results
            { "ImportPrompt", "Please import your squadron from the game first." },
            { "StatsNotMet", "No configuration found that meets 100% of the mission requirements." },
            { "BestMatches", "Best recommendations:" },
            { "SuccessRatio", "Success: {0}/3 - [{1}]" },
            { "BoardTrainingLabel", "Board Training (400 pts):" },
            { "Physical", "Physical" },
            { "Mental", "Mental" },
            { "Tactical", "Tactical" },
            { "StatsFinalVsRequired", "Final Stats vs Required:" },
            { "OK", "OK" },
            { "Insufficient", "Insufficient" },
            
            // Members Tab
            { "ImportGameBtn", "Import from current Squadron" },
            { "ManageMembersLabel", "Manage your 8 squadron members here." },
            
            // Error handling
            { "ErrInit", "Window initialized." },
            { "ErrNotFound", "Error: Mission list not loaded." },
            { "ErrNoMembers", "Error: No members imported." },
            { "ErrPointer", "Error: Memory pointer not found." },
            { "ErrLuminaLoad", "Error loading missions: {0}" },
            { "MissionNameFallback", "Lumina load error" }
        };

        private static readonly Dictionary<string, string> FrenchStrings = new()
        {
            // General
            { "PluginName", "KaySquadron - Optimiseur d'Escouade GC" },
            { "SelectMission", "1. Choisissez une mission" },
            { "CurrentMissionPlaceholder", "Sélectionnez une mission..." },
            { "OptimizeBtn", "Optimiser l'Escouade" },
            { "ResetBtn", "Réinitialiser l'entraînement" },
            
            // Tabs
            { "OptimizationTab", "Optimisation" },
            { "MembersTab", "Membres" },
            
            // Optimization Results
            { "ImportPrompt", "Commencez par importer votre escouade depuis le jeu." },
            { "StatsNotMet", "Aucune configuration ne permet d'atteindre 100% des prérequis pour cette mission." },
            { "BestMatches", "Meilleures recommandations :" },
            { "SuccessRatio", "Succès : {0}/3 - [{1}]" },
            { "BoardTrainingLabel", "Entraînement au tableau (400 pts) :" },
            { "Physical", "Physique" },
            { "Mental", "Mental" },
            { "Tactical", "Tactique" },
            { "StatsFinalVsRequired", "Statistiques Finales vs Requises :" },
            { "OK", "OK" },
            { "Insufficient", "Insuffisant" },
            
            // Members Tab
            { "ImportGameBtn", "Importer de l'Escouade actuelle" },
            { "ManageMembersLabel", "Gérez vos 8 membres d'escouade ici." },
            
            // Error handling
            { "ErrInit", "Initialisation de la fenêtre." },
            { "ErrNotFound", "Erreur : la liste des missions n'est pas chargée." },
            { "ErrNoMembers", "Erreur : aucun membre importé." },
            { "ErrPointer", "Erreur : Pointeur mémoire introuvable." },
            { "ErrLuminaLoad", "Erreur lors du chargement des missions : {0}" },
            { "MissionNameFallback", "Erreur de chargement de Lumina" }
        };

        public static string T(string key)
        {
            var dict = _currentLanguage == ClientLanguage.French ? FrenchStrings : EnglishStrings;
            
            if (dict.TryGetValue(key, out var val))
            {
                return val;
            }

            // Fallback to English if key is missing in French
            if (EnglishStrings.TryGetValue(key, out var fallback))
            {
                // Optionally log missing translation here if needed
                return fallback;
            }

            return key; // return the key itself if not found anywhere
        }

        public static string T(string key, params object[] args)
        {
            string format = T(key);
            try
            {
                return string.Format(format, args);
            }
            catch
            {
                return format;
            }
        }
    }
}
