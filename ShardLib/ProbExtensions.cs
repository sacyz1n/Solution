namespace ShareLib
{
    /// <summary>
    /// 확률 항목을 나타내는 클래스
    /// </summary>
    public class ProbItem : IComparable<int>
    {
        public static readonly ProbItem Null = new ProbItem(-1, 0, 0);

        public int Index { get; }
        public int OwnProb { get; }
        public int MinProb { get; }
        public int MaxProb { get; }

        public ProbItem(int index, int ownProb, int totalProb)
        {
            Index = index;

            OwnProb = ownProb;
            MinProb = totalProb + 1;
            MaxProb = totalProb + ownProb;
        }

        public int CompareTo(int selectProb)
        {
            if (MinProb > selectProb) return 1;
            if (MaxProb < selectProb) return -1;
            return 0;
        }
    }

    /// <summary>
    /// 확률 관련 확장 메서드를 제공하는 클래스
    /// </summary>
    public static class ProbExtensions
    {
        public static bool IsSuccess(this Random random, int prob, int rate = 10000)
        {
            return random.Next(0, rate) < prob;
        }

        public static void AddProb(this List<ProbItem> list, int index, int ownProb, ref int totalProb)
        {
            list.Add(new ProbItem(index, ownProb, totalProb));
            totalProb += ownProb;
        }

        public static ProbItem SelectProb(this List<ProbItem> probList, Random random, int totalProb = 0)
        {
            if (totalProb == 0)
            {
                totalProb = probList.Last().MaxProb;
                if (totalProb == 0) return ProbItem.Null;
            }

            return BinarySearch(probList, random.Next(1, totalProb + 1));
        }

        public static ProbItem SelectProb(this List<ProbItem> probList, int value, int totalProb = 0)
        {
            if (totalProb == 0)
            {
                totalProb = probList.Last().MaxProb;
                if (totalProb == 0) return ProbItem.Null;
            }

            if (totalProb < value)
                return ProbItem.Null;

            return BinarySearch(probList, value);
        }

        public static List<int> GetSelectedDataIndexes(this List<ProbItem> probList, int count, bool overlapCheck, bool doSort)
        {
            return GetSelectedDataIndexes(probList, count, RandomNumber.Instance, overlapCheck, doSort);
        }

        public static List<int> GetSelectedDataIndexes(this List<ProbItem> probList, int count, Random random, bool overlapCheck, bool doSort)
        {
            var ret = new List<int>();

            if (overlapCheck == true && count > 1)
            {
                int maxProb = probList.Last().MaxProb;
                for (int i = 0; i < count; ++i)
                {
                    int prob = random.Next(0, maxProb);
                    ProbItem currentItem = null;
                    for (int probIdx = 0; probIdx < probList.Count; ++probIdx)
                    {
                        currentItem = probList[probIdx];

                        if (ret.Contains(currentItem.Index) == true)
                            continue;

                        if (currentItem.OwnProb > prob)
                        {
                            ret.Add(currentItem.Index);
                            maxProb -= currentItem.OwnProb;
                            break;
                        }

                        prob -= currentItem.OwnProb;
                    }

                }
            }
            else
            {
                for (int i = 0; i < count; ++i)
                    ret.Add(GetSelectedDataIndex(probList, random));
            }

            if (doSort == true)
                ret.Sort();

            return ret;
        }

        public static int GetSelectedDataIndex(this List<ProbItem> probList)
        {
            return GetSelectedDataIndex(probList, RandomNumber.Instance);
        }

        public static int GetSelectedDataIndex(this List<ProbItem> probList, int value)
        {
            var selected = SelectProb(probList, value);
            return selected.Index;
        }

        public static int GetSelectedDataIndex(this List<ProbItem> probList, Random random)
        {
            var selected = SelectProb(probList, random);
            return selected.Index;
        }

        private static ProbItem BinarySearch(this List<ProbItem> ranges, int value)
        {
            int min = 0;
            int max = ranges.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) >> 1;
                int comparison = ranges[mid].CompareTo(value);
                if (comparison == 0) return ranges[mid];
                if (comparison < 0) min = mid + 1;
                else max = mid - 1;
            }
            return ProbItem.Null;
        }
    }
}