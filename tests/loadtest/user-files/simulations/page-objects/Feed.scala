import io.gatling.core.Predef._
import io.gatling.http.Predef._

// Main feed actions
object Feed {

    // Get stats for the current user
    val stats = exec(
        http("Poll Stats")
        .get("/account/stats")
        .check(substring("Re-Borks</span>")))

    // Get top borks for the bork feed
    val topBorks = exec(
        http("Poll Top Borks")
        .get("/bork/topborks")
        .check(substring("<div class=\"bork-content\">")))

}