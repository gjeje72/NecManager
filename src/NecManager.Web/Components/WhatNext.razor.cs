namespace NecManager.Web.Components;

using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

public partial class WhatNext
{
    [Parameter]
    public List<string> Agendas { get; set; } = new List<string>()
    {
        "Prochain stage : 24 au 26 avril",
        "Prochain cours : mardi 18h",
        "Prochain stage : 24 au 26 avril",
        "Prochain cours : mardi 18h",
    };

    [Parameter]
    public List<string> Competitions { get; set; } = new List<string>()
    {
        "27 mai : Chp FR Fleuret Sénior Nantes",
        "3 juin : CD 44 - équipes Nantes",
    };
}
