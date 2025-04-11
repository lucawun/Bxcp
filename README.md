# BettercallPaul programming challenge

Grundsätzlich gilt es Aufgaben, hauptsächlich zu Datenanalyse zu lösen:

- **Klimaanalyse**: Finden des Tages mit der kleinsten Temperaturschwankung
- **Länderstatistik**: Ermitteln des Land mit der höchsten Bevölkerungsdichte

Obwohl beide Analysen technisch ähnlich sind (CSV-Daten einlesen und auswerten), repräsentieren sie unterschiedliche Fachgebiete.

### Disclaimer zu KI-Unterstützung
Für die Erstellung wurde KI-gestützte Tools, Typing Mind (via. Claude 3.7 Sonnet) und GitHub Co-Pilot (via. GPT-4o) genutzt.

## Ziele

Die Ziele wurden durch den Stakeholder "BettercallPaul" bereits festgelegt.

Qualitätsziel  |  Was bedeutet das für mich, was verstehe ich darunter ?      |
| --- | --- |
  Robustheit & Korrektheit (robustness & correctness)  |  Software muss unter allen Umständen funktionieren, nicht nur im Geradeausfall. Konsequente Validierung aller Eingaben, durchdachte Fehlerbehandlung und umfassende Tests. Systeme scheitern in der Praxis meist an unerwarteten Eingaben – daher ist defensive Programmierung für mich Pflicht, nicht Kür.  |
|  Lesbarkeit & Wartbarkeit (readability & maintainability)  |  Code wird häufiger gelesen als geschrieben. Klare Benennungen, konsistente Strukturen und nachvollziehbare Logik schaffen Code, den andere (und ich selbst in 6 Monaten) sofort verstehen. Wartbarkeit ist kein Luxus, sondern wirtschaftliche Notwendigkeit – die meisten Kosten entstehen nach der Erstentwicklung. |
|  Sauberes Softwaredesign (clean software design & architecture) |  Gute Architektur reduziert Komplexität durch sinnvolle Abstraktion. Die Trennung von Fachlogik und technischen Details macht ein System zukunftssicher und flexibel. Mit fokussierten Komponenten und definierten Schnittstellen schaffe ich ein System, das organisch mitwachsen kann, statt unter seinem eigenen Gewicht zu brechen. Stichwort: "Big Ball of Mud"  |

## Die Lösung: Hexagonal/Onion-Architektur

Die Hexagonale Architektur (auch "Ports & Adapters" oder "Onion Architecture" genannt) funktioniert wie eine Zwiebel mit mehreren Schichten:

<img src="hexagonal-onion-architecture-theory.png" alt="Hexagonal/Onion-Architektur Theorie" width="800">

[Quelle, Blog Herberto Graca](https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/comment-page-1)

Anders ausgedrückt: Im Zentrum steht die eigentliche Fachlogik (Was wird berechnet?), umgeben von Anwendungsfällen (Wie wird es genutzt?), und außen ist die technische Umsetzung (Woher kommen die Daten?).

### Warum habe ich mich dafür entschieden?

Die eigentliche Analyselogik bleibt unabhängig von technischen Details wie Dateiformat oder Benutzeroberfläche.
Wenn sich die Datenquelle ändert (z.B. von CSV zu JSON oder Datenbank), muss ich nur die äußere Schicht anpassen, nicht die Kernlogik.

Die klare Trennung ermöglicht isoliertes Testen jeder Komponente ohne Abhängigkeiten zu externen Systemen, was die Codequalität verbessert und Wartung vereinfacht.

Präzise Fehlerbehandlung auf jeder Ebene - fachliche Fehler in der Domäne, Prozessfehler in der Anwendungsschicht, technische Fehler in der Infrastruktur.

### 1. Domänenschicht/Domain-Layer (Core)

Der Kern meines Systems, der die eigentliche Fachlichkeit enthält – ohne Abhängigkeiten zu externen Systemen.

- Definiert die beiden Models `Weather` und `Country`
- Entwicklung nach DDD, BDD, TDD-Prinzipien
- Enthält Domain Services Analyselogik (kleinste Temperaturschwankung finden, höchste Bevölkerungsdichte ermitteln)
- Legt Schnittstellen (Ports) fest, über die mit der Außenwelt kommuniziert wird

Durch die Isolation der Fachlogik stelle ich sicher, dass sie verständlich bleibt, unabhängig von technischen Details funktioniert einfach durch meinen Stakeholder zu verstehen ist, leicht getestet werden kann.

### 2. Anwendungsschicht/Application-Layer

Die Schicht, die UseCase beinhaltet und zwischen Domäne und Infrastruktur vermittelt.

- Steuert den Ablauf der UseCases (Klimaanalyse / Länderstatistik)
- Übersetzt zwischen Domänenmodellen und externen Repositories
- Kümmert sich um die übergreifende Fehlerbehandlung

Es schafft eine klare Struktur und macht den Code selbsterklärend – jeder UseCase ist eine eigene abgeschlossene Einheit mit einem eindeutigen Zweck.

### 3. Infrastrukturschicht

Die äußerste Schicht, die konkrete technische Implementierungen enthält.

- CSV-Datenzugriff über das Repository-Pattern
- Transformation externer Daten in Domänenmodelle
- Stellt technische Services bereit

Die gemeinsame Schnittstelle `IRepository<T>` mit spezifischen Implementierungen für Wetter- und Länderdaten sorgt für eine klare Trennung zwischen Datenzugriff und -verarbeitung. Diese Kapselung schützt die Kernlogik vor technischen Änderungen und erhält die Stabilität der Fachlogik bei Technologiewechseln.

<img src="hexagonal-onion-architecture.png" alt="Hexagonal/Onion-Architektur" width="800">

[Quelle, Claude 3.7 Sonnet + Mermaid Live Editor](https://mermaid.live)

## Nachteile und Herausforderungen der Architektur

Für eine einfache Datenanalyse wie in dieser Aufgabe könnte der Architekturansatz zunächst überproportioniert wirken.

Die hexagonale Architektur führt auch zu mehr Klassen, Schnittstellen und Abstraktionsebenen als ein einfacherer, direkter Ansatz.
Mehr Code bedeutet mehr potenzielle Fehlerquellen mehr Wartungsaufwand und mehr Einarbeitungszeit für neue Entwickler.

Ich wollte es praktisch ausprobieren - die Literatur und das eine oder andere Hobby Projekt zur Hexagonalen Architektur kannte ich bereits, hatte jedoch nie ein Projekt von Grund auf neu damit aufgebaut. Daher hab ich es als persönliche kleine Challenge gesehen, das angeeignete Wissen anhand des Beispielprojekts in die Praxis umzusetzen.

Mein Zwischenfazit ist, dass die Architektur für diese spezifische Aufgabe zwar `overengineered` ist - bei größeren, komplexeren Projekten mit wachsenden Anforderungen sich der anfängliche Mehraufwand wahrscheinlich durch bessere Wartbarkeit und Erweiterbarkeit auszahlen würde.

Ich freue mich auf schon auf eine konstruktive Diskussionen im Vorstellungsgespräch, wo wir darüber sprechen können, welche Aspekte der Architektur für verschiedene Anwendungsfälle doch sinnvoll sind und wo noch vereinfacht werden könnte.
Stichwort: Vertical Slices, fachliche Zusammengehörigkeiten besser gruppieren, Domain-Kerin weiterhin isolieren, ... ?

## ToDos / Technische Schulden

- [ ] Bessere Make-or-Buy-Entscheidung: Implementierung einer fertigen Lösung für das Auslesen von CSV-Dateien z.B. [CsvHelper](https://github.com/JoshClose/CsvHelper) ebenso für die Spaltenzuordnung, bzw. das Mapping zwischen CSV-Spalten und Domänen/DTOs z.B. [AutoMapper](https://github.com/AutoMapper/AutoMapper).

- [ ] Bessere Testfälle (nicht teilweise KI generierte) - die genau das "wie" der Anwendung testen und nicht mögliche vorhandene Fehler abtesten

- [ ] Einsatz von [Fluent Validation](https://github.com/FluentValidation/FluentValidation) für bessere Validierungsprüfungen statt der Guard Clauses im Domain-Model, würde auch wieder die Testbarkeit erhöhen

- [ ] Integration eines strukturierten Logging-Frameworks wie [Serilog](https://github.com/serilog/serilog) oder [NLog](https://github.com/NLog/NLog), das verschiedene Log-Level unterstützt

- [ ] Dateipfade sind im Code festgelegt und nicht konfigurierbar -> Implementation einer Konfigurationsschicht, die Dateipfade aus Konfigurationsdateien oder Umgebungsvariablen liest.

- [ ] Update auf .NET 10 im Nov. 2025, 36 Monaten Support (Nov. 2028) -> derzeit .NET 8.0 (LTS), EOL: Nov. 2026