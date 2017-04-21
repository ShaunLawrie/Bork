import io.gatling.core.Predef._
import io.gatling.http.Predef._

// Home page actions
object About {

    // View the about page
    val about = exec(
        http("View About")
        .get("/About")
            .check(substring("Example Web App for Performance Testing Testing")))

}