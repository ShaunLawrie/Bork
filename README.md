```
      ,
      |`-.__      ____    ___   ____   __  _
      / ' _/     |    \  /   \ |    \ |  |/ |
     ****`       |  o  ||     ||  D  )|  ' /
    /    }       |     ||  O  ||    / |    \
   /  \ /        |  O  ||     ||    \ |     \
\ /`   \\\       |     ||     ||  .  \|  .  |
 `\____/_\\      |_____| \___/ |__|\_||__|\_|
```

# Example Web App for Performance Testing Testing

## Structure

* **Bork.Web**
 ASP.NET Core web front end for a Twitter style application. Only contains authentication and view logic.
* **Bork.Api**
 ASP.NET Core Web API for the web front end and notification processor to use. Contains business logic, data access etc.
* **Bork.Notifications:**
  .NET Core Console Application for async notification processing from a RabbitMQ queue.

## Description

This is a Twitter clone for the purpose of practicing performance scripting.
Users log into the web front end and are presented with a recent borks feed.
The user can create a new bork or re-bork someone elses.
If a user has their bork re-borked they are sent a notification via "email" (this doesn't actually send emails)
