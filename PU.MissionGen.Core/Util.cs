namespace PU.MissionGen.Core
{
    public static class Util
    {
        public static int MakeOdd(int val)
        {
            if(val % 2 == 0)
            {
                return val + 1;
            }

            return val;
        }
    }
}
