
import io.gatling.core.Predef._
import io.gatling.http.Predef._
import scala.concurrent.duration._

import flood._

class BorkLoadTest extends Simulation {

  // Use flood params
  val duration  = java.lang.Long.getLong("duration", 10L)

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
      .pause(1 seconds)
    }

  val pollingBorks = scenario("Polling Borks")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Feed.topBorks)
      .pause(1 seconds)
    }

  val creatingBorks = scenario("Creating Borks")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Bork.newBork)
      .pause(1 seconds)
    }
  
  val reBorking = scenario("ReBorking")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Bork.reBork)
      .pause(1 seconds)
    }
  
  val other = scenario("Other")
    .exec(Account.login)
    .asLongAs(true) {
      exec(Account.manage)
      .pause(1 seconds)
      .exec(About.about)
      .pause(1 seconds)
    }

  // Set up simulation
  setUp(
    pollingStats.inject(rampUsers(100) over(60 seconds)),
    pollingBorks.inject(rampUsers(100) over(60 seconds)),
    creatingBorks.inject(rampUsers(20) over(60 seconds)),
    reBorking.inject(rampUsers(10) over(60 seconds)),
    other.inject(rampUsers(10) over(60 seconds))
  ).protocols(httpProtocol)
  .maxDuration(duration minutes)
  
  uniformPausesPlusOrMinusPercentage(0.5)
}