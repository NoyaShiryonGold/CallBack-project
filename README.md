**Multi-Platform Hybrid Chat System (WCF & SignalR)**
🚀 Overview
This project is a high-performance, real-time communication system designed to bridge the gap between traditional desktop environments and modern mobile platforms. It demonstrates a sophisticated architectural solution for supporting Real-time Duplex communication across diverse ecosystems.

The system consists of a WPF Desktop Client, a .NET MAUI Mobile Client, and a centralized Hybrid Backend that manages state and message distribution.

💡 The Challenge & Solution
The Problem: Windows Communication Foundation (WCF) provides robust Duplex (two-way) communication for desktop applications. However, mobile platforms like .NET MAUI do not natively support WCF Callback contracts reliably, making real-time "push" notifications difficult to implement.

The Solution:
I developed a Hybrid Bridge Architecture:

WPF Clients communicate via WSDualHttpBinding for native duplex support.

MAUI Clients utilize a combination of BasicHttpBinding (for sending requests) and SignalR (to receive real-time push updates).

The server acts as a synchronizer, ensuring that a message sent from any platform is instantly broadcasted to all recipients, regardless of their connection protocol.

🛠 Tech Stack
Backend: .NET Framework WCF Service, Self-Hosted OWIN SignalR Server.

Desktop Client: WPF (Windows Presentation Foundation) using MVVM.

Mobile Client: .NET MAUI (Multi-platform App UI).

Networking: WCF Duplex, SignalR Hubs, RESTful-style WCF Endpoints.

Patterns: Singleton, Mediator, Factory Pattern, Async/Await.

🏗 Architecture Highlights
Dual-Endpoint Hosting: The service exposes multiple endpoints on the same base address to cater to different client requirements.

Thread Safety: Implements thread-locking mechanisms (_syncRoot) and concurrent collections to handle multiple simultaneous connections.

UI Synchronization: Uses MainThread.BeginInvokeOnMainThread in the mobile client to ensure background network events safely update the user interface.

State Management: A centralized SharedData singleton manages local session persistence and chat history.

📂 Main Components
WcfService: The core logic, managing user sessions and the message database.

WpfHost: The server entry point that initializes both the WCF ServiceHost and the SignalR pipeline.

MauiChatUser: The cross-platform mobile app featuring a custom ServiceHelper to manage hybrid connectivity.

🔧 Setup & Installation
Clone the repository.

Run the WpfHost project first to initialize the server.

For mobile testing, update the _wcfAddress and _signalrAddress in ServiceHelper.cs to your local IP or a DevTunnel URL.

Launch the WPF or MAUI clients and start chatting!

👨‍💻 Author
Developed as a final project in Computer Science, focusing on distributed systems and cross-platform communication.
