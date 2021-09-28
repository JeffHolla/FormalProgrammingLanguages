using System.Collections.Generic;

namespace Formal_Languages_Task_1_KDA
{
    public static class KNA_Manager
    {
        public static List<KNA> KNAs { get; set; }

        static KNA_Manager()
        {
            KNAs = new List<KNA>();
        }

        public static void Add(KNA kna)
        {
            KNAs.Add(kna);
        }

        public static void StartAll(List<int> inputSeq)
        {
            KNAs.RemoveAt(0);

            while (KNAs.Count != 0)
            {
                KNAs[0].Start(inputSeq);
                KNAs.RemoveAt(0);
            }
        }

        public static void StartAll_KNAs_With_E(List<int> inputSeq)
        {
            KNAs.RemoveAt(0);

            while (KNAs.Count != 0)
            {
                if (KNAs[0] is KNA_With_E)
                {
                    ((KNA_With_E)KNAs[0]).Start(inputSeq);
                }
                KNAs.RemoveAt(0);
            }
        }
    }
}
