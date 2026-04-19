**Hybrid Real-Time Chat System 📱💻**
מערכת צ'אט היברידית המאפשרת תקשורת בזמן אמת בין לקוחות WPF (Desktop) ללקוחות .NET MAUI (Mobile). הפרויקט מדגים פתרון הנדסי לאתגר התקשורת הדו-כיוונית במובייל על ידי שילוב של טכנולוגיות WCF ו-SignalR.

**🎯 האתגר הטכנולוגי
**
פרוטוקול WCF Duplex (תקשורת דו-כיוונית מלאה) עובד מצוין בסביבת דסקטופ, אך אינו נתמך באופן טבעי או יציב בפלטפורמות מובייל מודרניות כמו .NET MAUI. המערכת פותרת זאת על ידי שימוש ב-SignalR כ"גשר" (Bridge) לדחיפת הודעות ללקוחות המובייל.

🏗** ארכיטקטורת המערכת**
Hybrid Real-Time Chat System 📱💻

מערכת צ'אט היברידית המאפשרת תקשורת בזמן אמת בין לקוחות WPF (Desktop) ללקוחות .NET MAUI (Mobile). הפרויקט מדגים פתרון הנדסי לאתגר התקשורת הדו-כיוונית במובייל על ידי שילוב של טכנולוגיות WCF ו-SignalR.

**🎯 האתגר הטכנולוגי**

פרוטוקול WCF Duplex (תקשורת דו-כיוונית מלאה) עובד מצוין בסביבת דסקטופ, אך אינו נתמך באופן טבעי או יציב בפלטפורמות מובייל מודרניות כמו .NET MAUI. המערכת פותרת זאת על ידי שימוש ב-SignalR כ"גשר" (Bridge) לדחיפת הודעות ללקוחות המובייל.

🏗 **ארכיטקטורת המערכת**

המערכת מורכבת משלושה רכיבים עיקריים:

1. שרת היברידי (WpfHost + WcfService)

השרת מארח בו-זמנית שני סוגי תקשורת:

WCF Endpoint (wsDualHttpBinding): מיועד ללקוחות WPF, תומך ב-Callback Contract (IChatCallback) לקבלת הודעות בזמן אמת.

WCF Endpoint (basicHttpBinding): מיועד ללקוחות MAUI, מאפשר פעולות Stateless (שליחה, התחברות, משיכת היסטוריה).

SignalR Hub (ChatHub): משמש כערוץ ה-Callback עבור לקוחות המובייל.

2. לקוח דסקטופ (WPF)

משתמש בחוזה IChatService ובמימוש IChatCallback כדי לקבל הודעות בדחיפה ישירות מהשרת.

3. לקוח מובייל (.NET MAUI)

משתמש בשילוב טכנולוגי:

שליחה: דרך IMauiChatService (פרוטוקול WCF Basic).

קבלה: דרך HubConnection של SignalR המאזין לאירוע receiveMessage.

ניהול מצב: מחלקת ServiceHelper מרכזת את שתי התקשורות תחת Singleton אחד.

🛠 טכנולוגיות בשימוש

Server Side: WCF, Self-Hosted SignalR (OWIN), .NET Framework 4.8.

Client Side: .NET MAUI (Mobile), WPF (Desktop).

Data Access: שכבת ViewModel לניהול משתמשים והודעות ב-DB.

Networking: Hybrid Bridge (WCF Dual/Basic + SignalR) בשילוב DevTunnels.

🌐 קישוריות ומעקף localhost (DevTunnels)

אחד האתגרים בפיתוח אפליקציות מובייל הוא ש-localhost במכשיר (או באמולטור) אינו מצביע על המחשב המארח. כדי לאפשר למכשיר ה-MAUI לתקשר עם שרת ה-WCF/SignalR המקומי, הפרויקט עושה שימוש ב-Visual Studio DevTunnels:

מה זה עושה? יוצר כתובת URL ציבורית מאובטחת (HTTPS) המנתבת תעבורה ישירות לפורטים המקומיים במחשב הפיתוח.

הגדרה: ב-Visual Studio, תחת תפריט ה-Debug, הופעלו "Dev Tunnels" עבור הפורטים 8733 (WCF) ו-8080 (SignalR).

מימוש: בקובץ ServiceHelper.cs, כתובות ה-URL המקומיות הוחלפו בכתובות ה-Tunnel שנוצרו (לדוגמה: https://xxxx-8080.euw.devtunnels.ms).

📂 רכיבי מפתח בקוד

ChatService.cs: המימוש המרכזי של השירות. כאשר הודעה מתקבלת, היא מופצת גם ללקוחות ה-WCF (דרך ה-Callback) וגם ללקוחות ה-SignalR (דרך ה-HubContext).

ServiceHelper.cs (MAUI): מנהל את החיבור הכפול. הוא מחזיק את כתובות ה-DevTunnel ומבצע את החיבור ל-SignalR ואת יצירת ערוץ ה-WCF.

App.config: הגדרת ה-Endpoints המרובים באותה כתובת בסיס (Base Address).

🚀 הוראות הרצה

שרת: יש להריץ את פרויקט WpfHost. ודא שה-DevTunnels פעילים ב-Visual Studio. השרת יפתח את ה-WCF Host ואת שרת ה-SignalR.

מובייל: ודא שכתובות ה-URL ב-ServiceHelper.cs מעודכנות לכתובות ה-Tunnel הנוכחיות שלך.

שימוש: התחבר עם שם משתמש קיים, בחר איש קשר מהרשימה והתחל להתכתב.
המערכת מורכבת משלושה רכיבים עיקריים:

1. שרת היברידי (WpfHost + WcfService)

השרת מארח בו-זמנית שני סוגי תקשורת:

WCF Endpoint (wsDualHttpBinding): מיועד ללקוחות WPF, תומך ב-Callback Contract (IChatCallback) לקבלת הודעות בזמן אמת.

WCF Endpoint (basicHttpBinding): מיועד ללקוחות MAUI, מאפשר פעולות Stateless (שליחה, התחברות, משיכת היסטוריה).

SignalR Hub (ChatHub): משמש כערוץ ה-Callback עבור לקוחות המובייל.

2. לקוח דסקטופ (WPF)

משתמש בחוזה IChatService ובמימוש IChatCallback כדי לקבל הודעות בדחיפה ישירות מהשרת.

3. לקוח מובייל (.NET MAUI)

משתמש בשילוב טכנולוגי:

שליחה: דרך IMauiChatService (פרוטוקול WCF Basic).

קבלה: דרך HubConnection של SignalR המאזין לאירוע receiveMessage.

ניהול מצב: מחלקת ServiceHelper מרכזת את שתי התקשורות תחת Singleton אחד.

🛠 טכנולוגיות בשימוש

Server Side: WCF, Self-Hosted SignalR (OWIN), .NET Framework 4.8.

Client Side: .NET MAUI (Mobile), WPF (Desktop).

Data Access: שכבת ViewModel לניהול משתמשים והודעות ב-DB.

Networking: Hybrid Bridge (WCF Dual/Basic + SignalR) בשילוב DevTunnels.

🌐 קישוריות ומעקף localhost (DevTunnels)

אחד האתגרים בפיתוח אפליקציות מובייל הוא ש-localhost במכשיר (או באמולטור) אינו מצביע על המחשב המארח. כדי לאפשר למכשיר ה-MAUI לתקשר עם שרת ה-WCF/SignalR המקומי, הפרויקט עושה שימוש ב-Visual Studio DevTunnels:

מה זה עושה? יוצר כתובת URL ציבורית מאובטחת (HTTPS) המנתבת תעבורה ישירות לפורטים המקומיים במחשב הפיתוח.

הגדרה: ב-Visual Studio, תחת תפריט ה-Debug, הופעלו "Dev Tunnels" עבור הפורטים 8733 (WCF) ו-8080 (SignalR).

מימוש: בקובץ ServiceHelper.cs, כתובות ה-URL המקומיות הוחלפו בכתובות ה-Tunnel שנוצרו (לדוגמה: https://xxxx-8080.euw.devtunnels.ms).

📂 רכיבי מפתח בקוד

ChatService.cs: המימוש המרכזי של השירות. כאשר הודעה מתקבלת, היא מופצת גם ללקוחות ה-WCF (דרך ה-Callback) וגם ללקוחות ה-SignalR (דרך ה-HubContext).

ServiceHelper.cs (MAUI): מנהל את החיבור הכפול. הוא מחזיק את כתובות ה-DevTunnel ומבצע את החיבור ל-SignalR ואת יצירת ערוץ ה-WCF.

App.config: הגדרת ה-Endpoints המרובים באותה כתובת בסיס (Base Address).

🚀 הוראות הרצה

שרת: יש להריץ את פרויקט WpfHost. ודא שה-DevTunnels פעילים ב-Visual Studio. השרת יפתח את ה-WCF Host ואת שרת ה-SignalR.

מובייל: ודא שכתובות ה-URL ב-ServiceHelper.cs מעודכנות לכתובות ה-Tunnel הנוכחיות שלך.

שימוש: התחבר עם שם משתמש קיים, בחר איש קשר מהרשימה והתחל להתכתב.
