namespace ItemEditor
{
    public class ItemEntry
    {
        public uint   ID            { get; set; }
        public string SpriteName    { get; set; }
        public byte   Class         { get; set; }
        public byte   Proficiency   { get; set; }
        public byte   Level         { get; set; }
        public byte   Gender        { get; set; }
        public ushort ReqStrength   { get; set; }
        public ushort ReqAgility    { get; set; }
        public uint   Col8          { get; set; }
        public uint   Col9          { get; set; }
        public uint   Type          { get; set; }
        public uint   Weight        { get; set; }
        public uint   BuyPrice      { get; set; }
        public uint   SellPrice     { get; set; }
        public ushort MaxAttack     { get; set; }
        public ushort MinAttack     { get; set; }
        public ushort PhyDefense    { get; set; }
        public ushort Frequency     { get; set; }
        public byte   Dodge         { get; set; }
        public ushort ItemHP        { get; set; }
        public ushort ItemMP        { get; set; }
        public ushort Durability    { get; set; }
        public ushort MaxDurability { get; set; }
        public uint   Col23         { get; set; }
        public uint   Col24         { get; set; }
        public uint   Col25         { get; set; }
        public uint   Col26         { get; set; }
        public uint   Col27         { get; set; }
        public uint   Col28         { get; set; }
        public ushort MagicAttack   { get; set; }
        public ushort MagicDefense  { get; set; }
        public ushort AttackRange   { get; set; }
        public uint   Col32         { get; set; }
        public uint   Col33         { get; set; }
        public uint   Col34         { get; set; }
        public uint   Col35         { get; set; }
        public uint   CPWorth       { get; set; }
        public string DisplayName   { get; set; }
        public string ItemSet       { get; set; }
        public byte   Quality       { get; set; }

        // Extra columns if present
        public string ExtraData     { get; set; } = "";

        public static ItemEntry Parse(string line)
        {
            var parts = line.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 39) return null;

            var e = new ItemEntry();
            try
            {
                e.ID            = uint.Parse(parts[0]);
                e.SpriteName    = parts[1];
                e.Class         = byte.Parse(parts[2]);
                e.Proficiency   = byte.Parse(parts[3]);
                e.Level         = byte.Parse(parts[4]);
                e.Gender        = byte.Parse(parts[5]);
                e.ReqStrength   = ushort.Parse(parts[6]);
                e.ReqAgility    = ushort.Parse(parts[7]);
                e.Col8          = uint.Parse(parts[8]);
                e.Col9          = uint.Parse(parts[9]);
                e.Type          = uint.Parse(parts[10]);
                e.Weight        = uint.Parse(parts[11]);
                e.BuyPrice      = uint.Parse(parts[12]);
                e.SellPrice     = uint.Parse(parts[13]);
                e.MaxAttack     = ushort.Parse(parts[14]);
                e.MinAttack     = ushort.Parse(parts[15]);
                e.PhyDefense    = ushort.Parse(parts[16]);
                e.Frequency     = ushort.Parse(parts[17]);
                e.Dodge         = byte.Parse(parts[18]);
                e.ItemHP        = ushort.Parse(parts[19]);
                e.ItemMP        = ushort.Parse(parts[20]);
                e.Durability    = ushort.Parse(parts[21]);
                e.MaxDurability = ushort.Parse(parts[22]);
                e.Col23         = uint.Parse(parts[23]);
                e.Col24         = uint.Parse(parts[24]);
                e.Col25         = uint.Parse(parts[25]);
                e.Col26         = uint.Parse(parts[26]);
                e.Col27         = uint.Parse(parts[27]);
                e.Col28         = uint.Parse(parts[28]);
                e.MagicAttack   = ushort.Parse(parts[29]);
                e.MagicDefense  = ushort.Parse(parts[30]);
                e.AttackRange   = ushort.Parse(parts[31]);
                e.Col32         = uint.Parse(parts[32]);
                e.Col33         = uint.Parse(parts[33]);
                e.Col34         = uint.Parse(parts[34]);
                e.Col35         = uint.Parse(parts[35]);
                e.CPWorth       = uint.Parse(parts[36]);
                e.DisplayName   = parts[37];
                e.ItemSet       = parts[38];
                e.Quality       = parts.Length > 39 ? byte.Parse(parts[39]) : (byte)0;

                if (parts.Length > 40)
                {
                    e.ExtraData = string.Join(" ", parts, 40, parts.Length - 40);
                }
            }
            catch { return null; }

            return e;
        }

        public string ToLine()
        {
            string line = $"{ID} {SpriteName} {Class} {Proficiency} {Level} {Gender} " +
                          $"{ReqStrength} {ReqAgility} {Col8} {Col9} {Type} {Weight} " +
                          $"{BuyPrice} {SellPrice} {MaxAttack} {MinAttack} {PhyDefense} " +
                          $"{Frequency} {Dodge} {ItemHP} {ItemMP} {Durability} {MaxDurability} " +
                          $"{Col23} {Col24} {Col25} {Col26} {Col27} {Col28} " +
                          $"{MagicAttack} {MagicDefense} {AttackRange} " +
                          $"{Col32} {Col33} {Col34} {Col35} {CPWorth} " +
                          $"{DisplayName} {ItemSet} {Quality}";

            if (!string.IsNullOrEmpty(ExtraData))
                line += " " + ExtraData;

            return line;
        }
    }
}
