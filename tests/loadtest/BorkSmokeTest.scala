
import io.gatling.core.Predef._
import io.gatling.http.Predef._
import scala.concurrent.duration._

import flood._

class BorkSmokeTest extends Simulation {

  // Configure http defaults
  val httpProtocol = httpConfigFlood
    .baseURL("http://ec2-54-252-152-215.ap-southeast-2.compute.amazonaws.com")
    .acceptHeader("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8")
    .acceptLanguageHeader("en-US,en;q=0.5")
    .acceptEncodingHeader("gzip, deflate")
    .userAgentHeader("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:16.0) Gecko/20100101 Firefox/16.0")

  // Define scenarios
  val pollingStats = scenario("Polling Stats")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Feed.stats)
      .pause(5)
    }

  val pollingBorks = scenario("Polling Borks")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Feed.topBorks)
      .pause(5)
    }

  val creatingBorks = scenario("Creating Borks")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Bork.newBork)
      .pause(30)
    }
  
  val reBorking = scenario("ReBorking")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Bork.reBork)
      .pause(45)
    }
  
  val other = scenario("Other")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Account.manage)
      .pause(30)
      .exec(About.about)
      .pause(30)
    }

  // Set up simulation
  setUp(
    pollingStats.inject(atOnceUsers(5)),
    pollingBorks.inject(atOnceUsers(5)),
    creatingBorks.inject(atOnceUsers(5)),
    reBorking.inject(atOnceUsers(5)),
    other.inject(atOnceUsers(5))
  ).protocols(httpProtocol)
  .maxDuration(60)
}