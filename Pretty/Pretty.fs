namespace Pretty

module List =
    let rec intersperse sep list =
        match list with
        | []      -> []
        | [x]     -> [x]
        | (x::xs) -> x::sep::(intersperse sep xs)

(* The type used to create documents *) 
type Doc =
    | NIL
    | LINE
    | TEXT of string
    | NEST of int * Doc
    | CONCAT of Doc * Doc
    | UNION of Doc * Doc
    
    (* The constructors for Doc *)
    static member nil = NIL
    static member line = LINE
    static member text s = TEXT s
    member this.Indent i = NEST (i, this)

    static member (+) (a : IPrettyfiable, b : IPrettyfiable) = CONCAT (a.ToDoc(), b.ToDoc())
    
    ///Generates a new version of the Doc with no line breaks.
    member this.Flatten () = this.flatten
    member internal this.flatten =
        match this with
        | NIL          -> NIL
        | LINE         -> TEXT " "
        | TEXT s       -> TEXT s
        | NEST (i,a)   -> a.flatten
        | CONCAT (a,b) -> CONCAT (a.flatten, b.flatten)
        | UNION (a,_)  -> a.flatten
        
    ///Generates a new Doc which contains options for having line breaks or not.
    member this.Group () = this.group
    member internal this.group = UNION(this.Flatten(), this)
    
    (* Conversion from Doc to DOC, nice guideline on working with Docs *)
    /// Given a list of pairs (idt, Doc),
    /// being the idt an indentation level,
    /// generates an equivalent DOC.
    static member private toDOCP pairs =
        match pairs with
        | []                        -> Nil
        | (i, NIL)          :: rest -> Doc.toDOCP rest
        | (i, NEST   (j,a)) :: rest -> Doc.toDOCP ((i+j, a) :: rest)
        | (i, CONCAT (a,b)) :: rest -> Doc.toDOCP ((i,a)::(i,b)::rest)
        | (i, LINE)         :: rest -> Line (i, Doc.toDOCP rest)
        | (i, TEXT s)       :: rest -> Text (s, Doc.toDOCP rest)
        | (i, UNION  (a,b)) :: rest -> Union ( Doc.toDOCP ((i,a) :: rest), Doc.toDOCP ((i,b)::rest) )
    /// Conversion from a Doc to a DOC
    member internal this.toDOC() = Doc.toDOCP [(0,this)]
    
    /// Given a list of pairs (idt, Doc),
    /// being the idt an indentation level,
    /// generates the best possible DOC
    /// that fits in lines of size 'wid',
    /// the first already having 'k' chars
    static member private bestP wid k pairs =
        match pairs with
        | []                        -> Nil
        | (i, NIL)          :: rest -> Doc.bestP wid k rest
        | (i, NEST   (j,a)) :: rest -> Doc.bestP wid k ((i+j,a)::rest)
        | (i, CONCAT (a,b)) :: rest -> Doc.bestP wid k ((i,a)::(i,b)::rest)
        | (i, LINE)         :: rest -> Line (i, Doc.bestP wid i rest)
        | (i, TEXT s)       :: rest -> Text (s, Doc.bestP wid (k+s.Length) rest)
        | (i, UNION  (a,b)) :: rest -> DOC.better (wid-k) (Doc.bestP wid k ((i,a)::rest)) (Doc.bestP wid k ((i,b)::rest))
        
    /// Given a Doc, generates the best possible DOC
    /// that fits in lines of size 'wid', the first 
    /// already having 'k' chars, and 
    member internal this.best wid k = Doc.bestP wid k [(0, this)]

    /// Given a list of pairs (idt, Doc),
    /// being the idt an indentation level,
    /// generates the best possible DOC
    /// that fits in lines of size 'wid',
    /// the first already having 'k' chars,
    /// seeking 'rl' non-indentation chars 
    /// per line, having 'ni' already on 
    /// the first one.
    static member private besterP wid k rl ni pairs =
        match pairs with
        | []                        -> Nil
        | (i, NIL)          :: rest -> Doc.besterP wid k rl ni rest
        | (i, NEST   (j,a)) :: rest -> Doc.besterP wid k rl ni ((i+j,a)::rest)
        | (i, CONCAT (a,b)) :: rest -> Doc.besterP wid k rl ni ((i,a)::(i,b)::rest)
        | (i, LINE)         :: rest -> Line (i, Doc.besterP wid i rl 0 rest)
        | (i, TEXT s)       :: rest -> Text (s, Doc.besterP wid (k+s.Length) rl (ni+s.Length) rest)
        | (i, UNION  (a,b)) :: rest -> DOC.betterer (wid-k)
                                                    (rl-ni) 
                                                    (Doc.besterP wid k rl ni ((i,a)::rest))
                                                    (Doc.besterP wid k rl ni ((i,b)::rest))
        
    /// Given a Doc, generates the best possible DOC
    /// that fits in lines of size 'wid', the first 
    /// already having 'k' chars, seeking 'rl' non-
    /// indentation chars per line, having 'ni' 
    /// already on the first one.
    member internal this.bester wid k rl ni = Doc.besterP wid k rl ni [(0, this)]
    
    /// Given a list of pairs (idt, Doc),
    /// being the idt an indentation level,
    /// with no UNIONs, generates a string 
    /// for it. If there are UNIONs, the
    /// behaviour is undefined.
    static member private toStringP pairs =
        match pairs with
        | []                        -> ""
        | (i, NIL)          :: rest -> Doc.toStringP rest
        | (i, NEST   (j,a)) :: rest -> Doc.toStringP ((i+j, a) :: rest)
        | (i, CONCAT (a,b)) :: rest -> Doc.toStringP ((i,a)::(i,b)::rest)
        | (i, LINE)         :: rest -> System.Environment.NewLine + String.replicate i " " + Doc.toStringP rest
        | (i, TEXT s)       :: rest -> s + Doc.toStringP rest
        | (i, UNION  (a,_)) :: rest -> Doc.toStringP ((i,a) :: rest)

    /// Given a Doc, without UNIONs, generates a string for it.
    /// Behaviout on Docs with UNIONs is undefined
    override this.ToString () = this.toString
    member internal this.toString = Doc.toStringP [(0,this)]

    /// Given a Doc, generates the best possible
    /// string for lines of width 'w'.
    member this.Pretty(w, ?rl : int) =
        let rl = defaultArg rl w
        if rl = w
        then (this.best w 0).toString
        else (this.bester w 0 rl 0).toString
    member internal this.pretty w = this.Pretty w
    
    /// Given a Doc, generates a single-line string
    member this.Flat () = this.flat
    member internal this.flat = this.flatten.toString

    interface IPrettyfiable with
        member this.ToDoc () = this

(* Interface for types that may convert to Docs *)
and IPrettyfiable = interface
    ///Converts the current object to a Doc for pretty-printing
    abstract member ToDoc : unit -> Doc
end

(* The type used to pretty-print the documents *)
and internal DOC =
    | Nil
    | Text of string * DOC
    | Line of int * DOC
    | Union of DOC * DOC

    /// Checks if the given DOC fits in 'wid' characters.
    /// Undefined behaviour when contains a Union.
    member this.fits wid = 
        match this with
        | _ when wid < 0 -> false
        | Nil            -> true
        | Text (s,a)     -> a.fits (wid - s.Length)
        | Line (i,a)     -> true
        | _              -> failwith "Cannot test if Union fits a line."
    /// Given two best DOCs from a Union, returns
    /// the best option for wid characters
    static member better wid (a:DOC) (b:DOC) =
        if a.fits wid then a else b
        
    /// Checks if the given DOC fits in 'wid' characters,
    /// and in 'rl' non-indentation characters
    /// Undefined behaviour when contains a Union.
    member this.fitser wid rl = 
        match this with
        | _ when wid < 0 -> false
        | _ when rl < 0  -> false
        | Nil            -> true
        | Text (s,a)     -> a.fitser (wid - s.Length) (rl - s.Length)
        | Line (i,a)     -> true
        | _              -> failwith "Cannot test if Union fits a line."
    /// Given two best DOCs from a Union, returns
    /// the best option for wid characters
    static member betterer wid rl (a:DOC) (b:DOC) =
        if a.fitser wid rl then a else b

    /// Given a DOC without Unions, generates a string for it.
    /// Behaviour on DOCs with Unions is undefined.
    member this.toString =
        match this with
        | Nil          -> ""
        | Line (i,a)   -> System.Environment.NewLine + String.replicate i " " + a.toString
        | Text (s,a)   -> s + a.toString
        | Union (a,_)  -> a.toString

(* Module with some utility functions for PrettyPrinting *)
module PrettyPrinter =

    /// Constructs a Doc by concatenating the given
    /// left/right separators to the Doc and grouping it all
    let bracket left (middle : IPrettyfiable) right idt =
        let midDoc = middle.ToDoc()
        (Doc.text (left+" ") + midDoc.Indent idt + Doc.text (" "+right)).group

    /// Constructs a Doc from the list of strings,
    /// using the given left/right strings and
    /// the item separator
    let fromStringList (left : string) sequence sep (right : string) =
        let middle = Seq.toList sequence |> List.map (fun s -> Doc.text (s+sep)) |> List.intersperse Doc.line |> List.fold (+) (Doc.nil)
        bracket left middle right ((max left.Length right.Length) + 1)

    /// Constructs a Doc from the list of Docs,
    /// using the given left/right strings and
    /// the item separator
    let fromDocList left (sequence : seq<IPrettyfiable>) sep right =
        let middle = Seq.toList sequence |> List.map (fun o -> o.ToDoc()) |> List.intersperse (Doc.text(sep) + Doc.line) |> List.fold (+) (Doc.nil)
        bracket left middle right ((max left.Length right.Length) + 1)
    
    /// Given an IPrettyfiable, generates the best 
    /// possible string for lines of width 'w'.
    let prettify (obj : IPrettyfiable) w = obj.ToDoc().Pretty w

    /// Given an IPrettyfiable, generates the best 
    /// possible string for lines of width 'w', with 
    /// a "ribbon length" 'rl', i.e. the expected 
    /// number of non-indentation characters per line
    let prettierfy (obj : IPrettyfiable) w rl = obj.ToDoc().Pretty(w, rl)
