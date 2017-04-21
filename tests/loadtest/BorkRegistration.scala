package tests.loadtest

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import scala.concurrent.duration._

class BorkRegistration extends Simulation {

  // Common config
  val password = "Password1!"
  val json_headers = Map("Content-Type" -> "application/json")
  val form_headers = Map("Content-Type" -> "application/x-www-form-urlencoded")

  // Account actions
  object Account {

    val usernames = csv("register.csv").queue

    // Login a random user
    val register = exec(
      http("Registration")
        .get("/Account/Register")
          .check(substring("Log in"))
          .check(regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(.+?)\" />")
            .saveAs("request_verification_token")))
      .pause(1)
      .feed(usernames)
      .exec(
        http("Submit Registration")
          .post("/Account/Register")
            .headers(form_headers)
            .formParam("Username", "${username}")
            .formParam("Email", "${username}@bork.bork")
            .formParam("Password", password)
            .formParam("ConfirmPassword", password)
            .formParam("__RequestVerificationToken", "${request_verification_token}")
          .check(substring("<span id=\"big-username\">@${username}</span>")))
      .pause(1)

    // Logout a user
    val logout = exec(
      http("Logout")
        .get("/")
          .check(substring("<span id=\"big-username\">@${username}</span>"))
          .check(regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(.+?)\" />")
            .saveAs("request_verification_token")))
      .pause(1)
      .exec(
        http("Submit Logout")
          .post("/Account/Logout")
            .headers(form_headers)
            .formParam("__RequestVerificationToken", "${request_verification_token}")
          .check(substring("Log in")))
      .pause(1)
  }

  // Configure http defaults
  val httpConf = http
    .baseURL("http://ec2-54-252-152-215.ap-southeast-2.compute.amazonaws.com")
    .acceptHeader("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8")
    .acceptLanguageHeader("en-US,en;q=0.5")
    .acceptEncodingHeader("gzip, deflate")
    .userAgentHeader("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:16.0) Gecko/20100101 Firefox/16.0")
    .proxy(Proxy("127.0.0.1", 8888))

  // This scenario purposefully repeats forever. The test will stop when the feeder runs out
  val registerUser = scenario("Register User")
  .asLongAs(true) {
    exec(Account.register, Account.logout)
  }

  // Set up simulation
  setUp(
    registerUser.inject(atOnceUsers(1))
  ).protocols(httpConf)
}