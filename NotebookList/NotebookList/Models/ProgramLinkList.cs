using System;
using System.Collections.Generic;

namespace NotebookList.Models;

public partial class ProgramLinkList
{
    public int LinkId { get; set; }

    public string? LinkUrl { get; set; }

    public int? ProgramId { get; set; }

    public string? LinkTitle { get; set; }

    public virtual Program? Program { get; set; }
}
