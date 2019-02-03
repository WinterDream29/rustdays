using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.Clothes;
using Assets.Scripts.Models.Clothes.Backpack;
using Assets.Scripts.Models.Common;
using Assets.Scripts.Models.Constructions.Brick;
using Assets.Scripts.Models.Constructions.Fence;
using Assets.Scripts.Models.Constructions.Glass;
using Assets.Scripts.Models.Constructions.Stone;
using Assets.Scripts.Models.Constructions.Wood;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.Meds;
using Assets.Scripts.Models.MiningObjects.Bushes;
using Assets.Scripts.Models.MiningObjects.Rocks;
using Assets.Scripts.Models.MiningObjects.Woods;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Tools;
using Assets.Scripts.Models.Weapons;
using Assets.Scripts.Models.Decor;
using Assets.Scripts.Models.Decor.Pictures;
using Assets.Scripts.Models.Seedlings.Cabbage;
using Assets.Scripts.Models.Seedlings.Carrot;
using Assets.Scripts.Models.Seedlings.Corn;
using Assets.Scripts.Models.Seedlings.Pumpkin;
using Assets.Scripts.Models.Seedlings.Strawberry;
using Assets.Scripts.Models.Seedlings.Watermelon;

namespace Assets.Scripts.Models
{
    public static class BaseObjectFactory
    {
        private static Dictionary<string, Type> _variants = new Dictionary<string, Type>();

        public static BaseObject GetItem(Type type)
        {
            return Activator.CreateInstance(type) as BaseObject;
        }

        public static BaseObject GetItem(string typeName)
        {
            return Activator.CreateInstance(_variants[typeName]) as BaseObject;
        }

        public static void SetVariants()
        {
            //Common
            _variants["CampFire"] = typeof(CampFire);
            _variants["Furnace"] = typeof(Furnace);
            _variants["Tent"] = typeof(Tent);
            _variants["Bed"] = typeof(Bed);
            _variants["InventoryBox"] = typeof(InventoryBox);
            _variants["Chest"] = typeof(Chest);
            _variants["GardenBed"] = typeof(GardenBed);
            _variants["GardenBedStone"] = typeof(GardenBedStone);
            _variants["GardenBedBrick"] = typeof(GardenBedBrick);
            _variants["Barrel"] = typeof(Barrel);
            _variants["TorchOnWall"] = typeof(TorchOnWall);

            //Resources
            _variants["WoodResource"] = typeof(WoodResource);
            _variants["StoneResource"] = typeof(StoneResource);
            _variants["FiberResource"] = typeof(FiberResource);
            _variants["SandResource"] = typeof(SandResource);
            _variants["ClayResource"] = typeof(ClayResource);
            _variants["Bush"] = typeof(Bush);
            _variants["Rock"] = typeof(Rock);
            _variants["Stone"] = typeof(Stone);
            _variants["Fur"] = typeof(Fur);
            _variants["LeadOre"] = typeof(LeadOre);
            _variants["Leather"] = typeof(Leather);
            _variants["MetalOre"] = typeof(MetalOre);
            _variants["Stick"] = typeof(Stick);
            _variants["SulfurOre"] = typeof(SulfurOre);
            _variants["Charcoal"] = typeof(Charcoal);
            _variants["Lead"] = typeof(Lead);
            _variants["Metal"] = typeof(Metal);
            _variants["Rope"] = typeof(Rope);
            _variants["Sulfur"] = typeof(Sulfur);
            _variants["Fat"] = typeof(Fat);
            _variants["Palm"] = typeof(Palm);
            _variants["MedicinalPlant"] = typeof(MedicinalPlant);
            _variants["WoodPlank"] = typeof(WoodPlank);
            _variants["Cobblestone"] = typeof(Cobblestone);
            _variants["Brick"] = typeof(Brick);
            _variants["GlassFragments"] = typeof(GlassFragments);
            _variants["GroundResource"] = typeof(GroundResource);
            _variants["Manure"] = typeof(Manure);
            _variants["Gunpowder"] = typeof(Gunpowder);

            _variants["Carrot"] = typeof(Carrot);
            _variants["CarrotWithered"] = typeof(CarrotWithered);
            _variants["CarrotSeed"] = typeof(CarrotSeed);
            _variants["Pumpkin"] = typeof(Pumpkin);
            _variants["PumpkinWithered"] = typeof(PumpkinWithered);
            _variants["PumpkinSeed"] = typeof(PumpkinSeed);
            _variants["Corn"] = typeof(Corn);
            _variants["CornWithered"] = typeof(CornWithered);
            _variants["Popcorn"] = typeof(Popcorn);
            _variants["CornSeed"] = typeof(CornSeed);
            _variants["Watermelon"] = typeof(Watermelon);
            _variants["WatermelonWithered"] = typeof(WatermelonWithered);
            _variants["WatermelonSeed"] = typeof(WatermelonSeed);
            _variants["Cabbage"] = typeof(Cabbage);
            _variants["CabbageWithered"] = typeof(CabbageWithered);
            _variants["CabbageSeed"] = typeof(CabbageSeed);
            _variants["Strawberry"] = typeof(Strawberry);
            _variants["StrawberryWithered"] = typeof(StrawberryWithered);
            _variants["StrawberrySeed"] = typeof(StrawberrySeed);


            //Weapons & Tools
            _variants["Machete"] = typeof(Machete);
            _variants["StonePick"] = typeof(StonePick);
            _variants["SmallStoneHatchet"] = typeof(SmallStoneHatchet);
            _variants["StoneHatchet"] = typeof(StoneHatchet);
            _variants["MetalHatchet"] = typeof(MetalHatchet);
            _variants["MetalPick"] = typeof(MetalPick);
            _variants["Mace"] = typeof(Mace);
            _variants["Knife"] = typeof(Knife);
            _variants["StoneKnife"] = typeof(StoneKnife);
            _variants["Catana"] = typeof(Catana);
            _variants["Crowbar"] = typeof(Crowbar);
            _variants["Torch"] = typeof(Torch);
            _variants["Bow"] = typeof(Bow);
            _variants["Shovel"] = typeof(Shovel);
            _variants["Fishrod"] = typeof(Fishrod);
            _variants["Crossbow"] = typeof(Crossbow);
            _variants["Pistol"] = typeof(Pistol);
            _variants["Revolver"] = typeof(Revolver);
            _variants["Machinegun"] = typeof(Machinegun);
            _variants["Shotgun"] = typeof(Shotgun);
            _variants["DragunobSniperRifle"] = typeof(DragunobSniperRifle);

            //Ammo
            _variants["Arrow"] = typeof(Arrow);
            _variants["CrossbowArrow"] = typeof(CrossbowArrow);
            _variants["PistolBullet"] = typeof(PistolBullet);
            _variants["RevolverBullet"] = typeof(RevolverBullet);
            _variants["MachinegunAmmo"] = typeof(MachinegunAmmo);
            _variants["ShotgunAmmo"] = typeof(ShotgunAmmo);
            _variants["DragunovAmmo"] = typeof(DragunovAmmo);

            //Clothes
            _variants["LeatherBoots"] = typeof(LeatherBoots);
            _variants["LeatherCap"] = typeof(LeatherCap);
            _variants["LeatherShirt"] = typeof(LeatherShirt);
            _variants["LeatherPants"] = typeof(LeatherPants);
            _variants["FurBoots"] = typeof(FurBoots);
            _variants["FurCap"] = typeof(FurCap);
            _variants["FurShirt"] = typeof(FurShirt);
            _variants["FurPants"] = typeof(FurPants);
            _variants["SmallBackpack"] = typeof(SmallBackpack);
            _variants["MediumBackpack"] = typeof(MediumBackpack);
            _variants["BigBackpack"] = typeof(BigBackpack);

            //Food
            _variants["Meat"] = typeof(Meat);
            _variants["FriedMeat"] = typeof(FriedMeat);
            _variants["Fish"] = typeof(Fish);
            _variants["FriedFish"] = typeof(FriedFish);
            _variants["CoffeePack"] = typeof(CoffeePack);
            _variants["Adrenaline"] = typeof(Adrenaline);
			_variants["Apple"] = typeof(Apple);
			_variants["Banana"] = typeof(Banana);
            _variants["BottleWater"] = typeof(BottleWater);
            _variants["Chocolate"] = typeof(Chocolate);
            _variants["Jerrycan"] = typeof(Jerrycan);
            _variants["Soda"] = typeof(Soda);

            //Meds
            _variants["Medicines"] = typeof(Medicines);
            _variants["Bandage"] = typeof(Bandage);
            _variants["Medkit"] = typeof(Medkit);
            _variants["AntiRadiationPills"] = typeof(AntiRadiationPills);

            //construction
            _variants["WoodFoundation"] = typeof(WoodFoundation);
            _variants["WoodWall"] = typeof(WoodWall);
            _variants["WoodCeiling"] = typeof(WoodCeiling);
            _variants["WoodStairs"] = typeof(WoodStairs);
            _variants["WoodWallDoor"] = typeof(WoodWallDoor);
            _variants["WoodWallWindow"] = typeof(WoodWallWindow);
            _variants["WoodStreetStairs"] = typeof(WoodStreetStairs);

            _variants["StoneFoundation"] = typeof(StoneFoundation);
            _variants["StoneCeiling"] = typeof(StoneCeiling);
            _variants["StoneStairs"] = typeof(StoneStairs);
            _variants["StoneWall"] = typeof(StoneWall);
            _variants["StoneWallDoor"] = typeof(StoneWallDoor);
            _variants["StoneWallWindow"] = typeof(StoneWallWindow);

            _variants["BrickFoundation"] = typeof(BrickFoundation);
            _variants["BrickCeiling"] = typeof(BrickCeiling);
            _variants["BrickStairs"] = typeof(BrickStairs);
            _variants["BrickWall"] = typeof(BrickWall);
            _variants["BrickWallDoor"] = typeof(BrickWallDoor);
            _variants["BrickWallWindow"] = typeof(BrickWallWindow);

            _variants["GlassWall"] = typeof(GlassWall);
            _variants["GlassCeiling"] = typeof(GlassCeiling);

            _variants["WoodenFence"] = typeof(WoodenFence);

            //others
            _variants["Sofa"] = typeof(Sofa);
            _variants["Cupboard_Closed"] = typeof(Cupboard_Closed);
            _variants["CupboardRoom"] = typeof(CupboardRoom);
            _variants["Chair"] = typeof(Chair);
            _variants["CoffeeTable"] = typeof(CoffeeTable);
            _variants["KitchenTable"] = typeof(KitchenTable);
            _variants["BearRug"] = typeof(BearRug);
            _variants["SheepRug"] = typeof(SheepRug);
            _variants["MonaLisaPicture"] = typeof(MonaLisaPicture);
            _variants["MonroPicture"] = typeof(MonroPicture);
            _variants["Picture1"] = typeof(Picture1);
            _variants["Picture2"] = typeof(Picture2);
            _variants["Picture3"] = typeof(Picture3);
            _variants["Picture4"] = typeof(Picture4);
            _variants["Picture5"] = typeof(Picture5);
        }
    }
}
