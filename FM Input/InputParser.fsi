// Signature file for parser generated by fsyacc
module InputParser
type token = 
  | SAND
  | SOR
  | AND
  | OR
  | NEG
  | EQUAL
  | NOTEQ
  | GREATER
  | LESS
  | GREATEREQ
  | LESSEQ
  | SETR
  | DELIM
  | LBRAK
  | RBRAK
  | LCURL
  | RCURL
  | TIMES
  | DIV
  | PLUS
  | MINUS
  | POW
  | LPAR
  | RPAR
  | EOF
  | LN
  | LOG
  | BOOL of (bool)
  | VARIABLE of (string)
  | NUM of (int)
type tokenId = 
    | TOKEN_SAND
    | TOKEN_SOR
    | TOKEN_AND
    | TOKEN_OR
    | TOKEN_NEG
    | TOKEN_EQUAL
    | TOKEN_NOTEQ
    | TOKEN_GREATER
    | TOKEN_LESS
    | TOKEN_GREATEREQ
    | TOKEN_LESSEQ
    | TOKEN_SETR
    | TOKEN_DELIM
    | TOKEN_LBRAK
    | TOKEN_RBRAK
    | TOKEN_LCURL
    | TOKEN_RCURL
    | TOKEN_TIMES
    | TOKEN_DIV
    | TOKEN_PLUS
    | TOKEN_MINUS
    | TOKEN_POW
    | TOKEN_LPAR
    | TOKEN_RPAR
    | TOKEN_EOF
    | TOKEN_LN
    | TOKEN_LOG
    | TOKEN_BOOL
    | TOKEN_VARIABLE
    | TOKEN_NUM
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_cheat
    | NONTERM_expression
    | NONTERM_expression1
    | NONTERM_str
    | NONTERM_boolexpression
    | NONTERM_inputvalues
    | NONTERM_seqinput
    | NONTERM_signvalues
    | NONTERM_seqsign
/// This function maps tokens to integer indexes
val tagOfToken: token -> int

/// This function maps integer indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val start : (FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> FSharp.Text.Lexing.LexBuffer<'cty> -> (cheat) 
