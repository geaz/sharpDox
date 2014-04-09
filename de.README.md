Erstelle mit Leichtigkeit Code-Dokumentationen
------------------

**sharpDox** ist ein Dokumentationstool mit dem jede Art von C# Code-Dokumentation erstellt werden kann. 
Die Kernaufgabe von sharpDox ist es, den Quellcode eines Projektes einzulesen und zu analysieren. 
Dabei wird ein internes Modell aufgebaut, das alle Informationen zum Code beinhaltet. 
Neben der Inline-Dokumentation sind dies zum Beispiel Informationen darüber, ob ein Typ bestimmte Interfaces implementiert oder andere Typen erbt.

Das komplette Modell des Quellcodes wird den registrierten Exportern übergeben,
die das Modell in ein gewünschtes Ausgabeformat überführen. Dabei können Entwickler auf einfache Art und
Weise eigene Exporter schreiben, die das Model neben dem CHM- und HTML-Format zum Beispiel in ein Word-Dokument übertragen. 

Die Benutzer von sharpDox können zusätzlich zu der eigentlichen Code-Dokumentation auch benutzerdefinierte 
Seiten in die Dokumentation einbinden. Dies können zum Beispiel erweiterte Beschreibungen von
Namensräumen oder Anleitungen sein. sharpDox beherrscht noch einige weitere Dinge, wie zum Beispiel die Erstellung von eigenen
Dokumentations-Navigationen oder mehrsprachigen Dokumentationen.

**sharpDox** wird mit einem HTML-Exporter ausgeliefert, dieser soll die Möglichkeiten 
von **sharpDox** veranschaulichen.