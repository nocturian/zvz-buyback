﻿using System;
using System.Collections.Generic;

namespace zvz_buyback
{
    class AlbionEvent
    {
        public int EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string KillArea { get; set; }
        public string Type { get; set; }
        public AlbionPlayer Killer { get; set; }
        public AlbionPlayer Victim { get; set; }

        public override string ToString()
        {
            return Timestamp.ToShortDateString() + "" + Timestamp.ToShortTimeString() + $"-> {Killer} zabił " + Victim;
        }
    }

    class AlbionPlayer
    {
        public string Name { get; set; }
        public string GuildName { get; set; }
        public string AllianceName { get; set; }
        public AlbionPlayerEquipment Equipment { get; set; }
        public double AverageItemPower { get; set; }

        public override string ToString()
        {
            return Name + (string.IsNullOrWhiteSpace(AllianceName) ? "" : $"[{AllianceName}]") + (string.IsNullOrWhiteSpace(GuildName) ? "" : $"({GuildName})") + "IP:" + (int)AverageItemPower;
        }
    }


    class AlbionPlayerEquipment
    {
        public AlbionEquipment MainHand { get; set; }
        public AlbionEquipment OffHand { get; set; }
        public AlbionEquipment Head { get; set; }
        public AlbionEquipment Armor { get; set; }
        public AlbionEquipment Shoes { get; set; }
        public AlbionEquipment Bag { get; set; }
        public AlbionEquipment Cape { get; set; }
        public AlbionEquipment Mount { get; set; }
        public AlbionEquipment Potion { get; set; }
        public AlbionEquipment Food { get; set; }

        public override string ToString()
        {
            return "" + (MainHand == null ? "" : MainHand.ToString() + Environment.NewLine) +
                (OffHand == null ? "" : OffHand.ToString() + Environment.NewLine) +
                 (Head == null ? "" : Head.ToString() + Environment.NewLine) +
                 (Armor == null ? "" : Armor.ToString() + Environment.NewLine) +
                 (Shoes == null ? "" : Shoes.ToString() + Environment.NewLine);
        }
    }

    class AlbionEquipment
    {
        private static Dictionary<string, string> TierToString = new Dictionary<string, string>
        {
            { "T1", "Beginner's"},
            { "T2", "Novice's"},
            { "T3", "Journeyman's"},
            { "T4", "Adept's"},
            { "T5", "Expert's"},
            { "T6", "Master's"},
            { "T7", "Grandmaster's"},
            { "T8", "Elder's"},
        };

        private static string[] Qualities = new string[] { "All", "Normal", "Good", "Outstanding", "Excellent", "Masterpiece" };

        private static Dictionary<string, string> KeyToString = new Dictionary<string, string>
        {
            { "ARMOR_CLOTH_SET1","Scholar Robe"},
            { "ARMOR_CLOTH_SET2","Cleric Robe"},
            { "ARMOR_CLOTH_SET3","Mage Robe"},
            { "ARMOR_CLOTH_KEEPER","Druid Robe"},
            { "ARMOR_CLOTH_HELL","Fiend Robe"},
            { "ARMOR_CLOTH_MORGANA","Cultist Robe"},
            { "ARMOR_CLOTH_ROYAL","Royal Robe"},
            { "ARMOR_CLOTH_AVALON","Robe of Purity"},
            { "HEAD_CLOTH_SET1","Scholar Cowl"},
            { "HEAD_CLOTH_SET2","Cleric Cowl"},
            { "HEAD_CLOTH_SET3","Mage Cowl"},
            { "HEAD_CLOTH_KEEPER","Druid Cowl"},
            { "HEAD_CLOTH_HELL","Fiend Cowl"},
            { "HEAD_CLOTH_MORGANA","Cultist Cowl"},
            { "HEAD_CLOTH_ROYAL","Royal Cowl"},
            { "HEAD_CLOTH_AVALON","Cowl of Purity"},
            { "SHOES_CLOTH_SET1","Scholar Sandals"},
            { "SHOES_CLOTH_SET2","Cleric Sandals"},
            { "SHOES_CLOTH_SET3","Mage Sandals"},
            { "SHOES_CLOTH_KEEPER","Druid Sandals"},
            { "SHOES_CLOTH_HELL","Fiend Sandals"},
            { "SHOES_CLOTH_MORGANA","Cultist Sandals"},
            { "SHOES_CLOTH_ROYAL","Royal Sandals"},
            { "SHOES_CLOTH_AVALON","Sandals of Purity"},
            { "MAIN_ARCANESTAFF","Arcane Staff"},
            { "2H_ARCANESTAFF","Great Arcane Staff"},
            { "2H_ENIGMATICSTAFF","Enigmatic Staff"},
            { "MAIN_ARCANESTAFF_UNDEAD","Witchwork Staff"},
            { "2H_ARCANESTAFF_HELL","Occult Staff"},
            { "2H_ENIGMATICORB_MORGANA","Malevolent Locus"},
            { "MAIN_CURSEDSTAFF","Cursed Staff"},
            { "2H_CURSEDSTAFF","Great Cursed Staff"},
            { "2H_DEMONICSTAFF","Demonic Staff"},
            { "MAIN_CURSEDSTAFF_UNDEAD","Lifecurse Staff"},
            { "2H_SKULLORB_HELL","Cursed Skull"},
            { "2H_CURSEDSTAFF_MORGANA","Damnation Staff"},
            { "MAIN_FIRESTAFF","Fire Staff"},
            { "2H_FIRESTAFF","Great Fire Staff"},
            { "2H_INFERNOSTAFF","Infernal Staff"},
            { "MAIN_FIRESTAFF_KEEPER","Wildfire Staff"},
            { "2H_FIRESTAFF_HELL","Brimstone Staff"},
            { "2H_INFERNOSTAFF_MORGANA","Blazing Staff"},
            { "MAIN_FROSTSTAFF","Frost Staff"},
            { "2H_FROSTSTAFF","Great Frost Staff"},
            { "2H_GLACIALSTAFF","Glacial Staff"},
            { "MAIN_FROSTSTAFF_KEEPER","Hoarfrost Staff"},
            { "2H_ICEGAUNTLETS_HELL","Icicle Staff"},
            { "2H_ICECRYSTAL_UNDEAD","Permafrost Prism"},
            { "MAIN_HOLYSTAFF","Holy Staff"},
            { "2H_HOLYSTAFF","Great Holy Staff"},
            { "2H_DIVINESTAFF","Divine Staff"},
            { "MAIN_HOLYSTAFF_MORGANA","Lifetouch Staff"},
            { "2H_HOLYSTAFF_HELL","Fallen Staff"},
            { "2H_HOLYSTAFF_UNDEAD","Redemption Staff"},
            { "OFF_BOOK","Tome of Spells"},
            { "OFF_ORB_MORGANA","Eye of Secrets"},
            { "OFF_DEMONSKULL_HELL","Muisak"},
            { "OFF_TOTEM_KEEPER","Taproot"},
            { "ARMOR_LEATHER_SET1","Mercenary Jacket"},
            { "ARMOR_LEATHER_SET2","Hunter Jacket"},
            { "ARMOR_LEATHER_SET3","Assassin Jacket"},
            { "ARMOR_LEATHER_MORGANA","Stalker Jacket"},
            { "ARMOR_LEATHER_HELL","Hellion Jacket"},
            { "ARMOR_LEATHER_ROYAL","Royal Jacket"},
            { "ARMOR_LEATHER_UNDEAD","Specter Jacket"},
            { "ARMOR_LEATHER_AVALON","Jacket of Tenacity"},
            { "HEAD_LEATHER_SET1","Mercenary Hood"},
            { "HEAD_LEATHER_SET2","Hunter Hood"},
            { "HEAD_LEATHER_SET3","Assassin Hood"},
            { "HEAD_LEATHER_MORGANA","Stalker Hood"},
            { "HEAD_LEATHER_HELL","Hellion Hood"},
            { "HEAD_LEATHER_ROYAL","Royal Hood"},
            { "HEAD_LEATHER_UNDEAD","Specter Hood"},
            { "HEAD_LEATHER_AVALON","Hood of Tenacity"},
            { "SHOES_LEATHER_SET1","Mercenary Shoes"},
            { "SHOES_LEATHER_SET2","Hunter Shoes"},
            { "SHOES_LEATHER_SET3","Assassin Shoes"},
            { "SHOES_LEATHER_MORGANA","Stalker Shoes"},
            { "SHOES_LEATHER_HELL","Hellion Shoes"},
            { "SHOES_LEATHER_ROYAL","Royal Shoes"},
            { "SHOES_LEATHER_UNDEAD","Specter Shoes"},
            { "SHOES_LEATHER_AVALON","Shoes of Tenacity"},
            { "2H_BOW","Bow"},
            { "2H_BOW_HELL","Wailing Bow"},
            { "2H_BOW_KEEPER","Bow of Badon"},
            { "2H_LONGBOW","Longbow"},
            { "2H_LONGBOW_UNDEAD","Whispering Bow"},
            { "2H_WARBOW","Warbow"},
            { "2H_COMBATSTAFF_MORGANA","Black Monk Stave"},
            { "2H_DOUBLEBLADEDSTAFF","Double Bladed Staff"},
            { "2H_IRONCLADEDSTAFF","Iron-clad Staff"},
            { "2H_QUARTERSTAFF","Quarterstaff"},
            { "2H_ROCKSTAFF_KEEPER","Staff of Balance"},
            { "2H_TWINSCYTHE_HELL","Soulscythe"},
            { "2H_GLAIVE","Glaive"},
            { "2H_HARPOON_HELL","Spirithunter"},
            { "2H_SPEAR","Pike"},
            { "2H_TRIDENT_UNDEAD","Trinity Spear"},
            { "MAIN_SPEAR","Spear"},
            { "MAIN_SPEAR_KEEPER","Heron Spear"},
            { "MAIN_DAGGER","Dagger"},
            { "2H_DAGGERPAIR","Dagger Pair"},
            { "2H_CLAWPAIR","Claws"},
            { "MAIN_RAPIER_MORGANA","Bloodletter"},
            { "2H_IRONGAUNTLETS_HELL","Black Hands"},
            { "2H_DUALSICKLE_UNDEAD","Deathgivers"},
            { "MAIN_NATURESTAFF","Nature Staff"},
            { "2H_NATURESTAFF","Great Nature Staff"},
            { "2H_WILDSTAFF","Wild Staff"},
            { "MAIN_NATURESTAFF_KEEPER","Druidic Staff"},
            { "2H_NATURESTAFF_HELL","Blight Staff"},
            { "2H_NATURESTAFF_KEEPER","Rampant Staff"},
            { "OFF_TORCH","Torch"},
            { "OFF_HORN_KEEPER","Mistcaller"},
            { "OFF_JESTERCANE_HELL","Leering Cane"},
            { "OFF_LAMP_UNDEAD","Cryptcandle"},
            { "ARMOR_PLATE_SET1","Soldier Armor"},
            { "ARMOR_PLATE_SET2","Knight Armor"},
            { "ARMOR_PLATE_SET3","Guardian Armor"},
            { "ARMOR_PLATE_UNDEAD","Graveguard Armor"},
            { "ARMOR_PLATE_HELL","Demon Armor"},
            { "ARMOR_PLATE_KEEPER","Judicator Armor"},
            { "ARMOR_PLATE_ROYAL","Royal Armor"},
            { "ARMOR_PLATE_AVALON","Armor of Valor"},
            { "HEAD_PLATE_SET1","Soldier Helmet"},
            { "HEAD_PLATE_SET2","Knight Helmet"},
            { "HEAD_PLATE_SET3","Guardian Helmet"},
            { "HEAD_PLATE_UNDEAD","Graveguard Helmet"},
            { "HEAD_PLATE_HELL","Demon Helmet"},
            { "HEAD_PLATE_KEEPER","Judicator Helmet"},
            { "HEAD_PLATE_ROYAL","Royal Helmet"},
            { "HEAD_PLATE_AVALON","Helmet of Valor"},
            { "SHOES_PLATE_SET1","Soldier Boots"},
            { "SHOES_PLATE_SET2","Knight Boots"},
            { "SHOES_PLATE_SET3","Guardian Boots"},
            { "SHOES_PLATE_UNDEAD","Graveguard Boots"},
            { "SHOES_PLATE_HELL","Demon Boots"},
            { "SHOES_PLATE_KEEPER","Judicator Boots"},
            { "SHOES_PLATE_ROYAL","Royal Boots"},
            { "SHOES_PLATE_AVALON","Boots of Valor"},
            { "MAIN_AXE","Battleaxe"},
            { "2H_AXE","Greataxe"},
            { "2H_HALBERD","Halberd"},
            { "2H_HALBERD_MORGANA","Carrioncaller"},
            { "2H_SCYTHE_HELL","Infernal Scythe"},
            { "2H_DUALAXE_KEEPER","Bear Paws"},
            { "MAIN_HAMMER","Hammer"},
            { "2H_HAMMER","Great Hammer"},
            { "2H_POLEHAMMER","Polehammer"},
            { "2H_HAMMER_UNDEAD","Tombhammer"},
            { "2H_DUALHAMMER_HELL","Forge Hammers"},
            { "2H_RAM_KEEPER","Grovekeeper"},
            { "MAIN_MACE","Mace"},
            { "2H_MACE","Heavy Mace"},
            { "2H_FLAIL","Morning Star"},
            { "MAIN_ROCKMACE_KEEPER","Bedrock Mace"},
            { "MAIN_MACE_HELL","Incubus Mace"},
            { "2H_MACE_MORGANA","Camlann Mace"},
            { "MAIN_SWORD","Broadsword"},
            { "2H_DUALSWORD","Dual Swords"},
            { "2H_CLAYMORE","Claymore"},
            { "MAIN_SCIMITAR_MORGANA","Clarent Blade"},
            { "2H_CLEAVER_HELL","Carving Sword"},
            { "2H_DUALSCIMITAR_UNDEAD","Galatine Pair"},
            { "OFF_SHIELD","Shield"},
            { "OFF_TOWERSHIELD_UNDEAD","Sarcophagus"},
            { "OFF_SHIELD_HELL","Caitiff Shield"},
            { "OFF_SPIKEDSHIELD_MORGANA","Facebreaker"},
            { "2H_CROSSBOW","Crossbow"},
            { "2H_CROSSBOWLARGE","Heavy Crossbow"},
            { "MAIN_1HCROSSBOW","Light Crossbow"},
            { "2H_REPEATINGCROSSBOW_UNDEAD","Weeping Repeater"},
            { "2H_DUALCROSSBOW_HELL","Boltcasters"},
            { "2H_CROSSBOWLARGE_MORGANA","Siegebow"},
        };

        public string Type { get; set; }
        public int Quality { get; set; }

        public override string ToString()
        {
            var last = Type.IndexOf('@');
            if (last == -1)
            {
                return string.Format("{0} {1}, {2}, {3}", TierToString[Type.Substring(0, 2)], KeyToString[Type.Substring(3)], 0, Qualities[Quality]);
            }
            else
            {
                return string.Format("{0} {1}, {2}, {3}", TierToString[Type.Substring(0, 2)], KeyToString[Type.Substring(3, last - 3)], Type.Substring(last + 1, 1), Qualities[Quality]);
            }
        }
    }
}


