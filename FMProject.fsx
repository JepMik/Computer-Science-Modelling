// This script implements our interactive calculator

// We need to import a couple of modules, including the generated lexer and parser

#r "FsLexYacc.Runtime.10.0.0/lib/net46/FsLexYacc.Runtime.dll"
//#r "~/fsharp/FsLexYacc.Runtime.dll"
open FSharp.Text.Lexing
open System
open System.IO
#load "FMProjectTypesAST.fs"
open FMProjectTypesAST
#load "FMProjectParser.fs"
open FMProjectParser
#load "FMProjectLexer.fs"
open FMProjectLexer

// We define the evaluation function recursively, by induction on the structure
// of arithmetic expressions (AST of type expr)
let rec evalA e =
  match e with
    | Num(x) -> x
    | TimesExpr(x,y) -> evalA(x) * evalA(y)
    | DivExpr(x,y) -> evalA(x) / evalA (y)
    | PlusExpr(x,y) -> evalA(x) + evalA (y)
    | MinusExpr(x,y) -> evalA(x) - evalA (y)
    | PowExpr(x,y) -> evalA(x) ** evalA (y)
    | UPlusExpr(x) -> evalA(x)
    | UMinusExpr(x) -> - evalA(x)
    | LogExpr(x) -> Math.Log(evalA(x),2)
    | LnExpr(x) -> Math.Log(evalA(x))
    | _ -> 0.0 //#TODO

// "Pretty Printer" for arithmetic expressions to show precedence of the operators
let rec printA e = 
    match e with
    | StrA(x) -> " "+x
    | Num(x) -> " "+x.ToString()
    | TimesExpr(x,y) -> " TIMES( "+(printA x)+","+(printA y)+" )"
    | DivExpr(x,y) -> " DIV( "+(printA x)+","+(printA y)+" )"
    | PlusExpr(x,y) -> " PLUS( "+(printA x)+","+(printA y)+" )"
    | MinusExpr(x,y) -> " MINUS( "+(printA x)+","+(printA y)+" )"
    | PowExpr(x,y) -> " POW( "+(printA x)+","+(printA y)+" )"
    | UPlusExpr(x) -> " UPLUS( "+(printA x)+" )"
    | UMinusExpr(x) -> " UMINUS( "+(printA x)+" )"
    | IndexExpr(A,x) -> A+"["+(printA x)+"]"
    | LogExpr(x) -> " LOG( "+(printA x)+" )"
    | LnExpr(x) -> " LN( "+(printA x)+" )"

// "Pretty Printer" for boolean expressions to show precedence of the operators
let rec printB e = 
    match e with 
    | Bool(x) -> " "+x.ToString()
    | StrB(x) -> " "+x
    | BitWiseAnd(x,y) -> " BITWAND( "+(printB x)+","+(printB y)+" )"
    | BitWiseOr(x,y) -> " BITWOR( "+(printB x)+","+(printB y)+" )"
    | LogAnd(x,y) -> " AND( "+(printB x)+","+(printB y)+" )"
    | LogOr(x,y) -> " OR( "+(printB x)+","+(printB y)+" )"
    | Neg(x) -> " NEG( "+(printB x)+" )"
    | Equal(x,y) -> " EQUAL( "+(printA x)+","+(printA y)+" )"
    | NotEqual(x,y) -> " NOTEQUAL( "+(printA x)+","+(printA y)+" )"
    | Greater(x,y) -> " GREATER( "+(printA x)+","+(printA y)+" )"
    | GreaterEqual(x,y) -> " GREATEREQUAL( "+(printA x)+","+(printA y)+" )"
    | Less(x,y) -> " LESS( "+(printA x)+","+(printA y)+" )"
    | LessEqual(x,y) -> " LESSQUAL( "+(printA x)+","+(printA y)+" )"

// "Pretty Printer" for guarded commands and commands to show precedence of the operators
let rec printGC e = 
    match e with
    | IfThen(x,y) -> (printB x)+" -> "+(printC y)
    | FatBar(x,y) -> (printGC x)+" [] "+(printGC y)
and printC e =
    match e with
    | ArrayAssign(x,y,z) -> x+"["+(printA y)+"]:="+(printA z)
    | Assign(x,y) -> x+" := "+(printA y)
    | Skip -> " SKIP "
    | Order(x,y) -> (printC x)+" ; "+(printC y)
    | If(x) -> " IFFI( "+(printGC x)+" )"
    | Do(x) -> " DOOD( "+(printGC x)+" )"

// Function that parses a given input
let parse input =
    // translate string into a buffer of characters
    let lexbuf = LexBuffer<char>.FromString input
    // translate the buffer into a stream of tokens and parse them
    let res = FMProjectParser.start FMProjectLexer.tokenize lexbuf
    // return the result of parsing (i.e. value of type "expr")
    res

// Function that interacts with the user
let rec compute n =
    if n = 0 then
        printfn "Bye bye"
    else
        printf "Enter an arithmetic expression: "
        try
        // We parse the input string
        let e = parse (Console.ReadLine())
        // and print the result of evaluating it
        //printfn "Result of evaluation: %f" (eval(e))
        //let str = (printA e)
        //printfn "%s" str;
        compute n
        with err -> compute (n-1)

try
    printfn "Insert your Guarded Commands program to be parsed:"
    //Read console input
    let input = Console.ReadLine()
    //Create the lexical buffer
    let lexbuf = LexBuffer<char>.FromString input
    
    try 
       //Parsed result
       let res = FMProjectParser.start FMProjectLexer.tokenize lexbuf 
       printfn "<---Pretty print:--->"
       printfn "%s" (printC res)

    //Undefined string encountered   
    with e -> printfn "Parse error at : Line %i, %i, Unexpected char: %s" (lexbuf.EndPos.pos_lnum+ 1) 
                    (lexbuf.EndPos.pos_cnum - lexbuf.EndPos.pos_bol) (LexBuffer<_>.LexemeString lexbuf)

with e -> printfn "ERROR: %s" e.Message

// Start interacting with the user
//compute 3
//printfn "%s" (printC (If(IfThen(Bool(true), Assign("x",Num(2))))))
//printfn "%s" (printB (LogAnd(Bool(false),Neg(Bool(true)))))
//let str = printC (parse (Console.ReadLine()))
//let str = (printC (parse (File.ReadAllText("test.txt") )))
//printfn "%s" str

