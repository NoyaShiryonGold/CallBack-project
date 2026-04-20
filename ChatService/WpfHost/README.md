# **הגדרת DevTunnels לפרויקט Hybrid Chat 🌐**

מדריך זה מסביר כיצד להגדיר ולהשתמש ב-**Microsoft DevTunnels** כדי לאפשר לאפליקציית ה-MAUI (מובייל) לתקשר עם שרת ה-WCF וה-SignalR שרץ על המחשב המקומי.

##  **למה צריך DevTunnel?**

כאשר מריצים אמולטור או מכשיר פיזי, הכתובת localhost מצביעה על המכשיר עצמו. **DevTunnels** יוצר "צינור" מאובטח מהאינטרנט לפורטים במחשב שלך, ומספק כתובת HTTPS ציבורית שניתן להזין בקוד הלקוח.

## **💻 הגדרה מלאה דרך ה-Terminal**

שימוש בטרמינל מאפשר שליטה מדויקת ומהירה יותר בניהול ה-Tunnels.

### פתיחת הטרמינל
   בתוך Visual Studio: הקש Ctrl \+ \~ (מילדה) או עבור לתפריט View \-\> Terminal.

### התקנת ה-CLI (במידה ולא מותקן)
הדרך הקלה ביותר להתקין את כלי ה-devtunnel היא דרך ה-Terminal (ב-Windows):
```shell
winget install Microsoft.devtunnel
```
לאחר ההתקנה, יש לסגור ולפתוח מחדש את הטרמינל.

### התחברות (Login)
יש לבצע התחברות לחשבון ה-Microsoft שלך (זהה לזה של ה-Visual Studio):
```shell
devtunnel login
```
### יצירת Tunnel חדש
ניצור Tunnel קבוע בשם chat-bridge המאפשר גישה אנונימית (כדי שהמובייל יוכל להתחבר ללא הזדהות):
```shell
devtunnel create chat-bridge --allow-anonymous
```
### הוספת הפורטים (Ports)
   כעת נגדיר ל-Tunnel אילו פורטים עליו לחשוף. הפרויקט שלנו משתמש ב-8733 עבור WCF וב-8080 עבור SignalR	:

```shell
devtunnel port create chat-bridge -p 8733devtunnel port create chat-bridge -p 8080
```

### הרצת ה-Tunnel (Hosting)
כדי להתחיל את ניתוב התעבורה בפועל:
```shell
devtunnel host chat-bridge
```
**שים לב:** הטרמינל יישאר פתוח ויציג את כתובות ה-URL הציבוריות. הן ייראו בערך כך:
* https://xxxx-8733.euw.devtunnels.ms
* https://xxxx-8080.euw.devtunnels.ms 

## **📝 עדכון קוד הלקוח (MAUI)**
העתק את הכתובות מהטרמינל ועדכן את הקובץ ServiceHelper.cs (לדוגמה):
```c#
private readonly string _signalrAddress= "[https://xxxx-8080.euw.devtunnels.ms/signalr](https://xxxx-8080.euw.devtunnels.ms/signalr)";
private readonly string _wcfAddress= "[https://xxxx-8733.euw.devtunnels.ms/Design_Time_Addresses/WcfService/ChatService/maui](https://xxxx-8733.euw.devtunnels.ms/Design_Time_Addresses/WcfService/ChatService/maui)";

```

## 
## **⚠️ דגשים חשובים ופתרון בעיות**

אישור גישה ראשוני (Landing Page)  
בפעם הראשונה שתריץ את ה-Tunnel, מיקרוסופט תחסום את הגישה עד לאישור ידני.

1. העתק את כתובת ה-URL של ה-WCF (פורט 8733\) לדפדפן במחשב.

2. יוצג דף "Landing Page". לחץ על כפתור **"Continue"**.

3. כעת האפליקציה במובייל תוכל לתקשר עם השרת בחופשיות.

בדיקת סטטוס \- כדי לראות את כל ה-Tunnels הפעילים שלך בטרמינל:
```shell
devtunnel list
```

