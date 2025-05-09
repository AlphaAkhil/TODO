using System;

namespace todoApp{
    class CreateTable{
        int numOfCol;
        int[] colWidth;

        public CreateTable(int coloumNum ,int[] width){
            numOfCol = coloumNum;
            colWidth = width;            
        }

        public void ShowTable(){
            
        }

        public void PrintRowLines(){
            Console.Write("|");
            for(int i = 0;i <numOfCol;i++){
                for(int j = 0;j < colWidth[i]+1;j++){
                    Console.Write("-");
                } 
                Console.Write("|");
            }
            Console.WriteLine();
        }

        public void PrintRow(string[] tableValue){
            // string rowData = "|";
            for(int i =0; i<colWidth.Length;i++)
                // rowData += tableValue[i].PadLeft(10,' ');
                Console.Write($"| {tableValue[i].PadRight(colWidth[i],' ')}");
            // return rowData;
            Console.WriteLine("|");
        }
    }
    
}