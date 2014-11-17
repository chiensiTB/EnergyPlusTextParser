using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VectorMath;
using System.IO;
using System.Text.RegularExpressions;

namespace EnergyPlus
{
    public class EPlusObjects
    {
        #region
        public class ZoneGroup
        {
            public string name;
            EPlusObjects.ZoneList zoneList;
            public int zoneListMultiplier;
            public string zoneListName;
        }

        public class EPlusConstructions
        {
            public List<EPlusMaterials> epMaterials { get; set; }
            public List<EPlusOpaqueConst> epOpaque {get;set;}
            public List<EPlusGlazingConst> epGlazing {get;set;}
            public List<EPlusGlazing> epGlazingLayers { get; set; }
            public List<EPlusGas> epGasLayers { get; set; }

            public EPlusConstructions()
            {
                epMaterials = new List<EPlusMaterials>();
                epOpaque = new List<EPlusOpaqueConst>();
                epGlazing = new List<EPlusGlazingConst>();
                epGlazingLayers = new List<EPlusGlazing>();
                epGasLayers = new List<EPlusGas>();
            }

        }

        public class EPlusMaterials
        {
            public string name { get; set; }
            public string roughness { get; set; }
            public decimal thickness { get; set; }
            public decimal conductivity { get; set; }
            public decimal density { get; set; }
            public decimal spht { get; set; }
            public decimal thermabs { get; set; }
            public decimal solarabs { get; set; }
            public decimal visabs { get; set; }
            public decimal resistance { get; set; }
        }

        public class EPlusOpaqueConst
        {
            public string name {get;set;}
            public List<string> layernames {get;set;}

            public EPlusOpaqueConst()
            {
                layernames = new List<string>();
            }
        }

        public class EPlusGlazingConst
        {
            public string name {get;set;}
            public List<string> glazingLayers {get;set;}
            public List<string> gasLayers {get;set;}

            public EPlusGlazingConst()
            {
                glazingLayers = new List<string>();
                gasLayers = new List<string>();
            }
        }

        public class EPlusGlazing
        {
            public string name {get;set;}
            public string opticalDataType {get;set;}
            public decimal thickness {get;set;}
            public decimal solarTransmittance {get;set;}
            public decimal solarReflectanceBack {get;set;}
            public decimal solarReflectanceFront {get;set;}
            public decimal visibleTransmittance {get;set;}
            public decimal visibleReflectanceFront {get;set;}
            public decimal visibleReflectanceBack {get;set;}
            public decimal IRTransmittance {get;set;}
            public decimal IREmittanceBack {get;set;}
            public decimal IREmittanceFront {get;set;}
            public decimal conductivity { get; set; }

        }

        public class EPlusGas
        {
            public string name {get;set;}
            public decimal thickness {get;set;}
            public string gastype {get;set;}
        }

        public class ZoneList
        {
            public List<string> zoneListNames;
            public string name;
        }

        public class RadiantSurfaceGroup
        {
            public string groupName;
            public Dictionary<string, double> surfaceNameAndFF;
        }

        public class ePlusTags
        {
            //Construction definitions
            public static string constructionName = "!-Construction Name";
        }

        public class TagEndings
        {
            public class DetFenEndings
            {
                public static string Name = "!- Name";
                public static string Type = "!- Surface Type";
                public static string ConstName = "!- Construction Name";
                public static string BldSurfName = "!- Building Surface Name";
                public static string OutsideBound = "!- Outside Boundary Condition Object";
                public static string viewFactor = "!- View Factor to Ground";
                public static string shadeControl = "!- Shading Control Name";
                public static string frameAndDivider = "!- Frame and Divider Name";
                public static string multiplier = "!- Multiplier";
                public static string numberVertices = "!- Number of Vertices";
            }

            public class SizingZoneEndings
            {
                public static string outdoorAirMethod = "!- Outdoor Air Method";
                public static string outdoorAirFlowPerson = "!- Outdoor Air Flow per Person";
                public static string outdoorAirFlowPerArea = "!- Outdoor Air Flow per Zone Floor Area";
                public static string outdoorAirFlowPerZone = "!- Outdoor Air Flow per Zone";
            }

            public class LowTempRadiantSurfaceGroupEndings
            {
                public static string surfaceNumName = "!- Surface [0-9]+ Name";
                public static string surfaceFlowFracNum = "!- Flow Fraction for Surface [0-9]+";
            }
        }

        public class EPlusRegexString
        {
            //the start and end of objects...the regexes needed to grab them
            public static string startSurface = "BuildingSurface:Detailed,";
            public static string startFenestration = "FenestrationSurface:Detailed,";
            public static string ADstartFenestration = "Window,";
            public static string ADwindowName = @"(?'1'.*)(?'Name'!-Opening Name)";
            public static string ADwindowconst = @"(?'1'.*)(?'Construction'!-Class and Construction Name)";
            public static string ADwindowparent = @"(?'1'.*)(?'Parent'!-Name of Parent Surface)";
            public static string ADwindowshade = @"(?'1'.*)(?'Shade'!-Shading Control)";
            public static string ADwindowframe = @"(?'1'.*)(?'Frame'!-Frame)";
            public static string ADwindowmultiplier = @"(?'1'.*)(?'Multiplier'!-Multiplier)";
            public static string ADwindowX = @"(?'1'.*)(?'X'!-x coord.)";
            public static string ADwindowZ = @"(?'1'.*)(?'Z'!-z coord.)";
            public static string ADwindowlength = @"(?'1'.*)(?'Length'!-Length)";
            public static string ADwindowheight = @"(?'1'.*)(?'Height'!-Height)";
            public static string startWindowFrameDesc = "WindowProperty:FrameAndDivider,";
            public static string startDetailedBuildingShade = "Shading:Building:Detailed,";
            public static string startDetailedSiteShade = "Shading:Site:Detailed";
            public static string startDesignSpecOA = "DesignSpecification:OutdoorAir,";
            public static string startRadiantSurfaceGroup = "ZoneHVAC:LowTemperatureRadiant:SurfaceGroup";
            public static string commentStart = "(!-).*";
            //super generic
            public static string Name = @"(?'1'.*)(?'Name'!- Name)";
            public static string Type = @"(?'1'.*)(?'Type'!- type)";
            public static string semicolon = @"(\S*)(;)(.*)";
            public static string constructionName = @"(?'1'.*)(?'construction'!- Construction Name)";
            public static string typicalVertex = @"(?'1'.*)(?'vertices'!- X,Y,Z)";

            //zones
            public static string ADzoneStart = "ZONE,";
            public static string ADzoneName = @"(?'1'.*)(?'zoneName'!Zone name)";
            public static string ADzoneMultiplier = @"(?'1'.*)(?'zoneMult'! Zone Multiplier)";
            public static string ADzoneVol = @"(?'1'.*)(?'zoneVol'! Zone Volume {m\*\*3})";
            public static string ADzoneArea = @"(?'1'.*)(?'zoneArea'! Floor area {m2})";

            //detailed surfaces
            public static string surfaceType = @"(?'1'.*)(?'surfaceType'!- Surface Type)";
            public static string outsideBoundaryName = @"(?'1'.*)(?'outsideBoundaryName'!- Outside Boundary Condition)";

            //detailed fenstration objects
            //start fenestration goes here
            public static string fenestrationName = @"(?'1'.*)(?'fenestrationName'"+TagEndings.DetFenEndings.Name+")";
            public static string fenestrationType = @"(?'1'.*)(?'fenType'"+TagEndings.DetFenEndings.Type+")";
            public static string fenConstructionName = @"(?'1'.*)(?'construction'"+TagEndings.DetFenEndings.ConstName+")";
            public static string parentSurfaceName = @"(?'1'.*)(?'parentSurface'"+TagEndings.DetFenEndings.BldSurfName+")";
            public static string outsideBoundaryCondition = @"(?'1'.*)(?'outsideBoundaryCondition'"+TagEndings.DetFenEndings.OutsideBound+")";
            public static string viewFactor2Ground = @"(?'1'.*)(?'viewFactor'"+TagEndings.DetFenEndings.viewFactor+")";
            public static string shadeControlName = @"(?'1'.*)(?'shadeControl'"+TagEndings.DetFenEndings.shadeControl+")";
            public static string frameAndDividerName = @"(?'1'.*)(?'frameAndDivider'"+TagEndings.DetFenEndings.frameAndDivider+")";
            public static string fenestrationMultiplier = @"(?'1'.*)(?'multiplier'"+TagEndings.DetFenEndings.multiplier+")";
            public static string numberofFenestrationVertices = @"(?'1'.*)(?'vertices'"+TagEndings.DetFenEndings.numberVertices+")";
            //typical vertex goes here

            //for zones
            public static string startZoneList = "ZoneList,";
            public static string startZoneGroup = "ZoneGroup,";
            public static string zoneListName = @"(?'1'.*)(?'zoneListName'!- Zone List Name)";
            public static string zoneListMultiplier = @"(?'1'.*)(?'multiplier'!- Zone List Multiplier)";
            public static string zoneSizing = "Sizing:Zone,";
            public static string zoneName = @"(?'1'.*)(?'zoneName'!- Zone or ZoneList Name)";

            //for Sizing:Zone, etc
            public static string outdoorAirMethod = @"(?'1'.*)(?'OAMethod'"+TagEndings.SizingZoneEndings.outdoorAirMethod+")";
            public static string outdoorAirFlowPerson = @"(?'1'.*)(?'OAFlowPerPerson'"+TagEndings.SizingZoneEndings.outdoorAirFlowPerson+")";
            public static string outdoorAirFlowPerArea = @"(?'1'.*)(?'OAMethod'"+TagEndings.SizingZoneEndings.outdoorAirFlowPerArea+")";
            public static string outdoorAirFlowPerZone = @"(?'1'.*)(?'OAMethod'"+TagEndings.SizingZoneEndings.outdoorAirFlowPerZone+")";

            //for ZoneHVAC:LowTemperatureRadiant:SurfaceGroup
            public static string surfaceNumName = @"(?'1'.*)(?'OAMethod'" + TagEndings.LowTempRadiantSurfaceGroupEndings.surfaceNumName + ")";
            public static string surfaceFlowFracNum = @"(?'1'.*)(?'OAMethod'" + TagEndings.LowTempRadiantSurfaceGroupEndings.surfaceFlowFracNum + ")";
            //for schedules
            public static string startDaySched = "Schedule:Day:Interval,";
            public static string startWeekSched = "Schedule:Week:Daily,";
            public static string startYearSched = "Schedule:Year,";
            public static string daySchedType = @"(?'1'.*)(?'type'!- Schedule Type Limits Name)";
            public static string interpolTimeStep = @"(?'1'.*)(?'Interp'!- Interpolate to Timestep)";
            public static string sundaySched = @"(?'1'.*)(?'day'Sunday Schedule:Day Name)";
            public static string mondaySched = @"(?'1'.*)(?'day'Monday Schedule:Day Name)";
            public static string tuesdaySched = @"(?'1'.*)(?'day'Tuesday Schedule:Day Name)";
            public static string wednesdaySched = @"(?'1'.*)(?'day'Wednesday Schedule:Day Name)";
            public static string thursdaySched = @"(?'1'.*)(?'day'Thursday Schedule:Day Name)";
            public static string fridaySched = @"(?'1'.*)(?'day'Friday Schedule:Day Name)";
            public static string saturdaySched = @"(?'1'.*)(?'day'Saturday Schedule:Day Name)";
            public static string holidaySched = @"(?'1'.*)(?'day'Holiday Schedule:Day Name)";
            public static string cddSched = @"(?'1'.*)(?'day'CoolingDesignDay Schedule:Day Name)";
            public static string hddSched = @"(?'1'.*)(?'day'HeatingDesignDay Schedule:Day Name)";
            public static string custom1Sched = @"(?'1'.*)(?'day'CustomDay1 Schedule:Day Name)";
            public static string custom2Sched = @"(?'1'.*)(?'day'CustomDay2 Schedule:Day Name)";
            public static string yearName = @"(?'1'.*)(?'Name'!-Name)";
            public static string yearType = @"(?'1'.*)(?'type'!-Type)";
            public static string yearWkSch = @"(?'1'.*)(?'wksch'!-Week Schedule)"; 
            public static string startMonth = @"(?'1'.*)(?'day'!-Start Month )(?'num'\d)";
            public static string startDay = @"(?'1'.*)(?'day'!-Start Day )(?'num'\d)";
            public static string endMonth = @"(?'1'.*)(?'day'!-End Month )(?'num'\d)";
            public static string endDay = @"(?'1'.*)(?'day'!-End Day )(?'num'\d)";
            public static string dayschedvals = @"(?'1'.*)(?'comma1',)(?'2'.*)(?'comma2',)";
            public static string dayschedvalslast = @"(?'1'.*)(?'comma1',)(?'2'.*)(?'semi';)";
            public static string scheduleCompact = "Schedule:Compact,";
            public static string compactSchType = @"(?'1'.*)(?'schedule'!Schedule Type)";
            public static string compactThrough = @"(?'1'.*)(?'through'Through:)(?'rest'.*)";
            public static string compactFor = @"(?'1'.*)(?'for'For:)(?'rest'.*)";
            public static string compactUntil = @"(?'1'.*)(?'until'Until:)(?'rest'.*)";

            //for construction definitions
            public static string ADconstructionDef = @"CONSTRUCTION,";
            public static string ADconstructionName = @"(?'name'.*)(?'comma1',)";
            public static string ADconstructionLayer = @"! Layer";
            public static string ADwindowGlazing = @"WindowMaterial:Glazing,";
            public static string ADName = @"(?'1'.*)(?'name'! Name)";
            public static string ADwindowConstruction = @"(?'1'.*)(window)(?'1'.*)";
            public static string ADopticaldata = @"(?'1'.*)(?'optical'! Optical Data Type)";
            public static string ADglazingthickness = @"(?'1'.*)(?'thickness'! Thickness)";
            public static string ADwindsolartrans = @"(?'1'.*)(?'solartrans'! Solar Transmittance)";
            public static string ADwindsolarrefback = @"(?'1'.*)(?'solarrefback'! Solar Reflectance Back)";
            public static string ADwindsolarreffront = @"(?'1'.*)(?'solarreffront'! Solar Reflectance Front)";
            public static string ADwindvisibletrans = @"(?'1'.*)(?'visbletrans'! Visible Transmittance)";
            public static string ADwindvisiblerefback = @"(?'1'.*)(?'visiblerefback'! Visible Reflectance Back)";
            public static string ADwindvisiblereffront = @"(?'1'.*)(?'visiblereffront'! Visible Reflectance Front)";
            public static string ADwindIRtrans = @"(?'1'.*)(?'IRtrans'! IR Transmittance)";
            public static string ADwindIRemback = @"(?'1'.*)(?'IRrefback'! IR Emittance Front)";
            public static string ADwindIRemfront = @"(?'1'.*)(?'IRreffront'! IR Emittance Back)";
            public static string ADwindconductivity = @"(?'1'.*)(?'conductivity'! Conductivity)";
            public static string ADwindowGas = @"WindowMaterial:Gas";
            public static string ADgasType = @"(?'1'.*)(?'gastype'! Gas Type)";

            //material definitions
            public static string ADmaterial = @"(?'1'\s*)(?'material'MATERIAL,)(?'2'.*)";
            public static string ADmatThermalResistance = @"(?'1'.*)(?'resistance'! Resistance {m2-K/w})";
            public static string ADmaterialNoMass = @"(?'1'\s*)(?'material'MATERIAL:NoMass,)(?'2'.*)";
            public static string ADmaterialAirGap = @"(?'1'\s*)(?'material'MATERIAL:AirGap,)(?'2'.*)";
            public static string ADmaterialroughness = @"(?'1'.*)(?'roughness'!-Roughness)";
            public static string ADmaterialthickness = @"(?'1'.*)(?'thickness'! Thickness \(m\))";
            public static string ADmaterialconductivity = @"(?'1'.*)(?'conductivity'! Conductivity {W/m-K})";
            public static string ADmaterialdensity = @"(?'1'.*)(?'density'! Density \(kg/m3\))";
            public static string ADspecificheat = @"(?'1'.*)(?'spht'! Specific Heat \(J/kgK\))";
            public static string ADthermabs = @"(?'1'.*)(?'thermabs'! Thermal Absorptance)";
            public static string ADsolabs = @"(?'1'.*)(?'solabs'! Solar Absorptance)";
            public static string ADvisabs = @"(?'1'.*)(?'visabs'! Visible Absorptance)";

            //zone internal gain definitions
            public static string people = "PEOPLE,";
            public static string ADpeopleGroupName = @"(?'1'.*)(?'pgn'! People Group Name)";
            public static string ADpeopleZoneName = @"(?'1'.*)(?'name'!Zone name)";
            public static string ADnumPeopleSch = @"(?'1'.*)(?'pnSched'!Number of People Schedule)";
            public static string ADnumPeople = @"(?'1'.*)(?'numPeop'!Number of People)";
            public static string ADpeoplePerArea = @"(?'1'.*)(?'peoplePerArea'!- People per Zone Floor Area {person/m2})";
            public static string ADareaPerPerson = @"(?'1'.*)(?'areaPerPerson'!- Zone Floor Area per Person {m2/person})";
            public static string ADpeoplefracRadiant = @"(?'1'.*)(?'fracRadiant'!Fraction Radiant)";
            public static string ADsensibleFrac = @"(?'1'.*)(?'fracSensible'! Sensible fraction)";
            public static string ADactivityLevelSched = @"(?'1'.*)(?'activityLevelSched'! Activity Level Schedule)";
            public static string lights = "LIGHTS,";
            public static string ADintGainName = @"(?'1'.*)(?'name'!-Name)";
            public static string ADintGainZoneName = @"(?'1'.*)(?'zonename'!-Zone name)";
            public static string ADSchedule = @"(?'1'.*)(?'schedule'!-Schedule)";
            public static string ADPower = @"(?'1'.*)(?'lightsPower'!-Design Level)";
            public static string ADPowerPerArea = @"(?'1'.*)(?'lightsPowerPerArea'!-Watts per Zone Floor Area {W/m2})";
            public static string ADAreaPerPerson = @"(?'1'.*)(?'lightsAreaPerPerson'!-Watts per Person {W/person})";
            public static string ADfractionRadiant = @"(?'1'.*)(?'fractionRadiant'!-Fraction Radiant)";
            public static string ADfractionVisible = @"(?'1'.*)(?'fractionVisible'!-Fraction Visible)";
            public static string equipment = "ElectricEquipment,";
            public static string ADfractionLatent = @"(?'1'.*)(?'fractionLatent'!-Fraction Latent)";



        }
        #endregion
        #region
        public class Surface
        {
            public string name;
            public SurfaceTypes surfaceType;
            public string constructionName;
            public OutsideBoundary outsideBoundary;
            public enum SurfaceTypes
            {
                Wall,
                Floor,
                Ceiling,
                Roof,
                Blank
            }
            public enum OutsideBoundary
            {
                Surface,
                Ground,
                Outdoors,
                Zone,
                OtherSideCoefficients,
                OtherSideConditionsModel,
                Blank

            }
            public string zoneName;
            public string outsideBoundaryCondition;
            public string sunExposureVar;
            public string windExposureVar;
            public double viewFactor;
            public int numVertices;
            public List<Vector.CartCoord> SurfaceCoords;
            public double tilt;
            public double azimuth;

            public void Clear()
            {
                name = "";
                surfaceType = SurfaceTypes.Blank;
                constructionName = "";
                outsideBoundary = OutsideBoundary.Blank;
                zoneName = "";
                sunExposureVar = "";
                windExposureVar = "";
                numVertices = 0;
                SurfaceCoords.Clear();

            }

        }


        public class OpeningDefinitions
        {
            public string nameId;
            public string openingType;
            public string parentSurfaceNameId;
            public double parentAzimuth;
            public double parentTilt;
            public string outsideBoundaryConditionObj;
            public double viewFactortoGround;
            public string shadeControlSch;
            public List<Vector.CartCoord> coordinateList;
            public double Azimuth;
            public double Tilt;
            public Vector.CartVect rHRVector;
            public string constructionName;

            public string frameAndDividerName;
            public int multiplier;
            public int numVertices;
            public double area;
        }

        public class DayIntervalSchedule
        {
            public string Name { get; set; }
            public string type { get; set; }
            public string Interpolate { get; set; }
            public List<List<decimal>> timevals { get; set; }

            public DayIntervalSchedule()
            {
                timevals = new List<List<decimal>>();
            }

        }

        public class WeekIntervalSchedule
        {
            public string Name { get; set; }
            public Dictionary<string, string> dayScheds { get; set; }

            public WeekIntervalSchedule()
            {
                dayScheds = new Dictionary<string, string>();
                dayScheds["Sunday"] = null;
                dayScheds["Monday"] = null;
                dayScheds["Tuesday"] = null;
                dayScheds["Wednesday"] = null;
                dayScheds["Thursday"] = null;
                dayScheds["Friday"] = null;
                dayScheds["Saturday"] = null;
                dayScheds["Holiday"] = null;
                dayScheds["cdd"] = null;
                dayScheds["hdd"] = null;
                dayScheds["Custom1"] = null;
                dayScheds["Custom2"] = null;

            }
        }

        public class Lighting
        {
            public enum DesignLevelCalc
            {
                LightingLevel
            }

            public enum LightEndUseKey
            {
                GeneralLights
            }

            public string name { get; set; }
            public string zoneName { get; set; }
            public decimal lightPower { get; set; }
            public decimal lightPowerPerArea { get; set; }
            public decimal lightPowerPerPerson { get; set; }
            public decimal lightFractionRadiant { get; set; }
            public decimal lightFractionVisible { get; set; }
            public decimal returnAirFraction { get; set; }
            public string lightSch { get; set; }
            public LightEndUseKey endUseKey { get; set; }
            public DesignLevelCalc levelCalc { get; set; }

            public Lighting()
            {
                endUseKey = LightEndUseKey.GeneralLights;
                levelCalc = DesignLevelCalc.LightingLevel;
            }

        }

        public class Equipment
        {
            public string name { get; set; }
            public string zoneName { get; set; }
            public decimal equipPower { get; set; }
            public decimal equipPowerPerArea { get; set; }
            public decimal equipPowerPerPerson { get; set; }
            public decimal fractionLatent { get; set; }
            public decimal fractionRadiant { get; set; }
            public decimal fractionLost { get; set; }
            public string equipSch { get; set; }

            public Equipment()
            {

            }
        }

        public class People
        {
            public enum PeopleNumberCalc
            {
                People
            }

            public string name { get; set; }
            public string zoneName { get; set; }
            public decimal numPeople { get; set; }
            public decimal peoplePerArea { get; set; }
            public decimal areaPerPerson { get; set; }
            public string peopleNumSch { get; set; }
            public string peopleHGSch { get; set; }
            public decimal fractionRadiant { get; set; }
            public decimal fractionSensible { get; set; }
            public PeopleNumberCalc numberCalc { get; set; }

            public People()
            {
                numberCalc = PeopleNumberCalc.People;
            }

        }

        public class Zone
        {
            public string name { get; set; }
            public decimal area { get; set; }
            public decimal volume { get; set; }
            public int multiplier { get; set; }
            public List<ModelingUtilities.BuildingObjects.Surface> spaceSurfaces { get; set; }
            public List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> msspaceSurfaces { get; set; }
            public People peopleDef { get; set; }
            public Equipment equipDef { get; set; }
            public Lighting lightDef { get; set; }

            public Zone(string aname)
            {
                name = aname;
                spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                msspaceSurfaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
            }

        }

        public class YearWeeks
        {
            public string wkschname { get; set; }
            public int startmonth { get; set; }
            public int startday { get; set; }
            public int endmonth { get; set; }
            public int endday { get; set; }

            public YearWeeks(string wknm)
            {
                wkschname = wknm;
                startmonth = 1;
                startday = 1;
                endmonth = 12;
                endday = 31;
            }
        }

        public class YearSchedule
        {
            public string Name { get; set; }
            public string type { get; set; }
            public List<YearWeeks> listofwks { get; set; }

            public YearSchedule()
            {
                listofwks = new List<YearWeeks>();
            }
        }

        public class EPSchedules
        {
            public List<DayIntervalSchedule> epDaySch { get; set; }
            public List<WeekIntervalSchedule> epWkSch { get; set; }
            public List<YearSchedule> epYrSch { get; set; }
            public EPSchedules()
            {
                epDaySch = new List<DayIntervalSchedule>();
                epWkSch = new List<WeekIntervalSchedule>();
                epYrSch = new List<YearSchedule>();
            }
        }
    }
        #endregion
    public class EPlusFunctions
    {
        static StringBuilder makeWindowLogFile = new StringBuilder();
        static string makeWindowLogFileLocation = @"C:\Temp\makeWindowLogFile.txt";
        //Basics
        static public double FindTilt(Vector.MemorySafe_CartVect normalVector)
        {
            double calculatedTilt = -999;
            //may need to also take into account other factors that, at this stage, seem to not be important
            //building Direction of Relative North
            //zone Direction of Relative North
            //GlobalGeometryRules coordinate system
            //I may need to know this in the future then rotate the axis vectors I am making below

            //x-axis [1 0 0] points east, y-axis [0 1 0] points north, z-axis[0 0 1] points up to the sky
            //alignment with y axis means north pointing, alignment with z-axis means it is pointing up to the sky (like a flat roof)
            double nX = 0;
            double nY = 1;
            double nZ = 0;
            Vector.MemorySafe_CartVect northVector = new Vector.MemorySafe_CartVect(nX,nY,nZ);
           
            double uX = 0;
            double uY = 0;
            double uZ = 1;
            Vector.MemorySafe_CartVect upVector = new Vector.MemorySafe_CartVect(uX,uY,uZ);

            //rotate the axis vectors for the future

            //ensure the vector passed into the function is a unit vector
            normalVector = Vector.UnitVector(normalVector);
            //get tilt:  cross product of normal vector and upVector
            //since parallel and anti parallel vectors will return the same cross product [0,0,0] I need to filter out the antiparalll case
            if (normalVector.X == upVector.X * -1 && normalVector.Y == upVector.Y * -1 && normalVector.Z == upVector.Z * -1)
            {
                calculatedTilt = 180;
                return calculatedTilt;
            }
            else
            {
                Vector.MemorySafe_CartVect tiltVector = Vector.CrossProduct(normalVector, upVector);
                double tiltVectorMagnitude = Vector.VectorMagnitude(tiltVector);
                calculatedTilt = Math.Round(Math.Asin(tiltVectorMagnitude) * 180 / Math.PI, 2);
                return calculatedTilt;
            }
        }

        static public double FindAzimuth(Vector.CartVect normalVector)
        {
            double calculatedAzimuth = -999;
            //may need to also take into account other factors that, at this stage, seem to not be important
            //building Direction of Relative North
            //zone Direction of Relative North
            //GlobalGeometryRules coordinate system
            //I may need to know this in the future then rotate the axis vectors I am making below

            //x-axis [1 0 0] points east, y-axis [0 1 0] points north, z-axis[0 0 1] points up to the sky
            //alignment with y axis means north pointing, alignment with z-axis means it is pointing up to the sky (like a flat roof)

            Vector.CartVect northVector = new Vector.CartVect();
            northVector.X = 0;
            northVector.Y = 1;
            northVector.Z = 0;


            Vector.CartVect southVector = new Vector.CartVect();
            southVector.X = 0;
            southVector.Y = -1;
            southVector.Z = 0;
            
            Vector.CartVect eastVector = new Vector.CartVect();
            eastVector.X = 1;
            eastVector.Y = 0;
            eastVector.Z = 0;

            Vector.CartVect westVector = new Vector.CartVect();
            westVector.X = -1;
            westVector.Y = 0;
            westVector.Z = 0;

            Vector.CartVect upVector = new Vector.CartVect();
            upVector.X = 0;
            upVector.Y = 0;
            upVector.Z = 1;


            //rotate the axis vectors for the future

            //ensure the vector passed into the function is a unit vector
            normalVector = Vector.UnitVector(normalVector);
            //get X-Y projection of the normal vector
            normalVector.Z = 0;
            //get azimuth:  cross product of normal vector x-y projection and northVector
            //1st quadrant
            if ((normalVector.X == 0 && normalVector.Y == 1) || (normalVector.X == 1 && normalVector.Y == 0) || (normalVector.X > 0 && normalVector.Y > 0))
            {
                //get azimuth:  cross product of normal vector x-y projection and northVector
                Vector.CartVect azVector = Vector.CrossProduct(normalVector, northVector);
                double azVectorMagnitude = Vector.VectorMagnitude(azVector);

                //modification for when the vector is in different quadrants
                calculatedAzimuth = Math.Round(Math.Asin(azVectorMagnitude) * 180 / Math.PI, 2);
                return calculatedAzimuth;
            }
            //second quadrant
            else if (normalVector.X < 0 && normalVector.Y > 0)
            {
                Vector.CartVect azVector = Vector.CrossProduct(normalVector, westVector);
                double azVectorMagnitude = Vector.VectorMagnitude(azVector);

                //modification for when the vector is in different quadrants
                calculatedAzimuth = Math.Round(Math.Asin(azVectorMagnitude) * 180 / Math.PI, 2) + 270;
                return calculatedAzimuth;
            }
            //quadrant 3
            else if ((normalVector.X < 0 && normalVector.Y < 0) || (normalVector.X == -1 && normalVector.Y == 0))
            {
                Vector.CartVect azVector = Vector.CrossProduct(normalVector, southVector);
                double azVectorMagnitude = Vector.VectorMagnitude(azVector);

                //modification for when the vector is in different quadrants
                calculatedAzimuth = Math.Round(Math.Asin(azVectorMagnitude) * 180 / Math.PI, 2) + 180;
                return calculatedAzimuth;
            }
            //quadrant 4
            else if ((normalVector.X > 0 && normalVector.Y < 0) || (normalVector.X == 0 && normalVector.Y == -1))
            {
                Vector.CartVect azVector = Vector.CrossProduct(normalVector, eastVector);
                double azVectorMagnitude = Vector.VectorMagnitude(azVector);

                //modification for when the vector is in different quadrants
                calculatedAzimuth = Math.Round(Math.Asin(azVectorMagnitude) * 180 / Math.PI, 2) + 90;
                return calculatedAzimuth;
            }
            //this will happen to vectors that point straight down or straight up because we are only interested in the X-Y projection and set the Z to zero anyways
            else if (normalVector.X == 0 && normalVector.Y == 0 && normalVector.Z == 0)
            {
                calculatedAzimuth = 0;
                return calculatedAzimuth;
            }

            //get the 

            return calculatedAzimuth;
        }

        public static ModelingUtilities.BuildingObjects.MemorySafe_Surface GetSurface(string parentSurfaceId, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            string name = "";
            int multiplier = 0;
            ModelingUtilities.BuildingObjects.SurfaceTypes SurfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            string constName = "";
            ModelingUtilities.BuildingObjects.OutsideBoundary ob = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
            string zoneName = "";
            string outsideBC = "";
            string sunExp = "";
            string windExp = "";
            double vF = 0;
            int numVert = 0;
            List<Vector.MemorySafe_CartCoord> sc = new List<Vector.MemorySafe_CartCoord>();
            double tilt = 0;
            double az = 0;
            ModelingUtilities.BuildingObjects.MemorySafe_Surface emptySurface = new ModelingUtilities.BuildingObjects.MemorySafe_Surface(name, multiplier,
                SurfaceType, constName, ob, zoneName, outsideBC, sunExp, windExp, vF, numVert, sc, tilt, az);

            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in projectSurfaces)
            {
                if (parentSurfaceId == surface.name)
                {
                    return surface;
                }
            }
            return emptySurface;
        }

        private static EPlusObjects.EPlusGlazingConst makeWindowConstruction(List<string> linestuff)
        {
            Regex scfound = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            EPlusObjects.EPlusGlazingConst retwindow = new EPlusObjects.EPlusGlazingConst();

            for (int line = 0; line < linestuff.Count; line++)
            {
                if (line == 0)
                {
                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    Regex purifyRegex = new Regex(purify);
                    Match pure = purifyRegex.Match(linestuff[line]);
                    if (pure.Success)
                    {
                        retwindow.name = pure.Groups[2].Value.Trim();
                        continue;
                    }
                }
                Match semicolon = scfound.Match(linestuff[line]);
                if (semicolon.Success)
                {
                    if (line % 2 == 1)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(linestuff[line]);
                        if (pure.Success)
                        {
                            retwindow.glazingLayers.Add(pure.Groups[2].Value.Trim());
                        }
                    }
                    if (line % 2 == 0)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(linestuff[line]);
                        if (pure.Success)
                        {
                            retwindow.gasLayers.Add(pure.Groups[2].Value.Trim());
                        }
                    }
                }
                else
                {
                    if (line % 2 == 1)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(linestuff[line]);
                        if (pure.Success)
                        {
                            retwindow.glazingLayers.Add(pure.Groups[2].Value.Trim());
                        }
                    }
                    if (line % 2 == 0)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(linestuff[line]);
                        if (pure.Success)
                        {
                            retwindow.gasLayers.Add(pure.Groups[2].Value.Trim());
                        }
                    }
                }
            }
            return retwindow;
        }

        private static EPlusObjects.EPlusOpaqueConst makeOpaqueConstruction(List<string> linestuff)
        {
            Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            EPlusObjects.EPlusOpaqueConst retconst = new EPlusObjects.EPlusOpaqueConst();
            for (int line = 0; line < linestuff.Count; line++)
            {
                if (line == 0)
                {
                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    Regex purifyRegex = new Regex(purify);
                    Match pure = purifyRegex.Match(linestuff[line]);
                    if (pure.Success)
                    {
                        retconst.name = pure.Groups[2].Value.Trim();
                    }
                }
                else
                {
                    Match semimatch = semicolon.Match(linestuff[line]);
                    string purify = "";
                    if (semimatch.Success)
                    {
                        purify = @"(?'ws'\s*)(?'goods'.*)(?'semic';)";
                    }
                    else
                    {
                        purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    }
                    Regex purifyRegex = new Regex(purify);
                    Match pure = purifyRegex.Match(linestuff[line]);
                    if (pure.Success)
                    {
                        retconst.layernames.Add(pure.Groups[2].Value.Trim());
                    }
                }
            }
            return retconst;
        }

        public static EPlusObjects.EPlusGlazing makeGlazingLayer(List<string> linestuff)
        {
            EPlusObjects.EPlusGlazing retglaz = new EPlusObjects.EPlusGlazing();
            Regex windowMaterialGlazing = new Regex(EPlusObjects.EPlusRegexString.ADwindowGlazing);
            Regex windowMaterialName = new Regex(EPlusObjects.EPlusRegexString.ADName);
            Regex opticalData = new Regex(EPlusObjects.EPlusRegexString.ADopticaldata);
            Regex thickness = new Regex(EPlusObjects.EPlusRegexString.ADglazingthickness);
            Regex visTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindvisibletrans);
            Regex visReflBack = new Regex(EPlusObjects.EPlusRegexString.ADwindvisiblerefback);
            Regex visReflFront = new Regex(EPlusObjects.EPlusRegexString.ADwindvisiblereffront);
            Regex solTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindsolartrans);
            Regex solReflBack = new Regex(EPlusObjects.EPlusRegexString.ADwindsolarrefback);
            Regex solReflFront = new Regex(EPlusObjects.EPlusRegexString.ADwindsolarreffront);
            Regex IRTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindIRtrans);
            Regex IREmBack = new Regex(EPlusObjects.EPlusRegexString.ADwindIRemback);
            Regex IREMFront = new Regex(EPlusObjects.EPlusRegexString.ADwindIRemfront);
            Regex cond = new Regex(EPlusObjects.EPlusRegexString.ADwindconductivity);
            Regex semicolonReg = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            
            for(int linecount = 0; linecount < linestuff.Count; linecount++)
            {
                if (linecount == 0)
                {
                    Match namematch = windowMaterialName.Match(linestuff[linecount]);
                    if (namematch.Success)
                    {
                        retglaz.name = Purify(namematch, ",");
                        continue;
                    }
                }
                else
                {
                    Match opticalMatch = opticalData.Match(linestuff[linecount]);
                    if (opticalMatch.Success)
                    {
                        retglaz.opticalDataType = Purify(opticalMatch, ",");
                        continue;
                    }

                    Match thckmatch = thickness.Match(linestuff[linecount]);
                    if (thckmatch.Success)
                    {
                        retglaz.thickness = Convert.ToDecimal(Purify(thckmatch, ","));
                        continue;
                    }
                    Match stmatch = solTrans.Match(linestuff[linecount]);
                    if (stmatch.Success)
                    {
                        retglaz.solarTransmittance = Convert.ToDecimal(Purify(stmatch, ","));
                        continue;
                    }
                    Match srfmatch = solReflFront.Match(linestuff[linecount]);
                    if (srfmatch.Success)
                    {
                        retglaz.solarReflectanceFront = Convert.ToDecimal(Purify(srfmatch,","));
                        continue;
                    }
                    Match srbmatch = solReflBack.Match(linestuff[linecount]);
                    if (srbmatch.Success)
                    {
                        retglaz.solarReflectanceBack = Convert.ToDecimal(Purify(srbmatch,","));
                        continue;
                    }
                    Match vltmatch = visTrans.Match(linestuff[linecount]);
                    if(vltmatch.Success)
                    {
                        retglaz.visibleTransmittance = Convert.ToDecimal(Purify(vltmatch,","));
                        continue;
                    }
                    Match vrfmatch = visReflFront.Match(linestuff[linecount]);
                    if (vrfmatch.Success)
                    {
                        retglaz.visibleReflectanceFront = Convert.ToDecimal(Purify(vrfmatch,","));
                        continue;
                    }
                    Match vrbmatch = visReflBack.Match(linestuff[linecount]);
                    if (vrbmatch.Success)
                    {
                        retglaz.visibleReflectanceBack = Convert.ToDecimal(Purify(vrbmatch, ","));
                        continue;
                    }
                    Match irtmatch = IRTrans.Match(linestuff[linecount]);
                    if (irtmatch.Success)
                    {
                        retglaz.IRTransmittance = Convert.ToDecimal(Purify(irtmatch,","));
                        continue;
                    }
                    Match irefmatch = IREMFront.Match(linestuff[linecount]);
                    if (irefmatch.Success)
                    {
                        retglaz.IREmittanceFront = Convert.ToDecimal(Purify(irefmatch,","));
                        continue;
                    }
                    Match irebmatch = IREmBack.Match(linestuff[linecount]);
                    if (irebmatch.Success)
                    {
                        retglaz.IREmittanceBack = Convert.ToDecimal(Purify(irebmatch,","));
                        continue;
                    }
                    Match condmatch = cond.Match(linestuff[linecount]);
                    if (condmatch.Success)
                    {
                        retglaz.conductivity = Convert.ToDecimal(Purify(condmatch, ";"));
                        continue;
                    }
                }
            }
            return retglaz;
        }

        public static EPlusObjects.EPlusGas makeGasLayer(List<string> linestuff)
        {
            EPlusObjects.EPlusGas retgas = new EPlusObjects.EPlusGas();
            Regex windowMaterialName = new Regex(EPlusObjects.EPlusRegexString.ADName);
            Regex thickness = new Regex(EPlusObjects.EPlusRegexString.ADglazingthickness);
            Regex gastype = new Regex(EPlusObjects.EPlusRegexString.ADgasType);

            for (int lnct = 0; lnct < linestuff.Count; lnct++)
            {
                Match nmatch = windowMaterialName.Match(linestuff[lnct]);
                if (nmatch.Success)
                {
                    retgas.name = Purify(nmatch, ",");
                    continue;
                }
                Match gasmatch = gastype.Match(linestuff[lnct]);
                if (gasmatch.Success)
                {
                    retgas.gastype = Purify(gasmatch, ",");
                }
                Match thickmatch = thickness.Match(linestuff[lnct]);
                if (thickmatch.Success)
                {
                    retgas.thickness = Convert.ToDecimal(Purify(thickmatch, ";"));
                    continue;
                }

            }
            return retgas;
        }

        public static EPlusObjects.EPlusMaterials makeMaterial(List<string> linestuff)
        {
            Regex namereg = new Regex(EPlusObjects.EPlusRegexString.ADName);
            Regex resreg = new Regex(EPlusObjects.EPlusRegexString.ADmatThermalResistance);
            Regex rufreg = new Regex(EPlusObjects.EPlusRegexString.ADmaterialroughness);
            Regex thckreg = new Regex(EPlusObjects.EPlusRegexString.ADmaterialthickness);
            Regex condreg = new Regex(EPlusObjects.EPlusRegexString.ADmaterialconductivity);
            Regex densreg = new Regex(EPlusObjects.EPlusRegexString.ADmaterialdensity);
            Regex spehtreg = new Regex(EPlusObjects.EPlusRegexString.ADspecificheat);
            Regex solabs = new Regex(EPlusObjects.EPlusRegexString.ADsolabs);
            Regex visabs = new Regex(EPlusObjects.EPlusRegexString.ADvisabs);
            Regex thermabs = new Regex(EPlusObjects.EPlusRegexString.ADthermabs);

            EPlusObjects.EPlusMaterials retmat = new EPlusObjects.EPlusMaterials();
            for (int lnct = 0; lnct < linestuff.Count; lnct++)
            {
                Match nmmtch = namereg.Match(linestuff[lnct]);
                if (nmmtch.Success)
                {
                    retmat.name = Purify(nmmtch, ",");
                    continue;
                }
                Match rufmtch = rufreg.Match(linestuff[lnct]);
                if (rufmtch.Success)
                {
                    retmat.roughness = Purify(rufmtch,",");
                    continue;
                }
                Match thkm = thckreg.Match(linestuff[lnct]);
                if (thkm.Success)
                {
                    retmat.thickness = Convert.ToDecimal(Purify(thkm,","));
                    continue;
                }
                Match condmatch = condreg.Match(linestuff[lnct]);
                if (condmatch.Success)
                {
                    retmat.conductivity = Convert.ToDecimal(Purify(condmatch,","));
                    continue;
                }
                Match pmatch = densreg.Match(linestuff[lnct]);
                if (pmatch.Success)
                {
                    retmat.density = Convert.ToDecimal(Purify(pmatch,","));
                    continue;
                }
                Match cpmatch = spehtreg.Match(linestuff[lnct]);
                if (cpmatch.Success)
                {
                    retmat.spht = Convert.ToDecimal(Purify(cpmatch,","));
                    continue;
                }
                Match thermabsm = thermabs.Match(linestuff[lnct]);
                if (thermabsm.Success)
                {
                    retmat.thermabs = Convert.ToDecimal(Purify(thermabsm, ","));
                    continue;
                }
                Match solabsm = solabs.Match(linestuff[lnct]);
                if (solabsm.Success)
                {
                    retmat.solarabs = Convert.ToDecimal(Purify(solabsm,","));
                    continue;
                }
                Match visabsm = visabs.Match(linestuff[lnct]);
                if (visabsm.Success)
                {
                    retmat.visabs = Convert.ToDecimal(Purify(visabsm,";"));
                    continue;
                }
                Match resmat = resreg.Match(linestuff[lnct]);
                if (resmat.Success)
                {
                    retmat.resistance = Convert.ToDecimal(Purify(resmat, ";"));
                }
            }
            return retmat;
        }

        //Convert file to Object Instances 
        public static EPlusObjects.EPlusConstructions getAllEPConstructions(string idfsource)
        {
            
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Temp\EPSchedGrabLog.txt";
            StringBuilder output = new StringBuilder();

            //start with all the regexes you will need
            Regex materialStart = new Regex(EPlusObjects.EPlusRegexString.ADmaterial);
            Regex materialStart2 = new Regex(EPlusObjects.EPlusRegexString.ADmaterialNoMass);
            Regex materialStart3 = new Regex(EPlusObjects.EPlusRegexString.ADmaterialAirGap);
            Regex constructionstart = new Regex(EPlusObjects.EPlusRegexString.ADconstructionDef);
            Regex constName = new Regex(EPlusObjects.EPlusRegexString.ADconstructionName);
            Regex constLayer = new Regex(EPlusObjects.EPlusRegexString.ADconstructionLayer);
            Regex windowConstFind = new Regex(EPlusObjects.EPlusRegexString.ADwindowConstruction);
            Regex windowMaterialGlazing = new Regex(EPlusObjects.EPlusRegexString.ADwindowGlazing);
            Regex windowMaterialGas = new Regex(EPlusObjects.EPlusRegexString.ADwindowGas);
            Regex opticalData = new Regex(EPlusObjects.EPlusRegexString.ADopticaldata);
            Regex thickness = new Regex(EPlusObjects.EPlusRegexString.ADglazingthickness);
            Regex visTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindvisibletrans);
            Regex visReflBack = new Regex(EPlusObjects.EPlusRegexString.ADwindvisiblerefback);
            Regex visReflFront = new Regex(EPlusObjects.EPlusRegexString.ADwindvisiblereffront);
            Regex solTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindsolartrans);
            Regex solReflBack = new Regex(EPlusObjects.EPlusRegexString.ADwindsolarrefback);
            Regex solReflFront = new Regex(EPlusObjects.EPlusRegexString.ADwindsolarreffront);
            Regex IRTrans = new Regex(EPlusObjects.EPlusRegexString.ADwindIRtrans);
            Regex IRReflBack = new Regex(EPlusObjects.EPlusRegexString.ADwindIRemback);
            Regex IRReflFront = new Regex(EPlusObjects.EPlusRegexString.ADwindIRemfront);
            Regex semicolonReg = new Regex(EPlusObjects.EPlusRegexString.semicolon);

            List<EPlusObjects.EPlusGlazingConst> epglazecon = new List<EPlusObjects.EPlusGlazingConst>();
            List<EPlusObjects.EPlusOpaqueConst> epopaque = new List<EPlusObjects.EPlusOpaqueConst>();
            EPlusObjects.EPlusConstructions epconst = new EPlusObjects.EPlusConstructions();

            Encoding encoding;
            List<string> linestuff = new List<string>();
            using (StreamReader reader = new StreamReader(idfsource))
            {
                string line;
                encoding = reader.CurrentEncoding;
                //construction parsing
                //name (if it has window in it, move to opaque solution) else glazing solution
                //opaque solution ...just take names through the semicolon (look for layers as a luxury)
                //window solution... call for each type and place it in the instance
                //initialize the values to collect and return
                bool foundMaterial = false;
                bool foundConstruction = false;
                bool foundGlazedConstruction = false;
                bool foundGlazingMaterial = false;
                bool foundGasMaterial = false;
                int linect = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    //find the material start
                    Match matstart = materialStart.Match(line);
                    Match matstart2 = materialStart2.Match(line);
                    Match matstart3 = materialStart3.Match(line);
                    if (matstart.Success || matstart2.Success || matstart3.Success)
                    {
                        foundMaterial = true;
                        continue;
                    }
                    //find the construction start
                    Match constrStart = constructionstart.Match(line);
                    if (constrStart.Success)
                    {
                        foundConstruction = true;
                        continue;
                    }
                    Match glazingMaterial = windowMaterialGlazing.Match(line);
                    if (glazingMaterial.Success)
                    {
                        foundGlazingMaterial = true;
                        continue;
                    }
                    Match gasMaterial = windowMaterialGas.Match(line);
                    if (gasMaterial.Success)
                    {
                        foundGasMaterial = true;
                        continue;
                    }
                    #region
                    if (foundMaterial)
                    {
                        linestuff.Add(line);
                        Match semic = semicolonReg.Match(line);
                        if (semic.Success)
                        {
                            epconst.epMaterials.Add(makeMaterial(linestuff));
                            linestuff.Clear();
                            foundMaterial = false;
                            continue;
                        }
                    }
                    if (foundConstruction)
                    {
                        //identify if the construction is a window or opaque
                        //Window?
                        if (!foundGlazedConstruction)
                        {
                            Match window = windowConstFind.Match(line);
                            if (window.Success)
                            {
                                foundGlazedConstruction = true;
                                linestuff.Add(line);
                                continue;
                            }
                        }
                        //the control flow for windows is a little unusual because of the way that we have to search.
                        if (foundGlazedConstruction == true)
                        {
                            linestuff.Add(line);
                            Match semicolon = semicolonReg.Match(line);
                            if (semicolon.Success)
                            {
                                epconst.epGlazing.Add(makeWindowConstruction(linestuff));
                                linestuff.Clear();
                                foundConstruction = false;
                                foundGlazedConstruction = false;
                                continue;
                            }
                        }
                        else
                        {
                            //it is safe (at least we think, to assume this is an opaque surface)
                            linestuff.Add(line);
                            Match semicolon = semicolonReg.Match(line);
                            if (semicolon.Success)
                            {
                                epconst.epOpaque.Add(makeOpaqueConstruction(linestuff));
                                linestuff.Clear();
                                foundConstruction = false;
                                continue;
                            }
                        }
                        
                    }
                    #endregion
                    if (foundGlazingMaterial)
                    {
                        linestuff.Add(line);
                        Match semicolon = semicolonReg.Match(line);
                        if (semicolon.Success)
                        {
                            epconst.epGlazingLayers.Add(makeGlazingLayer(linestuff));
                            linestuff.Clear();
                            foundGlazingMaterial = false;
                            continue;
                        }
                    }
                    if (foundGasMaterial)
                    {
                        linestuff.Add(line);
                        Match semicolon = semicolonReg.Match(line);
                        if (semicolon.Success)
                        {
                            epconst.epGasLayers.Add(makeGasLayer(linestuff));
                            linestuff.Clear();
                            foundGasMaterial = false;
                            continue;
                        }
                    }
                    linect++;
                }
            }
            return epconst;
        }


        static public EPlusObjects.EPSchedules getEPSchedules(string idfsource)
        {
            EPlusObjects.EPSchedules retscheds = new EPlusObjects.EPSchedules();
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Temp\EPSchedGrabLog.txt";
            StringBuilder output = new StringBuilder();

            //start with all the regexes you will need
            //day sched
            Regex dayschedstart = new Regex(EPlusObjects.EPlusRegexString.startDaySched);

            Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);

            Regex weekschstart = new Regex(EPlusObjects.EPlusRegexString.startWeekSched);
            Regex weekCustom2 = new Regex(EPlusObjects.EPlusRegexString.custom2Sched);

            Regex yearstart = new Regex(EPlusObjects.EPlusRegexString.startYearSched);
            Regex yearname = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex yearType = new Regex(EPlusObjects.EPlusRegexString.yearType);
            Regex yearWkSch = new Regex(EPlusObjects.EPlusRegexString.yearWkSch);
            Regex yearstartmonth = new Regex(EPlusObjects.EPlusRegexString.startMonth);
            Regex yearstartday = new Regex(EPlusObjects.EPlusRegexString.startDay);
            Regex yearendmonth = new Regex(EPlusObjects.EPlusRegexString.endMonth);
            Regex yearendday = new Regex(EPlusObjects.EPlusRegexString.endDay);

            Regex dayschedvalsreglast = new Regex(EPlusObjects.EPlusRegexString.dayschedvalslast);

            Encoding encoding;
            List<string> linestuff = new List<string>();
            using (StreamReader reader = new StreamReader(idfsource))
            {
                string line;
                encoding = reader.CurrentEncoding;
                bool dm = false;
                bool wm = false;
                bool ym = false;
                while ((line = reader.ReadLine()) != null)
                {
                    MatchCollection daymatch = dayschedstart.Matches(line);
                    if (daymatch.Count > 0)
                    {
                        dm = true;
                        continue;
                    }
                    Match wkmatch = weekschstart.Match(line);
                    if (wkmatch.Success)
                    {
                        wm = true;
                        continue;
                    }
                    Match yrmatch = yearstart.Match(line);
                    if (yrmatch.Success)
                    {
                        ym = true;
                        continue;
                    }
                    if (dm)
                    {
                        Match dyvalsend = dayschedvalsreglast.Match(line);
                        if (dyvalsend.Success)
                        {
                            linestuff.Add(line);
                            retscheds.epDaySch.Add(makeDayIntervalSchedule(linestuff));
                            linestuff.Clear();
                            dm = false;
                            continue;
                        }
                        else
                        {
                            linestuff.Add(line);
                            continue;
                        }
                    }
                    if (wm)
                    {
                        Match custm2 = weekCustom2.Match(line);
                        if (custm2.Success)
                        {
                            linestuff.Add(line);
                            retscheds.epWkSch.Add(makeWeekIntervalSchedule(linestuff));
                            linestuff.Clear();
                            wm = false;
                            continue;
                        }
                        else
                        {
                            linestuff.Add(line);
                            continue;
                        }
                    }
                    if (ym)
                    {
                        Match yrendday = yearendday.Match(line);
                        if (yrendday.Success)
                        {
                            var ret = string.Empty;
                            Match sc = semicolon.Match(line);
                            if (sc.Success)
                            {
                                linestuff.Add(line);
                                retscheds.epYrSch.Add(makeYearSchedule(linestuff));
                                linestuff.Clear();
                                ym = false;
                                continue;
                            }
                            else
                            {
                                linestuff.Add(line);
                                continue;
                            }
                        }
                        else
                        {
                            linestuff.Add(line);
                            continue;
                        }

                        
                    }
                }
            }
            return retscheds;
        }

        static public EPlusObjects.YearSchedule makeYearSchedule(List<string> stuff)
        {
            Regex yearname = new Regex(EPlusObjects.EPlusRegexString.yearName);
            Regex yearType = new Regex(EPlusObjects.EPlusRegexString.yearType);
            Regex yearWkSch = new Regex(EPlusObjects.EPlusRegexString.yearWkSch);
            Regex yearstartmonth = new Regex(EPlusObjects.EPlusRegexString.startMonth);
            Regex yearstartday = new Regex(EPlusObjects.EPlusRegexString.startDay);
            Regex yearendmonth = new Regex(EPlusObjects.EPlusRegexString.endMonth);
            Regex yearendday = new Regex(EPlusObjects.EPlusRegexString.endDay);
            Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            EPlusObjects.YearSchedule yrsch = new EPlusObjects.YearSchedule();
            int intervalct = 0;
            List<string> yrWeekStrings = new List<string>();

            foreach (string line in stuff)
            {
                Match yrnam = yearname.Match(line);
                if (yrnam.Success)
                {
                    var ret = Purify(yrnam, ",");
                    if (ret != null) { yrsch.Name = ret; };
                    continue;
                }
                Match yrtype = yearType.Match(line);
                if (yrtype.Success)
                {
                    var ret = Purify(yrtype, ",");
                    if (ret != null) { yrsch.type = ret; }
                    continue;
                }
                Match wksched = yearWkSch.Match(line);
                if (wksched.Success)
                {
                    var ret = Purify(wksched, ",");
                    if (ret != null) { yrWeekStrings.Add(ret); }
                    continue;
                }
                Match yrstartmonth = yearstartmonth.Match(line);
                if (yrstartmonth.Success)
                {
                    var ret = Purify(yrstartmonth, ",");
                    if (ret != null) { yrWeekStrings.Add(ret); }
                    continue;
                }
                Match yrstartday = yearstartday.Match(line);
                if (yrstartday.Success)
                {
                    var ret = Purify(yrstartday, ",");
                    if (ret != null) { yrWeekStrings.Add(ret); }
                    continue;
                }
                Match yrendmonth = yearendmonth.Match(line);
                if (yrendmonth.Success)
                {
                    var ret = Purify(yrendmonth, ",");
                    if (ret != null) { yrWeekStrings.Add(ret); }
                    continue;
                }
                Match yrendday = yearendday.Match(line);
                if (yrendday.Success)
                {
                    var ret = string.Empty;
                    Match sc = semicolon.Match(line);
                    if (sc.Success)
                    {
                        ret = Purify(yrendday, ";");
                        if (intervalct == 0)
                        {
                            string wkname = yrWeekStrings[0];
                            EPlusObjects.YearWeeks yrWeeks = new EPlusObjects.YearWeeks(wkname);
                            yrsch.listofwks.Add(yrWeeks); //we know by default with only one interval, that this must be whole year
                        }
                    }
                    else
                    {
                        ret = Purify(yrendday, ",");
                        if (ret != null) { yrWeekStrings.Add(ret); }
                        yrsch.listofwks.Add(makeYearWeekObject(yrWeekStrings));
                    }
                }
            }
            return yrsch;
        }

        static public EPlusObjects.YearWeeks makeYearWeekObject(List<string> yrWeekStrings)
        {
            EPlusObjects.YearWeeks week = new EPlusObjects.YearWeeks(yrWeekStrings[0]);
            week.startmonth = Convert.ToInt16(yrWeekStrings[1]);
            week.startday = Convert.ToInt16(yrWeekStrings[2]);
            week.endmonth = Convert.ToInt16(yrWeekStrings[3]);
            week.endday = Convert.ToInt16(yrWeekStrings[4]);

            return week;
        }

        static public EPlusObjects.WeekIntervalSchedule makeWeekIntervalSchedule(List<string> stuff)
        {
            Regex weekschstart = new Regex(EPlusObjects.EPlusRegexString.startWeekSched);
            Regex weekName = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex weekMonday = new Regex(EPlusObjects.EPlusRegexString.mondaySched);
            Regex weekTuesday = new Regex(EPlusObjects.EPlusRegexString.tuesdaySched);
            Regex weekWednesday = new Regex(EPlusObjects.EPlusRegexString.wednesdaySched);
            Regex weekThursday = new Regex(EPlusObjects.EPlusRegexString.thursdaySched);
            Regex weekFriday = new Regex(EPlusObjects.EPlusRegexString.fridaySched);
            Regex weekSaturday = new Regex(EPlusObjects.EPlusRegexString.saturdaySched);
            Regex weekSunday = new Regex(EPlusObjects.EPlusRegexString.sundaySched);
            Regex weekHoliday = new Regex(EPlusObjects.EPlusRegexString.holidaySched);
            Regex weekCDD = new Regex(EPlusObjects.EPlusRegexString.cddSched);
            Regex weekHDD = new Regex(EPlusObjects.EPlusRegexString.hddSched);
            Regex weekCustom1 = new Regex(EPlusObjects.EPlusRegexString.custom1Sched);
            Regex weekCustom2 = new Regex(EPlusObjects.EPlusRegexString.custom2Sched);

            EPlusObjects.WeekIntervalSchedule wsch = new EPlusObjects.WeekIntervalSchedule();
            foreach (string line in stuff)
            {
                Match weekmatch = weekName.Match(line);
                if (weekmatch.Success)
                {
                    var ret = Purify(weekmatch, ",");
                    if (ret != null) { wsch.Name = ret; }
                    continue;
                }
                Match sundaym = weekSunday.Match(line);
                if (sundaym.Success)
                {
                    var ret = Purify(sundaym, ",");
                    if (ret != null) { wsch.dayScheds["Sunday"] = ret; }
                    continue;
                }
                Match mondaym = weekMonday.Match(line);
                if (mondaym.Success)
                {
                    var ret = Purify(mondaym, ",");
                    if (ret != null) { wsch.dayScheds["Monday"] = ret; }
                    continue;
                }
                Match tuesdaym = weekTuesday.Match(line);
                if (tuesdaym.Success)
                {
                    var ret = Purify(tuesdaym, ",");
                    if (ret != null) { wsch.dayScheds["Tuesday"] = ret; }
                    continue;
                }
                Match wednesdaym = weekWednesday.Match(line);
                if (wednesdaym.Success)
                {
                    var ret = Purify(wednesdaym, ",");
                    if (ret != null) { wsch.dayScheds["Wednesday"] = ret; }
                    continue;
                }
                Match thursdaym = weekThursday.Match(line);
                if (thursdaym.Success)
                {
                    var ret = Purify(thursdaym, ",");
                    if (ret != null) { wsch.dayScheds["Thursday"] = ret; }
                    continue;
                }
                Match fridaym = weekFriday.Match(line);
                if (fridaym.Success)
                {
                    var ret = Purify(fridaym, ",");
                    if (ret != null) { wsch.dayScheds["Friday"] = ret; }
                    continue;
                }
                Match saturdaym = weekSaturday.Match(line);
                if (saturdaym.Success)
                {
                    var ret = Purify(saturdaym, ",");
                    if (ret != null) { wsch.dayScheds["Saturday"] = ret; }
                    continue;
                }
                Match holidaym = weekHoliday.Match(line);
                if (holidaym.Success)
                {
                    var ret = Purify(holidaym, ",");
                    if (ret != null) { wsch.dayScheds["Holiday"] = ret; }
                    continue;
                }
                Match cddm = weekCDD.Match(line);
                if (cddm.Success)
                {
                    var ret = Purify(cddm, ",");
                    if (ret != null) { wsch.dayScheds["cdd"] = ret; }
                    continue;
                }
                Match hddm = weekHDD.Match(line);
                if (hddm.Success)
                {
                    var ret = Purify(hddm, ",");
                    if (ret != null) { wsch.dayScheds["hdd"] = ret; }
                    continue;
                }
                Match custm1 = weekCustom1.Match(line);
                if (custm1.Success)
                {
                    var ret = Purify(custm1, ",");
                    if (ret != null) { wsch.dayScheds["Custom1"] = ret; }
                    continue;
                }
                Match custm2 = weekCustom2.Match(line);
                if (custm2.Success)
                {
                    var ret = Purify(custm2, ";");
                    if (ret != null) { wsch.dayScheds["Custom2"] = ret; }
                    continue;
                }
            }
            return wsch;
        }
        static public EPlusObjects.DayIntervalSchedule makeDayIntervalSchedule(List<string> stuff)
        {
            Regex dayschedstart = new Regex(EPlusObjects.EPlusRegexString.startDaySched);
            Regex dayschedname = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex dayschedinterp = new Regex(EPlusObjects.EPlusRegexString.interpolTimeStep);
            Regex dayschedtype = new Regex(EPlusObjects.EPlusRegexString.daySchedType);
            Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            Regex dayschedvalsreg = new Regex(EPlusObjects.EPlusRegexString.dayschedvals);
            Regex dayschedvalsreglast = new Regex(EPlusObjects.EPlusRegexString.dayschedvalslast);

            EPlusObjects.DayIntervalSchedule dsint = new EPlusObjects.DayIntervalSchedule();
            foreach (string line in stuff)
            {
                Match name = dayschedname.Match(line);
                if (name.Success)
                {
                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    Regex purifyRegex = new Regex(purify);
                    Match pure = purifyRegex.Match(name.Groups["1"].Value);
                    if (pure.Success)
                    {
                        dsint.Name = pure.Groups["goods"].Value;
                        continue;
                    }
                }
                Match schedlimits = dayschedtype.Match(line);
                if (schedlimits.Success)
                {
                    var ret = Purify(schedlimits, ",");
                    if (ret != null) { dsint.type = ret; }
                    continue;
                }
                Match interpol = dayschedinterp.Match(line);
                if (interpol.Success)
                {
                    var ret = Purify(interpol, ",");
                    if (ret != null) { dsint.Interpolate = ret; }
                    continue;
                }
                Match dyvals = dayschedvalsreg.Match(line);
                if (dyvals.Success)
                {
                    List<decimal> temp = new List<decimal>();
                    string eptime = @"(?'hr'\d+)(?'colon':)(?'min'\d+)";
                    Regex eptimereg = new Regex(eptime);
                    Match eptimematch = eptimereg.Match(dyvals.Groups["1"].Value.Trim());
                    if (eptimematch.Success)
                    {
                        decimal hr = Convert.ToDecimal(eptimematch.Groups["hr"].Value);
                        decimal min = Convert.ToDecimal(eptimematch.Groups["min"].Value)/60;
                        //decimal time = Math.Round(hr + min,2);
                        decimal time = hr + min;
                        temp.Add(time);
                        temp.Add(Convert.ToDecimal(dyvals.Groups["2"].Value.Trim()));
                        dsint.timevals.Add(temp);
                    }
                    continue;
                }
                Match dyvalsend = dayschedvalsreglast.Match(line);
                if (dyvalsend.Success)
                {
                    List<decimal> temp = new List<decimal>();
                    string eptime = @"(?'hr'\d+)(?'colon':)(?'min'\d+)";
                    Regex eptimereg = new Regex(eptime);
                    Match eptimematch = eptimereg.Match(dyvalsend.Groups["1"].Value.Trim());
                    if (eptimematch.Success)
                    {
                        decimal hr = Convert.ToDecimal(eptimematch.Groups["hr"].Value);
                        decimal min = Convert.ToDecimal(eptimematch.Groups["min"].Value) / 60;
                        //decimal time = Math.Round(hr + min, 2);
                        decimal time = hr + min;
                        temp.Add(time);
                        temp.Add(Convert.ToDecimal(dyvalsend.Groups["2"].Value.Trim()));
                        dsint.timevals.Add(temp);
                    }
                    
                }
            }
            return dsint;
        }

        static public string Purify(Match line, string delimiter)
        {
            if (delimiter == ",")
            {
                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                Regex purifyRegex = new Regex(purify);
                Match pure = purifyRegex.Match(line.Groups["1"].Value);
                if (pure.Success)
                {
                    return pure.Groups["goods"].Value;
                }
            }
            else if (delimiter == ";")
            {
                string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                Regex purifyRegex = new Regex(purify);
                Match pure = purifyRegex.Match(line.Groups["1"].Value);
                if (pure.Success)
                {
                    return pure.Groups["goods"].Value;
                }
            }
            return null;
        }

        static public ModelingUtilities.BuildingObjects.Surface EPlusSurfacetoObject(string sourcefile)
        {
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Users\Chiensi\Documents\AAATerabuild\EnergyPlus\homework\surf2objlog.txt";
            StringBuilder output = new StringBuilder();
            //need to add a try/catch clause and start to work on try/catches when I get the chance

            //initialize the surface to be returned
            ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
            currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
            currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;

            StringBuilder logline = new StringBuilder();
            //Constructions - Opaque Detailed...

            #region
            //start with the Regexes needed to parse out the opaque constructions
            //Regex for beginning of a detailed surface element,
            string startSurface = "BuildingSurface:Detailed,";
            Regex surfaceYes = new Regex(startSurface);
            //Regex for surfaceName
            //string surfaceName = @"(?'ws1'\s*)(?'name'.*)(?'ws2'\s*)(?'surfaceName'!- Name)";
            string surfaceName = @"(?'1'.*)(?'surfaceName'!- Name)";
            //string surfaceName = @"(?'surfaceName'!- Name)";
            Regex surfaceNameRegex = new Regex(surfaceName);
            //Regex for surface Type
            string surfaceType = @"(?'1'.*)(?'surfaceType'!- Surface Type)";
            Regex surfaceTypeRegex = new Regex(surfaceType);
            //Regext for surfaceConstructionName
            string surfaceConstructionName = @"(?'1'.*)(?'construction'!- Construction Name)";
            Regex surfaceConstructionNameRegex = new Regex(surfaceConstructionName);
            //Regext for surfaceZoneName
            string surfaceZoneName = @"(?'1'.*)(?'zoneName'!- Zone Name)";
            Regex surfaceZoneNameRegex = new Regex(surfaceZoneName);
            //Regex for outsideBoundary
            string outsideBoundaryName = @"(?'1'.*)(?'outsideBoundaryName'!- Outside Boundary Condition)";
            Regex outsideBoundaryRegex = new Regex(outsideBoundaryName);
            //Regex for outsideBoundary Condition
            string outsideBoundaryCondition = @"(?'1'.*)(?'outsideBoundaryCondition'!- Outside Boundary Condition Object)";
            Regex outsideBoundaryConditionRegex = new Regex(outsideBoundaryCondition);
            //Regex for sunExposure
            string sunExposure = @"(?'1'.*)(?'sunExposure'!- Sun Exposure)";
            Regex sunExposureRegex = new Regex(sunExposure);
            //Regex for windExposure
            string windExposure = @"(?'1'.*)(?'windExposure'!- Wind Exposure)";
            Regex windExposureRegex = new Regex(windExposure);
            //Regex for ViewFactor
            string viewFactor = @"(?'1'.*)(?'viewFactor'!- View Factor to Ground)";
            Regex viewFactorRegex = new Regex(viewFactor);
            //Regex for numberofVertices
            string numberofVertices = @"(?'1'.*)(?'vertices'!- Number of Vertices)";
            Regex numberofVerticesRegex = new Regex(numberofVertices);
            //something else for the vertex
            //Regex for generic Vertex
            string typicalVertex = @"(?'1'.*)(?'Xvertex'!- X[0-9]*),\s*(?'Yvertex'Y[0-9]*),\s*(?'ZVertex'Z[0-9]*)\s*";
            Regex typicalVertexRegex = new Regex(typicalVertex);

            //a regex for finding a semicolon
            string semicolon = @"(\S*)(;)(.*)";
            Regex smicln = new Regex(semicolon);
            #endregion

            //make a list of spaces



            //special needed to allow the routine to run successfully
            //needed because the regex may return true in the wrong instance
            bool outsideBoundaryMatched = false;
            bool semicolonfound = false;
            Encoding encoding;

            using (StreamReader reader = new StreamReader(sourcefile))
            {
                string line;
                encoding = reader.CurrentEncoding;
                bool detailedsurface = false;
                bool vertexMatching = false;
                #region
                while ((line = reader.ReadLine()) != null)
                {
                    MatchCollection surfaceStart = surfaceYes.Matches(line);
                    if (surfaceStart.Count > 0)
                    {
                        detailedsurface = true;
                        continue;
                    }
                    //now that a surface element is established in the IDF, we can work through it to create surface objects
                    if (detailedsurface == true)
                    {
                        //Surface Name
                        //get the name in the file
                        if (!vertexMatching)
                        {
                            Match surfaceNameMatch = surfaceNameRegex.Match(line);
                            int matchstart, matchlength = -1;
                            if (surfaceNameMatch.Success)
                            {
                                matchstart = surfaceNameMatch.Index;
                                matchlength = surfaceNameMatch.Length;
                                //strip off the whitespace and comma
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(surfaceNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.name = pure.Groups["goods"].Value;
                                    continue;
                                }

                            }
                            //Get Surface Type
                            Match surfaceTypeMatch = surfaceTypeRegex.Match(line);
                            if (surfaceTypeMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(surfaceTypeMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    string type = pure.Groups["goods"].Value;
                                    type = type.ToLower();
                                    if (type == EPlusObjects.Surface.SurfaceTypes.Ceiling.ToString().ToLower())
                                    {
                                        currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Ceiling;
                                        continue;
                                    }
                                    else if (type == EPlusObjects.Surface.SurfaceTypes.Floor.ToString().ToLower())
                                    {
                                        currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Floor;
                                        continue;
                                    }
                                    else if (type == EPlusObjects.Surface.SurfaceTypes.Roof.ToString().ToLower())
                                    {
                                        currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Roof;
                                        continue;
                                    }
                                    else if (type == EPlusObjects.Surface.SurfaceTypes.Wall.ToString().ToLower())
                                    {
                                        currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Wall;
                                        continue;
                                    }

                                }
                            }
                            //Get Construction Type
                            Match constructionTypeMatch = surfaceConstructionNameRegex.Match(line);
                            if (constructionTypeMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(constructionTypeMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.constructionName = pure.Groups["goods"].Value;
                                    continue;
                                }

                            }
                            //GetZone Name
                            Match zoneNameMatch = surfaceZoneNameRegex.Match(line);
                            if (zoneNameMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(zoneNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.zoneName = pure.Groups["goods"].Value;
                                    continue;
                                }
                            }
                            //GetOutside Boundary Name

                            if (!outsideBoundaryMatched)
                            {
                                Match outsideBoundaryMatch = outsideBoundaryRegex.Match(line);
                                if (outsideBoundaryMatch.Success)
                                #region
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(outsideBoundaryMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {

                                        string type = pure.Groups["goods"].Value;
                                        type = type.ToLower();
                                        if (type == EPlusObjects.Surface.OutsideBoundary.Ground.ToString().ToLower())
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Ground;
                                            outsideBoundaryMatched = true;
                                        }
                                        else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideCoefficients.ToString().ToLower())
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideCoefficients;
                                            outsideBoundaryMatched = true;
                                        }
                                        else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideConditionsModel.ToString().ToLower())
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideConditionsModel;
                                            outsideBoundaryMatched = true;
                                        }
                                        else if (type == EPlusObjects.Surface.OutsideBoundary.Outdoors.ToString().ToLower())
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors;
                                            outsideBoundaryMatched = true;
                                        }
                                        else if (type == EPlusObjects.Surface.OutsideBoundary.Surface.ToString().ToLower())
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Surface;
                                            outsideBoundaryMatched = true;
                                        }
                                        else
                                        {
                                            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Zone;
                                            outsideBoundaryMatched = true;
                                        }
                                        continue;
                                    }
                                }
                            }
                                #endregion

                            //Get Outside Boundary Condition Object
                            Match outsideBoundaryConditionMatch = outsideBoundaryConditionRegex.Match(line);
                            if (outsideBoundaryConditionMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(outsideBoundaryConditionMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.outsideBoundaryCondition = pure.Groups["goods"].Value;
                                    continue;
                                }
                            }
                            //Get Sun Exposure
                            Match sunExposureMatch = sunExposureRegex.Match(line);
                            if (sunExposureMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(sunExposureMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.sunExposureVar = pure.Groups["goods"].Value;
                                    continue;
                                }
                            }
                            //Get Wind Exposure
                            Match windExposureMatch = windExposureRegex.Match(line);
                            if (windExposureMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(windExposureMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.windExposureVar = pure.Groups["goods"].Value;
                                    continue;
                                }
                            }
                            //View Factor
                            Match viewFactorMatch = viewFactorRegex.Match(line);
                            if (viewFactorMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(viewFactorMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.viewFactor = Convert.ToDouble(pure.Groups["goods"].Value);
                                    continue;
                                }
                            }
                            //Number of Vertices
                            Match numVerticesMatch = numberofVerticesRegex.Match(line);
                            if (numVerticesMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(numVerticesMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurface.numVertices = Convert.ToInt32(pure.Groups["goods"].Value);
                                    continue;
                                }
                            }
                        }
                        //Get Vertices
                        //loop through them until the end


                        Match vertexMatch = typicalVertexRegex.Match(line);
                        if (vertexMatch.Success)
                        {
                            vertexMatching = true;
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma')";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(vertexMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                //extract the X,Y, and Z coordinate from the purified string
                                //string coordinateString = @"(?'X'[-]\d+[\.]\d+|\d+),(?'Y'[-]\d+[\.]\d+|\d+),(?'Z'[-]\d+[\.]\d+|\d+)";
                                string coordinateString = @"(?'X'[-+]?([0-9]*\.[0-9]+|[0-9]+)),\s*(?'Y'[-+]?([0-9]*\.[0-9]+|[0-9]+)),\s*(?'Z'[-+]?([0-9]*\.[0-9]+|[0-9]+)\s*)";
                                Regex coordRegex = new Regex(coordinateString);
                                Match XYZMatch = coordRegex.Match(pure.Groups["goods"].Value);
                                if (XYZMatch.Success)
                                {
                                    Vector.CartCoord surfaceCoord = new Vector.CartCoord();
                                    surfaceCoord.X = Convert.ToDouble(XYZMatch.Groups["X"].Value);
                                    surfaceCoord.Y = Convert.ToDouble(XYZMatch.Groups["Y"].Value);
                                    surfaceCoord.Z = Convert.ToDouble(XYZMatch.Groups["Z"].Value);
                                    currentSurface.SurfaceCoords.Add(surfaceCoord);
                                }
                            }
                        }

                        //see if there is a semi-colon
                        Match smicolonMatch = smicln.Match(line);
                        if (smicolonMatch.Success)
                        {
                            semicolonfound = true;
                            vertexMatching = false;
                        }
                    }
                }
                #endregion
                //close the reader
                reader.Close();

                //get the RHR Normal Vector
                Vector.CartVect RHRNormalVector = Vector.GetRHR(currentSurface.SurfaceCoords);
                logline.AppendLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
                Console.WriteLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
                //get azimuth
                currentSurface.azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
                //get tilt
                Vector.MemorySafe_CartVect memRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
                currentSurface.tilt = EPlusFunctions.FindTilt(memRHR);
                //in any case, return the current surface
                logline.AppendLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());
                Console.WriteLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());

                using (StreamWriter writer = new StreamWriter(log, false))
                {
                    writer.Write(logline.ToString());
                }
            }
            
            return currentSurface;
        }

        static public ModelingUtilities.BuildingObjects.Surface EPlusSurfacetoObject(List<string> detailedSurfaceString)
        {
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Users\Chiensi\Documents\AAATerabuild\EnergyPlus\homework\surf2objlog.txt";
            StringBuilder output = new StringBuilder();
            //need to add a try/catch clause and start to work on try/catches when I get the chance

            //initialize the surface to be returned
            ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
            currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
            currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;

            StringBuilder logline = new StringBuilder();
            //Constructions - Opaque Detailed...

            #region
            //start with the Regexes needed to parse out the opaque constructions
            //Regex for beginning of a detailed surface element,
            string startSurface = "BuildingSurface:Detailed,";
            Regex surfaceYes = new Regex(startSurface);
            //Regex for surfaceName
            //string surfaceName = @"(?'ws1'\s*)(?'name'.*)(?'ws2'\s*)(?'surfaceName'!- Name)";
            string surfaceName = @"(?'1'.*)(?'surfaceName'!- Name)";
            //string surfaceName = @"(?'surfaceName'!- Name)";
            Regex surfaceNameRegex = new Regex(surfaceName);
            //Regex for surface Type
            string surfaceType = @"(?'1'.*)(?'surfaceType'!- Surface Type)";
            Regex surfaceTypeRegex = new Regex(surfaceType);
            //Regext for surfaceConstructionName
            string surfaceConstructionName = @"(?'1'.*)(?'construction'!- Construction Name)";
            Regex surfaceConstructionNameRegex = new Regex(surfaceConstructionName);
            //Regext for surfaceZoneName
            string surfaceZoneName = @"(?'1'.*)(?'zoneName'!- Zone Name)";
            Regex surfaceZoneNameRegex = new Regex(surfaceZoneName);
            //Regex for outsideBoundary
            string outsideBoundaryName = @"(?'1'.*)(?'outsideBoundaryName'!- Outside Boundary Condition)";
            Regex outsideBoundaryRegex = new Regex(outsideBoundaryName);
            //Regex for outsideBoundary Condition
            string outsideBoundaryCondition = @"(?'1'.*)(?'outsideBoundaryCondition'!- Outside Boundary Condition Object)";
            Regex outsideBoundaryConditionRegex = new Regex(outsideBoundaryCondition);
            //Regex for sunExposure
            string sunExposure = @"(?'1'.*)(?'sunExposure'!- Sun Exposure)";
            Regex sunExposureRegex = new Regex(sunExposure);
            //Regex for windExposure
            string windExposure = @"(?'1'.*)(?'windExposure'!- Wind Exposure)";
            Regex windExposureRegex = new Regex(windExposure);
            //Regex for ViewFactor
            string viewFactor = @"(?'1'.*)(?'viewFactor'!- View Factor to Ground)";
            Regex viewFactorRegex = new Regex(viewFactor);
            //Regex for numberofVertices
            string numberofVertices = @"(?'1'.*)(?'vertices'!- Number of Vertices)";
            Regex numberofVerticesRegex = new Regex(numberofVertices);
            //something else for the vertex
            //Regex for generic Vertex
            string typicalVertex = @"(?'1'.*)(?'vertices'!- X\d*\s*,Y\d*\s*,Z\d*\s*)";
            Regex typicalVertexRegex = new Regex(typicalVertex);

            //a regex for finding a semicolon
            string semicolon = @"(\S*)(;)(.*)";
            Regex smicln = new Regex(semicolon);
            #endregion

            //make a list of spaces



            //special needed to allow the routine to run successfully
            //needed because the regex may return true in the wrong instance
            bool outsideBoundaryMatched = false;
            bool semicolonfound = false;
            bool detailedsurface = false;
            bool vertexMatching = false;
            
            foreach (string line in detailedSurfaceString)
            {

                #region
                MatchCollection surfaceStart = surfaceYes.Matches(line);
                if (surfaceStart.Count > 0)
                {
                    detailedsurface = true;
                    continue;
                }
                //now that a surface element is established in the IDF, we can work through it to create surface objects
                if (detailedsurface == true)
                {
                    //Surface Name
                    //get the name in the file
                    if (!vertexMatching)
                    {
                        Match surfaceNameMatch = surfaceNameRegex.Match(line);
                        int matchstart, matchlength = -1;
                        if (surfaceNameMatch.Success)
                        {
                            matchstart = surfaceNameMatch.Index;
                            matchlength = surfaceNameMatch.Length;
                            //strip off the whitespace and comma
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(surfaceNameMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.name = pure.Groups["goods"].Value;
                                continue;
                            }

                        }
                        //Get Surface Type
                        Match surfaceTypeMatch = surfaceTypeRegex.Match(line);
                        if (surfaceTypeMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(surfaceTypeMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                string type = pure.Groups["goods"].Value;
                                type = type.ToLower();
                                if (type == EPlusObjects.Surface.SurfaceTypes.Ceiling.ToString().ToLower())
                                {
                                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Ceiling;
                                    continue;
                                }
                                else if (type == EPlusObjects.Surface.SurfaceTypes.Floor.ToString().ToLower())
                                {
                                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Floor;
                                    continue;
                                }
                                else if (type == EPlusObjects.Surface.SurfaceTypes.Roof.ToString().ToLower())
                                {
                                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Roof;
                                    continue;
                                }
                                else if (type == EPlusObjects.Surface.SurfaceTypes.Wall.ToString().ToLower())
                                {
                                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Wall;
                                    continue;
                                }

                            }
                        }
                        //Get Construction Type
                        Match constructionTypeMatch = surfaceConstructionNameRegex.Match(line);
                        if (constructionTypeMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(constructionTypeMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.constructionName = pure.Groups["goods"].Value;
                                continue;
                            }

                        }
                        //GetZone Name
                        Match zoneNameMatch = surfaceZoneNameRegex.Match(line);
                        if (zoneNameMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(zoneNameMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.zoneName = pure.Groups["goods"].Value;
                                continue;
                            }
                        }
                        //GetOutside Boundary Name

                        if (!outsideBoundaryMatched)
                        {
                            Match outsideBoundaryMatch = outsideBoundaryRegex.Match(line);
                            if (outsideBoundaryMatch.Success)
                            #region
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(outsideBoundaryMatch.Groups["1"].Value);
                                if (pure.Success)
                                {

                                    string type = pure.Groups["goods"].Value;
                                    type = type.ToLower();
                                    if (type == EPlusObjects.Surface.OutsideBoundary.Ground.ToString().ToLower())
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Ground;
                                        outsideBoundaryMatched = true;
                                    }
                                    else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideCoefficients.ToString().ToLower())
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideCoefficients;
                                        outsideBoundaryMatched = true;
                                    }
                                    else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideConditionsModel.ToString().ToLower())
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideConditionsModel;
                                        outsideBoundaryMatched = true;
                                    }
                                    else if (type == EPlusObjects.Surface.OutsideBoundary.Outdoors.ToString().ToLower())
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors;
                                        outsideBoundaryMatched = true;
                                    }
                                    else if (type == EPlusObjects.Surface.OutsideBoundary.Surface.ToString().ToLower())
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Surface;
                                        outsideBoundaryMatched = true;
                                    }
                                    else
                                    {
                                        currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Zone;
                                        outsideBoundaryMatched = true;
                                    }
                                    continue;
                                }
                            }
                        }
                            #endregion

                        //Get Outside Boundary Condition Object
                        Match outsideBoundaryConditionMatch = outsideBoundaryConditionRegex.Match(line);
                        if (outsideBoundaryConditionMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(outsideBoundaryConditionMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.outsideBoundaryCondition = pure.Groups["goods"].Value;
                                continue;
                            }
                        }
                        //Get Sun Exposure
                        Match sunExposureMatch = sunExposureRegex.Match(line);
                        if (sunExposureMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(sunExposureMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.sunExposureVar = pure.Groups["goods"].Value;
                                continue;
                            }
                        }
                        //Get Wind Exposure
                        Match windExposureMatch = windExposureRegex.Match(line);
                        if (windExposureMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(windExposureMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.windExposureVar = pure.Groups["goods"].Value;
                                continue;
                            }
                        }
                        //View Factor
                        Match viewFactorMatch = viewFactorRegex.Match(line);
                        if (viewFactorMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(viewFactorMatch.Groups["1"].Value);
                            if (pure.Success)
                            {

                                if (pure.Groups["goods"].Value == "AutoCalculate")
                                {
                                    currentSurface.viewFactor = -999;
                                }
                                else
                                {
                                    currentSurface.viewFactor = Convert.ToDouble(pure.Groups["goods"].Value);
                                }
                                continue;
                            }
                        }
                        //Number of Vertices
                        Match numVerticesMatch = numberofVerticesRegex.Match(line);
                        if (numVerticesMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(numVerticesMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                currentSurface.numVertices = Convert.ToInt32(pure.Groups["goods"].Value);
                                continue;
                            }
                        }
                    }
                    //Get Vertices
                    //loop through them until the end


                    Match vertexMatch = typicalVertexRegex.Match(line);
                    if (vertexMatch.Success)
                    {
                        vertexMatching = true;
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma')";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(vertexMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            //extract the X,Y, and Z coordinate from the purified string
                            //string coordinateString = @"(?'X'[-]\d+[\.]\d+|\d+),(?'Y'[-]\d+[\.]\d+|\d+),(?'Z'[-]\d+[\.]\d+|\d+)";
                            string coordinateString = @"(?'X'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Y'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Z'[-+]?([0-9]*\.[0-9]+|[0-9]+))";
                            Regex coordRegex = new Regex(coordinateString);
                            Match XYZMatch = coordRegex.Match(pure.Groups["goods"].Value);
                            if (XYZMatch.Success)
                            {
                                Vector.CartCoord surfaceCoord = new Vector.CartCoord();
                                surfaceCoord.X = Convert.ToDouble(XYZMatch.Groups["X"].Value);
                                surfaceCoord.Y = Convert.ToDouble(XYZMatch.Groups["Y"].Value);
                                surfaceCoord.Z = Convert.ToDouble(XYZMatch.Groups["Z"].Value);
                                currentSurface.SurfaceCoords.Add(surfaceCoord);
                            }
                        }


                        //see if there is a semi-colon
                        Match smicolonMatch = smicln.Match(line);
                        if (smicolonMatch.Success)
                        {
                            semicolonfound = true;
                            vertexMatching = false;
                        }
                    }
                }
                
            }
                #endregion
            //close the reader


            //get the RHR Normal Vector
            Vector.CartVect RHRNormalVector = Vector.GetRHR(currentSurface.SurfaceCoords);
            logline.AppendLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            Console.WriteLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            //get azimuth
            currentSurface.azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
            //get tilt
            Vector.MemorySafe_CartVect memRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
            currentSurface.tilt = EPlusFunctions.FindTilt(memRHR);
            //in any case, return the current surface
            logline.AppendLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());
            Console.WriteLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());

            
            return currentSurface;
        }

        static public ModelingUtilities.BuildingObjects.Surface ADEPlusSurfacetoObject(List<string> detailedSurfaceString)
        {
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Users\Chiensi\Documents\AAATerabuild\EnergyPlus\homework\surf2objlog.txt";
            StringBuilder output = new StringBuilder();
            //need to add a try/catch clause and start to work on try/catches when I get the chance

            //initialize the surface to be returned
            ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
            currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
            currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;

            StringBuilder logline = new StringBuilder();
            //Constructions - Opaque Detailed...

            #region
            //start with the Regexes needed to parse out the opaque constructions
            //Regex for beginning of a detailed surface element,
            string startSurface = "(?'1'.*)(?'surfaceStart'BuildingSurface:Detailed,)";
            Regex surfaceYes = new Regex(startSurface);
            //Regex for surfaceName
            //string surfaceName = @"(?'ws1'\s*)(?'name'.*)(?'ws2'\s*)(?'surfaceName'!- Name)";
            string surfaceName = @"(?'1'.*)(?'surfaceName'! Surface Name)";
            //string surfaceName = @"(?'surfaceName'!- Name)";
            Regex surfaceNameRegex = new Regex(surfaceName);
            //Regex for surface Type
            string surfaceType = @"(?'1'.*)(?'surfaceType'! Surface Type)";
            Regex surfaceTypeRegex = new Regex(surfaceType);
            //Regext for surfaceConstructionName
            string surfaceConstructionName = @"(?'1'.*)(?'construction'! Class and Construction Name)";
            Regex surfaceConstructionNameRegex = new Regex(surfaceConstructionName);
            //unused by ADesk **
            //Regext for surfaceZoneName
            //string surfaceZoneName = @"(?'1'.*)(?'zoneName'!- Zone Name)";
            //Regex surfaceZoneNameRegex = new Regex(surfaceZoneName);
            //Regex for inside Surface Boundary
            string surfaceInsideBounds = @"(?'1'.*)(?'zoneName'!-Inside Face Environment)";
            Regex surfaceInsideBoundsRegex = new Regex(surfaceInsideBounds);
            //unused by ADesk **
            //Regex for outsideBoundary
            //string outsideBoundaryName = @"(?'1'.*)(?'outsideBoundaryName'!- Outside Boundary Condition)";
            //Regex outsideBoundaryRegex = new Regex(outsideBoundaryName);
            //unused by ADesk **
            //Regex for outsideBoundary Condition
            //string outsideBoundaryCondition = @"(?'1'.*)(?'outsideBoundaryCondition'!- Outside Boundary Condition Object)";
            //Regex outsideBoundaryConditionRegex = new Regex(outsideBoundaryCondition);
            //unused by ADesk **
            //Regex for sunExposure
            //string sunExposure = @"(?'1'.*)(?'sunExposure'!- Sun Exposure)";
            //Regex sunExposureRegex = new Regex(sunExposure);
            //unused by ADesk **
            //Regex for windExposure
            //string windExposure = @"(?'1'.*)(?'windExposure'!- Wind Exposure)";
            //Regex windExposureRegex = new Regex(windExposure);
            //
            //Regex for ViewFactor
            //string viewFactor = @"(?'1'.*)(?'viewFactor'!- View Factor to Ground)";
            //Regex viewFactorRegex = new Regex(viewFactor);
            //unused by ADesk **
            //Regex for numberofVertices
            //string numberofVertices = @"(?'1'.*)(?'vertices'!- Number of Vertices)";
            //Regex numberofVerticesRegex = new Regex(numberofVertices);
            //something else for the vertex
            // We are ignoring this in Adesk because we use a counter ! Next 12 lines are xyz coordinates of 4 vertices
            //Regex for generic Vertex
            string typicalVertex = @"(?'1'.*)(?'vertices'!- X\d*\s*,Y\d*\s*,Z\d*\s*)";
            Regex typicalVertexRegex = new Regex(typicalVertex);

            //a regex for finding a semicolon
            string semicolon = @"(\S*)(;)(.*)";
            Regex smicln = new Regex(semicolon);
            #endregion

            //make a list of spaces



            //special needed to allow the routine to run successfully
            //needed because the regex may return true in the wrong instance
            bool outsideBoundaryMatched = false;
            bool semicolonfound = false;
            bool detailedsurface = false;
            bool vertexMatching = false;
            int linecount = 0;
            foreach (string line in detailedSurfaceString)
            {
                linecount++;
                #region
                MatchCollection surfaceStart = surfaceYes.Matches(line);
                if (surfaceStart.Count > 0)
                {
                    detailedsurface = true;
                    continue;
                }
                //now that a surface element is established in the IDF, we can work through it to create surface objects
                if (detailedsurface == true)
                {
                    //Surface Name
                    //get the name in the file
                    Match surfaceNameMatch = surfaceNameRegex.Match(line);
                    int matchstart, matchlength = -1;
                    if (surfaceNameMatch.Success)
                    {
                        matchstart = surfaceNameMatch.Index;
                        matchlength = surfaceNameMatch.Length;
                        //strip off the whitespace and comma
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(surfaceNameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            currentSurface.name = pure.Groups["goods"].Value;
                            continue;
                        }

                    }
                    //Get Surface Type
                    Match surfaceTypeMatch = surfaceTypeRegex.Match(line);
                    if (surfaceTypeMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(surfaceTypeMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            string type = pure.Groups["goods"].Value;
                            type = type.ToLower();
                            if (type == EPlusObjects.Surface.SurfaceTypes.Ceiling.ToString().ToLower())
                            {
                                currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Ceiling;
                                continue;
                            }
                            else if (type == EPlusObjects.Surface.SurfaceTypes.Floor.ToString().ToLower())
                            {
                                currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Floor;
                                continue;
                            }
                            else if (type == EPlusObjects.Surface.SurfaceTypes.Roof.ToString().ToLower())
                            {
                                currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Roof;
                                continue;
                            }
                            else if (type == EPlusObjects.Surface.SurfaceTypes.Wall.ToString().ToLower())
                            {
                                currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Wall;
                                continue;
                            }

                        }
                    }
                    //Get Construction Type
                    Match constructionTypeMatch = surfaceConstructionNameRegex.Match(line);
                    if (constructionTypeMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(constructionTypeMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            currentSurface.constructionName = pure.Groups["goods"].Value;
                            continue;
                        }

                    }
                    Match insideFaceEnvMatch = surfaceInsideBoundsRegex.Match(line);
                    if (insideFaceEnvMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(insideFaceEnvMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            currentSurface.zoneName = pure.Groups["goods"].Value;
                            continue;
                        }
                    }
                    //Outside Boundary condition lineCount = 6
                    if (linecount == 6)
                    {
                        string commapurify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(commapurify);
                        Match pure = purifyRegex.Match(line);
                        if (pure.Success)
                        {
                            string type = pure.Groups["goods"].Value;
                            type = type.ToLower();
                            if (type == EPlusObjects.Surface.OutsideBoundary.Ground.ToString().ToLower())
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Ground;
                                outsideBoundaryMatched = true;
                            }
                            else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideCoefficients.ToString().ToLower())
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideCoefficients;
                                outsideBoundaryMatched = true;
                            }
                            else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideConditionsModel.ToString().ToLower())
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideConditionsModel;
                                outsideBoundaryMatched = true;
                            }
                            else if (type == EPlusObjects.Surface.OutsideBoundary.Outdoors.ToString().ToLower())
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors;
                                outsideBoundaryMatched = true;
                            }
                            else if (type == EPlusObjects.Surface.OutsideBoundary.Surface.ToString().ToLower())
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Surface;
                                outsideBoundaryMatched = true;
                            }
                            else
                            {
                                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Zone;
                                outsideBoundaryMatched = true;
                            }
                            continue;
                        }
                    }
                    //Outside Boundary Condition Object lineCount = 7
                    if (linecount == 7)
                    {
                        continue;
                    }
                    //SunExposure lineCount = 8
                    if (linecount == 8)
                    {
                        string commapurify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(commapurify);
                        Match pure = purifyRegex.Match(line);
                        if (pure.Success)
                        {
                            currentSurface.sunExposureVar = pure.Groups["goods"].Value;
                            continue;
                        }
                    }
                    //WindExposure lineCount = 9
                    if (linecount == 9)
                    {
                        string commapurify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(commapurify);
                        Match pure = purifyRegex.Match(line);
                        if (pure.Success)
                        {
                            currentSurface.windExposureVar = pure.Groups["goods"].Value;
                            continue;
                        }
                    }
                    //View Factor (skipping) lineCount = 10
                    if (linecount == 10)
                    {
                        continue;
                    }
                    //PolyGon vertices (skipping) lineCount = 11
                    if (linecount == 11)
                    {
                        continue;
                    }
                    //next 12 lines //skip lineCount = 12
                    if (linecount == 12)
                    {
                        continue;
                    }
                    //GetZone Name
                    //Match zoneNameMatch = surfaceZoneNameRegex.Match(line);
                    //if (zoneNameMatch.Success)
                    //{
                    //    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //    Regex purifyRegex = new Regex(purify);
                    //    Match pure = purifyRegex.Match(zoneNameMatch.Groups["1"].Value);
                    //    if (pure.Success)
                    //    {
                    //        currentSurface.zoneName = pure.Groups["goods"].Value;
                    //        continue;
                    //    }
                    //}
                    //GetOutside Boundary Name

                    //if (!outsideBoundaryMatched)
                    //{
                    //    Match outsideBoundaryMatch = outsideBoundaryRegex.Match(line);
                    //    if (outsideBoundaryMatch.Success)
                    //    #region
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(outsideBoundaryMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {

                    //            string type = pure.Groups["goods"].Value;
                    //            type = type.ToLower();
                    //            if (type == EPlusObjects.Surface.OutsideBoundary.Ground.ToString().ToLower())
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Ground;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideCoefficients.ToString().ToLower())
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideCoefficients;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            else if (type == EPlusObjects.Surface.OutsideBoundary.OtherSideConditionsModel.ToString().ToLower())
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.OtherSideConditionsModel;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            else if (type == EPlusObjects.Surface.OutsideBoundary.Outdoors.ToString().ToLower())
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            else if (type == EPlusObjects.Surface.OutsideBoundary.Surface.ToString().ToLower())
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Surface;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            else
                    //            {
                    //                currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Zone;
                    //                outsideBoundaryMatched = true;
                    //            }
                    //            continue;
                    //        }
                    //    }
                    //}
            #endregion

                    //    //Get Outside Boundary Condition Object
                    //    Match outsideBoundaryConditionMatch = outsideBoundaryConditionRegex.Match(line);
                    //    if (outsideBoundaryConditionMatch.Success)
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(outsideBoundaryConditionMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {
                    //            currentSurface.outsideBoundaryCondition = pure.Groups["goods"].Value;
                    //            continue;
                    //        }
                    //    }
                    //    //Get Sun Exposure
                    //    Match sunExposureMatch = sunExposureRegex.Match(line);
                    //    if (sunExposureMatch.Success)
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(sunExposureMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {
                    //            currentSurface.sunExposureVar = pure.Groups["goods"].Value;
                    //            continue;
                    //        }
                    //    }
                    //    //Get Wind Exposure
                    //    Match windExposureMatch = windExposureRegex.Match(line);
                    //    if (windExposureMatch.Success)
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(windExposureMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {
                    //            currentSurface.windExposureVar = pure.Groups["goods"].Value;
                    //            continue;
                    //        }
                    //    }
                    //    //View Factor
                    //    Match viewFactorMatch = viewFactorRegex.Match(line);
                    //    if (viewFactorMatch.Success)
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(viewFactorMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {

                    //            if (pure.Groups["goods"].Value == "AutoCalculate")
                    //            {
                    //                currentSurface.viewFactor = -999;
                    //            }
                    //            else
                    //            {
                    //                currentSurface.viewFactor = Convert.ToDouble(pure.Groups["goods"].Value);
                    //            }
                    //            continue;
                    //        }
                    //    }
                    //    //Number of Vertices
                    //    Match numVerticesMatch = numberofVerticesRegex.Match(line);
                    //    if (numVerticesMatch.Success)
                    //    {
                    //        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                    //        Regex purifyRegex = new Regex(purify);
                    //        Match pure = purifyRegex.Match(numVerticesMatch.Groups["1"].Value);
                    //        if (pure.Success)
                    //        {
                    //            currentSurface.numVertices = Convert.ToInt32(pure.Groups["goods"].Value);
                    //            continue;
                    //        }
                    //    }
                    //}
                    //Get Vertices
                    //loop through them until the end


                    if (linecount > 12)
                    {
                        vertexMatching = true;
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma')";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(line);
                        if (pure.Success)
                        {
                            //extract the X,Y, and Z coordinate from the purified string
                            //string coordinateString = @"(?'X'[-]\d+[\.]\d+|\d+),(?'Y'[-]\d+[\.]\d+|\d+),(?'Z'[-]\d+[\.]\d+|\d+)";
                            string coordinateString = @"(?'X'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Y'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Z'[-+]?([0-9]*\.[0-9]+|[0-9]+))";
                            Regex coordRegex = new Regex(coordinateString);
                            string coords = pure.Groups["goods"].Value.Replace(" ", string.Empty);
                            Match XYZMatch = coordRegex.Match(coords);
                            if (XYZMatch.Success)
                            {
                                Vector.CartCoord surfaceCoord = new Vector.CartCoord();
                                surfaceCoord.X = Convert.ToDouble(XYZMatch.Groups["X"].Value);
                                surfaceCoord.Y = Convert.ToDouble(XYZMatch.Groups["Y"].Value);
                                surfaceCoord.Z = Convert.ToDouble(XYZMatch.Groups["Z"].Value);
                                currentSurface.SurfaceCoords.Add(surfaceCoord);
                            }
                        }


                        //see if there is a semi-colon
                        Match smicolonMatch = smicln.Match(line);
                        if (smicolonMatch.Success)
                        {
                            semicolonfound = true;
                            vertexMatching = false;
                        }
                    }
                    
                }
                
            }
                
            //close the reader


            //get the RHR Normal Vector
            Vector.CartVect RHRNormalVector = Vector.GetRHR(currentSurface.SurfaceCoords);
            logline.AppendLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            Console.WriteLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            //get azimuth
            currentSurface.azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
            //get tilt
            Vector.MemorySafe_CartVect memRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
            currentSurface.tilt = EPlusFunctions.FindTilt(memRHR);
            //in any case, return the current surface
            logline.AppendLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());
            Console.WriteLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());


            return currentSurface;
        }

        static public ModelingUtilities.BuildingObjects.Surface ADEPlusShadetoObject(List<string> detailedSurfaceString)
        {
            //create your log file writer, that will be used in stream writer at the bottom of this page
            string log = @"C:\Users\Chiensi\Documents\AAATerabuild\EnergyPlus\homework\surf2objlog.txt";
            StringBuilder output = new StringBuilder();
            //need to add a try/catch clause and start to work on try/catches when I get the chance

            //initialize the surface to be returned
            ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
            currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
            currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;

            StringBuilder logline = new StringBuilder();
            //Constructions - Opaque Detailed...

            #region
            //start with the Regexes needed to parse out the opaque constructions
            //Regex for beginning of a detailed surface element,
            string startSurface = "(?'1'.*)(?'surfaceStart'Shading:Site:Detailed,)";
            Regex surfaceYes = new Regex(startSurface);
            //Regex for surfaceName
            //string surfaceName = @"(?'ws1'\s*)(?'name'.*)(?'ws2'\s*)(?'surfaceName'!- Name)";
            string surfaceName = @"(?'1'.*)(?'surfaceName'! Surface Name)";
            //string surfaceName = @"(?'surfaceName'!- Name)";
            Regex surfaceNameRegex = new Regex(surfaceName);
            //Regex for generic Vertex
            string typicalVertex = @"(?'1'.*)(?'vertices'!- X\d*\s*,Y\d*\s*,Z\d*\s*)";
            Regex typicalVertexRegex = new Regex(typicalVertex);

            //a regex for finding a semicolon
            string semicolon = @"(\S*)(;)(.*)";
            Regex smicln = new Regex(semicolon);
            #endregion

            //make a list of spaces



            //special needed to allow the routine to run successfully
            //needed because the regex may return true in the wrong instance
            bool semicolonfound = false;
            bool detailedsurface = false;
            bool vertexMatching = false;
            int linecount = 0;
            foreach (string line in detailedSurfaceString)
            {
                linecount++;
                #region
                MatchCollection surfaceStart = surfaceYes.Matches(line);
                if (surfaceStart.Count > 0)
                {
                    detailedsurface = true;
                    continue;
                }
                //now that a surface element is established in the IDF, we can work through it to create surface objects
                if (detailedsurface == true)
                {
                    //Surface Name
                    //get the name in the file
                    Match surfaceNameMatch = surfaceNameRegex.Match(line);
                    int matchstart, matchlength = -1;
                    if (surfaceNameMatch.Success)
                    {
                        matchstart = surfaceNameMatch.Index;
                        matchlength = surfaceNameMatch.Length;
                        //strip off the whitespace and comma
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(surfaceNameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            currentSurface.name = pure.Groups["goods"].Value;
                            continue;
                        }

                    }
                    
                    //View Factor (skipping) lineCount = 3
                    if (linecount == 3)
                    {
                        continue;
                    }
                    //PolyGon vertices (skipping) lineCount = 4
                    if (linecount == 4)
                    {
                        continue;
                    }
                    //next 12 lines //skip lineCount = 5
                    if (linecount == 5)
                    {
                        continue;
                    }
                   
                #endregion

                    if (linecount > 5)
                    {
                        vertexMatching = true;
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma')";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(line);
                        if (pure.Success)
                        {
                            //extract the X,Y, and Z coordinate from the purified string
                            //string coordinateString = @"(?'X'[-]\d+[\.]\d+|\d+),(?'Y'[-]\d+[\.]\d+|\d+),(?'Z'[-]\d+[\.]\d+|\d+)";
                            string coordinateString = @"(?'X'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Y'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Z'[-+]?([0-9]*\.[0-9]+|[0-9]+))";
                            Regex coordRegex = new Regex(coordinateString);
                            string coords = pure.Groups["goods"].Value.Replace(" ", string.Empty);
                            Match XYZMatch = coordRegex.Match(coords);
                            if (XYZMatch.Success)
                            {
                                Vector.CartCoord surfaceCoord = new Vector.CartCoord();
                                surfaceCoord.X = Convert.ToDouble(XYZMatch.Groups["X"].Value);
                                surfaceCoord.Y = Convert.ToDouble(XYZMatch.Groups["Y"].Value);
                                surfaceCoord.Z = Convert.ToDouble(XYZMatch.Groups["Z"].Value);
                                currentSurface.SurfaceCoords.Add(surfaceCoord);
                            }
                        }


                        //see if there is a semi-colon
                        Match smicolonMatch = smicln.Match(line);
                        if (smicolonMatch.Success)
                        {
                            semicolonfound = true;
                            vertexMatching = false;
                        }
                    }

                }

            }

            //close the reader


            //get the RHR Normal Vector
            Vector.CartVect RHRNormalVector = Vector.GetRHR(currentSurface.SurfaceCoords);
            logline.AppendLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            Console.WriteLine(currentSurface.name + ", " + currentSurface.surfaceType + ", " + currentSurface.outsideBoundary.ToString());
            //get azimuth
            currentSurface.azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
            //get tilt
            Vector.MemorySafe_CartVect memRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
            currentSurface.tilt = EPlusFunctions.FindTilt(memRHR);
            //in any case, return the current surface
            logline.AppendLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());
            Console.WriteLine(RHRNormalVector.X.ToString() + ", " + RHRNormalVector.Y.ToString() + ", " + RHRNormalVector.Z.ToString() + ", " + currentSurface.azimuth.ToString() + ", " + currentSurface.tilt.ToString());


            return currentSurface;
        }

        static public List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> MakeEPlusSurfaceList(string idfName)
        {

            StringBuilder output = new StringBuilder();

            //set up the zone Group and Zone Lists
            List<EPlusObjects.ZoneGroup> zoneGroups = new List<EPlusObjects.ZoneGroup>();
            List<EPlusObjects.ZoneList> zoneLists = new List<EPlusObjects.ZoneList>();
            try
            {
                //may also want to have 
                //the zone list and affiliated spaces should come first

                Regex zoneListYes = new Regex(EPlusObjects.EPlusRegexString.startZoneList);
                Regex zoneGroupYes = new Regex(EPlusObjects.EPlusRegexString.startZoneGroup);
                Regex zoneGroupNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex zoneListNameRegex = new Regex(EPlusObjects.EPlusRegexString.zoneListName);
                Regex zoneListNameMultiplier = new Regex(EPlusObjects.EPlusRegexString.zoneListMultiplier);
                Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                Encoding encoding;

                using (StreamReader reader = new StreamReader(idfName))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool zoneListBool = false;
                    bool zoneGroupBool = false;
                    List<string> zoneListStrings = new List<string>();
                    List<string> zoneGroupStrings = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match zoneListFound = zoneListYes.Match(line);
                        if (zoneListFound.Success)
                        {
                            zoneListBool = true;
                            zoneListStrings.Add(line);
                            continue;
                        }
                        if (zoneListBool)
                        {
                            //parse the line to get the name of the zone
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneListStrings.Add(line);

                            }
                            else
                            {
                                zoneListStrings.Add(line);
                                zoneListBool = false;
                                EPlusObjects.ZoneList zoneList = EPlusFunctions.MakeZoneList(zoneListStrings);
                                zoneLists.Add(zoneList);
                                zoneListStrings.Clear();
                            }
                        }
                        Match zoneGroupFound = zoneGroupYes.Match(line);
                        if (zoneGroupFound.Success)
                        {
                            zoneGroupBool = true;
                            zoneGroupStrings.Add(line);
                            continue;
                        }
                        if (zoneGroupBool)
                        {
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneGroupStrings.Add(line);
                            }
                            else
                            {
                                zoneGroupStrings.Add(line);
                                zoneGroupBool = false;
                                EPlusObjects.ZoneGroup zoneGroup = EPlusFunctions.MakeZoneGroup(zoneGroupStrings);
                                zoneGroups.Add(zoneGroup);
                                zoneGroupStrings.Clear();

                            }
                        }

                        //make a zone List object 

                        //find the zone group that is relate to this list
                    }
                    reader.Close();

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //set up the surface list
            List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
            try
            {
                //list of strings that define a detailed surface
                List<string> stuff = new List<string>();
                //need regexes
                Regex surfaceYes = new Regex(EPlusObjects.EPlusRegexString.startSurface);
                Regex smicln = new Regex(EPlusObjects.EPlusRegexString.semicolon);


                using (StreamReader reader = new StreamReader(idfName))
                {
                    string line;
                    bool detailedsurface = false;

                    //set up the surface
                    ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
                    currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
                    currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
                    while ((line = reader.ReadLine()) != null)
                    {
                        #region
                        MatchCollection surfaceStart = surfaceYes.Matches(line);
                        if (surfaceStart.Count > 0)
                        {
                            detailedsurface = true;
                            currentSurface.Clear();
                            stuff.Add(line);
                            continue;
                        }
                        //now that a surface element is established in the IDF, we can work through it to create surface objects
                        if (detailedsurface == true)
                        {
                            //GetAllDetailedSurfaces
                            //use streamswriter to make a little text file that will then be turned into an object
                            //write to the small output stream until you encounter a semi-colon
                            stuff.Add(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedsurface = false;
                                //write the output file
                                //send the output file to a function, returning a surface
                                ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.ADEPlusSurfacetoObject(stuff);
                                foreach (EPlusObjects.ZoneGroup zoneGroup in zoneGroups)
                                {
                                    string zoneListName = zoneGroup.zoneListName;
                                    foreach (EPlusObjects.ZoneList zoneList in zoneLists)
                                    {
                                        if (zoneList.name == zoneListName)
                                        {
                                            foreach (string zoneName in zoneList.zoneListNames)
                                            {
                                                if (surfaceReturned.zoneName == zoneName)
                                                {
                                                    //add a multiplier to the surface
                                                    surfaceReturned.multiplier = zoneGroup.zoneListMultiplier;
                                                }
                                            }
                                        }
                                    }
                                }
                                //destroy the temporary text file
                                stuff.Clear();
                                ModelingUtilities.BuildingObjects.MemorySafe_Surface memSurf = ModelingUtilities.BuildingObjects.convert2MemorySafeSurface(surfaceReturned);
                                projectSurfaces.Add(memSurf);
                                //write to the console and to the log file
                                output.AppendLine(currentSurface.name + "," + currentSurface.numVertices + "," + currentSurface.tilt);
                            }
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedWall.csv", false))
                {
                    writer.Write(output.ToString());
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return projectSurfaces;
        }

        static public List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> MakeEPlusShadeList(string idfName)
        {

            StringBuilder output = new StringBuilder();

            //set up the surface list
            List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectShades = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
            try
            {
                //list of strings that define a detailed surface
                List<string> stuff = new List<string>();
                //need regexes
                Regex shadeYes = new Regex(EPlusObjects.EPlusRegexString.startDetailedSiteShade);
                Regex smicln = new Regex(EPlusObjects.EPlusRegexString.semicolon);


                using (StreamReader reader = new StreamReader(idfName))
                {
                    string line;
                    bool detailedsurface = false;

                    //set up the surface object (which is an expanded version of a shade)
                    ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
                    currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
                    currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        MatchCollection surfaceStart = shadeYes.Matches(line);
                        if (surfaceStart.Count > 0)
                        {
                            detailedsurface = true;
                            currentSurface.Clear();
                            stuff.Add(line);
                            continue;
                        }
                        //now that a surface element is established in the IDF, we can work through it to create surface objects
                        if (detailedsurface == true)
                        {
                            //GetAllDetailedSurfaces
                            //use streamswriter to make a little text file that will then be turned into an object
                            //write to the small output stream until you encounter a semi-colon
                            stuff.Add(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedsurface = false;
                                //write the output file
                                //send the output file to a function, returning a surface
                                ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.ADEPlusShadetoObject(stuff);
                                
                                //destroy the temporary text file
                                stuff.Clear();
                                ModelingUtilities.BuildingObjects.MemorySafe_Surface memSurf = ModelingUtilities.BuildingObjects.convert2MemorySafeSurface(surfaceReturned);
                                projectShades.Add(memSurf);
                                //write to the console and to the log file
                                output.AppendLine(currentSurface.name + "," + currentSurface.numVertices + "," + currentSurface.tilt);
                            }
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedWall.csv", false))
                {
                    writer.Write(output.ToString());
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return projectShades;
        }

        public static List<string> EPlusSpacesNames(string idfname)
        {
            //find multipliers if they exist by looking for Zone Groups


                
            List<string> zoneNames = new List<string>();
            //set up the list of spaces that will be collected

            //set up the log
            string log = @"C:\Temp\log.txt";
            StringBuilder logline = new StringBuilder();
            try
            {
                Encoding encoding;
                StringBuilder output = new StringBuilder();
                List<string> stuff = new List<string>();

                //needed regular expressions to build the surface description prior to it being made an object
 
                Regex zoneYes = new Regex(EPlusObjects.EPlusRegexString.zoneSizing);
                Regex zoneName = new Regex(EPlusObjects.EPlusRegexString.zoneName);
                string semicolon = @"(\S*)(;)(.*)";
                Regex smicln = new Regex(semicolon);



                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool zoneSizing = false;
                    //set up the surface

                    int surfcount = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        #region
                        Match zoneStart = zoneYes.Match(line);
                        if (zoneStart.Success)
                        {
                            zoneSizing = true;
                            continue;
                        }
                        //now that a surface element is established in the IDF, we can work through it to create surface objects
                        if (zoneSizing == true)
                        {
                            //GetAllDetailedSurfaces
                            //use streamswriter to make a little text file that will then be turned into an object
                            //write to the small output stream until you encounter a semi-colon
                            Match zoneNameMatch = zoneName.Match(line);
                            if(zoneNameMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(zoneNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    zoneNames.Add(pure.Groups["goods"].Value);
                                    logline.AppendLine(pure.Groups["goods"].Value);
                                    continue;
                                }
                                zoneSizing = false;
                            }

                        }
                        //while line reader is reading
                    }
                    //streamreader
                    reader.Close();
                    using (StreamWriter writer = new StreamWriter(log, false, encoding))
                    {
                        writer.Write(logline.ToString());
                    }
                }

                        #endregion


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return zoneNames;
        }

        public static List<ModelingUtilities.BuildingObjects.MemorySafe_Spaces> EPlusSpacestoObjectList(string idfname)
        {
            //find multipliers if they exist by looking for Zone Groups
            //I set this up to read the idf file twice, once in each try block
            List<ModelingUtilities.BuildingObjects.MemorySafe_Spaces> memSafeProjectSpaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Spaces>();
            List<EPlusObjects.ZoneGroup> zoneGroups = new List<EPlusObjects.ZoneGroup>();
            List<EPlusObjects.ZoneList> zoneLists = new List<EPlusObjects.ZoneList>();
            try
            {
                //may also want to have 
                //the zone list and affiliated spaces should come first

                Regex zoneListYes = new Regex(EPlusObjects.EPlusRegexString.startZoneList);
                Regex zoneGroupYes = new Regex(EPlusObjects.EPlusRegexString.startZoneGroup);
                Regex zoneGroupNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex zoneListNameRegex = new Regex(EPlusObjects.EPlusRegexString.zoneListName);
                Regex zoneListNameMultiplier = new Regex(EPlusObjects.EPlusRegexString.zoneListMultiplier);
                Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                Encoding encoding;

                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool zoneListBool = false;
                    bool zoneGroupBool = false;
                    List<string> zoneListStrings = new List<string>();
                    List<string> zoneGroupStrings = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match zoneListFound = zoneListYes.Match(line);
                        if (zoneListFound.Success)
                        {
                            zoneListBool = true;
                            zoneListStrings.Add(line);
                            continue;
                        }
                        if (zoneListBool)
                        {
                            //parse the line to get the name of the zone
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneListStrings.Add(line);

                            }
                            else
                            {
                                zoneListStrings.Add(line);
                                zoneListBool = false;
                                EPlusObjects.ZoneList zoneList = EPlusFunctions.MakeZoneList(zoneListStrings);
                                zoneLists.Add(zoneList);
                                zoneListStrings.Clear();
                            }
                        }
                        Match zoneGroupFound = zoneGroupYes.Match(line);
                        if (zoneGroupFound.Success)
                        {
                            zoneGroupBool = true;
                            zoneGroupStrings.Add(line);
                            continue;
                        }
                        if (zoneGroupBool)
                        {
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneGroupStrings.Add(line);
                            }
                            else
                            {
                                zoneGroupStrings.Add(line);
                                zoneGroupBool = false;
                                EPlusObjects.ZoneGroup zoneGroup = EPlusFunctions.MakeZoneGroup(zoneGroupStrings);
                                zoneGroups.Add(zoneGroup);
                                zoneGroupStrings.Clear();

                            }
                        }

                        //make a zone List object 

                        //find the zone group that is relate to this list
                    }
                    reader.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //set up the list of spaces that will be collected
            List<ModelingUtilities.BuildingObjects.Spaces> projectSpaces = new List<ModelingUtilities.BuildingObjects.Spaces>();
            //set up the log
            string linelog = @"C:\Temp\foundEPsurfaceslog.txt";
            string surfacelog = @"C:\Temp\surfaceCountlog.txt";
            StringBuilder logline = new StringBuilder();
            StringBuilder countline = new StringBuilder();
            try
            {
                Encoding encoding;
                StringBuilder output = new StringBuilder();
                List<string> stuff = new List<string>();

                //needed regular expressions to build the surface description prior to it being made an object
                string startSurface = "(?'1'.*)(?'surfaceStart'BuildingSurface:Detailed,)";
                Regex surfaceYes = new Regex(startSurface);
                Regex peopleStart = new Regex(EPlusObjects.EPlusRegexString.people);
                Regex lightStart = new Regex(EPlusObjects.EPlusRegexString.lights);
                Regex equipStart = new Regex(EPlusObjects.EPlusRegexString.equipment);
                string semicolon = @"(\S*)(;)(.*)";
                Regex smicln = new Regex(semicolon);
                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedsurface = false;
                    bool lightsFound = false;
                    string lightsname = "";
                    bool equipFound = false;
                    string equipname = "";
                    bool peopleFound = false;
                    string peoplename = "";
                    //set up the surface
                    ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
                    currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
                    currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
                    int surfcount = 0;
                    int linecount = 0;
                    List<int> linelist = new List<int>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        linecount++;
                        #region
                        MatchCollection surfaceStart = surfaceYes.Matches(line);
                        if (surfaceStart.Count > 0)
                        {
                            detailedsurface = true;
                            currentSurface.Clear();
                            stuff.Add(line);
                            output.AppendLine(line);
                            linelist.Add(linecount);
                            logline.Append(linecount.ToString()+Environment.NewLine);
                            continue;
                        }
                        Match eplight = lightStart.Match(line);
                        if (eplight.Success)
                        {
                            lightsFound = true;
                            continue;
                        }
                        Match epEquip = equipStart.Match(line);
                        if (epEquip.Success)
                        {
                            equipFound = true;
                            continue;
                        }
                        Match epPeople = peopleStart.Match(line);
                        if (epPeople.Success)
                        {
                            peopleFound = true;
                            continue;
                        }
                        if (lightsFound)
                        {
                            Match name = lightStart.Match(line);
                            if (name.Success)
                            {

                            }
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                lightsFound = false;
                                //do stuff
                                stuff.Clear();
                                output.Clear();
                            }
                            
                        }
                        if (equipFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                equipFound = false;
                                //do stuff
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        if (peopleFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                peopleFound = false;
                                //do stuff
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        //now that a surface element is established in the IDF, we can work through it to create surface objects
                        if (detailedsurface == true)
                        {
                            //GetAllDetailedSurfaces
                            //use streamswriter to make a little text file that will then be turned into an object
                            //write to the small output stream until you encounter a semi-colon
                            stuff.Add(line);
                            output.AppendLine(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedsurface = false;
                                string pass = output.ToString();
                                //write the output file
                                //send the output file to a function, returning a surface
                                ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.ADEPlusSurfacetoObject(stuff);
                                //add a multiplier to the surface if needed
                                foreach (EPlusObjects.ZoneGroup zoneGroup in zoneGroups)
                                {
                                    string zoneListName = zoneGroup.zoneListName;
                                    foreach (EPlusObjects.ZoneList zoneList in zoneLists)
                                    {
                                        if (zoneList.name == zoneListName)
                                        {
                                            foreach (string zoneName in zoneList.zoneListNames)
                                            {
                                                if (surfaceReturned.zoneName == zoneName)
                                                {
                                                    //add a multiplier to the surface
                                                    surfaceReturned.multiplier = zoneGroup.zoneListMultiplier;
                                                }
                                            }
                                        }
                                    }
                                }
                                surfcount++;
                                countline.Append(surfcount.ToString()+Environment.NewLine);
                                //ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.EPlusSurfacetoObject("C:\\Temp\\detailedSurface.txt");                                
                                output.Clear();
                                stuff.Clear();
                                if (projectSpaces.Count == 0)
                                {
                                    string tagline = "First project zone detected in Surfaces.";
                                    //logline.AppendLine(line);
                                    string zoneName = surfaceReturned.zoneName;
                                    string surfaceName = surfaceReturned.name;
                                    //logline.AppendLine(zoneName + ": " + surfaceName);
                                    Console.WriteLine(tagline);
                                    Console.WriteLine(zoneName + ": " + surfaceName);
                                    ModelingUtilities.BuildingObjects.Spaces spaceInstance = new ModelingUtilities.BuildingObjects.Spaces();
                                    spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                    spaceInstance.name = surfaceReturned.zoneName;
                                    spaceInstance.spaceSurfaces.Add(surfaceReturned);
                                    projectSpaces.Add(spaceInstance);
                                }
                                else
                                {
                                    //search for the space name in the existing List of Spaces
                                    bool spacefound = false;
                                    for (int i = 0; i < projectSpaces.Count; i++)
                                    {
                                        string projname = projectSpaces[i].name;
                                        if (projname == surfaceReturned.zoneName)
                                        {
                                            string tagline = "Existing project zone detected in Surfaces.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            string zoneName = surfaceReturned.zoneName;
                                            string surfaceName = surfaceReturned.name;
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": " + surfaceName);
                                            projectSpaces[i].spaceSurfaces.Add(surfaceReturned);
                                            spacefound = true;
                                            break;
                                        }
                                    }
                                    //if spacefound is never set to true
                                    if (!spacefound)
                                    {
                                        string tagline = "New project zone detected in Surfaces.";
                                        //logline.AppendLine(tagline);
                                        Console.WriteLine(tagline);
                                        string zoneName = surfaceReturned.zoneName;
                                        string surfaceName = surfaceReturned.name;
                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(zoneName + ": " + surfaceName);
                                        ModelingUtilities.BuildingObjects.Spaces spaceInstance = new ModelingUtilities.BuildingObjects.Spaces();
                                        spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                        spaceInstance.name = surfaceReturned.zoneName;
                                        spaceInstance.spaceSurfaces.Add(surfaceReturned);
                                        projectSpaces.Add(spaceInstance);
                                    }

                                }
                                //semicolon match = true
                            }
                            //detailed surfaces = true
                        }
                        //while line reader is reading
                    }
                    //convert these spaces to memory safe spaces
                    
                    foreach (ModelingUtilities.BuildingObjects.Spaces space in projectSpaces)
                    {
                        string zoneName = space.name;
                        int multiplier = space.multiplier;
                        List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSpaceSurfaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
                        foreach (ModelingUtilities.BuildingObjects.Surface surface in space.spaceSurfaces)
                        {
                            ModelingUtilities.BuildingObjects.MemorySafe_Surface memsurf = ModelingUtilities.BuildingObjects.convert2MemorySafeSurface(surface);
                            projectSpaceSurfaces.Add(memsurf);
                        }

                        ModelingUtilities.BuildingObjects.MemorySafe_Spaces memSpace = new ModelingUtilities.BuildingObjects.MemorySafe_Spaces(zoneName,multiplier,projectSpaceSurfaces);
                        memSafeProjectSpaces.Add(memSpace);

                    }
                    //streamreader
                    reader.Close();
                    using (StreamWriter writer = new StreamWriter(linelog, false, encoding))
                    {
                        writer.Write(logline.ToString());
                    }
                    using (StreamWriter countwrite = new StreamWriter(surfacelog, false, encoding))
                    {
                        countwrite.Write(countline.ToString());
                    }
                }

                        #endregion


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return memSafeProjectSpaces;
        }

        public static List<EPlusObjects.Zone> EPlusZonestoObjectList(string idfname)
        {
            //find multipliers if they exist by looking for Zone Groups
            //I set this up to read the idf file twice, once in each try block
            List<EPlusObjects.Zone> projectSpaces = new List<EPlusObjects.Zone>();
            List<EPlusObjects.ZoneGroup> zoneGroups = new List<EPlusObjects.ZoneGroup>();
            List<EPlusObjects.ZoneList> zoneLists = new List<EPlusObjects.ZoneList>();
            try
            {
                //may also want to have 
                //the zone list and affiliated spaces should come first

                Regex zoneListYes = new Regex(EPlusObjects.EPlusRegexString.startZoneList);
                Regex zoneGroupYes = new Regex(EPlusObjects.EPlusRegexString.startZoneGroup);
                Regex zoneGroupNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex zoneListNameRegex = new Regex(EPlusObjects.EPlusRegexString.zoneListName);
                Regex zoneListNameMultiplier = new Regex(EPlusObjects.EPlusRegexString.zoneListMultiplier);
                Regex semicolon = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                Encoding encoding;

                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool zoneListBool = false;
                    bool zoneGroupBool = false;
                    List<string> zoneListStrings = new List<string>();
                    List<string> zoneGroupStrings = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match zoneListFound = zoneListYes.Match(line);
                        if (zoneListFound.Success)
                        {
                            zoneListBool = true;
                            zoneListStrings.Add(line);
                            continue;
                        }
                        if (zoneListBool)
                        {
                            //parse the line to get the name of the zone
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneListStrings.Add(line);

                            }
                            else
                            {
                                zoneListStrings.Add(line);
                                zoneListBool = false;
                                EPlusObjects.ZoneList zoneList = EPlusFunctions.MakeZoneList(zoneListStrings);
                                zoneLists.Add(zoneList);
                                zoneListStrings.Clear();
                            }
                        }
                        Match zoneGroupFound = zoneGroupYes.Match(line);
                        if (zoneGroupFound.Success)
                        {
                            zoneGroupBool = true;
                            zoneGroupStrings.Add(line);
                            continue;
                        }
                        if (zoneGroupBool)
                        {
                            Match semiColonMatch = semicolon.Match(line);
                            if (!semiColonMatch.Success)
                            {
                                zoneGroupStrings.Add(line);
                            }
                            else
                            {
                                zoneGroupStrings.Add(line);
                                zoneGroupBool = false;
                                EPlusObjects.ZoneGroup zoneGroup = EPlusFunctions.MakeZoneGroup(zoneGroupStrings);
                                zoneGroups.Add(zoneGroup);
                                zoneGroupStrings.Clear();

                            }
                        }

                        //make a zone List object 

                        //find the zone group that is relate to this list
                    }
                    reader.Close();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //set up the list of spaces that will be collected
            
            //set up the log
            string linelog = @"C:\Temp\foundEPsurfaceslog.txt";
            string surfacelog = @"C:\Temp\surfaceCountlog.txt";
            StringBuilder logline = new StringBuilder();
            StringBuilder countline = new StringBuilder();
            try
            {
                Encoding encoding;
                StringBuilder output = new StringBuilder();
                List<string> stuff = new List<string>();

                //needed regular expressions to build the surface description prior to it being made an object
                string startSurface = "(?'1'.*)(?'surfaceStart'BuildingSurface:Detailed,)";
                Regex surfaceYes = new Regex(startSurface);
                Regex peopleStart = new Regex(EPlusObjects.EPlusRegexString.people);
                Regex lightStart = new Regex(EPlusObjects.EPlusRegexString.lights);
                Regex zoneStart = new Regex(EPlusObjects.EPlusRegexString.ADzoneStart);
                Regex equipStart = new Regex(EPlusObjects.EPlusRegexString.equipment);
                string semicolon = @"(\S*)(;)(.*)";
                Regex smicln = new Regex(semicolon);
                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedsurface = false;
                    bool lightsFound = false;
                    bool equipFound = false;
                    bool peopleFound = false;
                    bool zoneFound = false;
                    //set up the surface
                    ModelingUtilities.BuildingObjects.Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
                    currentSurface.SurfaceCoords = new List<Vector.CartCoord>();
                    currentSurface.surfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
                    currentSurface.outsideBoundary = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
                    int surfcount = 0;
                    int linecount = 0;
                    List<int> linelist = new List<int>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        linecount++;
                        #region
                        MatchCollection surfaceStart = surfaceYes.Matches(line);
                        if (surfaceStart.Count > 0)
                        {
                            detailedsurface = true;
                            currentSurface.Clear();
                            stuff.Add(line);
                            output.AppendLine(line);
                            linelist.Add(linecount);
                            logline.Append(linecount.ToString() + Environment.NewLine);
                            continue;
                        }
                        Match eplight = lightStart.Match(line);
                        if (eplight.Success)
                        {
                            lightsFound = true;
                            continue;
                        }
                        Match epEquip = equipStart.Match(line);
                        if (epEquip.Success)
                        {
                            equipFound = true;
                            continue;
                        }
                        Match epPeople = peopleStart.Match(line);
                        if (epPeople.Success)
                        {
                            peopleFound = true;
                            continue;
                        }
                        Match zoneM = zoneStart.Match(line);
                        if (zoneM.Success)
                        {
                            zoneFound = true;
                            continue;
                        }
                        if (zoneFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                zoneFound = false;
                                //do stuff
                                EPlusObjects.Zone zoneInst = ADepZoneToObj(stuff);
                                if (zoneInst.name == "" || zoneInst.name == null)
                                {
                                    //this is bad
                                }
                                else
                                {
                                    string zoneName = zoneInst.name;
                                    
                                    if (projectSpaces.Count == 0)
                                    {
                                        string tagline = "First project zone detected.";
                                        //logline.AppendLine(line);

                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(tagline);
                                        projectSpaces.Add(zoneInst);
                                    }
                                    else
                                    {
                                        //search for the space name in the existing List of Spaces
                                        bool spacefound = false;
                                        for (int i = 0; i < projectSpaces.Count; i++)
                                        {
                                            string projname = projectSpaces[i].name;
                                            if (projname == zoneName)
                                            {
                                                string tagline = "Existing project zone detected.";
                                                //logline.AppendLine(tagline);
                                                Console.WriteLine(tagline);
                                                //logline.AppendLine(zoneName + ": " + surfaceName);
                                                Console.WriteLine(zoneName + ":");
                                                projectSpaces[i].multiplier = zoneInst.multiplier;
                                                projectSpaces[i].volume = zoneInst.volume;
                                                projectSpaces[i].area = zoneInst.area;
                                                spacefound = true;
                                                break;
                                            }
                                        }
                                        //if spacefound is never set to true
                                        if (!spacefound)
                                        {
                                            string tagline = "New project zone detected.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": ");
                                            projectSpaces.Add(zoneInst);
                                        }
                                    }
                                }
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        if (lightsFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                lightsFound = false;
                                //do stuff
                                EPlusObjects.Lighting lightInst = ADeplightToObj(stuff);
                                //if it returns nothing...vs if it returns something
                                if (lightInst.name == "" || lightInst.name == null)
                                {
                                    //something is wrong
                                }
                                else
                                {
                                    if (projectSpaces.Count == 0)
                                    {
                                        string tagline = "First project zone detected in Surfaces.";
                                        //logline.AppendLine(line);
                                        string zoneName = lightInst.zoneName;
                                        string lightingName = lightInst.name;
                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(tagline);
                                        Console.WriteLine(zoneName + ": " + lightingName);
                                        EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                        spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                        spaceInstance.name = lightInst.zoneName;
                                        spaceInstance.lightDef = lightInst;
                                        projectSpaces.Add(spaceInstance);
                                    }
                                    else
                                    {
                                        //search for the space name in the existing List of Spaces
                                        bool spacefound = false;
                                        for (int i = 0; i < projectSpaces.Count; i++)
                                        {
                                            string projname = projectSpaces[i].name;
                                            if (projname == lightInst.zoneName)
                                            {
                                                string tagline = "Existing project zone detected.";
                                                //logline.AppendLine(tagline);
                                                Console.WriteLine(tagline);
                                                string zoneName = lightInst.zoneName;
                                                string lightName = lightInst.name;
                                                //logline.AppendLine(zoneName + ": " + surfaceName);
                                                Console.WriteLine(zoneName + ": " + lightName);
                                                projectSpaces[i].lightDef = lightInst;
                                                spacefound = true;
                                                break;
                                            }
                                        }
                                        //if spacefound is never set to true
                                        if (!spacefound)
                                        {
                                            string tagline = "New project zone detected.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            string zoneName = lightInst.zoneName;
                                            string lightName = lightInst.name;
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": " + lightName);
                                            EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                            spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                            spaceInstance.name = lightInst.zoneName;
                                            spaceInstance.lightDef = lightInst;
                                            projectSpaces.Add(spaceInstance);
                                        }
                                    }
                                }
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        if (equipFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                equipFound = false;
                                //do stuff
                                EPlusObjects.Equipment equipInst = ADepEquipToObj(stuff);
                                if (equipInst.name == "" || equipInst.name == null)
                                {
                                    //something wrong and don't add
                                }
                                else
                                {
                                    if (projectSpaces.Count == 0)
                                    {
                                        string tagline = "First project zone detected.";
                                        //logline.AppendLine(line);
                                        string zoneName = equipInst.zoneName;
                                        string equipName = equipInst.name;
                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(tagline);
                                        Console.WriteLine(zoneName + ": " + equipName);
                                        EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                        spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                        spaceInstance.name = equipInst.zoneName;
                                        spaceInstance.equipDef = equipInst;
                                        projectSpaces.Add(spaceInstance);
                                    }
                                    else
                                    {
                                        //search for the space name in the existing List of Spaces
                                        bool spacefound = false;
                                        for (int i = 0; i < projectSpaces.Count; i++)
                                        {
                                            string projname = projectSpaces[i].name;
                                            if (projname == equipInst.zoneName)
                                            {
                                                string tagline = "Existing project zone detected.";
                                                //logline.AppendLine(tagline);
                                                Console.WriteLine(tagline);
                                                string zoneName = equipInst.zoneName;
                                                string equipName = equipInst.name;
                                                //logline.AppendLine(zoneName + ": " + surfaceName);
                                                Console.WriteLine(zoneName + ": " + equipName);
                                                projectSpaces[i].equipDef = equipInst;
                                                spacefound = true;
                                                break;
                                            }
                                        }
                                        //if spacefound is never set to true
                                        if (!spacefound)
                                        {
                                            string tagline = "New project zone detected.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            string zoneName = equipInst.zoneName;
                                            string equipName = equipInst.name;
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": " + equipName);
                                            EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                            spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                            spaceInstance.name = equipInst.zoneName;
                                            spaceInstance.equipDef = equipInst;
                                            projectSpaces.Add(spaceInstance);
                                        }
                                    }
                                }
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        if (peopleFound)
                        {
                            stuff.Add(line);
                            output.Append(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                peopleFound = false;
                                //do stuff
                                EPlusObjects.People peepInst = ADepPeopleToObj(stuff);
                                if (peepInst.name == "" || peepInst.name == null)
                                {
                                    //this should not happen
                                }
                                else
                                {
                                    string zoneName = peepInst.zoneName;
                                    string peepName = peepInst.name;
                                    if (projectSpaces.Count == 0)
                                    {
                                        string tagline = "First project zone detected.";
                                        //logline.AppendLine(line);

                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(tagline);
                                        Console.WriteLine(zoneName + ": " + peepName);
                                        EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                        spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                        spaceInstance.name = zoneName;
                                        spaceInstance.peopleDef = peepInst;
                                        projectSpaces.Add(spaceInstance);
                                    }
                                    else
                                    {
                                        //search for the space name in the existing List of Spaces
                                        bool spacefound = false;
                                        for (int i = 0; i < projectSpaces.Count; i++)
                                        {
                                            string projname = projectSpaces[i].name;
                                            if (projname == zoneName)
                                            {
                                                string tagline = "Existing project zone detected.";
                                                //logline.AppendLine(tagline);
                                                Console.WriteLine(tagline);
                                                //logline.AppendLine(zoneName + ": " + surfaceName);
                                                Console.WriteLine(zoneName + ": " + peepName);
                                                projectSpaces[i].peopleDef = peepInst;
                                                spacefound = true;
                                                break;
                                            }
                                        }
                                        //if spacefound is never set to true
                                        if (!spacefound)
                                        {
                                            string tagline = "New project zone detected.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": " + peepName);
                                            EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                            spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                            spaceInstance.name = zoneName;
                                            spaceInstance.peopleDef = peepInst;
                                            projectSpaces.Add(spaceInstance);
                                        }
                                    }
                                }
                                stuff.Clear();
                                output.Clear();
                            }
                        }
                        //now that a surface element is established in the IDF, we can work through it to create surface objects
                        if (detailedsurface == true)
                        {
                            //GetAllDetailedSurfaces
                            //use streamswriter to make a little text file that will then be turned into an object
                            //write to the small output stream until you encounter a semi-colon
                            stuff.Add(line);
                            output.AppendLine(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedsurface = false;
                                string pass = output.ToString();
                                //write the output file
                                //send the output file to a function, returning a surface
                                ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.ADEPlusSurfacetoObject(stuff);
                                //add a multiplier to the surface if needed
                                foreach (EPlusObjects.ZoneGroup zoneGroup in zoneGroups)
                                {
                                    string zoneListName = zoneGroup.zoneListName;
                                    foreach (EPlusObjects.ZoneList zoneList in zoneLists)
                                    {
                                        if (zoneList.name == zoneListName)
                                        {
                                            foreach (string zoneName in zoneList.zoneListNames)
                                            {
                                                if (surfaceReturned.zoneName == zoneName)
                                                {
                                                    //add a multiplier to the surface
                                                    surfaceReturned.multiplier = zoneGroup.zoneListMultiplier;
                                                }
                                            }
                                        }
                                    }
                                }
                                surfcount++;
                                countline.Append(surfcount.ToString() + Environment.NewLine);
                                //ModelingUtilities.BuildingObjects.Surface surfaceReturned = EPlusFunctions.EPlusSurfacetoObject("C:\\Temp\\detailedSurface.txt");                                
                                output.Clear();
                                stuff.Clear();
                                if (projectSpaces.Count == 0)
                                {
                                    string tagline = "First project zone detected in Surfaces.";
                                    //logline.AppendLine(line);
                                    string zoneName = surfaceReturned.zoneName;
                                    string surfaceName = surfaceReturned.name;
                                    //logline.AppendLine(zoneName + ": " + surfaceName);
                                    Console.WriteLine(tagline);
                                    Console.WriteLine(zoneName + ": " + surfaceName);
                                    EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                    spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                    spaceInstance.name = surfaceReturned.zoneName;
                                    spaceInstance.spaceSurfaces.Add(surfaceReturned);
                                    projectSpaces.Add(spaceInstance);
                                }
                                else
                                {
                                    //search for the space name in the existing List of Spaces
                                    bool spacefound = false;
                                    for (int i = 0; i < projectSpaces.Count; i++)
                                    {
                                        string projname = projectSpaces[i].name;
                                        if (projname == surfaceReturned.zoneName)
                                        {
                                            string tagline = "Existing project zone detected in Surfaces.";
                                            //logline.AppendLine(tagline);
                                            Console.WriteLine(tagline);
                                            string zoneName = surfaceReturned.zoneName;
                                            string surfaceName = surfaceReturned.name;
                                            //logline.AppendLine(zoneName + ": " + surfaceName);
                                            Console.WriteLine(zoneName + ": " + surfaceName);
                                            projectSpaces[i].spaceSurfaces.Add(surfaceReturned);
                                            spacefound = true;
                                            break;
                                        }
                                    }
                                    //if spacefound is never set to true
                                    if (!spacefound)
                                    {
                                        string tagline = "New project zone detected in Surfaces.";
                                        //logline.AppendLine(tagline);
                                        Console.WriteLine(tagline);
                                        string zoneName = surfaceReturned.zoneName;
                                        string surfaceName = surfaceReturned.name;
                                        //logline.AppendLine(zoneName + ": " + surfaceName);
                                        Console.WriteLine(zoneName + ": " + surfaceName);
                                        EPlusObjects.Zone spaceInstance = new EPlusObjects.Zone(zoneName);
                                        spaceInstance.spaceSurfaces = new List<ModelingUtilities.BuildingObjects.Surface>();
                                        spaceInstance.name = surfaceReturned.zoneName;
                                        spaceInstance.spaceSurfaces.Add(surfaceReturned);
                                        projectSpaces.Add(spaceInstance);
                                    }

                                }
                                //semicolon match = true
                            }
                            //detailed surfaces = true
                        }
                        //while line reader is reading
                    }
                    //convert these spaces to memory safe spaces

                    foreach (EPlusObjects.Zone space in projectSpaces)
                    {
                        string zoneName = space.name;
                        int multiplier = space.multiplier;
                        foreach (ModelingUtilities.BuildingObjects.Surface surface in space.spaceSurfaces)
                        {
                            ModelingUtilities.BuildingObjects.MemorySafe_Surface memsurf = ModelingUtilities.BuildingObjects.convert2MemorySafeSurface(surface);
                            space.msspaceSurfaces.Add(memsurf);
                        }
                        space.spaceSurfaces.Clear(); //now memory safe, don't need anymore

                    }
                    //streamreader
                    reader.Close();
                    using (StreamWriter writer = new StreamWriter(linelog, false, encoding))
                    {
                        writer.Write(logline.ToString());
                    }
                    using (StreamWriter countwrite = new StreamWriter(surfacelog, false, encoding))
                    {
                        countwrite.Write(countline.ToString());
                    }
                }

                        #endregion


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return projectSpaces;
        }

        private static EPlusObjects.Zone ADepZoneToObj(List<string> stuff)
        {
            Regex zoneName = new Regex(EPlusObjects.EPlusRegexString.ADzoneName);
            Regex zoneMult = new Regex(EPlusObjects.EPlusRegexString.ADzoneMultiplier);
            Regex zoneArea = new Regex(EPlusObjects.EPlusRegexString.ADzoneArea);
            Regex zoneVol = new Regex(EPlusObjects.EPlusRegexString.ADzoneVol);

            EPlusObjects.Zone zoneInst = new EPlusObjects.Zone("tempName");
            foreach (string line in stuff)
            {
                Match nameM = zoneName.Match(line);
                if (nameM.Success)
                {
                    zoneInst.name = Purify(",", nameM.Groups["1"].Value);
                    continue;
                }
                Match multM = zoneMult.Match(line);
                if (multM.Success)
                {
                    zoneInst.multiplier = Convert.ToInt16(Purify(",",multM.Groups["1"].Value));
                    continue;
                }
                Match volM = zoneVol.Match(line);
                if (volM.Success)
                {
                    zoneInst.volume = Convert.ToDecimal(Purify(",", volM.Groups["1"].Value));
                    continue;
                }
                Match areaM = zoneArea.Match(line);
                if (areaM.Success)
                {
                    zoneInst.area = Convert.ToDecimal(Purify(";",areaM.Groups["1"].Value));
                    continue;
                }
            }
            return zoneInst;
        }

        private static EPlusObjects.People ADepPeopleToObj(List<string> stuff)
        {
            Regex groupName = new Regex(EPlusObjects.EPlusRegexString.ADpeopleGroupName);
            Regex zoneName = new Regex(EPlusObjects.EPlusRegexString.ADpeopleZoneName);
            Regex numPeopSch = new Regex(EPlusObjects.EPlusRegexString.ADnumPeopleSch);
            Regex numPeople = new Regex(EPlusObjects.EPlusRegexString.ADnumPeople);
            Regex peoplePerArea = new Regex(EPlusObjects.EPlusRegexString.ADpeoplePerArea);
            Regex areaPerPerson = new Regex(EPlusObjects.EPlusRegexString.ADareaPerPerson);
            Regex fracRadiant = new Regex(EPlusObjects.EPlusRegexString.ADpeoplefracRadiant);
            Regex fracSensible = new Regex(EPlusObjects.EPlusRegexString.ADsensibleFrac);
            Regex activityLevelSched = new Regex(EPlusObjects.EPlusRegexString.ADactivityLevelSched);

            EPlusObjects.People peopleinst = new EPlusObjects.People();
            foreach (string line in stuff)
            {
                Match gnameM = groupName.Match(line);
                if (gnameM.Success)
                {
                    peopleinst.name = Purify(",", gnameM.Groups["1"].Value);
                    continue;
                }
                Match zoneNmM = zoneName.Match(line);
                if (zoneNmM.Success)
                {
                    peopleinst.zoneName = Purify(",", zoneNmM.Groups["1"].Value);
                    continue;
                }
                Match numPeopSchM = numPeopSch.Match(line);
                if (numPeopSchM.Success)
                {
                    if (Purify(",", numPeopSchM.Groups["1"].Value) != "")
                    {
                        peopleinst.peopleNumSch = Purify(",", numPeopSchM.Groups["1"].Value);
                    }
                    continue;
                }
                Match numPeopM = numPeople.Match(line);
                if (numPeopM.Success)
                {
                    if (Purify(",", numPeopM.Groups["1"].Value) != "")
                    {
                        peopleinst.numPeople = Convert.ToDecimal(Purify(",", numPeopM.Groups["1"].Value));
                    }
                    continue;
                }
                Match peopPerAreaM = peoplePerArea.Match(line);
                if (peopPerAreaM.Success)
                {
                    if (Purify(",", peopPerAreaM.Groups["1"].Value) != "")
                    {
                        peopleinst.peoplePerArea = Convert.ToDecimal(Purify(",", peopPerAreaM.Groups["1"].Value));
                    }
                    continue;
                }
                Match areaPerPerM = areaPerPerson.Match(line);
                if (areaPerPerM.Success)
                {
                    if (Purify(",", areaPerPerM.Groups["1"].Value) != "")
                    {
                        peopleinst.areaPerPerson = Convert.ToDecimal(Purify(",", areaPerPerM.Groups["1"].Value));
                    }
                    continue;
                }
                Match fracRadM = fracRadiant.Match(line);
                if (fracRadM.Success)
                {
                    peopleinst.fractionRadiant = Convert.ToDecimal(Purify(",", fracRadM.Groups["1"].Value));
                    continue;
                }
                Match fracSensM = fracSensible.Match(line);
                if (fracSensM.Success)
                {
                    peopleinst.fractionSensible = Convert.ToDecimal(Purify(",", fracSensM.Groups["1"].Value));
                    continue;
                }
                Match actLevel = activityLevelSched.Match(line);
                if (actLevel.Success)
                {
                    peopleinst.peopleHGSch = Purify(";", actLevel.Groups["1"].Value);
                    continue;
                }
            }
            return peopleinst;
            
        }

        private static EPlusObjects.Equipment ADepEquipToObj(List<string> stuff)
        {

            Regex equipName = new Regex(EPlusObjects.EPlusRegexString.ADintGainName);
            Regex eZoneName = new Regex(EPlusObjects.EPlusRegexString.ADintGainZoneName);
            Regex eSched = new Regex(EPlusObjects.EPlusRegexString.ADSchedule);
            Regex ePower = new Regex(EPlusObjects.EPlusRegexString.ADPower);
            Regex ePowerPerArea = new Regex(EPlusObjects.EPlusRegexString.ADPowerPerArea);
            Regex ePowerPerPerson = new Regex(EPlusObjects.EPlusRegexString.ADAreaPerPerson);
            Regex fractionLatent = new Regex(EPlusObjects.EPlusRegexString.ADfractionLatent);
            Regex fracRadiant = new Regex(EPlusObjects.EPlusRegexString.ADfractionRadiant);

            EPlusObjects.Equipment einst = new EPlusObjects.Equipment();
            foreach (string line in stuff)
            {
                Match name = equipName.Match(line);
                if (name.Success)
                {
                    einst.name = Purify(",", name.Groups["1"].Value);
                    continue;
                }
                Match zonenm = eZoneName.Match(line);
                if (zonenm.Success)
                {
                    einst.zoneName = Purify(",", zonenm.Groups["1"].Value);
                    continue;
                }
                Match eschedMatch = eSched.Match(line);
                if (eschedMatch.Success)
                {
                    einst.equipSch = Purify(",", eschedMatch.Groups["1"].Value);
                    continue;
                }
                Match ePowerMatch = ePower.Match(line);
                if (ePowerMatch.Success)
                {
                    if (Purify(",", ePowerMatch.Groups["1"].Value) != "")
                    {
                        einst.equipPower = Convert.ToDecimal(Purify(",", ePowerMatch.Groups["1"].Value));
                    }
                    continue;
                }
                Match ePowerPerAreaMatch = ePowerPerArea.Match(line);
                if (ePowerPerAreaMatch.Success)
                {
                    if (Purify(",", ePowerPerAreaMatch.Groups["1"].Value) != "")
                    {
                        einst.equipPowerPerArea = Convert.ToDecimal(Purify(",", ePowerPerAreaMatch.Groups["1"].Value));
                    }
                    continue;
                }
                Match ePerPerson = ePowerPerPerson.Match(line);
                if (ePerPerson.Success)
                {
                    if (Purify(",", ePerPerson.Groups["1"].Value) != "")
                    {
                        einst.equipPowerPerPerson = Convert.ToDecimal(Purify(",", ePerPerson.Groups["1"].Value));
                    }
                    continue;
                }
                Match eFracL = fractionLatent.Match(line);
                if (eFracL.Success)
                {
                    if (Purify(",", eFracL.Groups["1"].Value) != "")
                    {
                        einst.fractionLatent = Convert.ToDecimal(Purify(",", eFracL.Groups["1"].Value));
                    }
                    continue;
                }
                Match efracr = fracRadiant.Match(line);
                if (efracr.Success)
                {
                    if (Purify(",", efracr.Groups["1"].Value) != "")
                    {
                        einst.fractionRadiant = Convert.ToDecimal(Purify(",", efracr.Groups["1"].Value));
                    }
                    continue;
                }
            }
            return einst;
        }

        private static EPlusObjects.Lighting ADeplightToObj(List<string> stuff)
        {
            //needed regex-es
            Regex lightsName = new Regex(EPlusObjects.EPlusRegexString.ADintGainName);
            Regex lightsZone = new Regex(EPlusObjects.EPlusRegexString.ADintGainZoneName);
            Regex lightsSchedule = new Regex(EPlusObjects.EPlusRegexString.ADSchedule);
            Regex lightsDesignPower = new Regex(EPlusObjects.EPlusRegexString.ADPower);
            Regex lightsPowerPerArea = new Regex(EPlusObjects.EPlusRegexString.ADPowerPerArea);
            Regex lightsAreaPerPerson = new Regex(EPlusObjects.EPlusRegexString.ADAreaPerPerson);
            Regex lightsFracRadiant = new Regex(EPlusObjects.EPlusRegexString.ADfractionRadiant);
            Regex lightsFracVis = new Regex(EPlusObjects.EPlusRegexString.ADfractionVisible);

            EPlusObjects.Lighting lightinst = new EPlusObjects.Lighting();
            foreach (string line in stuff)
            {
                Match name = lightsName.Match(line);
                if (name.Success)
                {
                    lightinst.name = Purify(",", name.Groups["1"].Value);
                    continue;
                }
                Match zoneName = lightsZone.Match(line);
                if (zoneName.Success)
                {
                    lightinst.zoneName = Purify(",", zoneName.Groups["1"].Value);
                    continue;
                }
                Match lightSched = lightsSchedule.Match(line);
                if (lightSched.Success)
                {
                    lightinst.lightSch = Purify(",", lightSched.Groups["1"].Value);
                    continue;
                }
                Match lightPow = lightsDesignPower.Match(line);
                if (lightPow.Success)
                {
                    string ret = Purify(",", lightPow.Groups["1"].Value);
                    if (ret != "")
                    {
                        if (ret.ToLower() != "lightinglevel")
                        {
                            lightinst.lightPower = Convert.ToDecimal(Purify(",", lightPow.Groups["1"].Value));
                        }
                    }
                    continue;
                }
                Match wattPerArea = lightsPowerPerArea.Match(line);
                if (wattPerArea.Success)
                {
                    if (Purify(",", wattPerArea.Groups["1"].Value) != "")
                    {
                        lightinst.lightPowerPerArea = Convert.ToDecimal(Purify(",", wattPerArea.Groups["1"].Value));
                    }
                    continue;
                }
                Match wattPerson = lightsAreaPerPerson.Match(line);
                if (wattPerson.Success)
                {
                    if (Purify(",", wattPerson.Groups["1"].Value) != "")
                    {
                        lightinst.lightPowerPerPerson = Convert.ToDecimal(Purify(",", wattPerson.Groups["1"].Value));
                    }
                    continue;
                }
                Match fracRad = lightsFracRadiant.Match(line);
                if (fracRad.Success)
                {
                    if (Purify(",", fracRad.Groups["1"].Value) != "")
                    {
                        lightinst.lightFractionRadiant = Convert.ToDecimal(Purify(",", fracRad.Groups["1"].Value));
                    }
                    continue;
                }
                Match fracVis = lightsFracVis.Match(line);
                if (fracVis.Success)
                {
                    if (Purify(",", fracVis.Groups["1"].Value) != "")
                    {
                        lightinst.lightFractionVisible = Convert.ToDecimal(Purify(",", fracVis.Groups["1"].Value));
                    }
                    continue;
                }

            }
            return lightinst;
        }

        private static string Purify(string ending, string line)
        {
            string ret = "";
            if (ending == ",")
            {
                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                Regex purifyRegex = new Regex(purify);
                Match pure = purifyRegex.Match(line);
                if (pure.Success)
                {
                    ret = pure.Groups["goods"].Value;
                }
            }
            else if (ending == ";")
            {
                string purify = @"(?'ws'\s*)(?'goods'.*)(?'semi';)";
                Regex purifyRegex = new Regex(purify);
                Match pure = purifyRegex.Match(line);
                if (pure.Success)
                {
                    ret = pure.Groups["goods"].Value;
                }
            }
            return ret;
        }

        private static EPlusObjects.ZoneGroup MakeZoneGroup(List<string> zoneGroupStrings)
        {
            EPlusObjects.ZoneGroup newZoneGroup = new EPlusObjects.ZoneGroup();

            Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            Regex nameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex zoneListNameRegex = new Regex(EPlusObjects.EPlusRegexString.zoneListName);
            Regex zoneListMultiplierRegex = new Regex(EPlusObjects.EPlusRegexString.zoneListMultiplier);
            try
            {
                foreach (string line in zoneGroupStrings)
                {
                    Match nameMatch = nameRegex.Match(line);
                    if (nameMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(nameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            newZoneGroup.name = pure.Groups["goods"].Value;
                            continue;
                        }
                    }
                    //match the Zone List Name
                    Match zoneListNameMatch = zoneListNameRegex.Match(line);
                    if (zoneListNameMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(zoneListNameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            newZoneGroup.zoneListName = pure.Groups["goods"].Value;
                            continue;
                        }
                    }
                    //match the Zone Multiplier
                    Match zoneListMultiplierMatch = zoneListMultiplierRegex.Match(line);
                    if (zoneListMultiplierMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(zoneListMultiplierMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            newZoneGroup.zoneListMultiplier = Convert.ToInt32(pure.Groups["goods"].Value);
                            continue;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return newZoneGroup;
        }

        private static EPlusObjects.ZoneList MakeZoneList(List<string> zoneListStrings)
        {
            EPlusObjects.ZoneList newZoneList = new EPlusObjects.ZoneList();
            newZoneList.zoneListNames = new List<string>();
            Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
            Regex nameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex zoneListStartRegex = new Regex(EPlusObjects.EPlusRegexString.startZoneList);

            try
            {
                foreach (string line in zoneListStrings)
                {
                    Match semiColonMatch = semiColonRegex.Match(line);
                    if (!semiColonMatch.Success)
                    {
                        Match zoneListStart = zoneListStartRegex.Match(line);
                        if (zoneListStart.Success)
                        {
                            //do not need to do anything.
                            continue;
                        }
                        Match nameMatch = nameRegex.Match(line);
                        if (nameMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(nameMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                newZoneList.name = pure.Groups["goods"].Value;
                                continue;
                            }
                        }
                        //match any generic line that does not have a semicolon in it
                        //dangerous as this will greedily match just about anything
                        string purifyList = @"(?'ws'\s*)(?'goods'.*)(?'comma',)(?'stuff'.*)";
                        Regex purifyRegex2 = new Regex(purifyList);
                        Match pureList = purifyRegex2.Match(line);
                        if (pureList.Success)
                        {
                            string zoneName = pureList.Groups["goods"].Value;
                            newZoneList.zoneListNames.Add(zoneName);
                            continue;
                        }
                    }
                    else
                    {
                        //string purification with a semicolong instead of a comma
                        string purifyList = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)(?'stuff'.*)";
                        Regex purifyRegex2 = new Regex(purifyList);
                        Match pureList = purifyRegex2.Match(line);
                        if (pureList.Success)
                        {
                            string zoneName = pureList.Groups["goods"].Value;
                            newZoneList.zoneListNames.Add(zoneName);
                            continue;
                        }

                    }
                }
                return newZoneList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return newZoneList;
        }

        public static ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions EPlusWindowstoObject(string idfSnippet)
        {


            //make a window
            ModelingUtilities.BuildingObjects.OpeningDefinitions window = new ModelingUtilities.BuildingObjects.OpeningDefinitions();
            window.coordinateList = new List<Vector.CartCoord>();
            try
            {
                //logfile if appropriate

                #region
                //write the regexes
                //Regex for beginning of a detailed fenestration element,
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                //Regex for fenestrationName
                Regex fenNameRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationName);
                //Regex for fenestration Type
                Regex fenTypeRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationType);
                //Regex for fenestration ConstructionName
                Regex fenConstructionNameRegex = new Regex(EPlusObjects.EPlusRegexString.fenConstructionName);
                //Regex for parent Surface Name
                Regex parentSurfaceNameRegex = new Regex(EPlusObjects.EPlusRegexString.parentSurfaceName);
                //Regex for outsideBoundary Condition
                Regex outsideBoundaryConditionRegex = new Regex(EPlusObjects.EPlusRegexString.outsideBoundaryCondition);
                //Regex for View Factor to Ground
                Regex viewFactor2GroundRegex = new Regex(EPlusObjects.EPlusRegexString.viewFactor2Ground);
                //Regex for shade control name
                Regex shadeControlNameRegex = new Regex(EPlusObjects.EPlusRegexString.shadeControlName);
                //Regex for frame and divider name
                Regex frameAndDividerNameRegex = new Regex(EPlusObjects.EPlusRegexString.frameAndDividerName);
                //Regex for multiplier
                Regex multiplierRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationMultiplier);
                //Regex for numberofVertices
                Regex numberofVerticesRegex = new Regex(EPlusObjects.EPlusRegexString.numberofFenestrationVertices);
                //something else for the vertex
                //Regex for generic Vertex
                string typicalVertex = @"(?'1'.*)(?'vertices'!- X,Y,Z)";
                Regex typicalVertexRegex = new Regex(typicalVertex);

                //a regex for finding a semicolon
                string semicolon = @"(\S*)(;)(.*)";
                Regex smicln = new Regex(semicolon);
                #endregion

                //use streamreader to read through the list
                //special needed to allow the routine to run successfully
                //needed because the regex may return true in the wrong instance
                bool firstLineofFenestration = false;
                bool semicolonfound = false;
                Encoding encoding;

                using (StreamReader reader = new StreamReader(idfSnippet))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            firstLineofFenestration = true;

                            continue;
                        }
                        //now that a fenestration element is established in the IDF, we can work through it to create surface objects
                        if (detailedFenestration == true)
                        {
                            if (!vertexMatching)
                            {
                                Match fenNameMatch = fenNameRegex.Match(line);
                                if (fenNameMatch.Success)
                                {
                                    firstLineofFenestration = false;
                                    //strip off the whitespace and comma
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(fenNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.nameId = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }
                                //Match SurfaceType
                                Match fenTypeMatch = fenTypeRegex.Match(line);
                                if (fenTypeMatch.Success)
                                {
                                    //strip off the whitespace and comma
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(fenTypeMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.openingType = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get Construction Type
                                Match constructionTypeMatch = fenConstructionNameRegex.Match(line);
                                if (constructionTypeMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(constructionTypeMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.constructionName = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }

                                //Get the Parent Surface name
                                Match parentSurfaceNameMatch = parentSurfaceNameRegex.Match(line);
                                if (parentSurfaceNameMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(parentSurfaceNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.parentSurfaceNameId = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }

                                //Get Outside Boundary Condition Object
                                Match outsideBoundaryConditionMatch = outsideBoundaryConditionRegex.Match(line);
                                if (outsideBoundaryConditionMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(outsideBoundaryConditionMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.outsideBoundaryConditionObj = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get View Factor
                                Match viewFactorMatch = viewFactor2GroundRegex.Match(line);
                                if (viewFactorMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(viewFactorMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.viewFactortoGround = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }

                                //Get ShadeControl Name
                                Match shadeControlNameMatch = shadeControlNameRegex.Match(line);
                                if (shadeControlNameMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(shadeControlNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.shadeControlSch = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get Frameand Divider Name
                                Match frameAndDividerMatch = frameAndDividerNameRegex.Match(line);
                                if (frameAndDividerMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(frameAndDividerMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.frameAndDividerName = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get multiplier
                                Match multiplierMatch = multiplierRegex.Match(line);
                                if (multiplierMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(multiplierMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.multiplier = Convert.ToInt32(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //Number of Vertices
                                Match numVerticesMatch = numberofVerticesRegex.Match(line);
                                if (numVerticesMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(numVerticesMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.numVertices = Convert.ToInt32(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                            }

                            //Get Vertices
                            //loop through them until the end


                            Match vertexMatch = typicalVertexRegex.Match(line);
                            if (vertexMatch.Success)
                            {
                                vertexMatching = true;
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma')";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(vertexMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    //extract the X,Y, and Z coordinate from the purified string
                                    string coordinateString = @"(?'X'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Y'[-+]?([0-9]*\.[0-9]+|[0-9]+)),(?'Z'[-+]?([0-9]*\.[0-9]+|[0-9]+))";
                                    Regex coordRegex = new Regex(coordinateString);
                                    Match XYZMatch = coordRegex.Match(pure.Groups["goods"].Value);
                                    if (XYZMatch.Success)
                                    {
                                        Vector.CartCoord Coord = new Vector.CartCoord();
                                        Coord.X = Convert.ToDouble(XYZMatch.Groups["X"].Value);
                                        Coord.Y = Convert.ToDouble(XYZMatch.Groups["Y"].Value);
                                        Coord.Z = Convert.ToDouble(XYZMatch.Groups["Z"].Value);
                                        window.coordinateList.Add(Coord);
                                    }
                                }
                            }

                            //see if there is a semi-colon
                            Match smicolonMatch = smicln.Match(line);
                            if (smicolonMatch.Success)
                            {
                                semicolonfound = true;
                                vertexMatching = false;
                                detailedFenestration = false;
                                string tagline = "Window match found for window: " + window.nameId;
                                Console.WriteLine(tagline);
                            }
                        }

                    }
                    reader.Close();

                    //get the RHR of the coordinates
                    Vector.CartVect RHRNormalVector = Vector.GetRHR(window.coordinateList);
                    window.rHRVector = RHRNormalVector;
                    window.Azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
                    Vector.MemorySafe_CartVect memSafeRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
                    
                    window.Tilt = EPlusFunctions.FindTilt(memSafeRHR);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions memOpening = ModelingUtilities.BuildingObjects.convert2MemorySafeOpening(window);
            return memOpening;
        }

        public static ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions ADEPlusWindowstoObject(string idfSnippet)
        {


            //make a window
            ModelingUtilities.BuildingObjects.OpeningDefinitions window = new ModelingUtilities.BuildingObjects.OpeningDefinitions();
            window.coordinateList = new List<Vector.CartCoord>();
            try
            {
                //logfile if appropriate
                //public static string ADwindowName = @"(?'1'.*)(?'Name'!-Opening Name)";
                //public static string ADwindowconst = @"(?'1'.*)(?'Construction'!-Class and Construction Name)";
                //public static string ADwindowparent = @"(?'1'.*)(?'Parent'!-Name of Parent Surface)";
                //public static string ADwindowshade = @"(?'1'.*)(?'Shade'!-Shading Control)";
                //public static string ADwindowframe = @"(?'1'.*)(?'Frame'!-Frame)";
                //public static string ADwindowmultiplier = @"(?'1'.*)(?'Multiplier'!-Multiplier)";
                //public static string ADwindowX = @"(?'1'.*)(?'X'!-x coord.)";
                //public static string ADwindowZ = @"(?'1'.*)(?'Z'!-z coord.)";
                //public static string ADwindowlength = @"(?'1'.*)(?'Length'!-Length)";
                //public static string ADwindowwidth = @"(?'1'.*)(?'Width'!-Width)";

                #region
                //write the regexes
                //Regex for beginning of a detailed fenestration element,
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.ADstartFenestration);
                //Regex for fenestrationName
                Regex fenNameRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowName);
                //Regex for fenestration Type
                Regex fenTypeRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationType); //remove
                //Regex for fenestration ConstructionName
                Regex fenConstructionNameRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowconst);
                //Regex for parent Surface Name
                Regex parentSurfaceNameRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowparent);
                //Regex for outsideBoundary Condition
                Regex outsideBoundaryConditionRegex = new Regex(EPlusObjects.EPlusRegexString.outsideBoundaryCondition); //remove
                //Regex for View Factor to Ground
                Regex viewFactor2GroundRegex = new Regex(EPlusObjects.EPlusRegexString.viewFactor2Ground); //remove
                //Regex for shade control name
                Regex shadeControlNameRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowshade);
                //Regex for frame and divider name
                Regex frameAndDividerNameRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowframe); 
                //Regex for multiplier
                Regex multiplierRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowmultiplier);
                //Regex for x coordinate
                Regex windowXRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowX);
                //Regex for Z coordinate
                Regex windowZRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowZ);
                //Regex for width
                Regex windowHeightRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowheight);
                //Regex for Z coordinate
                Regex windowLengthRegex = new Regex(EPlusObjects.EPlusRegexString.ADwindowlength);
                //Regex for numberofVertices
                Regex numberofVerticesRegex = new Regex(EPlusObjects.EPlusRegexString.numberofFenestrationVertices); //remove
                //something else for the vertex
                //Regex for generic Vertex
                string typicalVertex = @"(?'1'.*)(?'vertices'!- X,Y,Z)";
                Regex typicalVertexRegex = new Regex(typicalVertex);

                //a regex for finding a semicolon
                string semicolon = @"(\S*)(;)(.*)";
                Regex smicln = new Regex(semicolon);
                #endregion

                //use streamreader to read through the list
                //special needed to allow the routine to run successfully
                //needed because the regex may return true in the wrong instance
                bool firstLineofFenestration = false;
                bool semicolonfound = false;
                Encoding encoding;

                using (StreamReader reader = new StreamReader(idfSnippet))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            firstLineofFenestration = true;

                            continue;
                        }
                        //now that a fenestration element is established in the IDF, we can work through it to create surface objects
                        if (detailedFenestration == true)
                        {
                            if (!vertexMatching)
                            {
                                Match fenNameMatch = fenNameRegex.Match(line);
                                if (fenNameMatch.Success)
                                {
                                    firstLineofFenestration = false;
                                    //strip off the whitespace and comma
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(fenNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.nameId = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }
                                //Match SurfaceType
                                Match fenTypeMatch = fenTypeRegex.Match(line);
                                if (fenTypeMatch.Success)
                                {
                                    //strip off the whitespace and comma
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(fenTypeMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.openingType = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get Construction Type
                                Match constructionTypeMatch = fenConstructionNameRegex.Match(line);
                                if (constructionTypeMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(constructionTypeMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.constructionName = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }

                                //Get the Parent Surface name
                                Match parentSurfaceNameMatch = parentSurfaceNameRegex.Match(line);
                                if (parentSurfaceNameMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(parentSurfaceNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.parentSurfaceNameId = pure.Groups["goods"].Value;
                                        continue;
                                    }

                                }

                                //Get Outside Boundary Condition Object
                                Match outsideBoundaryConditionMatch = outsideBoundaryConditionRegex.Match(line);
                                if (outsideBoundaryConditionMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(outsideBoundaryConditionMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.outsideBoundaryConditionObj = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get View Factor
                                Match viewFactorMatch = viewFactor2GroundRegex.Match(line);
                                if (viewFactorMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(viewFactorMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.viewFactortoGround = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }

                                //Get ShadeControl Name
                                Match shadeControlNameMatch = shadeControlNameRegex.Match(line);
                                if (shadeControlNameMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(shadeControlNameMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.shadeControlSch = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get Frameand Divider Name
                                Match frameAndDividerMatch = frameAndDividerNameRegex.Match(line);
                                if (frameAndDividerMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(frameAndDividerMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.frameAndDividerName = pure.Groups["goods"].Value;
                                        continue;
                                    }
                                }

                                //Get multiplier
                                Match multiplierMatch = multiplierRegex.Match(line);
                                if (multiplierMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(multiplierMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.multiplier = Convert.ToInt32(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //Number of Vertices
                                Match numVerticesMatch = numberofVerticesRegex.Match(line);
                                if (numVerticesMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(numVerticesMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.numVertices = Convert.ToInt32(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //Lower left X
                                Match xMatch = windowXRegex.Match(line);
                                if (xMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(xMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.X = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //Lower Left z
                                Match zMatch = windowZRegex.Match(line);
                                if (zMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(zMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.Z = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //Window Length
                                Match lengthMatch = windowLengthRegex.Match(line);
                                if (lengthMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(lengthMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.length = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                                //window height
                                Match heightMatch = windowHeightRegex.Match(line);
                                if (heightMatch.Success)
                                {
                                    string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                                    Regex purifyRegex = new Regex(purify);
                                    Match pure = purifyRegex.Match(heightMatch.Groups["1"].Value);
                                    if (pure.Success)
                                    {
                                        window.height = Convert.ToDouble(pure.Groups["goods"].Value);
                                        continue;
                                    }
                                }
                            }


                            //see if there is a semi-colon
                            Match smicolonMatch = smicln.Match(line);
                            if (smicolonMatch.Success)
                            {
                                semicolonfound = true;
                                vertexMatching = false;
                                detailedFenestration = false;
                                string tagline = "Window match found for window: " + window.nameId;
                                Console.WriteLine(tagline);
                            }
                        }

                    }
                    reader.Close();

                    //get the RHR of the coordinates
                    //Vector.CartVect RHRNormalVector = Vector.GetRHR(window.coordinateList);
                    //window.rHRVector = RHRNormalVector;
                    //window.Azimuth = EPlusFunctions.FindAzimuth(RHRNormalVector);
                    //Vector.MemorySafe_CartVect memSafeRHR = Vector.convertToMemorySafeVector(RHRNormalVector);
                    
                    //window.Tilt = EPlusFunctions.FindTilt(memSafeRHR);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions memwindow = ModelingUtilities.BuildingObjects.convert2ADMemorySafeOpening(window);
            return memwindow;
        }

        public static List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> MakeEPlusWindowsList(string idfName)
        {
            List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> openings = new List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions>();

            try
            {
                //temporary output
                Encoding encoding;
                StringBuilder output = new StringBuilder();

                //Regex setup
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex smicln = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                //logfile if appropriate
                using (StreamReader reader = new StreamReader(idfName))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestration)
                        {
                            output.AppendLine(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedFenestration = false;
                                //write the output file
                                using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedSurface.txt", false, encoding))
                                {
                                    writer.Write(output.ToString());
                                    writer.Close();
                                }
                                //send the output file to a function, returning an opening
                                ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions openingReturned = EPlusFunctions.EPlusWindowstoObject("C:\\Temp\\detailedSurface.txt");
                                //destroy the temporary text file
                                output.Clear();
                                File.Delete("C:\\Temp\\detailedSurface.txt");
                                Console.WriteLine("C:\\Temp\\detailedSurface.txt deleted.");
                                Console.WriteLine("Adding an opening to the list.  Opening: " + openingReturned.nameId);
                                openings.Add(openingReturned);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return openings;
        }


        public static List<ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions> ADMakeEPlusWindowsList(string idfName)
        {
            List<ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions> openings = new List<ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions>();
            try
            {
                //temporary output
                Encoding encoding;
                StringBuilder output = new StringBuilder();

                //Regex setup
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.ADstartFenestration);
                Regex smicln = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                //logfile if appropriate
                using (StreamReader reader = new StreamReader(idfName))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestration)
                        {
                            output.AppendLine(line);
                            Match smicolMatch = smicln.Match(line);
                            if (smicolMatch.Success)
                            {
                                detailedFenestration = false;
                                //write the output file
                                using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedSurface.txt", false, encoding))
                                {
                                    writer.Write(output.ToString());
                                    writer.Close();
                                }
                                //send the output file to a function, returning an opening
                                ModelingUtilities.BuildingObjects.MemorySafe_ADOpeningDefinitions openingReturned = EPlusFunctions.ADEPlusWindowstoObject("C:\\Temp\\detailedSurface.txt");
                                //destroy the temporary text file
                                output.Clear();
                                File.Delete("C:\\Temp\\detailedSurface.txt");
                                Console.WriteLine("C:\\Temp\\detailedSurface.txt deleted.");
                                Console.WriteLine("Adding an opening to the list.  Opening: " + openingReturned.nameId);
                                openings.Add(openingReturned);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return openings;
        }

        public static Dictionary<string, double> GetHighLevelProjectWWR(List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings)
        {
            StringBuilder output = new StringBuilder();

            //make a dictionary that will store the various WWRs.  The dictionary is sort of flexible
            //String key is the level of the WWR (eg - Building-East, Building-Total,etc.)  Value is the actual WWR
            Dictionary<string, double> WWRList = new Dictionary<string, double>();
            double buildingWestExtWallArea = 0;
            double buildingWestWindowArea = 0;
            double buildingSouthExtWallArea = 0;
            double buildingSouthWindowArea = 0;
            double buildingEastExtWallArea = 0;
            double buildingEastWindowArea = 0;
            double buildingNorthExtWallArea = 0;
            double buildingNorthWindowArea = 0;

            List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> foundSurfaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
 
            //loop through the windows and get the parent name.  Figure out what direction the parent is facing. 
            for (int i = 0; i < projectOpenings.Count(); i++)
            {
                string parentSurfaceName = projectOpenings[i].parentSurfaceNameId;
                //search for parent surface
                for (int j = 0; j < projectSurfaces.Count(); j++)
                {
                    if (projectSurfaces[j].name != parentSurfaceName) continue;
                    else if (projectSurfaces[j].name == parentSurfaceName)
                    {
                        bool surfaceMatch = false;
                        //initialize found surfaces
                        if (i == 0) foundSurfaces.Add(projectSurfaces[j]);
                        double parentAzimuth = projectSurfaces[j].azimuth;
                        //bin the surface areas based on orientation
                        if ((parentAzimuth > 315 && parentAzimuth <= 360) || (parentAzimuth >= 0 && parentAzimuth < 45))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(projectSurfaces[j]);
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i]);
                            //modifications to the window and wall area due to multipliers
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                wallarea *= projectSurfaces[j].multiplier;
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            //do not want to double count wall areas
                            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in foundSurfaces)
                            {

                                if (surface.name == projectSurfaces[j].name)
                                {
                                    buildingNorthWindowArea += windowarea;
                                    if (buildingNorthExtWallArea == 0) buildingNorthExtWallArea += wallarea;
                                    surfaceMatch = true;
                                    break;
                                }
                            }
                            if (surfaceMatch == false)
                            {
                                buildingNorthExtWallArea += wallarea;
                                buildingNorthWindowArea += windowarea;
                                foundSurfaces.Add(projectSurfaces[j]);
                            }
                        }
                        else if ((parentAzimuth >= 45 && parentAzimuth < 135))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(projectSurfaces[j]);
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i]);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                wallarea *= projectSurfaces[j].multiplier;
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            //do not want to double count wall areas
                            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in foundSurfaces)
                            {

                                if (surface.name == projectSurfaces[j].name)
                                {
                                    buildingEastWindowArea += windowarea;
                                    if (buildingEastExtWallArea == 0) buildingEastExtWallArea += wallarea;
                                    surfaceMatch = true;
                                    break;
                                }

                            }
                            if (surfaceMatch == false)
                            {
                                buildingEastExtWallArea += wallarea;
                                buildingEastWindowArea += windowarea;
                                foundSurfaces.Add(projectSurfaces[j]);
                            }
                        }

                        else if ((parentAzimuth >= 135 && parentAzimuth < 225))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(projectSurfaces[j]);
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i]);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                wallarea *= projectSurfaces[j].multiplier;
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            //do not want to double count wall areas
                            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in foundSurfaces)
                            {

                                if (surface.name == projectSurfaces[j].name)
                                {
                                    buildingSouthWindowArea += windowarea;
                                    if (buildingSouthExtWallArea == 0) buildingSouthExtWallArea += wallarea;
                                    surfaceMatch = true;
                                    break;
                                }

                            }
                            if (surfaceMatch == false)
                            {
                                buildingSouthExtWallArea += wallarea;
                                buildingSouthWindowArea += windowarea;
                                foundSurfaces.Add(projectSurfaces[j]);
                            }
                        }
                        else if ((parentAzimuth >= 225 && parentAzimuth < 315))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(projectSurfaces[j]);
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i]);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                wallarea *= projectSurfaces[j].multiplier;
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            //do not want to double count wall areas
                            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in foundSurfaces)
                            {

                                if (surface.name == projectSurfaces[j].name)
                                {
                                    buildingWestWindowArea += windowarea;
                                    if (buildingWestExtWallArea == 0) buildingWestExtWallArea += wallarea;
                                    surfaceMatch = true;
                                    break;
                                }
                            }

                            if (surfaceMatch == false)
                            {
                                buildingWestExtWallArea += wallarea;
                                buildingWestWindowArea += windowarea;
                                foundSurfaces.Add(projectSurfaces[j]);
                            }
                        }
                    }
                }

            }

            
            

            string eastWWRKey = "buildingEastWWR";
            double buildingEastWWR = buildingEastWindowArea / buildingEastExtWallArea;
            string northWWRKey = "buildingNorthWWR";
            double buildingNorthWWR = buildingNorthWindowArea / buildingNorthExtWallArea;
            string southWWRKey = "buildingSouthWWR";
            double buildingSouthWWR = buildingSouthWindowArea / buildingSouthExtWallArea;
            string westWWRKey = "buildingWestWWR";
            double buildingWestWWR = buildingWestWindowArea / buildingWestExtWallArea;
            string buildingWWRKey = "buildingTotalWWR";
            double buildingWWR = (buildingEastWindowArea + buildingNorthWindowArea + buildingSouthWindowArea + buildingWestWindowArea) /
                (buildingEastExtWallArea + buildingNorthExtWallArea + buildingSouthExtWallArea + buildingWestExtWallArea);

            double buildingTotalExtWallArea = (buildingEastExtWallArea + buildingNorthExtWallArea + buildingSouthExtWallArea + buildingWestExtWallArea);
            double buildingTotalExtWindowArea = (buildingEastWindowArea + buildingNorthWindowArea + buildingSouthWindowArea + buildingWestWindowArea);
            //write to csv
            output.AppendLine("Category,Total,North,East,South,West");
            output.AppendLine("Wall Area(m2)," + buildingTotalExtWallArea.ToString() + "," + buildingNorthExtWallArea.ToString() +
                "," + buildingEastExtWallArea.ToString() + "," + buildingSouthExtWallArea.ToString() + "," + buildingWestExtWallArea.ToString());
            output.AppendLine("Window Area(m2)," + buildingTotalExtWindowArea.ToString() + "," + buildingNorthWindowArea.ToString() + ","
                + buildingEastWindowArea.ToString() + "," + buildingSouthWindowArea.ToString() + "," + buildingWestWindowArea.ToString());

            //add the values to the list
            WWRList.Add(buildingWWRKey, buildingWWR);
            WWRList.Add(northWWRKey, buildingNorthWWR);
            WWRList.Add(eastWWRKey, buildingEastWWR);
            WWRList.Add(southWWRKey, buildingSouthWWR);
            WWRList.Add(westWWRKey, buildingWestWWR);

            using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedWindow.csv", false))
            {
                writer.Write(output.ToString());
                writer.Close();
            }

            return WWRList;
        }

        public static Dictionary<string, double> AboveAndBelowGradeProjectWWR(List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings)
        {
            StringBuilder output = new StringBuilder();

            //make a dictionary that will store the various WWRs.  The dictionary is sort of flexible
            //String key is the level of the WWR (eg - Building-East, Building-Total,etc.)  Value is the actual WWR
            Dictionary<string, double> WWRList = new Dictionary<string, double>();
            double buildingWestExtWallArea = 0;
            double buildingWestWindowArea = 0;
            double buildingSouthExtWallArea = 0;
            double buildingSouthWindowArea = 0;
            double buildingEastExtWallArea = 0;
            double buildingEastWindowArea = 0;
            double buildingNorthExtWallArea = 0;
            double buildingNorthWindowArea = 0;

            List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> foundSurfaces = new List<ModelingUtilities.BuildingObjects.MemorySafe_Surface>();
            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface wallSurface in projectSurfaces)
            {
                if (wallSurface.outsideBoundary == ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors ||
                        wallSurface.outsideBoundary == ModelingUtilities.BuildingObjects.OutsideBoundary.Ground)
                {
                    string surfaceName = wallSurface.name;
                    string constructionType = wallSurface.constructionName;
                    string azimuth = wallSurface.azimuth.ToString();
                    string tilt = wallSurface.tilt.ToString();
                    double wallArea = Vector.GetAreaofSurface(wallSurface)/Math.Pow(.3048,2);
                    string multiplier = wallSurface.multiplier.ToString();
                    if(wallSurface.multiplier>0) wallArea = wallArea * wallSurface.multiplier;
                    
                    string comma = ",";
                    string outline = surfaceName+comma+constructionType+comma+wallArea.ToString()+comma+multiplier+comma+azimuth+comma+tilt;
                    output.AppendLine(outline);
                }
                if (wallSurface.surfaceType == ModelingUtilities.BuildingObjects.SurfaceTypes.Wall)
                {
                    if (wallSurface.outsideBoundary == ModelingUtilities.BuildingObjects.OutsideBoundary.Outdoors ||
                        wallSurface.outsideBoundary == ModelingUtilities.BuildingObjects.OutsideBoundary.Ground)
                    {
                        if (wallSurface.name == "2nd%Floor/1:02%office%center_Wall_11_2_1")
                        {
                            Console.Write("");
                        }
                        double azimuth = wallSurface.azimuth;
                        if ((azimuth > 315 && azimuth <= 360) || (azimuth >= 0 && azimuth < 45))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(wallSurface)/Math.Pow(0.3048,2);
                            if (wallSurface.multiplier > 0) buildingNorthExtWallArea += wallarea * wallSurface.multiplier;
                            else buildingNorthExtWallArea += wallarea;
                        }
                        else if ((azimuth >= 45 && azimuth < 135))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(wallSurface) / Math.Pow(0.3048, 2);
                            if (wallSurface.multiplier > 0) buildingEastExtWallArea += wallarea * wallSurface.multiplier;
                            else buildingEastExtWallArea += wallarea;
                        }
                        else if ((azimuth >= 135 && azimuth < 225))
                        {
                            double wallarea = Vector.GetAreaofSurface(wallSurface) / Math.Pow(0.3048, 2);
                            if (wallSurface.multiplier > 0) buildingSouthExtWallArea += wallarea * wallSurface.multiplier;
                            else buildingSouthExtWallArea += wallarea;
                        }
                        else if ((azimuth >= 225 && azimuth < 315))
                        {
                            //get the area of the surface
                            double wallarea = Vector.GetAreaofSurface(wallSurface) / Math.Pow(0.3048, 2);
                            if (wallSurface.multiplier > 0) buildingWestExtWallArea += wallarea * wallSurface.multiplier;
                            else buildingWestExtWallArea += wallarea;
                        }
                    }
                }
            }


            //loop through the windows and get the parent name.  Figure out what direction the parent is facing. 
            for (int i = 0; i < projectOpenings.Count(); i++)
            {

                string parentSurfaceName = projectOpenings[i].parentSurfaceNameId;
                //search for parent surface
                for (int j = 0; j < projectSurfaces.Count(); j++)
                {
                    if (projectSurfaces[j].name != parentSurfaceName) continue;
                    else if (projectSurfaces[j].name == parentSurfaceName)
                    {
                        
                        //initialize found surfaces
                        if (i == 0) foundSurfaces.Add(projectSurfaces[j]);
                        double parentAzimuth = projectSurfaces[j].azimuth;
                        //bin the surface areas based on orientation
                        if ((parentAzimuth > 315 && parentAzimuth <= 360) || (parentAzimuth >= 0 && parentAzimuth < 45))
                        {
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i])/Math.Pow(0.3048,2);
                            //modifications to the window and wall area due to multipliers
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                           
                            buildingNorthWindowArea += windowarea;
                        }
                        else if ((parentAzimuth >= 45 && parentAzimuth < 135))
                        {

                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i])/Math.Pow(0.3048,2);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            
                            buildingEastWindowArea += windowarea;
                        }

                        else if ((parentAzimuth >= 135 && parentAzimuth < 225))
                        {
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i])/Math.Pow(0.3048,2);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            
                            buildingSouthWindowArea += windowarea;
                        }
                        else if ((parentAzimuth >= 225 && parentAzimuth < 315))
                        {
                            //get the area of the window
                            double windowarea = Vector.GetAreaofWindow(projectOpenings[i])/Math.Pow(0.3048,2);
                            if (projectSurfaces[j].multiplier > 0)
                            {
                                windowarea *= projectOpenings[i].multiplier * projectSurfaces[j].multiplier;
                            }
                            else
                            {
                                windowarea *= projectOpenings[i].multiplier;
                            }
                            output.AppendLine(projectOpenings[i].nameId + "," + windowarea.ToString() + "," +
                                projectOpenings[i].parentSurfaceNameId + "," + projectOpenings[i].Azimuth.ToString() + ","
                                + projectOpenings[i].Tilt.ToString());
                            
                            buildingWestWindowArea += windowarea;
                        }
                    }
                }

            }

            string eastWWRKey = "buildingEastWWR";
            double buildingEastWWR = buildingEastWindowArea / buildingEastExtWallArea;
            string northWWRKey = "buildingNorthWWR";
            double buildingNorthWWR = buildingNorthWindowArea / buildingNorthExtWallArea;
            string southWWRKey = "buildingSouthWWR";
            double buildingSouthWWR = buildingSouthWindowArea / buildingSouthExtWallArea;
            string westWWRKey = "buildingWestWWR";
            double buildingWestWWR = buildingWestWindowArea / buildingWestExtWallArea;
            string buildingWWRKey = "buildingTotalWWR";
            double buildingWWR = (buildingEastWindowArea + buildingNorthWindowArea + buildingSouthWindowArea + buildingWestWindowArea) /
                (buildingEastExtWallArea + buildingNorthExtWallArea + buildingSouthExtWallArea + buildingWestExtWallArea);

            double buildingTotalExtWallArea = (buildingEastExtWallArea + buildingNorthExtWallArea + buildingSouthExtWallArea + buildingWestExtWallArea);
            double buildingTotalExtWindowArea = (buildingEastWindowArea + buildingNorthWindowArea + buildingSouthWindowArea + buildingWestWindowArea);
            //write to csv
            output.AppendLine("Category,Total,North,East,South,West");
            output.AppendLine("Wall Area(m2)," + buildingTotalExtWallArea.ToString() + "," + buildingNorthExtWallArea.ToString() +
                "," + buildingEastExtWallArea.ToString() + "," + buildingSouthExtWallArea.ToString() + "," + buildingWestExtWallArea.ToString());
            output.AppendLine("Window Area(m2)," + buildingTotalExtWindowArea.ToString() + "," + buildingNorthWindowArea.ToString() + ","
                + buildingEastWindowArea.ToString() + "," + buildingSouthWindowArea.ToString() + "," + buildingWestWindowArea.ToString());

            //add the values to the list
            WWRList.Add(buildingWWRKey, buildingWWR);
            WWRList.Add(northWWRKey, buildingNorthWWR);
            WWRList.Add(eastWWRKey, buildingEastWWR);
            WWRList.Add(southWWRKey, buildingSouthWWR);
            WWRList.Add(westWWRKey, buildingWestWWR);

            using (StreamWriter writer = new StreamWriter("C:\\Temp\\detailedWindow.csv", false))
            {
                writer.Write(output.ToString());
                writer.Close();
            }

            return WWRList;
        }

        //file comparisons

        //public static void CompareTwoIDFFileGeometries(string idf1, string idf2)
        //{
        //    List<EPlusObjects.Spaces> matchFound = new List<EPlusObjects.Spaces>();
        //    List<EPlusObjects.Spaces> notMatched = new List<EPlusObjects.Spaces>();

        //    try
        //    {
        //        //set up the Log file
        //        string log = @"C:\Users\Chiensi\Documents\AAATerabuild\EnergyPlus\homework\log.txt";
        //        StringBuilder logline = new StringBuilder();
        //        Encoding encoding;

        //        logline.AppendLine("Comparing Spaces in two IDF files...................................");

        //        List<ModelingUtilities.BuildingObjects.Spaces> idf1Spaces = new List<ModelingUtilities.BuildingObjects.Spaces>();
        //        List<ModelingUtilities.BuildingObjects.Spaces> idf2Spaces = new List<ModelingUtilities.BuildingObjects.Spaces>();

        //        idf1Spaces = EPlusSpacestoObjectList(idf1);
        //        idf2Spaces = EPlusSpacestoObjectList(idf2);
        //        //compare Spaces 
        //        //look for common names
        //        int idf1SpaceCount = idf1Spaces.Count();
        //        int idf2SpaceCount = idf2Spaces.Count();
        //        if (idf1SpaceCount == idf2SpaceCount)
        //        {
        //            //success
        //            logline.AppendLine("Counts of Spaces in IDF Files the Same");
        //            //for each space from idf1, try to find a match in all of idf2
        //            logline = MatchIDFFileSpaces(idf1Spaces, idf2Spaces, logline);
        //            for (int i = 0; i < idf1SpaceCount; i++)
        //            {
        //                int j = 0;
        //                foreach (EPlusObjects.Spaces space in idf2Spaces)
        //                {

        //                    if (space.name == idf1Spaces[i].name)
        //                    {
        //                        #region

        //                        logline.AppendLine("Space named: " + idf1Spaces[i].name + " found.");
        //                        int surfaceCount1 = idf1Spaces[i].spaceSurfaces.Count();
        //                        int surfaceCount2 = space.spaceSurfaces.Count();

        //                        if (surfaceCount1 == surfaceCount2)
        //                        {
        //                            logline.AppendLine("Same number of Surfaces for the matched spaces");
        //                        }



        //                        foreach (EPlusObjects.Surface surface in idf1Spaces[i].spaceSurfaces)
        //                        {
        //                            int surfaceMatchCount = 0;
        //                            //coordinates in matched surface of IDF1
        //                            foreach (Vector.CartCoord coord in surface.SurfaceCoords)
        //                            {
        //                                double diffX = -1;
        //                                double diffY = -1;
        //                                double diffZ = -1;
        //                                foreach (EPlusObjects.Surface surface2 in space.spaceSurfaces)
        //                                {

        //                                    //coordinates in matched surface in IDF2
        //                                    foreach (Vector.CartCoord coord2 in surface2.SurfaceCoords)
        //                                    {
        //                                        if (diffX == 0 && diffY == 0 && diffZ == 0) break;
        //                                        if (diffX == -1)
        //                                        {
        //                                            diffX = Math.Abs(coord.X - coord2.X);
        //                                        }
        //                                        else
        //                                        {
        //                                            if (diffX > Math.Abs(coord.X - coord2.X))
        //                                            {
        //                                                diffX = Math.Abs(coord.X - coord2.X);
        //                                            }
        //                                        }
        //                                        if (diffY == -1)
        //                                        {
        //                                            diffY = Math.Abs(coord.Y - coord2.Y);
        //                                        }
        //                                        else
        //                                        {
        //                                            if (diffY > Math.Abs(coord.Y - coord2.Y))
        //                                            {
        //                                                diffY = Math.Abs(coord.Y - coord2.Y);
        //                                            }
        //                                        }
        //                                        if (diffZ == -1)
        //                                        {
        //                                            diffZ = Math.Abs(coord.Z - coord2.Z);
        //                                        }
        //                                        else
        //                                        {
        //                                            if (diffZ > Math.Abs(coord.Z - coord2.Z))
        //                                            {
        //                                                diffZ = Math.Abs(coord.Z - coord2.Z);
        //                                            }
        //                                        }
        //                                    }
        //                                    if (diffX == 0 && diffY == 0 && diffZ == 0)
        //                                    {
        //                                        logline.AppendLine("Coordinate match, X: " + coord.X.ToString() + "Y: " + coord.Y.ToString() + "Z: " + coord.Z.ToString());
        //                                        surfaceMatchCount++;
        //                                        matchFound.Add(idf1Spaces[i]);
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        if (j == idf2SpaceCount - 1)
        //                        {
        //                            logline.AppendLine("No space name match found for : " + idf1Spaces[i].name);
        //                            notMatched.Add(idf1Spaces[i]);
        //                        }
        //                        continue;
        //                    }
        //                    j++;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            logline.AppendLine("Counts of Spaces in IDF Files not the Same. ERROR.");
        //            //pass the larger of the two spaces first to the function
        //            if (idf1SpaceCount > idf2SpaceCount)
        //            {
        //                logline.AppendLine(idf1 + " is primary because it has more spaces.");
        //                logline = MatchIDFFileSpaces(idf1Spaces, idf2Spaces, logline);
        //            }
        //            else
        //            {
        //                logline.AppendLine(idf2 + " is primary because it has more spaces.");
        //                logline = MatchIDFFileSpaces(idf2Spaces, idf1Spaces, logline);
        //            }
        //        }
        //        using (StreamWriter writer = new StreamWriter(log, false))
        //        {
        //            writer.Write(logline.ToString());
        //            writer.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }

        //}

        private static StringBuilder MatchIDFFileSpaces(List<ModelingUtilities.BuildingObjects.Spaces> idf1Spaces, List<ModelingUtilities.BuildingObjects.Spaces> idf2Spaces, StringBuilder logline)
        {
            List<ModelingUtilities.BuildingObjects.Spaces> matchFound = new List<ModelingUtilities.BuildingObjects.Spaces>();
            List<ModelingUtilities.BuildingObjects.Spaces> notMatched = new List<ModelingUtilities.BuildingObjects.Spaces>();

            int idf1SpaceCount = idf1Spaces.Count();
            int idf2SpaceCount = idf2Spaces.Count();
            bool spaceMatch = false;
            for (int i = 0; i < idf1SpaceCount; i++)
            {
                int j = 0;
                foreach (ModelingUtilities.BuildingObjects.Spaces space in idf2Spaces)
                {
                    //this function hinges on the names being the same for the spaces in both files.  Used to compare two spaces of similar size
                    if (space.name == idf1Spaces[i].name)
                    {
                        #region

                        logline.AppendLine("Space named: " + idf1Spaces[i].name + " found.");
                        int surfaceCount1 = idf1Spaces[i].spaceSurfaces.Count();
                        int surfaceCount2 = space.spaceSurfaces.Count();

                        if (surfaceCount1 == surfaceCount2)
                        {
                            logline.AppendLine("Same number of Surfaces for the matched spaces.  Good sign.");
                        }


                        int surfaceMatchCount = 0;
                        bool IssurfaceMatch = false;

                        for (int l = 0; l < idf1Spaces[i].spaceSurfaces.Count; l++)
                        {
                            //it will try to look through every surface in that space, and find a coordinate match
                            for (int q = 0; q < space.spaceSurfaces.Count; q++)
                            {
                                int coordmatchCount = 0;
                                bool IscoordMatch = false;
                                //coordinates in matched surface of IDF1
                                foreach (Vector.CartCoord coord in idf1Spaces[i].spaceSurfaces[l].SurfaceCoords)
                                {
                                    double diffX = -1;
                                    double diffY = -1;
                                    double diffZ = -1;
                                    //it will try to look through every surface in that space, and find a coordinate match
                                    //coordinates in matched surface in IDF2

                                    foreach (Vector.CartCoord coord2 in space.spaceSurfaces[q].SurfaceCoords)
                                    {

                                        diffX = Math.Abs(coord.X - coord2.X);
                                        diffY = Math.Abs(coord.Y - coord2.Y);
                                        diffZ = Math.Abs(coord.Z - coord2.Z);
                                        if (diffX == 0 && diffY == 0 && diffZ == 0) break;
                                    }
                                    if (diffX == 0 && diffY == 0 && diffZ == 0)
                                    {
                                        logline.AppendLine("Coordinate match, X: " + coord.X.ToString() + "Y: " + coord.Y.ToString() + "Z: " + coord.Z.ToString());
                                        //one match found.  If all coordinates match, then it is a perfect match
                                        coordmatchCount++;
                                        IscoordMatch = true;
                                        //if all the coordinate values match, then the surface matches
                                        if (coordmatchCount == idf1Spaces[i].spaceSurfaces[l].SurfaceCoords.Count())
                                        {
                                            //also need to check for the outward facing normal and self intersection tests
                                            surfaceMatchCount++;
                                            logline.AppendLine("Surface Match for surface " + l.ToString() + " for space " + idf1Spaces[i].name);
                                            IssurfaceMatch = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //could not find a coordinate match for the given coordinate, this fails the surface, but this does not mean the test fails
                                        IssurfaceMatch = false;
                                        break;
                                        //logline.AppendLine("Could not find match for coordinate X: " + coord.X.ToString() + "Y: " + coord.Y.ToString() + "Z: " + coord.Z.ToString());
                                        //I could try and break out of the whole routine at this point, because I could not find a coordinate match for a given coordinate 
                                    }
                                }
                                //if the coordinate match count is good, it kicks us to here, but could also be reached when the coordinate matching is not successful
                                if (IssurfaceMatch)
                                {
                                    if (IssurfaceMatch) break;
                                }
                            }
                            if (surfaceMatchCount == idf1Spaces[i].spaceSurfaces.Count())
                            {
                                matchFound.Add(idf1Spaces[i]);
                                spaceMatch = true;
                                logline.AppendLine("Found all surface matches for space: " + idf1Spaces[i].name);
                                break;
                            }
                            else
                            {
                                if (!IssurfaceMatch)
                                {
                                    logline.AppendLine("No space name match found for : " + idf1Spaces[i].name + ".  Surfaces could not be matched, though name matched.");
                                    notMatched.Add(idf1Spaces[i]);
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (j == idf2SpaceCount - 1)
                        {
                            logline.AppendLine("No space name match found for : " + idf1Spaces[i].name + ".  No name match.");
                            notMatched.Add(idf1Spaces[i]);

                        }

                    }
                    j++;
                    if (spaceMatch)
                    {
                        spaceMatch = false;
                        break;
                    }
                }
            }
            return logline;

        }

        //public static bool ModifyWWR(double projectWWR, double desiredUserWWR, string idfname)
        //{
        //    //every time I encounter a window, I want to see what its WWR is.  
        //    //get window area
        //    //get wall area
        //    //calculated the WWR
        //    //if it is not what is desired, make changes to the idf File (a temp file or something)
        //    //continue until completed
        //    throw new NotImplementedException();
        //}

        public static bool ModifyWWR(string tempFileLocation, double projectWWR, double desiredUserWWR, double sillHeight, string idfname, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            //the bool that is returned 
            bool successfulUpdate = false;

            List<Vector.MemorySafe_CartCoord> newCoords = new List<Vector.MemorySafe_CartCoord>();
            StringBuilder output = new StringBuilder();

            //open the idf file and start to read it
            //every time I encounter a window, I want to see what its WWR is.  
            //get window area
            //get wall area
            //calculated the WWR
            //if it is not what is desired, make changes to the idf File (a temp file or something)
            //continue until completed
            try
            {
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex fenestrationNameRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationName);
                Regex fenestrationParentSurfaceRegex = new Regex(EPlusObjects.EPlusRegexString.parentSurfaceName);
                Regex typicalVertexRegex = new Regex(EPlusObjects.EPlusRegexString.typicalVertex);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
                Regex multiplierRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationMultiplier);

                Encoding encoding;
                bool firstLineofFenestration = false;
                bool semicolonfound = false;

                string windowName = "";
                string parentSurfaceName = "";

                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    int vertexCounter = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            firstLineofFenestration = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestration)
                        {
                            //Match the name of the window
                            Match fenestrationNameMatch = fenestrationNameRegex.Match(line);
                            if (fenestrationNameMatch.Success)
                            {
                                firstLineofFenestration = false;
                                //strip off the whitespace and comma
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(fenestrationNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    windowName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    continue;
                                }
                            }
                            Match parentSurfaceIdMatch = fenestrationParentSurfaceRegex.Match(line);
                            if (parentSurfaceIdMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(parentSurfaceIdMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    parentSurfaceName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    continue;
                                }
                            }
                            //Get multiplier
                            Match multiplierMatch = multiplierRegex.Match(line);
                            if (multiplierMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(multiplierMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    string frontSpace = "    ";
                                    //string multiPlier = pure.Groups["goods"].Value;
                                    string multiPlier = "1";
                                    string comma = ",";
                                    string endString = "                       !- Multiplier";
                                    output.AppendLine(frontSpace + multiPlier + comma + endString);
                                    continue;
                                }
                            }
                            //My next match is the coordinates, since the rest I should be able to skip at this point
                            Match typicalVertexMatch = typicalVertexRegex.Match(line);
                            if (typicalVertexMatch.Success)
                            {
                                vertexMatching = true;

                                if (windowName.Length > 0 && parentSurfaceName.Length > 0)
                                {
                                    #region
                                    //globally change all the windows to a new window area based on the surface attached
                                    //get the areas
                                    foreach (ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window in projectOpenings)
                                    {
                                        if (window.nameId == windowName)
                                        {
                                            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in projectSurfaces)
                                            {
                                                if (window.parentSurfaceNameId == surface.name)
                                                {
                                                    double windowArea = Vector.GetAreaofWindow(window);
                                                    double wallArea = Vector.GetAreaofSurface(surface);
                                                    if (surface.multiplier > 0)
                                                    {
                                                        wallArea *= surface.multiplier;
                                                        windowArea *= surface.multiplier * window.multiplier;
                                                    }
                                                    else
                                                    {
                                                        windowArea *= window.multiplier;
                                                    }

                                                    double WWR = windowArea / wallArea;
                                                    if (WWR != desiredUserWWR)
                                                    {
                                                        //simple here in assuming that the wall is a square surface, otherwise
                                                        //we would have to use some sort of scaling function that emanates from the
                                                        //centroid, which we currently do not have, so we only work when the num vertices
                                                        //is equal to four
                                                        if (surface.numVertices == 4)
                                                        {
                                                            //under the simplifying assumption that walls are tilted to 90oF
                                                            double zlow = 0.0;
                                                            double zHigh = 0.0;
                                                            for (int j = 0; j < surface.SurfaceCoords.Count; j++)
                                                            {
                                                                if (j == 0)
                                                                {
                                                                    zlow = surface.SurfaceCoords[j].Z;
                                                                    zHigh = surface.SurfaceCoords[j].Z;
                                                                }
                                                                else
                                                                {
                                                                    if (surface.SurfaceCoords[j].Z < zlow) zlow = surface.SurfaceCoords[j].Z;
                                                                    if (surface.SurfaceCoords[j].Z > zHigh) zHigh = surface.SurfaceCoords[j].Z;
                                                                }
                                                            }
                                                            double wallHeight = zHigh - zlow;
                                                            //make each x, y vertex the same in both the surface and window
                                                            //and strip window heights that are adjusted based on user preference
                                                            if (window.numVertices == 4)
                                                            {
                                                                //we assume that the vertexes start in the lower left hand corner, though 
                                                                //subject to change
                                                                for (int i = 0; i < window.coordinateList.Count; i++)
                                                                {
                                                                    
                                                                    //assume that the coordinates both go in the same order
                                                                    switch (i)
                                                                    {
                                                                        case 0:
                                                                            double X = surface.SurfaceCoords[i].X;
                                                                            double Y = surface.SurfaceCoords[i].Y;
                                                                            //this should be settable
                                                                            double Z = surface.SurfaceCoords[i].Z + sillHeight;
                                                                            Vector.MemorySafe_CartCoord newWindowCoord = new Vector.MemorySafe_CartCoord(X,Y,Z);
                                                                            newCoords.Add(newWindowCoord);
                                                                            break;
                                                                        case 1:
                                                                            double X1 = surface.SurfaceCoords[i].X;
                                                                            double Y1 = surface.SurfaceCoords[i].Y;
                                                                            //this should be settable
                                                                            double Z1 = surface.SurfaceCoords[i].Z + sillHeight;
                                                                            Vector.MemorySafe_CartCoord newWindowCoord1 = new Vector.MemorySafe_CartCoord(X1, Y1, Z1);
                                                                            newCoords.Add(newWindowCoord1);
                                                                            break;
                                                                        case 2:
                                                                            double X2 = surface.SurfaceCoords[i].X;
                                                                            double Y2 = surface.SurfaceCoords[i].Y;
                                                                            //this should be settable
                                                                            double Z2 = zlow + sillHeight + desiredUserWWR * wallHeight;
                                                                            Vector.MemorySafe_CartCoord newWindowCoord2 = new Vector.MemorySafe_CartCoord(X2, Y2, Z2);
                                                                            newCoords.Add(newWindowCoord2);
                                                                            break;
                                                                        case 3:
                                                                            double X3 = surface.SurfaceCoords[i].X;
                                                                            double Y3 = surface.SurfaceCoords[i].Y;
                                                                            //this should be settable
                                                                            double Z3 = zlow + sillHeight + desiredUserWWR * wallHeight;
                                                                            Vector.MemorySafe_CartCoord newWindowCoord3 = new Vector.MemorySafe_CartCoord(X3, Y3, Z3);
                                                                            newCoords.Add(newWindowCoord3);
                                                                            break;
                                                                    }
                                                                    
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                //once I have the newCoordinates, then I should go on to change the values in the file
                                //we can assume that if there are no new coords, then the old file matches the new exactly
                                if (newCoords.Count > 0)
                                {
                                    //make a new vertex point and append it to the line
                                    //do a check for the semicolon now because this changes the type of exchange that we make
                                    Match semicolonMatch = semiColonRegex.Match(line);
                                    if (semicolonMatch.Success)
                                    {
                                        string frontBlank = "     ";
                                        string comma = ",";
                                        string newX = newCoords[vertexCounter - 1].X.ToString();
                                        string newY = newCoords[vertexCounter - 1].Y.ToString();
                                        string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                        string semicolon = ";";
                                        string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                        string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                                        output.AppendLine(newVertexLine);
                                        vertexCounter++;
                                        newCoords.Clear();
                                        //reset
                                        vertexMatching = false;
                                        detailedFenestration = false;
                                        vertexCounter = 1;
                                        continue;
                                    }
                                    //it is a normal typical vertex line
                                    else
                                    {
                                        string frontBlank = "     ";
                                        string comma = ",";
                                        string newX = newCoords[vertexCounter - 1].X.ToString();
                                        string newY = newCoords[vertexCounter - 1].Y.ToString();
                                        string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                        string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                        string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                                        output.AppendLine(newVertexLine);
                                        vertexCounter++;
                                        newCoords.Clear();
                                        continue;
                                    }

                                }
                                //apparently the lines were exactly the same
                                else
                                {
                                    output.AppendLine(line);
                                    vertexCounter++;
                                    continue;
                                }
                            }
                            //this is admittedly a bit sloppy and should be cleaned up
                            //basically says, if there is no match, then move append and work on the next line
                            else
                            {
                                output.AppendLine(line);
                            }
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successfulUpdate = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return successfulUpdate;
        }

        public static bool MakeStripWindow(string tempFileLocation, double northWWR, double eastWWR, double southWWR, double westWWR, double sillHeight, string idfname, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            //the bool that is returned 
            bool successfulUpdate = false;

            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            StringBuilder output = new StringBuilder();
            double dlBuffer = 0;

            //open the idf file and start to read it
            //every time I encounter a window, I want to see what its WWR is.  
            //get window area
            //get wall area
            //calculated the WWR
            //if it is not what is desired, make changes to the idf File (a temp file or something)
            //continue until completed
            try
            {
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex fenestrationNameRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationName);
                Regex fenestrationParentSurfaceRegex = new Regex(EPlusObjects.EPlusRegexString.parentSurfaceName);
                Regex typicalVertexRegex = new Regex(EPlusObjects.EPlusRegexString.typicalVertex);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
                Regex multiplierRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationMultiplier);

                Encoding encoding;
                bool firstLineofFenestration = false;
                bool semicolonfound = false;

                string windowName = "";
                string parentSurfaceName = "";

                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    int vertexCounter = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            firstLineofFenestration = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestration)
                        {
                            //Match the name of the window
                            Match fenestrationNameMatch = fenestrationNameRegex.Match(line);
                            if (fenestrationNameMatch.Success)
                            {
                                firstLineofFenestration = false;
                                //strip off the whitespace and comma
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(fenestrationNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    windowName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    continue;
                                }
                            }
                            Match parentSurfaceIdMatch = fenestrationParentSurfaceRegex.Match(line);
                            if (parentSurfaceIdMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(parentSurfaceIdMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    parentSurfaceName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    continue;
                                }
                            }
                            //Get multiplier
                            Match multiplierMatch = multiplierRegex.Match(line);
                            if (multiplierMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(multiplierMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    string frontSpace = "    ";
                                    //string multiPlier = pure.Groups["goods"].Value;
                                    string multiPlier = "1";
                                    string comma = ",";
                                    string endString = "                       !- Multiplier";
                                    output.AppendLine(frontSpace + multiPlier + comma + endString);
                                    continue;
                                }
                            }
                            //My next match is the coordinates, since the rest I should be able to skip at this point
                            Match typicalVertexMatch = typicalVertexRegex.Match(line);
                            if (typicalVertexMatch.Success)
                            {
                                vertexMatching = true;

                                if (windowName.Length > 0 && parentSurfaceName.Length > 0)
                                {
                                    #region
                                    //globally change all the windows to a new window area based on the surface attached
                                    //get the areas
                                    foreach (ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window in projectOpenings)
                                    {
                                        if (window.nameId == windowName)
                                        {
                                            ModelingUtilities.BuildingObjects.MemorySafe_Surface surface = GetParentSurface(window.parentSurfaceNameId, projectSurfaces);
                                            double windowArea = Vector.GetAreaofWindow(window);
                                            double wallArea = Vector.GetAreaofSurface(surface);
                                            if (surface.multiplier > 0)
                                            {
                                                wallArea *= surface.multiplier;
                                                windowArea *= surface.multiplier * window.multiplier;
                                            }
                                            else
                                            {
                                                windowArea *= window.multiplier;
                                            }

                                            double WWR = windowArea / wallArea;
                                            if (ModelingUtilities.OrientationUtil.isNorth(window, 0))
                                            {
                                                double wallHeight = GetNormalQuadWallHeight(surface);
                                                newCoords = GetNewNormalQuadStripWindowCoordinates(surface, northWWR, sillHeight, wallHeight, dlBuffer);
                                            }
                                            else if (ModelingUtilities.OrientationUtil.isEast(window, 0))
                                            {
                                                double wallHeight = GetNormalQuadWallHeight(surface);
                                                newCoords = GetNewNormalQuadStripWindowCoordinates(surface, eastWWR, sillHeight, wallHeight, dlBuffer);
                                            }
                                            else if (ModelingUtilities.OrientationUtil.isSouth(window, 0))
                                            {
                                                double wallHeight = GetNormalQuadWallHeight(surface);
                                                newCoords = GetNewNormalQuadStripWindowCoordinates(surface, southWWR, sillHeight, wallHeight, dlBuffer);
                                            }
                                            else if (ModelingUtilities.OrientationUtil.isWest(window, 0))
                                            {
                                                double wallHeight = GetNormalQuadWallHeight(surface);
                                                newCoords = GetNewNormalQuadStripWindowCoordinates(surface, westWWR, sillHeight, wallHeight, dlBuffer);
                                            }
                                        }
                                    }



                                    #endregion

                                    //once I have the newCoordinates, then I should go on to change the values in the file
                                    //we can assume that if there are no new coords, then the old file matches the new exactly
                                    if (newCoords.Count > 0)
                                    {
                                        //make a new vertex point and append it to the line
                                        //do a check for the semicolon now because this changes the type of exchange that we make
                                        Match semicolonMatch = semiColonRegex.Match(line);
                                        if (semicolonMatch.Success)
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string semicolon = ";";
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                                            output.AppendLine(newVertexLine);
                                            vertexCounter++;
                                            newCoords.Clear();
                                            //reset
                                            vertexMatching = false;
                                            detailedFenestration = false;
                                            vertexCounter = 1;
                                            continue;
                                        }
                                        //it is a normal typical vertex line
                                        else
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                                            output.AppendLine(newVertexLine);
                                            vertexCounter++;
                                            newCoords.Clear();
                                            continue;
                                        }

                                    }
                                    //apparently the lines were exactly the same
                                    else
                                    {
                                        output.AppendLine(line);
                                        vertexCounter++;
                                        continue;
                                    }
                                }
                            }
                            //this is admittedly a bit sloppy and should be cleaned up
                            //basically says, if there is no match, then move append and work on the next line
                            else
                            {
                                output.AppendLine(line);
                            }
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successfulUpdate = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return successfulUpdate;
        }

        public static List<EPlusObjects.RadiantSurfaceGroup> MakeSurfaceGroupObject(string tempFileLocation)
        {
            List<EPlusObjects.RadiantSurfaceGroup> radiantSurfaceGroups = new List<EPlusObjects.RadiantSurfaceGroup>();

            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            StringBuilder output = new StringBuilder();

            //open the idf file and start to read it
            //every time I encounter a window, I want to see what its WWR is.  
            //get window area
            //get wall area
            //calculated the WWR
            //if it is not what is desired, make changes to the idf File (a temp file or something)
            //continue until completed
            try
            {
                Regex surfaceGroupStart = new Regex(EPlusObjects.EPlusRegexString.startRadiantSurfaceGroup);
                Regex surfaceGroupName = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex surfaceNumNameRegex = new Regex(EPlusObjects.EPlusRegexString.surfaceNumName);
                Regex flowFractionRegex = new Regex(EPlusObjects.EPlusRegexString.surfaceFlowFracNum);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);


                Encoding encoding;
                bool surfaceGroupFirstLine = false;
                bool semicolonfound = false;

                string surfaceName = "";
                double surfaceFlowFrac = 0.0;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    //List of strings to make a surface Group
                    List<string> surfaceGroupStrings = new List<string>();

                    string line;
                    encoding = reader.CurrentEncoding;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match surfaceGroupStartMatch = surfaceGroupStart.Match(line);
                        if(surfaceGroupStartMatch.Success)
                        {
                            surfaceGroupFirstLine = true;
                            surfaceGroupStrings.Add(line);
                            continue;
                        }
                        if (surfaceGroupFirstLine)
                        {
                            Match semiColonMatch = semiColonRegex.Match(line);
                            if (semiColonMatch.Success)
                            {
                                surfaceGroupStrings.Add(line);
                                EPlusObjects.RadiantSurfaceGroup radSurfaceGroup = MakeRadSurfaceGroup(surfaceGroupStrings);
                                surfaceGroupFirstLine = false;
                                radiantSurfaceGroups.Add(radSurfaceGroup);
                                surfaceGroupStrings.Clear();
                            }
                            else
                            {
                                surfaceGroupStrings.Add(line);
                            }

                        }
                    }

                }

            }
            catch (Exception e)
            {

            }
            return radiantSurfaceGroups;
        }

        private static EPlusObjects.RadiantSurfaceGroup MakeRadSurfaceGroup(List<string> surfaceGroupStrings)
        {
            EPlusObjects.RadiantSurfaceGroup radSurfaceGroup = new EPlusObjects.RadiantSurfaceGroup();
            radSurfaceGroup.surfaceNameAndFF = new Dictionary<string, double>();
            string surfaceName = "";
            double surfaceFlowFrac = 0.0;

            Regex surfaceGroupStart = new Regex(EPlusObjects.EPlusRegexString.startRadiantSurfaceGroup);
            Regex surfaceGroupName = new Regex(EPlusObjects.EPlusRegexString.Name);
            Regex surfaceNumNameRegex = new Regex(EPlusObjects.EPlusRegexString.surfaceNumName);
            Regex flowFractionRegex = new Regex(EPlusObjects.EPlusRegexString.surfaceFlowFracNum);
            Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);


            Encoding encoding;
            bool surfaceGroupFirstLine = false;
            bool semicolonfound = false;

            foreach (string line in surfaceGroupStrings)
            {
                Match surfaceGroupStartMatch = surfaceGroupStart.Match(line);
                if (surfaceGroupStartMatch.Success)
                {
                    surfaceGroupFirstLine = true;
                    continue;
                }
                if (surfaceGroupFirstLine)
                {
                    Match surfaceGroupNameMatch = surfaceGroupName.Match(line);
                    if (surfaceGroupNameMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(surfaceGroupNameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            radSurfaceGroup.groupName = pure.Groups["goods"].Value;
                            continue;
                        }
                    }

                    Match surfaceNumNameMatch = surfaceNumNameRegex.Match(line);
                    if (surfaceNumNameMatch.Success)
                    {
                        string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                        Regex purifyRegex = new Regex(purify);
                        Match pure = purifyRegex.Match(surfaceNumNameMatch.Groups["1"].Value);
                        if (pure.Success)
                        {
                            surfaceName = pure.Groups["goods"].Value;
                            continue;
                        }
                    }


                    Match flowFractionMatch = flowFractionRegex.Match(line);
                    if (flowFractionMatch.Success)
                    {
                        Match semiColonMatch = semiColonRegex.Match(line);
                        if (semiColonMatch.Success)
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'semicolon';)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(flowFractionMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                surfaceFlowFrac = Convert.ToDouble(pure.Groups["goods"].Value);
                                surfaceFlowFrac = Math.Round(surfaceFlowFrac, 2);
                            }
                            surfaceGroupFirstLine = false;
                            semicolonfound = true;
                            radSurfaceGroup.surfaceNameAndFF.Add(surfaceName, surfaceFlowFrac);
                        }
                        else
                        {
                            string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                            Regex purifyRegex = new Regex(purify);
                            Match pure = purifyRegex.Match(flowFractionMatch.Groups["1"].Value);
                            if (pure.Success)
                            {
                                surfaceFlowFrac = Convert.ToDouble(pure.Groups["goods"].Value);
                                surfaceFlowFrac = Math.Round(surfaceFlowFrac, 2);
                            }
                        }
                    }

                    if (!semicolonfound) radSurfaceGroup.surfaceNameAndFF.Add(surfaceName, surfaceFlowFrac);
                }
            }
            return radSurfaceGroup;
        }

        public static bool MakePunchedWindowFromMultiples(string tempFileLocation, double northWWR, double eastWWR, double southWWR, double westWWR, double sillHeight, string idfname, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {

            makeWindowLogFile.AppendLine("MakePunchedWindowFromMultiples");

            //the bool that is returned 
            bool successfulUpdate = false;
            //the local window and surface
            //ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window = new ModelingUtilities.BuildingObjects.OpeningDefinitions();
            //ModelingUtilities.BuildingObjects.Surface surface = new ModelingUtilities.BuildingObjects.Surface();
            int windowFoundCount = 0;

            List<Vector.MemorySafe_CartCoord> newCoords = new List<Vector.MemorySafe_CartCoord>();
            StringBuilder output = new StringBuilder();

            //open the idf file and start to read it
            //every time I encounter a window, I want to see what its WWR is.  
            //get window area
            //get wall area
            //calculated the WWR
            //if it is not what is desired, make changes to the idf File (a temp file or something)
            //continue until completed
            try
            {
                Regex fenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex fenestrationNameRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationName);
                Regex fenestrationParentSurfaceRegex = new Regex(EPlusObjects.EPlusRegexString.parentSurfaceName);
                Regex typicalVertexRegex = new Regex(EPlusObjects.EPlusRegexString.typicalVertex);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
                Regex multiplierRegex = new Regex(EPlusObjects.EPlusRegexString.fenestrationMultiplier);

                Encoding encoding;
                bool firstLineofFenestration = false;
                bool semicolonfound = false;

                string windowName = "";
                string parentSurfaceName = "";

                using (StreamReader reader = new StreamReader(idfname))
                {
                    string line;
                    encoding = reader.CurrentEncoding;
                    bool detailedFenestration = false;
                    bool vertexMatching = false;
                    int vertexCounter = 1;
                    int newWindowCounter = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        MatchCollection fenStart = fenestrationYes.Matches(line);
                        if (fenStart.Count > 0)
                        {
                            detailedFenestration = true;
                            windowFoundCount++;
                            makeWindowLogFile.AppendLine("Window:"+windowFoundCount.ToString());
                            firstLineofFenestration = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestration)
                        {
                            //Match the name of the window
                            Match fenestrationNameMatch = fenestrationNameRegex.Match(line);
                            if (fenestrationNameMatch.Success)
                            {
                                firstLineofFenestration = false;
                                //strip off the whitespace and comma
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(fenestrationNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    windowName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    makeWindowLogFile.AppendLine("windowName : " +windowName);
                                    continue;
                                }
                            }
                            Match parentSurfaceIdMatch = fenestrationParentSurfaceRegex.Match(line);
                            if (parentSurfaceIdMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(parentSurfaceIdMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    parentSurfaceName = pure.Groups["goods"].Value;
                                    output.AppendLine(line);
                                    makeWindowLogFile.AppendLine("parentSurface : "+parentSurfaceName);
                                    continue;
                                }
                            }
                            //Get multiplier
                            Match multiplierMatch = multiplierRegex.Match(line);
                            if (multiplierMatch.Success)
                            {
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(multiplierMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    string frontSpace = "    ";
                                    string multiplier = pure.Groups["goods"].Value;

                                    string newmultiPlier = "1";
                                    string comma = ",";
                                    string endString = "                       !- Multiplier";
                                    output.AppendLine(frontSpace + newmultiPlier + comma + endString);
                                    makeWindowLogFile.AppendLine("multiplier : "+multiplier);
                                    continue;
                                }
                            }
                            //My next match is the coordinates, since the rest I should be able to skip at this point
                            Match typicalVertexMatch = typicalVertexRegex.Match(line);
                            if (typicalVertexMatch.Success)
                            {
                                ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window = GetOpeningfromStringId(windowName, projectOpenings);
                                //this little bit of code had to be added so that I could prevent extra lines from being written
                                //after the first vertex has been discovered
                                if (!detailedFenestration) continue;
                                

                                if (windowName.Length > 0 && parentSurfaceName.Length > 0)
                                {
                                    #region
                                    //globally change all the windows to a new window area based on the surface attached
                                    //get the areas
                                    //if(!vertexMatching) window = GetOpeningfromStringId(windowName, projectOpenings);
                                    if (window.nameId == windowName)
                                    {
                                        ModelingUtilities.BuildingObjects.MemorySafe_Surface surface = GetParentSurface(window.parentSurfaceNameId, projectSurfaces);
                                        vertexMatching = true;
                                        double wallarea = Vector.GetAreaofSurface(surface);

                                        if (window.nameId == "2nd%Floor/1:02%office%N_Wall_3_0_0_0_0_0_Win")
                                        {
                                            Console.Write("");
                                        }

                                        if (surface.numVertices == 4 && window.numVertices == 4)
                                        {
                                            makeWindowLogFile.AppendLine("Surface and window is normal quad..........");
                                            //this routine is only for square windows
                                            double wallHeight = GetNormalQuadWallHeight(surface);
                                            double wallWidth = GetNormalQuadWallWidth(surface);
                                            double windowWidth = GetNormalQuadWindowWidth(window);
                                            double windowSpacing = (wallWidth - (windowWidth * window.multiplier)) / window.multiplier;
                                            double firstSpace = windowSpacing / 4;
                                            if (newWindowCounter == 1)
                                            {
                                                if (newCoords.Count() == 0)
                                                {
                                                    
                                                    if (window.multiplier > 1)
                                                    {
                                                        makeWindowLogFile.AppendLine("Window Multiplier is > 1");
                                                        makeWindowLogFile.AppendLine("Getting new Coordinates");
                                                        newCoords = GetNewNormalQuadPunchedWindowCoordinates(surface, window, firstSpace, windowSpacing, windowWidth, newWindowCounter);
                                                    }
                                                    else newCoords = window.coordinateList;
                                                }
                                                                                                                                 
                                                //once I have the newCoordinates, then I should go on to change the values in the file
                                                //we can assume that if there are no new coords, then the old file matches the new exactly
                                                if (newCoords.Count > 0)
                                                {
                                                    //make a new vertex point and append it to the line
                                                    if (vertexCounter != newCoords.Count)
                                                    {
                                                        string frontBlank = "     ";
                                                        string comma = ",";
                                                        string newX = newCoords[vertexCounter - 1].X.ToString();
                                                        string newY = newCoords[vertexCounter - 1].Y.ToString();
                                                        string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                                        string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                                        string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                                                        output.AppendLine(newVertexLine);
                                                        vertexCounter++;
                                                        continue;
                                                    }
                                                    //it is a normal typical vertex line with a semicolon at the end (the last one)
                                                    else
                                                    {
                                                        string frontBlank = "     ";
                                                        string comma = ",";
                                                        string newX = newCoords[vertexCounter - 1].X.ToString();
                                                        string newY = newCoords[vertexCounter - 1].Y.ToString();
                                                        string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                                        string semicolon = ";";
                                                        string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                                        string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                                                        output.AppendLine(newVertexLine);
                                                        //reset which should prevent the rest of the vertices from being written
                                                        vertexMatching = false;
                                                        detailedFenestration = false;
                                                        newCoords.Clear();
                                                        vertexCounter = 1;
                                                        if (window.multiplier > 1) newWindowCounter++;

                                                        //Now that the original Window is Complete, make the new Window(s) if multiplier needs to
                                                        if (window.multiplier > 1)
                                                        {
                                                            output.AppendLine("");
                                                            while (newWindowCounter <= window.multiplier)
                                                            {
                                                                //make an entirely new window from the original window
                                                                //make coords
                                                                newCoords = GetNewNormalQuadPunchedWindowCoordinates(surface, window, firstSpace, windowSpacing, windowWidth, newWindowCounter);
                                                                if (newCoords.Count() == 0)
                                                                {
                                                                    makeWindowLogFile.AppendLine("No window was created in the idf because newCoordinates not within parent surface Bound");
                                                                    //newWindowCounter++;
                                                                    //continue;
                                                                }
                                                                
                                                                //now make the new window Text
                                                                //copy the window
                                                                int newMultiplier = 1;
                                                                ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions newWindow = new ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions(window, newMultiplier);
                                                                //make the window
                                                                output = MakePunchedWindowDuplicates(newWindow, newCoords, output, newWindowCounter);
                                                                if (newWindowCounter == window.multiplier)
                                                                {
                                                                    detailedFenestration = false;
                                                                    vertexMatching = false;
                                                                    newWindowCounter = 1;
                                                                    newCoords.Clear();
                                                                    break;
                                                                }
                                                                newWindowCounter++;
                                                            }
                                                        } 

                                                    }
                                                }
                                                else
                                                {
                                                    makeWindowLogFile.AppendLine("No window was created in the idf because newCoordinates not within parent surface Bound");
                                                    continue;
                                                }

                                            }
                                            else
                                            {

                                            }

                                        }
                                    }

                                    #endregion
                                }
                            }
                            //this is admittedly a bit sloppy and should be cleaned up
                            //basically says, if there is no match, then move append and work on the next line
                            else
                            {
                                output.AppendLine(line);
                            }
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successfulUpdate = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
                bool logWritten = FileMng.CreateLogFile(makeWindowLogFileLocation, makeWindowLogFile, false, encoding);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return successfulUpdate;
        }

        private static StringBuilder MakePunchedWindowDuplicates(ModelingUtilities.BuildingObjects.OpeningDefinitions currentOpening,
            List<Vector.CartCoord> newCoords, StringBuilder output, int windowCount)
        {
            List<Vector.CartCoord> dlCoords = newCoords;

            //copy the existing window
            ModelingUtilities.BuildingObjects.OpeningDefinitions newOpening = new ModelingUtilities.BuildingObjects.OpeningDefinitions(currentOpening);
            //change the coordinates
            //sanity check to make sure that this window will fit future


            //
            output.AppendLine(" ");
            output.AppendLine("  " + EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + newOpening.nameId + "_AD_" + windowCount.ToString() 
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);

            output.AppendLine("    " + newOpening.openingType + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.Type);

            output.AppendLine("    " + newOpening.constructionName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);

            output.AppendLine("    " + newOpening.parentSurfaceNameId + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);

            output.AppendLine("    " + newOpening.outsideBoundaryConditionObj + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + newOpening.viewFactortoGround + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + newOpening.shadeControlSch + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + newOpening.frameAndDividerName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + newOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + newOpening.numVertices + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 1; i <= newCoords.Count; i++)
            {
                if (i < newCoords.Count)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i-1].X.ToString();
                    string newY = dlCoords[i-1].Y.ToString();
                    string newZ = dlCoords[i-1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i-1].X.ToString();
                    string newY = dlCoords[i-1].Y.ToString();
                    string newZ = dlCoords[i-1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                output.AppendLine(" ");
            }


            return output;
            
        }

        private static StringBuilder MakePunchedWindowDuplicates(ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions currentOpening,
    List<Vector.CartCoord> newCoords, StringBuilder output, int windowCount)
        {
            List<Vector.CartCoord> dlCoords = newCoords;

            //copy the existing window
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions newOpening = new ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions(currentOpening);
            //change the coordinates
            //sanity check to make sure that this window will fit future


            //
            output.AppendLine(" ");
            output.AppendLine("  " + EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + newOpening.nameId + "_AD_" + windowCount.ToString()
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);

            output.AppendLine("    " + newOpening.openingType + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.Type);

            output.AppendLine("    " + newOpening.constructionName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);

            output.AppendLine("    " + newOpening.parentSurfaceNameId + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);

            output.AppendLine("    " + newOpening.outsideBoundaryConditionObj + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + newOpening.viewFactortoGround + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + newOpening.shadeControlSch + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + newOpening.frameAndDividerName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + newOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + newOpening.numVertices + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 1; i <= newCoords.Count; i++)
            {
                if (i < newCoords.Count)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i - 1].X.ToString();
                    string newY = dlCoords[i - 1].Y.ToString();
                    string newZ = dlCoords[i - 1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i - 1].X.ToString();
                    string newY = dlCoords[i - 1].Y.ToString();
                    string newZ = dlCoords[i - 1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                output.AppendLine(" ");
            }


            return output;
        }

        private static StringBuilder MakePunchedWindowDuplicates(ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions currentOpening,
List<Vector.MemorySafe_CartCoord> newCoords, StringBuilder output, int windowCount)
        {
            List<Vector.MemorySafe_CartCoord> dlCoords = newCoords;

            //copy the existing window
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions newOpening = new ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions(currentOpening);
            //change the coordinates
            //sanity check to make sure that this window will fit future


            //
            output.AppendLine(" ");
            output.AppendLine("  " + EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + newOpening.nameId + "_AD_" + windowCount.ToString()
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);

            output.AppendLine("    " + newOpening.openingType + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.Type);

            output.AppendLine("    " + newOpening.constructionName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);

            output.AppendLine("    " + newOpening.parentSurfaceNameId + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);

            output.AppendLine("    " + newOpening.outsideBoundaryConditionObj + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + newOpening.viewFactortoGround + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + newOpening.shadeControlSch + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + newOpening.frameAndDividerName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + newOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + newOpening.numVertices + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 1; i <= newCoords.Count; i++)
            {
                if (i < newCoords.Count)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i - 1].X.ToString();
                    string newY = dlCoords[i - 1].Y.ToString();
                    string newZ = dlCoords[i - 1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i - 1].X.ToString();
                    string newY = dlCoords[i - 1].Y.ToString();
                    string newZ = dlCoords[i - 1].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                output.AppendLine(" ");
            }


            return output;

        }

        public static bool MakeSouthOverhangs(string tempFileLocation, List<ModelingUtilities.BuildingObjects.OpeningDefinitions> projectOpenings, double viewOHDepth, double dlOHDepth)
        {
            //setup
            bool success = false;
            List<List<Vector.CartCoord>> dlWindowCoords = new List<List<Vector.CartCoord>>();
            List<Vector.CartVect> normalVectors = new List<Vector.CartVect>();
            List<List<Vector.CartCoord>> ShadeCoords = new List<List<Vector.CartCoord>>();

            try
            {
                string dLRegexString = ".*(_DL)";
                Regex dLRegex = new Regex(dLRegexString);
                foreach (ModelingUtilities.BuildingObjects.OpeningDefinitions opening in projectOpenings)
                {
                    Match dLOpeningMatch = dLRegex.Match(opening.nameId);
                    if (dLOpeningMatch.Success)
                    {
                        dlWindowCoords.Add(opening.coordinateList);
                    }
                }
                //sanity check
                foreach (List<Vector.CartCoord> coordLists in dlWindowCoords)
                {
                    if (coordLists.Count > 4)
                    {
                        Console.WriteLine("");
                    }
                }
                //make the RHR vectors
                foreach (List<Vector.CartCoord> coordLIsts in dlWindowCoords)
                {
                    Vector.CartVect RHRVector = new Vector.CartVect();
                    RHRVector = Vector.GetRHR(coordLIsts);
                    RHRVector = Vector.UnitVector(RHRVector);
                    normalVectors.Add(RHRVector);
                }
                int dlCoordListCounter = 0;
                foreach (List<Vector.CartCoord> coordLists in dlWindowCoords)
                {
                    Vector.CartCoord viewCoord = new Vector.CartCoord();
                    Vector.CartCoord dlCoord = new Vector.CartCoord();
                    List<Vector.CartCoord> viewShadeCoord = new List<Vector.CartCoord>();
                    List<Vector.CartCoord> dLShadeCoord = new List<Vector.CartCoord>();
                    double zlow = 0;
                    double zHigh = 0;
                    //find the top and bottom of the coordinates.  The top is for DL, bottom is for view
                    for (int i = 0; i < coordLists.Count; i++)
                    {
                        if (coordLists[i].Z < zlow) zlow = coordLists[i].Z;
                        if (coordLists[i].Z > zHigh) zHigh = coordLists[i].Z;
                    }

                    //second pass to make the coordinates for the overhang
                    //this is a hack based on what I know about the sequence of coordinates.  This could definitely break
                    //first the view shade
                    viewCoord.X = coordLists[1].X + normalVectors[dlCoordListCounter].X * viewOHDepth;
                    viewCoord.Y = coordLists[1].Y + normalVectors[dlCoordListCounter].Y * viewOHDepth;
                    viewCoord.Z = coordLists[1].Z;
                    viewShadeCoord.Add(viewCoord);
                    viewCoord.X = coordLists[0].X + normalVectors[dlCoordListCounter].X * viewOHDepth;
                    viewCoord.Y = coordLists[0].Y + normalVectors[dlCoordListCounter].Y * viewOHDepth;
                    viewCoord.Z = coordLists[0].Z;
                    viewShadeCoord.Add(viewCoord);
                    viewShadeCoord.Add(coordLists[0]);
                    viewShadeCoord.Add(coordLists[1]);
                    ShadeCoords.Add(viewShadeCoord);
                    //next the DL shade
                    dlCoord.X = coordLists[2].X + normalVectors[dlCoordListCounter].X * dlOHDepth;
                    dlCoord.Y = coordLists[2].Y + normalVectors[dlCoordListCounter].Y * dlOHDepth;
                    dlCoord.Z = coordLists[2].Z;
                    dLShadeCoord.Add(dlCoord);
                    dlCoord.X = coordLists[3].X + normalVectors[dlCoordListCounter].X * dlOHDepth;
                    dlCoord.Y = coordLists[3].Y + normalVectors[dlCoordListCounter].Y * dlOHDepth;
                    dlCoord.Z = coordLists[3].Z;
                    dLShadeCoord.Add(dlCoord);
                    dLShadeCoord.Add(coordLists[3]);
                    dLShadeCoord.Add(coordLists[2]);
                    ShadeCoords.Add(dLShadeCoord);
                    dlCoordListCounter++;
                }
                //make the new file with the appropriate overhangs.
                //set up a stringbuilder
                StringBuilder output = new StringBuilder();
                //make the regex objects
                Regex frameAndDividerDescYes = new Regex(EPlusObjects.EPlusRegexString.startWindowFrameDesc);
                Regex detailedBuildingShadeYes = new Regex(EPlusObjects.EPlusRegexString.startDetailedBuildingShade);
                Regex commentStartYes = new Regex(EPlusObjects.EPlusRegexString.commentStart);
                Regex semiColonYes = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                Encoding encoding;

                string line;
                //bools as needed
                bool frameAndDividerStart = false;
                bool lastFrameAndDivider = false;
                bool semiColonFound = false;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    //set up the surface
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match detailedFrameAndDivideMatch = frameAndDividerDescYes.Match(line);
                        if (detailedFrameAndDivideMatch.Success)
                        {
                            frameAndDividerStart = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (frameAndDividerStart)
                        {
                            Match semiColonMatch = semiColonYes.Match(line);
                            if (semiColonMatch.Success)
                            {
                                semiColonFound = true;
                                output.AppendLine(line);
                                frameAndDividerStart = false;
                                continue;
                            }
                            else
                            {
                                output.AppendLine(line);
                                continue;
                            }
                        }
                        if (semiColonFound)
                        {

                            detailedFrameAndDivideMatch = frameAndDividerDescYes.Match(line);
                            if (detailedFrameAndDivideMatch.Success)
                            {
                                semiColonFound = false;
                                frameAndDividerStart = true;
                                output.AppendLine(line);
                            }
                            else if (line == "") output.AppendLine(line);
                            else
                            {
                                semiColonFound = false;
                                Match commentStartMatch = commentStartYes.Match(line);
                                if (commentStartMatch.Success)
                                {
                                    //dont write this line yet, insert the shading objects.  We assume it is empty
                                    output.AppendLine("!- CHarriman Shading Surfaces");
                                    output.AppendLine("");
                                    int coordSetCounter = 1;
                                    foreach (List<Vector.CartCoord> shadingCoords in ShadeCoords)
                                    {
                                        //write the shading device basics
                                        output.AppendLine(EPlusObjects.EPlusRegexString.startDetailedBuildingShade);
                                        output.AppendLine("     Shade-" + coordSetCounter.ToString() + ",    !-Name");
                                        output.AppendLine("    Transmittance schedule for Extrnl Shades,   !- Transmittance Schedule Name");
                                        output.AppendLine("    " + shadingCoords.Count.ToString() + ",    !- Number of Vertices");
                                        //write the coordinates
                                        int coordCounter = 1;
                                        foreach (Vector.CartCoord shadingCoord in shadingCoords)
                                        {
                                            output.AppendLine("     "+shadingCoord.X.ToString() + ",          !- Vertex " + coordCounter.ToString() + " X-Coordinate {m}");
                                            output.AppendLine("     "+shadingCoord.Y.ToString() + ",          !- Vertex " + coordCounter.ToString() + " Y-Coordinate {m}");
                                            if (coordCounter < shadingCoords.Count)
                                            {
                                                output.AppendLine("     "+shadingCoord.Z.ToString() + ",          !- Vertex " + coordCounter.ToString() + " Z-Coordinate {m}");
                                            }
                                            else
                                            {
                                                output.AppendLine("     "+shadingCoord.Z.ToString() + ";          !- Vertex " + coordCounter.ToString() + " Z-Coordinate {m}");
                                                output.AppendLine("");
                                            }
                                            coordCounter++;
                                        }
                                        coordSetCounter++;
                                    }
                                    output.AppendLine(line);
                                }

                            }

                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                success = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);

            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public static bool MakeSouthOverhangs(string tempFileLocation, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, double viewOHDepth, double dlOHDepth)
        {
            //setup
            string shadeFileLoc = @"C:\Temp\ShadeData.csv";
            StringBuilder ShadeLog = new StringBuilder();
            bool success = false;
            List<List<Vector.MemorySafe_CartCoord>> dlWindowCoords = new List<List<Vector.MemorySafe_CartCoord>>();
            List<Vector.MemorySafe_CartVect> normalVectors = new List<Vector.MemorySafe_CartVect>();
            List<List<Vector.MemorySafe_CartCoord>> ShadeCoords = new List<List<Vector.MemorySafe_CartCoord>>();

            try
            {
                string dLRegexString = ".*(_DL)";
                Regex dLRegex = new Regex(dLRegexString);
                foreach (ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions opening in projectOpenings)
                {
                    Match dLOpeningMatch = dLRegex.Match(opening.nameId);
                    if (dLOpeningMatch.Success)
                    {
                        dlWindowCoords.Add(opening.coordinateList);
                    }
                }
                //sanity check
                foreach (List<Vector.MemorySafe_CartCoord> coordLists in dlWindowCoords)
                {
                    if (coordLists.Count > 4)
                    {
                        Console.WriteLine("");
                    }
                }
                //make the RHR vectors
                foreach (List<Vector.MemorySafe_CartCoord> coordLIsts in dlWindowCoords)
                {
                    
                    Vector.MemorySafe_CartVect RHRVector = Vector.GetMemRHR(coordLIsts);
                    RHRVector = Vector.UnitVector(RHRVector);
                    normalVectors.Add(RHRVector);
                }
                int dlCoordListCounter = 0;
                foreach (List<Vector.MemorySafe_CartCoord> coordLists in dlWindowCoords)
                {
                    Vector.CartCoord viewCoord = new Vector.CartCoord();
                    Vector.CartCoord dlCoord = new Vector.CartCoord();
                    List<Vector.MemorySafe_CartCoord> viewShadeCoord = new List<Vector.MemorySafe_CartCoord>();
                    List<Vector.MemorySafe_CartCoord> dLShadeCoord = new List<Vector.MemorySafe_CartCoord>();
                    double zlow = 0;
                    double zHigh = 0;
                    //find the top and bottom of the coordinates.  The top is for DL, bottom is for view
                    for (int i = 0; i < coordLists.Count; i++)
                    {
                        if (coordLists[i].Z < zlow) zlow = coordLists[i].Z;
                        if (coordLists[i].Z > zHigh) zHigh = coordLists[i].Z;
                    }

                    //second pass to make the coordinates for the overhang
                    //this is a hack based on what I know about the sequence of coordinates.  This could definitely break
                    //first the view shade
                    viewCoord.X = coordLists[1].X + normalVectors[dlCoordListCounter].X * viewOHDepth;
                    viewCoord.Y = coordLists[1].Y + normalVectors[dlCoordListCounter].Y * viewOHDepth;
                    viewCoord.Z = coordLists[1].Z;
                    Vector.MemorySafe_CartCoord viewShadeCoord1 = Vector.convertToMemorySafeCoord(viewCoord);
                    viewShadeCoord.Add(viewShadeCoord1);
                    viewCoord.X = coordLists[0].X + normalVectors[dlCoordListCounter].X * viewOHDepth;
                    viewCoord.Y = coordLists[0].Y + normalVectors[dlCoordListCounter].Y * viewOHDepth;
                    viewCoord.Z = coordLists[0].Z;
                    Vector.MemorySafe_CartCoord viewShadeCoord2 = Vector.convertToMemorySafeCoord(viewCoord);
                    viewShadeCoord.Add(viewShadeCoord2);
                    viewShadeCoord.Add(coordLists[0]);
                    viewShadeCoord.Add(coordLists[1]);
                    ShadeCoords.Add(viewShadeCoord);
                    //next the DL shade
                    dlCoord.X = coordLists[2].X + normalVectors[dlCoordListCounter].X * dlOHDepth;
                    dlCoord.Y = coordLists[2].Y + normalVectors[dlCoordListCounter].Y * dlOHDepth;
                    dlCoord.Z = coordLists[2].Z;
                    Vector.MemorySafe_CartCoord dlShadeCoord1 = Vector.convertToMemorySafeCoord(dlCoord);
                    dLShadeCoord.Add(dlShadeCoord1);
                    dlCoord.X = coordLists[3].X + normalVectors[dlCoordListCounter].X * dlOHDepth;
                    dlCoord.Y = coordLists[3].Y + normalVectors[dlCoordListCounter].Y * dlOHDepth;
                    dlCoord.Z = coordLists[3].Z;
                    Vector.MemorySafe_CartCoord dlShadeCoord2 = Vector.convertToMemorySafeCoord(dlCoord);
                    dLShadeCoord.Add(dlShadeCoord2);
                    dLShadeCoord.Add(coordLists[3]);
                    dLShadeCoord.Add(coordLists[2]);
                    ShadeCoords.Add(dLShadeCoord);
                    dlCoordListCounter++;
                }
                //make the new file with the appropriate overhangs.
                //set up a stringbuilder
                StringBuilder output = new StringBuilder();
                //make the regex objects
                Regex frameAndDividerDescYes = new Regex(EPlusObjects.EPlusRegexString.startWindowFrameDesc);
                Regex detailedBuildingShadeYes = new Regex(EPlusObjects.EPlusRegexString.startDetailedBuildingShade);
                Regex commentStartYes = new Regex(EPlusObjects.EPlusRegexString.commentStart);
                Regex semiColonYes = new Regex(EPlusObjects.EPlusRegexString.semicolon);

                Encoding encoding;

                string line;
                //bools as needed
                bool frameAndDividerStart = false;
                bool lastFrameAndDivider = false;
                bool semiColonFound = false;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    //set up the surface
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match detailedFrameAndDivideMatch = frameAndDividerDescYes.Match(line);
                        if (detailedFrameAndDivideMatch.Success)
                        {
                            frameAndDividerStart = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (frameAndDividerStart)
                        {
                            Match semiColonMatch = semiColonYes.Match(line);
                            if (semiColonMatch.Success)
                            {
                                semiColonFound = true;
                                output.AppendLine(line);
                                frameAndDividerStart = false;
                                continue;
                            }
                            else
                            {
                                output.AppendLine(line);
                                continue;
                            }
                        }
                        if (semiColonFound)
                        {

                            detailedFrameAndDivideMatch = frameAndDividerDescYes.Match(line);
                            if (detailedFrameAndDivideMatch.Success)
                            {
                                semiColonFound = false;
                                frameAndDividerStart = true;
                                output.AppendLine(line);
                            }
                            else if (line == "") output.AppendLine(line);
                            else
                            {
                                semiColonFound = false;
                                Match commentStartMatch = commentStartYes.Match(line);
                                if (commentStartMatch.Success)
                                {

                                    //dont write this line yet, insert the shading objects.  We assume it is empty
                                    output.AppendLine("!- CHarriman Shading Surfaces");

                                    output.AppendLine("");
                                    int coordSetCounter = 1;
                                    foreach (List<Vector.MemorySafe_CartCoord> shadingCoords in ShadeCoords)
                                    {
                                        double area = Vector.GetAreaFrom2DPolyLoop(shadingCoords);
                                        ShadeLog.AppendLine(area.ToString());
                                        for (int i = 0; i < (shadingCoords.Count()-1); i++)
                                        {
                                            Vector.MemorySafe_CartVect vector = Vector.CreateMemorySafe_Vector(shadingCoords[i], shadingCoords[i+1]);
                                            string vecCoords = vector.X.ToString() + ',' + vector.Y.ToString() + ',' + vector.Z.ToString();
                                            string distance = (Math.Sqrt(Math.Pow(vector.X,2) +Math.Pow(vector.Y,2))).ToString();
                                            ShadeLog.AppendLine(vecCoords);
                                            ShadeLog.AppendLine(distance);
                                        }
                                    
                                        //write the shading device basics
                                        output.AppendLine(EPlusObjects.EPlusRegexString.startDetailedBuildingShade);
                                        output.AppendLine("     Shade-" + coordSetCounter.ToString() + ",    !-Name");
                                        output.AppendLine("    Transmittance schedule for Extrnl Shades,   !- Transmittance Schedule Name");
                                        output.AppendLine("    " + shadingCoords.Count.ToString() + ",    !- Number of Vertices");
                                        //write the coordinates
                                        int coordCounter = 1;
                                        foreach (Vector.MemorySafe_CartCoord shadingCoord in shadingCoords)
                                        {
                                            output.AppendLine("     " + shadingCoord.X.ToString() + ",          !- Vertex " + coordCounter.ToString() + " X-Coordinate {m}");
                                            output.AppendLine("     " + shadingCoord.Y.ToString() + ",          !- Vertex " + coordCounter.ToString() + " Y-Coordinate {m}");
                                            if (coordCounter < shadingCoords.Count)
                                            {
                                                output.AppendLine("     " + shadingCoord.Z.ToString() + ",          !- Vertex " + coordCounter.ToString() + " Z-Coordinate {m}");
                                            }
                                            else
                                            {
                                                output.AppendLine("     " + shadingCoord.Z.ToString() + ";          !- Vertex " + coordCounter.ToString() + " Z-Coordinate {m}");
                                                output.AppendLine("");
                                            }
                                            coordCounter++;
                                        }
                                        coordSetCounter++;
                                    }
                                    output.AppendLine(line);
                                }

                            }

                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                success = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
                using (StreamWriter writer = new StreamWriter(shadeFileLoc, false))
                {
                    writer.Write(ShadeLog.ToString());
                }

            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
        
        //this function will make the south facade a vision area and a daylight area, based on user-provided WWR of each
        //assume that the daylight area is shoved to the very top of the wall, as high as possible
        public static bool UpdateSouthFacade(string tempFileLocation, double visionWWR, double dlWWR, string idfname, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {

            bool successful = false;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            double dlHeight = 0;

            int vertexCounter = 1;
            double wallHeight = 0;
            double visionHeight = 0;
            ModelingUtilities.BuildingObjects.OpeningDefinitions tempOpening = new ModelingUtilities.BuildingObjects.OpeningDefinitions();
            ModelingUtilities.BuildingObjects.Surface tempSurface = new ModelingUtilities.BuildingObjects.Surface();
            //read the file.  If it encounters a window that is a south-facing window, then change it to be smaller and add a 
            //daylight glazing system after it that goes from 9' to 12'
            try
            {

                //set up a stringbuilder
                StringBuilder output = new StringBuilder();
                //make the regex objects
                Regex detailedSurfaceYes = new Regex(EPlusObjects.EPlusRegexString.startSurface);
                Regex detailedFenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex detailedObjectNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex constructionNameRegex = new Regex(EPlusObjects.EPlusRegexString.constructionName);
                Regex typicalVertexRegex = new Regex(EPlusObjects.EPlusRegexString.typicalVertex);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
                Encoding encoding;


                string line;
                //bools as needed
                bool detailedFenestrationbool = false;
                bool isSouth = false;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    //set up the surface
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match detailedFenestrationMatch = detailedFenestrationYes.Match(line);
                        if (detailedFenestrationMatch.Success)
                        {
                            detailedFenestrationbool = true;
                            output.AppendLine(line);
                            continue;
                        }
                        if (detailedFenestrationbool)
                        {
                            #region
                            //match name
                            Match fenestrationNameMatch = detailedObjectNameRegex.Match(line);
                            //once I get a match, set up a bunch of the variables that I will need 
                            string currentOpeningName = "";
                            if (fenestrationNameMatch.Success)
                            {
                                #region
                                //tease out the name using purify
                                
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(fenestrationNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentOpeningName = pure.Groups["goods"].Value;
                                }

                                //I do not know which orientation this is facing yet, but I can use the objects to figure it out
                                #endregion
                            }
                            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions currentOpening = GetOpeningfromStringId(currentOpeningName, projectOpenings);
                            isSouth = ModelingUtilities.OrientationUtil.isSouth(currentOpening, 0);
                            ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface = GetParentSurface(currentOpening.parentSurfaceNameId, projectSurfaces);
                            //this essentially tells the loop to go back into its normal cycle
                            if (!isSouth)
                            {
                                detailedFenestrationbool = false;
                            }
                            //My next match is the coordinates, since the rest I should be able to skip at this point
                            Match typicalVertexMatch = typicalVertexRegex.Match(line);
                            if (typicalVertexMatch.Success)
                            {
                                //if a south window, then make these changes
                                if (isSouth)
                                {
                                    #region
                                    
                                    //only use this for quad windows and only capture when starting (vertexCounter == 1)
                                    if (currentOpening.numVertices == 4 && parentSurface.numVertices == 4 && vertexCounter == 1)
                                    {
                                        //these functions work well for square walls
                                        wallHeight = GetNormalQuadWallHeight(parentSurface);
                                        //these variables could be stored by the computer
                                        //.2140 is the wwr of the daylight window
                                        dlHeight = wallHeight * dlWWR;
                                        //.209 is the wwr of the vision window
                                        
                                        visionHeight = wallHeight * visionWWR;
                                        //newCoords = GetNewNormalQuadStripWindowCoordinates(parentSurface, visionWWR, sillHeight, wallHeight);
                                        //makes a new window based on 
                                        newCoords = GetNewNormalQuadPunchedWindowCoordinates(currentOpening,visionHeight);
                                         
                                    }

                                    if (newCoords.Count > 0)
                                    {
                                        //make a new vertex point and append it to the line
                                        //do a check for the semicolon now because this changes the type of exchange that we make
                                        Match semicolonMatch = semiColonRegex.Match(line);
                                        if (semicolonMatch.Success)
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string semicolon = ";";
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                                            output.AppendLine(newVertexLine);

                                            //ok, this window is done, so we are now going to make another one
                                            //that will sit right above it, in the same window...
                                            //output = MakeDaylightWindow(parentSurface, currentOpening, newCoords, dlHeight, wallHeight, output, projectOpenings);
                                            output = MakePunchedDaylightWindow(tempOpening,newCoords, visionHeight, dlHeight, output);
                                            //reset

                                            detailedFenestrationbool = false;
                                            vertexCounter = 1; newCoords.Clear();
                                            isSouth = false;
                                            continue;
                                        }
                                        //it is a normal typical vertex line
                                        else
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                                            output.AppendLine(newVertexLine);
                                            vertexCounter++;
                                            continue;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            //in the case where I do not find a match but I am in detailedFenestration mode
                            //not a vertex match
                            //vertex match but not a southern window
                            output.AppendLine(line);
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successful = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
                return successful;

            }
            catch (Exception e)
            {
                return successful;
            }

        }

        //this function will make the south facade a vision area and a daylight area, based on user-provided WWR of each
        //assume that the daylight area is shoved to the very top of the wall, as high as possible
        //one feature that we added to this function is the ability to only update zones of a certain class
        public static bool UpdateSouthFacadePunchedWindows(string tempFileLocation, double visionWWR, double dlWWR, string idfname, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            //eventually, we want to create this list from the user, or from some sort of best-case list, and pass it in
            List<string> allowableZones = new List<string>();
            allowableZones.Add("office");
            allowableZones.Add( "lab");
            allowableZones.Add("class");
            bool successful = false;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            ModelingUtilities.BuildingObjects.Surface tempParentSurface = new ModelingUtilities.BuildingObjects.Surface();
            ModelingUtilities.BuildingObjects.OpeningDefinitions tempCurrentOpening = new ModelingUtilities.BuildingObjects.OpeningDefinitions();
            int vertexCounter = 1;
            double wallHeight = 0;
            double visionHeight = 0;
            double sillHeight = 0;
            double dlHeight = 0;
            double dlBuffer = 0.25;
            string currentOpeningName = "";
            //read the file.  If it encounters a window that is a south-facing window, then change it to be smaller and add a 
            //daylight glazing system after it that goes from 9' to 12'
            try
            {

                //set up a stringbuilder
                StringBuilder output = new StringBuilder();
                //make the regex objects
                Regex detailedSurfaceYes = new Regex(EPlusObjects.EPlusRegexString.startSurface);
                Regex detailedFenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex detailedObjectNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex constructionNameRegex = new Regex(EPlusObjects.EPlusRegexString.constructionName);
                Regex typicalVertexRegex = new Regex(EPlusObjects.EPlusRegexString.typicalVertex);
                Regex semiColonRegex = new Regex(EPlusObjects.EPlusRegexString.semicolon);
                Encoding encoding;


                string line;
                //bools as needed
                bool detailedFenestrationbool = false;
                bool isSouth = false;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    //set up the surface
                    while ((line = reader.ReadLine()) != null)
                    {
                        Match detailedFenestrationMatch = detailedFenestrationYes.Match(line);
                        if (detailedFenestrationMatch.Success)
                        {
                            detailedFenestrationbool = true;
                            output.AppendLine(line);
                            continue;
                        }
                        
                        if (detailedFenestrationbool)
                        {
                            #region
                            //match name
                            Match fenestrationNameMatch = detailedObjectNameRegex.Match(line);
                            //once I get a match, set up a bunch of the variables that I will need 
                            if (fenestrationNameMatch.Success)
                            {
                                #region
                                //tease out the name using purify
                                
                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(fenestrationNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentOpeningName = pure.Groups["goods"].Value;
                                }
                                //if it is not eligible, don't do it.  Just continue and forget the rest
                                bool eligible = GetEligibility(currentOpeningName, allowableZones);
                                if (!eligible)
                                {
                                    output.AppendLine(line);
                                    detailedFenestrationbool = false;
                                    continue;
                                }

                                #endregion
                            }
                            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions currentOpening = GetOpeningfromStringId(currentOpeningName, projectOpenings);
                            isSouth = ModelingUtilities.OrientationUtil.isSouth(currentOpening, 0);
                            ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface = GetParentSurface(currentOpening.parentSurfaceNameId, projectSurfaces);
                            //this essentially tells the loop to go back into its normal cycle
                            if (!isSouth)
                            {
                                detailedFenestrationbool = false;
                            }
                            //My next match is the coordinates, since the rest I should be able to skip at this point
                            Match typicalVertexMatch = typicalVertexRegex.Match(line);
                            if (typicalVertexMatch.Success)
                            {
                                //if a south window, then make these changes
                                if (isSouth)
                                {
                                    #region

                                    //only use this for quad windows and only capture when starting (vertexCounter == 1)
                                    if (currentOpening.numVertices == 4 && parentSurface.numVertices == 4 && vertexCounter == 1)
                                    {
                                        //these functions work well for square walls
                                        
                                        List<double> zLowandHeight = GetNormalQuadWallZLowandHeight(parentSurface);
                                        double zLow = zLowandHeight[0];
                                        wallHeight = zLowandHeight[1];
                                        //because this will be a strip window, this is an easy calculation
                                        dlHeight = wallHeight * dlWWR;

                                        //calculate the sill height
                                        //Height of wall - dlHeight-viewwindowHeight = sill height
                                        //assuming we want to push the dL window to the top of the window

                                        double wallArea = Vector.GetAreaofSurface(parentSurface);
                                        double windowWidth = GetNormalQuadWindowWidth(currentOpening);
                                        visionHeight = visionWWR * wallArea / windowWidth;
                                        sillHeight = wallHeight - dlHeight - visionHeight;
                                        
                                        //newCoords = GetNewNormalQuadStripWindowCoordinates(parentSurface, visionWWR, sillHeight, wallHeight);
                                        //makes a new punched window of a slightly smaller size
                                        newCoords = GetNewNormalQuadPunchedWindowCoordinates(currentOpening, visionHeight, sillHeight, zLow,dlBuffer);
                                    }

                                    if (newCoords.Count > 0)
                                    {
                                        //make a new vertex point and append it to the line
                                        //do a check for the semicolon now because this changes the type of exchange that we make
                                        Match semicolonMatch = semiColonRegex.Match(line);
                                        if (semicolonMatch.Success)
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string semicolon = ";";
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                                            output.AppendLine(newVertexLine);

                                            //ok, this window is done, so we are now going to make another one
                                            //that will sit right above it, in the same window...
                                            output = MakeDaylightWindow(parentSurface, currentOpening, newCoords, dlHeight, wallHeight, output, projectOpenings,dlBuffer);
                                            //output = MakePunchedDaylightWindow(currentOpening, newCoords, visionHeight, dlHeight, output);
                                            
                                            //reset

                                            detailedFenestrationbool = false;
                                            vertexCounter = 1; newCoords.Clear();
                                            isSouth = false;
                                            continue;
                                        }
                                        //it is a normal typical vertex line
                                        else
                                        {
                                            string frontBlank = "     ";
                                            string comma = ",";
                                            string newX = newCoords[vertexCounter - 1].X.ToString();
                                            string newY = newCoords[vertexCounter - 1].Y.ToString();
                                            string newZ = newCoords[vertexCounter - 1].Z.ToString();
                                            string remainder = "   !- X,Y,Z ==> Vertex " + vertexCounter.ToString() + " {m}";

                                            string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                                            output.AppendLine(newVertexLine);
                                            vertexCounter++;
                                            continue;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            //in the case where I do not find a match but I am in detailedFenestration mode
                            //not a vertex match
                            //vertex match but not a southern window
                            output.AppendLine(line);
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successful = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
                return successful;

            }
            catch (Exception e)
            {
                return successful;
            }

        }

        private static bool GetEligibility(string currentOpeningName, List<string> allowableZones)
        {
            bool upgradeEligible = false;
            string matchString;
            foreach (string match in allowableZones)
            {
                matchString = "(" + match + ")";
                Regex zoneCategoryRegex = new Regex(matchString);
                Match zoneCategoryMatch = zoneCategoryRegex.Match(currentOpeningName);
                if (zoneCategoryMatch.Success) upgradeEligible = true;

            }
            return upgradeEligible;
        }

        

        private static StringBuilder MakePunchedDaylightWindow(ModelingUtilities.BuildingObjects.OpeningDefinitions currentOpening, List<Vector.CartCoord> visionCoords, double visionHeight, double dlHeight, StringBuilder output)
        {
            List<Vector.CartCoord> dlCoords = new List<Vector.CartCoord>();
            ModelingUtilities.BuildingObjects.OpeningDefinitions dlOpening = new ModelingUtilities.BuildingObjects.OpeningDefinitions(currentOpening);
            //make coords
            //dlCoords = GetNewNormalQuadPunchedWindowCoordinates(currentOpening,dlHeight);
            dlCoords = GetNewDLUpperQuadPunchedWindowCoordinates(visionCoords, visionHeight, dlHeight);

            //
            output.AppendLine(" ");
            output.AppendLine("  " + EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + dlOpening.nameId + "_DL"
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);

            output.AppendLine("    " + dlOpening.openingType + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.Type);

            output.AppendLine("    " + dlOpening.constructionName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);

            output.AppendLine("    " + dlOpening.parentSurfaceNameId + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);

            output.AppendLine("    " + dlOpening.outsideBoundaryConditionObj + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + dlOpening.viewFactortoGround + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + dlOpening.shadeControlSch + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + dlOpening.frameAndDividerName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + dlOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + dlOpening.numVertices + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 0; i < dlCoords.Count; i++)
            {
                if (i < dlCoords.Count - 1)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + i.ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                
            }

            output.AppendLine(" ");
            return output;
        }

        private static StringBuilder MakeDaylightWindow(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface,
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions currentOpening, List<Vector.CartCoord> newCoords, double dlHeight, double wallHeight,
            StringBuilder output, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings, double dlBuffer)
        {
            List<Vector.CartCoord> dlCoords = new List<Vector.CartCoord>();
            
            //copy the existing window
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions dlOpening = new ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions(currentOpening);
            //change the coordinates
            double dlWWR = dlHeight / wallHeight;
            double sillHeight = wallHeight - dlHeight;
            //sanity check to make sure that this window will fit

            //make coords
            dlCoords = GetNewNormalQuadStripWindowCoordinates(parentSurface, dlWWR, sillHeight, wallHeight, dlBuffer);


           //
            output.AppendLine(" ");
            output.AppendLine("  "+EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + dlOpening.nameId + "_DL" 
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);
            
            output.AppendLine("    " + dlOpening.openingType + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.Type);
            
            output.AppendLine("    " + dlOpening.constructionName + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);
            
            output.AppendLine("    " + dlOpening.parentSurfaceNameId + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);
            
            output.AppendLine("    " + dlOpening.outsideBoundaryConditionObj + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + dlOpening.viewFactortoGround + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + dlOpening.shadeControlSch + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + dlOpening.frameAndDividerName + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + dlOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + dlOpening.numVertices + ",                 " 
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 0; i < newCoords.Count; i++)
            {
                if (i < newCoords.Count - 1)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + (i+1).ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + (i+1).ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                
            }
            output.AppendLine(" ");

            return output;

        }

        private static StringBuilder MakeDaylightWindow(ModelingUtilities.BuildingObjects.Surface parentSurface,
    ModelingUtilities.BuildingObjects.OpeningDefinitions currentOpening, List<Vector.CartCoord> newCoords, double dlHeight, double wallHeight,
    StringBuilder output, List<ModelingUtilities.BuildingObjects.OpeningDefinitions> projectOpenings)
        {
            List<Vector.CartCoord> dlCoords = new List<Vector.CartCoord>();

            //copy the existing window
            ModelingUtilities.BuildingObjects.OpeningDefinitions dlOpening = new ModelingUtilities.BuildingObjects.OpeningDefinitions(currentOpening);
            //change the coordinates
            double dlWWR = dlHeight / wallHeight;
            double sillHeight = wallHeight - dlHeight;
            //sanity check to make sure that this window will fit

            //make coords
            dlCoords = GetNewNormalQuadStripWindowCoordinates(parentSurface, dlWWR, sillHeight, wallHeight);


            //
            output.AppendLine(" ");
            output.AppendLine("  " + EPlusObjects.EPlusRegexString.startFenestration);
            output.AppendLine("    " + dlOpening.nameId + "_DL"
                + ",    " + EPlusObjects.TagEndings.DetFenEndings.Name);

            output.AppendLine("    " + dlOpening.openingType + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.Type);

            output.AppendLine("    " + dlOpening.constructionName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.ConstName);

            output.AppendLine("    " + dlOpening.parentSurfaceNameId + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.BldSurfName);

            output.AppendLine("    " + dlOpening.outsideBoundaryConditionObj + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.OutsideBound);

            output.AppendLine("    " + dlOpening.viewFactortoGround + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.viewFactor);

            output.AppendLine("    " + dlOpening.shadeControlSch + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.shadeControl);

            output.AppendLine("    " + dlOpening.frameAndDividerName + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.frameAndDivider);

            output.AppendLine("    " + dlOpening.multiplier + ",                 "
    + EPlusObjects.TagEndings.DetFenEndings.multiplier);

            output.AppendLine("    " + dlOpening.numVertices + ",                 "
                + EPlusObjects.TagEndings.DetFenEndings.numberVertices);

            for (int i = 0; i < newCoords.Count; i++)
            {
                if (i < newCoords.Count - 1)
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + (i + 1).ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + comma + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }
                else
                {
                    string frontBlank = "     ";
                    string comma = ",";
                    string semicolon = ";";
                    string newX = dlCoords[i].X.ToString();
                    string newY = dlCoords[i].Y.ToString();
                    string newZ = dlCoords[i].Z.ToString();
                    string remainder = "   !- X,Y,Z ==> Vertex " + (i + 1).ToString() + " {m}";

                    string newVertexLine = frontBlank + newX + comma + newY + comma + newZ + semicolon + remainder;
                    output.AppendLine(newVertexLine);
                    continue;
                }

            }
            output.AppendLine(" ");

            return output;

        }

        private static double GetNormalQuadWallHeight(ModelingUtilities.BuildingObjects.Surface parentSurface)
        {
            double zlow = 0.0;
            double zHigh = 0.0;
            for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
            {
                if (j == 0)
                {
                    zlow = parentSurface.SurfaceCoords[j].Z;
                    zHigh = parentSurface.SurfaceCoords[j].Z;
                }
                else
                {
                    if (parentSurface.SurfaceCoords[j].Z < zlow) zlow = parentSurface.SurfaceCoords[j].Z;
                    if (parentSurface.SurfaceCoords[j].Z > zHigh) zHigh = parentSurface.SurfaceCoords[j].Z;
                }
            }

            double wallHeight = zHigh - zlow;
            return wallHeight;
        }

        private static double GetNormalQuadWallHeight(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface)
        {
            double zlow = 0.0;
            double zHigh = 0.0;
            for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
            {
                if (j == 0)
                {
                    zlow = parentSurface.SurfaceCoords[j].Z;
                    zHigh = parentSurface.SurfaceCoords[j].Z;
                }
                else
                {
                    if (parentSurface.SurfaceCoords[j].Z < zlow) zlow = parentSurface.SurfaceCoords[j].Z;
                    if (parentSurface.SurfaceCoords[j].Z > zHigh) zHigh = parentSurface.SurfaceCoords[j].Z;
                }
            }

            double wallHeight = zHigh - zlow;
            return wallHeight;
        }

        private static List<double> GetNormalQuadWallZLowandHeight(ModelingUtilities.BuildingObjects.Surface parentSurface)
        {
            List<double> zLowThenHeight = new List<double>();
            double zlow = 0.0;
            double zHigh = 0.0;
            for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
            {
                if (j == 0)
                {
                    zlow = parentSurface.SurfaceCoords[j].Z;
                    zHigh = parentSurface.SurfaceCoords[j].Z;
                }
                else
                {
                    if (parentSurface.SurfaceCoords[j].Z < zlow) zlow = parentSurface.SurfaceCoords[j].Z;
                    if (parentSurface.SurfaceCoords[j].Z > zHigh) zHigh = parentSurface.SurfaceCoords[j].Z;
                }
            }

            double wallHeight = zHigh - zlow;
            zLowThenHeight.Add(zlow);
            zLowThenHeight.Add(wallHeight);
            return zLowThenHeight;
        }

        private static List<double> GetNormalQuadWallZLowandHeight(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface)
        {
            List<double> zLowThenHeight = new List<double>();
            double zlow = 0.0;
            double zHigh = 0.0;
            for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
            {
                if (j == 0)
                {
                    zlow = parentSurface.SurfaceCoords[j].Z;
                    zHigh = parentSurface.SurfaceCoords[j].Z;
                }
                else
                {
                    if (parentSurface.SurfaceCoords[j].Z < zlow) zlow = parentSurface.SurfaceCoords[j].Z;
                    if (parentSurface.SurfaceCoords[j].Z > zHigh) zHigh = parentSurface.SurfaceCoords[j].Z;
                }
            }

            double wallHeight = zHigh - zlow;
            zLowThenHeight.Add(zlow);
            zLowThenHeight.Add(wallHeight);
            return zLowThenHeight;
        }

        private static double GetNormalQuadWallWidth(ModelingUtilities.BuildingObjects.Surface parentSurface)
        {
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            bool x1Filled=false;
            bool x2Filled=false;
            bool y1Filled=false;
            bool y2Filled=false;

            for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
            {
                if (j == 0)
                {
                    x1 = parentSurface.SurfaceCoords[j].X;
                    x1Filled = true;
                    y1 = parentSurface.SurfaceCoords[j].Y;
                    y1Filled = true;
                }
                else
                {
                    if (parentSurface.SurfaceCoords[j].X != x1)
                    {
                        x2 = parentSurface.SurfaceCoords[j].X;
                        x2Filled = true;
                    }
                    if (parentSurface.SurfaceCoords[j].Y != y1)
                    {
                        y2 = parentSurface.SurfaceCoords[j].Y;
                        y2Filled = true;
                    }
                }
                if (x1Filled && y1Filled && x2Filled && y2Filled) break;
            }
            double wallWidth = Math.Pow((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)), 0.5);
            return wallWidth;

        }

        private static double GetNormalQuadWallWidth(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface)
        {
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            bool x1Filled = false;
            bool x2Filled = false;
            bool y1Filled = false;
            bool y2Filled = false;

            Vector.MemorySafe_CartVect RHR = Vector.GetMemRHR(parentSurface.SurfaceCoords);

            if (Math.Abs(RHR.X) == 1 && RHR.Y == 0 && RHR.Z == 0)
            {
                for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
                {
                    if (j == 0)
                    {
                        x1 = parentSurface.SurfaceCoords[j].X;
                        x1Filled = true;
                        y1 = parentSurface.SurfaceCoords[j].Y;
                        y1Filled = true;
                    }
                    else
                    {
                        //because it is parallel to the Y axis
                        x2 = x1;
                        x2Filled = true;

                        if (parentSurface.SurfaceCoords[j].Y != y1)
                        {
                            y2Filled = true;
                            y2 = parentSurface.SurfaceCoords[j].Y;
                        }
                    }
                    if (x1Filled && y1Filled && x2Filled && y2Filled) break;
                }
            }
            else if (RHR.X == 0 && Math.Abs(RHR.Y) == 1 && RHR.Z == 0)
            {
                for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
                {
                    if (j == 0)
                    {
                        x1 = parentSurface.SurfaceCoords[j].X;
                        x1Filled = true;
                        y1 = parentSurface.SurfaceCoords[j].Y;
                        y1Filled = true;
                    }
                    else
                    {
                        if (parentSurface.SurfaceCoords[j].X != x1)
                        {
                            x2 = parentSurface.SurfaceCoords[j].X;
                            x2Filled = true;
                        }
                        //because parallel to the X axis
                        y2 = y1;
                        y2Filled = true;
                    }
                    if (x1Filled && y1Filled && x2Filled && y2Filled) break;
                }
            }
            else
            {
                for (int j = 0; j < parentSurface.SurfaceCoords.Count; j++)
                {
                    if (j == 0)
                    {
                        x1 = parentSurface.SurfaceCoords[j].X;
                        x1Filled = true;
                        y1 = parentSurface.SurfaceCoords[j].Y;
                        y1Filled = true;
                    }
                    else
                    {
                        if (parentSurface.SurfaceCoords[j].X != x1)
                        {
                            x2 = parentSurface.SurfaceCoords[j].X;
                            x2Filled = true;
                        }
                        if (parentSurface.SurfaceCoords[j].Y != y1)
                        {
                            y2Filled = true;
                            y2 = parentSurface.SurfaceCoords[j].Y;
                        }
                    }
                    if (x1Filled && y1Filled && x2Filled && y2Filled) break;
                }
            }

            double surfaceWidth = Math.Pow((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)), 0.5);
            return surfaceWidth;

        }

        private static double GetNormalQuadWindowWidth(ModelingUtilities.BuildingObjects.OpeningDefinitions Opening)
        {
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            bool x1Filled = false;
            bool y1Filled = false;
            bool x2Filled = false;
            bool y2Filled = false;

            for (int j = 0; j < Opening.coordinateList.Count; j++)
            {
                if (j == 0)
                {
                    x1 = Opening.coordinateList[j].X;
                    x1Filled = true;
                    y1 = Opening.coordinateList[j].Y;
                    y1Filled = true;
                }
                else
                {
                    if (Opening.coordinateList[j].X != x1)
                    {
                        x2 = Opening.coordinateList[j].X;
                        x2Filled = true;
                    }
                    if (Opening.coordinateList[j].Y != y1)
                    {
                        y2Filled = true;
                        y2 = Opening.coordinateList[j].Y;
                    }
                }
                if (x1Filled && y1Filled && x2Filled && y2Filled) break;
            }
            double windowWidth = Math.Pow((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)), 0.5);
            return windowWidth;

        }

        private static double GetNormalQuadWindowWidth(ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions Opening)
        {
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            bool x1Filled = false;
            bool y1Filled = false;
            bool x2Filled = false;
            bool y2Filled = false;

            Vector.MemorySafe_CartVect RHR = Vector.GetMemRHR(Opening.coordinateList);

            if (Math.Abs(RHR.X) == 1 && RHR.Y == 0 && RHR.Z == 0)
            {
            for (int j = 0; j < Opening.coordinateList.Count; j++)
            {
                if (j == 0)
                {
                    x1 = Opening.coordinateList[j].X;
                    x1Filled = true;
                    y1 = Opening.coordinateList[j].Y;
                    y1Filled = true;
                }
                else
                {
                    //because it is parallel to the Y axis
                    x2 = x1;
                    x2Filled = true;
                    
                    if (Opening.coordinateList[j].Y != y1)
                    {
                        y2Filled = true;
                        y2 = Opening.coordinateList[j].Y;
                    }
                }
                if (x1Filled && y1Filled && x2Filled && y2Filled) break;
            }
            }
            else if (RHR.X == 0 && Math.Abs(RHR.Y) == 1 && RHR.Z == 0)
            {
                for (int j = 0; j < Opening.coordinateList.Count; j++)
                {
                    if (j == 0)
                    {
                        x1 = Opening.coordinateList[j].X;
                        x1Filled = true;
                        y1 = Opening.coordinateList[j].Y;
                        y1Filled = true;
                    }
                    else
                    {
                        if (Opening.coordinateList[j].X != x1)
                        {
                            x2 = Opening.coordinateList[j].X;
                            x2Filled = true;
                        }
                        //because parallel to the X axis
                        y2 = y1;
                        y2Filled = true;
                    }
                    if (x1Filled && y1Filled && x2Filled && y2Filled) break;
                }
            }
            else
            {
                for (int j = 0; j < Opening.coordinateList.Count; j++)
                {
                    if (j == 0)
                    {
                        x1 = Opening.coordinateList[j].X;
                        x1Filled = true;
                        y1 = Opening.coordinateList[j].Y;
                        y1Filled = true;
                    }
                    else
                    {
                        if (Opening.coordinateList[j].X != x1)
                        {
                            x2 = Opening.coordinateList[j].X;
                            x2Filled = true;
                        }
                        if (Opening.coordinateList[j].Y != y1)
                        {
                            y2Filled = true;
                            y2 = Opening.coordinateList[j].Y;
                        }
                    }
                    if (x1Filled && y1Filled && x2Filled && y2Filled) break;
                }
            }

            double windowWidth = Math.Pow((Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)), 0.5);
            return windowWidth;

        }

        private static List<Vector.CartCoord> GetNewNormalQuadStripWindowCoordinates(ModelingUtilities.BuildingObjects.Surface parentSurface, double projectWWR, double sillHeight, double wallHeight)
        {
            double zLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentSurface.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = parentSurface.SurfaceCoords[i].Z + sillHeight;
                        zLow = parentSurface.SurfaceCoords[i].Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = parentSurface.SurfaceCoords[i].Z + sillHeight;
                        if (zLow > parentSurface.SurfaceCoords[i].Z) zLow = parentSurface.SurfaceCoords[i].Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        if (zLow > parentSurface.SurfaceCoords[i].Z) zLow = parentSurface.SurfaceCoords[i].Z;
                        newWindowCoord.Z = zLow + sillHeight + projectWWR * wallHeight;
                        break;
                    case 3:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = zLow + sillHeight + projectWWR * wallHeight;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        private static List<Vector.CartCoord> GetNewNormalQuadStripWindowCoordinates(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface, double projectWWR, double sillHeight, double wallHeight, double dlBuffer)
        {
            double zLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentSurface.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = parentSurface.SurfaceCoords[i].Z + sillHeight - dlBuffer;
                        zLow = parentSurface.SurfaceCoords[i].Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = parentSurface.SurfaceCoords[i].Z + sillHeight - dlBuffer;
                        if (zLow > parentSurface.SurfaceCoords[i].Z) zLow = parentSurface.SurfaceCoords[i].Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        if (zLow > parentSurface.SurfaceCoords[i].Z) zLow = parentSurface.SurfaceCoords[i].Z;
                        newWindowCoord.Z = zLow + sillHeight + projectWWR * wallHeight - dlBuffer;
                        break;
                    case 3:
                        newWindowCoord.X = parentSurface.SurfaceCoords[i].X;
                        newWindowCoord.Y = parentSurface.SurfaceCoords[i].Y;
                        newWindowCoord.Z = zLow + sillHeight + projectWWR * wallHeight - dlBuffer;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        //this is used for an existing quad window that needs to be redefined for Daylighting updating on the south facade
        //this will push the view window up to its highest extent, so the DL window will be at the top of the wall
        private static List<Vector.CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.OpeningDefinitions parentWindow, double visionHeight, double sillHeight, double zLow)
        {
            double windowZLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentWindow.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = zLow + sillHeight;
                        windowZLow = newWindowCoord.Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = zLow + sillHeight;
                        if (windowZLow > newWindowCoord.Z) windowZLow = newWindowCoord.Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                    case 3:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        private static List<Vector.CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions parentWindow, double visionHeight, double sillHeight, double zLow, double dlBuffer)
        {
            double windowZLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentWindow.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = zLow + sillHeight - dlBuffer;
                        windowZLow = newWindowCoord.Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = zLow + sillHeight - dlBuffer;
                        if (windowZLow > newWindowCoord.Z) windowZLow = newWindowCoord.Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight - dlBuffer;
                        break;
                    case 3:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight-dlBuffer;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        //this will just change the height of the window to whatever new height you want.  It will not 
        //check to see if it is too high or anything
        //the current sill height stays fixed.
        private static List<Vector.CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.OpeningDefinitions parentWindow, double visionHeight)
        {
            double windowZLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentWindow.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = parentWindow.coordinateList[i].Z;
                        windowZLow = parentWindow.coordinateList[i].Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = parentWindow.coordinateList[i].Z;
                        if (windowZLow > parentWindow.coordinateList[i].Z) windowZLow = parentWindow.coordinateList[i].Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                    case 3:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        private static List<Vector.CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions parentWindow, double visionHeight)
        {
            double windowZLow = 0;
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();


            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < parentWindow.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = parentWindow.coordinateList[i].Z;
                        windowZLow = parentWindow.coordinateList[i].Z;
                        break;
                    case 1:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = parentWindow.coordinateList[i].Z;
                        if (windowZLow > parentWindow.coordinateList[i].Z) windowZLow = parentWindow.coordinateList[i].Z;
                        break;
                    case 2:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                    case 3:
                        newWindowCoord.X = parentWindow.coordinateList[i].X;
                        newWindowCoord.Y = parentWindow.coordinateList[i].Y;
                        newWindowCoord.Z = windowZLow + visionHeight;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }
        
        //this is used when we have a single punched window with multiplier and we wish to break it out
        private static List<Vector.CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.OpeningDefinitions window, double windowSpacing, double windowWidth, int multiplier)
        {
            //this only works for the setup of vertices in this project

            Vector.CartVect localY = new Vector.CartVect();
            Vector.CartVect localX = new Vector.CartVect();

            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            Vector.CartVect RHRVector = Vector.GetRHR(window.coordinateList);
            RHRVector = Vector.UnitVector(RHRVector);
            if (RHRVector.X == 1 && RHRVector.Y == 0 && RHRVector.Z == 0)
            {
                localX.X = 0;
                localX.Y = 1;
                localX.Z = 0;
                //not required
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
            }
            else if (RHRVector.X == -1 && RHRVector.Y == 0 && RHRVector.Z == 0)
            {
                localX.X = 0;
                localX.Y = -1;
                localX.Z = 0;
                //not required
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
            }
            else if (RHRVector.X == 0 && RHRVector.Y == 1 && RHRVector.Z == 0)
            {
                localX.X = -1;
                localX.Y = 0;
                localX.Z = 0;
                //not required
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
            }
            else if (RHRVector.X == 0 && RHRVector.Y == -1 && RHRVector.Z == 0)
            {
                localX.X = 1;
                localX.Y = 0;
                localX.Z = 0;
                //not required 
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
            }
            else
            {
                Vector.CartVect globalReferenceX = new Vector.CartVect();
                globalReferenceX.X = 1;
                globalReferenceX.Y = 0;
                globalReferenceX.Z = 0;
                localY = Vector.CrossProduct(RHRVector, globalReferenceX);
                localY = Vector.UnitVector(localY);

                //new X axis is the localY cross the surface normal vector

                localX = Vector.CrossProduct(localY, RHRVector);
                localX = Vector.UnitVector(localX);
            }

            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < window.numVertices; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        if (multiplier == 1)
                        {
                            newWindowCoord.X = window.coordinateList[i].X;
                            newWindowCoord.Y = window.coordinateList[i].Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        else
                        {
                            newWindowCoord.X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (multiplier - 1) * localX.X;
                            newWindowCoord.Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (multiplier - 1) * localX.Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }

                        break;
                    case 1:
                        if (multiplier == 1)
                        {
                            newWindowCoord.X = window.coordinateList[i].X;
                            newWindowCoord.Y = window.coordinateList[i].Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        else
                        {
                            newWindowCoord.X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (multiplier - 1) * localX.X;
                            newWindowCoord.Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (multiplier - 1) * localX.Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        break;
                    case 2:
                        if (multiplier == 1)
                        {
                            newWindowCoord.X = window.coordinateList[i].X;
                            newWindowCoord.Y = window.coordinateList[i].Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        else
                        {
                            newWindowCoord.X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (multiplier - 1) * localX.X;
                            newWindowCoord.Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (multiplier - 1) * localX.Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        break;
                    case 3:
                        if (multiplier == 1)
                        {
                            newWindowCoord.X = window.coordinateList[i].X;
                            newWindowCoord.Y = window.coordinateList[i].Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        else
                        {
                            newWindowCoord.X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (multiplier - 1) * localX.X;
                            newWindowCoord.Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (multiplier - 1) * localX.Y;
                            newWindowCoord.Z = window.coordinateList[i].Z;
                        }
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        private static List<Vector.MemorySafe_CartCoord> GetNewNormalQuadPunchedWindowCoordinates(ModelingUtilities.BuildingObjects.MemorySafe_Surface parentSurface, 
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window, 
            double firstSpace, double windowSpacing, double windowWidth, int windowCopyCount)
        {
            //
            string cma = ",";
            //this only works for the setup of vertices in this project
            //Check the parent coordinates relative to the window coordinates to figure out how the window coordinates should be reconstructed.
            bool writeForward=false;
            
            
            Vector.CartVect localY = new Vector.CartVect();
            Vector.CartVect localX = new Vector.CartVect();
            bool isAxisPositive = false;

            List<Vector.MemorySafe_CartCoord> newCoords = new List<Vector.MemorySafe_CartCoord>();

            Vector.MemorySafe_CartVect RHRVector = Vector.GetMemRHR(window.coordinateList);
            RHRVector = Vector.UnitVector(RHRVector);
            
            string RHRComponents = RHRVector.X.ToString()+cma+RHRVector.Y.ToString()+cma+RHRVector.Z.ToString();
            makeWindowLogFile.AppendLine(RHRComponents);
            //make a new local coordinate system to shift the vertices
            if (RHRVector.X == 1 && RHRVector.Y == 0 && RHRVector.Z == 0)
            {
                isAxisPositive = true;
                makeWindowLogFile.AppendLine("Local X Axis for drawing will be positive in global coordinates");
                localX.X = 0;
                localX.Y = 1;
                localX.Z = 0;
                //not required in the case of a wall with tilt = 90
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
                //see where the original window is relative to its parent surface
                List<double> surfaceYTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in parentSurface.SurfaceCoords)
                {
                    surfaceYTraverse.Add(coord.Y);
                }
                List<double> windowYTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in window.coordinateList)
                {
                    windowYTraverse.Add(coord.Y);
                }
                writeForward = GetWindowWriteDirection(surfaceYTraverse, windowYTraverse, isAxisPositive);
                
            }
            else if (RHRVector.X == -1 && RHRVector.Y == 0 && RHRVector.Z == 0)
            {
                isAxisPositive = false;
                makeWindowLogFile.AppendLine("Local X Axis for drawing will be negative in global coordinates");
                localX.X = 0;
                localX.Y = -1;
                localX.Z = 0;
                //not required
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
                List<double> surfaceYTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in parentSurface.SurfaceCoords)
                {
                    surfaceYTraverse.Add(coord.Y);
                }
                List<double> windowYTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in window.coordinateList)
                {
                    windowYTraverse.Add(coord.Y);
                }
                writeForward = GetWindowWriteDirection(surfaceYTraverse, windowYTraverse, isAxisPositive);

            }
            else if (RHRVector.X == 0 && RHRVector.Y == 1 && RHRVector.Z == 0)
            {
                isAxisPositive = false;
                makeWindowLogFile.AppendLine("Local X Axis for drawing will be negative in global coordinates");
                localX.X = -1;
                localX.Y = 0;
                localX.Z = 0;
                //not required
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
                List<double> surfaceXTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in parentSurface.SurfaceCoords)
                {
                    surfaceXTraverse.Add(coord.X);
                }
                List<double> windowXTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in window.coordinateList)
                {
                    windowXTraverse.Add(coord.X);
                }
                writeForward = GetWindowWriteDirection(surfaceXTraverse, windowXTraverse, isAxisPositive);
            }
            else if (RHRVector.X == 0 && RHRVector.Y == -1 && RHRVector.Z == 0)
            {
                isAxisPositive = true;
                makeWindowLogFile.AppendLine("Local X Axis for drawing will be positive in global coordinates");
                localX.X = 1;
                localX.Y = 0;
                localX.Z = 0;
                //not required 
                localY.X = 0;
                localY.Y = 0;
                localY.Z = 1;
                List<double> surfaceXTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in parentSurface.SurfaceCoords)
                {
                    surfaceXTraverse.Add(coord.X);
                }
                List<double> windowXTraverse = new List<double>();
                foreach (Vector.MemorySafe_CartCoord coord in window.coordinateList)
                {
                    windowXTraverse.Add(coord.X);
                }
                writeForward = GetWindowWriteDirection(surfaceXTraverse, windowXTraverse, isAxisPositive);
            }
            //the wall is not aligned on an axis
            else
            {
                makeWindowLogFile.AppendLine("Local X Axis for drawing is not parallel to a global coordinate axis");
                Vector.CartVect globalReferenceX = new Vector.CartVect();
                globalReferenceX.X = 1;
                globalReferenceX.Y = 0;
                globalReferenceX.Z = 0;
                localY = Vector.CrossProductNVRetMSNV(RHRVector, globalReferenceX);
                localY = Vector.UnitVector(localY);

                //new X axis is the localY cross the surface normal vector

                localX = Vector.CrossProductNVRetNVMS(localY, RHRVector);
                localX = Vector.UnitVector(localX);
                //wrote a custom routine to figure this out
                writeForward = GetWindowWriteDirectionGeneral(localX,parentSurface.SurfaceCoords,window.coordinateList);
            }

            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < window.numVertices; i++)
            {
                //Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                double X;
                double Y;
                double Z;
                //assume that the coordinates both go in the same order
                if (windowCopyCount == 1)
                {
                    if (writeForward)
                    {
                        if (localX.X != 0) X = window.coordinateList[i].X + firstSpace * localX.X;
                        else X = window.coordinateList[i].X + firstSpace * localX.X;
                        if(localX.Y != 0) Y = window.coordinateList[i].Y + firstSpace * localX.Y;
                        else Y = window.coordinateList[i].Y + firstSpace * localX.Y;
                        Z = window.coordinateList[i].Z;
                        Vector.MemorySafe_CartCoord newWindowCoord = new Vector.MemorySafe_CartCoord(X, Y, Z);
                        newCoords.Add(newWindowCoord);
                    }
                    else
                    {
                        if (localX.X != 0) X = window.coordinateList[i].X - firstSpace * localX.X;
                        else X = window.coordinateList[i].X - firstSpace * localX.X;
                        if (localX.Y != 0) Y = window.coordinateList[i].Y - firstSpace * localX.Y;
                        else Y = window.coordinateList[i].Y - firstSpace * localX.Y;
                        Z = window.coordinateList[i].Z;
                        Vector.MemorySafe_CartCoord newWindowCoord = new Vector.MemorySafe_CartCoord(X, Y, Z);
                        newCoords.Add(newWindowCoord);
                    }
                }
                else
                {
                    if (writeForward)
                    {
                        //this assumes we write from the bottom left(VFO), because the local has already been defined positive or negative.
                        if (localX.X != 0)
                        {
                            X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (windowCopyCount - 1) * localX.X + firstSpace * localX.X;
                        }
                        else
                        {
                            X = window.coordinateList[i].X + (windowSpacing + windowWidth) * (windowCopyCount - 1) * localX.X + firstSpace * localX.X;
                        }
                        if (localX.Y != 0)
                        {
                            Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (windowCopyCount - 1) * localX.Y + firstSpace * localX.Y;
                        }
                        else
                        {
                            Y = window.coordinateList[i].Y + (windowSpacing + windowWidth) * (windowCopyCount - 1) * localX.Y + firstSpace * localX.Y;
                        }
                        Z = window.coordinateList[i].Z;
                        Vector.MemorySafe_CartCoord newWindowCoord = new Vector.MemorySafe_CartCoord(X, Y, Z);
                        newCoords.Add(newWindowCoord);
                        makeWindowLogFile.AppendLine("["+newWindowCoord.X+cma+newWindowCoord.Y+cma+newWindowCoord.Z+"]");
                    }
                    else
                    {
                        //this assumes that the window is to be written from the bottom right (VFO)
                        if (localX.X != 0)
                        {
                            X = window.coordinateList[i].X - (((windowSpacing + windowWidth) * (windowCopyCount - 1)) * localX.X) - firstSpace * localX.X;
                        }
                        else
                        {
                            X = window.coordinateList[i].X - (((windowSpacing + windowWidth) * (windowCopyCount - 1)) * localX.X) - firstSpace * localX.X;
                        }
                        if (localX.Y != 0)
                        {
                            Y = window.coordinateList[i].Y - (((windowSpacing + windowWidth) * (windowCopyCount - 1)) * localX.Y) - firstSpace * localX.Y;
                        }
                        else
                        {
                            Y = window.coordinateList[i].Y - (((windowSpacing + windowWidth) * (windowCopyCount - 1)) * localX.Y) - firstSpace * localX.Y;
                        }
                        Z = window.coordinateList[i].Z;
                        Vector.MemorySafe_CartCoord newWindowCoord = new Vector.MemorySafe_CartCoord(X, Y, Z);
                        newCoords.Add(newWindowCoord);
                        makeWindowLogFile.AppendLine("["+newWindowCoord.X+cma+newWindowCoord.Y+cma+newWindowCoord.Z+"]");
                    }
                }
                
                
            }
            //are these new coordinates contained inside of the surface or are they not?
            bool isWithinSurface = isWindowBound(newCoords, parentSurface.SurfaceCoords);
            if (isWithinSurface)
            {
                makeWindowLogFile.AppendLine("Window coordinate is inside the parent surface Boundary");
                return newCoords;
            }
            else
            {
                makeWindowLogFile.AppendLine("Window coordinate is not inside the parent surface Boundary");
                newCoords.Clear();
                return newCoords;
            }
        }


        private static bool GetWindowWriteDirectionGeneral(Vector.CartVect localX, List<Vector.CartCoord> surfaceCoords, List<Vector.CartCoord> windowCoords)
        {
            bool writeForward = false;
            //find the vector that is parallel to the localX

            double surfaceXMin = 0;
            double surfaceXMax = 0;
            double surfaceYMin = 0;
            double surfaceYMax = 0;
            for (int i = 0; i < surfaceCoords.Count() - 1; i++)
            {
                Vector.CartVect vector = Vector.CreateVector(surfaceCoords[i], surfaceCoords[i + 1]);
                localX = Vector.UnitVector(localX);
                vector = Vector.UnitVector(vector);
                //this means they are parallel and pointing in the same direction
                if ((Math.Abs(vector.X -localX.X)<.0001) && (Math.Abs(vector.Y - localX.Y) < .0001))
                {
                    if (localX.X > 0)
                    {
                        surfaceXMin = surfaceCoords[i].X;
                        surfaceXMax = surfaceCoords[i + 1].X;
                    }
                    else
                    {
                        surfaceXMin = surfaceCoords[i+1].X;
                        surfaceXMax = surfaceCoords[i].X;
                    }
                    if (localX.Y > 0)
                    {
                        surfaceYMin = surfaceCoords[i].Y;
                        surfaceYMax = surfaceCoords[i + 1].Y;
                    }
                    else
                    {
                        surfaceYMin = surfaceCoords[i+1].Y;
                        surfaceYMax = surfaceCoords[i].Y;
                    }
                }
            }
            double windowXMin = 0;
            double windowXMax = 0;
            double windowYMin = 0;
            double windowYMax = 0;
            for (int i = 0; i < windowCoords.Count() - 1; i++)
            {
                Vector.CartVect vector = Vector.CreateVector(windowCoords[i], windowCoords[i + 1]);
                localX = Vector.UnitVector(localX);
                vector = Vector.UnitVector(vector);
                //this means they are parallel and pointing in the same direction
                if ((Math.Abs(vector.X - localX.X) < .0001) && (Math.Abs(vector.Y-localX.Y) < 0.0001))
                {
                    //if localX axis X component is positive
                    //it must be true that the second window coordinate X value is greater than the first's
                    if (localX.X > 0)
                    {
                        windowXMin = windowCoords[i].X;
                        windowXMax = windowCoords[i + 1].X;
                    }
                    else
                    {
                        windowXMin = windowCoords[i + 1].X;
                        windowXMax = windowCoords[i].X;
                    }
                    //same for the Y value
                    if (localX.Y > 0)
                    {
                        windowYMin = windowCoords[i].Y;
                        windowYMax = windowCoords[i + 1].Y;
                    }
                    else
                    {
                        windowYMin = windowCoords[i + 1].Y;
                        windowYMax = windowCoords[i].Y;
                    }
                }
            }

            if (localX.X > 0 && localX.Y > 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMin - windowXMin),2) + Math.Pow(Math.Abs(surfaceYMin-windowYMin),2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distancetoLR = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distancetoLR = Math.Sqrt(distancetoLR);

                if (distanceToLL < distancetoLR)
                {
                    writeForward = true;
                    return writeForward;
                }
                    
            }
            else if (localX.X > 0 && localX.Y < 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }
            else if (localX.X < 0 && localX.Y < 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }
            else if (localX.X < 0 && localX.Y > 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }

            return writeForward;
        }

        private static bool GetWindowWriteDirectionGeneral(Vector.CartVect localX, List<Vector.MemorySafe_CartCoord> surfaceCoords, List<Vector.MemorySafe_CartCoord> windowCoords)
        {
            bool writeForward = false;
            //find the vector that is parallel to the localX

            double surfaceXMin = 0;
            double surfaceXMax = 0;
            double surfaceYMin = 0;
            double surfaceYMax = 0;
            for (int i = 0; i < surfaceCoords.Count() - 1; i++)
            {
                Vector.CartVect vector = Vector.CreateVector(surfaceCoords[i], surfaceCoords[i + 1]);
                localX = Vector.UnitVector(localX);
                vector = Vector.UnitVector(vector);
                //this means they are parallel and pointing in the same direction
                if ((Math.Abs(vector.X - localX.X) < .0001) && (Math.Abs(vector.Y - localX.Y) < .0001))
                {
                    if (localX.X > 0)
                    {
                        surfaceXMin = surfaceCoords[i].X;
                        surfaceXMax = surfaceCoords[i + 1].X;
                    }
                    else
                    {
                        surfaceXMin = surfaceCoords[i + 1].X;
                        surfaceXMax = surfaceCoords[i].X;
                    }
                    if (localX.Y > 0)
                    {
                        surfaceYMin = surfaceCoords[i].Y;
                        surfaceYMax = surfaceCoords[i + 1].Y;
                    }
                    else
                    {
                        surfaceYMin = surfaceCoords[i + 1].Y;
                        surfaceYMax = surfaceCoords[i].Y;
                    }
                }
            }
            double windowXMin = 0;
            double windowXMax = 0;
            double windowYMin = 0;
            double windowYMax = 0;
            for (int i = 0; i < windowCoords.Count() - 1; i++)
            {
                Vector.CartVect vector = Vector.CreateVector(windowCoords[i], windowCoords[i + 1]);
                localX = Vector.UnitVector(localX);
                vector = Vector.UnitVector(vector);
                //this means they are parallel and pointing in the same direction
                if ((Math.Abs(vector.X - localX.X) < .0001) && (Math.Abs(vector.Y - localX.Y) < 0.0001))
                {
                    //if localX axis X component is positive
                    //it must be true that the second window coordinate X value is greater than the first's
                    if (localX.X > 0)
                    {
                        windowXMin = windowCoords[i].X;
                        windowXMax = windowCoords[i + 1].X;
                    }
                    else
                    {
                        windowXMin = windowCoords[i + 1].X;
                        windowXMax = windowCoords[i].X;
                    }
                    //same for the Y value
                    if (localX.Y > 0)
                    {
                        windowYMin = windowCoords[i].Y;
                        windowYMax = windowCoords[i + 1].Y;
                    }
                    else
                    {
                        windowYMin = windowCoords[i + 1].Y;
                        windowYMax = windowCoords[i].Y;
                    }
                }
            }

            if (localX.X > 0 && localX.Y > 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distancetoLR = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distancetoLR = Math.Sqrt(distancetoLR);

                if (distanceToLL < distancetoLR)
                {
                    writeForward = true;
                    return writeForward;
                }

            }
            else if (localX.X > 0 && localX.Y < 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }
            else if (localX.X < 0 && localX.Y < 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }
            else if (localX.X < 0 && localX.Y > 0)
            {
                double distanceToLL = Math.Pow(Math.Abs(surfaceXMax - windowXMax), 2) + Math.Pow(Math.Abs(surfaceYMin - windowYMin), 2);
                distanceToLL = Math.Sqrt(distanceToLL);
                double distanceToLR = Math.Pow(Math.Abs(surfaceXMin - windowXMin), 2) + Math.Pow(Math.Abs(surfaceYMax - windowYMax), 2);
                distanceToLR = Math.Sqrt(distanceToLR);

                if (distanceToLL < distanceToLR)
                {
                    writeForward = true;
                    return writeForward;
                }
            }

            return writeForward;
        }
        //works whenever the surface is parallel to the X-Y-Z global reference frame.
        private static bool GetWindowWriteDirection(List<double> surfaceCoords, List<double> windowCoords, bool isAxisPositive)
        {
            bool writeForward = false;

            //now try and figure out whether you are going to be writing in the positive or negative direction on this new coordinate system
            //if the difference between the greater Y's is larger than the difference between the lesser Y's, then I move in positive direction.
            //or if the difference between one is less than the other, then I know which corner I am in.
            //this assumes that my window actually starts in one corner or the other.  The logice needs a re-write if I have to 
            //deal with the situation where the differences are identical, or it isn't starting in a corner at all.
            double maxSurfaceCoord = 0;
            double minSurfaceCoord = 0;
            double maxWindowCoord = 0;
            double minWindowCoord = 0;
            for (int i = 0; i < surfaceCoords.Count(); i++)
            {
                if (i == 0)
                {
                    maxSurfaceCoord = surfaceCoords[i];
                    minSurfaceCoord = surfaceCoords[i];
                    continue;
                }
                if (surfaceCoords[i] < minSurfaceCoord) minSurfaceCoord = surfaceCoords[i];
                if (surfaceCoords[i] > maxSurfaceCoord) maxSurfaceCoord = surfaceCoords[i];

            }
            for (int i = 0; i < windowCoords.Count(); i++)
            {
                if (i == 0)
                {
                    maxWindowCoord = windowCoords[i];
                    minWindowCoord = windowCoords[i];
                    continue;
                }
                if (windowCoords[i] < minWindowCoord) minWindowCoord = windowCoords[i];
                if (windowCoords[i] > maxWindowCoord) maxWindowCoord = windowCoords[i];

            }
            //write forward means that the new window coordinates are supposed to progress in the direction of the local X axis defined above.
            if (isAxisPositive)
            {
                //if the difference between the mins is less than the max, your first window is in the lower left (VFO)
                if (Math.Abs(minSurfaceCoord - minWindowCoord) < Math.Abs(maxSurfaceCoord - maxWindowCoord))
                {
                    writeForward = true;
                }
            }
            else
            {
                //if the difference between the Max is less than the Mins, you are in the lower left (VFO)
                if (Math.Abs(minSurfaceCoord - minWindowCoord) > Math.Abs(maxSurfaceCoord - maxWindowCoord))
                {
                    writeForward = true;
                }
            }

            return writeForward;
        }

        private static bool isWindowBound(List<Vector.CartCoord> windowCoordinates, List<Vector.CartCoord> parentSurfaceCoords)
        {
            bool isBound = false;
            //we check if the planes intersect and the window is inside of the confines of the parentSurfaceCoordinates
            //Points lie in the same plane if we can prove the vectors formed by these points are orthogonal, using the scalar triple product
            List<Vector.CartVect> parentSurfVect = new List<Vector.CartVect>();
            for(int i = 0; i<parentSurfaceCoords.Count()-1; i++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(parentSurfaceCoords[i], parentSurfaceCoords[i + 1]);
                parentSurfVect.Add(sV1);
            }

            List<Vector.CartVect> windowVect = new List<Vector.CartVect>();
            for (int j = 0; j< windowCoordinates.Count() - 1; j++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(windowCoordinates[j], windowCoordinates[j + 1]);
                windowVect.Add(sV1);
            }
            bool isInPlane = false;
            Vector.CartVect xProduct = Vector.CrossProduct(parentSurfVect[0], parentSurfVect[1]);
            makeWindowLogFile.AppendLine("Parent RHR Vector is ["+Math.Round(xProduct.X,4)+","+Math.Round(xProduct.Y,4)+","+Math.Round(xProduct.Z,4));
            //test the Window vectors based on this xProduct
            int windowVectorCounter = 0;
            foreach (Vector.CartVect vector in windowVect)
            {
                double tripleXProduct = vector.X*xProduct.X + vector.Y*xProduct.Y + vector.Z*xProduct.Z;
                if(Math.Abs(tripleXProduct) <= 0.0001) isInPlane = true;
                else 
                {
                    isInPlane = false;
                    makeWindowLogFile.AppendLine("Window is not in Surface Plane at window Vector: " + windowVectorCounter);
                    //isBound is false
                    return isBound;
                }
                windowVectorCounter++;
            }

            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            double minZ = 0;
            double maxZ = 0;
            for(int i = 0; i< parentSurfaceCoords.Count(); i++)
            {
                if(i==0)
                {
                    minX = maxX = parentSurfaceCoords[i].X;
                    minY = maxY = parentSurfaceCoords[i].Y;
                    minZ = maxZ = parentSurfaceCoords[i].Z;
                    continue;
                }
                if (parentSurfaceCoords[i].X < minX) minX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].X > maxX) maxX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].Y < minY) minY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Y > maxY) maxY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Z < minZ) minZ = parentSurfaceCoords[i].Z;
                if (parentSurfaceCoords[i].Z > maxZ) maxZ = parentSurfaceCoords[i].Z;
                makeWindowLogFile.AppendLine("Parent Surface Mins and Maxes:");
                string cma = ",";
                makeWindowLogFile.AppendLine(minX + cma + maxX + cma + minY + cma + maxY + cma + minZ + cma + maxZ);
            }
            for (int i = 0; i < windowCoordinates.Count(); i++)
            {
                if (i == windowCoordinates.Count() - 1)
                {
                    isBound = true;
                    makeWindowLogFile.AppendLine("Window Coordinates are Bound by the Surface Coordinates");
                    return isBound;
                }
                if (windowCoordinates[i].X < minX || windowCoordinates[i].X > maxX)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window X is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].X.ToString());
                    return isBound;
                }
                if (windowCoordinates[i].Y < minY || windowCoordinates[i].Y > maxY)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window Y is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].Y.ToString());
                    return isBound;
                }
                if (windowCoordinates[i].Z < minZ || windowCoordinates[i].Z > maxZ)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window Z is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].Z.ToString());
                    return isBound;
                }
            }
            //this return should never occur
            return isBound;
        }

        private static bool isWindowBound(List<Vector.MemorySafe_CartCoord> windowCoordinates, List<Vector.MemorySafe_CartCoord> parentSurfaceCoords)
        {
            bool isBound = false;
            //we check if the planes intersect and the window is inside of the confines of the parentSurfaceCoordinates
            //Points lie in the same plane if we can prove the vectors formed by these points are orthogonal, using the scalar triple product
            List<Vector.CartVect> parentSurfVect = new List<Vector.CartVect>();
            for (int i = 0; i < parentSurfaceCoords.Count() - 1; i++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(parentSurfaceCoords[i], parentSurfaceCoords[i + 1]);
                parentSurfVect.Add(sV1);
            }

            List<Vector.CartVect> windowVect = new List<Vector.CartVect>();
            for (int j = 0; j < windowCoordinates.Count() - 1; j++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(windowCoordinates[j], windowCoordinates[j + 1]);
                windowVect.Add(sV1);
            }
            bool isInPlane = false;
            Vector.CartVect xProduct = Vector.CrossProduct(parentSurfVect[0], parentSurfVect[1]);
            makeWindowLogFile.AppendLine("Parent RHR Vector is [" + Math.Round(xProduct.X, 4) + "," + Math.Round(xProduct.Y, 4) + "," + Math.Round(xProduct.Z, 4));
            //test the Window vectors based on this xProduct
            int windowVectorCounter = 0;
            foreach (Vector.CartVect vector in windowVect)
            {
                double tripleXProduct = vector.X * xProduct.X + vector.Y * xProduct.Y + vector.Z * xProduct.Z;
                if (Math.Abs(tripleXProduct) <= 0.0001) isInPlane = true;
                else
                {
                    isInPlane = false;
                    makeWindowLogFile.AppendLine("Window is not in Surface Plane at window Vector: " + windowVectorCounter);
                    //isBound is false
                    return isBound;
                }
                windowVectorCounter++;
            }

            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            double minZ = 0;
            double maxZ = 0;
            for (int i = 0; i < parentSurfaceCoords.Count(); i++)
            {
                if (i == 0)
                {
                    minX = maxX = parentSurfaceCoords[i].X;
                    minY = maxY = parentSurfaceCoords[i].Y;
                    minZ = maxZ = parentSurfaceCoords[i].Z;
                    continue;
                }
                if (parentSurfaceCoords[i].X < minX) minX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].X > maxX) maxX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].Y < minY) minY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Y > maxY) maxY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Z < minZ) minZ = parentSurfaceCoords[i].Z;
                if (parentSurfaceCoords[i].Z > maxZ) maxZ = parentSurfaceCoords[i].Z;
                makeWindowLogFile.AppendLine("Parent Surface Mins and Maxes:");
                string cma = ",";
                makeWindowLogFile.AppendLine(minX + cma + maxX + cma + minY + cma + maxY + cma + minZ + cma + maxZ);
            }
            for (int i = 0; i < windowCoordinates.Count(); i++)
            {
                if (i == windowCoordinates.Count() - 1)
                {
                    isBound = true;
                    makeWindowLogFile.AppendLine("Window Coordinates are Bound by the Surface Coordinates");
                    return isBound;
                }
                if (windowCoordinates[i].X < minX)
                {
                    if (Math.Abs(windowCoordinates[i].X - minX) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window X is out of bounds.  Too small");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].X.ToString());
                        return isBound;
                    }
                }
                if (windowCoordinates[i].X > maxX)
                {
                    if (Math.Abs(windowCoordinates[i].X - maxX) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window X is out of bounds.  Too large");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].X.ToString());
                        return isBound;
                    }
                }
                if (windowCoordinates[i].Y < minY)
                {
                    if (Math.Abs(windowCoordinates[i].Y - minY) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window Y is out of bounds.  Too large");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].Y.ToString());
                        return isBound;
                    }
                }
                if (windowCoordinates[i].Y > maxY)
                {
                    if (Math.Abs(windowCoordinates[i].Y - maxY) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window Y is out of bounds.  Too small.");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].Y.ToString());
                        return isBound;
                    }
                }
                if (windowCoordinates[i].Z < minZ)
                {
                    if (Math.Abs(windowCoordinates[i].Z - minZ) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window Z is out of bounds.  Too large.");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].Z.ToString());
                        return isBound;
                    }
                }
                if (windowCoordinates[i].Z > maxZ)
                {
                    if (Math.Abs(windowCoordinates[i].Z - maxZ) > 0.000001)
                    {
                        isBound = false;
                        makeWindowLogFile.AppendLine("Window Z is out of bounds.  Too small.");
                        makeWindowLogFile.AppendLine(windowCoordinates[i].Z.ToString());
                        return isBound;
                    }
                }
            }
            //this return should never occur
            return isBound;
        }

        private static bool isWindowBound(List<Vector.CartCoord> windowCoordinates, List<Vector.MemorySafe_CartCoord> parentSurfaceCoords)
        {
            bool isBound = false;
            //we check if the planes intersect and the window is inside of the confines of the parentSurfaceCoordinates
            //Points lie in the same plane if we can prove the vectors formed by these points are orthogonal, using the scalar triple product
            List<Vector.CartVect> parentSurfVect = new List<Vector.CartVect>();
            for (int i = 0; i < parentSurfaceCoords.Count() - 1; i++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(parentSurfaceCoords[i], parentSurfaceCoords[i + 1]);
                parentSurfVect.Add(sV1);
            }

            List<Vector.CartVect> windowVect = new List<Vector.CartVect>();
            for (int j = 0; j < windowCoordinates.Count() - 1; j++)
            {
                Vector.CartVect sV1 = Vector.CreateVector(windowCoordinates[j], windowCoordinates[j + 1]);
                windowVect.Add(sV1);
            }
            bool isInPlane = false;
            Vector.CartVect xProduct = Vector.CrossProduct(parentSurfVect[0], parentSurfVect[1]);
            makeWindowLogFile.AppendLine("Parent RHR Vector is [" + Math.Round(xProduct.X, 4) + "," + Math.Round(xProduct.Y, 4) + "," + Math.Round(xProduct.Z, 4));
            //test the Window vectors based on this xProduct
            int windowVectorCounter = 0;
            foreach (Vector.CartVect vector in windowVect)
            {
                double tripleXProduct = vector.X * xProduct.X + vector.Y * xProduct.Y + vector.Z * xProduct.Z;
                if (Math.Abs(tripleXProduct) <= 0.0001) isInPlane = true;
                else
                {
                    isInPlane = false;
                    makeWindowLogFile.AppendLine("Window is not in Surface Plane at window Vector: " + windowVectorCounter);
                    //isBound is false
                    return isBound;
                }
                windowVectorCounter++;
            }

            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            double minZ = 0;
            double maxZ = 0;
            for (int i = 0; i < parentSurfaceCoords.Count(); i++)
            {
                if (i == 0)
                {
                    minX = maxX = parentSurfaceCoords[i].X;
                    minY = maxY = parentSurfaceCoords[i].Y;
                    minZ = maxZ = parentSurfaceCoords[i].Z;
                    continue;
                }
                if (parentSurfaceCoords[i].X < minX) minX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].X > maxX) maxX = parentSurfaceCoords[i].X;
                if (parentSurfaceCoords[i].Y < minY) minY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Y > maxY) maxY = parentSurfaceCoords[i].Y;
                if (parentSurfaceCoords[i].Z < minZ) minZ = parentSurfaceCoords[i].Z;
                if (parentSurfaceCoords[i].Z > maxZ) maxZ = parentSurfaceCoords[i].Z;
                makeWindowLogFile.AppendLine("Parent Surface Mins and Maxes:");
                string cma = ",";
                makeWindowLogFile.AppendLine(minX + cma + maxX + cma + minY + cma + maxY + cma + minZ + cma + maxZ);
            }
            for (int i = 0; i < windowCoordinates.Count(); i++)
            {
                if (i == windowCoordinates.Count() - 1)
                {
                    isBound = true;
                    makeWindowLogFile.AppendLine("Window Coordinates are Bound by the Surface Coordinates");
                    return isBound;
                }
                if (windowCoordinates[i].X < minX || windowCoordinates[i].X > maxX)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window X is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].X.ToString());
                    return isBound;
                }
                if (windowCoordinates[i].Y < minY || windowCoordinates[i].Y > maxY)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window Y is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].Y.ToString());
                    return isBound;
                }
                if (windowCoordinates[i].Z < minZ || windowCoordinates[i].Z > maxZ)
                {
                    isBound = false;
                    makeWindowLogFile.AppendLine("Window Z is out of bounds");
                    makeWindowLogFile.AppendLine(windowCoordinates[i].Z.ToString());
                    return isBound;
                }
            }
            //this return should never occur
            return isBound;
        }
        private static List<Vector.CartCoord> GetNewDLUpperQuadPunchedWindowCoordinates(List<Vector.CartCoord> viewOpeningCoords, double visionHeight, double dlHeight)
        {
            
            List<Vector.CartCoord> newCoords = new List<Vector.CartCoord>();
            
            //this seems to work when the starting vertex is in the lower half of the window
            //it may change when it is in the upper portion of the window
            for (int i = 0; i < viewOpeningCoords.Count; i++)
            {
                Vector.CartCoord newWindowCoord = new Vector.CartCoord();
                //assume that the coordinates both go in the same order
                switch (i)
                {
                    case 0:
                        newWindowCoord.X = viewOpeningCoords[i].X;
                        newWindowCoord.Y = viewOpeningCoords[i].Y;
                        newWindowCoord.Z = viewOpeningCoords[i].Z + visionHeight;
                        break;
                    case 1:
                        newWindowCoord.X = viewOpeningCoords[i].X;
                        newWindowCoord.Y = viewOpeningCoords[i].Y;
                        newWindowCoord.Z = viewOpeningCoords[i].Z + visionHeight;
                        break;
                    case 2:
                        newWindowCoord.X = viewOpeningCoords[i].X;
                        newWindowCoord.Y = viewOpeningCoords[i].Y;
                        newWindowCoord.Z = viewOpeningCoords[i].Z + dlHeight;
                        break;
                    case 3:
                        newWindowCoord.X = viewOpeningCoords[i].X;
                        newWindowCoord.Y = viewOpeningCoords[i].Y;
                        newWindowCoord.Z = viewOpeningCoords[i].Z + dlHeight;
                        break;
                }

                newCoords.Add(newWindowCoord);
            }
            return newCoords;
        }

        public static ModelingUtilities.BuildingObjects.MemorySafe_Surface GetParentSurface(string parentSurfaceId, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            string name = "";
            int multiplier = 0;
            ModelingUtilities.BuildingObjects.SurfaceTypes SurfaceType = ModelingUtilities.BuildingObjects.SurfaceTypes.Blank;
            string constName = "";
            ModelingUtilities.BuildingObjects.OutsideBoundary ob = ModelingUtilities.BuildingObjects.OutsideBoundary.Blank;
            string zoneName = "";
            string outsideBC = "";
            string sunExp = "";
            string windExp = "";
            double vF = 0;
            int numVert = 0;
            List<Vector.MemorySafe_CartCoord> sc = new List<Vector.MemorySafe_CartCoord>();
            double tilt = 0;
            double az = 0;
            ModelingUtilities.BuildingObjects.MemorySafe_Surface emptySurface = new ModelingUtilities.BuildingObjects.MemorySafe_Surface(name, multiplier,
                SurfaceType, constName, ob, zoneName, outsideBC, sunExp, windExp, vF, numVert, sc, tilt, az);

            foreach (ModelingUtilities.BuildingObjects.MemorySafe_Surface surface in projectSurfaces)
            {
                if (parentSurfaceId == surface.name)
                {
                    return surface;
                }
            }
            return emptySurface;
        }

        public static ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions GetOpeningfromStringId(string openingName, List<ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions> projectOpenings) 
        {
            string nameId = "";
            string openType = "";
            string parentSurf = "";
            double parentAz = 0;
            double parentTilt=0;
            string oBCond = "";
            double vF = 0; 
            string shadeCntrlSch = "";
            List<Vector.MemorySafe_CartCoord> coordList = new List<Vector.MemorySafe_CartCoord>();
            double az = 0;
            double tilt = 0;
            double X = 0;
            double Y = 0;
            double Z = 0;
            Vector.MemorySafe_CartVect RHR = new Vector.MemorySafe_CartVect(X,Y,Z);
            string constName = "";
            string frameandDivider = "";
            int mult = 0;
            int numVert = 0;
            double area = 0;
            ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions emptyOpening = new ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions(nameId, openType, parentSurf,
                parentAz, parentTilt, oBCond, vF, shadeCntrlSch, coordList, az, tilt, RHR, constName, frameandDivider, mult, numVert, area);

            foreach (ModelingUtilities.BuildingObjects.MemorySafe_OpeningDefinitions window in projectOpenings)
            {
                if (window.nameId == openingName)
                {
                    return window;
                    
                }
            }
            return emptyOpening;
                                                
        }



        internal static bool UpdateRadSurfaceConstructions(string tempFileLocation,List<EPlusObjects.RadiantSurfaceGroup> surfaceGroups, List<ModelingUtilities.BuildingObjects.MemorySafe_Surface> projectSurfaces)
        {
            bool successfulUpdate = false;
            Dictionary<string,string> radiantSurfaces = new Dictionary<string,string>();
            string currentSurfaceName = "";
            try
            {
                foreach (EPlusObjects.RadiantSurfaceGroup surfaceGroup in surfaceGroups)
                {
                    foreach (KeyValuePair<string, double> pair in surfaceGroup.surfaceNameAndFF)
                    {
                        string surfaceName = pair.Key;
                        ModelingUtilities.BuildingObjects.MemorySafe_Surface surface = EPlusFunctions.GetSurface(surfaceName, projectSurfaces);
                        string outsideBoundaryConditionObject = surface.outsideBoundaryCondition;
                        ModelingUtilities.BuildingObjects.MemorySafe_Surface neighboringSurface =
                            EPlusFunctions.GetSurface(outsideBoundaryConditionObject, projectSurfaces);
                        //store the radiant surfaces that are used in the project
                        radiantSurfaces.Add(surface.name, neighboringSurface.name);
                    }

                }
                //set up a stringbuilder
                StringBuilder output = new StringBuilder();
                //make the regex objects
                Regex detailedSurfaceYes = new Regex(EPlusObjects.EPlusRegexString.startSurface);
                Regex detailedFenestrationYes = new Regex(EPlusObjects.EPlusRegexString.startFenestration);
                Regex detailedObjectNameRegex = new Regex(EPlusObjects.EPlusRegexString.Name);
                Regex constructionNameRegex = new Regex(EPlusObjects.EPlusRegexString.constructionName);
                Encoding encoding;


                string line;
                //ModelingUtilities.BuildingObjects.MemorySafe_Surface currentSurface = new ModelingUtilities.BuildingObjects.Surface();
                bool detailedSurfaceBool = false;
                bool radSurfaceYes = false;

                using (StreamReader reader = new StreamReader(tempFileLocation))
                {
                    encoding = reader.CurrentEncoding;
                    //set up the surface
                    while ((line = reader.ReadLine()) != null)
                    {
                        //try to match for a detailed surface or 

                        Match detailedSurfaceYesRegex = detailedSurfaceYes.Match(line);
                        if (detailedSurfaceYesRegex.Success)
                        {
                            detailedSurfaceBool = true;
                            output.AppendLine(line);
                            continue;
                        }

                        else if (detailedSurfaceBool)
                        {
                            //temporary objects
                            
                            Match detailedObjectNameMatch = detailedObjectNameRegex.Match(line);
                            if (detailedObjectNameMatch.Success)
                            {
                                

                                string purify = @"(?'ws'\s*)(?'goods'.*)(?'comma',)";
                                Regex purifyRegex = new Regex(purify);
                                Match pure = purifyRegex.Match(detailedObjectNameMatch.Groups["1"].Value);
                                if (pure.Success)
                                {
                                    currentSurfaceName = pure.Groups["goods"].Value;
                                }

                                
                                if(radiantSurfaces.ContainsKey(currentSurfaceName) || radiantSurfaces.ContainsValue(currentSurfaceName))
                                {
                                    radSurfaceYes = true;
                                }
                                if (!radSurfaceYes)
                                {
                                    detailedSurfaceBool = false;
                                    output.AppendLine(line);
                                    continue;
                                }
                                else
                                {
                                    output.AppendLine(line);
                                    continue;
                                }
                            }
                            ModelingUtilities.BuildingObjects.MemorySafe_Surface currentSurface = GetSurface(currentSurfaceName, projectSurfaces);
                            Match constructionNameMatch = constructionNameRegex.Match(line);
                            if (constructionNameMatch.Success)
                            {
                                if (currentSurface.surfaceType == ModelingUtilities.BuildingObjects.SurfaceTypes.Roof && radSurfaceYes)
                                {
                                    string tempLine = "    21,                      !- Construction Name";
                                    output.AppendLine(tempLine);
                                    detailedSurfaceBool = false;
                                    radSurfaceYes = false;
                                    continue;
                                }
                                else if (currentSurface.surfaceType == ModelingUtilities.BuildingObjects.SurfaceTypes.Ceiling && radSurfaceYes)
                                {
                                    string tempLine = "    18,                      !- Construction Name";
                                    output.AppendLine(tempLine);
                                    detailedSurfaceBool = false;
                                    radSurfaceYes = false;
                                    continue;
                                }
                                else if (currentSurface.surfaceType == ModelingUtilities.BuildingObjects.SurfaceTypes.Floor && radSurfaceYes)
                                {
                                    string tempLine = "    17,                      !- Construction Name";
                                    output.AppendLine(tempLine);
                                    detailedSurfaceBool = false;
                                    radSurfaceYes = false;
                                    continue;
                                }
                                else
                                {
                                    output.AppendLine(line);
                                }
                            }
                            else
                            {
                                output.AppendLine(line);
                            }
                        }
                        else
                        {
                            output.AppendLine(line);
                        }
                    }
                }
                successfulUpdate = FileMng.UpdateTmpFile(tempFileLocation, output, false, encoding);
            }
            catch (Exception e)
            {

            }
            return successfulUpdate;
        }
    }
}

                        #endregion    

               