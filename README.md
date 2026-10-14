  This is a standard mcp server created using the mcp server template, with the ability to using azure http tunnelling (azure relay) added.
  With azure relay, you can securely expose your local server to the internet without opening ports on your router or firewall. You can debug easliy

  
  To run this, you need to creat a Azure Relay namespace and a hybrid connection, and get the primary connection string.  Tested with Visaul Studio and Claude

  What is changed is that Azure Replay response to RelayResponseBodyFeature with a "not implemented" exception, if you just wire up Azure relay without doing anything, it will failed.
  
  I had to implement a custom RelayResponseBodyFeature to handle the response correctly. 
  
  
 