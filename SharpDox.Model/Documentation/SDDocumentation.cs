using System;
using System.Collections.Generic;
using SharpDox.Model.Documentation.Token;

namespace SharpDox.Model.Documentation
{
    /// <default>
    ///     <summary>
    ///     Represents the inline documentation of a code member.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert die Dokumentation eines Code-Elements.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDDocumentation
    {
        public SDDocumentation()
        {
            Summary = new SDTokenList();
            Remarks = new SDTokenList();
            Example = new SDTokenList();
            Returns = new SDTokenList();
            SeeAlso = new SDTokenList();
            Exceptions = new Dictionary<string, SDTokenList>();
            Params = new Dictionary<string, SDTokenList>();
            TypeParams = new Dictionary<string, SDTokenList>();
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of <c>SDToken</c>s, representing the summary section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Summary"-Sektion repräsentiert.
        ///     </summary>
        /// </de>
        public SDTokenList Summary { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of <c>SDToken</c>s,  representing the remarks section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Remarks"-Sektion repräsentiert.
        ///     </summary>
        /// </de>
        public SDTokenList Remarks { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of <c>SDToken</c>s,  representing the example section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Example"-Sektion repräsentiert.
        ///     </summary>
        /// </de>
        public SDTokenList Example { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of <c>SDToken</c>s,  representing the returns section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Returns"-Sektion repräsentiert.
        ///     </summary>
        /// </de>
        public SDTokenList Returns { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of <c>SDToken</c>s,  representing all seealso sections.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "seealso"-Sektionen repräsentiert.
        ///     </summary>
        /// </de>
        public SDTokenList SeeAlso { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a <c>Dictionary</c> of lists with <c>SDToken</c>s, representing the exception sections. 
        ///     The key of the <c>Dictionary</c> is the name property of the section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Exception"-Sektion repräsentiert.
        ///     Der Schlüssel des <c>Dictionary</c> ist der Wert der Namens-Eigenschaft.
        ///     </summary>
        /// </de>
        public Dictionary<string, SDTokenList> Exceptions { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a <c>Dictionary</c> of lists with <c>SDToken</c>s, representing the params sections. 
        ///     The key of the <c>Dictionary</c> is the name property of the section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Params"-Sektion repräsentiert.
        ///     Der Schlüssel des <c>Dictionary</c> ist der Wert der Namens-Eigenschaft.
        ///     </summary>
        /// </de>
        public Dictionary<string, SDTokenList> Params { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a <c>Dictionary</c> of lists with <c>SDToken</c>s, representing the typeparam sections. 
        ///     The key of the <c>Dictionary</c> is the name property of the section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste von <c>SDToken</c>s, welche die "Typeparam"-Sektion repräsentiert.
        ///     Der Schlüssel des <c>Dictionary</c> ist der Wert der Namens-Eigenschaft.
        ///     </summary>
        /// </de>
        public Dictionary<string, SDTokenList> TypeParams { get; set; }
    }
}