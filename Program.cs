using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//İçimi Demirağ - 191180033

namespace Hashing
{
    //This node is for reisch hash
    class Node
    {
        public int data;
        public int link;
        public Node()
        {
            data = -1;
            link = -1;
        }
        public Node(int data)
        {
            this.data = data;
            this.link = -1;
        }
    }
    class REISCH
    {
        private int rowCount { get; set; }
        public List<int> blankIndexes { get; set; }
        public Node[] table { get; set; }

        public REISCH(int rowCount)
        {
            this.rowCount = rowCount;
            blankIndexes = Enumerable.Range(0, rowCount).ToList<int>();
            table = new Node[rowCount];
        }

        public void insert(int data)
        {
            int index = hash(data);
            //If the home index is blank,then inserts the data to the home index
            if (table[index] == null)
            {
                table[index] = new Node(data);
                blankIndexes.Remove(index);
            }
            else
            {
                if(blankIndexes.Count > 0)
                {
                    int temp = new Random().Next(0,blankIndexes.Count);
                    int blankIndex = blankIndexes[temp];
                    
                    if(table[index].link == -1)
                    {
                        table[index].link = blankIndex;
                        table[blankIndex] = new Node(data);
                        blankIndexes.Remove(blankIndex);
                    }
                    else
                    {
                        int link = table[index].link;
                        table[index].link = blankIndex;
                        table[blankIndex] = new Node(data);
                        table[blankIndex].link = link;
                        blankIndexes.Remove(blankIndex);
                    } //Iterates through hash function until an empty index is found.
                }
                else
                {
                    Console.WriteLine("Reisch table is full.");
                }
            }
        }

        public int[] search(int data){
            int probe = 1;
            int index = hash(data);

            //If the home index is blank, it means the data is not in the table
            if(table[index] == null)
            {
                return new int[] { -1, 0 };
            }
            else
            {
                //If it is the same data as the data found in the home index, it means the index is home index and probe number is 1.
                if(table[index].data == data)
                {
                    return new int[] { index, probe };
                }
                else
                {
                    Node main = table[index]; //start point

                    int link = table[index].link;
                    while(link != -1)
                    {
                        probe++;
                        if(table[link] == null)
                        {
                            return new int[] { -1, 0 };
                        }
                        if(table[link].data == data)
                        {
                            return new int[] { link, probe };
                        }
                        else if(table[link] == main)
                        {
                            return new int[] { -1, 0 };
                        }
                        else
                        {
                            link = table[link].link;
                        }
                    } //Iterates through hashes until data is found.

                    return new int[] { -1, 0 }; //If the data is not found, it returns the index is -1 and probe number is 0.
                }
            }
        } //The returned value is the index and probe number.

        public void print()
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("| Index | Record | Link |");
            Console.WriteLine("-------------------------");
            for (int i = 0; i < rowCount; i++)
            {
                if (table[i] != null && table[i].link != -1)
                    Console.WriteLine("|   {0}   |   {1}   |   {2}   |", i, table[i].data, table[i].link);

                else if (table[i] == null)
                    Console.WriteLine("|   {0}   |   -   |   -   |", i);

                else
                    Console.WriteLine("|   {0}   |   {1}   |   -   |", i, table[i].data);
            }
            Console.WriteLine("-------------------------");
        }

        private int hash(int data) { 
            return data % rowCount; 
        }
    }
    //This node is for binary hash
    class TNode
    {
         public int data { get; set; }
         public int index { get; set; }
         public bool isRight { get; set; }
         public TNode parent { get; set; }

        public TNode()
        {
            data = -1;
            index = -1;
        }
        public TNode(int data, int index)
        {
            this.data = data;
            this.index = index;
        }
    }
    class BINARY{
        private int rowCount { get; set; }
        public List<int> blankIndexes { get; set; }
        public TNode[] table { get; set; }

        public BINARY(int rowCount)
        {
            this.rowCount = rowCount;
            blankIndexes = Enumerable.Range(0, rowCount).ToList<int>();
            table = new TNode[rowCount];
        }

        public void insert(int data)
        {
            List<TNode> tempTable = new List<TNode>();
            int index = hash(data);
            int index2 = quotient(data);

            bool isRight;
            bool isFound = true;

            TNode node = new TNode(data, -1);
            int foundedIndex = -1;

            //If the home index is blank,then inserts the data to the home index
            if (table[index] == null)
            {
                table[index] = new TNode(data, index);;
                blankIndexes.Remove(index);
            }
            else
            {
                if(blankIndexes.Count > 0)
                {
                    TNode path = null;

                    tempTable.Add(node);
                    
                    TNode parent = table[index];
                    parent.parent = node;
                    parent.isRight = true;
                    tempTable.Add(parent);
                    int newIndex = (index + index2) % rowCount;
                    if(table[newIndex] != null){
                        TNode child = new TNode(table[newIndex].data, table[newIndex].index);
                        child.parent = parent;
                        child.isRight = false;
                        tempTable.Add(child);
                    }else{
                        isFound = false;
                        foundedIndex = newIndex;
                        TNode child = new TNode(-1, newIndex);
                        child.parent = parent;
                        child.isRight = false;
                        tempTable.Add(child);
                        path = child;
                        blankIndexes.Remove(foundedIndex);
                    }


                    while(isFound){

                        if(tempTable.Count % 2 != 0){
                            isRight = true;
                        }
                        else{
                            isRight = false;
                        }

                        int x = Convert.ToInt32(Math.Floor(Convert.ToDouble(tempTable.Count / 2)));
                        TNode temp = tempTable[x];
                        int tempIndex;
                        if(isRight){
                            tempIndex = quotient(temp.data);
                            tempIndex = (temp.index + tempIndex) % rowCount;
                        }
                        else{
                            TNode temp2 = tempTable[x];
                            while(!temp2.isRight){
                                temp2 = temp2.parent;
                            }
                            tempIndex = quotient(temp2.parent.data);
                            tempIndex = (temp.index + tempIndex) % rowCount;

                        }
                        if(table[tempIndex] != null){
                            TNode child = new TNode(table[tempIndex].data, table[tempIndex].index);
                            child.parent = temp;
                            child.isRight = isRight;
                            tempTable.Add(child);
                        } else{
                            isFound = false;
                            foundedIndex = tempIndex;
                            TNode child = new TNode(-1, tempIndex);
                            child.parent = temp;
                            child.isRight = isRight;
                            tempTable.Add(child);
                            path = child;
                            blankIndexes.Remove(foundedIndex);
                        }

                    } //Depending on whether isRight is true or false, the last element of the tempTable will be processed.
                    
                    while(path.index != -1){
                        if(path.isRight){ //move
                            TNode temp = new TNode(path.parent.data, foundedIndex);
                            table[foundedIndex] = temp;
                            foundedIndex = path.parent.index;
                            path = path.parent;
                        }else{ //continue
                            path = path.parent;
                        }
                    } //The process will be repeated until it reaches the master node from the last element's process upwards.
                }
                else
                {
                    Console.WriteLine("Binary table is full.");
                }
            }
        }

        public int[] search(int data){
            int probe = 1;
            int index = hash(data);
            int index2 = quotient(data);

            //If the home index is blank, it means the data is not in the table
            if(table[index] == null)
            {
                return new int[] { -1, 0 };
            }
            else
            {
                //If it is the same data as the data found in the home index, it means the index is home index and probe number is 1.
                if(table[index].data == data)
                {
                    return new int[] { index, probe };
                }
                else
                {
                    TNode main = table[index]; //start point

                    while(table[index].data != -1)
                    {
                        index = (index + index2) % rowCount;
                        probe++;
                        if(table[index] == null)
                        {
                            return new int[] { -1, 0 };
                        }
                        else if(table[index].data == data)
                        {
                            return new int[] { index, probe };
                        }
                        else if(table[index] == main)
                        {
                            return new int[] { -1, 0 };
                        }
                    } //Iterates through hashes until data is found.

                    return new int[] { -1, 0 }; //If the data is not found, it returns the index is -1 and probe number is 0.
                }
            }
        } //The returned value is the index and probe number.

        public void print()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("| Index |   Key   |");
            Console.WriteLine("-------------------");
            for (int i = 0; i < rowCount; i++)
            {
                if (table[i] != null)
                    Console.WriteLine("|   {0}   |   {1}   |", i, table[i].data);
                else
                    Console.WriteLine("|   {0}   |   -   |", i);
            }
            Console.WriteLine("-------------------------");
        }

        private int hash(int data) { 
            return data % rowCount; 
        }

        private int quotient(int data) { 
            if(data < rowCount)
                return 1;
            else
                return (data / rowCount) % rowCount; 
        }
    }
    class Program
    {
        public static int rowCount;
        static void Main(string[] args)
        {
            Console.Write("Please, enter the row count of each table: ");
            rowCount = Convert.ToInt32(Console.ReadLine());
            if(rowCount > 1000)
            {
                Console.WriteLine("Row count must be less than 1000 cause that packing factor should be %90 for at most 900 random keys.");
                Console.ReadKey();
                return;
            }else{
                Console.Clear();
            }

            REISCH reisch = new REISCH(rowCount);
            BINARY binary = new BINARY(rowCount);

            // reisch.insert(72);
            // reisch.insert(19);
            // reisch.insert(63);
            // reisch.insert(29);
            // reisch.insert(95);
            // reisch.insert(14);
            // reisch.insert(50);
            // reisch.insert(86);
            // reisch.insert(12);
            // reisch.insert(30);
            // reisch.insert(39);

            // binary.insert(72);
            // binary.insert(19);
            // binary.insert(63);
            // binary.insert(29);
            // binary.insert(95);
            // binary.insert(14);
            // binary.insert(50);
            // binary.insert(86);
            // binary.insert(12);
            // binary.insert(30);
            // binary.insert(39);

            //--------------------

            // reisch.insert(27);
            // reisch.insert(18);
            // reisch.insert(29);
            // reisch.insert(28);
            // reisch.insert(39);
            // reisch.insert(13);
            // reisch.insert(16);
            // reisch.insert(41);
            // reisch.insert(17);
            // reisch.insert(19);

            // binary.insert(27);
            // binary.insert(18);
            // binary.insert(29);
            // binary.insert(28);
            // binary.insert(39);
            // binary.insert(13);
            // binary.insert(16);
            // binary.insert(41);
            // binary.insert(17);
            // binary.insert(19);

            while (true)
            {
                Console.Write("Insert: 1\nSearch: 2\nPrint: 3\nProbe campare: 4\nExit: -1\nPlease, enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case -1:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.Clear();
                        Console.Write("Enter the number you want to add: ");
                        int number = Convert.ToInt32(Console.ReadLine());

                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        reisch.insert(number);
                        watch.Stop();
                        var elapsedMs = watch.Elapsed;

                        var watch2 = System.Diagnostics.Stopwatch.StartNew();
                        binary.insert(number);
                        watch2.Stop();
                        var elapsedMs2 = watch2.Elapsed;

                        decimal packingFactor = Math.Round((Convert.ToDecimal(rowCount - reisch.blankIndexes.Count) / Convert.ToDecimal(rowCount))*100);
                        Console.WriteLine("\nPacking factor: {0}%\n", packingFactor);

                        Console.WriteLine("\nReisch insert time: {0} ms", elapsedMs);
                        Console.WriteLine("Binary insert time: {0} ms", elapsedMs2);
                        
                        Console.WriteLine("\nPress any key to back");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Enter the number you want to search: ");
                        int search = Convert.ToInt32(Console.ReadLine());

                        var watch3 = System.Diagnostics.Stopwatch.StartNew();
                         int[] dizi1 = reisch.search(search);
                        watch3.Stop();
                        var elapsedMs3 = watch3.Elapsed;

                        var watch4 = System.Diagnostics.Stopwatch.StartNew();
                        int[] dizi2 = binary.search(search);
                        watch4.Stop();
                        var elapsedMs4 = watch4.Elapsed;

                        if(dizi1[0] == -1 && dizi2[0] == -1)
                        {
                            Console.WriteLine("\nThere is no such number in the table.");
                        }
                        else
                        {
                            Console.WriteLine("\nReisch search time: {0} ms", elapsedMs3);
                            Console.WriteLine("Binary search time: {0} ms", elapsedMs4);

                            Console.WriteLine("\nReisch table: {0} is found in {1} index and {2} probe", search, dizi1[0], dizi1[1]);
                            Console.WriteLine("Binary table: {0} is found in {1} index and {2} probe", search, dizi2[0], dizi2[1]);
                        }
                        
                        Console.WriteLine("\nPress any key to back");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();

                        Console.WriteLine("REISCH");
                        reisch.print();
                        Console.WriteLine("\nBINARY");
                        binary.print();

                        Console.WriteLine("\nPress any key to back");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 4:
                        Console.Clear();

                        int reischProbesRequired = 0;
                        int binaryProbesRequired = 0;

                        for (int i = 0; i < rowCount; i++)
                        {
                            if((reisch.table[i] != null))
                            {
                                int[] probeAndIndex = reisch.search(reisch.table[i].data);
                                int[] probeAndIndex2 = binary.search(reisch.table[i].data);
                                reischProbesRequired += probeAndIndex[1];
                                binaryProbesRequired += probeAndIndex2[1];
                                Console.WriteLine("Reisch table: {0} is found in {1} index and {2} probe", reisch.table[i].data, probeAndIndex[0], probeAndIndex[1]);
                                Console.WriteLine("Binary table: {0} is found in {1} index and {2} probe\n", reisch.table[i].data, probeAndIndex2[0], probeAndIndex2[1]);
                            }
                        }

                        Console.WriteLine("\n REISCH total probe: {0}, BINARY total probe: {1}", reischProbesRequired, binaryProbesRequired);
                        
                        Console.WriteLine("\nPress any key to back");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid entry");
                        System.Threading.Thread.Sleep(2000);
                        Console.Clear();
                        break;
                }
            }

        }

    }
}
