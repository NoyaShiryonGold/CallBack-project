# **Hybrid Real-Time Chat System 📱💻**

## **Hebrew Description | תיאור בעברית**

מערכת צ'אט היברידית המאפשרת תקשורת בזמן אמת בין לקוחות **WPF (Desktop)** ללקוחות **.NET MAUI (Mobile)**. הפרויקט מדגים פתרון הנדסי לאתגר התקשורת הדו-כיוונית במובייל על ידי שילוב של טכנולוגיות WCF ו-SignalR.

### **🎯 האתגר הטכנולוגי**

פרוטוקול **WCF Duplex** (תקשורת דו-כיוונית מלאה) עובד מצוין בסביבת דסקטופ, אך אינו נתמך באופן טבעי או יציב בפלטפורמות מובייל מודרניות כמו .NET MAUI. המערכת פותרת זאת על ידי שימוש ב-SignalR כ"גשר" (Bridge) לדחיפת הודעות ללקוחות המובייל.

### **🏗 ארכיטקטורת המערכת**

המערכת מורכבת משלושה רכיבים עיקריים:

1. **שרת היברידי (WpfHost \+ WcfService):** מארח WCF Endpoint מסוג wsDualHttpBinding ל-WPF ו-basicHttpBinding ל-MAUI, בשילוב SignalR Hub להודעות דחיפה למובייל.  
2. **לקוח דסקטופ (WPF):** משתמש ב-WCF Duplex לקבלת הודעות.  
3. **לקוח מובייל (.NET MAUI):** משתמש ב-WCF לשליחת נתונים וב-SignalR לקבלתם.

### **🌐 קישוריות (DevTunnels)**

הפרויקט עושה שימוש ב-**Visual Studio DevTunnels** כדי לאפשר לאמולטורים ומכשירים חיצוניים לגשת לשרת ה-localhost דרך כתובת HTTPS ציבורית ומאובטחת.

## **English Description**

A hybrid chat system enabling real-time communication between **WPF (Desktop)** and **.NET MAUI (Mobile)** clients. This project demonstrates an engineering solution for mobile duplex communication challenges by bridging WCF and SignalR technologies.

### **🎯 The Technical Challenge**

The **WCF Duplex** protocol (full bi-directional communication) works seamlessly in desktop environments but lacks native or stable support in modern mobile platforms like .NET MAUI. This system overcomes this limitation by utilizing SignalR as a "Bridge" to push real-time messages to mobile clients.

### **🏗 System Architecture**

The system consists of three main components:

1. **Hybrid Server (WpfHost \+ WcfService):** Hosts simultaneous WCF endpoints (wsDualHttpBinding for WPF and basicHttpBinding for MAUI) alongside a SignalR Hub for mobile push notifications.  
2. **Desktop Client (WPF):** Utilizes WCF Duplex and Callback Contracts to receive real-time updates.  
3. **Mobile Client (.NET MAUI):** Implements a hybrid approach—sending data via WCF (Basic) and receiving real-time events via a SignalR HubConnection.

### **🌐 Connectivity (DevTunnels)**

To bypass localhost limitations on mobile devices/emulators, the project utilizes **Visual Studio DevTunnels**. This creates a secure public HTTPS URL that routes traffic directly to the local development environment.

## **🛠 Technologies Stack**

* **Server Side:** WCF, Self-Hosted SignalR (OWIN), .NET Framework 4.8.  
* **Client Side:** .NET MAUI (Mobile), WPF (Desktop).  
* **Networking:** Hybrid Bridge (WCF Dual/Basic \+ SignalR) \+ DevTunnels.  
* **Logic:** Task-based Asynchronous Pattern, Singleton Pattern (ServiceHelper).

## **🚀 How to Run**

1. **Server:** Run the WpfHost project. Ensure DevTunnels are active in Visual Studio.  
2. **Mobile:** Verify that the Tunnel URLs in ServiceHelper.cs match your active DevTunnel.  
3. **Usage:** Login with an existing username, select a contact, and start chatting.

## **👨‍💻 Project Context**

Developed as a final project focusing on distributed systems and protocol integration to create a unified user experience across different platforms.
