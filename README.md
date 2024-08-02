Beispiel für einen einfachen Zugriff auf eine SharePoint 2019 Dokumentenbibliothek mit .NET 7.

Folgende Dinge werden illustriert:
   - Auslesen der Spalten einer SharePoint-Bibliothek
   - Anzeigen einer gefilterten Liste von Dokumenten
   - Angabe expliziter Credentials

Der Benutzername für die Authentifizierung kann als Domäne\Benutzer oder einfach als Benutzer angegeben werden.

Zum Filtern fügt man $filter=<Spalte>> eq 'Wert' hinzu

Zum Begrenzen der Anzahl der zurückgegebenen Einträge nimmt man $top=<Zahl>. Die Zahl muss kleiner oder gleich 5000 sein.

Zum Sortieren nimmt man $orderby <Spaltenname> asc oder desc

Zum Auswählen der zurückzugebenden Spalten nimmt man $select=<Spalte1>,<Spalte2> usw.

