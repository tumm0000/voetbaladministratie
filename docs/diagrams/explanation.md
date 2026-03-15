# mermaid-diagram.png
Deze afbeelding is een weergave van het contextdiagram. Deze is gegenereerd door de volgende Mermaid-code in te voeren op [mermaid.live](https://mermaid.live).
```
flowchart LR
    subgraph actoren["Actoren"]
        Bestuur[Bestuur]
        Trainers[Trainers]
    end

    subgraph subsystemen["Subsystemen"]
        Ledenbeheer[Ledenbeheer]
        Wedstrijdplanning[Wedstrijdplanning]
        Betalingssysteem["Betalingssysteem en herinneringen"]
    end

    Bestuur --> Ledenbeheer
    Bestuur --> Wedstrijdplanning
    Bestuur --> Betalingssysteem
    Trainers --> Ledenbeheer
    Trainers --> Wedstrijdplanning
```