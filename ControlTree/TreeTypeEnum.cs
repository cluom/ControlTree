using Netcode;

namespace ControlTree
{
    public class TreeTypeEnum
    {
        public NetString Id { get; private set; }

        private TreeTypeEnum(string id)
        {
            Id = new(id);
        }

        // "1" : oak tree : 橡树
        public static readonly TreeTypeEnum Oak = new("1");

        // "2" : maple tree : 枫树
        public static readonly TreeTypeEnum Maple = new("2");

        // "3" : pine tree : 松树
        public static readonly TreeTypeEnum Pine = new("3");

        // "8" : mahogany tree : 桃花心木
        public static readonly TreeTypeEnum Mahogany = new("8");

        // "10": green rain type 1 tree : 苔藓树1
        public static readonly TreeTypeEnum GreenRainType1 = new("10");

        // "11" : green rain type 2 tree : 苔藓树2
        public static readonly TreeTypeEnum GreenRainType2 = new("11");

        // "12" : green rain type 3 tree : 蕨树
        public static readonly TreeTypeEnum GreenRainType3 = new("12");

        // "13" : mystic tree : 神秘树
        public static readonly TreeTypeEnum Mystic = new("13");
    }
}
