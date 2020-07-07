# net-clipboard  
A dotnet Core 3.1 network-enabled clipboard monitor.  

## What does it do?  How Does it work?  
**net-clipboard** monitors the local clipboard.  When it changes, the new clipboard data is broadcast via UDP
on the local subnet.

## What does it not do?  What are the limitations?
* Does not implement any form of security.
* Does not traverse NAT/firewall.
* Only works on Windows OS.

## Roadmap / What I would do different
I would really like to use this application cross-platform.  To realize that I'll likely remake the application using
proper x-plat technologies.  Perhaps Electron.  Perhaps Blazor in a hosted webView.  Perhaps React Native.  

Also, proper traversal of NAT/firewalls is a must for those working off-network or among VMs running host-only networking.
Will likely implement this via Azure Event Grid / Event Hub.  

Lastly, some rudimentary security may be in order.  Options include:
* Some kind of IPSEC
* Some kind of API KEY type of thing
* Unobtrusively allowing the user to select when to accept an incoming clipboard change
* Pub/Sub via Event Grid endpoints.